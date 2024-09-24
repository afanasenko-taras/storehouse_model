using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenterCreate : FastAbstractEvent
    {
        string uid;
        PostObject poj;

        public SortingCenterCreate(string uid, PostObject poj)
        {
            this.uid = uid;
            this.poj = poj;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            SortingCenter sortingCenter = new SortingCenter();
            sortingCenter.wrapper = (PostWrapper)wrapper;
            sortingCenter.uid = this.uid;
            sortingCenter.poj = poj;
            

            foreach (var gate in poj.Gates)
            {
                sortingCenter.AddGate(sortingCenter.wrapper.id2index[gate]);
                wrapper.WriteDebug($"{sortingCenter.uid} added gate to {sortingCenter.wrapper.id2index[gate]} time {timeSpan}");
            }

            foreach (var rules in poj.Route)
            {
                string msgType = rules.Key;
                foreach (var route in rules.Value)
                {
                    if (route.Value is null)
                    {
                        sortingCenter.AddRoute(sortingCenter.wrapper.id2index[route.Key], null, msgType);
                        wrapper.WriteDebug($"PostCenter {sortingCenter.uid} added route rules {sortingCenter.wrapper.id2index[route.Key]} send to gate local time {timeSpan}");
                    }
                    else
                    {
                        sortingCenter.AddRoute(sortingCenter.wrapper.id2index[route.Key], 
                            sortingCenter.wrapper.id2index[route.Value], msgType);
                        wrapper.WriteDebug($"PostCenter {sortingCenter.uid} added route rules {sortingCenter.wrapper.id2index[route.Key]} send to gate {sortingCenter.wrapper.id2index[route.Value]} time {timeSpan}");
                    }
                }
            }


            wrapper.addObject(sortingCenter);
            wrapper.WriteDebug($"sortingCenter {sortingCenter.uid} created {sortingCenter.lastUpdated} time {timeSpan}");
        }

    }
}
