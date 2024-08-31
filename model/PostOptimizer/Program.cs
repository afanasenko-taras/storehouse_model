using AbstractModel;
using PostModel;
using System;
using System.IO;
using YamlConverter;

namespace PostOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskConfig taskConfig = Helper.FileDeserialize<TaskConfig>(@"D:\PR\buffer\taskConfig.bnr");

            TaskConfig taskConfig = YamlConvert.DeserializeObject<TaskConfig>(File.ReadAllText(
                @"D:\post_front2\data\PMI\test\POST-PMI-SET-TASK-3.2.yaml"));
            PostGraph postGraph = new PostGraph(taskConfig);
            postGraph.RunOptimizeDeystra();
            //File.WriteAllText(@"D:\PR\buffer\taskConfig.yaml", YamlConvert.SerializeObject(taskConfig));
        }
    }
}
