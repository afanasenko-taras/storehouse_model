using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    abstract class PostCenter : FastAbstractObject
    {
        protected Dictionary<string, List<Message>> gates = new Dictionary<string, List<Message>>();

        public void AddGate(string gateUid)
        {
            gates.Add(gateUid, new List<Message>());
        }
    }
}
