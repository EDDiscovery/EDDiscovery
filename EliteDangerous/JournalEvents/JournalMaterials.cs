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
                FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
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


    [JournalEntryType(JournalTypeEnum.MaterialCollected)]
    public class JournalMaterialCollected : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalMaterialCollected(JObject evt) : base(evt, JournalTypeEnum.MaterialCollected)
        {
            Category = JournalFieldNaming.NormaliseMaterialCategory(evt["Category"].Str());
            Name = JournalFieldNaming.FDNameTranslation(evt["Name"].Str());     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
            Count = evt["Count"].Int(1);
        }
        public string Category { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(Category, Name, Count, 0, conn);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", FriendlyName, "< ; items".Txb(this, "MatC"), Count);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.MaterialDiscarded)]
    public class JournalMaterialDiscarded : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalMaterialDiscarded(JObject evt) : base(evt, JournalTypeEnum.MaterialDiscarded)
        {
            Category = JournalFieldNaming.NormaliseMaterialCategory(evt["Category"].Str());
            Name = JournalFieldNaming.FDNameTranslation(evt["Name"].Str());     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
            Count = evt["Count"].Int();
        }

        public string Category { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }    // FDName
        public int Count { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(Category, Name, -Count, 0, conn);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", FriendlyName, "< ; items".Txb(this, "MatC"), Count);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.MaterialDiscovered)]
    public class JournalMaterialDiscovered : JournalEntry
    {
        public JournalMaterialDiscovered(JObject evt) : base(evt, JournalTypeEnum.MaterialDiscovered)
        {
            Category = JournalFieldNaming.NormaliseMaterialCategory(evt["Category"].Str());
            Name = JournalFieldNaming.FDNameTranslation(evt["Name"].Str());     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
            DiscoveryNumber = evt["DiscoveryNumber"].Int();
        }

        public string Category { get; set; }
        public string Name { get; set; }    // FDName
        public string FriendlyName { get; set; }
        public int DiscoveryNumber { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", FriendlyName);
            if (DiscoveryNumber > 0)
                info += string.Format(", Discovery {0}".Tx(this, "DN"), DiscoveryNumber);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.MaterialTrade)]
    public class JournalMaterialTrade : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalMaterialTrade(JObject evt) : base(evt, JournalTypeEnum.MaterialTrade)
        {
            MarketID = evt["MarketID"].LongNull();
            TraderType = evt["TraderType"].Str();

            Paid = evt["Paid"]?.ToObjectProtected<Traded>();
            if (Paid != null)
                Paid.Normalise();

            Received = evt["Received"]?.ToObjectProtected<Traded>();
            if (Received != null)
                Received.Normalise();
        }

        public string TraderType { get; set; }
        public long? MarketID { get; set; }
        public Traded Paid { get; set; }      // may be null
        public Traded Received { get; set; } // may be null

        public class Traded
        {
            public string Material;     //fdname
            public string FriendlyMaterial; // our name
            public string Material_Localised;   // their localised name if present
            public string Category;     // journal says always there.  If not, use tradertype
            public string Category_Localised;
            public int Quantity;

            public void Normalise()
            {
                Material = JournalFieldNaming.FDNameTranslation(Material);
                FriendlyMaterial = MaterialCommodityData.GetNameByFDName(Material);
                Material_Localised = JournalFieldNaming.CheckLocalisationTranslation(Material_Localised ?? "", FriendlyMaterial);       // ensure.

                if (Category != null)       // some entries do not have this
                {
                    Category = JournalFieldNaming.NormaliseMaterialCategory(Category);  // fix up any strangeness
                    Category_Localised = JournalFieldNaming.CheckLocalisation(Category_Localised ?? "", Category);
                }
            }
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Paid != null && Received != null)
            {
                mc.Change(Paid.Category.Alt(TraderType), Paid.Material, -Paid.Quantity, 0, conn);
                mc.Change(Received.Category.Alt(TraderType), Received.Material, Received.Quantity, 0, conn);
            }
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = detailed = "";

            if (Paid != null && Received != null)
            {
                info = BaseUtils.FieldBuilder.Build("Sold:".Txb(this), Paid.Quantity, "< ", Paid.Material_Localised,
                                                    "Received:".Txb(this), Received.Quantity, "< ", Received.Material_Localised);
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.Synthesis)]
    public class JournalSynthesis : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalSynthesis(JObject evt) : base(evt, JournalTypeEnum.Synthesis)
        {
            Materials = null;

            Name = evt["Name"].Str().SplitCapsWordFull();
            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                Materials = new Dictionary<string, int>();

                if (mats.Type == JTokenType.Object)
                {
                    Dictionary<string, int> temp = mats?.ToObjectProtected<Dictionary<string, int>>();

                    if (temp != null)
                    {
                        foreach (string key in temp.Keys)
                            Materials[JournalFieldNaming.FDNameTranslation(key)] = temp[key];
                    }
                }
                else
                {
                    foreach (JObject ja in (JArray)mats)
                    {
                        Materials[JournalFieldNaming.FDNameTranslation((string)ja["Name"])] = ja["Count"].Int();
                    }
                }
            }
        }
        public string Name { get; set; }
        public Dictionary<string, int> Materials { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (KeyValuePair<string, int> k in Materials)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);        // same as this, uses up materials
            }

            if (Name.Contains("Limpet", StringComparison.InvariantCultureIgnoreCase) )      // hard code limpets mean 1 more cargo of them
            {
                mc.Change(MaterialCommodityData.CommodityCategory, "drones", 1, 0, conn);
            }
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Name;
            if (Materials != null)
                foreach (KeyValuePair<string, int> k in Materials)
                    info += ", " + MaterialCommodityData.GetNameByFDName(k.Key) + ":" + k.Value.ToString();

            detailed = "";
        }
    }


}
