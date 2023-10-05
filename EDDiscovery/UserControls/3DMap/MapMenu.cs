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

using EliteDangerousCore.GMO;
using GLOFC.GL4.Controls;
using OpenTK;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
        private bool orderedclosemainmenu = false;  // import

        public MapMenu(Map g, Map.Parts parts, ImageCache imagecache)
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
            menuimage.MouseClick = (o, e1) => { 
                ShowMenu(parts,imagecache); 
            };
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
                EntryTextBox.ShowWaitButton = true;
                map.displaycontrol.Add(EntryTextBox);
                hpos += EntryTextBox.Width + hpad;
            }

            if ((parts & Map.Parts.PrepopulateGalaxyStarsLocalArea) != 0)
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

                GLForm mapform = map.displaycontrol["Galmenu"] as GLForm;

                if (mapform != null && ctrl == map.displaycontrol && map.displaycontrol.ModalFormsActive == false && mapform.FormShown && !orderedclosemainmenu)
                {
                    System.Diagnostics.Debug.WriteLine($"Ordered close");       // import
                    ((GLForm)mapform).Close();
                }
            };

        }

        // on menu button..
        public void ShowMenu(Map.Parts parts, ImageCache imagecache)
        {
            //map.displaycontrol.ApplyToControlOfName("InfoBoxForm*", (c) => { ((GLForm)c).Close(); });      // close any info box forms (don't want to I think)

            map.displaycontrol.ApplyToControlOfName("MS*", (c) => { c.Visible = false; });      // hide the visiblity of the on screen controls

            const int leftmargin = 4;
            const int hpad = 8;
            const int ypad = 10;

            orderedclosemainmenu = false;       // reset

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
                orderedclosemainmenu = true;
                var ani = new GLControlAnimateScale(10, 400, true, new SizeF(0, 0));       // add a close animation
                ani.FinishAction += (a, c, t) => { pform.ForceClose(); };   // when its complete, force close
                pform.Animators.Add(ani); 
            };

            int vpos = 10;

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


            if ((parts & Map.Parts.GalaxyStars) != 0)
            {
                GLGroupBox tpgb = new GLGroupBox("GalaxyStarsGB", "Galaxy Stars", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize * 2));
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

                if ((parts & Map.Parts.PrepopulateGalaxyStarsLocalArea) != 0)
                {
                    GLNumberBoxLong nblong = new GLNumberBoxLong("LYDist", new Rectangle(hpos, 0, 50, iconsize), map.LocalAreaSize);
                    nblong.Minimum = 1;
                    nblong.Maximum = 500;
                    nblong.ToolTipText = "Set distance in ly of the box around the current star to show";
                    tpgb.Add(nblong);
                    hpos += nblong.Width + hpad;

                    GLLabel lylab = new GLLabel("LYLab", new Rectangle(hpos, 0, 30, iconsize), "Ly");
                    tpgb.Add(lylab);
                    hpos += lylab.Width + hpad;
                }

                if ((parts & Map.Parts.LimitSelector) != 0)
                {
                    GLComboBox cbstars = new GLComboBox("GalaxyStarsNumber", new Rectangle(hpos, 0, 120, iconsize));
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
                    hpos += cbstars.Width + hpad;
                }

                GLLabel lab = new GLLabel("ScaleLab", new Rectangle(hpos, 0, 50, iconsize), "Scale:");
                GLTrackBar tb = new GLTrackBar("ScaleGS", new Rectangle(lab.Right + hpad, 0, iconsize * 4, iconsize));
                tpgb.Add(lab);
                tb.Minimum = 1;
                tb.Maximum = 10;
                tb.TickFrequency = 1;
                tb.Value = map.AutoScaleGalaxyStars;
                tb.ValueChanged += (s, v) => { map.AutoScaleGalaxyStars = v; };
                tpgb.Add(tb);

                vpos += tpgb.Height + ypad;
            }

            if ((parts & Map.Parts.TravelPath) != 0)
            {
                GLGroupBox tpgb = new GLGroupBox("TravelPathGB", "Travel Path", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize * 3));
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
                dtps.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(map.TravelPathStartDateUTC);
                dtps.Checked = map.TravelPathStartDateEnable;
                dtps.ValueChanged += (e1) => { map.TravelPathStartDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromPicker(dtps.Value); map.TravelPathRefresh(); };
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
                dtpe.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(map.TravelPathEndDateUTC);
                dtpe.Checked = map.TravelPathEndDateEnable;
                dtpe.ValueChanged += (e1) => { map.TravelPathEndDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromPicker(dtpe.Value); map.TravelPathRefresh(); };
                dtpe.CheckChanged += (e1) => { map.TravelPathEndDateEnable = dtpe.Checked; map.TravelPathRefresh(); };
                dtpe.ShowUpDown = true;
                dtpe.ResumeLayout();
                tpgb.Add(dtpe);

                vpos += tpgb.Height + ypad;
            }

            if ((parts & Map.Parts.GalObjects) != 0)
            {
                GLGroupBox galgb = new GLGroupBox("GalGB", "Galaxy Objects", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 10));
                pform.Add(galgb);

                GLFlowLayoutPanel galfp = new GLFlowLayoutPanel("GALFP", DockingType.Top,0);
                galfp.AutoSize = true;
                galfp.FlowPadding = new PaddingType(2, 2, 2, 2);
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

                GLLabel lab = new GLLabel("ScaleLab", new Rectangle(leftmargin, galfp.Height + ypad, 50, iconsize), "Scale:");
                galgb.Add(lab);
                GLTrackBar tb = new GLTrackBar("ScaleGMO", new Rectangle(lab.Right, lab.Top, iconsize*8, iconsize));
                tb.Minimum = 1;
                tb.Maximum = 250;
                tb.Value = map.AutoScaleGMOs;
                tb.ValueChanged += (s,v) => { map.AutoScaleGMOs = v; };
                galgb.Add(tb);

                galgb.ClientHeight = galfp.Height + ypad + tb.Height + ypad;

                vpos += galgb.Height + ypad;
            }

            if ((parts & Map.Parts.Regions) != 0)
            {
                // EDSM regions

                GLGroupBox edsmregionsgb = new GLGroupBox("EDSMR", "EDSM Regions", new Rectangle(leftmargin, vpos, pform.ClientWidth / 2, 50));
                edsmregionsgb.ClientHeight = iconsize + 8;
                pform.Add(edsmregionsgb);

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

                GLGroupBox eliteregionsgb = new GLGroupBox("ELITER", "Elite Regions", new Rectangle(edsmregionsgb.Right + hpad, vpos, pform.ClientWidth - leftmargin*2 - edsmregionsgb.Width - hpad, 50));
                eliteregionsgb.ClientHeight = iconsize + 8;
                pform.Add(eliteregionsgb);

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

                vpos += edsmregionsgb.Height + ypad;
            }

            if ((parts & Map.Parts.ImageList) != 0)
            { 
                GLGroupBox imagesgb = new GLGroupBox("Images", "Overlay Images", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                imagesgb.ClientHeight = iconsize + 8;
                pform.Add(imagesgb);

                GLCheckBox b1 = new GLCheckBox("ImagesEnable", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.Images"), null);
                b1.ToolTipText = "Enable or disable images";
                b1.Checked = map.UserImagesEnable;
                b1.CheckOnClick = true;
                b1.CheckChanged += (e1) => { map.UserImagesEnable = b1.Checked; };
                imagesgb.Add(b1);

                GLButton b2 = new GLButton("ImagesConfigure", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("GalMap.ImagesEdit"), true);
                b2.ToolTipText = "Configure image list";
                b2.Click += (e, s) => ShowImagesMenu(imagecache.GetImageList(), (list) => { imagecache.SetImageList(list); });
                imagesgb.Add(b2);

                vpos += imagesgb.Height + ypad;
            }

            if ((parts & Map.Parts.Bookmarks) != 0)
            {
                GLGroupBox scalegb = new GLGroupBox("Scalar", "Bookmarks Scaling", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                scalegb.ClientHeight = (iconsize + 4) * 1;
                pform.Add(scalegb);
                vpos += scalegb.Height + ypad;
                GLTrackBar tb = new GLTrackBar("ScaleBK", new Rectangle(0, 0, iconsize * 8, iconsize));
                tb.Minimum = 1;
                tb.Maximum = 250;
                tb.Value = map.AutoScaleBookmarks;
                tb.ValueChanged += (s, v) => { map.AutoScaleBookmarks = v; };
                scalegb.Add(tb);
            }

            pform.ClientHeight = vpos;

            map.displaycontrol.Add(pform);
        }

        public void ShowImagesMenu(List<ImageCache.ImageEntry> imagelist, Action<List<ImageCache.ImageEntry>> onok)
        {
            GLForm iform = new GLForm("Imagesmenu", "Configure Images", new Rectangle(100, 50, 1200, 500), Color.FromArgb(220, 60, 60, 160), Color.Orange, true);

            // provide opening animation
            iform.ScaleWindow = new SizeF(0.0f, 0.0f);
            iform.Animators.Add(new GLControlAnimateScale(10, 300, true, new SizeF(1, 1)));

            // and closing animation
            iform.FormClosing += (f, e) =>
            {
                e.Handled = true;       // stop close
                var ani = new GLControlAnimateScale(10, 200, true, new SizeF(0, 0));       // add a close animation
                ani.FinishAction += (a, c, t) => { iform.ForceClose(); };   // when its complete, force close
                iform.Animators.Add(ani);
                if (iform.DialogResult == GLForm.DialogResultEnum.OK)
                    onok(imagelist);
            };

            GLScrollPanelScrollBar spanel = new GLScrollPanelScrollBar("ImagesScrollPanel", DockingType.Fill, 0F);
            spanel.EnableHorzScrolling = false;
            iform.Add(spanel);


            GLPanel top = new GLPanel("Paneltop", new Size(0, 32), DockingType.Top, 0F);
            iform.Add(top);

            List<ImageCache.ImageEntry> inbuiltlist = new List<ImageCache.ImageEntry>
            {
                new ImageCache.ImageEntry("EDAstro Indexed Heat Map", @"https://edastro.com/mapcharts/visited-systems-indexedheatmap.png",
                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Indexed Log Heat Map", @"https://edastro.com/mapcharts/visited-systems-heatmap.png",
                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Indexed Heat Map+Regions", @"https://edastro.com/mapcharts/visited-systems-indexedregions.jpg",
                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Indexed Log Heat Map+Regions", @"https://edastro.com/mapcharts/visited-systems-regions.png",
                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Alien Map", @"https://edastro.com/mapcharts/codex/codex-aliens-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro ?-Type Anomalies Map", @"https://edastro.com/mapcharts/codex/codex-anomalies-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Crystal Structures Map", @"https://edastro.com/mapcharts/codex/codex-crystals-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Lagrange Clouds Map", @"https://edastro.com/mapcharts/codex/codex-lagrangeclouds-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Molluscs Map", @"https://edastro.com/mapcharts/codex/codex-molluscs-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Pre-Odyssey Surface Life Map", @"https://edastro.com/mapcharts/codex/codex-surface-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Trees Map", @"https://edastro.com/mapcharts/codex/codex-trees-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Pods Map", @"https://edastro.com/mapcharts/codex/codex-pods-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Bark Mounds Map", @"https://edastro.com/mapcharts/organic/organic-bark-mounds-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Brain Trees Map", @"https://edastro.com/mapcharts/organic/organic-brain-tree-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
                new ImageCache.ImageEntry("EDAstro Sinous Tubers Map", @"https://edastro.com/mapcharts/organic/organic-sinuous-tubers-regions.jpg",
                                        true,new Vector3(6140,0,18850),new Vector2(102300,102300),new Vector3(0,0,0),false,false,0,0.4f),
            };

            GLComboBox presel = new GLComboBox("Preselects", new Rectangle(40, 4, 240, 24), inbuiltlist.Select(x => x.Name).ToList());
            presel.ToolTipText = "Preselected images from the internet";
            top.Add(presel);

            map.displaycontrol.AddModalForm(iform);
            iform.FormClosed += (s) =>
            {
                if (iform.DialogResult == GLForm.DialogResultEnum.OK)
                    map.LoadImages();
            };

            List<string> resourcenames = BaseUtils.Icons.IconSet.Instance.Names().ToList();

            int availablewidth = iform.ClientRectangle.Width - spanel.ScrollBarWidth;

            PopulateImagesScrollPanel(spanel, top, imagelist, resourcenames, availablewidth);  // add after iform added to get correct scroll bar width

            presel.SelectedIndexChanged += (s) =>
            {
                imagelist.Add(inbuiltlist[presel.SelectedIndex]);
                PopulateImagesScrollPanel(spanel, null, imagelist, resourcenames, availablewidth);
                spanel.VertScrollPos = int.MaxValue;        // goto bottom
            };
            presel.TabOrder = 0;
            presel.SetFocus();
        }

        private void PopulateImagesScrollPanel(GLScrollPanelScrollBar spanel, GLPanel toppanel, List<ImageCache.ImageEntry> imagelist, List<string> resourcenames, int width)
        {
            spanel.SuspendLayout();
            spanel.Remove();

            int tabno = 1;

            int leftmargin = 4;
            int vpos = 10;
            int ypad = 10;
            int hpad = 8;
            for (int entry = 0; entry < imagelist.Count; entry++)
            {
                var ie = imagelist[entry];

                var cenable = new GLCheckBox($"En{entry}", new Rectangle(leftmargin, vpos, iconsize, iconsize), "", ie.Enabled);
                cenable.CheckChanged += (s) => ie.Enabled = cenable.Checked;
                cenable.ToolTipText = "Enable or disable this entry from showing";
                spanel.Add(cenable, ref tabno);

                int spaceavailable = width - iconsize * 11 - hpad * 12 - leftmargin * 2;

                var name = new GLTextBoxAutoComplete("name", new Rectangle(cenable.Right + hpad, vpos, spaceavailable / 3, iconsize), ie.Name);
                name.TextChanged += (s) => { ie.Name = name.Text; };
                name.ToolTipText = "Name of entry";
                name.ShowWaitButton = true;
                spanel.Add(name, ref tabno);

                spaceavailable -= spaceavailable / 3 + hpad;

                var urlpath = new GLTextBoxAutoComplete("urlpath", new Rectangle(name.Right + hpad, vpos, spaceavailable, iconsize), ie.ImagePathOrURL);
                urlpath.PerformAutoCompleteInThread += (input, sender, set) =>
                {
                    if (input.StartsWith("Image:", StringComparison.InvariantCultureIgnoreCase))
                        input = input.Substring(9);
                    foreach (var x in resourcenames)
                    {
                        if (x.Contains(input, StringComparison.InvariantCultureIgnoreCase))
                            set.Add("Image:" + x);
                    }
                };

                urlpath.ToolTipText = "Use Image:name for a EDD icon\r\n\"text\"[,font,size,forecolour,backcolour,format,bitmapwidth,bitmapheight] for text\r\nhttp:\\pathto for a internet image\r\n<path> for a local file";
                urlpath.TextChanged += (s) => { ie.ImagePathOrURL = urlpath.Text; };
                spanel.Add(urlpath, ref tabno);

                var ccentre = new GLButton("cent", new Rectangle(urlpath.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.Position"), true);
                ccentre.Click += (s, e) => ShowVector3Menu(ie.Centre, true, ccentre.FindScreenCoords(), "Centre", (v) => ie.Centre = v);
                ccentre.ToolTipText = "Centre of image in lightyears";
                spanel.Add(ccentre, ref tabno);

                var csize = new GLButton("size", new Rectangle(ccentre.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.Sizer"), true);
                csize.Click += (s, e) => ShowVector3Menu(new Vector3(ie.Size.X, ie.Size.Y, 0), false, csize.FindScreenCoords(), "Size", (v) => ie.Size = new Vector2(v.X, v.Y));
                csize.ToolTipText = "Size of image in lightyears";
                spanel.Add(csize, ref tabno);

                var crot = new GLButton("rot", new Rectangle(csize.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.Rotate"), true);
                crot.Click += (s, e) => ShowVector3Menu(ie.RotationDegrees, true, crot.FindScreenCoords(), "Rotation Degrees", (v) => ie.RotationDegrees = v);
                crot.ToolTipText = "Rotation around X,Y,Z in degrees";
                spanel.Add(crot, ref tabno);

                var crotaz = new GLCheckBox("rotaz", new Rectangle(crot.Right + hpad, vpos, iconsize, iconsize), "", ie.RotateToViewer);
                crotaz.CheckChanged += (s) => ie.RotateToViewer = crotaz.Checked;
                crotaz.ToolTipText = "Enables rotation to the viewer azimuth so it faces you";
                spanel.Add(crotaz, ref tabno);

                var crotel = new GLCheckBox("rotel", new Rectangle(crotaz.Right + hpad, vpos, iconsize, iconsize), "", ie.RotateElevation);
                crotel.CheckChanged += (s) => ie.RotateElevation = crotel.Checked;
                crotel.ToolTipText = "Enables rotation to the viewer elevation so it faces you";
                spanel.Add(crotel, ref tabno);

                var calscalar = new GLNumberBoxFloat("scalar", new Rectangle(crotel.Right + hpad, vpos, iconsize * 3 / 2, iconsize), ie.AlphaFadeScalar);
                calscalar.Format = "0.#";
                calscalar.ValueChanged += (s) => { ie.AlphaFadeScalar = calscalar.Value; };
                calscalar.Minimum = 0;
                calscalar.ToolTipText = "Alpha scaling by distance from eye. >0 fade out as distance decreases. <0 fade out as distance increase. 0 = fixed fade (determined by position)";
                spanel.Add(calscalar, ref tabno);

                var calpos = new GLNumberBoxFloat("pos", new Rectangle(calscalar.Right + hpad, vpos, iconsize * 3 / 2, iconsize), ie.AlphaFadePosition);
                calpos.Format = "0.#";
                calpos.ValueChanged += (s) => { ie.AlphaFadePosition = calpos.Value; };
                calpos.Minimum = 0;
                calpos.ToolTipText = "if scalar != 0, Distance where fade starts/end. If scalar = 0, alpha fading between 0-1";
                spanel.Add(calpos, ref tabno);

                GLButton delb = new GLButton("Del", new Rectangle(calpos.Right + hpad + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.Delete"), true);
                delb.Click += (s, e) =>
                {
                    int sp = spanel.VertScrollPos;
                    imagelist.Remove(ie);
                    PopulateImagesScrollPanel(spanel, null, imagelist, resourcenames, width);
                    spanel.VertScrollPos = sp;
                };
                delb.ToolTipText = "Delete entry";
                spanel.Add(delb, ref tabno);

                if (toppanel != null)
                {
                    toppanel.Add(new GLLabel("LEn", new Rectangle(cenable.Left, 4, iconsize + hpad, 20), "En"));
                    toppanel.Add(new GLLabel("Lurl", new Rectangle(urlpath.Left, 4, 200, 20), "URL/Resource/Image/File"));
                    toppanel.Add(new GLLabel("LPos", new Rectangle(ccentre.Left, 4, iconsize + hpad, 20), "Pos"));
                    toppanel.Add(new GLLabel("LSize", new Rectangle(csize.Left, 4, iconsize + hpad, 20), "Size"));
                    toppanel.Add(new GLLabel("LRot", new Rectangle(crot.Left, 4, iconsize + hpad, 20), "Rot"));
                    toppanel.Add(new GLLabel("LDel", new Rectangle(delb.Left, 4, iconsize + hpad, 20), "Del"));
                    toppanel.Add(new GLLabel("LMU", new Rectangle(delb.Right + hpad, 4, iconsize + hpad, 20), "Up"));
                    toppanel.Add(new GLLabel("AZRot", new Rectangle(crotaz.Left, 4, iconsize + hpad, 20), "AZr"));
                    toppanel.Add(new GLLabel("AZEl", new Rectangle(crotel.Left, 4, iconsize + hpad, 20), "ELr"));
                    toppanel.Add(new GLLabel("als", new Rectangle(calscalar.Left, 4, iconsize * 3 / 2 + hpad, 20), "AlSc"));
                    toppanel.Add(new GLLabel("ald", new Rectangle(calpos.Left, 4, iconsize * 3 / 2 + hpad, 20), "AlP/F"));
                }

                if (entry > 0)
                {
                    GLButton upb = new GLButton("Up", new Rectangle(delb.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.ArrowsUp"), true);
                    upb.Click += (s, e) =>
                    {
                        int sp = spanel.VertScrollPos;
                        int entryno = imagelist.IndexOf(ie);
                        if (entryno > 0)
                        {
                            var previous = imagelist[entryno - 1];
                            imagelist.Remove(previous);      // this makes our current be in the previous slot
                            imagelist.Insert(entryno, previous); // and we insert previous into our pos
                        }
                        PopulateImagesScrollPanel(spanel, null, imagelist, resourcenames, width);
                        spanel.VertScrollPos = sp;
                    };
                    upb.ToolTipText = "Move entry up. Images are displayed in the order show, the lowest one has greates priority";
                    spanel.Add(upb, ref tabno);
                }

                vpos += cenable.Height + ypad;
            }

            GLButton add = new GLButton("Add", new Rectangle(leftmargin, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.Add"), true);
            add.Click += (s, e) =>
            {
                imagelist.Add(new ImageCache.ImageEntry("", "", true, new Vector3(0, 0, 0), new Vector2(200, 200), new Vector3(0, 0, 0)));
                PopulateImagesScrollPanel(spanel, null, imagelist, resourcenames, width);
                spanel.VertScrollPos = int.MaxValue;        // goto bottom
            };
            add.ToolTipText = "Add new entry";
            spanel.Add(add, ref tabno);

            GLButton save = new GLButton("Save", new Rectangle(add.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.ExportFile"), true);
            save.Click += (s, e) =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "ImageList";
                sfd.DefaultExt = "eddil";
                sfd.Filter = "EDD Image List (*.eddil)|*.eddil|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool success = false;
                    JArray ret = JToken.FromObjectWithError(imagelist, true, membersearchflags: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Array();
                    if (ret != null)
                    {
                        var str = ret.ToString(true);
                        success = BaseUtils.FileHelpers.TryWriteToFile(sfd.FileName, str);
                    }
                    if (!success)
                    {
                        GLMessageBox.Show("m1", spanel, new Point(int.MinValue, 0), $"Failed to save {sfd.FileName}", "Error",
                                    GLMessageBox.MessageBoxButtons.OK);
                    }
                }

            };
            save.ToolTipText = "Save list to file";
            spanel.Add(save, ref tabno);

            GLButton load = new GLButton("Load", new Rectangle(save.Right + hpad, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.GetBitmap("Controls.ImportFile"), true);
            load.Click += (s, e) =>
            {
                OpenFileDialog sfd = new OpenFileDialog();
                sfd.FileName = "ImageList";
                sfd.DefaultExt = "eddil";
                sfd.Filter = "EDD Image List (*.eddil)|*.eddil|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bool success = false;
                    string str = BaseUtils.FileHelpers.TryReadAllTextFromFile(sfd.FileName);
                    if (str != null)
                    {
                        JToken json = QuickJSON.JToken.Parse(str, QuickJSON.JToken.ParseOptions.CheckEOL);
                        if (json != null)
                        {
                            var list = json.ToObject<List<ImageCache.ImageEntry>>();
                            if (list != null)
                            {
                                imagelist.Clear();          // don't replace the imagelist, we need to use the same class, clear and add
                                imagelist.AddRange(list);
                                success = true;
                            }
                        }
                    }

                    if (success)
                    {
                        PopulateImagesScrollPanel(spanel, null, imagelist, resourcenames, width);
                    }
                    else
                    {
                        GLMessageBox.Show("m1", spanel, new Point(int.MinValue, 0), $"Failed to load {sfd.FileName}", "Error",
                                    GLMessageBox.MessageBoxButtons.OK);
                    }
                }
            };
            load.ToolTipText = "Load the list from a file";
            spanel.Add(load, ref tabno);

            GLButton ok = new GLButton("OK", new Rectangle(width - leftmargin - 80, vpos, 80, iconsize), "OK");
            ok.Click += (s, e) =>
            {
                spanel.FindForm().DialogResult = GLForm.DialogResultEnum.OK;
                spanel.FindForm().Close();
            };

            ok.ToolTipText = "Accept and use this list";
            spanel.Add(ok, ref tabno);

            spanel.ResumeLayout();
        }

        public void ShowVector3Menu(Vector3 value, bool vector3, Point pos, string name, Action<Vector3> onok)
        {
            GLFormVector3 iform = new GLFormVector3("Vectormenu", name, value, new Rectangle(pos, new Size(250, 150)), vector2: !vector3);
            iform.DialogResultChanged += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"ON OK {iform.Value.X} {iform.Value.Y} {iform.Value.Z}");
                onok(new Vector3(iform.Value.X, iform.Value.Y, vector3 ? iform.Value.Z : 0));
            };
            map.displaycontrol.AddModalForm(iform);
            iform.BackColor = Color.FromArgb(255, 90, 90, 100);

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
