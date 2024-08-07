using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public class Message
    {
        public List<MessageLog> log = new List<MessageLog>();
        public Message()
        {

        }
        public Message(string directionFrom, string directionTo, bool in_teraplan=false)
        {
            this.directionFrom = directionFrom;
            this.directionTo = directionTo;
            this.in_teraplan = in_teraplan;
        }
        public string directionFrom;
        public string directionTo;
        public bool in_teraplan;
    }

    public class MessageLog
    {
        public TimeSpan ActionTime;
        public string PostID;
        public string TransportID;
        public string Action;

        public MessageLog() { }
        public MessageLog(TimeSpan timeSpan, string index, string TransportID, string action)
        {
            this.ActionTime = timeSpan;
            this.PostID = index;
            this.TransportID = TransportID;
            this.Action = action;
        }
    }
}
