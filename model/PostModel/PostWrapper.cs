using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public class PostWrapper : FastAbstractWrapper
    {


        public void AddPostOffice(string uid)
        {
            AddEvent(TimeSpan.Zero, new PostOfficeCreate(uid));
        }

        public void AddSortingCenter(string uid)
        {
            AddEvent(TimeSpan.Zero, new SortingCenterCreate(uid));
        }

        public void CreateGate(string sortingCenterUid, string gateUid)
        {
            AddEvent(TimeSpan.Zero, new GateCreate(sortingCenterUid, gateUid));
        }

        public void AddRoute(string sortingUid, string directionUid, string gateUid)
        {
            AddEvent(TimeSpan.Zero, new AddRouteRules(sortingUid, directionUid, gateUid));
        }

        public void AddMessage(TimeSpan timeSpan, string fromUid, string toUid)
        {
            AddEvent(timeSpan, new AddMessage(fromUid, toUid));
        }

        public void AddPostTransport(long tick, Dictionary<long, (string postUid, TransportAction tAction)> shedule)
        {
            AddEvent( new TimeSpan(tick), new PostTransportCreate(shedule));
        }

        public void GenerateTestMessages(int dayNumber, int mailNumber)
        {
            AddEvent(TimeSpan.Zero, new GenerateTestMessages(dayNumber, mailNumber));
        }

    }
}
