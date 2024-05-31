using AbstractModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostModel
{
    class PostOfficeCreate : FastAbstractEvent
    {
        string uid;

        public PostOfficeCreate(string uid)
        {
            this.uid = uid;
        }

        public override List<FastAbstractObject> runEvent(Dictionary<string, FastAbstractObject> objects, TimeSpan timeSpan)
        {
            PostOffice postOffice = new PostOffice();
            postOffice.uid = this.uid;
            objects.Add(postOffice.uid, postOffice);
            Console.WriteLine($"postOffice {postOffice.uid} created {postOffice.lastUpdated}");
            List<FastAbstractObject> result = new List<FastAbstractObject>();
            result.Add(postOffice);
            return result;
        }
    }
}
