using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;
using PostModel;

namespace PostModel
{
    class GenerateFullMessage : FastAbstractEvent
    {
        TaskConfig taskConfig;
        Dictionary<string, Dictionary<string, InData>> inData;
        public GenerateFullMessage(TaskConfig taskConfig, Dictionary<string, Dictionary<string, InData>> inData)
        {
            this.taskConfig = taskConfig;
            this.inData = inData;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            PostWrapper pw = (PostWrapper)wrapper;
            Dictionary<string, SortingCenter> su = new Dictionary<string, SortingCenter>();
            foreach (var poj in taskConfig.PostObjects)
            {
                su.Add(poj.Index, (SortingCenter)wrapper.getObject(poj.Index));
            }
            foreach (var poj1 in taskConfig.PostObjects)
            {
                if (poj1.SuType != "A")
                    continue;
                foreach (var poj2 in taskConfig.PostObjects)
                {
                    if (poj2.SuType != "A")
                        continue;
                    if (poj1 == poj2) continue;
                    bool in_teraplan = false;

                    if (inData.ContainsKey(poj1.Index) && inData[poj1.Index].ContainsKey(poj2.Index))
                        in_teraplan = true;

                    Message msg = new Message(poj1.Index, poj2.Index, "1", "", in_teraplan);
                    msg.log.Add(new MessageLog(timeSpan, poj1.Index, "", "Created"));
                    pw.messages.Add(msg);
                    su[poj1.Index].inLine.Enqueue((TimeSpan.FromSeconds(10), msg));
                }
            }
        }
    }
}
