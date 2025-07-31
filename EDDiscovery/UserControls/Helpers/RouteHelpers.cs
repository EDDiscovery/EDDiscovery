/*
 * Copyright © 2017-2023 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EDDiscovery.UserControls.UserControlCommonBase;

namespace EDDiscovery.UserControls
{ 
    class RouteHelpers
    {
        public static void ExpeditionSave(Form frm, string exampleroutename, List<ISystem> routeSystems)
        {
            string name = ExtendedControls.PromptSingleLine.ShowDialog(frm, "Route Name:".Tx(), exampleroutename,
                            "Save Expedition".Tx(), frm.Icon, widthboxes: 400, requireinput:true);

            if (name != null)
            {
                var savedroutes = EliteDangerousCore.DB.SavedRouteClass.GetAllSavedRoutes();
                var overwriteroute = savedroutes.Where(r => r.Name.Equals(name)).FirstOrDefault();

                if (overwriteroute != null)
                {
                    if (ExtendedControls.MessageBoxTheme.Show(frm, "Warning: route already exists. Would you like to overwrite it?".Tx(), "Warning".Tx(), MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    overwriteroute.Delete();
                }

                EliteDangerousCore.DB.SavedRouteClass newrt = new EliteDangerousCore.DB.SavedRouteClass(name);

                foreach (var sys in routeSystems)
                {
                    var entry = new EliteDangerousCore.DB.SavedRouteClass.SystemEntry(sys);
                    newrt.Systems.Add(entry);
                }

                newrt.Add();
            }
        }

        public static void ExpeditionPush(string exampleroutename, List<ISystem> routeSystems, UserControlCommonBase ucb, EDDiscoveryForm form)
        {
            var req = new UserControlCommonBase.PushStars() { PushTo = PushStars.PushType.Expedition, SystemList = routeSystems, MakeVisible = true, RouteTitle = exampleroutename };

            bool serviced = ucb.RequestPanelOperation.Invoke(ucb, req) != PanelActionState.NotHandled;

            if (!serviced) // no-one serviced it, so create an expedition tab, and then reissue
            {
                form.SelectTabPage("Expedition", true, false);         // ensure expedition is open
                ucb.RequestPanelOperation.Invoke(ucb, req);
            }
        }

        public static void Open3DMap(List<ISystem> routeSystems, EDDiscoveryForm form)
        {
            if (routeSystems != null && routeSystems.Any())
            {
                form.Open3DMap(routeSystems.First(), routeSystems);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(form, "No route set up, retry".Tx(), "Warning".Tx(), MessageBoxButtons.OK);
                return;
            }
        }

    }
}
