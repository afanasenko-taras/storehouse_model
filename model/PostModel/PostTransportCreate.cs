using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostTransportCreate : FastAbstractEvent
    {
        Dictionary<int, (string postUid, TransportAction tAction)> shedule;
        public PostTransportCreate(Dictionary<int, (string postUid, TransportAction tAction)> shedule)
        {
            this.shedule = shedule;
        }


        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostTransport postTransport = new PostTransport(shedule);
            wrapper.addObject(postTransport);
            ((PostWrapper)wrapper).WriteDebug($"postTransport {postTransport.uid} created {timeSpan}");
        }
    }
}
