using System;
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

            while (postWrapper.Next())
            {
            }
        }
    }
}
