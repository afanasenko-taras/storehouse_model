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

    }
}
