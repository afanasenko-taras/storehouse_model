using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            PostWrapper postWrapper = new PostWrapper();
            Dictionary<string, string> id2index = new Dictionary<string, string>();
            Dictionary<string, PostObject> id2poj = new Dictionary<string, PostObject>();
            Dictionary<string, Dictionary<string, InData>> inData = new Dictionary<string, Dictionary<string, InData>>();
            Dictionary<long, (string start_id, string end_id, double time, double distance)> bones = new Dictionary<long, (string start_id, string end_id, double time, double distance)>();
            foreach (var poj in taskConfig.PostObjects)
            {
                id2index.Add(poj.Id, poj.Index);
                id2poj.Add(poj.Id, poj);
            }

            foreach(var ind in taskConfig.InDates)
            {
                if (!inData.ContainsKey(ind.StartIndex))
                    inData.Add(ind.StartIndex, new Dictionary<string, InData>());
                if (!inData[ind.StartIndex].ContainsKey(ind.EndIndex))
                    inData[ind.StartIndex].Add(ind.EndIndex, ind);
            }

            foreach (var poj in taskConfig.PostObjects)
            {
                postWrapper.AddSortingCenter(poj.Index);
                foreach(var gate in poj.Gates)
                {
                    postWrapper.CreateGate(poj.Index, id2index[gate]);
                }
                foreach (var route in poj.Route)
                {
                    postWrapper.AddRoute(poj.Index, id2index[route.Key], id2index[route.Value]);
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
                var bone = bones[tr.Shedule["1"]];
                shedule.Add((long)(tr.StartTime * TimeSpan.TicksPerHour + tr.Id), (id2index[bone.start_id], TransportAction.Load));
                shedule.Add((long)((tr.StartTime + 1) * TimeSpan.TicksPerHour + tr.Id), (id2index[bone.end_id], TransportAction.Unload));
                postWrapper.AddPostTransport(shedule);
            }




            int dayNumber = 30;
            postWrapper.isDebug = true;
            Console.WriteLine("Start!");

            postWrapper.GenerateFullMessages(taskConfig, inData);
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
            File.WriteAllBytes("POST-Messages-Log.xml", Helper.SerializeXML(postWrapper.messages));
        }
    }
}
