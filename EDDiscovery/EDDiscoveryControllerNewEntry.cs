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
            OnNewJournalEntry?.Invoke(je);          // Always call this on all entries...

            // filter out commanders, and filter out any UI events
            if (je.CommanderId == history.CommanderId)
            {
                HistoryEntry he = history.AddJournalEntry(je, h => LogLineHighlight(h));        // add a new one on top
                //System.Diagnostics.Debug.WriteLine("Add HE " + he.EventSummary);
                OnNewEntry?.Invoke(he, history);            // major hook
                OnNewEntrySecond?.Invoke(he, history);      // secondary hook..
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

            var uifuel = u as EliteDangerousCore.UIEvents.UIFuel;       // UI Fuel has information on fuel level - update it.
            if (uifuel != null && history != null)
            {
                history.shipinformationlist.UIFuel(uifuel);             // update the SI global value
                history.GetLast?.UpdateShipInformation(history.shipinformationlist.CurrentShip);    // and make the last entry have this updated info.
            }

            OnNewUIEvent?.Invoke(u);
        }
    }
}
