using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public abstract class PostCenter : FastAbstractObject
    {
        public Dictionary<string, List<Message>> gates = new Dictionary<string, List<Message>>();

        public void AddGate(string gateUid)
        {
            if (!gates.ContainsKey(gateUid))
                gates.Add(gateUid, new List<Message>());
        }


        protected Dictionary<string, Dictionary<string, string>> routeTable = new Dictionary<string, Dictionary<string, string>>();

        public void AddRoute(string directionUid, string gateUid, string typeMsg)
        {
            if (!routeTable.ContainsKey(typeMsg))
                routeTable.Add(typeMsg, new Dictionary<string, string>());

            routeTable[typeMsg].Add(directionUid, gateUid);
        }
    }
}
