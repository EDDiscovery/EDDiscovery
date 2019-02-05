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
    [JournalEntryType(JournalTypeEnum.MiningRefined)]
    public class JournalMiningRefined : JournalEntry, ICommodityJournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalMiningRefined(JObject evt) : base(evt, JournalTypeEnum.MiningRefined)
        {
            Type = JournalFieldNaming.FixCommodityName(evt["Type"].Str());          // instances of $.._name, translate to FDNAME
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = MaterialCommodityData.GetNameByFDName(Type);
            Type_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Type_Localised"].Str(), FriendlyType);
        }

        public string Type { get; set; }                                        // FIXED fdname always.. vital it stays this way
        public string FriendlyType { get; set; }
        public string Type_Localised { get; set; }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, Type, 1, 0, conn);
        }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, Type_Localised);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Type_Localised;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.AsteroidCracked)]
    public class JournalAsteroidCracked : JournalEntry
    {
        public JournalAsteroidCracked(JObject evt) : base(evt, JournalTypeEnum.AsteroidCracked)
        {
            Body = evt["Body"].Str();
        }

        public string Body { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Body;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.ProspectedAsteroid)]
    public class JournalProspectedAsteroid : JournalEntry
    {
        public class Material
        {
            public string Name { get; set; }        //FDNAME
            public string FriendlyName { get; set; }        //friendly
            public double Proportion { get; set; }      // 0-100

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
            }
        }

        public JournalProspectedAsteroid(JObject evt) : base(evt, JournalTypeEnum.ProspectedAsteroid)
        {
            Content = evt["Content"].Str();     // strange string with $AsteroidMaterialContent_High
            Content_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Content_Localised"].Str(), Content);

            MotherlodeMaterial = JournalFieldNaming.FDNameTranslation(evt["MotherlodeMaterial"].Str());
            FriendlyMotherlodeMaterial = MaterialCommodityData.GetNameByFDName(MotherlodeMaterial);

            Remaining = evt["Remaining"].Double();      // 0-100o
            Materials = evt["Materials"]?.ToObjectProtected<Material[]>().OrderBy(x => x.Name)?.ToArray();

            if ( Materials != null )
            {
                foreach (Material m in Materials)
                    m.Normalise();
            }
        }

        public string Content { get; set; }
        public string Content_Localised { get; set; }

        public string MotherlodeMaterial { get; set; }
        public string FriendlyMotherlodeMaterial { get; set; }

        public double Remaining { get; set; }
        public Material[] Materials { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", FriendlyMotherlodeMaterial, "", Content_Localised, "Remaining:;%;N1".Tx(this), Remaining);
            detailed = "";

            if ( Materials != null )
            {
                foreach (Material m in Materials)
                {
                    detailed = detailed.AppendPrePad( BaseUtils.FieldBuilder.Build("", m.FriendlyName, "< ;%;N1", m.Proportion), System.Environment.NewLine );
                }
            }
        }
    }

}
