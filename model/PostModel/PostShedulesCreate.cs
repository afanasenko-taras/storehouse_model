using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostShedulesCreate : FastAbstractEvent
    {
        List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules;
        public PostShedulesCreate(List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules)
        {
            this.shedules = shedules;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostShedules postShedules = new PostShedules(shedules, timeSpan);
            wrapper.addObject(postShedules);
            wrapper.WriteDebug($"postShedules {postShedules.uid} created {timeSpan}");
        }
    }
}
