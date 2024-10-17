using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AbstractModel;
using PostModel;
using YamlConverter;

namespace PostRunner
{
    class Program
    {


        static void Main(string[] args)
        {
            string fileName = @"D:\post_front2\data\POST-setup-full.yaml";
            
            var sw = new Stopwatch();
            sw.Start();

            var taskConfig = YamlConvert.DeserializeObject<TaskConfig>(File.ReadAllText(fileName));
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            sw.Reset();
            sw.Start();
            //taskConfig.InDates = null;
            //Helper.FileSerialize(taskConfig, @"D:\PR\buffer\taskConfig.bnr");
            //return;

            PostWrapper postWrapper = new PostWrapper(taskConfig);
            Dictionary<long, (string start_id, string end_id, double time, double distance)> bones = new Dictionary<long, (string start_id, string end_id, double time, double distance)>();


            //Graph gr = new Graph(taskConfig, id2index, id2poj);
            //File.WriteAllBytes("post-zone-a-and-b_v2.xml", Helper.SerializeXML(gr));
            //return;


            foreach (var poj in taskConfig.PostObjects)
            {
                postWrapper.AddSortingCenter(poj.Index, poj);
            }

            foreach (var bone in taskConfig.TransportBones)
            {
                bones.Add(bone.Id,
                          (bone.Start_id, bone.End_id, bone.Time, bone.Distance));
            }

            Random rnd = new Random(DateTime.Now.Millisecond);
            List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules = new List<Dictionary<long, (string postUid, TransportAction tAction)>>();
            foreach (var tr in taskConfig.TransportRoutes)
            {
                Dictionary<long, (string postUid, TransportAction tAction)> shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                int random_part = rnd.Next(-11,11);
                foreach (var sh in tr.Shedule)
                {
                    var number = int.Parse(sh.Key);
                    var bone = bones[sh.Value];
                    shedule.Add((long)((tr.StartTime + number - 1 + random_part) * TimeSpan.TicksPerHour + tr.Id * 100 + number), (postWrapper.id2index[bone.start_id], TransportAction.Both));
                    shedule.Add((long)((tr.StartTime + number + random_part) * TimeSpan.TicksPerHour + tr.Id * 150 + number), (postWrapper.id2index[bone.end_id], TransportAction.Both));
                }
                shedules.Add(shedule);
            }
            postWrapper.PostSedulesCreate(shedules);




            int dayNumber = 30;
            postWrapper.isDebug = true;
            Console.WriteLine("Start!");

            //postWrapper.GenerateFullMessages(taskConfig, inData);
            postWrapper.GenerateTeraplan(taskConfig);
            postWrapper.ForceUpdate(TimeSpan.FromHours(8));
            postWrapper.ForceFinish(TimeSpan.FromDays(dayNumber-1)+TimeSpan.FromHours(23));

            StreamWriter outputFile = new StreamWriter("POST-DES.log");
            StreamWriter errorFile = new StreamWriter("router_test.csv");
            postWrapper.writeDebug = outputFile.WriteLine;
            postWrapper.writeError = errorFile.WriteLine;
            while (postWrapper.Next() & postWrapper.updatedTime < TimeSpan.FromDays(dayNumber))
            {
                if (postWrapper.isFinished)
                    break;
            }   
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            outputFile.Close();
            errorFile.Close();

            int created = 0;
            int local = 0;
            int load = 0;
            int unload = 0;
            int undelivered = 0;
            int unsorted = 0;
            int noRouter = 0;
            int inTime = 0;

            SortedDictionary<int, int> ksDiff = new SortedDictionary<int, int>();
            foreach (var msg in postWrapper.messages)
            {
                foreach(var m in msg.log)
                {
                    if (m.Action == "Created")
                        created++;
                    if (m.Action == "Local")
                    {
                        local++;
                        if (m.ActionTime.TotalHours < msg.ks)
                            inTime++;
                        int diff = (int)m.ActionTime.TotalHours - msg.ks;
                        if (!ksDiff.ContainsKey(diff))
                            ksDiff.Add(diff, 0);
                        ksDiff[diff]++;
                    }
                    if (m.Action == "LoadOnTransport")
                        load++;
                    if (m.Action == "UnloadFromTransport")
                        unload++;
                    if (m.Action == "Undelivered")
                        undelivered++;
                    if (m.Action == "Unsorted")
                        undelivered++;
                    if (m.Action == "NoRouterFound")
                        noRouter++;
                }
            }

            Console.WriteLine($"Created     : {created}");
            Console.WriteLine($"Local       : {local}");
            Console.WriteLine($"InTime      : {inTime}");
            Console.WriteLine($"Load        : {load}");
            Console.WriteLine($"Unload      : {unload}");
            Console.WriteLine($"Undelivered : {undelivered}");
            Console.WriteLine($"Unsorted    : {unsorted}");
            Console.WriteLine($"NoRouter    : {noRouter}");
            Console.WriteLine($"Controll    : {created-local-(load-unload)-undelivered-unsorted-noRouter}");

            StreamWriter ksFile = new StreamWriter("ks.csv");
            foreach (var diff in ksDiff.Keys)
                ksFile.WriteLine($"{diff},{ksDiff[diff]}");
            ksFile.Close();


            //File.WriteAllBytes("POST-Messages-Log-700K.xml", Helper.SerializeXML(postWrapper.messages));
        }
    }
}
