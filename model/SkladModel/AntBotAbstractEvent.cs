using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkladModel
{
    public abstract class AntBotAbstractEvent : AbstractEvent
    {
        public AntBot antBot;
        public Dictionary<int, int> RotateCount = new Dictionary<int, int>();
        public Dictionary<int, int> MoveCount = new Dictionary<int, int>();
        public Dictionary<int, double> WaitCount = new Dictionary<int, double>();
        
        public AntBotAbstractEvent()
        {
            for (int sp = 0; sp < 8; sp++)
            {
                RotateCount.Add((int)sp, 0);
                MoveCount.Add((int)sp, 0);
                WaitCount.Add((int)sp, 0);
            }
        }

        public abstract TimeSpan getStartTime();
        public abstract TimeSpan getEndTime();
        public abstract bool CheckReservation();
        public abstract void ReserveRoom();
        public abstract AntBotAbstractEvent Clone();

        public virtual void CalculatePenalty() { }


    }

}
