using AbstractModel;
using System;

namespace PostModel
{
    internal class PostTransportDestroy : FastAbstractEvent
    {
        private PostTransport postTransport;

        public PostTransportDestroy(PostTransport postTransport)
        {
            this.postTransport = postTransport;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            wrapper.RemoveObjects(postTransport.uid);
        }
    }
}