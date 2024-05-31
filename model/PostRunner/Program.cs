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

            while (postWrapper.Next())
            {
            }
        }
    }
}
