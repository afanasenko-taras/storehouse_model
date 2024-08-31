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
        public Message(string directionFrom, string directionTo, string typeMsg, string groupPrediction="", bool in_teraplan=false)
        {
            this.directionFrom = directionFrom;
            this.directionTo = directionTo;
            this.in_teraplan = in_teraplan;
            this.typeMsg = typeMsg;
            this.groupPrediction = groupPrediction;
        }
        public string directionFrom;
        public string directionTo;
        public string typeMsg;
        public bool in_teraplan;
        public string groupPrediction;
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
