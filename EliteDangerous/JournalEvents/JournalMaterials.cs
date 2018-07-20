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
    [JournalEntryType(JournalTypeEnum.Materials)]
    public class JournalMaterials : JournalEntry, IMaterialCommodityJournalEntry
    {
        public class Material
        {
            public string Name { get; set; }        //FDNAME
            public string FriendlyName { get; set; }        //friendly
            public int Count { get; set; }

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = JournalFieldNaming.RMat(Name);
            }
        }

        public JournalMaterials(JObject evt) : base(evt, JournalTypeEnum.Materials)
        {
            Raw = evt["Raw"]?.ToObjectProtected<Material[]>().OrderBy(x => x.Name)?.ToArray();
            FixNames(Raw);
            Manufactured = evt["Manufactured"]?.ToObjectProtected<Material[]>().OrderBy(x => x.Name)?.ToArray();
            FixNames(Manufactured);
            Encoded = evt["Encoded"]?.ToObjectProtected<Material[]>().OrderBy(x => x.Name)?.ToArray();
            FixNames(Encoded);
        }

        public Material[] Raw { get; set; }             //FDNAMES on purpose
        public Material[] Manufactured { get; set; }
        public Material[] Encoded { get; set; }

        void FixNames(Material[] a)
        {
            if (a != null)
            {
                foreach (Material m in a)
                    m.Normalise();
            }
        }

        public override void FillInformation(out string info, out string detailed)  
        {
            
            info = "";
            detailed = "";
            if (Raw != null && Raw.Length>0)
            {
                info += BaseUtils.FieldBuilder.Build("Raw:".Tx(this) + "; ", Raw.Length);
                detailed += "Raw:".Tx(this) + List(Raw);
            }
            if (Manufactured != null && Manufactured.Length>0)
            {
                info += BaseUtils.FieldBuilder.Build("Manufactured:".Tx(this) + "; ", Manufactured.Length);// NOT DONE
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;
                detailed += "Manufactured:".Tx(this) + List(Manufactured);
            }
            if (Encoded != null && Encoded.Length > 0)
            {
                info += BaseUtils.FieldBuilder.Build("Encoded:".Tx(this) + "; ", Encoded.Length);// NOT DONE
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;
                detailed += "Encoded:".Tx(this) + List(Encoded);
            }
        }

        public string List(Material[] mat)
        {
            StringBuilder sb = new StringBuilder(64);

            foreach (Material m in mat)
            {
                sb.Append(Environment.NewLine);
                sb.Append(BaseUtils.FieldBuilder.Build(" ", m.FriendlyName, "; items".Txb(this), m.Count));
            }
            return sb.ToString();
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine("Updated at " + this.EventTimeUTC.ToString());
            mc.Clear(false);

            if ( Raw != null )
                foreach (Material m in Raw)
                    mc.Set(MaterialCommodityData.MaterialRawCategory, m.Name, m.Count, 0, conn);

            if ( Manufactured != null )
                foreach (Material m in Manufactured)
                    mc.Set(MaterialCommodityData.MaterialManufacturedCategory, m.Name, m.Count, 0, conn);

            if ( Encoded != null )
                foreach (Material m in Encoded)
                    mc.Set(MaterialCommodityData.MaterialEncodedCategory, m.Name, m.Count, 0, conn);
        }
    }
}
