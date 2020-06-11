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

            TargetLocalised = Target = evt["Target"].StrNull();       // only set for skimmer target missions

            if (Target != null)
            {
                TargetLocalised = JournalFieldNaming.CheckLocalisation(evt["Target_Localised"].Str(), Target);  // 3.7 added a localised target field, so try it

                var sp = ShipModuleData.Instance.GetShipProperties(Target);
                if (sp != null)
                {
                    TargetLocalised = ((ShipModuleData.ShipInfoString)sp[ShipModuleData.ShipPropID.Name]).Value;
                }
            }


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
        public string TargetLocalised { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public void LedgerNC(Ledger mcl)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} total {1:N0}".T(EDTx.JournalEntry_LegBounty), VictimFactionLocalised, TotalReward));
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("; cr;N0", TotalReward, "Target:".T(EDTx.JournalEntry_Target), TargetLocalised, "Victim faction:".T(EDTx.JournalEntry_Victimfaction), VictimFactionLocalised);

            detailed = "";
            if ( Rewards!=null)
            {
                foreach (BountyReward r in Rewards)
                {
                    if (detailed.Length > 0)
                        detailed += ", ";

                    detailed += BaseUtils.FieldBuilder.Build("Faction:".T(EDTx.JournalEntry_Faction), r.Faction, "; cr;N0", r.Reward);
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

        public void LedgerNC(Ledger mcl)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction_Localised.Alt(AwardingFaction) + " " + Reward);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("; cr;N0", Reward, "< from ".T(EDTx.JournalEntry_from), AwardingFaction_Localised,
                "< , due to ".T(EDTx.JournalEntry_dueto), VictimFaction_Localised);
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

        public void LedgerNC(Ledger mcl)
        {
            string v = (VictimLocalised.Length > 0) ? VictimLocalised : Victim;

            if (v.Length == 0)
                v = Faction;

            if (Fine.HasValue)
                v += string.Format(" Fine {0:N0}".T(EDTx.JournalCommitCrime_Fine), Fine.Value);

            if (Bounty.HasValue)
                v += string.Format(" Bounty {0:N0}".T(EDTx.JournalCommitCrime_Bounty), Bounty.Value);

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} on {1}".T(EDTx.JournalEntry_0), CrimeType, v));
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", CrimeType, "< on faction ".T(EDTx.JournalEntry_onfaction), Faction, "Against ".T(EDTx.JournalEntry_Against), VictimLocalised, "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Fine, "Bounty:; cr;N0".T(EDTx.JournalEntry_Bounty), Bounty);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrimeVictim)]
    public class JournalCrimeVictim : JournalEntry      
    {
        // presuming its co-incident with commit crime so don't double count bounties

        public JournalCrimeVictim(JObject evt) : base(evt, JournalTypeEnum.CrimeVictim)
        {
            CrimeType = evt["CrimeType"].Str().SplitCapsWordFull();
            Offender = evt["Offender"].Str();
            OffenderLocalised = JournalFieldNaming.CheckLocalisation(evt["Offender_Localised"].Str(), Offender);
            Bounty = evt["Bounty"].Long();
        }
        public string CrimeType { get; set; }
        public string Offender { get; set; }
        public string OffenderLocalised { get; set; }
        public long Bounty { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", CrimeType, "Offender ".T(EDTx.JournalEntry_Offender), OffenderLocalised, "Bounty:; cr;N0".T(EDTx.JournalEntry_Bounty), Bounty);
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

        public void LedgerNC(Ledger mcl)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction_Localised.Alt(AwardingFaction) + " " + Reward.ToString("N0"));
        }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Reward:; cr;N0".T(EDTx.JournalEntry_Reward), Reward, "< from ".T(EDTx.JournalEntry_from), AwardingFaction_Localised,
                "< , due to ".T(EDTx.JournalEntry_dueto), VictimFaction_Localised);
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

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, (Faction_Localised.Length > 0 ? "Faction " + Faction_Localised : "") + " Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Amount, "< to ".T(EDTx.JournalEntry_to), Faction_Localised);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".T(EDTx.JournalEntry_Brokertook), BrokerPercentage);
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

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, (Faction_Localised.Length > 0 ? "Faction " + Faction_Localised : "") + " Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Amount, "< to ".T(EDTx.JournalEntry_to), Faction_Localised);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".T(EDTx.JournalEntry_Brokertook), BrokerPercentage);
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

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Amount);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".T(EDTx.JournalEntry_Brokertook), BrokerPercentage);
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

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " Broker " + BrokerPercentage.ToString("0.0") + "%", Amount);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Type:".T(EDTx.JournalEntry_Type), Type, "Amount:; cr;N0".T(EDTx.JournalEntry_Amount), Amount, "Faction:".T(EDTx.JournalEntry_Faction), Faction);
            if (BrokerPercentage > 0)
                info += string.Format(", Broker took {0:N0}%".T(EDTx.JournalEntry_Brokertook), BrokerPercentage);
            detailed = "";
        }
    }


}
