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
 *
 * Data courtesy of Coriolis.IO https://github.com/EDCD/coriolis , data is intellectual property and copyright of Frontier Developments plc ('Frontier', 'Frontier Developments') and are subject to their terms and conditions.
 */

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    public class ModulesInStore
    {
        public class StoredModule: IEquatable<StoredModule> // storage used by journal event..
        {
            public int StorageSlot;
            public string NameFD;
            public string Name;
            public string Name_Localised;
            public string StarSystem;       // not while in transit
            public long MarketID;       // not while in transit
            public long TransferCost;   // not while in transit
            public int TransferTime;    // not while in transit
            public string EngineerModifications;    // null if none present
            public double Quality;      // may not be there
            public int Level;           // may not be there
            public bool Hot;
            public bool InTransit;
            public int BuyPrice;

            public System.TimeSpan TransferTimeSpan;        // computed
            public string TransferTimeString; // computed, empty if not in tranfer (time<=0)

            public double Mass
            {
                get
                {
                    ShipModuleData.ShipModule smd = ShipModuleData.Instance.GetItemProperties(Name);
                    return smd?.Mass ?? 0;
                }
            }

            public void Normalise()
            {
                NameFD = JournalFieldNaming.NormaliseFDItemName(Name);          // Name comes in with strange characters, normalise out
                Name = JournalFieldNaming.GetBetterItemName(NameFD);      // and look up a better name
                Name_Localised = Name_Localised.Alt(Name);
                TransferTimeSpan = new System.TimeSpan((int)(TransferTime / 60 / 60), (int)((TransferTime / 60) % 60), (int)(TransferTime % 60));
                TransferTimeString = TransferTime > 0 ? TransferTimeSpan.ToString() : "";
            }

            public StoredModule(string item, string item_localised)
            {
                Name = item;
                Name_Localised = item_localised.Alt(Name);
                //System.Diagnostics.Debug.WriteLine("Store module '" + item + "' '" + item_localised + "'");
            }

            public StoredModule()
            {
            }

            public bool Equals(StoredModule other)
            {
                return (StorageSlot == other.StorageSlot && string.Compare(Name, other.Name) == 0 && string.Compare(Name_Localised, other.Name_Localised) == 0 &&
                         string.Compare(StarSystem, other.StarSystem) == 0 && MarketID == other.MarketID && TransferCost == other.TransferCost &&
                         TransferTime == other.TransferTime && string.Compare(EngineerModifications, other.EngineerModifications) == 0 &&
                         Quality == other.Quality && Level == other.Level && Hot == other.Hot && InTransit == other.InTransit && BuyPrice == other.BuyPrice);
            }
        }


        public List<StoredModule> StoredModules { get; private set; }       // by STORE id

        public ModulesInStore()
        {
            StoredModules = new List<StoredModule>();
        }

        public ModulesInStore(List<StoredModule> list)
        {
            StoredModules = new List<StoredModule>(list);
        }

        public ModulesInStore StoreModule(string item, string itemlocalised)
        {
            ModulesInStore mis = this.ShallowClone();
            mis.StoredModules.Add(new StoredModule(item, itemlocalised));
            return mis;
        }

        public ModulesInStore StoreModule(JournalMassModuleStore.ModuleItem[] items, Dictionary<string, string> itemlocalisation)
        {
            ModulesInStore mis = this.ShallowClone();
            foreach (var it in items)
            {
                string local = itemlocalisation.ContainsKey(it.Name) ? itemlocalisation[it.Name] : "";
                mis.StoredModules.Add(new StoredModule(it.Name, local));
            }
            return mis;
        }

        public ModulesInStore RemoveModule(string item)
        {
            int index = StoredModules.FindIndex(x => x.Name.Equals(item, StringComparison.InvariantCultureIgnoreCase));  // if we have an item of this name
            if (index != -1)
            {
                //System.Diagnostics.Debug.WriteLine("Remove module '" + item + "'  '" + StoredModules[index].Name_Localised + "'");
                ModulesInStore mis = this.ShallowClone();
                mis.StoredModules.RemoveAt(index);
                return mis;
            }
            else
                return this;
        }

        public ModulesInStore UpdateStoredModules(StoredModule[] newlist)
        {
            ModulesInStore mis = new ModulesInStore(newlist.ToList());      // copy constructor ..
            return mis;
        }

        public ModulesInStore ShallowClone()          // shallow clone.. does not clone the ship modules, just the dictionary
        {
            ModulesInStore mis = new ModulesInStore(this.StoredModules);
            return mis;
        }
    }
}
