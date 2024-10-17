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
        public Dictionary<string, List<Message>> messageOnBoard = new Dictionary<string, List<Message>>();
        public TimeSpan startTime;
        public Queue<(long time, string postUid, TransportAction tAction)> actions = new Queue<(long time, string postUid, TransportAction tAction)>();
        public Dictionary<long, (string postUid, TransportAction tAction)> shedule;

        public PostTransport(Dictionary<long, (string postUid, TransportAction tAction)> shedule, TimeSpan startTime)
        {
            this.startTime = TimeSpan.FromDays((int)startTime.TotalDays);
            this.shedule = shedule;
            foreach (var tick in shedule.Keys)
            {
                var value = shedule[tick];
                actions.Enqueue((tick, value.postUid, value.tAction));
            }
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            if (actions.Count > 0)
            {
                var action = actions.Peek();
                return (startTime + TimeSpan.FromTicks(action.time), new PostTransportComing(this.uid, shedule[action.time]));

            } else
            {
                return (lastUpdated, new PostTransportDestroy(this));
            }

        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
