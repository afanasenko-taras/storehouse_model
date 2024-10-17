using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostShedules : FastAbstractObject
    {
        List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules;
        TimeSpan lastGenerated;

        public PostShedules(List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules, TimeSpan timeSpan)
        {
            this.shedules = shedules;
            this.lastGenerated = timeSpan;
        }


        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            TimeSpan timeSpan = lastGenerated;
            lastGenerated = TimeSpan.FromDays((int)lastGenerated.TotalDays + 1);
            return (timeSpan, new PostTransportsCreate(shedules, timeSpan));
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
