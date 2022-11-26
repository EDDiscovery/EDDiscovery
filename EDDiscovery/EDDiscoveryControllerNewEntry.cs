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
using System.Linq;
using System.Threading;
using EliteDangerousCore;

namespace EDDiscovery
{
    public partial class EDDiscoveryController
    {
        private Queue<JournalEntry> journalqueue = new Queue<JournalEntry>();
        private System.Threading.Timer journalqueuedelaytimer;

        // on UI thread. hooked into journal monitor and receives new entries.. Also call if you programatically add an entry
        // sr may be null if programatically made, not read from logs. Only a few events are made this way, check the references.
        public void NewJournalEntryFromScanner(JournalEntry je, StatusReader sr)        
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (je.EventTypeID == JournalTypeEnum.CodexEntry)
            {
                System.Diagnostics.Debug.Assert(sr != null, "Made codex programatically - wrong");
                var jce = je as EliteDangerousCore.JournalEvents.JournalCodexEntry;
                jce.EDDBodyName = sr.BodyName;     // copy in the SR Body name to the entry - this is a feed thru of status to the entry
                System.Diagnostics.Debug.WriteLine($"Journal Codex set body name to {jce.EDDBodyName} due to status record");
            }

            int playdelay = HistoryList.MergeTypeDelayForJournalEntries(je); // see if there is a delay needed..

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
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " No delay, issue " + je.EventTypeID);
                PlayJournalList();    // and play
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

        public void PlayJournalList()                 // UI Thread play delay list out..
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Play out list of {journalqueue.Count}");

            while (journalqueue.Count > 0)
            {
                var current = journalqueue.Dequeue();

                System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} New JEntry {current.EventTimeUTC} {current.EventTypeStr}");

                BaseUtils.AppTicks.TickCountLapDelta("CTNE", true);

                if (current.CommanderId != history.CommanderId)         // remove non relevant jes
                    continue;

                OnNewJournalEntryUnfiltered?.Invoke(current);         // Called before any removal or merging, so this is the raw journal list

                HistoryEntry historyentry = history.MakeHistoryEntry(current);
                OnNewHistoryEntryUnfiltered?.Invoke(historyentry);

                while (journalqueue.Count > 0)                      // go thru the list and find merge candidates
                {
                    var peek = journalqueue.Peek();

                    if (peek.CommanderId != history.CommanderId)     // remove non relevant jes
                    {
                        journalqueue.Dequeue();                     // remove it
                    }
                    else if (HistoryList.MergeJournalEntries(current, peek))  // if the peeked is merged into current
                    {
                        OnNewJournalEntryUnfiltered?.Invoke(peek);         // send the peeked, unmodified
                        OnNewHistoryEntryUnfiltered?.Invoke(history.MakeHistoryEntry(peek));
                        journalqueue.Dequeue();                     // remove it
                        System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} ..merged {peek.EventTimeUTC} {peek.EventTypeStr}");
                    }
                    else
                        break;                                      // not mergable and since we peeked not removed
                }

                var historyentries = history.AddHistoryEntryToListWithReorder(historyentry, h => LogLineHighlight(h));   // add a new one on top of the HL, reorder, remove, return a list of ones to process

                foreach (var he in historyentries.EmptyIfNull())
                {
                    System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} ** Process {he.EventTimeUTC} {he.EntryType}");

                    he.journalEntry.SetSystemNote();                // since we are displaying it, we can check here to see if a system note needs assigning

                    if (he.EntryType == JournalTypeEnum.CodexEntry)     // need to do some work on codex entry.. set bodyid as long as recorded body name matches tracking name, and update DB
                    {
                        var jce = he.journalEntry as EliteDangerousCore.JournalEvents.JournalCodexEntry;
                        if (jce.EDDBodyName == he.Status.BodyName)        // following EDDN advice, use status body name as master key
                        {
                            jce.EDDBodyId = he.Status.BodyID ?? -1;
                            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Journal Codex set body ID to {jce.EDDBodyId} as ID");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"{Environment.TickCount} Journal Codex WARNING name does not match {he.Status.BodyName} vs {jce.EDDBodyName} {jce.EDDBodyId}");
                        }
                        jce.UpdateDB();                     // write back
                    }

                    if (OnNewEntry != null)    // issue to OnNewEntry handlers
                    {
                        foreach (var e in OnNewEntry.GetInvocationList())       // do the invokation manually, so we can time each method
                        {
                            Stopwatch sw = new Stopwatch(); sw.Start();
                            e.DynamicInvoke(he, history);
                            if (sw.ElapsedMilliseconds >= 80)
                                System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} OnNewEntry Add Method {e.Method.DeclaringType} took {sw.ElapsedMilliseconds}");
                        }
                    }

                    var t2 = BaseUtils.AppTicks.TickCountLapDelta("CTNE");
                    if (t2.Item2 >= 80)
                        System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} OnNewEntry slow {t2.Item1}");

                    OnNewEntrySecond?.Invoke(he, history);      // secondary hook..

                    // finally, CAPI, if docked, and CAPI is go for pc commander, do capi procedure

                    if (he.EntryType == JournalTypeEnum.Docked && FrontierCAPI.Active && !EDCommander.Current.ConsoleCommander)
                    {
                        var dockevt = he.journalEntry as EliteDangerousCore.JournalEvents.JournalDocked;
                        DoCAPI(dockevt.StationName, he.System.Name, he.journalEntry.IsBeta, history.Shipyards.AllowCobraMkIV);
                    }

                    var t3 = BaseUtils.AppTicks.TickCountLapDelta("CTNE");
                    System.Diagnostics.Trace.WriteLine($"{Environment.TickCount} ** EndProcess {he.EventTimeUTC} {he.EntryType} {t3.Item1} {(t3.Item3 > 99 ? "!!!!!!!!!!!!!" : "")}");
                }

                if (historyentry.EntryType == JournalTypeEnum.LoadGame) // and issue this on Load game
                {
                    OnRefreshCommanders?.Invoke();
                }
            }
        }

        // New UI event. SR will be null if programatically made

        void NewUIEventFromScanner(UIEvent u, StatusReader sr)                  // UI thread new event
        {
            Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            //System.Diagnostics.Debug.WriteLine("Dispatch from controller UI event " + u.EventTypeStr);

            BaseUtils.AppTicks.TickCountLapDelta("CTUI", true);

            var uifuel = u as EliteDangerousCore.UIEvents.UIFuel;       // UI Fuel has information on fuel level - update it.
            if (uifuel != null && history != null)
            {
                history.ShipInformationList.UIFuel(uifuel);             // update the SI global value
                if (history.ShipInformationList.CurrentShip != null )   // just to be paranoid
                    history.GetLast?.UpdateShipInformation(history.ShipInformationList.CurrentShip);    // and make the last entry have this updated info.
            }

            OnNewUIEvent?.Invoke(u);

            var t = BaseUtils.AppTicks.TickCountLapDelta("CTUI");
            if ( t.Item2 > 25 )
                System.Diagnostics.Debug.WriteLine( t.Item1 + " Controller UI !!!");
        }

        public void DoCAPI(string station, string system, bool beta , bool? allowcobramkiv)
        {
            // don't hold up the main thread, do it in a task, as its a HTTP operation

            System.Threading.Tasks.Task.Run(() =>
            {
                bool donemarket = false, doneshipyard = false;

                for (int tries = 3; tries >= 1 && (donemarket == false || doneshipyard == false); tries--)
                {
                    Thread.Sleep(10000);        // for the first go, give the CAPI servers a chance to update, for the next goes, spread out the requests

                    FrontierCAPI.GameIsBeta = beta;

                    if (!donemarket)
                    {
                        string marketjson = FrontierCAPI.Market();

                        if ( marketjson != null )
                        {
                            //System.IO.File.WriteAllText(@"c:\code\market.json", marketjson);

                            CAPI.Market mk = new CAPI.Market(marketjson);
                            if (mk.IsValid && station.Equals(mk.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                System.Diagnostics.Trace.WriteLine($"CAPI got market {mk.Name}");

                                var entry = new EliteDangerousCore.JournalEvents.JournalEDDCommodityPrices(DateTime.UtcNow,
                                                mk.ID, mk.Name, system, EDCommander.CurrentCmdrID, mk.Commodities);

                                var jo = entry.ToJSON();        // get json of it, and add it to the db
                                entry.Add(jo);

                                InvokeAsyncOnUiThread(() =>
                                {
                                    Debug.Assert(System.Windows.Forms.Application.MessageLoop);
                                    NewJournalEntryFromScanner(entry,null);                // then push it thru. this will cause another set of calls to NewEntry First/Second
                                                                    // EDDN handler will pick up EDDCommodityPrices and send it.
                                });

                                donemarket = true;
                                Thread.Sleep(500);      // space the next check out a bit
                            }
                        }
                    }

                    if (!donemarket)
                    {
                        LogLine("CAPI failed to get market data" + (tries > 1 ? ", retrying" : ", give up"));
                    }

                    if (!doneshipyard)
                    {
                        string shipyardjson = FrontierCAPI.Shipyard();

                        if (shipyardjson != null)
                        {
                            CAPI.Shipyard sh = new CAPI.Shipyard(shipyardjson);
                            
                            //System.IO.File.WriteAllText(@"c:\code\shipyard.json", shipyardjson);

                            if (sh.IsValid && station.Equals(sh.Name, StringComparison.InvariantCultureIgnoreCase))
                            {
                                System.Diagnostics.Trace.WriteLine($"CAPI got shipyard {sh.Name}");

                                var modules = sh.GetModules();
                                if ( modules?.Count > 0 )
                                {
                                    var list = modules.Select(x => new Tuple<long, string, long>(x.ID, x.Name.ToLowerInvariant(), x.Cost)).ToArray();
                                    var outfitting = new EliteDangerousCore.JournalEvents.JournalOutfitting(DateTime.UtcNow, station, system, sh.ID, list, EDCommander.CurrentCmdrID);

                                    var jo = outfitting.ToJSON();        // get json of it, and add it to the db
                                    outfitting.Add(jo);

                                    InvokeAsyncOnUiThread(() =>
                                    {
                                        NewJournalEntryFromScanner(outfitting,null);                // then push it thru. this will cause another set of calls to NewEntry First/Second, then EDDN will send it
                                    });
                                }

                                var shipyard = sh.GetShips();

                                if ( shipyard?.Count > 0 && allowcobramkiv.HasValue)
                                {
                                    var list = shipyard.Select(x => new Tuple<long, string, long>(x.ID, x.Name.ToLowerInvariant(), x.BaseValue)).ToArray();
                                    var shipyardevent = new EliteDangerousCore.JournalEvents.JournalShipyard(DateTime.UtcNow, station, system, sh.ID, list, EDCommander.CurrentCmdrID, allowcobramkiv.Value);

                                    var jo = shipyardevent.ToJSON();        // get json of it, and add it to the db
                                    shipyardevent.Add(jo);

                                    InvokeAsyncOnUiThread(() =>
                                    {
                                        NewJournalEntryFromScanner(shipyardevent,null);                // then push it thru. this will cause another set of calls to NewEntry First/Second, then EDDN will send it
                                    });
                                }

                                doneshipyard = true;
                            }
                        }
                    }

                    if (!doneshipyard)
                    {
                        LogLine("CAPI failed to get shipyard data" + (tries > 1 ? ", retrying" : ", give up"));
                    }
                }

            });

        }


    }
}
