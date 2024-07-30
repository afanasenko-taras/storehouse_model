using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public class PostWrapper : FastAbstractWrapper
    {

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

        public void AddPostTransport(long tick, Dictionary<long, (string postUid, TransportAction tAction)> shedule)
        {
            AddEvent(new TimeSpan(tick), new PostTransportCreate(shedule));
        }

        public void GenerateTestMessages(int dayNumber, int mailNumber)
        {
            lastAdd = AddEvent(lastAdd, new GenerateTestMessages(dayNumber, mailNumber));
        }

    }
}
