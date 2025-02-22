﻿using AbstractModel;
using System;

namespace PostModel
{
    class ForceFinish : FastAbstractEvent
    {
        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            foreach (string key in wrapper.getObjectKeys())
            {
                var obj = wrapper.getObject(key);
                if (obj is SortingCenter)
                {
                    var sc = (SortingCenter)obj;
                    foreach (var gate in sc.gates)
                    {
                        foreach (var msg in gate.Value)
                        {
                            msg.log.Add(new MessageLog(timeSpan, sc.uid, "", "Undelivered"));
                        }
                    }
                    foreach (var msg in sc.inLine)
                    {
                        msg.message.log.Add(new MessageLog(timeSpan, sc.uid, "", "Unsorted"));
                    }
                }
            }
            ((PostWrapper)wrapper).isFinished = true;
            Console.WriteLine("ForceFinish!");
        }
    }
}
