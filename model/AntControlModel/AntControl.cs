using SkladModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntControlModel
{
    class AntControl
    {
        SkladWrapper skladWrapper;
        public AntControl(SkladWrapper skladWrapper)
        {
            this.skladWrapper = skladWrapper;
        }

        public void Run()
        {
            Run(TimeSpan.MaxValue);
        }
        public void Run(TimeSpan maxModelTime)
        {

            while (skladWrapper.Next())
            {

                if (!skladWrapper.isEventCountEmpty())
                    continue;


                if (skladWrapper.updatedTime > maxModelTime)
                    break;


                if (skladWrapper.GetAllAnts().Any(x => x.state == AntBotState.UnCharged))
                {
                    Console.WriteLine("UNCHARGED");
                    break;
                }

            }
        }



    }
}
