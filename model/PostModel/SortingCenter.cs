using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenter : PostCenter
    {

        public Queue<(TimeSpan exitTime, Message message)> inLine = new Queue<(TimeSpan exitTime, Message message)>();

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
            (TimeSpan exitTime, Message message) msg;
            int count = 0;
            while(inLine.TryPeek(out msg) & msg.exitTime + TimeSpan.FromHours(4) < lastUpdated)
            {
                gates[routeTable[msg.message.directionTo]].Add(msg.message);
                count++;
                inLine.Dequeue();
            }
        }
    }
}
