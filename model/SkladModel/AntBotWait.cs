﻿using AbstractModel;
using System;
using System.Collections.Generic;

namespace SkladModel
{
    public class AntBotWait : AntBotAbstractEvent
    {
        TimeSpan time;
        public override AntBotAbstractEvent Clone() => new AntBotWait(antBot, time);
        public AntBotWait(AntBot antBot, TimeSpan time) {
            this.antBot = antBot;
            this.time = time;
        }

        public override bool CheckReservation()
        {
            return antBot.CheckRoom(getStartTime(), getEndTime());
        }

        public override TimeSpan getStartTime() => antBot.lastUpdated;
        public override TimeSpan getEndTime() => antBot.lastUpdated + time;

        public override void ReserveRoom()
        {
            antBot.ReserveRoom(getStartTime(), getEndTime());
        }

        public override void runEvent(List<AbstractObject> objects, TimeSpan timeSpan)
        {
            antBot.state = AntBotState.Work;
            antBot.waitTime = timeSpan + time;
            antBot.RemoveFirstCommand(timeSpan);
            antBot.isFree = (antBot.commandList.commands.Count == 0);
            if (antBot.skladLogger != null)
            {
                antBot.skladLogger.AddLog(antBot, "Wait");
                if (antBot.isDebug)
                {
                    Console.WriteLine($"antBot {antBot.uid} Wait {antBot.lastUpdated} coordinate {antBot.xCoordinate}, {antBot.yCoordinate}");
                }
            }

        }
    }

}
