using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class GenerateTeraplan : FastAbstractEvent
    {
        TaskConfig taskConfig;
        public GenerateTeraplan(TaskConfig taskConfig)
        {
            this.taskConfig = taskConfig;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostWrapper pw = (PostWrapper)wrapper;
            Dictionary<string, SortingCenter> su = new Dictionary<string, SortingCenter>();
            foreach (var poj in taskConfig.PostObjects)
            {
                su.Add(poj.Index, (SortingCenter)wrapper.getObject(poj.Index));
            }
            bool in_teraplan = true;
            foreach (var data in taskConfig.InDates)
            {
                if (data.WeightResult == 0)
                    continue;
                string typeMsg = data.TypeMessageId;
                Message msg = new Message(data.StartIndex, data.EndIndex, typeMsg, data.GroupPrediction, in_teraplan);
                msg.log.Add(new MessageLog(timeSpan, data.StartIndex, "", "Created"));
                pw.messages.Add(msg);
                su[data.StartIndex].inLine.Enqueue((TimeSpan.FromSeconds(10), msg));
            }
        }
    }
}
