/*
 * Copyright © 2016-2018 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Powerplay)]
    public class JournalPowerplay : JournalEntry
    {
        public JournalPowerplay(JObject evt) : base(evt, JournalTypeEnum.Powerplay)
        {
            Power = evt["Power"].Str();
            Rank = evt["Rank"].Int();
            Merits = evt["Merits"].Int();
            Votes = evt["Votes"].Int();
            TimePledged = evt["TimePledged"].Long();
            TimePledgedSpan = new TimeSpan((int)(TimePledged/60/60),(int)((TimePledged/60)%60),(int)(TimePledged%60));
            TimePledgedString = TimePledgedSpan.ToString();
        }

        public string Power { get; set; }
        public int Rank { get; set; }
        public int Merits { get; set; }
        public int Votes { get; set; }
        public long TimePledged { get; set; }
        public TimeSpan TimePledgedSpan { get; set; }
        public string TimePledgedString { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "Rank:".Txb(this), Rank, "Merits:".Txb(this), Merits, "Votes:".Txb(this), Votes, "Pledged:".Txb(this), TimePledgedString);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayCollect)]
    public class JournalPowerplayCollect : JournalEntry
    {
        public JournalPowerplayCollect(JObject evt) : base(evt, JournalTypeEnum.PowerplayCollect)
        {
            Power = evt["Power"].Str();
            Type = evt["Type"].Str();
            Type_Localised = JournalFieldNaming.CheckLocalisation(evt["Type_Localised"].Str(), Type);
            Count = evt["Count"].Int();

        }
        public string Power { get; set; }
        public string Type { get; set; }
        public string Type_Localised { get; set; }
        public int Count { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "Type:".Txb(this), Type_Localised, "Count:".Txb(this), Count);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayDefect)]
    public class JournalPowerplayDefect : JournalEntry
    {
        public JournalPowerplayDefect(JObject evt) : base(evt, JournalTypeEnum.PowerplayDefect)
        {
            FromPower = evt["FromPower"].Str();
            ToPower = evt["ToPower"].Str();
        }

        public string FromPower { get; set; }
        public string ToPower { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("From Power:".Txb(this), FromPower, "To Power:".Txb(this), ToPower);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayDeliver)]
    public class JournalPowerplayDeliver : JournalEntry
    {
        public JournalPowerplayDeliver(JObject evt) : base(evt, JournalTypeEnum.PowerplayDeliver)
        {
            Power = evt["Power"].Str();
            Type = evt["Type"].Str();
            Type_Localised = JournalFieldNaming.CheckLocalisation(evt["Type_Localised"].Str(), Type);
            Count = evt["Count"].Int();
        }

        public string Power { get; set; }
        public string Type { get; set; }
        public string Type_Localised { get; set; }
        public int Count { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "Type:".Txb(this), Type_Localised, "Count:".Txb(this), Count);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayFastTrack)]
    public class JournalPowerplayFastTrack : JournalEntry, ILedgerJournalEntry
    {
        public JournalPowerplayFastTrack(JObject evt) : base(evt, JournalTypeEnum.PowerplayFastTrack)
        {
            Power = evt["Power"].Str();
            Cost = evt["Cost"].Long();
        }

        public string Power { get; set; }
        public long Cost { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Power, -Cost);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "Cost:; cr;N0".Txb(this), Cost);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayJoin)]
    public class JournalPowerplayJoin : JournalEntry
    {
        public JournalPowerplayJoin(JObject evt) : base(evt, JournalTypeEnum.PowerplayJoin)
        {
            Power = evt["Power"].Str();
        }

        public string Power { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Power;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayLeave)]
    public class JournalPowerplayLeave : JournalEntry
    {
        public JournalPowerplayLeave(JObject evt) : base(evt, JournalTypeEnum.PowerplayLeave)
        {
            Power = evt["Power"].Str();
        }

        public string Power { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Power;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplaySalary)]
    public class JournalPowerplaySalary : JournalEntry, ILedgerJournalEntry
    {
        public JournalPowerplaySalary(JObject evt) : base(evt, JournalTypeEnum.PowerplaySalary)
        {
            Power = evt["Power"].Str();
            Amount = evt["Amount"].Long();
        }

        public string Power { get; set; }
        public long Amount { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Power, Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "Amount:; cr;N0".Txb(this), Amount);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayVote)]
    public class JournalPowerplayVote : JournalEntry
    {
        public JournalPowerplayVote(JObject evt) : base(evt, JournalTypeEnum.PowerplayVote)
        {
            Power = evt["Power"].Str();
            System = evt["System"].Str();
            Votes = evt["Votes"].Int();
        }

        public string Power { get; set; }
        public string System { get; set; }
        public int Votes { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Power, "System:".Txb(this), System, "Votes:".Txb(this), Votes);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PowerplayVoucher)]
    public class JournalPowerplayVoucher : JournalEntry
    {
        public JournalPowerplayVoucher(JObject evt) : base(evt, JournalTypeEnum.PowerplayVoucher)
        {
            Power = evt["Power"].Str();
            Systems = evt["Systems"]?.ToObjectProtected<string[]>();
        }

        public string Power { get; set; }
        public string[] Systems { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Power;

            if (Systems != null)
            {
                info += ", Systems:".Txb(this);

                bool comma = false;
                foreach (string s in Systems)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += s;
                }
            }

            detailed = "";
        }
    }


}
