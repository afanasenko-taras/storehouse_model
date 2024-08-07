using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenter : PostCenter
    {

        public Queue<(TimeSpan exitTime, Message message)> inLine = new Queue<(TimeSpan exitTime, Message message)>();
        internal FastAbstractWrapper wrapper;

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
            (TimeSpan exitTime, Message message) msg;
            while(inLine.TryPeek(out msg) & msg.exitTime + TimeSpan.FromHours(4) < lastUpdated)
            {
                if (!routeTable.ContainsKey(msg.message.directionTo))
                {
                    inLine.Dequeue();
                    msg.message.log.Add(new MessageLog(timeSpan, uid, "", "NoRouterFound"));
                    continue;
                }
                gates[routeTable[msg.message.directionTo]].Add(msg.message);
                inLine.Dequeue();
            }
        }
    }
}
