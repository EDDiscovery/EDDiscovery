/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using EliteDangerousCore;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        private Queue<JournalEntry> journalqueue = new Queue<JournalEntry>();
        private System.Threading.Timer journalqueuedelaytimer;

        public void NewEntry(JournalEntry je)        // on UI thread. hooked into journal monitor and receives new entries.. Also call if you programatically add an entry
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            int playdelay = HistoryList.MergeTypeDelay(je); // see if there is a delay needed..

            if (playdelay > 0)  // if delaying to see if a companion event occurs. add it to list. Set timer so we pick it up
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Delay Play queue " + je.EventTypeID + " Delay for " + playdelay);
                journalqueue.Enqueue(je);
                journalqueuedelaytimer.Change(playdelay, Timeout.Infinite);
            }
            else
            {
                journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);  // stop the timer, but if it occurs before this, not the end of the world
                journalqueue.Enqueue(je);  // add it to the play list.
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " No delay, issue " + je.EventTypeID );
                PlayJournalList();    // and play
            }
        }

        public void PlayJournalList()                 // UI Threead play delay list out..
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Play out list");

            JournalEntry prev = null;  // we start afresh from the point of merging so we don't merge with previous ones already shown

            while (journalqueue.Count > 0)
            {
                JournalEntry je = journalqueue.Dequeue();

                if (!HistoryList.MergeEntries(prev, je))                // if not merged
                {
                    if (prev != null)                       // no merge, so if we have a merge candidate on top, run actions on it.
                        ActionEntry(prev);

                    prev = je;                              // record
                }
            }

            if (prev != null)                               // any left.. action it
                ActionEntry(prev);
        }

        private void ActionEntry(JournalEntry je)               // UI thread issue the JE to the system
        {
            System.Diagnostics.Trace.WriteLine(string.Format(Environment.NewLine + "New JEntry {0} {1}", je.EventTimeUTC, je.EventTypeStr));

            OnNewJournalEntry?.Invoke(je);          // Always call this on all entries...

            // filter out commanders, and filter out any UI events
            if (je.CommanderId == history.CommanderId)
            {
                BaseUtils.AppTicks.TickCountLapDelta("CTNE", true);

                HistoryEntry he = history.AddJournalEntryToHistory(je, h => LogLineHighlight(h));        // add a new one on top

                var t1 = BaseUtils.AppTicks.TickCountLapDelta("CTNE");
                if (t1.Item2 >= 20)
                    System.Diagnostics.Trace.WriteLine(" NE Add Journal slow " + t1.Item1);

                if (he != null)     // may reject it 
                {
                    if ( OnNewEntry != null)
                    {
                        foreach (var e in OnNewEntry.GetInvocationList())       // do the invokation manually, so we can time each method
                        {
                            Stopwatch sw = new Stopwatch(); sw.Start();
                            e.DynamicInvoke(he, history);
                            if ( sw.ElapsedMilliseconds >= 20)
                                System.Diagnostics.Trace.WriteLine(" NE Add Method " + e.Method.DeclaringType + " took " + sw.ElapsedMilliseconds);
                        }
                    }

                    var t2 = BaseUtils.AppTicks.TickCountLapDelta("CTNE");
                    if (t2.Item2 >= 40)
                        System.Diagnostics.Trace.WriteLine(" NE First Slow " + t2.Item1);

                    OnNewEntrySecond?.Invoke(he, history);      // secondary hook..


                    // finally, CAPI, if docked, try and get commodity data, and if so, create a new EDD record.  Do not do this for console commanders

                    if (he.EntryType == JournalTypeEnum.Docked)
                    {
                        if (FrontierCAPI.Active && !EDCommander.Current.ConsoleCommander)
                        {
                            // don't hold up the main thread, do it in a task, as its a HTTP operation
                            // and wait for the CAPI to recover by delaying for 15 s

                            System.Threading.Tasks.Task.Delay(15000).ContinueWith((task) =>           
                            {
                                var dockevt = he.journalEntry as EliteDangerousCore.JournalEvents.JournalDocked;

                                for (int tries = 0; tries < 3; tries++)
                                {
                                    FrontierCAPI.GameIsBeta = he.IsBetaMessage;
                                    string marketjson = FrontierCAPI.Market();

                                    CAPI.Market mk = new CAPI.Market(marketjson);
                                    if (mk.IsValid)
                                    {
                                        //System.IO.File.WriteAllText(@"c:\code\market.json", marketjson);

                                        if (dockevt.StationName.Equals(mk.Name, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            System.Diagnostics.Trace.WriteLine($"CAPI got market {mk.Name}");

                                            var entry = new EliteDangerousCore.JournalEvents.JournalEDDCommodityPrices(he.EventTimeUTC.AddSeconds(1),
                                                            mk.ID, mk.Name, he.System.Name, EDCommander.CurrentCmdrID, mk.Commodities);

                                            var jo = entry.ToJSON();        // get json of it, and add it to the db
                                            entry.Add(jo);

                                            InvokeAsyncOnUiThread(() =>
                                            {
                                                Debug.Assert(System.Windows.Forms.Application.MessageLoop);
                                                System.Diagnostics.Debug.WriteLine("CAPI fire new entry");
                                                NewEntry(entry);                // then push it thru. this will cause another set of calls to NewEntry First/Second
                                                                                // EDDN handler will pick up EDDCommodityPrices and send it.
                                            });

                                            break;
                                        }
                                        else
                                        {
                                            LogLine("CAPI received incorrect information, retrying");
                                            System.Diagnostics.Trace.WriteLine($"CAPI disagree on market {dockevt.StationName} vs {mk.Name}");
                                        }
                                    }
                                    else
                                    {
                                        LogLine("CAPI market data invalid, retrying");
                                        System.Diagnostics.Trace.WriteLine($"CAPI market invalid {marketjson}");
                                    }

                                    Thread.Sleep(10000);
                                }
                            });
                        }
                    }


                    var t3 = BaseUtils.AppTicks.TickCountLapDelta("CTNE");
                    System.Diagnostics.Trace.WriteLine("NE END " + t3.Item1 + " " + (t3.Item3 > 99 ? "!!!!!!!!!!!!!" : ""));
                }
            }

            if (je.EventTypeID == JournalTypeEnum.LoadGame) // and issue this on Load game
            {
                OnRefreshCommanders?.Invoke();
            }
        }

        public void DelayPlay(Object s)             // timer thread timeout after play delay.. 
        {
            System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Delay Play timer executed");
            journalqueuedelaytimer.Change(Timeout.Infinite, Timeout.Infinite);
            InvokeAsyncOnUiThread(() =>
            {
                PlayJournalList();
            });
        }

        void NewUIEvent(UIEvent u)                  // UI thread new event
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine("Dispatch from controller UI event " + u.EventTypeStr);

            BaseUtils.AppTicks.TickCountLapDelta("CTUI", true);

            var uifuel = u as EliteDangerousCore.UIEvents.UIFuel;       // UI Fuel has information on fuel level - update it.
            if (uifuel != null && history != null)
            {
                history.ShipInformationList.UIFuel(uifuel);             // update the SI global value
                history.GetLast?.UpdateShipInformation(history.ShipInformationList.CurrentShip);    // and make the last entry have this updated info.
            }

            OnNewUIEvent?.Invoke(u);

            var t = BaseUtils.AppTicks.TickCountLapDelta("CTUI");
            if ( t.Item2 > 25 )
                System.Diagnostics.Debug.WriteLine( t.Item1 + " Controller UI !!!");
        }
    }
}
