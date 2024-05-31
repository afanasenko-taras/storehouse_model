using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenter : FastAbstractObject
    {

        Dictionary<string, string> routeTable = new Dictionary<string, string>();
        Dictionary<string, List<Message>> gates = new Dictionary<string, List<Message>>(); 

        public void AddGate(string gateUid)
        {
            gates.Add(gateUid, new List<Message>());
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
