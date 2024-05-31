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

        public override List<FastAbstractObject> runEvent(Dictionary<string, FastAbstractObject> objects, TimeSpan timeSpan)
        {
            SortingCenter sortingCenter = new SortingCenter();
            sortingCenter.uid = this.uid;
            objects.Add(sortingCenter.uid, sortingCenter);
            Console.WriteLine($"sortingCenter {sortingCenter.uid} created {sortingCenter.lastUpdated}");
            List<FastAbstractObject> result = new List<FastAbstractObject>();
            result.Add(sortingCenter);
            return result;
        }



    }
}
