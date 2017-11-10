/*
 * Copyright © 2016 EDDiscovery development team
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
    //When Written: when scooping cargo from space or planet surface
    //Parameters:
    //•	Type: cargo type
    //•	Stolen: whether stolen goods
    [JournalEntryType(JournalTypeEnum.CollectCargo)]
    public class JournalCollectCargo : JournalEntry, IMaterialCommodityJournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCollectCargo(JObject evt ) : base(evt, JournalTypeEnum.CollectCargo)
        {
            Type = evt["Type"].Str();                               //FDNAME
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = JournalFieldNaming.RMat(Type);
            Stolen = evt["Stolen"].Bool();
        }

        public string Type { get; set; }            // FDNAME..
        public string FriendlyType { get; set; }            // translated name
        public bool Stolen { get; set; }

        public override System.Drawing.Bitmap DefaultIcon { get { return EliteDangerous.Properties.Resources.collectcargo; } }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodities.CommodityCategory, Type, 1, 0, conn);
        }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, FriendlyType );
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", FriendlyType, ";Stolen", Stolen);
            detailed = "";
        }
    }
}
