﻿using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedXmlSerializer.Configuration;
using System.Xml;
using SkladModel;
using ExtendedXmlSerializer;
using System.IO;
using ExtendedXmlSerializer.ExtensionModel.Content;
using System.Text;
using ControlModel;
using System.Xml.Linq;
using System.Data.Common;

namespace TestSklad2
{
    class Program
    {
        private static double timeEnergyMetric(CommandList arg)
        {
            return arg.lastTime.TotalSeconds; //+
                //Math.Abs(arg.antState.xCord - arg.antState.targetXCoordinate) * 2.0 +
                //Math.Abs(arg.antState.yCord - arg.antState.targetYCoordinate) * 2.0;
           // +
           //((arg.antState.targetDirection == arg.antState.isXDirection) ? 1 : 0) * 2.0;
            return arg.lastTime.TotalSeconds +
                (1 - arg.antState.charge / arg.antBot.unitChargeValue) *
                arg.antBot.unitChargeTime;
            return arg.lastTime.TotalSeconds;
            return arg.lastTime.TotalSeconds +
                (1 - arg.antState.charge / arg.antBot.unitChargeValue) *
                arg.antBot.unitChargeTime +
            
                (arg.RotateCount[(int)SquareProperty.LoadX] + arg.RotateCount[(int)SquareProperty.LoadY]) * 4 +
                (arg.MoveCount[(int)SquareProperty.LoadX] + arg.MoveCount[(int)SquareProperty.LoadY]) * 5 +
                (arg.MoveCount[(int)SquareProperty.UnLoadX] + arg.MoveCount[(int)SquareProperty.UnLoadY]) * 0.33 +
                (arg.RotateCount[(int)SquareProperty.ChargeX] + arg.RotateCount[(int)SquareProperty.ChargeY]) * 2 +
                (arg.MoveCount[(int)SquareProperty.ChargeX] + arg.MoveCount[(int)SquareProperty.ChargeY]);
           
        }


        static void Main(string[] args)
        {            
            SkladWrapper skladWrapper = new SkladWrapper(@"..\..\..\..\..\wms-config2.xml", false);
            //skladWrapper.isDebug = true;
            skladWrapper.AddLogger();
            skladWrapper.AddSklad(timeEnergyMetric);
            skladWrapper.AddAnts(12);
            //new MoveSort(skladWrapper).Run(TimeSpan.FromSeconds(3600));
            new MoveSort(skladWrapper).Run();
            Console.WriteLine(ActionCounter.next_count);
            skladWrapper.SaveLog(@"..\..\..\..\..\log.xml");
            SkladLogger logger = (SkladLogger)skladWrapper.objects.First(x => x is SkladLogger);
            File.WriteAllBytes(@"..\..\..\..\..\log_unity.xml", SkladWrapper.SerializeXML(logger.logs.ToArray()));
            Console.ReadLine();
        }


    }
}
