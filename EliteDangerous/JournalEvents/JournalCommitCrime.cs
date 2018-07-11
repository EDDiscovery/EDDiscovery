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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.CommitCrime)]
    public class JournalCommitCrime : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCommitCrime(JObject evt ) : base(evt, JournalTypeEnum.CommitCrime)
        {
            CrimeType = evt["CrimeType"].Str().SplitCapsWordFull();
            Faction = evt["Faction"].Str();
            Victim = evt["Victim"].Str();
            VictimLocalised = JournalFieldNaming.CheckLocalisation(evt["Victim_Localised"].Str(),Victim);
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
                v += string.Format(" Bounty {0:N0}".Tx(this,"Bounty"), Bounty.Value);

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} on {1}".Txb(this) , CrimeType , v));
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("", CrimeType, "< on faction ".Txb(this), Faction, "Against ".Txb(this), VictimLocalised, "Cost:; cr;N0".Txb(this), Fine, "Bounty:; cr;N0".Txb(this), Bounty);
            detailed = "";
        }
    }
}
