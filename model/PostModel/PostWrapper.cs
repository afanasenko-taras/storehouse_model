using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    public class PostWrapper : FastAbstractWrapper
    {

        public void AddPostOffice(string uid)
        {
            AddEvent(TimeSpan.Zero, new PostOfficeCreate(uid));
        }


    }
}
