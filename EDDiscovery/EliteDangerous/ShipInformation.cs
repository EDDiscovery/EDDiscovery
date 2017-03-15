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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.EliteDangerous.JournalEvents;

namespace EDDiscovery.EliteDangerous
{
    [System.Diagnostics.DebuggerDisplay("{Ship} {Modules.Count}")]
    public class ShipInformation
    {
        public int ID { get; private set; }                 // its ID.     ID's are moved to high range when sold
        public bool Sold { get; set; }                      // if sold.
        public string Ship { get; private set; }            // ship type name, fer-de-lance, etc.           can be null
        public string ShipName { get; private set; }        // ship name, may be empty or null
        public string ShipIdent { get; private set; }       // ship ident, may be empty or null
        public double FuelCapacity { get; private set; }   // fuel capacity, 0 unknown

        public enum SubVehicleType
        {
            None, SRV, Fighter
        }

        public SubVehicleType SubVehicle { get; private set; } = SubVehicleType.None;    // if in a sub vehicle or mothership

        public Dictionary<string, JournalLoadout.ShipModule> Modules { get; private set; }

        public string ShipFullName
        {
            get                  // unique ID
            {
                StringBuilder sb = new StringBuilder(64);
                if ( ShipIdent!=null)
                    sb.Append(ShipIdent);
                sb.AppendPrePad(ShipName);
                sb.AppendPrePad(Ship);
                sb.AppendPrePad("(" + ID.ToString() + ")");

                if (SubVehicle == SubVehicleType.SRV)
                    sb.AppendPrePad(" in SRV");
                if (SubVehicle == SubVehicleType.Fighter)
                    sb.AppendPrePad(" Control Fighter");
                return sb.ToString();
            }
        }

        public string ShipShortName
        {
            get                  // unique ID
            {
                StringBuilder sb = new StringBuilder(64);
                if (ShipName != null && ShipName.Length > 0)
                {
                    sb.AppendPrePad(ShipName);
                }
                else
                {
                    sb.AppendPrePad(Ship);
                    sb.AppendPrePad("(" + ID.ToString() + ")");
                }
                return sb.ToString();
            }
        }

        public ShipInformation(int id)
        {
            ID = id;
            Modules = new Dictionary<string, JournalLoadout.ShipModule>();
        }

        public ShipInformation ShallowClone()          // shallow clone.. does not clone the ship modules, just the dictionary
        {
            ShipInformation sm = new ShipInformation(this.ID);
            sm.Sold = this.Sold;
            sm.Ship = this.Ship;
            sm.ShipName = this.ShipName;
            sm.ShipIdent = this.ShipIdent;
            sm.FuelCapacity = this.FuelCapacity;
            sm.SubVehicle = this.SubVehicle;
            sm.Modules = new Dictionary<string, JournalLoadout.ShipModule>(this.Modules);
            return sm;
        }

        public bool Contains(string slot)
        {
            return Modules.ContainsKey(slot);
        }

        public bool Same(JournalLoadout.ShipModule sm)
        {
            if (Modules.ContainsKey(sm.Slot))
            {
                bool v = Modules[sm.Slot].Same(sm);
                if (v)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public void Set(JournalLoadout.ShipModule sm)
        {
            if ( Modules.ContainsKey(sm.Slot) )
            {
                JournalLoadout.ShipModule oldsm = Modules[sm.Slot];

                if (sm.Item.Equals(oldsm.Item) && sm.LocalisedItem == null && oldsm.LocalisedItem != null)  // if item the same, old one has a localised name..
                    sm.LocalisedItem = oldsm.LocalisedItem;

            }
            Modules[sm.Slot] = sm;
        }

        public ShipInformation Set(string ship, string name = null, string ident = null, double capacity = -1 )
        {
            if (ship != Ship || (name != null && name != ShipName) || 
                            (ident != null && ident != ShipIdent) || 
                            (capacity >= 0 && capacity != FuelCapacity) )
            {
                ShipInformation sm = this.ShallowClone();

                sm.Ship = ship;
                if (name != null)
                    sm.ShipName = name;
                if (ident != null)
                    sm.ShipIdent = ident;
                if (capacity >= 0)
                    sm.FuelCapacity = capacity;

                return sm;
            }

            return this;
        }

        public ShipInformation SetSubVehicle(SubVehicleType vh)
        {
            if (vh != this.SubVehicle)
            {
                ShipInformation sm = this.ShallowClone();
                sm.SubVehicle = vh;
                return sm;
            }
            else
                return this;
        }

        public ShipInformation AddModule(string slot, string item, string itemlocalised)
        {
            if (!Modules.ContainsKey(slot) || !Modules[slot].Same(item))       // if does not have it, or item is not the same..
            {
                ShipInformation sm = this.ShallowClone();
                sm.Modules[slot] = new JournalLoadout.ShipModule(slot, item, itemlocalised);
                return sm;
            }
            return this;
        }

        public ShipInformation RemoveModule(string slot)
        {
            if (Modules.ContainsKey(slot))       // if has it..
            {
                ShipInformation sm = this.ShallowClone();
                sm.Modules.Remove(slot);
                return sm;
            }
            return this;
        }

        public ShipInformation SwapModule(string fromslot, string fromitem, string fromitemlocalised,
                                          string toslot, string toitem, string toitemlocalised )
        {
            ShipInformation sm = this.ShallowClone();
            sm.Modules[fromslot] = new JournalLoadout.ShipModule(fromslot, toitem, toitemlocalised);
            sm.Modules[toslot] = new JournalLoadout.ShipModule(toslot, fromitem, fromitemlocalised);
            return sm;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(256);
            sb.AppendFormat("Ship {0}", ShipFullName);
            sb.Append(Environment.NewLine);
            foreach (JournalLoadout.ShipModule sm in Modules.Values)
            {
                sb.AppendFormat(sm.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }

    public class ModulesInStore
    {
        public List<JournalLoadout.ShipModule> StoredModules { get; private set; }       // by STORE id

        public ModulesInStore()
        {
            StoredModules = new List<JournalLoadout.ShipModule>();
        }

        public ModulesInStore( List<JournalLoadout.ShipModule> list )
        {
            StoredModules = new List<JournalLoadout.ShipModule>(list);
        }

        public ModulesInStore StoreModule(string item, string itemlocalised)
        {
            ModulesInStore mis = this.ShallowClone();
            mis.StoredModules.Add(new JournalLoadout.ShipModule("", item, itemlocalised));
            return mis;
        }

        public ModulesInStore RemoveModule(string item)
        {
            int index = StoredModules.FindIndex(x => x.Item.Equals(item, StringComparison.InvariantCultureIgnoreCase));  // if we have an item of this name
            if (index != -1)
            {
                ModulesInStore mis = this.ShallowClone();
                mis.StoredModules.RemoveAt(index);
                return mis;
            }
            else
                return this;
        }

        public ModulesInStore ShallowClone()          // shallow clone.. does not clone the ship modules, just the dictionary
        {
            ModulesInStore mis = new ModulesInStore(this.StoredModules);
            return mis;
        }
    }



    [System.Diagnostics.DebuggerDisplay("{currentid} ships {Ships.Count}")]
    public class ShipInformationList
    {
        public Dictionary<int, ShipInformation> Ships { get; private set; }         // by shipid

        public ModulesInStore StoredModules { get; private set; }                   // stored modules

        private Dictionary<string, string> itemlocalisation;

        private int currentid;
        public bool HaveCurrentShip { get { return currentid >= 0; } }
        public ShipInformation CurrentShip { get { return (HaveCurrentShip) ? Ships[currentid] : null; } }

        public ShipInformation GetShipByShortName( string sn )
        {
            List<ShipInformation> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipShortName.Equals(sn));
            return (index >= 0) ? lst[index] : null;
        }

        private int newsoldid = 30000;

        public ShipInformationList()
        {
            Ships = new Dictionary<int, ShipInformation>();
            StoredModules = new ModulesInStore();
            itemlocalisation = new Dictionary<string, string>();
            currentid = -1;
        }

        public void Loadout(int id, string ship, string name, string ident, List<JournalLoadout.ShipModule> modulelist)
        {
            ShipInformation sm = EnsureShip(id);            // this either gets current ship or makes a new one.
            sm.Set(ship, name, ident);
            System.Diagnostics.Debug.WriteLine("Loadout " + sm.ID + " " + sm.Ship);

            ShipInformation newsm = null;

            foreach (JournalLoadout.ShipModule m in modulelist)
            {
                if (!sm.Contains(m.Slot) || !sm.Same(m))  // no slot, or not the same data.. (ignore localised item)
                {
                    if (m.LocalisedItem == null && itemlocalisation.ContainsKey(m.Item))        // if we have a cached localisation, use it
                        m.LocalisedItem = itemlocalisation[m.Item];

                    if (newsm == null)              // if not cloned
                    {
                        newsm = sm.ShallowClone();  // we need a clone, pointing to the same modules, but with a new dictionary
                        Ships[id] = newsm;              // update our record of last module list for this ship
                    }

                    newsm.Set(m);                   // update entry only.. rest will still point to same entries
                }
            }
        }

        public void LoadGame(int id, string ship, string name, string ident, double capacity)        // LoadGame..
        {
            ShipInformation sm = EnsureShip(id);            // this either gets current ship or makes a new one.

            Ships[id] = sm = sm.Set(ship, name, ident, capacity);   // this makes a shallow copy if any data has changed..

            System.Diagnostics.Debug.WriteLine("Load Game " + sm.ID + " " + sm.Ship);

            if (!JournalFieldNaming.IsSRVOrFighter(ship))
                currentid = sm.ID;
        }

        public void LaunchSRV()
        {
            System.Diagnostics.Debug.WriteLine("Launch SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.SRV);
        }

        public void DockSRV()
        {
            System.Diagnostics.Debug.WriteLine("Dock SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.None);
        }

        public void LaunchFighter(bool pc)
        {
            System.Diagnostics.Debug.WriteLine("Launch Fighter");
            if (HaveCurrentShip && pc==true)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.Fighter);
        }

        public void DockFighter()
        {
            System.Diagnostics.Debug.WriteLine("Dock Fighter");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.None);
        }

        public void Resurrect()       
        {
            if (HaveCurrentShip)           // resurrect always in ship
            {
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.None);
            }
        }

        public void VehicleSwitch(string to)
        {
            if (HaveCurrentShip)
            {
                if (to == "Fighter")
                    Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.Fighter);
                else
                    Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.None);
            }
        }

        public void ShipyardSwap(JournalShipyardSwap e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.Set(e.ShipType); // shallow copy if changed
            currentid = e.ShipId;
        }

        public void ShipyardNew(JournalShipyardNew e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.Set(e.ShipType); // shallow copy if changed
            currentid = e.ShipId;
        }

        public void SetUserShipName(JournalSetUserShipName e)
        {
            ShipInformation sm = EnsureShip(e.ShipID);            // this either gets current ship or makes a new one.
            Ships[e.ShipID] = sm.Set(e.Ship, e.ShipName, e.ShipIdent);
            currentid = e.ShipID;           // must be in it to do this
        }

        public void ModuleBuy(JournalModuleBuy e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.AddModule(e.Slot,e.BuyItem,e.BuyItemLocalised);      // replace the slot with this

            itemlocalisation[e.BuyItem] = e.BuyItemLocalised;
            if (e.SellItem != null)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;

            currentid = e.ShipId;           // must be in it to do this
        }

        public void ModuleSell(JournalModuleSell e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.RemoveModule(e.Slot);

            if (e.SellItem != null)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;

            currentid = e.ShipId;           // must be in it to do this
        }

        public void ModuleSwap(JournalModuleSwap e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.SwapModule(e.FromSlot, e.FromItem, e.FromItemLocalised, e.ToSlot, e.ToItem, e.ToItemLocalised);
            currentid = e.ShipId;           // must be in it to do this
        }

        public void ModuleStore(JournalModuleStore e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.

            if (e.ReplacementItem.Length > 0)
                Ships[e.ShipId] = sm.AddModule(e.Slot, e.ReplacementItem, e.ReplacementItemLocalised);
            else
                Ships[e.ShipId] = sm.RemoveModule(e.Slot);

            StoredModules = StoredModules.StoreModule(e.StoredItem, e.StoredItemLocalised);
            currentid = e.ShipId;           // must be in it to do this
        }

        public void FetchRemoteModule(JournalFetchRemoteModule e)
        {

        }

        public void ModuleRetrieve(JournalModuleRetrieve e)
        {
            ShipInformation sm = EnsureShip(e.ShipId);            // this either gets current ship or makes a new one.
            Ships[e.ShipId] = sm.AddModule(e.Slot, e.RetrievedItem, e.RetrievedItemLocalised);
            StoredModules = StoredModules.RemoveModule(e.RetrievedItem);
        }

        public void ModuleSellRemote(JournalModuleSellRemote e)
        {
            StoredModules = StoredModules.RemoveModule(e.SellItem);
        }

        public void MassModuleStore(JournalMassModuleStore e)
        {

        }

        #region Helpers

        private ShipInformation EnsureShip(int id)      // ensure we have an ID of this type..
        {
            if (Ships.ContainsKey(id))
            {
                ShipInformation sm = Ships[id];
                if (!sm.Sold)               // if not sold, ok
                    return sm;             
                else
                {
                    Ships[newsoldid++] = sm;                      // okay, we place this information on 30000+  all Ids of this will now refer to new entry
                }
            }

            ShipInformation smn = new ShipInformation(id);
            Ships[id] = smn;
            return smn;
        }

        #endregion

        #region process

        public Tuple<ShipInformation,ModulesInStore> Process(JournalEntry je, DB.SQLiteConnectionUser conn)
        {
            if (je is IShipInformation)
            {
                IShipInformation e = je as IShipInformation;
                e.ShipInformation(this, conn);                             // not cloned.. up to callers to see if they need to
            }

            return new Tuple<ShipInformation,ModulesInStore>(CurrentShip,StoredModules);
        }

        #endregion
    }

}


#if false

#endif