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
using System.Drawing;

namespace EDDiscovery.UserControls.Map3D
{
    public class MapMenu
    {
        private Map map;
        private GLLabel status;
        private const int iconsize = 32;

        public const string EntryTextName = "MSEntryText";

        public MapMenu(Map g)
        {
            map = g;

            // names of MS* are on screen items hidden during main menu presentation

            GLImage menuimage = new GLImage("MSMainMenu", new Rectangle(10, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.hamburgermenu") as Bitmap);
            menuimage.ToolTipText = "Open configuration menu";
            map.displaycontrol.Add(menuimage);
            menuimage.MouseClick = (o, e1) => { ShowMenu(); };

            GLImage tpback = new GLImage("MSTPBack", new Rectangle(50, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoBackward") as Bitmap);
            tpback.ToolTipText = "Go back one system";
            map.displaycontrol.Add(tpback);
            tpback.MouseClick = (o, e1) => { g.GoToTravelSystem(-1); };

            GLImage tphome = new GLImage("MSTPHome", new Rectangle(90, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoToHomeSystem") as Bitmap);
            tphome.ToolTipText = "Go to current home system";
            map.displaycontrol.Add(tphome);
            tphome.MouseClick = (o, e1) => { g.GoToTravelSystem(0); };

            GLImage tpforward = new GLImage("MSTPForward", new Rectangle(130, 10, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.GoForward") as Bitmap);
            tpforward.ToolTipText = "Go forward one system";
            map.displaycontrol.Add(tpforward);
            tpforward.MouseClick = (o, e1) => { g.GoToTravelSystem(1); };

            GLTextBoxAutoComplete tptextbox = new GLTextBoxAutoComplete(EntryTextName, new Rectangle(170, 10, 300, iconsize), "");
            tptextbox.TextAlign = ContentAlignment.MiddleLeft;
            tptextbox.BackColor = Color.FromArgb(96,50,50,50);
            tptextbox.BorderColor = Color.Gray;
            tptextbox.BorderWidth = 1;
            map.displaycontrol.Add(tptextbox);

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

        public void ShowMenu()
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

            // and closing animation
            pform.FormClosing += (f,e) => { 
                e.Handled = true;       // stop close
                var ani = new AnimateScale(map.ElapsedTimems + 10, map.ElapsedTimems + 400, new SizeF(0, 0));       // add a close animation
                ani.FinishAction += (a, c, t) => { pform.ForceClose(); };   // when its complete, force close
                pform.Animators.Add(ani); 
            };

            {   // top buttons
                GLPanel p3d2d = new GLPanel("3d2d", new Rectangle(leftmargin, vpos, 80, iconsize), Color.Transparent);

                GLCheckBox but3d = new GLCheckBox("3d", new Rectangle(0, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.3d") as Bitmap, null);
                but3d.Checked = map.gl3dcontroller.MatrixCalc.InPerspectiveMode;
                but3d.ToolTipText = "3D View";
                but3d.GroupRadioButton = true;
                but3d.MouseClick += (e1, e2) => { map.gl3dcontroller.ChangePerspectiveMode(true); };
                p3d2d.Add(but3d);

                GLCheckBox but2d = new GLCheckBox("2d", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.2d") as Bitmap, null);
                but2d.Checked = !map.gl3dcontroller.MatrixCalc.InPerspectiveMode;
                but2d.ToolTipText = "2D View";
                but2d.GroupRadioButton = true;
                but2d.MouseClick += (e1, e2) => { map.gl3dcontroller.ChangePerspectiveMode(false); };
                p3d2d.Add(but2d);

                pform.Add(p3d2d);

                GLCheckBox butelite = new GLCheckBox("Elite", new Rectangle(100, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.EliteMovement") as Bitmap, null);
                butelite.ToolTipText = "Select elite movement (on Y plain)";
                butelite.Checked = map.gl3dcontroller.YHoldMovement;
                butelite.CheckChanged += (e1) => { map.gl3dcontroller.YHoldMovement = butelite.Checked; };
                pform.Add(butelite);

                GLCheckBox butgal = new GLCheckBox("Galaxy", new Rectangle(150, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butgal.ToolTipText = "Show galaxy image";
                butgal.Checked = map.GalaxyDisplay;
                butgal.CheckChanged += (e1) => { map.GalaxyDisplay = butgal.Checked; };
                pform.Add(butgal);

                GLCheckBox butsd = new GLCheckBox("StarDots", new Rectangle(200, vpos, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.StarDots") as Bitmap, null);
                butsd.ToolTipText = "Show star field";
                butsd.Checked = map.StarDotsDisplay;
                butsd.CheckChanged += (e1) => { map.StarDotsDisplay = butsd.Checked; };
                pform.Add(butsd);

                vpos += butgal.Height + ypad;
            }

            {
                GLGroupBox tpgb = new GLGroupBox("TravelPathGB", "Travel Path", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, iconsize *2));
                tpgb.BackColor = pform.BackColor;
                tpgb.ForeColor = Color.Orange;
                pform.Add(tpgb);

                //GLCheckBox buttp = new GLCheckBox("TravelPath", new Rectangle(leftmargin, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.StarDots") as Bitmap, null);
                //buttp.ToolTipText = "Show travel path";
                //buttp.Checked = map.TravelPathDisplay;
                //buttp.CheckChanged += (e1) => { map.TravelPathDisplay = buttp.Checked; };
                //tpgb.Add(buttp);

                //GLDateTimePicker dtps = new GLDateTimePicker("TPStart", new Rectangle(50, 0, 250, 30), DateTime.Now);
                //dtps.Font = new Font("Ms Sans Serif", 8.25f);
                //dtps.ShowCheckBox = dtps.ShowCalendar = true;
                //dtps.Value = map.TravelPathStartDate;
                //dtps.Checked = map.TravelPathStartDateEnable;
                //dtps.ValueChanged += (e1) => { map.TravelPathStartDate = dtps.Value; map.TravelPathRefresh(); };
                //dtps.CheckChanged += (e1) => { map.TravelPathStartDateEnable = dtps.Checked; map.TravelPathRefresh(); };
                //dtps.ShowUpDown = true;
                //tpgb.Add(dtps);

                //GLDateTimePicker dtpe = new GLDateTimePicker("TPEnd", new Rectangle(320, 0, 250, 30), DateTime.Now);
                //dtpe.Font = new Font("Ms Sans Serif", 8.25f);
                //dtpe.ShowCheckBox = dtps.ShowCalendar = true;
                //dtpe.Value = map.TravelPathEndDate;
                //dtpe.Checked = map.TravelPathEndDateEnable;
                //dtpe.ValueChanged += (e1) => { map.TravelPathEndDate = dtpe.Value; map.TravelPathRefresh(); };
                //dtpe.CheckChanged += (e1) => { map.TravelPathEndDateEnable = dtpe.Checked; map.TravelPathRefresh(); };
                //dtpe.ShowUpDown = true;
                //tpgb.Add(dtpe);


                vpos += tpgb.Height + ypad;
            }

            { // Galaxy objects
                GLGroupBox galgb = new GLGroupBox("GalGB", "Galaxy Objects", new Rectangle(leftmargin, vpos, pform.ClientWidth - leftmargin * 2, 50));
                galgb.ClientHeight = (iconsize + 4) * 2;
                galgb.BackColor = pform.BackColor;
                galgb.ForeColor = Color.Orange;
                pform.Add(galgb);
                GLFlowLayoutPanel galfp = new GLFlowLayoutPanel("GALFP", DockingType.Fill, 0);
                galfp.FlowPadding = new Padding(2, 2, 2, 2);
                galfp.BackColor = pform.BackColor;
                galgb.Add(galfp);

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

                vpos += galgb.Height + ypad;
            }

            { // EDSM regions
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

                GLCheckBox buted2 = new GLCheckBox("EDSMR2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                buted2.Checked = map.EDSMRegionsOutlineEnable;
                buted2.Enabled = map.EDSMRegionsEnable;
                buted2.ToolTipText = "Enable Region Outlines";
                buted2.CheckChanged += (e1) => { map.EDSMRegionsOutlineEnable = !map.EDSMRegionsOutlineEnable; };
                edsmregionsgb.Add(buted2);

                GLCheckBox buted3 = new GLCheckBox("EDSMR3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                buted3.Checked = map.EDSMRegionsShadingEnable;
                buted3.Enabled = map.EDSMRegionsEnable;
                buted3.ToolTipText = "Enable Region Shading";
                buted3.CheckChanged += (e1) => { map.EDSMRegionsShadingEnable = !map.EDSMRegionsShadingEnable; };
                edsmregionsgb.Add(buted3);

                GLCheckBox buted4 = new GLCheckBox("EDSMR4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
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

                GLCheckBox butel2 = new GLCheckBox("ELITER2", new Rectangle(50, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butel2.Checked = map.EliteRegionsOutlineEnable;
                butel2.Enabled = map.EliteRegionsEnable;
                butel2.ToolTipText = "Enable Region Outlines";
                butel2.CheckChanged += (e1) => { map.EliteRegionsOutlineEnable = !map.EliteRegionsOutlineEnable; };
                eliteregionsgb.Add(butel2);

                GLCheckBox butel3 = new GLCheckBox("ELITER3", new Rectangle(100, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
                butel3.Checked = map.EliteRegionsShadingEnable;
                butel3.Enabled = map.EliteRegionsEnable;
                butel3.ToolTipText = "Enable Region Shading";
                butel3.CheckChanged += (e1) => { map.EliteRegionsShadingEnable = !map.EliteRegionsShadingEnable; };
                eliteregionsgb.Add(butel3);

                GLCheckBox butel4 = new GLCheckBox("ELITER4", new Rectangle(150, 0, iconsize, iconsize), BaseUtils.Icons.IconSet.Instance.Get("GalMap.ShowGalaxy") as Bitmap, null);
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

            map.displaycontrol.Add(pform);
        }

        public void UpdateCoords(GLOFC.GLMatrixCalc c)
        {
            status.Text = c.TargetPosition.X.ToStringInvariant("N1") + " ," + c.TargetPosition.Y.ToStringInvariant("N1") + " ,"
                         + c.TargetPosition.Z.ToStringInvariant("N1") + " Dist " + c.EyeDistance.ToStringInvariant("N1") + " Eye " +
                         c.EyePosition.X.ToStringInvariant("N1") + " ," + c.EyePosition.Y.ToStringInvariant("N1") + " ," + c.EyePosition.Z.ToStringInvariant("N1");
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
        }


    }
}
