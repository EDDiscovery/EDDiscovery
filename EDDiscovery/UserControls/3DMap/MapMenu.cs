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

using GLOFC.GL4.Controls;
using System.Collections.Generic;
using System.Drawing;

namespace EDDiscovery.UserControls.Map3D
{
    public class MapMenu
    {
        private Map map;
        private GLLabel status;
        private const int iconsize = 32;
        public GLTextBoxAutoComplete EntryTextBox { get; private set; }

        public MapMenu(Map g, Map.Parts parts)
        {
            map = g;

            // names of MS* are on screen items hidden during main menu presentation

            GLImage menuimage = new GLImage("MSMainMenu", new Rectangle(10, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.hamburgermenu") as Bitmap);
            menuimage.ToolTipText = "Open configuration menu";
            map.displaycontrol.Add(menuimage);
            menuimage.MouseClick = (o, e1) => { ShowMenu(parts); };

            int hpos = 50;
            if ((parts & Map.Parts.TravelPath) != 0)
            {
                GLImage tpback = new GLImage("MSTPBack", new Rectangle(hpos, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoBackward") as Bitmap);
                tpback.ToolTipText = "Go back one system";
                map.displaycontrol.Add(tpback);
                tpback.MouseClick = (o, e1) => { g.GoToTravelSystem(-1); };
                hpos += iconsize + 8;

                GLImage tphome = new GLImage("MSTPHome", new Rectangle(hpos, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoToHomeSystem") as Bitmap);
                tphome.ToolTipText = "Go to current home system";
                map.displaycontrol.Add(tphome);
                tphome.MouseClick = (o, e1) => { g.GoToTravelSystem(0); };
                hpos += iconsize + 8;

                GLImage tpforward = new GLImage("MSTPForward", new Rectangle(hpos, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoForward") as Bitmap);
                tpforward.ToolTipText = "Go forward one system";
                map.displaycontrol.Add(tpforward);
                tpforward.MouseClick = (o, e1) => { g.GoToTravelSystem(1); };
                hpos += iconsize + 8;
            }

            if ((parts & Map.Parts.GalaxyResetPos) != 0)
            {
                GLImage tpgalview = new GLImage("MSTPGalaxy", new Rectangle(hpos, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap);
                tpgalview.ToolTipText = "View Galaxy";
                map.displaycontrol.Add(tpgalview);
                tpgalview.MouseClick = (o, e1) => { g.ViewGalaxy(); };
                hpos += iconsize + 8;
            }

            if ((parts & Map.Parts.SearchBox) != 0)
            {
                EntryTextBox = new GLTextBoxAutoComplete("MSTPEntryText", new Rectangle(hpos, 10, 300, iconsize), "");
                EntryTextBox.TextAlign = ContentAlignment.MiddleLeft;
                EntryTextBox.BackColor = Color.FromArgb(96, 50, 50, 50);
                EntryTextBox.BorderColor = Color.Gray;
                EntryTextBox.BorderWidth = 1;
                map.displaycontrol.Add(EntryTextBox);
                hpos += EntryTextBox.Width + 8;
            }

            GLToolTip maintooltip = new GLToolTip("MTT",Color.FromArgb(180,50,50,50));
            maintooltip.ForeColor = Color.Orange;
            map.displaycontrol.Add(maintooltip);

            status = new GLLabel("Status", new Rectangle(10, 500, 600, 24), "x");
            status.Dock = DockingType.BottomLeft;
            status.ForeColor = Color.Orange;
            status.BackColor = Color.FromArgb(50, 50, 50, 50);
            map.displaycontrol.Add(status);

            GLBaseControl.Themer = Theme;

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
            map.displaycontrol.ApplyToControlOfName("InfoBoxForm*", (c) => { ((GLForm)c).Close(); });      // close any info box forms
            map.displaycontrol.ApplyToControlOfName("MS*", (c) => { c.Visible = false; });      // hide the visiblity of the on screen controls

            int leftmargin = 4;
            int vpos = 10;
            int ypad = 10;

            GLForm pform = new GLForm("Galmenu", "Configure Map", new Rectangle(10, 10, 600, 600));
            pform.BackColor = Color.FromArgb(220, 60, 60, 70);
            pform.ForeColor = Color.Orange;
            pform.FormClosed = (frm) => { map.displaycontrol.ApplyToControlOfName("MS*", (c) => { c.Visible = true; }); };
            pform.Resizeable = pform.Moveable = false;

            // provide opening animation
            pform.ScaleWindow = new SizeF(0.0f, 0.0f);
            pform.Animators.Add(new AnimateScale(map.ElapsedTimems + 10, map.ElapsedTimems + 400, new SizeF(1, 1)));
            pform.Font = new Font("Arial", 8.25f);

            // and closing animation
            pform.FormClosing += (f,e) => { 
                e.Handled = true;       // stop close
                var ani = new AnimateScale(map.ElapsedTimems + 10, map.ElapsedTimems + 400, new SizeF(0, 0));       // add a close animation
                ani.FinishAction += (a, c, t) => { pform.ForceClose(); };   // when its complete, force close
                pform.Animators.Add(ani); 
            };

            int hpad = 8;

            {   // top buttons
                int hpos = 0;

                if ((parts & Map.Parts.PerspectiveChange) != 0)
                {
                    GLPanel p3d2d = new GLPanel("3d2d", new Rectangle(leftmargin, vpos, 80, iconsize), Color.Transparent);

                    GLCheckBox but3d = new GLCheckBox("3d", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.3d") as Bitmap, null);
                    but3d.Checked = map.gl3dcontroller.MatrixCalc.InPerspectiveMode;
                    but3d.ToolTipText = "3D View";
                    but3d.GroupRadioButton = true;
                    but3d.MouseClick += (e1, e2) => { map.gl3dcontroller.ChangePerspectiveMode(true); };
                    p3d2d.Add(but3d);
                    hpos += but3d.Width + hpad;

                    GLCheckBox but2d = new GLCheckBox("2d", new Rectangle(hpos, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.2d") as Bitmap, null);
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
                    GLCheckBox butelite = new GLCheckBox("Elite", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.EliteMovement") as Bitmap, null);
                    butelite.ToolTipText = "Select elite movement (on Y plain)";
                    butelite.Checked = map.gl3dcontroller.YHoldMovement;
                    butelite.CheckChanged += (e1) => { map.gl3dcontroller.YHoldMovement = butelite.Checked; };
                    pform.Add(butelite);
                    hpos += butelite.Width + hpad;
                }

                if ((parts & Map.Parts.Galaxy) != 0)
                {
                    GLCheckBox butgal = new GLCheckBox("Galaxy", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                    butgal.ToolTipText = "Show galaxy image";
                    butgal.Checked = map.GalaxyDisplay;
                    butgal.CheckChanged += (e1) => { map.GalaxyDisplay = butgal.Checked; };
                    pform.Add(butgal);
                    hpos += butgal.Width + hpad;
                }

                if ((parts & Map.Parts.Grid) != 0)
                {
                    GLCheckBox butgrid = new GLCheckBox("GridLines", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.Grid") as Bitmap, null);
                    butgrid.ToolTipText = "Show grid";
                    butgrid.Checked = map.Grid;
                    butgrid.CheckChanged += (e1) => { map.Grid = butgrid.Checked; };
                    pform.Add(butgrid);
                    hpos += butgrid.Width + hpad;
                }

                if ((parts & Map.Parts.StarDots) != 0)
                {
                    GLCheckBox butsd = new GLCheckBox("StarDots", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.StarDots") as Bitmap, null);
                    butsd.ToolTipText = "Show star field";
                    butsd.Checked = map.StarDotsSpritesDisplay;
                    butsd.CheckChanged += (e1) => { map.StarDotsSpritesDisplay = butsd.Checked; };
                    pform.Add(butsd);
                    hpos += butsd.Width + hpad;
                }

                if ((parts & Map.Parts.NavRoute) != 0)
                {
                    GLCheckBox butnr = new GLCheckBox("NavRoute", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.NavRoute") as Bitmap, null);
                    butnr.ToolTipText = "Show nav route";
                    butnr.Checked = map.StarDotsSpritesDisplay;
                    butnr.CheckChanged += (e1) => { map.NavRouteDisplay = butnr.Checked; };
                    pform.Add(butnr);
                    hpos += butnr.Width + hpad;
                }

                if ((parts & Map.Parts.EDSMStars) != 0)
                {
                    GLCheckBox butgalstars = new GLCheckBox("GalaxyStars", new Rectangle(hpos, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GalaxyStars") as Bitmap, null);
                    butgalstars.ToolTipText = "Show stars when zoomed in";
                    butgalstars.Checked = map.GalaxyStars;
                    butgalstars.CheckChanged += (e1) => { map.GalaxyStars = butgalstars.Checked; };
                    pform.Add(butgalstars);
                    hpos += butgalstars.Width + hpad;

                    GLComboBox cbstars = new GLComboBox("GalaxyStarsNumber", new Rectangle(hpos, vpos, 100, iconsize));
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
                    pform.Add(cbstars);
                    hpos += cbstars.Width + hpad;
                }

                vpos += iconsize + ypad;
            }


            if ((parts & Map.Parts.TravelPath) != 0)
            {
                GLGroupBox tpgb = new GLGroupBox("TravelPathGB", "Travel Path", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize *2));
                tpgb.BackColor = pform.BackColor;
                tpgb.ForeColor = Color.Orange;
                pform.Add(tpgb);

                GLCheckBox buttp = new GLCheckBox("TravelPathTape", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.TravelPath") as Bitmap, null);
                buttp.ToolTipText = "Show travel path";
                buttp.Checked = map.TravelPathTapeDisplay;
                buttp.CheckChanged += (e1) => { map.TravelPathTapeDisplay = buttp.Checked; };
                tpgb.Add(buttp);

                GLDateTimePicker dtps = new GLDateTimePicker("TPStart", new Rectangle(50, 0, 250, 30), System.DateTime.Now);
                dtps.ShowCheckBox = dtps.ShowCalendar = true;
                dtps.Value = map.TravelPathStartDate;
                dtps.Checked = map.TravelPathStartDateEnable;
                dtps.ValueChanged += (e1) => { map.TravelPathStartDate = dtps.Value; map.TravelPathRefresh(); };
                dtps.CheckChanged += (e1) => { map.TravelPathStartDateEnable = dtps.Checked; map.TravelPathRefresh(); };
                dtps.ShowUpDown = true;
                tpgb.Add(dtps);

                GLDateTimePicker dtpe = new GLDateTimePicker("TPEnd", new Rectangle(320, 0, 250, 30), System.DateTime.Now);
                dtpe.ShowCheckBox = dtps.ShowCalendar = true;
                dtpe.Value = map.TravelPathEndDate;
                dtpe.Checked = map.TravelPathEndDateEnable;
                dtpe.ValueChanged += (e1) => { map.TravelPathEndDate = dtpe.Value; map.TravelPathRefresh(); };
                dtpe.CheckChanged += (e1) => { map.TravelPathEndDateEnable = dtpe.Checked; map.TravelPathRefresh(); };
                dtpe.ShowUpDown = true;
                tpgb.Add(dtpe);

                vpos += tpgb.Height + ypad;
            }

            if ((parts & Map.Parts.GalObjects) != 0)
            {
                GLGroupBox galgb = new GLGroupBox("GalGB", "Galaxy Objects", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                galgb.ClientHeight = (iconsize + 4) * 2;
                galgb.BackColor = pform.BackColor;
                galgb.ForeColor = Color.Orange;
                pform.Add(galgb);
                GLFlowLayoutPanel galfp = new GLFlowLayoutPanel("GALFP", DockingType.Fill, 0);
                galfp.FlowPadding = new Padding(2, 2, 2, 2);
                galfp.BackColor = pform.BackColor;
                galgb.Add(galfp);
                vpos += galgb.Height + ypad;

                for (int i = map.edsmmapping.RenderableMapTypes.Length - 1; i >= 0; i--)
                {
                    var gt = map.edsmmapping.RenderableMapTypes[i];
                    bool en = map.GetGalObjectTypeEnable(gt.Typeid);
                    GLCheckBox butg = new GLCheckBox("GMSEL"+i, new Rectangle(0, 0, iconsize, iconsize), gt.Image, null);
                    butg.ToolTipText = "Enable/Disable " + gt.Description;
                    butg.Checked = en;
                    butg.CheckChanged += (e1) =>
                    {
                        map.SetGalObjectTypeEnable(gt.Typeid, butg.Checked);
                    };
                    galfp.Add(butg);
                }

                GLCheckBox butgonoff = new GLCheckBox("GMONOFF", new Rectangle(0, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butgonoff.ToolTipText = "Enable/Disable Display";
                butgonoff.Checked = map.GalObjectDisplay;
                butgonoff.CheckChanged += (e1) => { map.GalObjectDisplay = !map.GalObjectDisplay; };
                galfp.Add(butgonoff);
            }


            if ((parts & Map.Parts.Regions) != 0)
            {
                // EDSM regions

                GLGroupBox edsmregionsgb = new GLGroupBox("EDSMR", "EDSM Regions", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                edsmregionsgb.ClientHeight = iconsize + 8;
                edsmregionsgb.BackColor = pform.BackColor;
                edsmregionsgb.ForeColor = Color.Orange;
                pform.Add(edsmregionsgb);
                vpos += edsmregionsgb.Height + ypad;

                GLCheckBox butedre = new GLCheckBox("EDSMRE", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butedre.ToolTipText = "Enable EDSM Regions";
                butedre.Checked = map.EDSMRegionsEnable;
                butedre.UserCanOnlyCheck = true;
                edsmregionsgb.Add(butedre);

                GLCheckBox buted2 = new GLCheckBox("EDSMR2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionOutlines") as Bitmap, null);
                buted2.Checked = map.EDSMRegionsOutlineEnable;
                buted2.Enabled = map.EDSMRegionsEnable;
                buted2.ToolTipText = "Enable Region Outlines";
                buted2.CheckChanged += (e1) => { map.EDSMRegionsOutlineEnable = !map.EDSMRegionsOutlineEnable; };
                edsmregionsgb.Add(buted2);

                GLCheckBox buted3 = new GLCheckBox("EDSMR3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionShading") as Bitmap, null);
                buted3.Checked = map.EDSMRegionsShadingEnable;
                buted3.Enabled = map.EDSMRegionsEnable;
                buted3.ToolTipText = "Enable Region Shading";
                buted3.CheckChanged += (e1) => { map.EDSMRegionsShadingEnable = !map.EDSMRegionsShadingEnable; };
                edsmregionsgb.Add(buted3);

                GLCheckBox buted4 = new GLCheckBox("EDSMR4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionNames") as Bitmap, null);
                buted4.Checked = map.EDSMRegionsTextEnable;
                buted4.Enabled = map.EDSMRegionsEnable;
                buted4.ToolTipText = "Enable Region Naming";
                buted4.CheckChanged += (e1) => { map.EDSMRegionsTextEnable = !map.EDSMRegionsTextEnable; };
                edsmregionsgb.Add(buted4);

                // elite regions

                GLGroupBox eliteregionsgb = new GLGroupBox("ELITER", "Elite Regions", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                eliteregionsgb.ClientHeight = iconsize + 8;
                eliteregionsgb.BackColor = pform.BackColor;
                eliteregionsgb.ForeColor = Color.Orange;
                pform.Add(eliteregionsgb);
                vpos += eliteregionsgb.Height + ypad;

                GLCheckBox butelre = new GLCheckBox("ELITERE", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butelre.ToolTipText = "Enable Elite Regions";
                butelre.Checked = map.EliteRegionsEnable;
                butelre.UserCanOnlyCheck = true;
                eliteregionsgb.Add(butelre);

                GLCheckBox butel2 = new GLCheckBox("ELITER2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionOutlines") as Bitmap, null);
                butel2.Checked = map.EliteRegionsOutlineEnable;
                butel2.Enabled = map.EliteRegionsEnable;
                butel2.ToolTipText = "Enable Region Outlines";
                butel2.CheckChanged += (e1) => { map.EliteRegionsOutlineEnable = !map.EliteRegionsOutlineEnable; };
                eliteregionsgb.Add(butel2);

                GLCheckBox butel3 = new GLCheckBox("ELITER3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionShading") as Bitmap, null);
                butel3.Checked = map.EliteRegionsShadingEnable;
                butel3.Enabled = map.EliteRegionsEnable;
                butel3.ToolTipText = "Enable Region Shading";
                butel3.CheckChanged += (e1) => { map.EliteRegionsShadingEnable = !map.EliteRegionsShadingEnable; };
                eliteregionsgb.Add(butel3);

                GLCheckBox butel4 = new GLCheckBox("ELITER4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.RegionNames") as Bitmap, null);
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

        static void Theme(GLBaseControl s)      // run on each control during add, theme it
        {
            var cb = s as GLCheckBox;
            if (cb != null)
            {
                float[][] colorMatrixElements = {
                           new float[] {0.5f,  0,  0,  0, 0},        // red scaling factor of 0.5
                           new float[] {0,  0.5f,  0,  0, 0},        // green scaling factor of 1
                           new float[] {0,  0,  0.5f,  0, 0},        // blue scaling factor of 1
                           new float[] {0,  0,  0,  1, 0},        // alpha scaling factor of 1
                           new float[] {0.0f, 0.0f, 0.0f, 0, 1}};    // three translations of 

                var colormap1 = new System.Drawing.Imaging.ColorMap();
                cb.SetDrawnBitmapUnchecked(new System.Drawing.Imaging.ColorMap[] { colormap1 }, colorMatrixElements);
            }
            else
            {
                var but = s as GLButton;
                if (but != null)
                    but.ForeColor = Color.DarkOrange;
            }
        }


    }
}
