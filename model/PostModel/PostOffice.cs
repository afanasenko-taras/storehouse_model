using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{

    class PostOffice : FastAbstractObject
    {
        // direcion, type of message
        Dictionary<int, List<Message>> exitPools = new Dictionary<int, List<Message>>();


        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }
    }
}
