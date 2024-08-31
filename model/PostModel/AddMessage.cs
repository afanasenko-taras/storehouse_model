using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class AddMessage : FastAbstractEvent
    {
        Message message;

        public AddMessage(Message message)
        {
            this.message = message;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {

            ((PostOffice)wrapper.getObject(message.directionFrom)).AddMessage(timeSpan, message);
            wrapper.WriteDebug($"Message {message.directionFrom} to {message.directionTo} added at {timeSpan}");
        }
    }
}
