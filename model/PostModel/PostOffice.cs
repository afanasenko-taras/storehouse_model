using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{

    class PostOffice : PostCenter
    {


        SortedList<TimeSpan, string> messageSource = new SortedList<TimeSpan, string>();

        public void AddMessage(TimeSpan timeSpan, string toUid)
        {
            messageSource.Add(timeSpan, toUid);
        }

        public override (TimeSpan, FastAbstractEvent) getNearestEvent()
        {
            return (TimeSpan.MaxValue, null);
        }

        public override void Update(TimeSpan timeSpan)
        {
            // No need update
        }
    }
}
