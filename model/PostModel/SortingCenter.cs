using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenter : PostCenter
    {

        Dictionary<string, string> routeTable = new Dictionary<string, string>();
 



        public void AddRoute(string directionUid, string gateUid)
        {
            routeTable.Add(directionUid, gateUid);
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
