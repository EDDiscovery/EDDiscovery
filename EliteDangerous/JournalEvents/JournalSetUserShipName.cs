/*
 * Copyright © 2017 EDDiscovery development team
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
    [JournalEntryType(JournalTypeEnum.SetUserShipName)]
    public class JournalSetUserShipName : JournalEntry , IShipInformation
    {
        public JournalSetUserShipName(JObject evt) : base(evt, JournalTypeEnum.SetUserShipName)
        {
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipID = evt["ShipID"].Int();
            ShipName = evt["UserShipName"].Str();// name to match LoadGame
            ShipIdent = evt["UserShipId"].Str();     // name to match LoadGame
        }

        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string ShipIdent { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.IUserDatabase conn)
        {
            shp.SetUserShipName(this);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("",ShipName,"", ShipIdent, "On:".T(EDTx.JournalSetUserShipName_On) , Ship);
            detailed = "";
        }
    }
}
