using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    abstract class PostCenter : FastAbstractObject
    {
        public Dictionary<string, List<Message>> gates = new Dictionary<string, List<Message>>();

        public void AddGate(string gateUid)
        {
            gates.Add(gateUid, new List<Message>());
        }


        protected Dictionary<string, string> routeTable = new Dictionary<string, string>();

        public void AddRoute(string directionUid, string gateUid)
        {
            routeTable.Add(directionUid, gateUid);
        }
    }
}
