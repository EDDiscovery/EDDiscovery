/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
 */

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class SurveyorPanel : UserControlCommonBase
    {

        // Target, direct, no async
        private void DrawTarget()
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw target");

            string lasttargettext = "No Target";

            if (TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z))
            {
                if (cur_sys != null)     // double checky
                {
                    double dist = cur_sys.Distance(x, y, z);

                    string jumpstr = "";
                    if (shipfsdinfo != null)
                    {
                        int jumps = (int)Math.Ceiling(dist / shipfsdinfo.avgsinglejump);
                        if (jumps > 0)
                            jumpstr = jumps.ToString() + " " + ((jumps == 1) ? "jump".Tx() : "jumps".Tx());
                    }

                    lasttargettext = $"T-> {name} {dist:N1}ly {jumpstr}";
                }
                else
                    lasttargettext = "No known system";
            }

            ClearThenDrawText(extPictureBoxTarget, lasttargettext);
        }



        // Fuel, direct, no async
        private void DrawFuel()
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw fuel");

            string fueltext = "";

            if (shipinfo != null)
            {
                shipinfo.UpdateFuelWarningPercent();      // ensure its fresh from the DB

                double fuel = shipinfo.FuelLevel;
                double tanksize = shipinfo.FuelCapacity;
                double warninglevelpercent = shipinfo.FuelWarningPercent;

                fueltext = $"{fuel:N1}/{tanksize:N1}t";

                if (warninglevelpercent > 0 && fuel < tanksize * warninglevelpercent / 100.0)
                {
                    fueltext += $" < {warninglevelpercent:N1}%";
                }

                if (shipfsdinfo != null)
                {
                    fueltext += string.Format(" " + "Avg {0:N1}ly Fume {1:N1}ly Range {2:N1}ly".Tx(), shipfsdinfo.avgsinglejump, shipfsdinfo.curfumessinglejump, shipfsdinfo.maxjumprange);
                }
            }

            ClearThenDrawText(extPictureBoxFuel, fueltext);
        }

        // See if manual target needs updating
        private void CheckManualTarget()
        {
            if (currentRoute != null)
            {
                var knownsystems = currentRoute.SystemsWithCoordinates();

                if (currentRouteManualTarget >= 0 && currentRouteManualTarget < knownsystems.Count - 1)        // paranoia
                {
                    // if we are the target system, increment target
                    if (cur_sys.Name.EqualsIIC(knownsystems[currentRouteManualTarget].Item1.Name))
                    {
                        currentRouteManualTarget++;
                        System.Diagnostics.Debug.WriteLine($"Saved Route arrived at manual target {cur_sys.Name} Index {currentRouteManualTarget}");
                    }
                }
            }
        }


        private int currentRouteManualTarget = -1;      // current route manual target


    }
}

  