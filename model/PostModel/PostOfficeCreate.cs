using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostOfficeCreate : FastAbstractEvent
    {
        string uid;

        public PostOfficeCreate(string uid)
        {
            this.uid = uid;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostOffice postOffice = new PostOffice();
            postOffice.uid = this.uid;
            wrapper.addObject(postOffice);
            wrapper.WriteDebug($"postOffice {postOffice.uid} created {postOffice.lastUpdated}");
        }
    }
}
