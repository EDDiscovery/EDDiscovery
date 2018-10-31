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
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System.IO;
using EliteDangerousCore.DB;

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("{ID}:{ShipType}:{ShipFD}:{Modules.Count}")]
    public class ShipInformation
    {
        #region Information interface

        public int ID { get; private set; }                 // its ID.     ID's are moved to high range when sold
        public enum ShipState { Owned, Sold, Destroyed};
        public ShipState State { get; set; } = ShipState.Owned; // if owned, sold, destroyed. Default owned
        public string ShipType { get; private set; }        // ship type name, nice, fer-de-lance, etc. can be null
        public string ShipFD { get; private set; }          // ship type name, fdname
        public string ShipUserName { get; private set; }    // ship name, may be empty or null
        public string ShipUserIdent { get; private set; }   // ship ident, may be empty or null
        public double FuelLevel { get; private set; }       // fuel level may be 0 not known
        public double FuelCapacity { get; private set; }    // fuel capacity may be 0 not known
        public long HullValue { get; private set; }         // may be 0, not known
        public long ModulesValue { get; private set; }      // may be 0, not known
        public long Rebuy { get; private set; }             // may be 0, not known
        public string StoredAtSystem { get; private set; }  // null if not stored, else where stored
        public string StoredAtStation { get; private set; } // null if not stored or unknown
        public DateTime TransferArrivalTimeUTC { get; private set; }     // if current UTC < this, its in transit
        public bool Hot { get; private set; }               // if known to be hot.

        public enum SubVehicleType
        {
            None, SRV, Fighter
        }

        public SubVehicleType SubVehicle { get; private set; } = SubVehicleType.None;    // if in a sub vehicle or mothership

        public Dictionary<string, ShipModule> Modules { get; private set; }

        public bool InTransit { get { return TransferArrivalTimeUTC.CompareTo(DateTime.UtcNow)>0; } }

        public ShipModule GetModule(string name) { return Modules.ContainsKey(name) ? Modules[name] : null; }      // Name is the nice Slot name.
        public ShipModule.EngineeringData GetEngineering(string name) { return Modules.ContainsKey(name) ? Modules[name].Engineering : null; }

        public string ShipFullInfo(bool cargo = true, bool fuel = true)
        {
            StringBuilder sb = new StringBuilder(64);
            if (ShipUserIdent != null)
                sb.Append(ShipUserIdent);
            sb.AppendPrePad(ShipUserName);
            sb.AppendPrePad(ShipType);
            sb.AppendPrePad("(" + ID.ToString() + ")");

            if (SubVehicle == SubVehicleType.SRV)
                sb.AppendPrePad(" in SRV");
            else if (SubVehicle == SubVehicleType.Fighter)
                sb.AppendPrePad(" in Fighter");
            else
            {
                if (State != ShipState.Owned)
                    sb.Append(" (" + State.ToString() + ")");

                if (InTransit)
                    sb.Append(" (Tx to " + StoredAtSystem + ")");
                else if (StoredAtSystem != null)
                    sb.Append(" (@" + StoredAtSystem + ")");

                if (fuel)
                {
                    double cap = FuelCapacity;
                    if (cap > 0)
                        sb.Append(" Fuel Cap " + cap.ToString("0.#"));
                }

                if (cargo)
                {
                    double cap = CargoCapacity();
                    if (cap > 0)
                        sb.Append(" Cargo Cap " + cap);
                }
            }

            return sb.ToString();
        }

        public string Name          // Name of ship, either user named or ship type
        {
            get                  // unique ID
            {
                if (ShipUserName != null && ShipUserName.Length > 0)
                    return ShipUserName;
                else
                    return ShipType;
            }
        }

        public string ShipShortName
        {
            get                  // unique ID
            {
                StringBuilder sb = new StringBuilder(64);
                if (ShipUserName != null && ShipUserName.Length > 0)
                {
                    sb.AppendPrePad(ShipUserName);
                }
                else
                {
                    sb.AppendPrePad(ShipType);
                    sb.AppendPrePad("(" + ID.ToString() + ")");
                }
                return sb.ToString();
            }
        }

        public string ShipNameIdentType
        {
            get                  // unique ID
            {
                string res = string.IsNullOrEmpty(ShipUserName) ? "" : ShipUserName;
                res = res.AppendPrePad(string.IsNullOrEmpty(ShipUserIdent) ? "" : ShipUserIdent, ",");
                bool empty = string.IsNullOrEmpty(res);
                res = res.AppendPrePad(ShipType, ",");
                if (empty)
                    res += " (" + ID.ToString() + ")";

                if (State != ShipState.Owned)
                    res += " (" + State.ToString() + ")";

                if (InTransit)
                    res += " (Tx to " + StoredAtSystem + ")";
                else if (StoredAtSystem != null)
                    res += " (@" + StoredAtSystem + ")";

                return res;
            }
        }

        public int GetFuelCapacity()
        {
            int cap = 0;
            foreach (ShipModule sm in Modules.Values)
            {
                int classpos;
                if (sm.Item.Contains("Fuel Tank") && (classpos = sm.Item.IndexOf("Class ")) != -1)
                {
                    char digit = sm.Item[classpos + 6];
                    cap += (1 << (digit - '0'));        // 1<<1 = 2.. 1<<2 = 4, etc.
                }
            }

            return cap;
        }

        public int CargoCapacity()
        {
            int cap = 0;
            foreach (ShipModule sm in Modules.Values)
            {
                int classpos;
                if (sm.Item.Contains("Cargo Rack") && (classpos = sm.Item.IndexOf("Class ")) != -1)
                {
                    char digit = sm.Item[classpos + 6];
                    cap += (1 << (digit - '0'));        // 1<<1 = 2.. 1<<2 = 4, etc.
                }
            }

            return cap;
        }

        public EliteDangerousCalculations.FSDSpec GetFSDSpec()          // may be null due to not having the info
        {
            ShipModule fsd = GetModule("Frame Shift Drive");
            return fsd?.GetFSDSpec() ?? null;
        }

        public double ModuleMass()
        {
            return (from var in Modules select var.Value.Mass).Sum();
        }

        public double HullMass()
        {
            ShipModuleData.ShipInfo md = ShipModuleData.Instance.GetShipProperty(ShipFD, ShipModuleData.ShipPropID.HullMass);
            return md != null ? (md as ShipModuleData.ShipInfoDouble).Value : 0;
        }

        public double FuelWarningPercent
        {
            get { return SQLiteDBClass.GetSettingDouble("ShipInformation:" + ShipFD + ID + "Warninglevel", 0); }
            set { SQLiteDBClass.PutSettingDouble("ShipInformation:" + ShipFD + ID + "Warninglevel", value); }
        }

        public string Manufacturer
        {
            get
            {
                ShipModuleData.ShipInfo md = ShipModuleData.Instance.GetShipProperty(ShipFD, ShipModuleData.ShipPropID.Manu);
                return md != null ? (md as ShipModuleData.ShipInfoString).Value : "Unknown";
            }
        }

        public double Boost
        {
            get
            {
                ShipModuleData.ShipInfo md = ShipModuleData.Instance.GetShipProperty(ShipFD, ShipModuleData.ShipPropID.Boost);
                double v = md != null ? (md as ShipModuleData.ShipInfoInt).Value : 0;
                ShipModule.EngineeringData ed = GetEngineering("Main Thrusters"); // aka "MainEngines" in fd speak, but we use a slot naming conversion
                ed?.EngineerThrusters(ref v);
                return v;
            }
        }

        public double Speed
        {
            get
            {
                ShipModuleData.ShipInfo md = ShipModuleData.Instance.GetShipProperty(ShipFD, ShipModuleData.ShipPropID.Speed);
                double v = md != null ? (md as ShipModuleData.ShipInfoInt).Value : 0;
                ShipModule.EngineeringData ed = GetEngineering("Main Thrusters");
                ed?.EngineerThrusters(ref v);
                return v;
            }
        }

        public string PadSize
        {
            get
            {
                ShipModuleData.ShipInfo md = ShipModuleData.Instance.GetShipProperty(ShipFD, ShipModuleData.ShipPropID.Class);
                if (md == null)
                    return "Unknown";
                else
                {
                    int i = (md as ShipModuleData.ShipInfoInt).Value;
                    if (i == 1)
                        return "Small";
                    else if (i == 2)
                        return "Medium";
                    else
                        return "Large";
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(256);
            sb.AppendFormat("Ship {0}", ShipFullInfo());
            sb.Append(Environment.NewLine);
            foreach (ShipModule sm in Modules.Values)
            {
                sb.AppendFormat(sm.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region Creating and changing

        public ShipInformation(int id)
        {
            ID = id;
            Modules = new Dictionary<string, ShipModule>();
        }

        public ShipInformation ShallowClone()          // shallow clone.. does not clone the ship modules, just the dictionary
        {
            ShipInformation sm = new ShipInformation(this.ID);
            sm.State = this.State;
            sm.ShipType = this.ShipType;
            sm.ShipFD = this.ShipFD;
            sm.ShipUserName = this.ShipUserName;
            sm.ShipUserIdent = this.ShipUserIdent;
            sm.FuelLevel = this.FuelLevel;
            sm.FuelCapacity = this.FuelCapacity;
            sm.SubVehicle = this.SubVehicle;
            sm.HullValue = this.HullValue;
            sm.ModulesValue = this.ModulesValue;
            sm.Rebuy = this.Rebuy;
            sm.StoredAtStation = this.StoredAtStation;
            sm.StoredAtSystem = this.StoredAtSystem;
            sm.TransferArrivalTimeUTC = this.TransferArrivalTimeUTC;
            sm.Hot = this.Hot;
            sm.Modules = new Dictionary<string, ShipModule>(this.Modules);
            return sm;
        }

        public bool Contains(string slot)
        {
            return Modules.ContainsKey(slot);
        }

        public bool Same(ShipModule sm)
        {
            if (Modules.ContainsKey(sm.Slot))
            {
                return Modules[sm.Slot].Same(sm);
            }
            else
                return false;
        }

        public void SetModule(ShipModule sm)                // changed the module array, so you should have cloned that first..
        {
            if (Modules.ContainsKey(sm.Slot))
            {
                ShipModule oldsm = Modules[sm.Slot];

                if (sm.Item.Equals(oldsm.Item) && sm.LocalisedItem == null && oldsm.LocalisedItem != null)  // if item the same, old one has a localised name..
                    sm.LocalisedItem = oldsm.LocalisedItem;

            }

            Modules[sm.Slot] = sm;

            if (sm.Item.Contains("Fuel Tank") && sm.Item.IndexOf("Class ") != -1)
            {
                FuelCapacity = GetFuelCapacity();
                if (FuelLevel > FuelCapacity)
                    FuelLevel = FuelCapacity;
            }
        }

        public ShipInformation SetShipDetails(string ship, string shipfd, string name = null, string ident = null, 
                                    double fuellevel = 0, double fueltotal = 0,
                                    long hullvalue = 0, long modulesvalue = 0, long rebuy = 0, bool? hot = null)
        {
            if (ShipFD != shipfd || ship != ShipType || (name != null && name != ShipUserName) ||
                                (ident != null && ident != ShipUserIdent) ||
                                (fuellevel != 0 && fuellevel != FuelLevel) ||
                                (fueltotal != 0 && fueltotal != FuelCapacity) ||
                                (hullvalue != 0 && hullvalue != HullValue) ||
                                (modulesvalue != 0 && modulesvalue != ModulesValue) ||
                                (rebuy != 0 && rebuy != Rebuy) ||
                                (hot != null && hot.Value != Hot)
                                )
            {
                ShipInformation sm = this.ShallowClone();

                sm.ShipType = ship;
                sm.ShipFD = shipfd;
                if (name != null)
                    sm.ShipUserName = name;
                if (ident != null)
                    sm.ShipUserIdent = ident;
                if (fuellevel != 0)
                    sm.FuelLevel = fuellevel;
                if (fueltotal == 0 && fuellevel > sm.FuelCapacity)
                    sm.FuelCapacity = fuellevel;
                if (fueltotal != 0)
                    sm.FuelCapacity = fueltotal;
                if (hullvalue != 0)
                    sm.HullValue = hullvalue;
                if (modulesvalue != 0)
                    sm.ModulesValue = modulesvalue;
                if (rebuy != 0)
                    sm.Rebuy = rebuy;
                if (hot != null)
                    sm.Hot = hot.Value;

                //System.Diagnostics.Debug.WriteLine(ship + " " + sm.FuelCapacity + " " + sm.FuelLevel);
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

        public ShipInformation SetFuelLevel(double fuellevel)
        {
            if (fuellevel != 0 && fuellevel != FuelLevel)
            {
                ShipInformation sm = this.ShallowClone();

                if (fuellevel != 0)
                    sm.FuelLevel = fuellevel;
                if (fuellevel > sm.FuelCapacity)
                    sm.FuelCapacity = fuellevel;

                return sm;
            }

            return this;
        }

        public ShipInformation AddModule(string slot, string slotfd, string item, string itemfd, string itemlocalised)
        {
            if (!Modules.ContainsKey(slot) || Modules[slot].Item.Equals(item) == false)       // if does not have it, or item is not the same..
            {
                ShipInformation sm = this.ShallowClone();
                sm.Modules[slot] = new ShipModule(slot, slotfd, item, itemfd, itemlocalised);
                //System.Diagnostics.Debug.WriteLine("Slot add " + slot);

                if (item.Contains("Fuel Tank") && item.IndexOf("Class ") != -1)
                {
                    sm.FuelCapacity = sm.GetFuelCapacity();
                    if (sm.FuelLevel > sm.FuelCapacity)
                        sm.FuelLevel = sm.FuelCapacity;
                }

                return sm;
            }
            return this;
        }

        public ShipInformation RemoveModule(string slot, string item)
        {
            if (Modules.ContainsKey(slot))       // if has it..
            {
                ShipInformation sm = this.ShallowClone();
                sm.Modules.Remove(slot);
                //System.Diagnostics.Debug.WriteLine("Slot remove " + slot);

                if (item.Contains("Fuel Tank") && item.IndexOf("Class ") != -1)
                {
                    sm.FuelCapacity = sm.GetFuelCapacity();
                    if (sm.FuelLevel > sm.FuelCapacity)
                        sm.FuelLevel = sm.FuelCapacity;
                }

                return sm;
            }
            return this;
        }

        public ShipInformation RemoveModules(JournalMassModuleStore.ModuleItem[] items)
        {
            ShipInformation sm = null;
            foreach (var it in items)
            {
                if (Modules.ContainsKey(it.Slot))       // if has it..
                {
                    if (sm == null)
                        sm = this.ShallowClone();

                    //System.Diagnostics.Debug.WriteLine("Slot mass remove " + it.Slot + " Exists " + sm.Modules.ContainsKey(it.Slot));
                    sm.Modules.Remove(it.Slot);

                    if (it.Name.Contains("Fuel Tank") && it.Name.IndexOf("Class ") != -1)
                    {
                        sm.FuelCapacity = sm.GetFuelCapacity();
                        if (sm.FuelLevel > sm.FuelCapacity)
                            sm.FuelLevel = sm.FuelCapacity;
                    }
                }
            }

            return sm ?? this;
        }

        public ShipInformation SwapModule(string fromslot, string fromslotfd, string fromitem, string fromitemfd, string fromiteml,
                                          string toslot, string toslotfd, string toitem, string toitemfd, string toiteml)
        {
            ShipInformation sm = this.ShallowClone();
            if (Modules.ContainsKey(fromslot))
            {
                if (Modules.ContainsKey(toslot))
                {
                    sm.Modules[fromslot] = new ShipModule(fromslot, fromslotfd, toitem, toitemfd, toiteml);
                }
                else
                    sm.Modules.Remove(fromslot);

                sm.Modules[toslot] = new ShipModule(toslot, toslotfd, fromitem, fromitemfd, fromiteml);

                if (fromitem != toitem && ((fromitem.Contains("Fuel Tank") && fromitem.IndexOf("Class ") != -1) ||
                                           (fromitem.Contains("Fuel Tank") && fromitem.IndexOf("Class ") != -1)))
                {
                    sm.FuelCapacity = sm.GetFuelCapacity();
                    if (sm.FuelLevel > sm.FuelCapacity)
                        sm.FuelLevel = sm.FuelCapacity;
                }
            }
            return sm;
        }

        public ShipInformation Craft(string slot, string item, ShipModule.EngineeringData eng)
        {
            if (Modules.ContainsKey(slot) && Modules[slot].Item.Equals(item))       // craft, module must be there, otherwise just ignore
            {
                ShipInformation sm = this.ShallowClone();
                sm.Modules[slot] = new ShipModule(sm.Modules[slot]);        // clone
                sm.Modules[slot].SetEngineering(eng);                       // and update engineering
                return sm;
            }

            return this;
        }

        public ShipInformation SellShip()
        {
            ShipInformation sm = this.ShallowClone();
            sm.State = ShipState.Sold;
            sm.SubVehicle = SubVehicleType.None;
            sm.ClearStorage();
            return sm;
        }

        public ShipInformation Destroyed()
        {
            ShipInformation sm = this.ShallowClone();
            sm.State = ShipState.Destroyed;
            sm.SubVehicle = SubVehicleType.None;
            sm.ClearStorage();
            return sm;
        }

        public ShipInformation Store(string station, string system)
        {
            ShipInformation sm = this.ShallowClone();
            //if (sm.StoredAtSystem != null) { if (sm.StoredAtSystem.Equals(system)) System.Diagnostics.Debug.WriteLine("..Previous known stored at" + sm.StoredAtSystem + ":" + sm.StoredAtStation); else System.Diagnostics.Debug.WriteLine("************************ DISGREEE..Previous known stored at" + sm.StoredAtSystem + ":" + sm.StoredAtStation); }
            sm.SubVehicle = SubVehicleType.None;
            sm.StoredAtSystem = system;
            sm.StoredAtStation = station ?? sm.StoredAtStation;     // we may get one with just the system, so use the previous station if we have one
            //System.Diagnostics.Debug.WriteLine(".." + ShipFD + " Stored at " + sm.StoredAtSystem + ":" + sm.StoredAtStation);
            return sm;                                              // don't change transfer time as it may be in progress..
        }

        public ShipInformation SwapTo()
        {
            ShipInformation sm = this.ShallowClone();
            sm.ClearStorage();    // just in case
            return sm;
        }

        public ShipInformation Transfer(string tosystem , string tostation, DateTime arrivaltimeutc)
        {
            ShipInformation sm = this.ShallowClone();
            sm.StoredAtStation = tostation;
            sm.StoredAtSystem = tosystem;
            sm.TransferArrivalTimeUTC = arrivaltimeutc;
            return sm;
        }

        private void ClearStorage()
        {
            StoredAtStation = StoredAtSystem = null;
            TransferArrivalTimeUTC = DateTime.MinValue;
        }

        #endregion

        #region Export

        public bool CheckMinimumJSONModules()
        {
            // these are required slots..
            string[] requiredmodules = { "PowerPlant", "MainEngines", "FrameShiftDrive", "LifeSupport", "PowerDistributor", "Radar", "FuelTank", "Armour" };
            int reqmodules = 0;

            foreach (ShipModule sm in Modules.Values)
            {
                int index = Array.FindIndex(requiredmodules, x => x.Equals(sm.SlotFD));
                if (index >= 0)
                    reqmodules |= (1 << index);     // bit map them in, the old fashioned way
            }

            return (reqmodules == (1 << requiredmodules.Length) - 1);
        }

        public string ToJSONCoriolis(out string errstring)
        {
            errstring = "";

            JObject jo = new JObject();

            jo["event"] = "Loadout";
            jo["Ship"] = ShipFD;

            JArray mlist = new JArray();
            foreach (ShipModule sm in Modules.Values)
            {
                JObject module = new JObject();

                ShipModuleData.ShipModule si = ShipModuleData.Instance.GetItemProperties(sm.ItemFD);

                if (si.ModuleID == 0)
                {
                    errstring += sm.Item + ":" + sm.ItemFD + Environment.NewLine;
                }
                else
                {
                    module["Item"] = sm.ItemFD;
                    module["Slot"] = sm.SlotFD;
                    module["On"] = sm.Enabled.HasValue ? sm.Enabled : true;
                    module["Priority"] = sm.Priority.HasValue ? sm.Priority : 0;

                    if (sm.Engineering != null)
                        module["Engineering"] = ToJsonCoriolisEngineering(sm);

                    mlist.Add(module);
                }
            }

            jo["Modules"] = mlist;

            System.Diagnostics.Debug.WriteLine("Export " + jo.ToString(Newtonsoft.Json.Formatting.Indented));

            return jo.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        private JObject ToJsonCoriolisEngineering(ShipModule module)
        {
            JObject engineering = new JObject();

            engineering["BlueprintID"] = module.Engineering.BlueprintID;
            engineering["BlueprintName"] = module.Engineering.BlueprintName;
            engineering["Level"] = module.Engineering.Level;
            engineering["Quality"] = module.Engineering.Quality;

            JArray modifiers = new JArray();
            foreach (ShipModule.EngineeringModifiers modifier in module.Engineering.Modifiers)
            {
                JObject jmodifier = new JObject();
                jmodifier["Label"] = modifier.Label;
                jmodifier["Value"] = modifier.Value;
                jmodifier["OriginalValue"] = modifier.OriginalValue;
                jmodifier["LessIsGood"] = modifier.LessIsGood;
                modifiers.Add(jmodifier);
            }


            engineering["Modifiers"] = modifiers;
            engineering["ExperimentalEffect"] = module.Engineering.ExperimentalEffect;
            return engineering;
        }

        public string ToJSONLoadout()
        {
            JObject jo = new JObject();

            jo["timestamp"] = DateTime.UtcNow.ToStringZulu();
            jo["event"] = "Loadout";
            jo["Ship"] = ShipFD;
            if (!string.IsNullOrEmpty(ShipUserName))
                jo["ShipName"] = ShipUserName;
            if (!string.IsNullOrEmpty(ShipUserIdent))
                jo["ShipIdent"] = ShipUserIdent;
            if (Rebuy > 0)
                jo["Rebuy"] = Rebuy;
            if (HullValue > 0)
                jo["HullValue"] = HullValue;
            if (ModulesValue > 0)
                jo["ModulesValue"] = ModulesValue;

            JArray mlist = new JArray();

            foreach (ShipModule sm in Modules.Values)
            {
                JObject module = new JObject();

                module["Slot"] = sm.SlotFD;
                module["Item"] = sm.ItemFD;
                module["On"] = sm.Enabled.HasValue ? sm.Enabled : true;
                module["Priority"] = sm.Priority.HasValue ? sm.Priority : 0;
                if (sm.Value.HasValue)
                    module["Value"] = sm.Value;

                mlist.Add(module);
            }

            jo["Modules"] = mlist;

            return jo.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        #endregion
    }
}

