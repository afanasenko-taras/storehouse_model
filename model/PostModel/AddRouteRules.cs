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
        string typeMsg;

        public AddRouteRules(string sortingUid, string directionUid, string gateUid, string typeMsg)
        {
            this.sortingUid = sortingUid;
            this.directionUid = directionUid;
            this.gateUid = gateUid;
            this.typeMsg = typeMsg;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostCenter sc = (PostCenter)wrapper.getObject(sortingUid);
            sc.AddRoute(directionUid, gateUid, typeMsg);
            wrapper.WriteDebug($"PostCenter {sc.uid} added route rules {directionUid} send to gate {gateUid} time {timeSpan}");
        }
    }
}
