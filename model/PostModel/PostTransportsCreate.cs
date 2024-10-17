using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostTransportsCreate : FastAbstractEvent
    {
        List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules;
        TimeSpan startTime;
        public PostTransportsCreate(List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules, TimeSpan timeSpan)
        {
            this.shedules = shedules;
            this.startTime = timeSpan;
        }


        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            foreach (var shedule in shedules)
            {
                PostTransport postTransport = new PostTransport(shedule, startTime);
                wrapper.addObject(postTransport);
                wrapper.WriteDebug($"postTransport {postTransport.uid} created {timeSpan}");
            }
        }
    }
}
