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

namespace EliteDangerousCore.JournalEvents
{
    //    When written: at startup, when loading from main menu into game
    //Parameters:
    //•	Raw: array of raw materials(each with name and count)
    //•	Manufactured: array of manufactured items
    //•	Encoded: array of scanned data

    //Example:
    //{ "timestamp":"2017-02-10T14:25:51Z", "event":"Materials", "Raw":[ { "Name":"chromium", "Count":28 }, { "Name":"zinc", "Count":18 }, { "Name":"iron", "Count":23 }, { "Name":"sulphur", "Count":19 } ], "Manufactured":[ { "Name":"refinedfocuscrystals", "Count":10 }, { "Name":"highdensitycomposites", "Count":3 }, { "Name":"mechanicalcomponents", "Count":3 } ], "Encoded":[ { "Name":"emissiondata", "Count":32 }, { "Name":"shielddensityreports", "Count":23 } } ] }

    [JournalEntryType(JournalTypeEnum.Materials)]
    public class JournalMaterials : JournalEntry, IMaterialCommodityJournalEntry
    {
        public class Material
        {
            public string Name { get; set; }        //FDNAME
            public int Count { get; set; }
        }

        public JournalMaterials(JObject evt) : base(evt, JournalTypeEnum.Materials)
        {
            Raw = evt["Raw"]?.ToObject<Material[]>().OrderBy(x => x.Name).ToArray();
            FixNames(Raw);
            Manufactured = evt["Manufactured"]?.ToObject<Material[]>().OrderBy(x => x.Name).ToArray();
            FixNames(Manufactured);
            Encoded = evt["Encoded"]?.ToObject<Material[]>().OrderBy(x => x.Name).ToArray();
            FixNames(Encoded);
        }

        public Material[] Raw { get; set; }             //FDNAMES on purpose
        public Material[] Manufactured { get; set; }
        public Material[] Encoded { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.materials; } }

        void FixNames(Material[] a)
        {
            foreach (Material m in a)
                m.Name = JournalFieldNaming.FDNameTranslation(m.Name);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
            if (Raw != null && Raw.Length>0)
            {
                info += BaseUtils.FieldBuilder.Build("Raw:; ", Raw.Length);
                detailed += "Raw:" + List(Raw);
            }
            if (Manufactured != null && Manufactured.Length>0)
            {
                info += BaseUtils.FieldBuilder.Build("Manufactured:; ", Manufactured.Length);// NOT DONE
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;
                detailed += "Manufactured:" + List(Manufactured);
            }
            if (Encoded != null && Encoded.Length > 0)
            {
                info += BaseUtils.FieldBuilder.Build("Encoded:; ", Encoded.Length);// NOT DONE
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;
                detailed += "Manufactured:" + List(Encoded);
            }
        }

        public string List(Material[] mat)
        {
            StringBuilder sb = new StringBuilder(64);

            foreach (Material m in mat)
            {
                sb.Append(Environment.NewLine);
                sb.Append(BaseUtils.FieldBuilder.Build(" ", JournalFieldNaming.RMat(m.Name), "; items", m.Count));
            }
            return sb.ToString();
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine("Updated at " + this.EventTimeUTC.ToString());
            mc.Clear(false);

            if ( Raw != null )
                foreach (Material m in Raw)
                    mc.Set(MaterialCommodities.MaterialRawCategory, m.Name, m.Count, 0, conn);

            if ( Manufactured != null )
                foreach (Material m in Manufactured)
                    mc.Set(MaterialCommodities.MaterialManufacturedCategory, m.Name, m.Count, 0, conn);

            if ( Encoded != null )
                foreach (Material m in Encoded)
                    mc.Set(MaterialCommodities.MaterialEncodedCategory, m.Name, m.Count, 0, conn);
        }
    }
}
