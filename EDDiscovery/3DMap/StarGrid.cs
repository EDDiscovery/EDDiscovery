using EDDiscovery.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class StarGrid
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Z { get; set; }
        public double Percentage { get; set; }          // foreground flags
        public bool Computed { get; set; }      // foreground flags
        public bool Working { get; set; }       // foreground flags

        public StarGrid(int id, double x, double z)
        {
            Id = id; X = x; Z = z;
            Percentage = 0;
            Computed = false;
            Working = false;
        }

        public double DistanceFrom(double x, double z)
        { return Math.Sqrt((x - X) * (x - X) + (z - Z) * (z - Z)); }
    }

    public class StarGrids
    {
        #region Vars
        private BlockingCollection<StarGrid> tocompute = new BlockingCollection<StarGrid>();
        private BlockingCollection<StarGrid> computed = new BlockingCollection<StarGrid>();
        private List<StarGrid> grids = new List<StarGrid>();
        private System.Threading.Thread computeThread;
        private bool computeExit = false;

        #endregion

        #region Initialise

        public void Initialise()
        {
            for (int z = 0; z < GridId.gridzrange; z++)
            {
                for (int x = 0; x < GridId.gridxrange; x++)
                {
                    int id = GridId.IdFromComponents(x, z);

                    double xp = 0, zp = 0;
                    bool ok = GridId.XZ(id, out xp, out zp);
                    Debug.Assert(ok);
                    grids.Add(new StarGrid(id, xp, zp));
                }
            }

            Console.WriteLine("Total grids " + grids.Count);

            computeThread = new System.Threading.Thread(ComputeThread) { Name = "Fill stars", IsBackground = true };
            computeThread.Start();
        }

        public void ShutDown()
        {
            if (computeThread.IsAlive)
            {
                computeExit = true;
                tocompute.Add(null);                                 // add to the compute list.. null is a marker saying shut down
                computeThread.Join();
            }
        }

        #endregion

        #region Update

        public void Update(double xp, double zp)            // Foreground UI thread
        {
            {
                StarGrid grd = null;
                while (computed.TryTake(out grd))               // remove from the computed queue and mark done
                {
                    grd.Working = false;
                    grd.Computed = true;
                }
            }

            SortedList<double, StarGrid> toupdate = new SortedList<double, StarGrid>(new DuplicateKeyComparer<double>());     // we sort in distance order

            foreach (StarGrid gcheck in grids)
            {
                if (gcheck.Working == false)                 // if not in the tocompute queue..
                {
                    double dist = gcheck.DistanceFrom(xp, zp);
                    double percentage = GetPercentage(dist);

                    if (gcheck.Computed == false || gcheck.Percentage != percentage) // if not computed, or percentage changed.
                    {
                        gcheck.Percentage = percentage;
                        toupdate.Add(dist, gcheck);                             // add to sorted list
                    }
                }
            }

            foreach (StarGrid gsubmit in toupdate.Values)              // and we submit to the queue in distance order, so we build outwards
            {
                gsubmit.Working = true;                                 // working..
                gsubmit.Computed = false;                               // not computed
                tocompute.Add(gsubmit);                                 // add to the compute list..
                Console.WriteLine("Compute " + gsubmit.Id + " at " + gsubmit.X + " , " + gsubmit.Z);
            }
        }

        public double GetPercentage(double dist)
        {
            return 100;
        }

        #endregion

        #region Compute

        void ComputeThread()
        {
            while (true)
            {
                StarGrid grd = tocompute.Take();

                if (grd == null || computeExit == true)
                    break;

                System.Threading.Thread.Sleep(50);
                Console.WriteLine("Computed " + grd.Id);
                computed.Add(grd);
            }
        }

        #endregion

        #region misc

        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);
                return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
            }
        }

        #endregion
    }


}
