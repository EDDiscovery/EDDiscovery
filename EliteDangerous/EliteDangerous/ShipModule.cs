/*
 * Copyright © 2018 EDDiscovery development team
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

namespace EliteDangerousCore
{
    [System.Diagnostics.DebuggerDisplay("{Slot} {Item} {LocalisedItem}")]
    public class ShipModule
    {
        #region Information interface

        public string Slot { get; private set; }        // never null       - nice name, used to track 
        public string SlotFD { get; private set; }      // never null       - FD normalised ID name (Int_CargoRack_Size6_Class1)
        public string Item { get; private set; }        // never null       - nice name, used to track
        public string ItemFD { get; private set; }      // never null     - FD normalised ID name

        public string LocalisedItem { get; set; }       // Modulex events only supply this. so it may be null if we have not seen one of them pass by with this Item name

        public bool? Enabled { get; private set; }      // Loadout events, may be null
        public int? Priority { get; private set; }      // 0..4 not 1..5
        public int? Health { get; private set; }        //0-100
        public long? Value { get; private set; }

        public int? AmmoClip { get; private set; }
        public int? AmmoHopper { get; private set; }
        public double? Power { get; private set; }      // ONLY via Modules Info

        public EngineeringData Engineering { get; private set; }       // may be NULL if module is not engineered or unknown
               
        public string PE()
        {
            string pe = "";
            if (Priority.HasValue)
                pe = "P" + (Priority.Value + 1).ToString();
            if (Enabled.HasValue)
                pe += Enabled.Value ? "E" : "D";

            return pe;
        }

        public bool IsFSDSlot { get { return SlotFD.Contains("FrameShiftDrive"); } }

        public EliteDangerousCalculations.FSDSpec GetFSDSpec()      // may be null - not found
        {
            if (IsFSDSlot)
            {
                EliteDangerousCalculations.FSDSpec spec = EliteDangerousCalculations.FindFSD(ClassOf, RatingOf);

                if (spec != null)
                {
                    Engineering?.EngineerFSD(ref spec);
                    return spec;
                }
            }

            return null;
        }


        public string RatingOf              // null if not rated
        {
            get
            {
                int p = Item.IndexOf("Rating");
                if (p >= 0 && Item.Length > p + 7)
                    return Item[p + 7] + "";
                else
                    return null;
            }
        }

        public int ClassOf                  // null if not classed
        {
            get
            {
                int p = Item.IndexOf("Class");
                if (p >= 0 && Item.Length > p + 6)
                    return Item[p + 6] - '0';
                else
                    return 0;
            }
        }

        public double Mass
        {
            get
            {
                ShipModuleData.ShipModule smd = ShipModuleData.Instance.GetItemProperties(ItemFD);
                if (smd != null)
                {
                    double mass = smd.Mass;
                    Engineering?.EngineerMass(ref mass);
                    return mass;
                }
                else
                    return 0;
            }
        }

        public bool Same(ShipModule other)      // ignore localisased item, it does not occur everywhere..
        {
            bool engsame = Engineering != null ? Engineering.Same(other.Engineering) : (other.Engineering == null);     // if null, both null, else use the same func

            return (Slot == other.Slot && Item == other.Item && Enabled == other.Enabled &&
                     Priority == other.Priority && AmmoClip == other.AmmoClip && AmmoHopper == other.AmmoHopper &&
                     Health == other.Health && Value == other.Value && engsame);
        }

        #endregion

        #region Init

        public ShipModule()
        { }

        public ShipModule(string s, string sfd, string i, string ifd,
                        bool? e, int? prior, int? ac, int? ah, double? health, long? value,
                        double? power,
                        EngineeringData engineering)
        {
            Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd; Enabled = e; Priority = prior; AmmoClip = ac; AmmoHopper = ah;
            if (health.HasValue)
                Health = (int)(health * 100.0);
            Value = value;
            Power = power;
            Engineering = engineering;
        }

        public ShipModule( ShipModule other)
        {
            Slot = other.Slot; SlotFD = other.SlotFD; Item = other.Item; ItemFD = other.ItemFD;
            LocalisedItem = other.LocalisedItem;
            Enabled = other.Enabled; Priority = other.Priority; Health = other.Health; Value = other.Value;
            AmmoClip = other.AmmoClip; AmmoHopper = other.AmmoHopper; Power = other.Power;
            Engineering = other.Engineering;
        }

        public ShipModule(string s, string sfd, string i, string ifd, string l)
        {
            Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd; LocalisedItem = l;
        }

        public void SetEngineering( EngineeringData eng )
        {
            Engineering = eng;
        }

        #endregion

        #region Engineering

        public class EngineeringModifiers
        {
            public string Label { get; set; }
            public string FriendlyLabel { get; set; }
            public string ValueStr { get; set; }            // 3.02 if set, means ones further on do not apply. check first
            public double Value { get; set; }               // may be 0
            public double OriginalValue { get; set; }
            public bool LessIsGood { get; set; }
        }

        public class EngineeringData
        {
            public string Engineer { get; set; }
            public string BlueprintName { get; set; }
            public string FriendlyBlueprintName { get; set; }
            public long EngineerID { get; set; }
            public long BlueprintID { get; set; }
            public int Level { get; set; }
            public double Quality { get; set; }
            public string ExperimentalEffect { get; set; }
            public string ExperimentalEffect_Localised { get; set; }
            public EngineeringModifiers[] Modifiers { get; set; }       // may be null

            public EngineeringData(JObject evt)
            {
                Engineer = evt["Engineer"].Str();
                EngineerID = evt["EngineerID"].Long();
                BlueprintName = evt["BlueprintName"].Str();
                FriendlyBlueprintName = BlueprintName.SplitCapsWordFull();
                BlueprintID = evt["BlueprintID"].Long();
                Level = evt["Level"].Int();
                Quality = evt["Quality"].Double(0);
                // EngineerCraft has it as Apply.. Loadout has just ExperimentalEffect.  Check both
                ExperimentalEffect = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "ExperimentalEffect", "ApplyExperimentalEffect" });
                ExperimentalEffect_Localised = JournalFieldNaming.CheckLocalisation(evt["ExperimentalEffect_Localised"].Str(),ExperimentalEffect);

                Modifiers = evt["Modifiers"]?.ToObjectProtected<EngineeringModifiers[]>();

                if (Modifiers != null)
                {
                    foreach (EngineeringModifiers v in Modifiers)
                        v.FriendlyLabel = v.Label.SplitCapsWord();
                }
            }

            public override string ToString()
            {
                string ret = BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine, "Engineer:".Tx(this), Engineer, "Blueprint:".Tx(this), FriendlyBlueprintName,
                        "Level:".Tx(this), Level, "Quality:".Tx(this), Quality, "Experimental Effect:".Tx(this), ExperimentalEffect_Localised);

                if (Modifiers != null)
                {
                    ret += Environment.NewLine;

                    foreach (EngineeringModifiers m in Modifiers)
                    {
                        if (m.ValueStr != null)
                            ret += BaseUtils.FieldBuilder.Build("", m.Label, "<:", m.ValueStr) + Environment.NewLine;
                        else
                        {
                            if (m.Value != m.OriginalValue)
                            {
                                bool better = m.LessIsGood ? (m.Value < m.OriginalValue) : (m.Value > m.OriginalValue);
                                ret += BaseUtils.FieldBuilder.Build("", m.FriendlyLabel, "<:;;N2", m.Value, "Original:;;N2".Tx(this), m.OriginalValue, "< (Worse); (Better)".Tx(this), better) + Environment.NewLine;
                            }
                            else
                                ret += BaseUtils.FieldBuilder.Build("", m.FriendlyLabel, "<:;;N2", m.Value) + Environment.NewLine;
                        }
                    }
                }

                return ret;
            }

            public bool Same(EngineeringData other)
            {
                if (other == null || Engineer != other.Engineer || BlueprintName != other.BlueprintName || EngineerID != other.EngineerID || BlueprintID != other.BlueprintID
                    || Level != other.Level || Quality != other.Quality || ExperimentalEffect != other.ExperimentalEffect || ExperimentalEffect_Localised != other.ExperimentalEffect_Localised)
                {
                    return false;
                }
                else if (Modifiers != null || other.Modifiers != null)
                {
                    if (Modifiers == null || other.Modifiers == null || Modifiers.Length != other.Modifiers.Length)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < Modifiers.LongLength; i++)
                        {
                            if (Modifiers[i].Label != other.Modifiers[i].Label || Modifiers[i].ValueStr != other.Modifiers[i].ValueStr ||
                                Modifiers[i].Value != other.Modifiers[i].Value || Modifiers[i].OriginalValue != other.Modifiers[i].OriginalValue || Modifiers[i].LessIsGood != other.Modifiers[i].LessIsGood)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }


            public EngineeringModifiers FindModification(string name)
            {
                return Modifiers != null ? Array.Find(Modifiers, x => x.Label.Equals(name, StringComparison.InvariantCultureIgnoreCase)) : null;
            }

            public bool EngineerMass(ref double mass)               // perform mass engineering
            {
                EngineeringModifiers mod = FindModification("Mass");
                if (mod != null)
                {
                    mass = mod.Value;
                    return true;
                }
                else
                    return false;
            }

            public bool EngineerFSD(ref EliteDangerousCalculations.FSDSpec spec)               // perform FSD
            {
                bool done = false;

                EngineeringModifiers mod = FindModification("FSDOptimalMass");
                if (mod != null)
                {
                    spec.OptimalMass = mod.Value;
                    done = true;
                }

                mod = FindModification("MaxFuelPerJump");
                if (mod != null)
                {
                    spec.MaxFuelPerJump = mod.Value;
                    done = true;
                }

                return done;
            }

            public bool EngineerThrusters(ref double speed)         
            {
                EngineeringModifiers mod = FindModification("EngineOptPerformance");
                if (mod != null)
                {
                    speed *= mod.Value/100.0;
                    return true;
                }
                else
                    return false;
            }

        }

        #endregion
    }
}
