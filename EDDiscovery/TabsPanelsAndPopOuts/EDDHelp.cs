/*
 * Copyright 2018-2025 EDDiscovery development team
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

using QuickJSON;
using ExtendedControls;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace EDDiscovery
{
    public static class EDDHelp
    {
        static Dictionary<string, string> NamesToVideos = new Dictionary<string, string> 
        {
            ["4. Log Panel"] = "https://youtu.be/PwTbnFikBgA?t=625",
            ["4. Nearest Stars Panel"] = "https://youtu.be/PwTbnFikBgA?t=590",
            ["4. Materials Panel"] = "https://youtu.be/U1id5TxS8bs",
            ["4. Commodities Panel"] = "https://youtu.be/U1id5TxS8bs",
            ["4. History Grid Panel"] = "https://youtu.be/PwTbnFikBgA?t=56",
            ["4. Scan Panel"] = "https://youtu.be/PwTbnFikBgA?t=636",
            ["4. Synthesis Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. Engineering Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. System Information Panel"] = "https://youtu.be/PwTbnFikBgA?t=520",
            ["4. Surveyor Panel"] = "https://youtu.be/dcuNn4o7gJ4",
            ["4. Grid Panel"] = "https://youtu.be/fSnxTDL90B4?t=346",
            ["4. Search Panel"] = "https://youtu.be/GZGVuimjEeI",
            ["4. Shopping List Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. Engineering Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. Systhesis Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. Route Panel"] = "https://youtu.be/zw_RlzX0yn4",
            ["1.2 Settings Panel"] = "https://youtu.be/v5g03mYdYAw",
            ["4. Compass Panel"] = "https://youtu.be/s-AVEYq5vCo",
            ["4. Splitter Control Panel"] = "https://youtu.be/fSnxTDL90B4?t=209",
            ["4. Surveyor Panel"] = "https://youtu.be/dcuNn4o7gJ4",
            ["4. Mining Overlay Panel"] = "https://youtu.be/D-U8euLWbKU",
            ["4. Spansh Panel"] = "https://youtu.be/qeLnq-pU14I",
            ["4. Micro Resources Panel"] = "https://youtu.be/U1id5TxS8bs",
            ["4. Suits Weapons Panel"] = "https://youtu.be/qKyhIXRfrAE",
            ["4. Map 3D Panel"] = "https://youtu.be/9c09d93zFB8",
            ["4. Local Map 3D Panel"] = "https://youtu.be/9c09d93zFB8",
            ["4. Organics Panel"] = "https://youtu.be/Uhw4zHnsorI",
            ["4. Engineers Panel"] = "https://youtu.be/gI6rKmgqGb0",
            ["4. Discoveries Panel"] = "https://youtu.be/GZGVuimjEeI",
            ["4. Carrier Panel"] = "https://youtu.be/zot3a7uHQfQ",
            ["4. Resources Panel"] = "https://youtu.be/U1id5TxS8bs",
            ["4. Spansh Stations Panel"] = "https://youtu.be/pivLR8FzpmY",
            ["1.6 History Tab"] = "https://youtu.be/PwTbnFikBgA",
            ["1.2 Settings Panel#Web Server"] = "https://youtu.be/GkOGB7WF1Lo?t=140",
        };

        // Used by Panels.
        // The caller has converted in HelpKeyOrAddress the panel ID into "X Y Panel" but has not corrected for some ancient naming 
        // issues below.
        // this function knows about the layout of the wiki, specifically mapping some panelid to wiki names, the 1.2 settings panel
        // ajnd the 4. for panels
        public static void HelpPanel(Form parent, Point pos, string name, string bookmark = null)
        {
            if (name == "Settings Panel")
            {
                name = "1.2 Settings Panel";
            }
            else
            {
                if (name == "Star Distance Panel")
                    name = "Nearest Stars Panel";
                else if (name == "Travel Grid Panel")
                    name = "History Grid Panel";
                else if (name == "Modules Panel")
                    name = "Ships Load Out Panel";
                
                if ( name.Contains(" Panel"))       // .. Panels are always in section 4 of the wiki
                    name = "4. " + name;
            }

            HelpName(parent, pos, name, bookmark);
        }

        // launch an HTTP or a wiki/video,
        // wikiname is the launch name with an optional bookmark
        // video is picked up from above list vs this name/bookmark combo

        public static void HelpName(Form parent, Point pos, string wikiname, string bookmark = null)
        {
            if (wikiname.StartsWith("http"))        // direct launch
            {
                BaseUtils.BrowserInfo.LaunchBrowser(wikiname);
            }
            else
            {
                wikiname += (bookmark != null ? "#" + bookmark : "");       // add bookmark

                NamesToVideos.TryGetValue(wikiname, out string video);      // see if video

                string wiki = Properties.Resources.URLProjectWiki + "/" + wikiname.Replace(" ","-");    // convert to URI

                System.Diagnostics.Debug.WriteLine($"Help on {wikiname} # {bookmark} at `{wiki}` video {video}");

                if (video.HasChars())
                {
                    ConfigurableForm cfg = new ExtendedControls.ConfigurableForm();
                    cfg.AllowSpaceForScrollBar = false;
                    cfg.RightMargin = cfg.BottomMargin = 0;
                    cfg.ForceNoWindowsBorder = true;
                    cfg.AllowSpaceForCloseButton = true;
                    cfg.BorderMargin = 0;
                    cfg.ExtraMarginRightBottom = new Size(0, 0);

                    ExtButton wikibutton = new ExtButton();
                    wikibutton.Image = global::EDDiscovery.Icons.Controls.Wiki;
                    cfg.Add(new ConfigurableEntryList.Entry(wikibutton, "Wiki", null, new Point(0, 0), new Size(24, 24), null));
                    ExtButton videobutton = new ExtButton();
                    videobutton.Image = global::EDDiscovery.Icons.Controls.Video;
                    cfg.Add(new ConfigurableEntryList.Entry(videobutton, "Video", null, new Point(26, 0), new Size(24, 24), null));

                    cfg.Trigger += (string logicalname, string ctrlname, object callertag) =>
                    {
                        if (ctrlname == "Close")
                            cfg.ReturnResult(DialogResult.Cancel);
                        else if (ctrlname == "Wiki")
                            cfg.ReturnResult(DialogResult.OK);
                        else if (ctrlname == "Video")
                            cfg.ReturnResult(DialogResult.Yes);
                    };

                    DialogResult res = cfg.ShowDialog(parent, pos, parent.Icon, "", closeicon: true);
                    if (res == DialogResult.OK)
                        BaseUtils.BrowserInfo.LaunchBrowser(wiki);
                    else if (res == DialogResult.Yes)
                        BaseUtils.BrowserInfo.LaunchBrowser(video);
                }
                else
                    BaseUtils.BrowserInfo.LaunchBrowser(wiki);
            }

        }
    }
}
