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
    [JournalEntryType(JournalTypeEnum.ApproachBody)]
    public class JournalApproachBody : JournalEntry, IBodyNameAndID
    {
        public JournalApproachBody(JObject evt) : base(evt, JournalTypeEnum.ApproachBody)
        {
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
        }

        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get { return "Planet"; } }
        public string BodyDesignation { get; set; }

        public override string SummaryName(ISystem sys)
        {
            string sn = base.SummaryName(sys);
            sn += " " + Body.ReplaceIfStartsWith(StarSystem);
            return sn;
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = "In ".Tx(this) + StarSystem;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.LeaveBody)]
    public class JournalLeaveBody : JournalEntry, IBodyNameAndID
    {
        public JournalLeaveBody(JObject evt) : base(evt, JournalTypeEnum.LeaveBody)
        {
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
        }

        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyDesignation { get; set; }
        public string BodyType { get { return "Planet"; } }

        public override string SummaryName(ISystem sys)
        {
            string sn = base.SummaryName(sys);
            sn += " " + Body.ReplaceIfStartsWith(StarSystem);
            return sn;
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "In ".Tx(this) + StarSystem;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.ApproachSettlement)]
    public class JournalApproachSettlement : JournalEntry
    {
        public JournalApproachSettlement(JObject evt) : base(evt, JournalTypeEnum.ApproachSettlement)
        {
            Name = evt["Name"].Str();
            Name_Localised = JournalFieldNaming.CheckLocalisation(evt["Name_Localised"].Str(), Name);
            MarketID = evt["MarketID"].LongNull();
            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();
        }

        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public long? MarketID { get; set; }
        public double? Latitude { get; set; }    // 3.3
        public double? Longitude { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Name_Localised;

            if (Latitude != null && Longitude != null)
                info += " " + JournalFieldNaming.RLat(Latitude) + " " + JournalFieldNaming.RLong(Longitude);

            detailed = "";
        }

    }


}
