﻿using System;
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
        public string objId = null;
        public abstract void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan);
    }

    public abstract class FastAbstractWrapper
    {
        private Dictionary<string, FastAbstractObject> objects = new Dictionary<string, FastAbstractObject>();
        
        protected SortedList<TimeSpan, FastAbstractEvent> eventList = new SortedList<TimeSpan, FastAbstractEvent>();
        protected Dictionary<string, TimeSpan> objectsEventTime = new Dictionary<string, TimeSpan>();
        public TimeSpan updatedTime;
        private HashSet<string> objectsKeyForUpdate;

        public FastAbstractObject getObject(string key)
        {
            var obj = objects[key];
            obj.Update(updatedTime);
            objectsKeyForUpdate.Add(key);
            return objects[key];
        }

        protected void AddEvent(TimeSpan timeSpan, FastAbstractEvent modelEvent, string objUid = null)
        {
            while (eventList.ContainsKey(timeSpan))
            {
                timeSpan = timeSpan.Add(TimeSpan.FromTicks(1));
            }
            modelEvent.objId = objUid;
            eventList.Add(timeSpan, modelEvent);
            if (!(objUid is null))
            {
                objectsEventTime.Add(objUid, timeSpan);
            }
        }

        public bool Next()
        {
            if (eventList.Count == 0)
                return false;
            objectsKeyForUpdate = new HashSet<string>();
            var task = eventList.First();
            updatedTime = task.Key;
            eventList.Remove(task.Key);
            if (!(task.Value.objId is null))
            {
                getObject(task.Value.objId);
            }
            task.Value.runEvent(this, task.Key);
            foreach (var objKey in objectsKeyForUpdate)
            {
                if (objectsEventTime.ContainsKey(objKey)) {
                    eventList.Remove(objectsEventTime[objKey]);
                    objectsEventTime.Remove(objKey);
                }
                var ev = objects[objKey].getNearestEvent();
                if (!(ev.Item2 is null))
                {
                    AddEvent(ev.Item1, ev.Item2, objKey);
                }
            }

            return true;
        }

        public void addObject(FastAbstractObject obj)
        {
            objects.Add(obj.uid, obj);
            var ev = obj.getNearestEvent();
            if (!(ev.Item2 is null))
            {
                AddEvent(ev.Item1, ev.Item2, obj.uid);
            }
        }
    }


}
