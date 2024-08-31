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
            string fileName = @"D:\post_front2\data\POST-setup.yaml";
            
            var sw = new Stopwatch();
            sw.Start();

            var taskConfig = YamlConvert.DeserializeObject<TaskConfig>(File.ReadAllText(fileName));
            //taskConfig.InDates = null;
            //Helper.FileSerialize(taskConfig, @"D:\PR\buffer\taskConfig.bnr");
            //return;

            PostWrapper postWrapper = new PostWrapper();
            Dictionary<string, string> id2index = new Dictionary<string, string>();
            Dictionary<string, PostObject> id2poj = new Dictionary<string, PostObject>();
            Dictionary<long, (string start_id, string end_id, double time, double distance)> bones = new Dictionary<long, (string start_id, string end_id, double time, double distance)>();
            foreach (var poj in taskConfig.PostObjects)
            {
                id2index.Add(poj.Id, poj.Index);
                id2poj.Add(poj.Id, poj);
            }

            //Graph gr = new Graph(taskConfig, id2index, id2poj);
            //File.WriteAllBytes("post-zone-a-and-b_v2.xml", Helper.SerializeXML(gr));
            //return;


            foreach (var poj in taskConfig.PostObjects)
            {
                postWrapper.AddSortingCenter(poj.Index);
                foreach(var gate in poj.Gates)
                {
                    postWrapper.CreateGate(poj.Index, id2index[gate]);
                }
                foreach (var rules in poj.Route)
                {
                    string msgType = rules.Key;
                    foreach (var route in rules.Value)
                    {
                        if (route.Value is null)
                            postWrapper.AddRoute(poj.Index, id2index[route.Key], null, msgType);
                        else 
                            postWrapper.AddRoute(poj.Index, id2index[route.Key], id2index[route.Value], msgType);
                    }
                }
            }

            foreach (var bone in taskConfig.TransportBones)
            {
                bones.Add(bone.Id,
                          (bone.Start_id, bone.End_id, bone.Time, bone.Distance));
            }

            foreach(var tr in taskConfig.TransportRoutes)
            {
                Dictionary<long, (string postUid, TransportAction tAction)> shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                foreach (var sh in tr.Shedule)
                {
                    var number = int.Parse(sh.Key);
                    var bone = bones[sh.Value];
                    shedule.Add((long)((tr.StartTime + number - 1) * TimeSpan.TicksPerHour + tr.Id * 100 + number), (id2index[bone.start_id], TransportAction.Both));
                    shedule.Add((long)((tr.StartTime + number) * TimeSpan.TicksPerHour + tr.Id * 150 + number), (id2index[bone.end_id], TransportAction.Both));
                }
                postWrapper.AddPostTransport(shedule);
            }




            int dayNumber = 30;
            postWrapper.isDebug = true;
            Console.WriteLine("Start!");

            //postWrapper.GenerateFullMessages(taskConfig, inData);
            postWrapper.GenerateTeraplan(taskConfig);
            postWrapper.ForceUpdate(TimeSpan.FromHours(8));
            postWrapper.ForceFinish(TimeSpan.FromDays(dayNumber-1)+ TimeSpan.FromHours(23));

            StreamWriter outputFile = new StreamWriter("POST-DES.log");
            StreamWriter errorFile = new StreamWriter("router_test.csv");
            postWrapper.writeDebug = outputFile.WriteLine;
            postWrapper.writeError = errorFile.WriteLine;
            while (postWrapper.Next() & postWrapper.updatedTime < TimeSpan.FromDays(dayNumber))
            {
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            outputFile.Close();
            errorFile.Close();
            File.WriteAllBytes("POST-Messages-Log-700K-Filtered.xml", Helper.SerializeXML(postWrapper.messages));
        }
    }
}
