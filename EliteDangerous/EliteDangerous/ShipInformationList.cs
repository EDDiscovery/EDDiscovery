using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("{currentid} ships {Ships.Count}")]
    public class ShipInformationList
    {
        public Dictionary<string, ShipInformation> Ships { get; private set; }         // by shipid

        public ModulesInStore StoredModules { get; private set; }                   // stored modules

        private Dictionary<string, string> itemlocalisation;

        private string currentid;
        public bool HaveCurrentShip { get { return currentid != null; } }
        public ShipInformation CurrentShip { get { return (HaveCurrentShip) ? Ships[currentid] : null; } }

        // IDs have been repeated, need more than just that
        private string Key(string fdname, int i) { return fdname.ToLower() + ":" + i.ToStringInvariant(); }

        public ShipInformation GetShipByShortName(string sn)
        {
            List<ShipInformation> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipShortName.Equals(sn));
            return (index >= 0) ? lst[index] : null;
        }

        public ShipInformation GetShipByNameIdentType(string sn)
        {
            List<ShipInformation> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipNameIdentType.Equals(sn));
            return (index >= 0) ? lst[index] : null;
        }

        public ShipInformation GetShipByFullInfoMatch(string sn)
        {
            List<ShipInformation> lst = Ships.Values.ToList();
            int index = lst.FindIndex(x => x.ShipFullInfo().IndexOf(sn, StringComparison.InvariantCultureIgnoreCase) != -1);
            return (index >= 0) ? lst[index] : null;
        }

        private int newsoldid = 30000;

        public ShipInformationList()
        {
            Ships = new Dictionary<string, ShipInformation>();
            StoredModules = new ModulesInStore();
            itemlocalisation = new Dictionary<string, string>();
            currentid = null;
        }

        public void Loadout(int id, string ship, string shipfd, string name, string ident, List<ShipModule> modulelist,
                        long HullValue, long ModulesValue, long Rebuy)
        {
            string sid = Key(shipfd, id);

            //System.Diagnostics.Debug.WriteLine("Loadout {0} {1} {2} {3}", id, ship, name, ident);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm = sm.SetShipDetails(ship, shipfd, name, ident, 0, 0, HullValue, ModulesValue, Rebuy);     // update ship key, make a fresh one if required.

            //System.Diagnostics.Debug.WriteLine("Loadout " + sm.ID + " " + sm.ShipFD);

            ShipInformation newsm = null;       // if we change anything, we need a new clone..

            foreach (ShipModule m in modulelist)
            {
                if (!sm.Contains(m.Slot) || !sm.Same(m))  // no slot, or not the same data.. (ignore localised item)
                {
                    if (m.LocalisedItem == null && itemlocalisation.ContainsKey(m.Item))        // if we have a cached localisation, use it
                    {
                        m.LocalisedItem = itemlocalisation[m.Item];
                        //                        System.Diagnostics.Debug.WriteLine("Have localisation for " + m.Item + ": " + m.LocalisedItem);
                    }

                    if (newsm == null)              // if not cloned
                    {
                        newsm = sm.ShallowClone();  // we need a clone, pointing to the same modules, but with a new dictionary
                        Ships[sid] = newsm;              // update our record of last module list for this ship
                    }

                    newsm.SetModule(m);                   // update entry only.. rest will still point to same entries
                }
            }
        }

        public void LoadGame(int id, string ship, string shipfd, string name, string ident, double fuellevel, double fueltotal)        // LoadGame..
        {
            string sid = Key(shipfd, id);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            Ships[sid] = sm = sm.SetShipDetails(ship, shipfd, name, ident, fuellevel, fueltotal);   // this makes a shallow copy if any data has changed..

            //System.Diagnostics.Debug.WriteLine("Load Game " + sm.ID + " " + sm.Ship);

            if (!JournalFieldNaming.IsSRVOrFighter(ship))
                currentid = sid;
        }

        public void LaunchSRV()
        {
            //System.Diagnostics.Debug.WriteLine("Launch SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.SRV);
        }

        public void DockSRV()
        {
            //System.Diagnostics.Debug.WriteLine("Dock SRV");
            if (HaveCurrentShip)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.None);
        }

        public void LaunchFighter(bool pc)
        {
            //System.Diagnostics.Debug.WriteLine("Launch Fighter");
            if (HaveCurrentShip && pc == true)
                Ships[currentid] = Ships[currentid].SetSubVehicle(ShipInformation.SubVehicleType.Fighter);
        }

        public void DockFighter()
        {
            //System.Diagnostics.Debug.WriteLine("Dock Fighter");
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

        public void ShipyardSwap(JournalShipyardSwap e, string station, string system)
        {
            if ( e.StoreShipId.HasValue)    // if we have an old ship ID (old records do not)
            { 
                string oldship = Key(e.StoreOldShipFD, e.StoreShipId.Value);

                if (Ships.ContainsKey(oldship))
                {
                    //System.Diagnostics.Debug.WriteLine(e.StoreOldShipFD + " Swap Store at " + system + ":" + station);
                    Ships[oldship] = Ships[oldship].Store(station, system);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(e.StoreOldShipFD + " Cant find to swap");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.StoreOldShipFD + " Cant find to swap");
            }

            //System.Diagnostics.Debug.WriteLine(e.ShipFD + " Swap to at " + system );
            string sid = Key(e.ShipFD, e.ShipId);           //swap to new ship

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(e.ShipType, e.ShipFD);   // shallow copy if changed
            sm = sm.SwapTo();                               // swap into
            Ships[sid] = sm;
            currentid = sid;
        }

        public void ShipyardNew(string ship, string shipFD, int id)
        {
            string sid = Key(shipFD, id);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.SetShipDetails(ship, shipFD); // shallow copy if changed
            currentid = sid;
        }

        public void Sell(string ShipFD, int id)
        {
            string sid = Key(ShipFD, id);
            if (Ships.ContainsKey(sid))       // if we don't have it, don't worry
            {
                //System.Diagnostics.Debug.WriteLine(ShipFD + " Sell ");
                Ships[sid] = Ships[sid].SellShip();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(ShipFD + " can't find to Sell");
            }
        }

        public void Transfer(string ship, string shipFD, int id, string fromsystem, string tosystem, string tostation, DateTime arrivaltime)
        {
            string sid = Key(shipFD, id);
            ShipInformation sm = EnsureShip(sid);              // this either gets current ship or makes a new one.
            sm = sm.SetShipDetails(ship, shipFD);               // set up minimum stuff we know about it
            sm = sm.Transfer(tosystem, tostation, arrivaltime);    // transfer set up
            Ships[sid] = sm;
            //System.Diagnostics.Debug.WriteLine(shipFD + " Transfer from " + fromsystem + " to " + tosystem + ":" + tostation + " arrives " + arrivaltime.ToString());
        }

        public void Store(string ShipFD, int id, string station, string system)
        {
            string sid = Key(ShipFD, id);
            if (Ships.ContainsKey(sid))       // if we don't have it, don't worry
            {
                //System.Diagnostics.Debug.WriteLine(ShipFD + " store on buy at " + system);
                Ships[sid] = Ships[sid].Store(station, system);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(ShipFD + " cannot find ship to store on buy");
            }
        }

        public void StoredShips(JournalStoredShips.StoredShipInformation[] ships)
        {
            foreach (var i in ships)
            {
                //System.Diagnostics.Debug.WriteLine(i.ShipTypeFD + " Stored info " + i.StarSystem + ":" + i.StationName + " transit" + i.InTransit);
                string sid = Key(i.ShipTypeFD, i.ShipID);
                ShipInformation sm = EnsureShip(sid);              // this either gets current ship or makes a new one.
                sm = sm.SetShipDetails(i.ShipType, i.ShipTypeFD,i.Name);  // set up minimum stuff we know about it

                if ( !i.InTransit )                                 // if in transit, we don't know where it is, ignore
                    sm = sm.Store(i.StationName, i.StarSystem);         // ship is not with us, its stored, so store it.

                Ships[sid] = sm;
            }
        }

        public void SetUserShipName(JournalSetUserShipName e)
        {
            string sid = Key(e.ShipFD, e.ShipID);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.SetShipDetails(e.Ship, e.ShipFD, e.ShipName, e.ShipIdent); // will clone if data changed..
            currentid = sid;           // must be in it to do this
        }

        public void ModuleBuy(JournalModuleBuy e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);              // this either gets current ship or makes a new one.

            if (e.StoredItem.Length > 0)                             // if we stored something
                StoredModules = StoredModules.StoreModule(e.StoredItem, e.StoredItemLocalised);

            // if we sold it, who cares?
            Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.BuyItem, e.BuyItemFD, e.BuyItemLocalised);      // replace the slot with this

            itemlocalisation[e.BuyItem] = e.BuyItemLocalised;       // record any localisations
            if (e.SellItem.Length > 0)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;
            if (e.StoredItem.Length > 0)
                itemlocalisation[e.StoredItem] = e.StoredItemLocalised;

            currentid = sid;           // must be in it to do this
        }

        public void ModuleSell(JournalModuleSell e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.RemoveModule(e.Slot, e.SellItem);

            if (e.SellItem.Length > 0)
                itemlocalisation[e.SellItem] = e.SellItemLocalised;

            currentid = sid;           // must be in it to do this
        }

        public void ModuleSwap(JournalModuleSwap e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.SwapModule(e.FromSlot, e.FromSlotFD, e.FromItem, e.FromItemFD, e.FromItemLocalised,
                                            e.ToSlot, e.ToSlotFD, e.ToItem, e.ToItemFD, e.ToItemLocalised);
            currentid = sid;           // must be in it to do this
        }

        public void ModuleStore(JournalModuleStore e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            if (e.ReplacementItem.Length > 0)
                Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.ReplacementItem, e.ReplacementItemFD, e.ReplacementItemLocalised);
            else
                Ships[sid] = sm.RemoveModule(e.Slot, e.StoredItem);

            StoredModules = StoredModules.StoreModule(e.StoredItem, e.StoredItemLocalised);
            currentid = sid;           // must be in it to do this
        }

        public void ModuleRetrieve(JournalModuleRetrieve e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.

            if (e.SwapOutItem.Length > 0)
                StoredModules = StoredModules.StoreModule(e.SwapOutItem, e.SwapOutItemLocalised);

            Ships[sid] = sm.AddModule(e.Slot, e.SlotFD, e.RetrievedItem, e.RetrievedItemFD, e.RetrievedItemLocalised);

            StoredModules = StoredModules.RemoveModule(e.RetrievedItem);
        }

        public void ModuleSellRemote(JournalModuleSellRemote e)
        {
            StoredModules = StoredModules.RemoveModule(e.SellItem);
        }

        public void MassModuleStore(JournalMassModuleStore e)
        {
            string sid = Key(e.ShipFD, e.ShipId);

            ShipInformation sm = EnsureShip(sid);            // this either gets current ship or makes a new one.
            Ships[sid] = sm.RemoveModules(e.ModuleItems);
            StoredModules = StoredModules.StoreModule(e.ModuleItems, itemlocalisation);
        }

        public void UpdateStoredModules(JournalStoredModules s)
        {
            StoredModules = StoredModules.UpdateStoredModules(s.ModuleItems);
        }

        public void FSDJump(JournalFSDJump e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.FuelLevel);
            }
        }

        public void FuelScoop(JournalFuelScoop e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(e.Total);
            }
        }

        public void RefuelAll(JournalRefuelAll e)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.SetFuelLevel(CurrentShip.FuelCapacity);
            }
        }

        public void RefuelPartial(JournalRefuelPartial e)
        {
            if (HaveCurrentShip)
            {
                // Amount includes reserve
                double level = CurrentShip.FuelLevel + e.Amount - 0.1;

                // If amount refuelled is less than 10%, then the tank is full
                if (e.Amount < CurrentShip.FuelCapacity / 10 || level > CurrentShip.FuelCapacity)
                    level = CurrentShip.FuelCapacity;

                Ships[currentid] = CurrentShip.SetFuelLevel(level);
            }
        }

        public void EngineerCraft(JournalEngineerCraftBase c)
        {
            if (HaveCurrentShip)
            {
                Ships[currentid] = CurrentShip.Craft(c.Slot, c.Module, c.Engineering);
            }
        }

        #region Helpers

        private ShipInformation EnsureShip(string id)      // ensure we have an ID of this type..
        {
            if (Ships.ContainsKey(id))
            {
                ShipInformation sm = Ships[id];
                if (!sm.Sold)               // if not sold, ok
                    return sm;
                else
                {
                    Ships[Key(sm.ShipFD, newsoldid++)] = sm;                      // okay, we place this information on 30000+  all Ids of this will now refer to new entry
                }
            }

            //System.Diagnostics.Debug.WriteLine("Made new ship " + id);

            int i;
            id.Substring(id.IndexOf(":") + 1).InvariantParse(out i);
            ShipInformation smn = new ShipInformation(i);
            Ships[id] = smn;
            return smn;
        }

        #endregion

        #region process

        public Tuple<ShipInformation, ModulesInStore> Process(JournalEntry je, DB.SQLiteConnectionUser conn, string whereami, ISystem system)
        {
            if (je is IShipInformation)
            {
                IShipInformation e = je as IShipInformation;
                e.ShipInformation(this, whereami, system, conn);                             // not cloned.. up to callers to see if they need to
            }

            return new Tuple<ShipInformation, ModulesInStore>(CurrentShip, StoredModules);
        }

        #endregion
    }


    public class ModulesInStore
    {
        public class StoredModule               // storage used by journal event..
        {
            public int StorageSlot;
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
                    ShipModuleData.ShipModule smd = ShipModuleData.Instance.GetModuleProperties(Name);
                    return smd?.mass ?? 0;
                }
            }

            public void Normalise()
            {
                Name = JournalFieldNaming.GetBetterItemNameEvents(Name);
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
