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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.CodexEntry)]
    public class JournalCodexEntry : JournalEntry
    {
        public JournalCodexEntry(JObject evt) : base(evt, JournalTypeEnum.CodexEntry)
        {
            EntryID = evt["EntryID"].Long();
            Name = evt["Name"].Str();
            Name_Localised = JournalFieldNaming.CheckLocalisation(evt["Name_Localised"].Str(), Name);
            SubCategory = evt["SubCategory"].Str();
            SubCategory_Localised = JournalFieldNaming.CheckLocalisation(evt["SubCategory_Localised"].Str(), SubCategory);
            Category = evt["Category"].Str();
            Category_Localised = JournalFieldNaming.CheckLocalisation(evt["Category_Localised"].Str(), Category);
            Region = evt["Region"].Str();
            Region_Localised = evt["Region_Localised"].Str();
            System = evt["System"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();
            IsNewEntry = evt["IsNewEntry"].BoolNull();
            NewTraitsDiscovered = evt["NewTraitsDiscovered"].BoolNull();
            if ( evt["Traits"] != null )
                Traits = evt["Traits"].ToObjectProtected<string[]>();
        }

        public long EntryID { get; set; }
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public string Category { get; set; }
        public string Category_Localised { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory_Localised { get; set; }
        public string Region { get; set; }
        public string Region_Localised { get; set; }
        public string System { get; set; }
        public long? SystemAddress { get; set; }
        public bool? IsNewEntry { get; set; }
        public bool? NewTraitsDiscovered { get; set; }
        public string [] Traits { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("At ".Tx(this), System,
                                                "in ".Tx(this), Region_Localised,
                                                "", Name_Localised,
                                                "", Category_Localised,
                                                "", SubCategory_Localised,
                                                ";New Entry".Tx(this), IsNewEntry,
                                                ";Traits".Tx(this), NewTraitsDiscovered);
            detailed = "";

            if (Traits != null)
                detailed = String.Join(",", Traits);
        }
    }
}