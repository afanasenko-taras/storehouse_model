using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostModel
{

    public enum TransportAction : int
    {
        Load = 0,
        Unload = 1,
        Both = 2
    }


    class PostTransport : FastAbstractObject
    {
        public Dictionary<long, (string postUid, TransportAction tAction)> shedule;
        public Dictionary<string, List<Message>> messageOnBoard = new Dictionary<string, List<Message>>();

        public PostTransport(Dictionary<long, (string postUid, TransportAction tAction)> shedule)
        {
            this.shedule = shedule;
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            long last_tick = this.lastUpdated.Ticks - this.lastUpdated.Days * TimeSpan.TicksPerDay;
            foreach(var tick in shedule.Keys)
            {
                if (tick > last_tick)
                {
                    return (TimeSpan.FromDays(lastUpdated.Days) + TimeSpan.FromTicks(tick), 
                        new PostTransportComing(this.uid, shedule[tick]));
                }
            }

            var tick1 = shedule.Keys.First();
            return (TimeSpan.FromDays(lastUpdated.Days + 1) + TimeSpan.FromTicks(tick1),
                        new PostTransportComing(this.uid, shedule[tick1]));

        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
