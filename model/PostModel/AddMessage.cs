using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class AddMessage : FastAbstractEvent
    {
        string fromUid;
        string toUid;

        public AddMessage(string fromUid, string toUid)
        {
            this.fromUid = fromUid;
            this.toUid = toUid;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            ((PostOffice)wrapper.getObject(fromUid)).AddMessage(timeSpan, toUid);
            Console.WriteLine($"Message {fromUid} to {toUid} added at {timeSpan}");
        }
    }
}
