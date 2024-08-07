using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenterCreate : FastAbstractEvent
    {
        string uid;

        public SortingCenterCreate(string uid)
        {
            this.uid = uid;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            SortingCenter sortingCenter = new SortingCenter();
            sortingCenter.wrapper = wrapper;
            sortingCenter.uid = this.uid;
            wrapper.addObject(sortingCenter);
            wrapper.WriteDebug($"sortingCenter {sortingCenter.uid} created {sortingCenter.lastUpdated} time {timeSpan}");
        }

    }
}
