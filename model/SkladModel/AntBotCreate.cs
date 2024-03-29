﻿using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkladModel
{
    public class AntBotCreate : AntBotAbstractEvent
    {
        string id;
        int x;
        int y;
        bool direction;
        double charge;
        double maxCharge;
        bool isDebug;
        SkladConfig skladConfig;

        public override AntBotAbstractEvent Clone() => new AntBotCreate(id, x, y, direction, charge, maxCharge, isDebug, skladConfig);
        public AntBotCreate(string id, int x, int y, bool direction, double charge, double maxCharge, bool isDebug, SkladConfig skladConfig)
        {
            this.x = x;
            this.y = y;
            this.id = id;
            this.direction = direction;
            this.charge = charge;
            this.maxCharge = maxCharge;
            this.isDebug = isDebug; 
            this.skladConfig = skladConfig;
        }

        public override bool CheckReservation()
        {
            throw new NotImplementedException();
        }

        public override TimeSpan getEndTime()
        {
            throw new NotImplementedException();
        }

        public override TimeSpan getStartTime()
        {
            throw new NotImplementedException();
        }

        public override void ReserveRoom()
        {
            throw new NotImplementedException();
        }

        public override void runEvent(List<AbstractObject> objects, TimeSpan timeSpan)
        {
            AntBot antBot = new AntBot();
            antBot.uid = id;
            antBot.unitSpeed = skladConfig.unitSpeed; 
            antBot.unitAccelerationTime = skladConfig.unitAccelerationTime; 
            antBot.unitStopTime = skladConfig.unitStopTime; 
            antBot.unitRotateTime = skladConfig.unitRotateTime; 
            antBot.unitAccelerationEnergy = skladConfig.unitAccelerationEnergy; 
            antBot.unitStopEnergy = skladConfig.unitStopEnergy;
            antBot.unitMoveEnergy = skladConfig.unitMoveEnergy;
            antBot.unitRotateEnergy = skladConfig.unitRotateEnergy; 
            antBot.loadTime = skladConfig.loadTime;  
            antBot.unloadTime = skladConfig.unloadTime; 
            antBot.unitLoadEnergy = skladConfig.unitLoadEnergy;
            antBot.unitUnloadEnergy = skladConfig.unitUnloadEnergy;
            antBot.unitWaitEnergy = skladConfig.unitWaitEnergy;
            antBot.unitChargeTime = skladConfig.unitChargeTime;
            antBot.unitChargeValue = maxCharge;//skladConfig.unitChargeValue;

            antBot.isDebug = isDebug;
            antBot.sklad = (Sklad)objects.First(x=> x is Sklad);
            antBot.xCoordinate = x;
            antBot.yCoordinate = y;
            antBot.isXDirection = direction; //true
            antBot.xSpeed = 0;
            antBot.ySpeed = 0;
            antBot.isLoaded = false;
            antBot.isFree = true;
            antBot.charge = charge;// antBot.unitChargeValue;
            antBot.targetXCoordinate = x;
            antBot.targetYCoordinate = y;
            antBot.state = AntBotState.Wait;
            antBot.lastUpdated = timeSpan;
            antBot.waitTime = TimeSpan.MaxValue;
            antBot.ReserveRoom(x, y, antBot.lastUpdated, TimeSpan.MaxValue);
            
            if (objects.Exists(x => x is SkladLogger)) {
                antBot.skladLogger = (SkladLogger)objects.First(x => x is SkladLogger);
                antBot.skladLogger.AddLog(antBot, "Create AntBot");
            }
            antBot.objects = objects;
            antBot.commandList = new CommandList(antBot);
            objects.Add(antBot);
            if (isDebug)
                Console.WriteLine($"antBot {antBot.uid} created {antBot.lastUpdated} coordinate {antBot.xCoordinate}, {antBot.yCoordinate}");
        }
    }

}
