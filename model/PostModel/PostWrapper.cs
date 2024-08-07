using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public class PostWrapper : FastAbstractWrapper
    {
        public List<Message> messages = new List<Message>();
        TimeSpan lastAdd = TimeSpan.Zero;
        public void AddPostOffice(string uid)
        {
            lastAdd = AddEvent(lastAdd, new PostOfficeCreate(uid));
        }

        public void AddSortingCenter(string uid)
        {
            lastAdd = AddEvent(lastAdd, new SortingCenterCreate(uid));
        }

        public void CreateGate(string sortingCenterUid, string gateUid)
        {
            lastAdd = AddEvent(lastAdd, new GateCreate(sortingCenterUid, gateUid));
        }

        public void AddRoute(string sortingUid, string directionUid, string gateUid)
        {
            lastAdd = AddEvent(lastAdd, new AddRouteRules(sortingUid, directionUid, gateUid));
        }

        public void AddMessage(TimeSpan timeSpan, string fromUid, string toUid)
        {
            AddEvent(timeSpan, new AddMessage(fromUid, toUid));
        }

        public void AddPostTransport(Dictionary<long, (string postUid, TransportAction tAction)> shedule)
        {
            lastAdd = AddEvent(lastAdd, new PostTransportCreate(shedule));
        }

        public void GenerateTestMessages(int dayNumber, int mailNumber)
        {
            lastAdd = AddEvent(lastAdd, new GenerateTestMessages(dayNumber, mailNumber));
        }

        public void GenerateFullMessages(TaskConfig taskConfig, Dictionary<string, Dictionary<string, InData>> inData)
        {
            lastAdd = AddEvent(lastAdd, new GenerateFullMessage(taskConfig, inData));
        }


        public void ForceUpdate(TimeSpan timeSpan)
        {
            AddEvent(timeSpan, new ForceUpdate());
        }

        public void ForceFinish(TimeSpan timeSpan)
        {
            AddEvent(timeSpan, new ForceFinish());
        }

        internal void MessageLogTransport(List<Message> messages, TimeSpan timeSpan, string post_uid, string transport_uid, string action)
        {
            foreach(var msg in messages)
            {
                msg.log.Add(new MessageLog(timeSpan, post_uid, transport_uid, action));
            }
        }

        internal void MessageLogPost(List<Message> messages, TimeSpan timeSpan, string post_uid, string action)
        {
            foreach (var msg in messages)
            {
                msg.log.Add(new MessageLog(timeSpan, post_uid, "", action));
            }
        }

        internal void MessageLogDelivered(List<Message> messages, TimeSpan timeSpan, string post_uid, string action)
        {
            foreach (var msg in messages)
            {
                if (msg.directionTo == post_uid)
                    msg.log.Add(new MessageLog(timeSpan, post_uid, "", action));
            }
        }
    }
}
