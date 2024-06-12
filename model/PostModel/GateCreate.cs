using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class GateCreate : FastAbstractEvent
    {
        string sortingUid;
        string directionUid;

        public GateCreate(string sortingUid, string directionUid)
        {
            this.sortingUid = sortingUid;
            this.directionUid = directionUid;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostCenter sc = (PostCenter)wrapper.getObject(sortingUid);
            sc.AddGate(directionUid);
            ((PostWrapper)wrapper).WriteDebug($"{sc.uid} added gate to {directionUid} time {timeSpan}");
        }
    }
}
