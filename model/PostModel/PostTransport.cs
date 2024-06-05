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
        public Dictionary<int, (string postUid, TransportAction tAction)> shedule;
        public Dictionary<string, List<Message>> messageOnBoard = new Dictionary<string, List<Message>>();

        public PostTransport(Dictionary<int, (string postUid, TransportAction tAction)> shedule)
        {
            this.shedule = shedule;
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            int last_hour = this.lastUpdated.Hours;
            foreach(var hour in shedule.Keys)
            {
                if (hour>last_hour)
                {
                    return (TimeSpan.FromDays(lastUpdated.Days) + TimeSpan.FromHours(hour), 
                        new PostTransportComing(this.uid, shedule[hour]));
                }
            }

            var hour1 = shedule.Keys.First();
            return (TimeSpan.FromDays(lastUpdated.Days + 1) + TimeSpan.FromHours(hour1),
                        new PostTransportComing(this.uid, shedule[hour1]));

        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
