﻿using AbstractModel;
using System;
using System.Collections.Generic;

namespace SkladModel
{
    public class AntBotCharge : AntBotAbstractEvent
    {
        public override AntBotAbstractEvent Clone() => new AntBotCharge(antBot);

        public AntBotCharge(AntBot antBot)
        {
            this.antBot = antBot;
        }

        public override bool CheckReservation()
        {
            return antBot.CheckRoom(getStartTime(), getEndTime());
        }



        public override TimeSpan getStartTime() => antBot.lastUpdated;
        public override TimeSpan getEndTime()
        {
            return getStartTime() + antBot.getTimeForFullCharge();
        }

        public override void ReserveRoom()
        {
            int x = antBot.xCord;
            int y = antBot.yCord;
            antBot.ReserveRoom(x, y, getStartTime(), getEndTime());
        }

        public override void runEvent(List<AbstractObject> objects, TimeSpan timeSpan)
        {
            antBot.xCoordinate = antBot.xCord;
            antBot.yCoordinate = antBot.yCord;
            antBot.xSpeed = 0;
            antBot.ySpeed = 0;
            antBot.state = AntBotState.Charging;
            antBot.waitTime = getEndTime();
            antBot.isFree = false;
            antBot.RemoveFirstCommand(timeSpan);
            if (antBot.skladLogger != null)
            {
                antBot.skladLogger.AddLog(antBot, "Charging");
                if (antBot.isDebug)
                {
                    Console.WriteLine($"antBot {antBot.uid} Charging {antBot.lastUpdated} coordinate {antBot.xCoordinate}, {antBot.yCoordinate}");
                }
            }

        }
    }
}