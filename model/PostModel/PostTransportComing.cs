using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostTransportComing : FastAbstractEvent
    {
        PostTransport postTransport;
        (string postUid, TransportAction tAction) postAction;
        public PostTransportComing(PostTransport postTransport, (string postUid, TransportAction tAction) postAction) 
        {
            this.postTransport = postTransport;
            this.postAction = postAction;
        }


        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            Console.WriteLine($"Transport {postTransport.uid} is comming to {postAction.postUid} at {timeSpan} for {postAction.tAction}");
        }
    }
}
