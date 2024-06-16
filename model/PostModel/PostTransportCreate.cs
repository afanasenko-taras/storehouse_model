using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostTransportCreate : FastAbstractEvent
    {
        Dictionary<long, (string postUid, TransportAction tAction)> shedule;
        public PostTransportCreate(Dictionary<long, (string postUid, TransportAction tAction)> shedule)
        {
            this.shedule = shedule;
        }


        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostTransport postTransport = new PostTransport(shedule);
            wrapper.addObject(postTransport);
            wrapper.WriteDebug($"postTransport {postTransport.uid} created {timeSpan}");
        }
    }
}
