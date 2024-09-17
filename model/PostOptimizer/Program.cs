using AbstractModel;
using PostModel;
using System;
using System.Diagnostics;
using System.IO;
using YamlConverter;

namespace PostOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskConfig taskConfig = Helper.FileDeserialize<TaskConfig>(@"D:\PR\buffer\taskConfig.bnr");
            //string fileName = @"D:\post_front2\data\PMI\test\POST-PMI-SET-TASK-3.2.yaml";
            string fileName = @"D:\post_front2\data\POST-setup.yaml";
            TaskConfig taskConfig = YamlConvert.DeserializeObject<TaskConfig>(File.ReadAllText(fileName));
            PostGraph postGraph = new PostGraph(taskConfig);
            var sw = new Stopwatch();
            sw.Start();
            postGraph.RunOptimizeDeystra();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            File.WriteAllText(@"D:\post_front2\data\POST-setup-result.yaml", YamlConvert.SerializeObject(taskConfig));
            //File.WriteAllText(@"D:\PR\buffer\taskConfig.yaml", YamlConvert.SerializeObject(taskConfig));
        }
    }
}
