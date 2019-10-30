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
    [JournalEntryType(JournalTypeEnum.TechnologyBroker)]
    public class JournalTechnologyBroker : JournalEntry
    {
        public JournalTechnologyBroker(JObject evt) : base(evt, JournalTypeEnum.TechnologyBroker)
        {
            BrokerType = evt["BrokerType"].Str("Unknown");
            MarketID = evt["MarketID"].LongNull();

            ItemsUnlocked = evt["ItemsUnlocked"]?.ToObjectProtected<Unlocked[]>();      //3.03 entry
            CommodityList = evt["Commodities"]?.ToObjectProtected<Commodities[]>();
            MaterialList = evt["Materials"]?.ToObjectProtected<Materials[]>();

            if (ItemsUnlocked != null)
                foreach (Unlocked u in ItemsUnlocked)
                    u.Name_Localised = JournalFieldNaming.CheckLocalisation(u.Name_Localised??"",u.Name);

            if (CommodityList != null)
                foreach (Commodities c in CommodityList)
                    c.FriendlyName = MaterialCommodityData.GetNameByFDName(c.Name);

            if (MaterialList != null)
                foreach (Materials m in MaterialList)
                {
                    m.FriendlyName = MaterialCommodityData.GetNameByFDName(m.Name);
                    m.Category = JournalFieldNaming.NormaliseMaterialCategory(m.Category);
                }

            string oldentry = evt["ItemUnlocked"].StrNull();        // 3.02 journal entry
            if (ItemsUnlocked == null && oldentry != null)
                ItemsUnlocked = new Unlocked[] { new Unlocked() { Name = oldentry, Name_Localised = oldentry } };
        }

        public string BrokerType { get; set; }
        public long? MarketID { get; set; }
        public Unlocked[] ItemsUnlocked { get; set; }
        public Materials[] MaterialList { get; set; }
        public Commodities[] CommodityList { get; set; }

        public class Unlocked
        {
            public string Name;
            public string Name_Localised;
        }

        public class Commodities
        {
            public string Name;
            public string FriendlyName;
            public int Count;
        }

        public class Materials
        {
            public string Name;
            public string FriendlyName;
            public string Category;
            public int Count;
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Type:".T(EDTx.JournalEntry_Type), BrokerType);

            if (ItemsUnlocked != null)
            {
                foreach (Unlocked u in ItemsUnlocked)
                    info = info.AppendPrePad(u.Name_Localised, ", ");
            }

            detailed = "";

            if (CommodityList != null)
                foreach (Commodities c in CommodityList)
                    detailed = detailed.AppendPrePad(c.FriendlyName + ":" + c.Count.ToString(), ", "); 

            if (MaterialList != null)
                foreach (Materials m in MaterialList)
                    detailed = detailed.AppendPrePad(m.FriendlyName + ":" + m.Count.ToString(), ", ");
        }
    }
}
