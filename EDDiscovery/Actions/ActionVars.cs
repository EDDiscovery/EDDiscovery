/*
 * Copyright © 2017 EDDiscovery development team
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
using BaseUtils;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    class ActionVars
    {
        static public void TriggerVars(Variables vars, string trigname, string triggertype)
        {
            vars["TriggerName"] = trigname;       // Program gets eventname which triggered it.. (onRefresh, LoadGame..)
            vars["TriggerType"] = triggertype;       // type (onRefresh, or OnNew, or ProgramTrigger for all others)
            vars["TriggerLocalTime"] = DateTime.Now.ToStringUSInvariant();  // time it was started, US format, to match JSON.
            vars["TriggerUTCTime"] = DateTime.UtcNow.ToStringUSInvariant();  // time it was started, US format, to match JSON.
        }

        static public void HistoryEventVars(Variables vars, HistoryEntry he, string prefix)
        {
            if (he != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                vars[prefix + "CurrentMode"] = he.TravelState.ToString();
                vars[prefix + "JID"] = he.Journalid.ToString(ct);
                vars[prefix + "UTCTime"] = he.EventTimeUTC.ToStringUSInvariant();
                vars[prefix + "LocalTime"] = he.EventTimeUTC.ToLocalTime().ToStringUSInvariant();
                vars[prefix + "GameTime"] = he.EventTimeUTC.AddYears(1286).ToStringUSInvariant();
                vars[prefix + "DockedState"] = he.IsDocked.ToStringIntValue();
                vars[prefix + "LandedState"] = he.IsLanded.ToStringIntValue();
                vars[prefix + "Hyperspace"] = he.IsInHyperSpace.ToStringIntValue();
                vars[prefix + "WhereAmI"] = he.WhereAmI ?? "";
                vars[prefix + "BodyType"] = he.Status.BodyType ?? "";
                vars[prefix + "BodyName"] = he.Status.BodyName ?? "";
                vars[prefix + "BodyID"] = (he.Status.BodyID ?? -1).ToStringInvariant();
                vars[prefix + "StationName"] = he.Status.StationName ?? "";
                vars[prefix + "StationType"] = he.Status.StationType ?? "";
                vars[prefix + "StationFaction"] = he.Status.StationFaction ?? "";
                vars[prefix + "ShipType"] = he.Status.ShipType ?? "";
                vars[prefix + "ShipTypeFD"] = he.Status.ShipTypeFD ?? "";
                vars[prefix + "OnFoot"] = he.Status.OnFoot.ToStringIntValue();
                vars[prefix + "IsSRV"] = he.Status.IsSRV.ToStringIntValue();
                vars[prefix + "IsFighter"] = he.Status.IsFighter.ToStringIntValue();
                vars[prefix + "BodyApproached"] = he.Status.BodyApproached.ToStringIntValue();
                vars[prefix + "BookedDropship"] = he.Status.BookedDropship.ToStringIntValue();
                vars[prefix + "BookedTaxi"] = he.Status.BookedTaxi.ToStringIntValue();
                vars[prefix + "ShipId"] = he.Status.ShipID.ToString(ct);
                vars[prefix + "IndexOf"] = he.EntryNumber.ToString(ct);

                vars[prefix + "Credits"] = he.Credits.ToString(ct);

                vars[prefix + "TravelledDistance"] = he.TravelledDistance.ToStringInvariant("0.0");
                vars[prefix + "TravelledSeconds"] = he.TravelledSeconds.ToString();
                vars[prefix + "IsTravelling"] = he.isTravelling.ToStringIntValue();
                vars[prefix + "TravelledJumps"] = he.Travelledjumps.ToStringInvariant();
                vars[prefix + "TravelledMissingJumps"] = he.TravelledMissingjump.ToStringInvariant();
                vars[prefix + "MultiPlayer"] = he.MultiPlayer.ToStringIntValue();
                vars[prefix + "EventSummary"] = he.EventSummary;

                vars[prefix + "StartMarker"] = he.StartMarker.ToStringIntValue();
                vars[prefix + "StopMarker"] = he.StopMarker.ToStringIntValue();
                vars[prefix + "EdsmSync"] = he.EdsmSync.ToStringIntValue();
                vars[prefix + "EddnSync"] = he.EDDNSync.ToStringIntValue();
                vars[prefix + "EgoSync"] = false.ToStringIntValue();        //removed due to no EGO
                vars[prefix + "Beta"] = he.journalEntry.IsBeta.ToStringIntValue();
                vars[prefix + "Horizons"] = he.journalEntry.IsHorizons.ToStringIntValue();
                vars[prefix + "Odyssey"] = he.journalEntry.IsOdyssey.ToStringIntValue();
                vars[prefix + "GameMode"] = he.GameMode;
                vars[prefix + "Group"] = he.Group;
                vars[prefix + "OnCrewWithCaptain"] = he.Status.OnCrewWithCaptain ?? "";
                vars[prefix + "Wanted"] = he.Wanted.ToStringIntValue();
                vars[prefix + "MarketId"] = he.MarketID.HasValue ? he.MarketID.ToStringInvariant() : "0";

                vars[prefix + "Note"] = he.GetNoteText;

                he.FillInformation(out string EventDescription, out string EventDetailedInfo);
                vars[prefix + "EventDescription"] = EventDescription;
                vars[prefix + "EventDetailedInfo"] = EventDetailedInfo;

                vars.AddPropertiesFieldsOfClass(he.journalEntry, prefix + "Class_", new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) }, 5);      //depth seems good enough

                // being backwards compatible to actions packs BEFORE the V3 change to remove JS vars
                // these were the ones used in the pack..

                vars[prefix + "JS_event"] = he.EntryType.ToString();        
                if (he.journalEntry is EliteDangerousCore.JournalEvents.JournalReceiveText)
                    vars[prefix + "JS_Channel"] = (he.journalEntry as EliteDangerousCore.JournalEvents.JournalReceiveText).Channel;
                if (he.journalEntry is EliteDangerousCore.JournalEvents.JournalBuyAmmo)
                    vars[prefix + "JS_Cost"] = (he.journalEntry as EliteDangerousCore.JournalEvents.JournalBuyAmmo).Cost.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        static public void SystemVars(Variables vars, ISystem s, string prefix)
        {
            if (s != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                vars[prefix + "StarSystem"] = s.Name;
                vars[prefix + "StarSystemEDSMID"] = s.EDSMID.ToString(ct);
                vars[prefix + "xpos"] = s.X.ToNANSafeString("0.###");
                vars[prefix + "ypos"] = s.Y.ToNANSafeString("0.###");
                vars[prefix + "zpos"] = s.Z.ToNANSafeString("0.###");
            }
        }


        static public void ShipBasicInformation(Variables vars, ShipInformation si, string prefix)
        {
            string ship = "Unknown", id = "0", name = "Unknown", ident = "Unknown", sv = "None", fullinfo = "Unknown", shortname = "Unknown", fuel = "0", cargo = "0", fuellevel = "0";

            if (si != null)
            {
                ship = si.ShipType.Alt("Unknown");
                id = si.ID.ToString(System.Globalization.CultureInfo.InvariantCulture);
                name = si.ShipUserName.Alt("");
                ident = si.ShipUserIdent.Alt("");
                sv = si.SubVehicle.ToString();
                fullinfo = si.ShipFullInfo();
                shortname = si.ShipShortName.Alt("Unknown");
                fuel = si.FuelCapacity.ToStringInvariant("0.0");
                fuellevel = si.FuelLevel.ToStringInvariant("0.0");
                cargo = si.CargoCapacity().ToStringInvariant();
            }

            vars[prefix + "Ship"] = ship;                   // need to be backwards compatible with older entries..
            vars[prefix + "Ship_ID"] = id;
            vars[prefix + "Ship_Name"] = name;
            vars[prefix + "Ship_Ident"] = ident;
            vars[prefix + "Ship_SubVehicle"] = sv;
            vars[prefix + "Ship_FullInfo"] = fullinfo;
            vars[prefix + "Ship_ShortName"] = shortname;
            vars[prefix + "Ship_FuelLevel"] = fuellevel;
            vars[prefix + "Ship_FuelCapacity"] = fuel;
            vars[prefix + "Ship_CargoCapacity"] = cargo;
        }

        // expensive. Scan command, Event Info, 
        static public void SystemVarsFurtherInfo(ActionLanguage.ActionProgramRun vars, HistoryList hl, ISystem s, string prefix)
        {
            System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

            vars[prefix + "VisitCount"] = hl.GetVisitsCount(s.Name).ToString(ct);
            // removed due to load in V21 (11.9.4+) vars[prefix + "ScanCount"] = hl.GetScans(s.Name).Count.ToString(ct); vars[prefix + "FSDJumpsTotal"] = hl.GetFSDCarrierJumps(new TimeSpan(100000, 0, 0, 0)).ToString(ct);
        }

        static public void ShipModuleInformation(ActionLanguage.ActionProgramRun vars, ShipInformation si, string prefix)
        {
            if (si != null && si.Modules != null)
            {
                vars[prefix + "Ship_Module_Count"] = si.Modules.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);

                int ind = 0;
                foreach (ShipModule m in si.Modules.Values)
                {
                    string mi = prefix + "Ship_Module[" + ind.ToString() + "]_";
                    vars[mi + "Slot"] = m.Slot;
                    vars[mi + "Item"] = m.Item;
                    vars[mi + "ItemLocalised"] = m.LocalisedItem.Alt(m.Item);
                    vars[mi + "Enabled"] = m.Enabled.ToStringInvariant();
                    vars[mi + "AmmoClip"] = m.AmmoClip.ToStringInvariant();
                    vars[mi + "AmmoHopper"] = m.AmmoHopper.ToStringInvariant();
                    vars[mi + "Blueprint"] = (m.Engineering != null) ? m.Engineering.FriendlyBlueprintName : "";
                    vars[mi + "Health"] = m.Health.ToStringInvariant();
                    vars[mi + "Value"] = m.Value.ToStringInvariant();
                    vars[mi + "Mass"] = m.Mass.ToStringInvariant();
                    ind++;
                }
            }
        }


        static public void MissionInformation(ActionLanguage.ActionProgramRun vars, List<MissionState> ml, string prefix)
        {
            vars[prefix + "_MissionCount"] = ml.Count.ToStringInvariant();

            int i = 0;
            foreach (MissionState ms in ml)
            {
                string mp = prefix + "Mission[" + i.ToStringInvariant() +"]_";

                vars[mp + "Name"] = ms.Mission.Name;
                vars[mp + "NameLocalised"] = ms.Mission.LocalisedName;
                vars[mp + "ID"] = ms.Mission.MissionId.ToStringInvariant();
                vars[mp + "UTC"] = ms.Mission.EventTimeUTC.ToStringUSInvariant();
                vars[mp + "Local"] = ms.Mission.EventTimeLocal.ToStringUSInvariant();
                vars[mp + "ExpiryUTC"] = ms.Mission.Expiry.ToStringUSInvariant();
                vars[mp + "ExpiryLocal"] = ms.Mission.Expiry.ToLocalTime().ToStringUSInvariant();
                vars[mp + "System"] = ms.OriginatingSystem;
                vars[mp + "Station"] = ms.OriginatingStation;
                vars[mp + "Faction"] = ms.Mission.Faction;
                vars[mp + "DestSystem"] = ms.Mission.DestinationSystem;
                vars[mp + "DestStation"] = ms.Mission.DestinationStation;
                vars[mp + "DestSettlement"] = ms.Mission.DestinationSettlement;
                vars[mp + "Influence"] = ms.Mission.Influence;
                vars[mp + "Reputation"] = ms.Mission.Reputation;
                vars[mp + "Commodity"] = ms.Mission.CommodityLocalised.Alt(ms.Mission.FriendlyCommodity);

                vars[mp + "TargetType"] = ms.Mission.TargetType;
                vars[mp + "TargetTypeFriendly"] = ms.Mission.TargetTypeFriendly;
                vars[mp + "TargetTypeLocalised"] = ms.Mission.TargetTypeLocalised;
                vars[mp + "TargetFaction"] = ms.Mission.TargetFaction;
                vars[mp + "Target"] = ms.Mission.Target;
                vars[mp + "TargetFriendly"] = ms.Mission.TargetFriendly;
                vars[mp + "TargetLocalised"] = ms.Mission.TargetLocalised;
                vars[mp + "KillCount"] = ms.Mission.KillCount != null ? ms.Mission.KillCount.Value.ToStringInvariant() : "";

                vars[mp + "Passengers"] = ms.Mission.PassengerCount.ToStringInvariant();

                vars[mp + "Completed"] = (ms.Completed != null).ToStringIntValue();
                if (ms.Completed != null)
                {
                    vars[mp + "Reward"] = ms.Completed.Reward.ToStringInvariant();
                    vars[mp + "Donation"] = ms.Completed.Donation.ToStringInvariant();
                    vars[mp + "RewardCommodity"] = ms.Completed.CommoditiesList(false,false);
                    vars[mp + "RewardPermit"] = ms.Completed.PermitsList(false, false);
                    vars[mp + "RewardMaterials"] = ms.Completed.MaterialList(false, false);
                }
                i++;
            }
        }


    }
}

