/*
 * Copyright © 2024 - 2024 EDDiscovery development team
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
using QuickJSON;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    { 
        private void PythonStart()
        {
            string pythonfolder = EDDOptions.Instance.PythonDirectory();

            foreach (var plugin in Directory.GetDirectories(pythonfolder))  // all folders in python folder
            {
                string pluginfolder = Path.Combine(pythonfolder, plugin);
                string optfile = Path.Combine(pluginfolder, "config.json");
                string optcontents;
                JToken cf;

                // check we can read the config
                if (File.Exists(optfile) && (optcontents = BaseUtils.FileHelpers.TryReadAllTextFromFile(optfile)) != null &&
                        (cf = JToken.Parse(optcontents, JToken.ParseOptions.CheckEOL | JToken.ParseOptions.AllowTrailingCommas)) != null )
                {
                    JObject panel = cf["Panel"].Object();
                    if (panel != null)
                    {
                        string id = panel["ID"].Str();      // name as known to EDD Panel plugins, must be unique
                        string wintitle = panel["WinTitle"].Str();      // name as known to EDD Panel plugins, must be unique
                        string refname = panel["RefName"].Str();      // name as known to EDD Panel plugins, must be unique
                        string description = panel["Description"].Str();      // name as known to EDD Panel plugins, must be unique
                        string icon = panel["Icon"].Str();      // icon

                        if (id != null && wintitle != null && refname != null && description != null && icon != null && File.Exists(Path.Combine(pluginfolder,icon)))
                        {
                            Image image = Image.FromFile(Path.Combine(pluginfolder,icon));

                            // registered panels, search the stored list, see if there, then it gets the index, else its added to the list
                            int panelid = EDDConfig.Instance.FindAddUserPanelID(id);

                            PanelInformation.AddPanel(panelid,
                                                typeof(UserControls.UserControlPythonPanel),       // driver panel containing the UC to draw into, responsible for running action scripts/talking to the plugin
                                                pluginfolder,
                                                wintitle, refname, description, image);
                        }
                        else
                            System.Diagnostics.Trace.WriteLine($"Python folder {pluginfolder} config missing items to create panel");
                    }
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine($"Python folder {pluginfolder} missing items to create panel");
                }
            }
        }

    }
}
