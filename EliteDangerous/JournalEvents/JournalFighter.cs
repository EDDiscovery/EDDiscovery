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
    [JournalEntryType(JournalTypeEnum.DockFighter)]
    public class JournalDockFighter : JournalEntry,  IShipInformation
    {
        public int? ID { get; set; }

        public JournalDockFighter(JObject evt ) : base(evt, JournalTypeEnum.DockFighter)
        {
            ID = evt["ID"].IntNull();
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.IUserDatabase conn)
        {
            shp.DockFighter();
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = "";
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.FighterDestroyed)]
    public class JournalFighterDestroyed : JournalEntry, IShipInformation
    {
        public int? ID { get; set; }

        public JournalFighterDestroyed(JObject evt) : base(evt, JournalTypeEnum.FighterDestroyed)
        {
            ID = evt["ID"].IntNull();
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.IUserDatabase conn)
        {
            shp.FighterDestroyed();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FighterRebuilt)]
    public class JournalFighterRebuilt : JournalEntry
    {
        public JournalFighterRebuilt(JObject evt) : base(evt, JournalTypeEnum.FighterRebuilt)
        {
            Loadout = evt["Loadout"].Str();
            ID = evt["ID"].IntNull();
        }

        public string Loadout { get; set; }
        public int? ID { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Loadout);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.LaunchFighter)]
    public class JournalLaunchFighter : JournalEntry, IShipInformation
    {
        public JournalLaunchFighter(JObject evt) : base(evt, JournalTypeEnum.LaunchFighter)
        {
            Loadout = evt["Loadout"].Str();
            PlayerControlled = evt["PlayerControlled"].Bool();
            ID = evt["ID"].IntNull();
        }
        public string Loadout { get; set; }
        public bool PlayerControlled { get; set; }
        public int? ID { get; set; }


        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.IUserDatabase conn)
        {
            shp.LaunchFighter(PlayerControlled);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Loadout:".T(EDTx.JournalEntry_Loadout), Loadout) + BaseUtils.FieldBuilder.Build(", NPC Controlled;".T(EDTx.JournalEntry_NPCControlled), PlayerControlled);
            detailed = "";
        }
    }


}
