using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class Message : AbstractObject
    {
        public override (TimeSpan, AbstractEvent) getNearestEvent(List<AbstractObject> objects)
        {
            throw new NotImplementedException();
        }

        public override void Update(TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        int type;
        int directionFrom;
        int directionTo;

    }
}
