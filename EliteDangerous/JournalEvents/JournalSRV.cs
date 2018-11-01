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
    [JournalEntryType(JournalTypeEnum.DockSRV)]
    public class JournalDockSRV : JournalEntry, IShipInformation
    {
        public JournalDockSRV(JObject evt ) : base(evt, JournalTypeEnum.DockSRV)
        {
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.DockSRV();
        }

        public override void FillInformation(out string info, out string detailed)  
        {
            info = "";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.LaunchSRV)]
    public class JournalLaunchSRV : JournalEntry, IShipInformation
    {
        public JournalLaunchSRV(JObject evt) : base(evt, JournalTypeEnum.LaunchSRV)
        {
            Loadout = evt["Loadout"].Str();
            PlayerControlled = evt["PlayerControlled"].Bool(true);

        }
        public string Loadout { get; set; }
        public bool PlayerControlled { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.LaunchSRV();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Loadout:".Txb(this), Loadout) + BaseUtils.FieldBuilder.Build(", NPC Controlled;".Txb(this), PlayerControlled);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.SRVDestroyed)]
    public class JournalSRVDestroyed : JournalEntry, IShipInformation
    {
        public JournalSRVDestroyed(JObject evt) : base(evt, JournalTypeEnum.SRVDestroyed)
        {
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.DestroyedSRV();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "";
            detailed = "";
        }

    }


}
