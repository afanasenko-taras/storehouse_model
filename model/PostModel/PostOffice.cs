using AbstractModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostModel
{

    class PostOffice : PostCenter
    {

        SortedDictionary<TimeSpan, Message> messageSource = new SortedDictionary<TimeSpan, Message>();

        public void AddMessage(TimeSpan timeSpan, Message message)
        {
            messageSource.Add(timeSpan, message);
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
                gates[routeTable[message.Value.typeMsg][message.Value.directionTo]].Add(message.Value);
                if (messageSource.Count == 0)
                    break;
                message = messageSource.First();
            }
        }
    }
}
