using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PostModel;
using YamlConverter;

namespace PostRunner
{
    class Program
    {


        static void Main(string[] args)
        {
            string fileName = @"D:\post_front2\data\setup1.yaml";
            
            var sw = new Stopwatch();
            sw.Start();
            
            var taskConfig = YamlConvert.DeserializeObject<TaskConfig>(File.ReadAllText(fileName));
            PostWrapper postWrapper = new PostWrapper();
            foreach (var poj in taskConfig.PostObjects)
            {
                postWrapper.AddSortingCenter(poj.Index);
                foreach(var gate in poj.Gates)
                {
                    postWrapper.CreateGate(poj.Index, gate);
                }
            }






            /*
            postWrapper.CreateGate("5", "1");
            postWrapper.CreateGate("5", "2");
            postWrapper.CreateGate("5", "7");
            postWrapper.CreateGate("6", "3");
            postWrapper.CreateGate("6", "4");
            postWrapper.CreateGate("6", "7");
            postWrapper.CreateGate("7", "5");
            postWrapper.CreateGate("7", "6");

            postWrapper.CreateGate("1", "5");
            postWrapper.CreateGate("2", "5");
            postWrapper.CreateGate("3", "6");
            postWrapper.CreateGate("4", "6");


            postWrapper.AddRoute("5", "3", "7");
            postWrapper.AddRoute("5", "4", "7");
            postWrapper.AddRoute("5", "2", "2");
            postWrapper.AddRoute("5", "1", "1");

            postWrapper.AddRoute("6", "1", "7");
            postWrapper.AddRoute("6", "2", "7");
            postWrapper.AddRoute("6", "3", "3");
            postWrapper.AddRoute("6", "4", "4");

            postWrapper.AddRoute("7", "3", "6");
            postWrapper.AddRoute("7", "4", "6");
            postWrapper.AddRoute("7", "2", "5");
            postWrapper.AddRoute("7", "1", "5");


            postWrapper.AddRoute("1", "2", "5");
            postWrapper.AddRoute("1", "3", "5");
            postWrapper.AddRoute("1", "4", "5");

            postWrapper.AddRoute("2", "1", "5");
            postWrapper.AddRoute("2", "3", "5");
            postWrapper.AddRoute("2", "4", "5");

            postWrapper.AddRoute("3", "4", "6");
            postWrapper.AddRoute("3", "1", "6");
            postWrapper.AddRoute("3", "2", "6");

            postWrapper.AddRoute("4", "3", "6");
            postWrapper.AddRoute("4", "1", "6");
            postWrapper.AddRoute("4", "2", "6");



            Dictionary<long, (string postUid, TransportAction tAction)> shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();

            
            int transportNumber = 30000;
            Enumerable.Range(1, transportNumber).ToList().ForEach(transport =>
            {
                long tick = TimeSpan.TicksPerHour / transportNumber * transport;

                shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                shedule.Add(10 * TimeSpan.TicksPerHour + tick, ("5", TransportAction.Load));
                shedule.Add(12 * TimeSpan.TicksPerHour + tick, ("1", TransportAction.Both));
                shedule.Add(14 * TimeSpan.TicksPerHour + tick, ("5", TransportAction.Unload));
                postWrapper.AddPostTransport(10 * transport, shedule);

                shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                shedule.Add(11 * TimeSpan.TicksPerHour + tick, ("5", TransportAction.Load));
                shedule.Add(13 * TimeSpan.TicksPerHour + tick, ("2", TransportAction.Both));
                shedule.Add(14 * TimeSpan.TicksPerHour + tick, ("5", TransportAction.Unload));
                postWrapper.AddPostTransport(10 * transport + 1, shedule);


                shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                shedule.Add(6 * TimeSpan.TicksPerHour + tick, ("7", TransportAction.Load));
                shedule.Add(12 * TimeSpan.TicksPerHour + tick, ("5", TransportAction.Both));
                shedule.Add(18 * TimeSpan.TicksPerHour + tick, ("7", TransportAction.Unload));
                postWrapper.AddPostTransport(10 * transport + 2, shedule);

                shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                shedule.Add(6 * TimeSpan.TicksPerHour + tick, ("7", TransportAction.Load));
                shedule.Add(12 * TimeSpan.TicksPerHour + tick, ("6", TransportAction.Both));
                shedule.Add(18 * TimeSpan.TicksPerHour + tick, ("7", TransportAction.Unload));
                postWrapper.AddPostTransport(10 * transport + 3, shedule);

                shedule = new Dictionary<long, (string postUid, TransportAction tAction)>();
                shedule.Add(6 * TimeSpan.TicksPerHour + tick, ("6", TransportAction.Load));
                shedule.Add(10 * TimeSpan.TicksPerHour + tick, ("3", TransportAction.Both));
                shedule.Add(12 * TimeSpan.TicksPerHour + tick, ("4", TransportAction.Both));
                shedule.Add(14 * TimeSpan.TicksPerHour + tick, ("6", TransportAction.Unload));
                postWrapper.AddPostTransport(10 * transport + 4, shedule);

            });


            
            int mailNumber = 100000;

            postWrapper.GenerateTestMessages(dayNumber, mailNumber);


            */
            int dayNumber = 2;
            postWrapper.isDebug = true;
            Console.WriteLine("Start!");

            StreamWriter outputFile = new StreamWriter("Post.log");
            postWrapper.writeDebug = outputFile.WriteLine;
            while (postWrapper.Next() & postWrapper.updatedTime < TimeSpan.FromDays(dayNumber))
            {
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            outputFile.Close();
        }
    }
}
