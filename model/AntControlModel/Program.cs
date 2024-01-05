using SkladModel;
using System;
using System.IO;
using System.Linq;

namespace AntControlModel
{
    class Program
    {
        static void Main(string[] args)
        {
            SkladWrapper skladWrapper = new SkladWrapper(@"..\..\..\..\..\ant-config.xml", false, false);
            skladWrapper.AddLogger();
            skladWrapper.AddSklad();
            skladWrapper.AddAnts(5);

            new AntControl(skladWrapper).RunTarget(TimeSpan.MaxValue);
            SkladLogger logger = (SkladLogger)skladWrapper.objects.First(x => x is SkladLogger);
            File.WriteAllBytes(@"..\..\..\..\..\log_unity.xml", SkladWrapper.SerializeXML(logger.logs.ToArray()));

        }
    }
}
