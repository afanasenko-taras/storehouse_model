using System;
using System.Collections.Generic;
using PostModel;

namespace PostRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            PostWrapper postWrapper = new PostWrapper();
            postWrapper.AddPostOffice("1");
            postWrapper.AddPostOffice("2");
            postWrapper.AddPostOffice("3");
            postWrapper.AddPostOffice("4");
            postWrapper.AddSortingCenter("5");
            postWrapper.AddSortingCenter("6");
            postWrapper.AddSortingCenter("7");


            postWrapper.CreateGate("5", "1");
            postWrapper.CreateGate("5", "2");
            postWrapper.CreateGate("5", "7");
            postWrapper.CreateGate("6", "3");
            postWrapper.CreateGate("6", "4");
            postWrapper.CreateGate("6", "7");
            postWrapper.CreateGate("7", "5");
            postWrapper.CreateGate("7", "6");

            postWrapper.CreateGate("1", "5");
            postWrapper.CreateGate("2", "5");
            postWrapper.CreateGate("3", "6");
            postWrapper.CreateGate("4", "6");


            postWrapper.AddRoute("5", "3", "7");
            postWrapper.AddRoute("5", "4", "7");
            postWrapper.AddRoute("5", "2", "2");
            postWrapper.AddRoute("5", "1", "1");

            postWrapper.AddRoute("6", "1", "7");
            postWrapper.AddRoute("6", "2", "7");
            postWrapper.AddRoute("6", "3", "3");
            postWrapper.AddRoute("6", "4", "4");

            postWrapper.AddRoute("7", "3", "6");
            postWrapper.AddRoute("7", "4", "6");
            postWrapper.AddRoute("7", "2", "5");
            postWrapper.AddRoute("7", "1", "5");


            postWrapper.AddRoute("1", "2", "5");
            postWrapper.AddRoute("1", "3", "5");
            postWrapper.AddRoute("1", "4", "5");

            postWrapper.AddRoute("2", "1", "5");
            postWrapper.AddRoute("2", "3", "5");
            postWrapper.AddRoute("2", "4", "5");

            postWrapper.AddRoute("3", "4", "6");
            postWrapper.AddRoute("3", "1", "6");
            postWrapper.AddRoute("3", "2", "6");

            postWrapper.AddRoute("4", "3", "6");
            postWrapper.AddRoute("4", "1", "6");
            postWrapper.AddRoute("4", "2", "6");


            postWrapper.AddMessage(TimeSpan.FromHours(1), "1", "2");
            postWrapper.AddMessage(TimeSpan.FromHours(2), "1", "3");
            postWrapper.AddMessage(TimeSpan.FromHours(20), "1", "4");


            Dictionary<int, (string postUid, TransportAction tAction)> shedule = new Dictionary<int, (string postUid, TransportAction tAction)>();
            
            shedule.Add(10, ("5", TransportAction.Load));
            shedule.Add(12, ("1", TransportAction.Both));
            shedule.Add(14, ("5", TransportAction.Unload));
            postWrapper.AddPostTransport(shedule);

            shedule = new Dictionary<int, (string postUid, TransportAction tAction)>();
            shedule.Add(11, ("5", TransportAction.Load));
            shedule.Add(13, ("2", TransportAction.Both));
            shedule.Add(14, ("5", TransportAction.Unload));
            postWrapper.AddPostTransport(shedule);

            while (postWrapper.Next() & postWrapper.updatedTime < TimeSpan.FromDays(5))
            {
            }
        }
    }
}
