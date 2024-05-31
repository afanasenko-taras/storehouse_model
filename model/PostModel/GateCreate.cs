using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class GateCreate : FastAbstractEvent
    {
        string sortingUid;
        string directionUid;

        public GateCreate(string sortingUid, string directionUid)
        {
            this.sortingUid = sortingUid;
            this.directionUid = directionUid;
        }

        public override List<FastAbstractObject> runEvent(Dictionary<string, FastAbstractObject> objects, TimeSpan timeSpan)
        {

            SortingCenter sc = (SortingCenter)objects[sortingUid];
            sc.AddGate(directionUid);
            Console.WriteLine($"sortingCenter {sc.uid} added gate too {directionUid}");
            List<FastAbstractObject> result = new List<FastAbstractObject>();
            return result;
        }
    }
}
