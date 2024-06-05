using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class Message
    { 
        public Message(string directionFrom, string directionTo)
        {
            this.directionFrom = directionFrom;
            this.directionTo = directionTo;
        }
        public string directionFrom;
        public string directionTo;
    }
}
