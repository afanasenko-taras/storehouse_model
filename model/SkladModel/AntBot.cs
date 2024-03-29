using AbstractModel;
using ExtendedXmlSerializer.Core.Sources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SkladModel
{
    public enum AntBotState : int 
    {
        Wait = 0,
        Move = 1,
        Rotate = 2, 
        Accelerating = 3,
        Stoping = 4, 
        Loading = 5,
        Unloading = 6,
        Charging = 7,
        UnCharged = 8, 
        Work = 9
    }

    public enum Direction : int
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    public enum SquareProperty : int
    {
        Border = 0,
        Free = 1,
        LoadX = 2,
        UnLoadX = 3,
        ChargeX = 4,
        LoadY = 5,
        UnLoadY = 6, 
        ChargeY = 7
    }

    public class CommandList
    {
        public CommandList() 
        {
            for (int sp = 0; sp < 8; sp++)
            {
                RotateCount.Add((int)sp, 0);
                MoveCount.Add((int)sp, 0);
                WaitCount.Add((int)sp, 0);
            }
        }

        public AntBot antBot;
        public AntBot antState;
        public AntBot debugAnt;
        public TimeSpan lastTime;

        public Dictionary<int, int> RotateCount = new Dictionary<int, int>();
        public Dictionary<int, int> MoveCount = new Dictionary<int, int>();
        public Dictionary<double, int> WaitCount = new Dictionary<double, int>();


        [XmlIgnore]
        public double metric
        {
            get
            {
                if (antBot.sklad.getMetric == null)
                    return lastTime.TotalMilliseconds;
                else
                    return antBot.sklad.getMetric(this);
            }
        }
        public CommandList(AntBot antBot) {
            this.antBot = antBot;
            antState = antBot.ShalowClone();
            debugAnt = antBot.ShalowClone();
            antState.commandList = new CommandList();
            lastTime = antBot.lastUpdated;
            for (int sp = 0; sp < 8; sp++)
            {
                RotateCount.Add((int)sp, 0);
                MoveCount.Add((int)sp, 0);
                WaitCount.Add((int)sp, 0);
            }
        }

        public CommandList Clone()
        {
            CommandList cl = new CommandList(antBot);
            cl.antState = antState.ShalowClone();
            cl.debugAnt = debugAnt.ShalowClone();
            commands.ForEach(c => {
                var ev = c.Ev.Clone();
                cl.commands.Add((c.Key, ev));
            });
            cl.lastTime= lastTime;
            for(int sp = 0; sp<8; sp++)
            { 
                cl.RotateCount[(int)sp] = RotateCount[(int)sp];
                cl.MoveCount[(int)sp] = MoveCount[(int)sp];
                cl.WaitCount[(int)sp] = WaitCount[(int)sp];
            }
            return cl;
        }

        public bool AddCommand(AntBotAbstractEvent abstractEvent, bool isNeedReserve = true)
        {

            if (commands.Count == 0)
            {
                antState = antBot.ShalowClone();
                lastTime = antBot.lastUpdated;
            }
            if (antState.commandList == null)
            {
                antState.commandList = new CommandList();
            }

            antState.Update(lastTime);
            abstractEvent.antBot = antState;
            if (isNeedReserve)
            {
                if (!abstractEvent.CheckReservation())
                {
                    abstractEvent.CheckReservation();
                    throw new AntBotNotPosibleMovement();
                }
                abstractEvent.ReserveRoom();
            }
            else
            {
                if (!abstractEvent.CheckReservation())
                {
                    return false;
                }
                abstractEvent.CalculatePenalty();
            }
            commands.Add((lastTime, abstractEvent));
            antState.commandList.commands.Add((lastTime, abstractEvent));
            for(int sp = 0; sp<8; sp++)
            {
                antState.commandList.RotateCount[sp] += abstractEvent.RotateCount[sp];
            }
            if (!antState.isClone)
                throw new ExecutionEngineException();
            abstractEvent.runEvent(null, abstractEvent.getEndTime());
            lastTime = abstractEvent.getEndTime();
            for (int sp = 0; sp < 8; sp++)
            {
                RotateCount[sp] += abstractEvent.RotateCount[sp];
                MoveCount[sp] += abstractEvent.MoveCount[sp];
            }
            abstractEvent.antBot = antBot;
            debugAnt = antState.ShalowClone();
            return true;
        }


        public List<(TimeSpan Key, AntBotAbstractEvent Ev)> commands = new List<(TimeSpan Key, AntBotAbstractEvent Ev)>();
    }

    public class AntBot : AbstractObject
    {
        public double xCoordinate;
        public double yCoordinate;
        [XmlIgnore]
        public int xCord => (int)Math.Round(xCoordinate);
        [XmlIgnore]
        public int yCord => (int)Math.Round(yCoordinate);

        public bool isXDirection;
        public double xSpeed;
        public double ySpeed;
        public double charge;
        public bool isLoaded;
        public bool isFree;
        public int targetXCoordinate;
        public int targetYCoordinate;
        public bool targetDirection;
        public TimeSpan waitTime;
        public AntBotState state;
        public Sklad sklad;
        public SkladLogger skladLogger;

        [XmlIgnore]
        public double unitSpeed; // Скорость робота
        [XmlIgnore]
        public double unitAccelerationTime; // Время набора скорости юнита от 0 до UnitSpeed
        [XmlIgnore]
        public double unitStopTime; // Время остановки юнита с UnitSpeed до 0
        [XmlIgnore]
        public double unitRotateTime; //Время разворота юнита на 90 градусов
        [XmlIgnore]
        public double unitAccelerationEnergy; //Стоимость разгона
        [XmlIgnore]
        public double unitStopEnergy; // Энергия на остановку
        [XmlIgnore]
        public double unitMoveEnergy; // Энергия на 1 секунду движения
        [XmlIgnore]
        public double unitRotateEnergy; // Энергия на разворот
        [XmlIgnore]
        public double loadTime;  // Время погрузки
        [XmlIgnore]
        public double unloadTime; // 
        [XmlIgnore]
        public double unitLoadEnergy;
        [XmlIgnore]
        public double unitUnloadEnergy;
        [XmlIgnore]
        public double unitWaitEnergy;
        [XmlIgnore]
        public double unitChargeTime;
        [XmlIgnore]
        public double unitChargeValue;

        [XmlIgnore]
        public bool isDebug;
        [XmlIgnore]
        public CommandList commandList;
        [XmlIgnore]
        public bool isClone = false;

        [XmlIgnore]
        public List<(int x, int y, TimeSpan from, TimeSpan to)> reserved = new List<(int x, int y, TimeSpan from, TimeSpan to)>();

        [XmlIgnore]
        internal List<AbstractObject> objects;

        [XmlIgnore]
        public CommandList escapePath;

        private int nextShift(double speed)
        {
            if (speed > 0)
                return 1;
            if (speed < 0) 
                return -1;
            return 0;
        }

        private double getDelta()
        {
            if (xSpeed < 0)
                return xCoordinate - Math.Floor(xCoordinate);
            if (xSpeed > 0)
                return Math.Ceiling(xCoordinate)-xCoordinate;
            if (ySpeed < 0)
                return yCoordinate - Math.Floor(yCoordinate);
            if (ySpeed > 0)
                return Math.Ceiling(yCoordinate) - yCoordinate;
            return 0;
        }
        



        public (int x, int y) getShift(int shift)
        {
            if (xSpeed < 0)
                return ((int)Math.Floor(xCoordinate) - 1 - shift, (int)yCoordinate);
            if (xSpeed > 0)
                return ((int)Math.Ceiling(xCoordinate) + 1 + shift, (int)yCoordinate);
            if (ySpeed < 0)
                return ((int)(xCoordinate), (int)Math.Floor(yCoordinate) - 1 - shift);
            if (ySpeed > 0)
                return ((int)xCoordinate, (int)Math.Ceiling(yCoordinate) + 1 + shift);
            throw new CheckStateException();
        }
        public override (TimeSpan, AbstractEvent) getNearestEvent(List<AbstractObject> objects)
        {
            TimeSpan timeUncharged;
            switch (state)
            {
                case AntBotState.UnCharged:
                    return (TimeSpan.MaxValue, null);
                case AntBotState.Wait:
                    timeUncharged = lastUpdated + TimeSpan.FromSeconds(charge / unitWaitEnergy);
                    if (commandList.commands.Count> 0)
                    {
                        var task = commandList.commands.First();
                        if (task.Key<timeUncharged)
                            return task;
                    }                   
                    return (timeUncharged, new AntBotUnCharging(this));
                case AntBotState.Move:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotEndTask(this));
                case AntBotState.Accelerating:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotEndTask(this));
                case AntBotState.Rotate:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotEndTask(this));
                case AntBotState.Charging:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotCharged(this));
                case AntBotState.Loading:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotEndTask(this));
                case AntBotState.Unloading:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    return (waitTime, new AntBotEndTask(this));
                case AntBotState.Work:
                    if (charge <= 0)
                        return (lastUpdated, new AntBotUnCharging(this));
                    timeUncharged = lastUpdated + TimeSpan.FromSeconds(charge / unitWaitEnergy);
                    return (waitTime, new AntBotEndTask(this));
            }
            return (TimeSpan.MaxValue, null);
        }



        public double getSecond(TimeSpan timeSpan)
        {
            return (timeSpan - lastUpdated).TotalSeconds;
        }

        public override void Update(TimeSpan timeSpan)
        {
            double second = getSecond(timeSpan);
            xCoordinate += second * xSpeed;
            yCoordinate += second * ySpeed;
            switch (state)
            {
                case AntBotState.Wait:
                    charge -= unitWaitEnergy * second;
                    break;
                case AntBotState.Move:
                    charge -= unitMoveEnergy * second;
                    break;
            }
            lastUpdated = timeSpan;
        }


        public void Stop(TimeSpan timeSpan)
        {
            commandList.commands.Add((timeSpan, new AntBotStop(this)));
        }



        public int getFreePath()
        {

            TimeSpan startInterval = lastUpdated;
            TimeSpan endInterval = startInterval + TimeSpan.FromSeconds(1.0 / unitSpeed);
            if (sklad.squaresIsBusy.CheckIsBusy(xCord, yCord, startInterval, endInterval, uid))
                return 0;

            double delta = getDelta();
            int shift = 0;
            while (isNotBusy(shift, delta)) { shift++; }

            return shift;
        }



        public void CleanReservation()
        {
            reserved.ForEach(res => sklad.squaresIsBusy.UnReserveRoom(res.x, res.y, res.from));
            reserved.Clear();
            commandList.lastTime = lastUpdated;
        }


        private bool isNotBusy(int shift, double delta)
        {
            var coord = getShift(shift);

            TimeSpan startInterval = lastUpdated + TimeSpan.FromSeconds((delta + shift) / unitSpeed);
            TimeSpan endInterval = startInterval + TimeSpan.FromSeconds(2.0 / unitSpeed);
            return !sklad.squaresIsBusy.CheckIsBusy(coord.x, coord.y, startInterval, endInterval, uid);
        }

        public bool CheckRoom(TimeSpan from, TimeSpan to)
        {
            return CheckRoom(xCord, yCord, from, to);
        }

        public bool CheckRoom(int x, int y, TimeSpan from, TimeSpan to)
        {
            return !sklad.squaresIsBusy.CheckIsBusy(x, y, from, to, uid);
        }
        public void ReserveRoom(TimeSpan from, TimeSpan to)
        {
            ReserveRoom(xCord, yCord, from, to);
        }
        public void ReserveRoom(int x, int y, TimeSpan from, TimeSpan to)
        {
            if (!CheckRoom(x, y, from, to)) 
            {
                sklad.squaresIsBusy.PrintRoom(x, y);
                throw new AntBotNotPosibleMovement();
            }
                
            sklad.squaresIsBusy.ReserveRoom(x, y, from, to, uid);         
            reserved.Add((x, y, from, to));
            if (isDebug)
                Console.WriteLine($"Reserve x:{x}, y:{y} from {from} to {to} uid {uid}");
        }


        public AntBot ShalowClone()
        {
            AntBot _antBot = new AntBot();
            _antBot.isClone = true;
            _antBot.charge = this.charge;
            _antBot.lastUpdated = this.lastUpdated;
            _antBot.xCoordinate = this.xCoordinate;
            _antBot.yCoordinate = this.yCoordinate;
            _antBot.xSpeed = this.xSpeed;
            _antBot.ySpeed = this.ySpeed;
            _antBot.uid = this.uid;
            _antBot.sklad = this.sklad;
            _antBot.isXDirection = this.isXDirection;
            _antBot.reserved= this.reserved;
            _antBot.isDebug= this.isDebug;

            _antBot.targetDirection = this.targetDirection;
            _antBot.targetXCoordinate = this.targetXCoordinate;
            _antBot.targetYCoordinate = this.targetYCoordinate;

            _antBot.unitSpeed = this.unitSpeed;
            _antBot.unitAccelerationTime = this.unitAccelerationTime;
            _antBot.unitStopTime = this.unitStopTime;
            _antBot.unitRotateTime = this.unitRotateTime;
            _antBot.unitAccelerationEnergy = this.unitAccelerationEnergy;
            _antBot.unitStopEnergy = this.unitStopEnergy;
            _antBot.unitMoveEnergy = this.unitMoveEnergy;
            _antBot.unitRotateEnergy = this.unitRotateEnergy;
            _antBot.loadTime = this.loadTime;
            _antBot.unloadTime = this.unloadTime;
            _antBot.unitLoadEnergy = this.unitLoadEnergy;
            _antBot.unitUnloadEnergy = this.unitUnloadEnergy;
            _antBot.unitWaitEnergy = this.unitWaitEnergy;
            _antBot.unitChargeTime = this.unitChargeTime;
            _antBot.unitChargeValue = this.unitChargeValue;

            return _antBot;
        }

        internal TimeSpan actionTime()
        {
            return commandList.lastTime;
        }

        internal void RemoveFirstCommand(TimeSpan timeSpan)
        {
            if (commandList != null
                && commandList.commands.Count > 0
                && commandList.commands.First().Key == timeSpan)
            {
                commandList.commands.RemoveAt(0);
            }
        }

        public bool isNeedRotateForDirection(Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Down)
                return isXDirection;
            return !isXDirection;
        }

        public TimeSpan getTimeForFullCharge()
        {
            return TimeSpan.FromSeconds(
                (unitChargeValue - charge) /
                unitChargeValue * unitChargeTime);
        }

        internal bool isHaveReservation()
        {
            return reserved.Any(res=>res.x == xCord && res.y == yCord && res.from <= lastUpdated && res.to >= lastUpdated);
        }

        public void setSpeedByDirection(Direction direction)
        {
            if (direction == Direction.Left)
                xSpeed = -unitSpeed;
            else if (direction == Direction.Right)
                xSpeed = unitSpeed;
            else if (direction == Direction.Up)
                ySpeed = -unitSpeed;
            else if (direction == Direction.Down)
                ySpeed = unitSpeed;
        }
    }

}