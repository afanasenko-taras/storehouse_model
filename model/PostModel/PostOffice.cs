using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{

    class PostOffice : AbstractObject
    {
        // direcion, type of message
        Dictionary<int, List<Message>> exitPools = new Dictionary<int, List<Message>>();

        

        public override (TimeSpan, AbstractEvent) getNearestEvent(List<AbstractObject> objects)
        {
            throw new NotImplementedException();
        }

        public override void Update(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
