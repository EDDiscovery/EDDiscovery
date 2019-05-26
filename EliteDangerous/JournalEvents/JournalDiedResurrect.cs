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
    [JournalEntryType(JournalTypeEnum.Died)]
    public class JournalDied : JournalEntry, IMissions, ICommodityJournalEntry
    {
        public class Killer
        {
            public string Name;
            public string Name_Localised;
            public string Ship;
            public string Rank;
        }

        public JournalDied(JObject evt ) : base(evt, JournalTypeEnum.Died)
        {
            string killerName = evt["KillerName"].Str();
            if (string.IsNullOrEmpty(killerName))
            {
                if (evt["Killers"] != null)
                    Killers = evt["Killers"].ToObjectProtected<Killer[]>();
            }
            else
            {
                // it was an individual
                Killers = new Killer[1]
                {
                        new Killer
                        {
                            Name = killerName,
                            Name_Localised = evt["KillerName_Localised"].Str(),
                            Ship = evt["KillerShip"].Str(),
                            Rank = evt["KillerRank"].Str()
                        }
                };
            }

            if (Killers != null)
            {
                foreach (Killer k in Killers)
                {
                    k.Ship = JournalFieldNaming.GetBetterShipName(k.Ship);
                    k.Name_Localised = JournalFieldNaming.CheckLocalisation(k.Name_Localised??"",k.Name);
                }
            }
        }

        public Killer[] Killers { get; set; }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.IUserDatabase conn)
        {
            mlist.Died(this.EventTimeUTC);
        }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.IUserDatabase conn)
        {
            mc.Died();
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = "";
            if (Killers != null)
            {
                foreach (Killer k in Killers)
                {
                    info = info.AppendPrePad(string.Format("{0} in ship type {1} rank {2}".T(EDTx.JournalEntry_Died) , k.Name_Localised , k.Ship , k.Rank) , ", ");
                }

                info = "Killed by ".T(EDTx.JournalEntry_Killedby) + info;
            }

            detailed = "";
        }

    }


    [JournalEntryType(JournalTypeEnum.SelfDestruct)]
    public class JournalSelfDestruct : JournalEntry
    {
        public JournalSelfDestruct(JObject evt) : base(evt, JournalTypeEnum.SelfDestruct)
        {
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Boom!".T(EDTx.JournalEntry_Boom);
            detailed = "";
        }
    }



    [JournalEntryType(JournalTypeEnum.Resurrect)]
    public class JournalResurrect : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalResurrect(JObject evt) : base(evt, JournalTypeEnum.Resurrect)
        {
            Option = evt["Option"].Str().SplitCapsWordFull();
            Cost = evt["Cost"].Long();
            Bankrupt = evt["Bankrupt"].Bool();
        }

        public string Option { get; set; }
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }

        public void Ledger(Ledger mcl, DB.IUserDatabase conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Option, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.IUserDatabase conn)
        {
            shp.Resurrect(Option.Equals("free", System.StringComparison.InvariantCultureIgnoreCase));    // if free, we did not rebuy the ship
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Option:".T(EDTx.JournalEntry_Option), Option, "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Cost, ";Bankrupt".T(EDTx.JournalEntry_Bankrupt), Bankrupt);
            detailed = "";
        }
    }


}
