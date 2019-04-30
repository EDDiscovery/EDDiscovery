/*
 * Copyright © 2016 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace EliteDangerousCore
{
    public class StarDistanceComputer
    {
        private Thread backgroundStardistWorker;
        private bool PendingClose { get; set; }           // we want to close boys!

        private class StardistRequest
        {
            public ISystem System;
            public bool IgnoreOnDuplicate;      // don't compute until last one is present
            public double MinDistance;
            public double MaxDistance;
            public bool Spherical;
            public int MaxItems;
            public Action<ISystem, BaseUtils.SortedListDoubleDuplicate<ISystem>> Callback;
        }

        private ConcurrentQueue<StardistRequest> closestsystem_queue = new ConcurrentQueue<StardistRequest>();

        private AutoResetEvent stardistRequested = new AutoResetEvent(false);
        private AutoResetEvent closeRequested = new AutoResetEvent(false);

        public StarDistanceComputer()
        {
            PendingClose = false;
            backgroundStardistWorker = new Thread(BackgroundStardistWorkerThread) { Name = "Star Distance Worker", IsBackground = true };
            backgroundStardistWorker.Start();
        }

        public void CalculateClosestSystems(ISystem sys, Action<ISystem, BaseUtils.SortedListDoubleDuplicate<ISystem>> callback,
                        int maxitems, double mindistance, double maxdistance, bool spherical, bool ignoreDuplicates = true)
        {
            closestsystem_queue.Enqueue(new StardistRequest
            {
                System = sys,
                Callback = callback,
                MaxItems = maxitems,
                MinDistance = mindistance,
                MaxDistance = maxdistance,
                Spherical = spherical,
                IgnoreOnDuplicate = ignoreDuplicates
            });
            stardistRequested.Set();
        }

        public void ShutDown()
        {
            PendingClose = true;
            closeRequested.Set();
            backgroundStardistWorker.Join();
        }

        private void BackgroundStardistWorkerThread()
        {
            while (!PendingClose)
            {
                int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, stardistRequested });

                if (PendingClose)
                    break;

                StardistRequest stardistreq = null;

                switch (wh)
                {
                    case 0:  // Close Requested
                        break;
                    case 1:  // Star Distances Requested
                        while (!PendingClose && closestsystem_queue.TryDequeue(out stardistreq))
                        {
                            if (!stardistreq.IgnoreOnDuplicate || closestsystem_queue.Count == 0)
                            {
                                StardistRequest req = stardistreq;
                                ISystem sys = req.System;
                                BaseUtils.SortedListDoubleDuplicate<ISystem> closestsystemlist = new BaseUtils.SortedListDoubleDuplicate<ISystem>(); //lovely list allowing duplicate keys - can only iterate in it.

                                //System.Diagnostics.Debug.WriteLine("DB Computer Max distance " + req.MaxDistance);

                                DB.SystemCache.GetSystemListBySqDistancesFrom(closestsystemlist, sys.X, sys.Y, sys.Z, req.MaxItems , 
                                              req.MinDistance, req.MaxDistance , req.Spherical);

                                if (!PendingClose)
                                {
                                    req.Callback(sys, closestsystemlist);
                                }
                            }
                        }

                        break;
                }
            }
        }
    }
}
 
