﻿using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class SortingCenter : AbstractObject
    {


        public override (TimeSpan, AbstractEvent) getNearestEvent(List<AbstractObject> objects)
        {
            throw new NotImplementedException();
        }

        public override void Update(TimeSpan timeSpan)
        {
            lastUpdated = timeSpan;
        }
    }
}
