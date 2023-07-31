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
            Random rnd = new Random(DateTime.Now.Millisecond);
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


                var freeAnts = skladWrapper.GetFreeUnloadedAnts();
                var sb = skladWrapper.GetSklad().squaresIsBusy;
                freeAnts.ForEach(freeAnt =>
                {
                    int d = rnd.Next(4);
                    Direction dir = (Direction)d;
                    int dist = skladWrapper.getFreePath(freeAnt, dir, freeAnt.lastUpdated);
                    if (dist != 0)
                    {
                        int x = freeAnt.xCord;
                        int y = freeAnt.yCord;
                        if (dir == Direction.Left)
                            x--;
                        if (dir == Direction.Right)
                            x++;
                        if (dir == Direction.Up)
                            y--;
                        if (dir == Direction.Down)
                            y++;

                        if (freeAnt.sklad.squaresIsBusy.GetPosibleReserve(x, y, freeAnt.lastUpdated) == TimeSpan.MaxValue)
                            skladWrapper.Move(freeAnt, dir, 1, 0);
                        else
                        {
                            freeAnt.CleanReservation();
                            freeAnt.commandList.AddCommand(new AntBotWait(freeAnt, TimeSpan.FromSeconds(1)));
                            freeAnt.commandList.AddCommand(new AntBotStop(freeAnt));
                        }
                    } else
                    {
                        freeAnt.CleanReservation();
                        freeAnt.commandList.AddCommand(new AntBotWait(freeAnt, TimeSpan.FromSeconds(1)));
                        freeAnt.commandList.AddCommand(new AntBotStop(freeAnt));
                    }
                });


            }
        }



        public class Feromon
        {
            private Dictionary<int, Dictionary<int, double>> feromon;
            Random rnd = new Random();
            public Feromon(Dictionary<int, Dictionary<int, double>> feromon)
            {
                this.feromon = feromon;
            }

            public void Activate(int x, int y)
            {
                feromon[x][y] += 1.0;
            }

            public void Update(double delta)
            {
                foreach (int x in feromon.Keys.ToList())
                    foreach (int y in feromon[x].Keys.ToList())
                        feromon[x][y] *= Math.Exp(-delta / 10);
            }


            public int GetDirection(int x0, int y0)
            {
                double total = 0;
                for(int d = 0; d < 4; d++)
                {
                    Direction dir = (Direction)d;
                    int x = x0;
                    int y = y0;
                    if (dir == Direction.Left)
                        x--;
                    if (dir == Direction.Right)
                        x++;
                    if (dir == Direction.Up)
                        y--;
                    if (dir == Direction.Down)
                        y++;
                    total += feromon[x][y];
                }
                double f = 0.05;
                if (total == 0)
                    return rnd.Next(4);
                double result = rnd.NextDouble() * (1 + 4 * f);
                double res = 0;
                for (int d = 0; d < 4; d++)
                {
                    Direction dir = (Direction)d;
                    int x = x0;
                    int y = y0;
                    if (dir == Direction.Left)
                        x--;
                    if (dir == Direction.Right)
                        x++;
                    if (dir == Direction.Up)
                        y--;
                    if (dir == Direction.Down)
                        y++;
                    res += f + feromon[x][y]/total;
                    if (res >= result)
                        return d;
                }
                throw new Exception("Wrong Path");
            }

        }

        public void RunFeromon(TimeSpan maxModelTime)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            void Shuffle<T>(IList<T> list)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rnd.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }



            
            Feromon feromon = null;
            double prev = 0;
            while (skladWrapper.Next())
            {
                

                if (!skladWrapper.isEventCountEmpty())
                    continue;


                if (skladWrapper.updatedTime > maxModelTime)
                    break;

                if (feromon is null)
                    feromon = new Feromon(skladWrapper.GetSklad().squaresIsBusy.getEmptyDesk());

                double delta = skladWrapper.updatedTime.TotalSeconds - prev;
                if (delta > 1)
                {
                    feromon.Update(delta);
                    prev = skladWrapper.updatedTime.TotalSeconds;
                }
                if (skladWrapper.GetAllAnts().Any(x => x.state == AntBotState.UnCharged))
                {
                    Console.WriteLine("UNCHARGED");
                    break;
                }


                var freeAnts = skladWrapper.GetFreeUnloadedAnts();
                var sb = skladWrapper.GetSklad().squaresIsBusy;
                Shuffle(freeAnts);
                freeAnts.ForEach(freeAnt =>
                {
                    int x = freeAnt.xCord;
                    int y = freeAnt.yCord;
                    int d = feromon.GetDirection(x, y);
                    Direction dir = (Direction)d;
                    int dist = skladWrapper.getFreePath(freeAnt, dir, freeAnt.lastUpdated);
                    if (dist != 0)
                    {
                        if (dir == Direction.Left)
                            x--;
                        if (dir == Direction.Right)
                            x++;
                        if (dir == Direction.Up)
                            y--;
                        if (dir == Direction.Down)
                            y++;

                        if (freeAnt.sklad.squaresIsBusy.GetPosibleReserve(x, y, freeAnt.lastUpdated) == TimeSpan.MaxValue)
                        {
                            skladWrapper.Move(freeAnt, dir, 1, 0, false);
                            feromon.Activate(x, y);
                        }                   
                        else
                        {
                            freeAnt.CleanReservation();
                            freeAnt.commandList.AddCommand(new AntBotWait(freeAnt, TimeSpan.FromSeconds(1)));
                            freeAnt.commandList.AddCommand(new AntBotStop(freeAnt));
                        }
                    }
                    else
                    {
                        freeAnt.CleanReservation();
                        freeAnt.commandList.AddCommand(new AntBotWait(freeAnt, TimeSpan.FromSeconds(1)));
                        freeAnt.commandList.AddCommand(new AntBotStop(freeAnt));
                    }
                });


            }
        }

    }
}
