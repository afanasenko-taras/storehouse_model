using AbstractModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PostModel
{
    class GenerateTestMessages : FastAbstractEvent
    {
        int dayNumber = 1;
        int mailNumber = 100000;

        public GenerateTestMessages(int dayNumber, int mailNumber)
        {
            this.dayNumber = dayNumber;
            this.mailNumber = mailNumber;
        }

        public override void runEvent(FastAbstractWrapper wrapper, TimeSpan timeSpan)
        {
            Dictionary<string, PostOffice> offices = new Dictionary<string, PostOffice>();
            offices.Add("1", (PostOffice)wrapper.getObject("1"));
            offices.Add("2", (PostOffice)wrapper.getObject("2"));
            offices.Add("3", (PostOffice)wrapper.getObject("3"));
            offices.Add("4", (PostOffice)wrapper.getObject("4"));

            var sw1 = new Stopwatch();
            sw1.Start();
            Random rnd = new Random();
            Enumerable.Range(0, dayNumber).ToList().ForEach(day => {
                Console.WriteLine($"{day} {sw1.Elapsed}");
                Enumerable.Range(0, mailNumber).ToList().ForEach(mail =>
                {
                    long tick = TimeSpan.TicksPerDay * day +
                        TimeSpan.TicksPerHour * 8 +
                        (8 * TimeSpan.TicksPerHour / mailNumber) * mail;

                    offices["1"].AddMessage(new TimeSpan(tick+0), "2");
                    offices["1"].AddMessage(new TimeSpan(tick+1), "3");
                    offices["1"].AddMessage(new TimeSpan(tick+2), "4");

                    offices["2"].AddMessage(new TimeSpan(tick + 3), "1");
                    offices["2"].AddMessage(new TimeSpan(tick + 4), "3");
                    offices["2"].AddMessage(new TimeSpan(tick + 5), "4");

                    offices["3"].AddMessage(new TimeSpan(tick + 6), "1");
                    offices["3"].AddMessage(new TimeSpan(tick + 7), "2");
                    offices["3"].AddMessage(new TimeSpan(tick + 8), "4");

                    offices["4"].AddMessage(new TimeSpan(tick + 9), "1");
                    offices["4"].AddMessage(new TimeSpan(tick + 10), "2");
                    offices["4"].AddMessage(new TimeSpan(tick + 11), "3");

                });
            });
            sw1.Stop();
            Console.WriteLine($"time for add test message {sw1.Elapsed}");
            wrapper.WriteDebug($"Messages ADD at {timeSpan}");
        }
    }
}
