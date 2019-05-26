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

using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("{Ident(true)} {Items.Length}")]
    public class Outfitting : IEquatable<Outfitting>
    {
        [System.Diagnostics.DebuggerDisplay("{Name} {BuyPrice}")]
        public class OutfittingItem: IEquatable<OutfittingItem>
        {
            public long id;
            public string Name;
            public long BuyPrice;

            public string ModType;      // computed fields..
            public string FDName;

            public void Normalise()
            {
                FDName = JournalFieldNaming.NormaliseFDItemName(Name);
                ShipModuleData.ShipModule item = ShipModuleData.Instance.GetItemProperties(FDName);
                Name = item.ModName;
                ModType = item.ModType;
            }

            public bool Equals(OutfittingItem other)
            {
                return (id == other.id && string.Compare(Name, other.Name) == 0 && string.Compare(FDName, other.FDName) == 0 &&
                         BuyPrice == other.BuyPrice);
            }

        }

        public OutfittingItem[] Items { get; private set; }
        public string StationName { get; private set; }
        public string StarSystem { get; private set; }
        public DateTime Datetime { get; private set; }

        public Outfitting()
        {
        }

        public Outfitting(string st, string sy, DateTime dt, OutfittingItem[] it)
        {
            StationName = st;
            StarSystem = sy;
            Datetime = dt;
            Items = it;
            if (Items != null)
                foreach (Outfitting.OutfittingItem i in Items)
                    i.Normalise();
        }

        public bool Equals(Outfitting other)
        {
            return string.Compare(StarSystem, other.StarSystem) == 0 && string.Compare(StationName, other.StationName) == 0 &&
                CollectionStaticHelpers.Equals(Items, other.Items);
        }

        public string Location { get { return StarSystem + ":" + StationName; } }

        public string Ident(bool utc)
        {
            return StarSystem + ":" + StationName + " on " + ((utc) ? Datetime.ToString() : Datetime.ToLocalTime().ToString());
        }

        public List<string> ItemList() { return (from x1 in Items select x1.Name).ToList(); }

        public OutfittingItem Find(string item) { return Array.Find(Items, x => x.Name.Equals(item)); }
        public List<OutfittingItem> FindType(string itemtype) { return (from x in Items where x.ModType.Equals(itemtype) select x).ToList(); }
    }

    [System.Diagnostics.DebuggerDisplay("Yards {OutfittingYards.Count}")]
    public class OutfittingList
    {
        public List<Outfitting> OutfittingYards { get; private set; }

        public OutfittingList()
        {
            OutfittingYards = new List<Outfitting>();
        }

        public List<string> OutfittingItemList()
        {
            List<string> items = new List<string>();
            foreach (Outfitting yard in OutfittingYards)
                items.AddRange(yard.ItemList());
            var dislist = (from x in items select x).Distinct().ToList();
            dislist.Sort();
            return dislist;
        }

        public List<Outfitting> GetFilteredList(bool nolocrepeats = false, int timeout = 60 * 5)           // latest first..
        {
            Outfitting last = null;
            List<Outfitting> yards = new List<Outfitting>();

            foreach (var yard in OutfittingYards.AsEnumerable().Reverse())        // give it to me in lastest one first..
            {
                if (!nolocrepeats || yards.Find(x => x.Location.Equals(yard.Location)) == null) // allow yard repeats or not in list
                {
                    // if no last or different name or time is older..
                    if (last == null || !yard.Location.Equals(last.Location) || (last.Datetime - yard.Datetime).TotalSeconds >= timeout)
                    {
                        yards.Add(yard);
                        last = yard;
                        //System.Diagnostics.Debug.WriteLine("OF return " + yard.Ident(true) + " " + yard.Datetime.ToString());
                    }
                }
            }

            return yards;
        }

        public List<Tuple<Outfitting, List<Outfitting.OutfittingItem>>> GetItemTypeLocationsFromYardsWithoutRepeat(string itemtype, bool nolocrepeats = false, int timeout = 60 * 5)       // without repeats note
        {
            List<Tuple<Outfitting, List<Outfitting.OutfittingItem>>> list = new List<Tuple<Outfitting, List<Outfitting.OutfittingItem>>>();

            List<Outfitting> yardswithoutrepeats = GetFilteredList(nolocrepeats, timeout);

            foreach (Outfitting yard in yardswithoutrepeats)
            {
                List<Outfitting.OutfittingItem> i = yard.FindType(itemtype);
                if ( i != null)
                    list.Add(new Tuple<Outfitting, List<Outfitting.OutfittingItem>>(yard, i));
            }

            return list;
        }

        public void Process(JournalEntry je, IUserDatabase conn)
        {
            if (je.EventTypeID == JournalTypeEnum.Outfitting)
            {
                JournalEvents.JournalOutfitting js = je as JournalEvents.JournalOutfitting;
                if (js.ItemList.Items != null)     // just in case we get a bad Outfitting with no data or its one which was not caught by the EDD at the time
                {
                    OutfittingYards.Add(js.ItemList);
                }
            }
        }
    }
}

