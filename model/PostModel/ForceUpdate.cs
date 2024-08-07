using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class ForceUpdate : FastAbstractEvent
    {
        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            foreach(string key in wrapper.getObjectKeys())
            {
                wrapper.getObject(key);
            }
            Console.WriteLine("ForceUpdate!");
        }
    }
}
