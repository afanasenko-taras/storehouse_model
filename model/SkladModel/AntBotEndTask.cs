﻿using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladModel
{
    public class AntBotEndTask : AntBotAbstractEvent
    {

        public override AntBotAbstractEvent Clone() => new AntBotEndTask(antBot);
        public AntBotEndTask(AntBot antBot)
        {
            this.antBot = antBot;
        }

        public override bool CheckReservation()
        {
            return antBot.CheckRoom(getStartTime(), getEndTime());
        }

        public override TimeSpan getStartTime() => antBot.lastUpdated;
        public override TimeSpan getEndTime() => antBot.lastUpdated;

        public override void ReserveRoom()
        {
            //antBot.ReserveRoom(getStartTime(), getEndTime());
        }

        public override void runEvent(List<AbstractObject> objects, TimeSpan timeSpan)
        {
            antBot.state = AntBotState.Wait;
            antBot.waitTime = TimeSpan.Zero;
            antBot.isFree = (antBot.commandList.commands.Count == 0);
            if (antBot.skladLogger != null)
            {
                antBot.skladLogger.AddLog(antBot, "EndTask");
                if (antBot.isDebug)
                {
                    Console.WriteLine($"antBot {antBot.uid} EndTask {antBot.lastUpdated} coordinate {antBot.xCoordinate}, {antBot.yCoordinate}");
                }
            }

        }
    }
}
