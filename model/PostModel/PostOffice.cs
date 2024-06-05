using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostModel
{

    class PostOffice : PostCenter
    {


        SortedList<TimeSpan, string> messageSource = new SortedList<TimeSpan, string>();

        public void AddMessage(TimeSpan timeSpan, string toUid)
        {
            messageSource.Add(timeSpan, toUid);
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
            if (messageSource.Count == 0)
                return;
            var message = messageSource.First();
            while (message.Key < lastUpdated)
            {
                messageSource.Remove(message.Key);
                gates[routeTable[message.Value]].Add(new Message(this.uid, message.Value));
                if (messageSource.Count == 0)
                    break;
                message = messageSource.First();
            }
        }
    }
}
