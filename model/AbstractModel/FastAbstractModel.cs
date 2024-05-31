using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AbstractModel
{
    public abstract class FastAbstractObject
    {
        public string uid = Guid.NewGuid().ToString();
        [XmlElement(Type = typeof(TimeSpan))]
        public TimeSpan lastUpdated;
        public abstract (TimeSpan, FastAbstractEvent) getNearestEvent();
        public abstract void Update(TimeSpan timeSpan);
    }

    public abstract class FastAbstractEvent
    {
        public abstract List<FastAbstractObject> runEvent(Dictionary<string, FastAbstractObject> objects, TimeSpan timeSpan);
    }

    public abstract class FastAbstractWrapper
    {
        public Dictionary<string, FastAbstractObject> objects = new Dictionary<string, FastAbstractObject>();
        protected SortedList<TimeSpan, FastAbstractEvent> eventList = new SortedList<TimeSpan, FastAbstractEvent>();
        public TimeSpan updatedTime;
        private List<FastAbstractObject> objForUpdate;

        protected void AddEvent(TimeSpan timeSpan, FastAbstractEvent modelEvent)
        {
            while (eventList.ContainsKey(timeSpan))
            {
                timeSpan = timeSpan.Add(TimeSpan.FromTicks(1));
            }
            eventList.Add(timeSpan, modelEvent);
        }

        public bool Next()
        {

            if (eventList.Count == 0)
                return false;

            var task = eventList.First();
            updatedTime = task.Key;
            eventList.Remove(task.Key);
            var objForUpdate = task.Value.runEvent(objects, task.Key);
            objForUpdate.ForEach(obj =>
            {
                var near = obj.getNearestEvent();
                if (!(near.Item2 is null))
                {
                    AddEvent(near.Item1, near.Item2);
                } 
            });

            return true;
        }
    }


}
