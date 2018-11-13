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
using EliteDangerousCore.DB;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.SupercruiseEntry)]
    public class JournalSupercruiseEntry : JournalEntry, IShipInformation
    {
        public JournalSupercruiseEntry(JObject evt ) : base(evt, JournalTypeEnum.SupercruiseEntry)
        {
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();

        }
        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = StarSystem;
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, SQLiteConnectionUser conn)
        {
            shp.SupercruiseEntry(this);
        }
    }

    [JournalEntryType(JournalTypeEnum.SupercruiseExit)]
    public class JournalSupercruiseExit : JournalEntry, IBodyNameAndID
    {
        public JournalSupercruiseExit(JObject evt) : base(evt, JournalTypeEnum.SupercruiseExit)
        {
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
            BodyType = JournalFieldNaming.NormaliseBodyType(evt["BodyType"].Str());
        }

        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }
        public string BodyDesignation { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("At ".Tx(this, "At"), Body, "< in ".Txb(this), StarSystem, "Type:".Txb(this), BodyType);
            detailed = "";
        }
    }

}
