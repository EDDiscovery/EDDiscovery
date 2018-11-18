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
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Bounty)]
    public class JournalBounty : JournalEntry, ILedgerNoCashJournalEntry
    {
        public class BountyReward
        {
            public string Faction;
            public long Reward;
        }

        public JournalBounty(JObject evt) : base(evt, JournalTypeEnum.Bounty)
        {
            TotalReward = evt["TotalReward"].Long();     // others of them..

            VictimFaction = evt["VictimFaction"].Str();
            VictimFactionLocalised = JournalFieldNaming.CheckLocalisation(evt["VictimFaction_Localised"].Str(),VictimFaction);

            SharedWithOthers = evt["SharedWithOthers"].Bool(false);
            Rewards = evt["Rewards"]?.ToObjectProtected<BountyReward[]>();

            Target = evt["Target"].StrNull();       // only set for skimmer target missions

            if ( Rewards == null )                  // for skimmers, its Faction/Reward.  Bug in manual reported to FD 23/5/2018
            {
                string faction = evt["Faction"].StrNull();
                long? reward = evt["Reward"].IntNull();

                if (faction != null && reward != null)
                {
                    Rewards = new BountyReward[1];
                    Rewards[0] = new BountyReward() { Faction = faction, Reward = reward.Value };
                    TotalReward = reward.Value;
                }
            }
        }

        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public string Target { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} total {1:N0}".Txb(this,"LegBounty"), VictimFactionLocalised, TotalReward));
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("; cr;N0", TotalReward, "Target:".Txb(this), (string)Target, "Victim faction:".Txb(this), VictimFactionLocalised);

            detailed = "";
            if ( Rewards!=null)
            {
                foreach (BountyReward r in Rewards)
                {
                    if (detailed.Length > 0)
                        detailed += ", ";

                    detailed += BaseUtils.FieldBuilder.Build("Faction:".Txb(this), r.Faction, "; cr;N0", r.Reward);
                }
            }
        }
    }

    [JournalEntryType(JournalTypeEnum.CapShipBond)]
    public class JournalCapShipBond : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCapShipBond(JObject evt) : base(evt, JournalTypeEnum.CapShipBond)
        {
            AwardingFaction = evt["AwardingFaction"].Str();
            AwardingFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["AwardingFaction_Localised"].Str(), AwardingFaction);
            VictimFaction = evt["VictimFaction"].Str();
            VictimFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["VictimFaction_Localised"].Str(), VictimFaction);
            Reward = evt["Reward"].Long();
        }

        public string AwardingFaction { get; set; }
        public string AwardingFaction_Localised { get; set; }       // may be empty
        public string VictimFaction { get; set; }
        public string VictimFaction_Localised { get; set; }         // may be empty

        public long Reward { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction_Localised.Alt(AwardingFaction) + " " + Reward);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("; cr;N0", Reward, "< from ".Txb(this), AwardingFaction_Localised,
                "< , due to ".Txb(this), VictimFaction_Localised);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CommitCrime)]
    public class JournalCommitCrime : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCommitCrime(JObject evt) : base(evt, JournalTypeEnum.CommitCrime)
        {
            CrimeType = evt["CrimeType"].Str().SplitCapsWordFull();
            Faction = evt["Faction"].Str();
            Victim = evt["Victim"].Str();
            VictimLocalised = JournalFieldNaming.CheckLocalisation(evt["Victim_Localised"].Str(), Victim);
            Fine = evt["Fine"].LongNull();
            Bounty = evt["Bounty"].LongNull();
        }
        public string CrimeType { get; set; }
        public string Faction { get; set; }
        public string Victim { get; set; }
        public string VictimLocalised { get; set; }
        public long? Fine { get; set; }
        public long? Bounty { get; set; }
        public long Cost { get { return (Fine.HasValue ? Fine.Value : 0) + (Bounty.HasValue ? Bounty.Value : 0); } }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string v = (VictimLocalised.Length > 0) ? VictimLocalised : Victim;

            if (v.Length == 0)
                v = Faction;

            if (Fine.HasValue)
                v += string.Format(" Fine {0:N0}".Tx(this), Fine.Value);

            if (Bounty.HasValue)
                v += string.Format(" Bounty {0:N0}".Tx(this, "Bounty"), Bounty.Value);

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} on {1}".Txb(this), CrimeType, v));
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", CrimeType, "< on faction ".Txb(this), Faction, "Against ".Txb(this), VictimLocalised, "Cost:; cr;N0".Txb(this), Fine, "Bounty:; cr;N0".Txb(this), Bounty);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FactionKillBond)]
    public class JournalFactionKillBond : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalFactionKillBond(JObject evt) : base(evt, JournalTypeEnum.FactionKillBond)
        {
            AwardingFaction = evt["AwardingFaction"].Str();
            AwardingFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["AwardingFaction_Localised"].Str(), AwardingFaction);
            VictimFaction = evt["VictimFaction"].Str();
            VictimFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["VictimFaction_Localised"].Str(), VictimFaction);
            Reward = evt["Reward"].Long();
        }

        public string AwardingFaction { get; set; }
        public string AwardingFaction_Localised { get; set; }       // may be empty
        public string VictimFaction { get; set; }
        public string VictimFaction_Localised { get; set; }         // may be empty
        public long Reward { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction_Localised.Alt(AwardingFaction) + " " + Reward.ToString("N0"));
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Reward:; cr;N0".Txb(this), Reward, "< from ".Txb(this), AwardingFaction_Localised,
                "< , due to ".Txb(this), VictimFaction_Localised);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.PayBounties)]
    public class JournalPayBounties : JournalEntry, ILedgerJournalEntry
    {
        public JournalPayBounties(JObject evt) : base(evt, JournalTypeEnum.PayBounties)
        {
            Amount = evt["Amount"].Long();
            BrokerPercentage = evt["BrokerPercentage"].Double();
            AllFines = evt["AllFines"].Bool();
            Faction = evt["Faction"].Str();
            Faction_Localised = JournalFieldNaming.CheckLocalisation(evt["Faction_Localised"].Str(), Faction);
            ShipId = evt["ShipID"].Int();
        }

        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        public string Faction { get; set; }      // may be blank
        public string Faction_Localised { get; set; }    // may be blank
        public int ShipId { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, (Faction_Localised.Length > 0 ? "Faction " + Faction_Localised : "") + " Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Amount, "< to ".Txb(this), Faction_Localised);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".Txb(this), BrokerPercentage);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.PayFines)]
    public class JournalPayFines : JournalEntry, ILedgerJournalEntry
    {
        public JournalPayFines(JObject evt) : base(evt, JournalTypeEnum.PayFines)
        {
            Amount = evt["Amount"].Long();
            BrokerPercentage = evt["BrokerPercentage"].Double();
            AllFines = evt["AllFines"].Bool();
            Faction = evt["Faction"].Str();
            Faction_Localised = JournalFieldNaming.CheckLocalisation(evt["Faction_Localised"].Str(), Faction);
            ShipId = evt["ShipID"].Int();
        }

        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        public string Faction { get; set; } // may be blank
        public string Faction_Localised { get; set; }       // may be blank
        public int ShipId { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, (Faction_Localised.Length > 0 ? "Faction " + Faction_Localised : "") + " Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Amount, "< to ".Txb(this), Faction_Localised);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".Txb(this), BrokerPercentage);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.PayLegacyFines)]
    public class JournalPayLegacyFines : JournalEntry, ILedgerJournalEntry
    {
        public JournalPayLegacyFines(JObject evt) : base(evt, JournalTypeEnum.PayLegacyFines)
        {
            Amount = evt["Amount"].Long();
            BrokerPercentage = evt["BrokerPercentage"].Double();
        }
        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Amount);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".Txb(this), BrokerPercentage);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.RedeemVoucher)]
    public class JournalRedeemVoucher : JournalEntry, ILedgerJournalEntry
    {
        public JournalRedeemVoucher(JObject evt) : base(evt, JournalTypeEnum.RedeemVoucher)
        {
            Type = evt["Type"].Str().SplitCapsWordFull();
            Amount = evt["Amount"].Long();
            Faction = evt["Faction"].Str();
            BrokerPercentage = evt["BrokerPercentage"].Double();
        }

        public string Type { get; set; }
        public long Amount { get; set; }
        public string Faction { get; set; }
        public double BrokerPercentage { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " Broker " + BrokerPercentage.ToString("0.0") + "%", Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Type:".Txb(this), Type, "Amount:; cr;N0".Txb(this), Amount, "Faction:".Txb(this), Faction);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".Txb(this), BrokerPercentage);
            detailed = "";
        }
    }


}
