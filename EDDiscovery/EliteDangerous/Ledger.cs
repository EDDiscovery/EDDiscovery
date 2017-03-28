/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using EDDiscovery.DB;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public class Ledger
    {
        public class Transaction
        {
            public long jid;
            public DateTime utctime;                               // when it was done.
            public JournalTypeEnum jtype;                          // what caused it..
            public string notes;                                   // notes about the entry
            public long cashadjust;                                // any profit, 0 if none (negative for cost, positive for profit)
            public double profitperunit;                             // profit per unit
            public long cash;                                      // cash total at this point

            public bool IsJournalEventInEventFilter(string[] events)
            {
                return events.Contains(jtype.ToString().SplitCapsWord());
            }
        }

        private List<Transaction> transactions;
        public long CashTotal = 0;

        public Ledger()
        {
            transactions = new List<Transaction>();
        }

        public List<Transaction> Transactions { get { return transactions; } }

        public void AddEvent(long jidn, DateTime t, JournalTypeEnum j, string n, long? ca, double ppu = 0)
        {
            AddEventCash(jidn, t, j, n, ca.HasValue ? ca.Value : 0, ppu);
        }

        public void AddEventNoCash(long jidn, DateTime t, JournalTypeEnum j, string n)
        {
            AddEventCash(jidn, t, j, n, 0, 0);
        }

        public void AddEventCash(long jidn, DateTime t, JournalTypeEnum j, string n, long ca, double ppu = 0)
        {
            long newcashtotal = CashTotal + ca;
            //System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} = {4}", j.ToString(), n, CashTotal, ca , newcashtotal);
            CashTotal = newcashtotal;

            Transaction tr = new Transaction
            {
                jid = jidn,
                utctime = t,
                jtype = j,
                notes = n,
                cashadjust = ca,
                cash = CashTotal,
                profitperunit = ppu
            };

            transactions.Add(tr);
        }


        public void Process(JournalEntry je, SQLiteConnectionUser conn)
        {
            if (je is ILedgerJournalEntry)
            {
                ((ILedgerJournalEntry)je).Ledger(this, conn);
            }
            else if (je is ILedgerNoCashJournalEntry)
            {
                ((ILedgerNoCashJournalEntry)je).LedgerNC(this, conn);
            }
        }

    }

}
