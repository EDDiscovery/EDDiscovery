/*
 * Copyright © 2019-2021 EDDiscovery development team
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

using QuickJSON;
using BaseUtils.WebServer;
using EliteDangerousCore;
using EliteDangerousCore.UIEvents;
using System.Net;

namespace EDDiscovery.WebServer
{
    public class IndicatorRequest : IJSONNode
    {
        UIOverallStatus uistate;

        public JToken Refresh(UIOverallStatus stat)     // request push of new states
        {
            uistate = stat;
            return NewIRec(stat, "indicatorpush");
        }

        public JToken Response(string key, JToken message, HttpListenerRequest request) // request indicator state
        {
            System.Diagnostics.Debug.WriteLine("indicator Request " + key + " Fields " + message.ToString());
            return NewIRec(uistate, "indicator");
        }

        //EliteDangerousCore.UIEvents.UIOverallStatus status,
        public JToken NewIRec(UIOverallStatus stat, string type)       // entry = -1 means latest
        {
            JObject response = new JObject();
            response["responsetype"] = type;

            if (stat == null) // because we don't have one
            {
                response["ShipType"] = "None";      // sending this clears the indicators
            }
            else
            {
                response["ShipType"] = stat.MajorMode.ToString();
                response["Mode"] = stat.Mode.ToString();
                response["GUIFocus"] = stat.Focus.ToString();

                JArray pips = new JArray();
                pips.Add(stat.Pips.Systems);
                pips.Add(stat.Pips.Engines);
                pips.Add(stat.Pips.Weapons);
                response["Pips"] = pips;
                response["ValidPips"] = stat.Pips.Valid;

                response["Lights"] = stat.Flags.Contains(UITypeEnum.Lights);
                response["Firegroup"] = stat.Firegroup;
                response["HasLatLong"] = stat.Flags.Contains(UITypeEnum.HasLatLong);

                JArray pos = new JArray();
                pos.Add(stat.Pos.Latitude);
                pos.Add(stat.Pos.Longitude);
                pos.Add(stat.Pos.Altitude);
                pos.Add(stat.Heading); // heading
                response["Position"] = pos;
                response["ValidPosition"] = stat.Pos.ValidPosition;
                response["ValidAltitude"] = stat.Pos.ValidAltitude;
                response["ValidHeading"] = stat.ValidHeading;
                response["AltitudeFromAverageRadius"] = stat.Pos.AltitudeFromAverageRadius;
                response["PlanetRadius"] = stat.PlanetRadius;
                response["ValidPlanetRadius"] = stat.ValidRadius;

                // main ship

                response["Docked"] = stat.Flags.Contains(UITypeEnum.Docked);       // S
                response["Landed"] = stat.Flags.Contains(UITypeEnum.Landed);  // S
                response["LandingGear"] = stat.Flags.Contains(UITypeEnum.LandingGear);   // S
                response["ShieldsUp"] = stat.Flags.Contains(UITypeEnum.ShieldsUp);         //S
                response["Supercruise"] = stat.Flags.Contains(UITypeEnum.Supercruise);   //S
                response["FlightAssist"] = stat.Flags.Contains(UITypeEnum.FlightAssist);     //S
                response["HardpointsDeployed"] = stat.Flags.Contains(UITypeEnum.HardpointsDeployed); //S
                response["InWing"] = stat.Flags.Contains(UITypeEnum.InWing);   // S
                response["CargoScoopDeployed"] = stat.Flags.Contains(UITypeEnum.CargoScoopDeployed);   // S
                response["SilentRunning"] = stat.Flags.Contains(UITypeEnum.SilentRunning);    // S
                response["ScoopingFuel"] = stat.Flags.Contains(UITypeEnum.ScoopingFuel);     // S

                // srv

                response["SrvHandbrake"] = stat.Flags.Contains(UITypeEnum.SrvHandbrake);
                response["SrvTurret"] = stat.Flags.Contains(UITypeEnum.SrvTurret);
                response["SrvUnderShip"] = stat.Flags.Contains(UITypeEnum.SrvUnderShip);
                response["SrvDriveAssist"] = stat.Flags.Contains(UITypeEnum.SrvDriveAssist);
                response["SrvHighBeam"] = stat.Flags.Contains(UITypeEnum.SrvHighBeam);

                // main ship
                response["FsdMassLocked"] = stat.Flags.Contains(UITypeEnum.FsdMassLocked);
                response["FsdCharging"] = stat.Flags.Contains(UITypeEnum.FsdCharging);
                response["FsdCooldown"] = stat.Flags.Contains(UITypeEnum.FsdCooldown);
                response["FsdJump"] = stat.Flags.Contains(UITypeEnum.FsdJump);

                // all ships

                response["LowFuel"] = stat.Flags.Contains(UITypeEnum.LowFuel);

                // main ship
                response["OverHeating"] = stat.Flags.Contains(UITypeEnum.OverHeating);
                response["IsInDanger"] = stat.Flags.Contains(UITypeEnum.IsInDanger);
                response["BeingInterdicted"] = stat.Flags.Contains(UITypeEnum.BeingInterdicted);
                response["HUDInAnalysisMode"] = stat.Flags.Contains(UITypeEnum.HUDInAnalysisMode);
                response["NightVision"] = stat.Flags.Contains(UITypeEnum.NightVision);

                response["GlideMode"] = stat.Flags.Contains(UITypeEnum.GlideMode);  // odyssey

                // all

                response["LegalState"] = stat.LegalState;
                response["BodyName"] = stat.BodyName;

                // Odyssey on foot
                response["AimDownSight"] = stat.Flags.Contains(UITypeEnum.AimDownSight);
                response["BreathableAtmosphere"] = stat.Flags.Contains(UITypeEnum.BreathableAtmosphere);

                response["Oxygen"] = stat.Oxygen;
                response["Gravity"] = stat.Gravity;
                response["Health"] = stat.Health;
                response["Temperature"] = stat.Temperature;
                response["TemperatureState"] = stat.TemperatureState.ToString();
                response["SelectedWeapon"] = stat.SelectedWeapon; ;
                response["SelectedWeaponLocalised"] = stat.SelectedWeapon_Localised;

            }

            return response;
        }
    }

}



