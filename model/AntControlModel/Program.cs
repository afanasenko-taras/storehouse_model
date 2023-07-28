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
            SkladWrapper skladWrapper = new SkladWrapper(@"..\..\..\..\..\wms-config.xml", false);

            skladWrapper.AddLogger();
            skladWrapper.AddSklad();
            skladWrapper.AddAnts(1);

            new AntControl(skladWrapper).Run();
            SkladLogger logger = (SkladLogger)skladWrapper.objects.First(x => x is SkladLogger);
            File.WriteAllBytes(@"..\..\..\..\..\log_unity.xml", SkladWrapper.SerializeXML(logger.logs.ToArray()));

        }
    }
}
