/*
 * Copyright 2019-2021 Robbyxp1 @ github.com
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

using EliteDangerousCore.EDSM;
using GLOFC.GL4.Controls;
using System.Collections.Generic;
using System.Drawing;
using static GLOFC.GL4.Controls.GLBaseControl;

namespace EDDiscovery.UserControls.Map3D
{
    public class MapMenu
    {
        private Map map;
        private GLLabel status;
        private const int iconsize = 32;
        public GLTextBoxAutoComplete EntryTextBox { get; private set; }
        public GLImage DBStatus { get; private set; }

        public MapMenu(Map g, Map.Parts parts)
        {
            map = g;

            // names of MS* are on screen items hidden during main menu presentation

            int leftmargin = 4;
            int vpos = 4;
            int hpad = 8;
            int hpos = leftmargin;

            GLImage menuimage = new GLImage("MSMainMenu", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.hamburgermenu") );
            menuimage.ToolTipText = "Open configuration menu";
            map.displaycontrol.Add(menuimage);
            menuimage.MouseClick = (o, e1) => { ShowMenu(parts); };
            hpos += menuimage.Width + hpad;
            
            if ((parts & Map.Parts.TravelPath) != 0)
            {
                GLImage tpback = new GLImage("MSTPBack", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GoBackward") );
                tpback.ToolTipText = "Go back one system";
                map.displaycontrol.Add(tpback);
                tpback.MouseClick = (o, e1) => { g.GoToTravelSystem(-1); };
                hpos += iconsize + hpad;

                GLImage tphome = new GLImage("MSTPHome", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GoToHomeSystem") );
                tphome.ToolTipText = "Go to current system";
                map.displaycontrol.Add(tphome);
                tphome.MouseClick = (o, e1) => { g.GoToCurrentSystem(); };
                hpos += iconsize + hpad;

                GLImage tpforward = new GLImage("MSTPForward", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GoForward") );
                tpforward.ToolTipText = "Go forward one system";
                map.displaycontrol.Add(tpforward);
                tpforward.MouseClick = (o, e1) => { g.GoToTravelSystem(1); };
                hpos += iconsize + hpad;
            }

            if ((parts & Map.Parts.GalaxyResetPos) != 0)
            {
                GLImage tpgalview = new GLImage("MSTPGalaxy", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowGalaxy") );
                tpgalview.ToolTipText = "View Galaxy";
                map.displaycontrol.Add(tpgalview);
                tpgalview.MouseClick = (o, e1) => { g.ViewGalaxy(); };
                hpos += iconsize + hpad;
            }

            if ((parts & Map.Parts.Bookmarks) != 0)
            {
                GLCheckBox butbkmks = new GLCheckBox("MSTPBookmarks", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowBookmarks"), null);
                butbkmks.ToolTipText = "Show bookmark list";
                butbkmks.CheckOnClick = true;
                butbkmks.CheckChanged += (e1) => { g.ToggleBookmarkList(butbkmks.Checked); };
                map.displaycontrol.Add(butbkmks);
                hpos += butbkmks.Width + hpad;
            }

            if ((parts & Map.Parts.SearchBox) != 0)
            {
                EntryTextBox = new GLTextBoxAutoComplete("MSTPEntryText", new Rectangle(hpos, vpos, 300, iconsize), "");
                EntryTextBox.TextAlign = ContentAlignment.MiddleLeft;
                EntryTextBox.BackColor = Color.FromArgb(96, 50, 50, 50);
                EntryTextBox.BorderColor = Color.Gray;
                EntryTextBox.BorderWidth = 1;
                map.displaycontrol.Add(EntryTextBox);
                hpos += EntryTextBox.Width + hpad;
            }

            if ((parts & Map.Parts.PrepopulateEDSMLocalArea) != 0)
            {
                GLImage butpopstars = new GLImage("MSPopulate", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowMoreStars"));
                butpopstars.ToolTipText = "Load star box at current look location";
                butpopstars.MouseClick = (o, e1) => { g.AddMoreStarsAtLookat(); };
                map.displaycontrol.Add(butpopstars);
                hpos += butpopstars.Width + hpad;
            }


            DBStatus = new GLImage("MSDB", new Rectangle(hpos,vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.db") );
            DBStatus.Dock = DockingType.BottomRight;
            DBStatus.ToolTipText = "Reading from DB";
            DBStatus.Visible = false;
            map.displaycontrol.Add(DBStatus);

            status = new GLLabel("Status", new Rectangle(leftmargin, 500, 600, 24), "x");
            status.Dock = DockingType.BottomLeft;
            status.ForeColor = Color.Orange;
            status.BackColor = Color.FromArgb(50, 50, 50, 50);
            map.displaycontrol.Add(status);

            GLToolTip maintooltip = new GLToolTip("MTT", Color.FromArgb(180, 50, 50, 50));
            maintooltip.ForeColor = Color.Orange;
            map.displaycontrol.Add(maintooltip);

            // detect mouse press with menu open and close it
            map.displaycontrol.GlobalMouseDown += (ctrl, e) =>
            {
                // if map open, and no ctrl hit or ctrl is not a child of galmenu

                if (map.displaycontrol["Galmenu"]!= null && (ctrl == null || !map.displaycontrol["Galmenu"].IsThisOrChildOf(ctrl)))       
                {
                    ((GLForm)map.displaycontrol["Galmenu"]).Close();
                }
            };

        }

        // on menu button..
        public void ShowMenu(Map.Parts parts)
        {
            //map.displaycontrol.ApplyToControlOfName("InfoBoxForm*", (c) => { ((GLForm)c).Close(); });      // close any info box forms (don't want to I think)

            map.displaycontrol.ApplyToControlOfName("MS*", (c) => { c.Visible = false; });      // hide the visiblity of the on screen controls

            int leftmargin = 4;
            int vpos = 10;
            int hpad = 8;
            int ypad = 10;

            GLForm pform = new GLForm("Galmenu", "Configure Map", new Rectangle(10, 10, 500, 600));
            pform.FormClosed = (frm) => { map.displaycontrol.ApplyToControlOfName("MS*", (c) => { c.Visible = true; }); };
            pform.Resizeable = pform.Moveable = false;

            // provide opening animation
            pform.ScaleWindow = new SizeF(0.0f, 0.0f);
            pform.Animators.Add(new GLControlAnimateScale(10, 400, true, new SizeF(1, 1)));
            pform.Font = new Font("Arial", 10f);

            // and closing animation
            pform.FormClosing += (f,e) => {
                var nb = ((GLNumberBoxLong)pform["GalaxyStarsGB"]?["LYDist"]);;
                if ( nb != null)
                    map.LocalAreaSize = (int)nb.Value;
                e.Handled = true;       // stop close
                var ani = new GLControlAnimateScale(10, 400, true, new SizeF(0, 0));       // add a close animation
                ani.FinishAction += (a, c, t) => { pform.ForceClose(); };   // when its complete, force close
                pform.Animators.Add(ani); 
            };

            {   // top buttons
                int hpos = 0;

                if ((parts & Map.Parts.PerspectiveChange) != 0)
                {
                    GLPanel p3d2d = new GLPanel("3d2d", new Rectangle(leftmargin, vpos, 80, iconsize), Color.Transparent);

                    GLCheckBox but3d = new GLCheckBox("3d", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.3d") , null);
                    but3d.Checked = map.gl3dcontroller.MatrixCalc.InPerspectiveMode;
                    but3d.ToolTipText = "3D View";
                    but3d.GroupRadioButton = true;
                    but3d.MouseClick += (e1, e2) => { map.gl3dcontroller.ChangePerspectiveMode(true); };
                    p3d2d.Add(but3d);
                    hpos += but3d.Width + hpad;

                    GLCheckBox but2d = new GLCheckBox("2d", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.2d") , null);
                    but2d.Checked = !map.gl3dcontroller.MatrixCalc.InPerspectiveMode;
                    but2d.ToolTipText = "2D View";
                    but2d.GroupRadioButton = true;
                    but2d.MouseClick += (e1, e2) => { map.gl3dcontroller.ChangePerspectiveMode(false); };
                    p3d2d.Add(but2d);
                    hpos += but2d.Width + hpad;

                    pform.Add(p3d2d);
                }

                if ((parts & Map.Parts.YHoldButton) != 0)
                {
                    GLCheckBox butelite = new GLCheckBox("Elite", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.EliteMovement") , null);
                    butelite.ToolTipText = "Select elite movement (on Y plain)";
                    butelite.Checked = map.gl3dcontroller.YHoldMovement;
                    butelite.CheckChanged += (e1) => { map.gl3dcontroller.YHoldMovement = butelite.Checked; };
                    pform.Add(butelite);
                    hpos += butelite.Width + hpad;
                }

                if ((parts & Map.Parts.Galaxy) != 0)
                {
                    GLCheckBox butgal = new GLCheckBox("Galaxy", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowGalaxy") , null);
                    butgal.ToolTipText = "Show galaxy image";
                    butgal.Checked = map.GalaxyDisplay;
                    butgal.CheckChanged += (e1) => { map.GalaxyDisplay = butgal.Checked; };
                    pform.Add(butgal);
                    hpos += butgal.Width + hpad;
                }

                if ((parts & Map.Parts.Grid) != 0)
                {
                    GLCheckBox butgrid = new GLCheckBox("GridLines", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.Grid") , null);
                    butgrid.ToolTipText = "Show grid";
                    butgrid.Checked = map.Grid;
                    butgrid.CheckChanged += (e1) => { map.Grid = butgrid.Checked; };
                    pform.Add(butgrid);
                    hpos += butgrid.Width + hpad;
                }

                if ((parts & Map.Parts.StarDots) != 0)
                {
                    GLCheckBox butsd = new GLCheckBox("StarDots", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.StarDots") , null);
                    butsd.ToolTipText = "Show star field";
                    butsd.Checked = map.StarDotsSpritesDisplay;
                    butsd.CheckChanged += (e1) => { map.StarDotsSpritesDisplay = butsd.Checked; };
                    pform.Add(butsd);
                    hpos += butsd.Width + hpad;
                }

                if ((parts & Map.Parts.NavRoute) != 0)
                {
                    GLCheckBox butnr = new GLCheckBox("NavRoute", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.NavRoute"), null);
                    butnr.ToolTipText = "Show nav route";
                    butnr.Checked = map.NavRouteDisplay;
                    butnr.CheckChanged += (e1) => { map.NavRouteDisplay = butnr.Checked; };
                    pform.Add(butnr);
                    hpos += butnr.Width + hpad;
                }

                if ((parts & Map.Parts.Bookmarks) != 0)
                {
                    GLCheckBox butbkmks = new GLCheckBox("Bookmarks", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowBookmarks"), null);
                    butbkmks.ToolTipText = "Show bookmarks";
                    butbkmks.Checked = map.ShowBookmarks;
                    butbkmks.CheckChanged += (e1) => { map.ShowBookmarks = butbkmks.Checked; };
                    pform.Add(butbkmks);
                    hpos += butbkmks.Width + hpad;
                }

                vpos += iconsize + ypad;
            }


            if ((parts & Map.Parts.EDSMStars) != 0)
            {
                GLGroupBox tpgb = new GLGroupBox("GalaxyStarsGB", "Galaxy Stars", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize * 2));
                tpgb.BackColor = Color.Transparent;
                tpgb.ForeColor = Color.Orange;
                pform.Add(tpgb);

                int hpos = leftmargin;

                GLCheckBox butgalstars = new GLCheckBox("GalaxyStars", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GalaxyStars") , null);
                butgalstars.ToolTipText = "Show stars when zoomed in";
                butgalstars.Checked = (map.GalaxyStars & 1) != 0;
                butgalstars.CheckChanged += (e1) => { map.GalaxyStars ^= 1; };
                tpgb.Add(butgalstars);
                hpos += butgalstars.Width + hpad;

                GLCheckBox butgalstarstext = new GLCheckBox("GalaxyStarsText", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GalaxyStarsText") , null);
                butgalstarstext.ToolTipText = "Show names of stars when zoomed in";
                butgalstarstext.Checked = (map.GalaxyStars & 2) != 0;
                butgalstarstext.CheckChanged += (e1) => { map.GalaxyStars ^= 2; };
                tpgb.Add(butgalstarstext);
                hpos += butgalstarstext.Width + hpad;

                if ((parts & Map.Parts.PrepopulateEDSMLocalArea) != 0)
                {
                    GLNumberBoxLong nblong = new GLNumberBoxLong("LYDist", new Rectangle(hpos, 0, 50, iconsize), map.LocalAreaSize);
                    nblong.Minimum = 1;
                    nblong.Maximum = 500;
                    nblong.ToolTipText = "Set distance in ly of the box around the current star to show";
                    tpgb.Add(nblong);
                    hpos += nblong.Width + hpad;

                    GLLabel lylab = new GLLabel("LYLab", new Rectangle(hpos, 0, 30, iconsize), "Ly");
                    lylab.ForeColor = Color.DarkOrange;
                    tpgb.Add(lylab);
                    hpos += lylab.Width + hpad;
                }

                if ((parts & Map.Parts.LimitSelector) != 0)
                {
                    GLComboBox cbstars = new GLComboBox("GalaxyStarsNumber", new Rectangle(hpos, 0, 150, iconsize));
                    cbstars.ToolTipText = "Control how many stars are shown when zoomes in";
                    cbstars.Items = new List<string>() { "Stars-Ultra", "Stars-High", "Stars-Medium", "Stars-Low" };
                    var list = new List<int>() { 750000, 500000, 250000, 100000 };
                    int itemno = list.IndexOf(map.GalaxyStarsMaxObjects); // may be -1
                    if (itemno < 0)
                    {
                        itemno = 2;
                        map.GalaxyStarsMaxObjects = list[itemno];
                    }

                    cbstars.SelectedIndex = itemno;       // high default
                    cbstars.SelectedIndexChanged += (e1) => { map.GalaxyStarsMaxObjects = list[cbstars.SelectedIndex]; };
                    tpgb.Add(cbstars);
                }

                vpos += tpgb.Height + ypad;
            }

            if ((parts & Map.Parts.TravelPath) != 0)
            {
                GLGroupBox tpgb = new GLGroupBox("TravelPathGB", "Travel Path", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize * 3));
                tpgb.BackColor = Color.Transparent;
                tpgb.ForeColor = Color.Orange;
                pform.Add(tpgb);

                GLCheckBox buttp = new GLCheckBox("TravelPathTape", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.TravelPath") , null);
                buttp.ToolTipText = "Show travel path";
                buttp.Checked = map.TravelPathTapeDisplay;
                buttp.CheckChanged += (e1) => { map.TravelPathTapeDisplay = buttp.Checked; };
                tpgb.Add(buttp);

                GLDateTimePicker dtps = new GLDateTimePicker("TPStart", new Rectangle(50, 0, 350, 30), System.DateTime.Now);
                dtps.SuspendLayout();
                dtps.AutoSize = true;
                dtps.ShowCheckBox = dtps.ShowCalendar = true;
                dtps.Value = map.TravelPathStartDate;
                dtps.Checked = map.TravelPathStartDateEnable;
                dtps.ValueChanged += (e1) => { map.TravelPathStartDate = dtps.Value; map.TravelPathRefresh(); };
                dtps.CheckChanged += (e1) => { map.TravelPathStartDateEnable = dtps.Checked; map.TravelPathRefresh(); };
                dtps.ShowUpDown = true;
                dtps.ResumeLayout();
                tpgb.Add(dtps);

                GLCheckBox buttptext = new GLCheckBox("TravelPathText", new Rectangle(leftmargin, 34, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.GalaxyStarsText") , null);
                buttptext.ToolTipText = "Show names of stars when zoomed in";
                buttptext.Checked = map.TravelPathTextDisplay;
                buttptext.CheckChanged += (e1) => { map.TravelPathTextDisplay = !map.TravelPathTextDisplay; };
                tpgb.Add(buttptext);

                GLDateTimePicker dtpe = new GLDateTimePicker("TPEnd", new Rectangle(50, 34, 350, 30), System.DateTime.Now);
                dtpe.SuspendLayout();
                dtpe.AutoSize = true;
                dtpe.ShowCheckBox = dtps.ShowCalendar = true;
                dtpe.Value = map.TravelPathEndDate;
                dtpe.Checked = map.TravelPathEndDateEnable;
                dtpe.ValueChanged += (e1) => { map.TravelPathEndDate = dtpe.Value; map.TravelPathRefresh(); };
                dtpe.CheckChanged += (e1) => { map.TravelPathEndDateEnable = dtpe.Checked; map.TravelPathRefresh(); };
                dtpe.ShowUpDown = true;
                dtpe.ResumeLayout();
                tpgb.Add(dtpe);

                vpos += tpgb.Height + ypad;
            }

            if ((parts & Map.Parts.GalObjects) != 0)
            {
                GLGroupBox galgb = new GLGroupBox("GalGB", "Galaxy Objects", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                galgb.ClientHeight = (iconsize + 4) * 2;
                galgb.BackColor = Color.Transparent;
                galgb.ForeColor = Color.Orange;
                pform.Add(galgb);
                vpos += galgb.Height + ypad;

                GLFlowLayoutPanel galfp = new GLFlowLayoutPanel("GALFP", DockingType.Fill, 0);
                galfp.FlowPadding = new PaddingType(2, 2, 2, 2);
                galfp.BackColor = Color.Transparent;
                galgb.Add(galfp);

                IReadOnlyDictionary<GalMapType.VisibleObjectsType, Image> icons = new BaseUtils.Icons.IconGroup<GalMapType.VisibleObjectsType>("GalMap");

                for (int i = GalMapType.VisibleTypes.Length - 1; i >= 0; i--)
                {
                    var gt = GalMapType.VisibleTypes[i];
                    bool en = map.GetGalObjectTypeEnable(gt.TypeName);
                    GLCheckBox butg = new GLCheckBox("GMSEL" + i, new Rectangle(0, 0, iconsize, iconsize), icons[gt.VisibleType.Value], null);
                    butg.ToolTipText = "Enable/Disable " + gt.Description;
                    butg.Checked = en;
                    butg.CheckChanged += (e1) =>
                    {
                        map.SetGalObjectTypeEnable(gt.TypeName, butg.Checked);
                    };
                    galfp.Add(butg);
                }

                GLCheckBox butgonoff = new GLCheckBox("GMONOFF", new Rectangle(0, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowGalaxy"), null);
                butgonoff.ToolTipText = "Enable/Disable Display";
                butgonoff.Checked = map.GalObjectDisplay;
                butgonoff.CheckChanged += (e1) => { map.GalObjectDisplay = !map.GalObjectDisplay; };
                galfp.Add(butgonoff);
            }

            if ((parts & Map.Parts.GalObjects) != 0 || (parts & Map.Parts.Bookmarks) != 0)
            {
                GLGroupBox scalegb = new GLGroupBox("Scalar", "Scaling", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                scalegb.ClientHeight = (iconsize + 4) * 1;
                scalegb.BackColor = Color.Transparent;
                scalegb.ForeColor = Color.Orange;
                pform.Add(scalegb);
                vpos += scalegb.Height + ypad;
                GLTrackBar tb = new GLTrackBar("ScaleTB", new Rectangle(0, 0, iconsize*8, iconsize));
                tb.Minimum = 1;
                tb.Maximum = 500;
                tb.Value = map.AutoScaleMax;
                tb.ValueChanged += (s,v) => { map.AutoScaleMax = v; };
                scalegb.Add(tb);
            }

            if ((parts & Map.Parts.Regions) != 0)
            {
                // EDSM regions

                GLGroupBox edsmregionsgb = new GLGroupBox("EDSMR", "EDSM Regions", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                edsmregionsgb.ClientHeight = iconsize + 8;
                edsmregionsgb.BackColor = Color.Transparent;
                edsmregionsgb.ForeColor = Color.Orange;
                pform.Add(edsmregionsgb);
                vpos += edsmregionsgb.Height + ypad;

                GLCheckBox butedre = new GLCheckBox("EDSMRE", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowGalaxy") , null);
                butedre.ToolTipText = "Enable EDSM Regions";
                butedre.Checked = map.EDSMRegionsEnable;
                butedre.UserCanOnlyCheck = true;
                edsmregionsgb.Add(butedre);

                GLCheckBox buted2 = new GLCheckBox("EDSMR2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionOutlines") , null);
                buted2.Checked = map.EDSMRegionsOutlineEnable;
                buted2.Enabled = map.EDSMRegionsEnable;
                buted2.ToolTipText = "Enable Region Outlines";
                buted2.CheckChanged += (e1) => { map.EDSMRegionsOutlineEnable = !map.EDSMRegionsOutlineEnable; };
                edsmregionsgb.Add(buted2);

                GLCheckBox buted3 = new GLCheckBox("EDSMR3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionShading") , null);
                buted3.Checked = map.EDSMRegionsShadingEnable;
                buted3.Enabled = map.EDSMRegionsEnable;
                buted3.ToolTipText = "Enable Region Shading";
                buted3.CheckChanged += (e1) => { map.EDSMRegionsShadingEnable = !map.EDSMRegionsShadingEnable; };
                edsmregionsgb.Add(buted3);

                GLCheckBox buted4 = new GLCheckBox("EDSMR4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionNames") , null);
                buted4.Checked = map.EDSMRegionsTextEnable;
                buted4.Enabled = map.EDSMRegionsEnable;
                buted4.ToolTipText = "Enable Region Naming";
                buted4.CheckChanged += (e1) => { map.EDSMRegionsTextEnable = !map.EDSMRegionsTextEnable; };
                edsmregionsgb.Add(buted4);

                // elite regions

                GLGroupBox eliteregionsgb = new GLGroupBox("ELITER", "Elite Regions", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                eliteregionsgb.ClientHeight = iconsize + 8;
                eliteregionsgb.BackColor = Color.Transparent;
                eliteregionsgb.ForeColor = Color.Orange;
                pform.Add(eliteregionsgb);
                vpos += eliteregionsgb.Height + ypad;

                GLCheckBox butelre = new GLCheckBox("ELITERE", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ShowGalaxy") , null);
                butelre.ToolTipText = "Enable Elite Regions";
                butelre.Checked = map.EliteRegionsEnable;
                butelre.UserCanOnlyCheck = true;
                eliteregionsgb.Add(butelre);

                GLCheckBox butel2 = new GLCheckBox("ELITER2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionOutlines") , null);
                butel2.Checked = map.EliteRegionsOutlineEnable;
                butel2.Enabled = map.EliteRegionsEnable;
                butel2.ToolTipText = "Enable Region Outlines";
                butel2.CheckChanged += (e1) => { map.EliteRegionsOutlineEnable = !map.EliteRegionsOutlineEnable; };
                eliteregionsgb.Add(butel2);

                GLCheckBox butel3 = new GLCheckBox("ELITER3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionShading") , null);
                butel3.Checked = map.EliteRegionsShadingEnable;
                butel3.Enabled = map.EliteRegionsEnable;
                butel3.ToolTipText = "Enable Region Shading";
                butel3.CheckChanged += (e1) => { map.EliteRegionsShadingEnable = !map.EliteRegionsShadingEnable; };
                eliteregionsgb.Add(butel3);

                GLCheckBox butel4 = new GLCheckBox("ELITER4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.RegionNames") , null);
                butel4.Checked = map.EliteRegionsTextEnable;
                butel4.Enabled = map.EliteRegionsEnable;
                butel4.ToolTipText = "Enable Region Naming";
                butel4.CheckChanged += (e1) => { map.EliteRegionsTextEnable = !map.EliteRegionsTextEnable; };
                eliteregionsgb.Add(butel4);

                butedre.CheckChanged += (e) =>
                {
                    if (e.Name == "EDSMRE")
                    {
                        butelre.CheckedNoChangeEvent = !butedre.Checked;
                    }
                    else
                    {
                        butedre.CheckedNoChangeEvent = !butelre.Checked;
                    }

                    map.EDSMRegionsEnable = butedre.Checked;
                    map.EliteRegionsEnable = butelre.Checked;

                    buted2.Enabled = buted3.Enabled = buted4.Enabled = butedre.Checked;
                    butel2.Enabled = butel3.Enabled = butel4.Enabled = butelre.Checked;
                };

                butelre.CheckChanged += butedre.CheckChanged;
            }

            pform.ClientHeight = vpos;

            map.displaycontrol.Add(pform);
        }

        public void UpdateCoords(GLOFC.Controller.Controller3D pc)
        {
            status.Text = pc.PosCamera.LookAt.X.ToStringInvariant("N1") + " ," + pc.PosCamera.LookAt.Y.ToStringInvariant("N1") + " ,"
                         + pc.PosCamera.LookAt.Z.ToStringInvariant("N1") + " Dist " + pc.PosCamera.EyeDistance.ToStringInvariant("N1") + " Eye " +
                         pc.PosCamera.EyePosition.X.ToStringInvariant("N1") + " ," + pc.PosCamera.EyePosition.Y.ToStringInvariant("N1") + " ," + pc.PosCamera.EyePosition.Z.ToStringInvariant("N1");
                         //+ " ! " + pc.PosCamera.CameraDirection + " R " + pc.PosCamera.CameraRotation;
        }

    }
}
