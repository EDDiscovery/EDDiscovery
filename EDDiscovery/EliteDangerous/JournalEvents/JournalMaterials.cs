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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: at startup, when loading from main menu into game
//Parameters:
//•	Raw: array of raw materials(each with name and count)
//•	Manufactured: array of manufactured items
//•	Encoded: array of scanned data

//Example:
//{ "timestamp":"2017-02-10T14:25:51Z", "event":"Materials", "Raw":[ { "Name":"chromium", "Count":28 }, { "Name":"zinc", "Count":18 }, { "Name":"iron", "Count":23 }, { "Name":"sulphur", "Count":19 } ], "Manufactured":[ { "Name":"refinedfocuscrystals", "Count":10 }, { "Name":"highdensitycomposites", "Count":3 }, { "Name":"mechanicalcomponents", "Count":3 } ], "Encoded":[ { "Name":"emissiondata", "Count":32 }, { "Name":"shielddensityreports", "Count":23 } } ] }

    [JournalEntryType(JournalTypeEnum.Materials)]
    public class JornalMaterials : JournalEntry
    {
        public class Material
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public JornalMaterials(JObject evt) : base(evt, JournalTypeEnum.Materials)
        {
            Raw = evt["Raw"]?.ToObject<Material[]>();
            Manufactured = evt["Manufactured"]?.ToObject<Material[]>();
            Encoded = evt["Encoded"]?.ToObject<Material[]>();


        }

        public Material[] Raw { get; set; }
        public Material[] Manufactured { get; set; }
        public Material[] Encoded { get; set; }





        //public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.location; } }

    }
}
