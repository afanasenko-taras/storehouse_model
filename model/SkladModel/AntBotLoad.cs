﻿using AbstractModel;
using System;
using System.Collections.Generic;

namespace SkladModel
{
    public class AntBotLoad : AntBotAbstractEvent
    {

        public override AntBotAbstractEvent Clone() => new AntBotLoad(antBot);
        public AntBotLoad(AntBot antBot)
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
            return getStartTime() + TimeSpan.FromSeconds(antBot.loadTime);
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
            antBot.charge -= antBot.unitLoadEnergy;
            antBot.state = AntBotState.Loading;
            antBot.isLoaded = true;
            antBot.RemoveFirstCommand(timeSpan);
            antBot.waitTime = getEndTime();
            antBot.isFree = (antBot.commandList.commands.Count == 0);
            if (antBot.skladLogger != null)
            {
                antBot.skladLogger.AddLog(antBot, "Load");
                if (antBot.isDebug)
                {
                    Console.WriteLine($"antBot {antBot.uid} Load {antBot.lastUpdated} coordinate {antBot.xCoordinate}, {antBot.yCoordinate}");
                }
            }

        }
    }
}