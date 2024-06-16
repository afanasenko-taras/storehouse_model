using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class AddRouteRules : FastAbstractEvent
    {

        string sortingUid;
        string directionUid;
        string gateUid;

        public AddRouteRules(string sortingUid, string directionUid, string gateUid)
        {
            this.sortingUid = sortingUid;
            this.directionUid = directionUid;
            this.gateUid = gateUid;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostCenter sc = (PostCenter)wrapper.getObject(sortingUid);
            sc.AddRoute(directionUid, gateUid);
            wrapper.WriteDebug($"PostCenter {sc.uid} added route rules {directionUid} send to gate {gateUid} time {timeSpan}");
        }
    }
}
