using AbstractModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PostModel
{
    public class PostWrapper : FastAbstractWrapper
    {
        public List<Message> messages = new List<Message>();
        TimeSpan lastAdd = TimeSpan.Zero;
        DateTime startModelTime = DateTime.Parse("01/01/2024", new CultureInfo("en-US", true));
        public Dictionary<string, string> id2index = new Dictionary<string, string>();
        public Dictionary<string, PostObject> id2poj = new Dictionary<string, PostObject>();
        public bool isFinished = false;

        public PostWrapper(TaskConfig taskConfig)
        {
            foreach (var poj in taskConfig.PostObjects)
            {
                id2index.Add(poj.Id, poj.Index);
                id2poj.Add(poj.Id, poj);
            }
        }

        public void AddPostOffice(string uid)
        {
            lastAdd = AddEvent(lastAdd, new PostOfficeCreate(uid));
        }

        public void AddSortingCenter(string uid, PostObject poj)
        {
            lastAdd = AddEvent(lastAdd, new SortingCenterCreate(uid, poj));
        }

        public void CreateGate(string sortingCenterUid, string gateUid)
        {
            lastAdd = AddEvent(lastAdd, new GateCreate(sortingCenterUid, gateUid));
        }

        public void AddRoute(string sortingUid, string directionUid, string? gateUid, string typeMsg)
        {
            lastAdd = AddEvent(lastAdd, new AddRouteRules(sortingUid, directionUid, gateUid, typeMsg));
        }

        public void AddMessage(TimeSpan timeSpan, Message message)
        {
            AddEvent(timeSpan, new AddMessage(message));
        }

        public void GenerateTestMessages(int dayNumber, int mailNumber)
        {
            lastAdd = AddEvent(lastAdd, new GenerateTestMessages(dayNumber, mailNumber));
        }

        public void GenerateFullMessages(TaskConfig taskConfig, Dictionary<string, Dictionary<string, InData>> inData)
        {
            lastAdd = AddEvent(lastAdd, new GenerateFullMessage(taskConfig, inData));
        }

        public void GenerateTeraplan(TaskConfig taskConfig)
        {
            lastAdd = AddEvent(lastAdd, new GenerateTeraplan(taskConfig));
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

        public void PostSedulesCreate(List<Dictionary<long, (string postUid, TransportAction tAction)>> shedules)
        {
            lastAdd = AddEvent(lastAdd, new PostShedulesCreate(shedules));
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
                {
                    msg.log.Add(new MessageLog(timeSpan, post_uid, "", action));
                }
            }
        }
    }
}
