﻿/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous.JournalEvents;
using ExtendedControls;
using EDDiscovery.DB;
using System.Drawing.Drawing2D;
using EDDiscovery.EliteDangerous;
using EDDiscovery2.EDSM;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;
        private int displaynumber = 0;
        Size starsize,planetsize,moonsize,materialsize;
        Size itemsepar;
        int leftmargin;
        int topmargin;
        const int materialspacer = 4;

        Font stdfont = new Font("Microsoft Sans Serif", 8.25F);
        Font stdfontUnderline  = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);

        private string DbSave { get { return "ScanPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        StarScan.SystemNode last_sn = null;
        HistoryList hl = null;
        HistoryEntry last_he = null;
        Point last_maxdisplayarea;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            Name = "Scan";
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            richTextBoxInfo.Visible = false;
            toolTip.ShowAlways = true;
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;
            travelhistorycontrol.OnTravelSelectionChanged += Display;
            discoveryform.OnNewEntry += NewEntry;

            checkBoxMaterials.Checked = SQLiteDBClass.GetSettingBool(DbSave+"Materials", true);
            checkBoxMaterialsRare.Checked = SQLiteDBClass.GetSettingBool(DbSave+"MaterialsRare", false);
            checkBoxMoons.Checked = SQLiteDBClass.GetSettingBool(DbSave+"Moons", true);
            checkBoxEDSM.Checked = SQLiteDBClass.GetSettingBool(DbSave + "EDSM", false);

            int size = SQLiteDBClass.GetSettingInt(DbSave+"Size", 64);
            SetSizeCheckBoxes(size);

            imagebox.ClickElement += ClickElement;
        }

        public override void LoadLayout()
        {
        }

        public override void Closing()
        {
            travelhistorycontrol.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        Color transparencycolor = Color.Green;
        public override Color ColorTransparency {  get { return transparencycolor; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            imagebox.BackColor = this.BackColor = panelStars.BackColor = panelStars.vsc.SliderColor = panelStars.vsc.BackColor = panelControls.BackColor = curcol;
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            PositionInfo();
            //System.Diagnostics.Debug.WriteLine("Resize panel stars {0} {1}", DisplayRectangle, panelStars.Size);

            if (last_sn != null)
            {
                int newspace = panelStars.Width - panelStars.ScrollBarWidth;

                if (newspace < last_maxdisplayarea.X || newspace > last_maxdisplayarea.X + starsize.Width * 2)
                {
                    DrawSystem(last_sn);
                }
            }
        }

        #endregion

        #region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {                                                                   // affecting our system
            StarScan.SystemNode newnode = (he != null) ? hl.starscan.FindSystem(he.System) : null;  // find node..
            last_he = he;

            if ( he.EntryType == EliteDangerous.JournalTypeEnum.Scan )  // if on same star system, and its a scan, it may have been updated..
            {
                if (newnode == last_sn || (newnode != null && last_sn != null && newnode.system != null && newnode.system.Equals(last_sn.system)))
                {
                    DrawSystem(last_sn);
                }
            }
        }

        public override void Display(HistoryEntry he, HistoryList hl)            // when user clicks around..
        {
            StarScan.SystemNode newnode = (he != null) ? hl.starscan.FindSystem(he.System) : null;
            last_he = he;
            this.hl = hl;

            if (newnode != last_sn)
            {
                last_sn = newnode;
                DrawSystem(last_sn);
            }
        }

        void DrawSystem(StarScan.SystemNode sn)
        {
            HideInfo();

                                                                 // remember in case we need to draw

            SetControlText((sn == null) ? "No Scan" : sn.system.name);

            imagebox.ClearImageList();  // does not clear the image, render will do that

            if (checkBoxEDSM.Checked && last_he!=null)
            {
                if (last_he.System.id_edsm > 0)  // If system has edsmid get bodies
                {
                    if (sn==null  || (sn != null && sn.EDSMAdded == false))
                    {
                        List<JournalScan> jl = EDSMClass.GetBodiesList((int)last_he.System.id_edsm);


                        if (jl != null)
                            foreach (JournalScan js in jl)
                                hl.starscan.Process(js, last_he.System);


                        if (sn == null)
                            sn = hl.starscan.FindSystem(last_he.System);

                        if (sn!=null)
                            sn.EDSMAdded = true;  // So we dont add it anymore this runtime.
                    }
                }
            }


            last_sn = sn;
            if (sn != null)     // 
            {
                Point curpos = new Point(leftmargin, topmargin);
                last_maxdisplayarea = curpos;

                List<PictureBoxHotspot.ImageElement> starcontrols = new List<PictureBoxHotspot.ImageElement>();

                //for( int i = 0; i < 1000; i +=100)  CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, new Point(i, 0), new Size(24, 24), i.ToString(), "");

                foreach (StarScan.ScanNode starnode in sn.starnodes.Values)        // always has scan nodes
                {
                    int offset = 0;
                    Point maxstarpos = DrawNode(starcontrols, starnode,
                                (starnode.type == StarScan.ScanNodeType.barycentre) ? EDDiscovery.Properties.Resources.Barycentre : JournalScan.GetStarImageNotScanned(),
                                curpos, starsize, ref offset , false, (planetsize.Height*6/4-starsize.Height)/2);       // the last part nerfs the label down to the right position

                    Point maxitemspos = maxstarpos;

                    curpos = new Point(maxitemspos.X + itemsepar.Width, curpos.Y);   // move to the right
                    curpos.Y += starsize.Height / 2 - planetsize.Height * 3 / 4;     // slide down for planet vs star difference in size

                    Point firstcolumn = curpos;

                    if (starnode.children != null)
                    {
                        foreach (StarScan.ScanNode planetnode in starnode.children.Values)
                        {
                            List<PictureBoxHotspot.ImageElement> pc = new List<PictureBoxHotspot.ImageElement>();

                            Point maxpos = CreatePlanetTree(pc, planetnode, curpos);

                            //System.Diagnostics.Debug.WriteLine("Planet " + planetnode.ownname + " " + curpos + " " + maxpos + " max " + (panelStars.Width - panelStars.ScrollBarWidth));

                            if (maxpos.X > panelStars.Width - panelStars.ScrollBarWidth)          // uh oh too wide..
                            {
                                int xoffset = firstcolumn.X - curpos.X;
                                int yoffset = maxitemspos.Y - curpos.Y;

                                RepositionTree(pc, xoffset, yoffset);        // shift co-ords of all you've drawn

                                maxpos = new Point(maxpos.X + xoffset, maxpos.Y + yoffset);
                                curpos = new Point(maxpos.X, curpos.Y + yoffset);
                            }
                            else
                                curpos = new Point(maxpos.X, curpos.Y);

                            maxitemspos = new Point(Math.Max(maxitemspos.X, maxpos.X), Math.Max(maxitemspos.Y, maxpos.Y));

                            starcontrols.AddRange(pc.ToArray());
                        }
                    }

                    last_maxdisplayarea = new Point(Math.Max(last_maxdisplayarea.X, maxitemspos.X), Math.Max(last_maxdisplayarea.Y, maxitemspos.Y));
                    curpos = new Point(leftmargin, maxitemspos.Y + itemsepar.Height);
                }

                imagebox.AddRange(starcontrols);
            }

            imagebox.Render();      // replaces image..
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<PictureBoxHotspot.ImageElement> pc, StarScan.ScanNode planetnode, Point curpos )
        {
            // PLANETWIDTH|PLANETWIDTH  (if drawing a full planet with rings/landing)
            // or
            // MOONWIDTH|MOONWIDTH      (if drawing a single width planet)
                                                                          // this offset, ONLY used if a single width planet, allows for two moons
            int offset = moonsize.Width - planetsize.Width / 2;           // centre is moon width, back off by planetwidth/2 to place the left edge of the planet

            Point maxtreepos = DrawNode(pc, planetnode, JournalScan.GetPlanetImageNotScanned(), 
                                curpos, planetsize, ref offset, true);        // offset passes in the suggested offset, returns the centre offset

            if (planetnode.children != null && checkBoxMoons.Checked)
            {
                offset -= moonsize.Width;               // offset is centre of planet image, back off by a moon width to allow for 2 moon widths centred

                Point moonpos = new Point(curpos.X + offset, maxtreepos.Y + itemsepar.Height);    // moon pos

                foreach (StarScan.ScanNode moonnode in planetnode.children.Values)
                {
                    int offsetm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                    Point mmax = DrawNode(pc, moonnode, JournalScan.GetMoonImageNotScanned(), moonpos, moonsize, ref offsetm);

                    maxtreepos = new Point(Math.Max(maxtreepos.X, mmax.X), Math.Max(maxtreepos.Y, mmax.Y));

                    if (moonnode.children != null)
                    {
                        Point submoonpos;

                        if (mmax.X <= moonpos.X + moonsize.Width * 2)           // if we have nothing wider than the 2 moon widths, we can go with it right aligned
                            submoonpos = new Point(moonpos.X + moonsize.Width * 2 + itemsepar.Width, moonpos.Y);    // moon pos
                        else
                            submoonpos = new Point(moonpos.X + moonsize.Width * 2 + itemsepar.Width, mmax.Y + itemsepar.Height);    // moon pos below and right

                        foreach (StarScan.ScanNode submoonnode in moonnode.children.Values)
                        {
                            int offsetsm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                            Point sbmax = DrawNode(pc, submoonnode, JournalScan.GetMoonImageNotScanned(), submoonpos, moonsize, ref offsetsm);

                            maxtreepos = new Point(Math.Max(maxtreepos.X, sbmax.X), Math.Max(maxtreepos.Y, sbmax.Y));

                            submoonpos = new Point(submoonpos.X, maxtreepos.Y + itemsepar.Height);
                        }
                    }

                    moonpos = new Point(moonpos.X, maxtreepos.Y + itemsepar.Height);
                }
            }

            return maxtreepos;
        }

        // Width:  Nodes are allowed 2 widths 
        // Height: Nodes are allowed 1.5 Heights.  0 = top, 1/2/3/4 = image, 5 = bottom.  
        // offset: pass in horizonal offset, return back middle of image
        // aligndown : if true, compensate for drawing normal size images and ones 1.5 by shifting down the image and the label by the right amounts
        // labelvoff : any additional compensation for label pos

        // return right bottom of area used from curpos
        Point DrawNode(List<PictureBoxHotspot.ImageElement> pc, StarScan.ScanNode sn, Image notscanned, Point curpos, 
                                    Size size, ref int offset , bool aligndown = false , int labelvoff = 0 )
        {
            //System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + curpos + " " + size + " hoff " + offset );
            bool eddnvisible = true;
            string tip;
            Point endpoint = curpos;
            int quarterheight = size.Height / 4;
            int alignv = aligndown ? quarterheight : 0;

            JournalScan sc = sn.ScanData;


            if (sc != null)
            {
                if (!checkBoxEDSM.Checked && sc.IsEDSMBody)  // Dont draw if EDDN  is loaded and npot shown
                    eddnvisible = false;

                tip = sc.DisplayString(true);

                if (sc.IsStar)
                {
                    endpoint = CreateImageLabel(pc, sc.GetStarTypeImage().Item1, 
                                                new Point(curpos.X+offset, curpos.Y + alignv) ,      // WE are basing it on a 1/4 + 1 + 1/4 grid, this is not being made bigger, move off
                                                size, sn.ownname, tip, alignv + labelvoff, eddnvisible, sc.IsEDSMBody);          // and the label needs to be a quarter height below it..

                    if ( sc.HasRings && eddnvisible )
                    {
                        curpos = new Point(endpoint.X + itemsepar.Width, curpos.Y);

                        Point endbelt = curpos;

                        for (int i = 0; i < sc.Rings.Length; i++)
                        {
                            string name = sc.Rings[i].Name;
                            if (name.Length > sn.fullname.Length && name.Substring(0, sn.fullname.Length).Equals(sn.fullname))
                                name = name.Substring(sn.fullname.Length).Trim();

                            curpos.X += 4;      // a little spacing, image is tight

                            endbelt = CreateImageLabel(pc, EDDiscovery.Properties.Resources.Belt, 
                                new Point( curpos.X, curpos.Y + alignv ), new Size(size.Width/2,size.Height), name,
                                                                sc.RingInformationMoons(i), alignv + labelvoff, eddnvisible, sc.IsEDSMBody);

                            curpos = new Point(endbelt.X + itemsepar.Width, curpos.Y);
                        }

                        endpoint = new Point(curpos.X, endpoint.Y);
                    }

                    offset += size.Width / 2;       // return the middle used was this..
                }
                else
                {
                    bool indicatematerials = sc.HasMaterials && !checkBoxMaterials.Checked;

                    Image nodeimage = sc.GetPlanetClassImage();

                    if ((sc.IsLandable || sc.HasRings || indicatematerials) && eddnvisible)
                    {
                        Bitmap bmp = new Bitmap(size.Width * 2, quarterheight * 6);          

                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.DrawImage(nodeimage, size.Width / 2, quarterheight, size.Width, size.Height);

                            if (sc.IsLandable)
                                g.DrawImage(EDDiscovery.Properties.Resources.planet_landing, new Rectangle(quarterheight, 0, quarterheight * 6, quarterheight * 6));

                            if (sc.HasRings)
                                g.DrawImage(sc.Rings.Count() > 1 ? EDDiscovery.Properties.Resources.RingGap512 : EDDiscovery.Properties.Resources.Ring_Only_512,
                                                new Rectangle(-2, quarterheight, size.Width * 2, size.Height));

                            if (indicatematerials)
                            {
                                Image mm = EDDiscovery.Properties.Resources.materiamoreindicator;
                                g.DrawImage(mm, new Rectangle(bmp.Width - mm.Width, bmp.Height - mm.Height, mm.Width, mm.Height));
                            }
                        }

                        endpoint = CreateImageLabel(pc, bmp, curpos, new Size(bmp.Width, bmp.Height), sn.ownname, tip, labelvoff, eddnvisible, sc.IsEDSMBody);
                        offset = size.Width;                                        // return that the middle is now this
                    }
                    else
                    {
                        endpoint = CreateImageLabel(pc, nodeimage, new Point(curpos.X + offset, curpos.Y + alignv), size, 
                                                    sn.ownname, tip, alignv + labelvoff, eddnvisible, sc.IsEDSMBody);
                        offset += size.Width / 2;
                    }

                    if (sc.HasMaterials && checkBoxMaterials.Checked && eddnvisible)
                    {
                        Point matpos = new Point(endpoint.X + 4, curpos.Y);
                        Point endmat = CreateMaterialNodes(pc, sc, matpos, materialsize);
                        endpoint = new Point(Math.Max(endpoint.X, endmat.X), Math.Max(endpoint.Y, endmat.Y)); // record new right point..
                    }
                }
            }
            else
            {
                if (sn.type == StarScan.ScanNodeType.barycentre)
                    tip = "Barycentre of " + sn.ownname;
                else
                    tip = sn.ownname + "\n\nNo scan data available";

                endpoint = CreateImageLabel(pc, notscanned, new Point(curpos.X + offset, curpos.Y + alignv), size, sn.ownname, tip , alignv + labelvoff, eddnvisible, false);
                offset += size.Width / 2;       // return the middle used was this..
            }

            return endpoint;
        }


        Point CreateMaterialNodes(List<PictureBoxHotspot.ImageElement> pc, JournalScan sn, Point matpos, Size matsize)
        {
            Point startpos = matpos;
            Point maximum = matpos;
            int noperline = 0;

            bool noncommon = checkBoxMaterialsRare.Checked;

            string matclicktext = sn.DisplayMaterials(2);

            foreach (KeyValuePair<string, double> sd in sn.Materials)
            {
                string abv = sd.Key.Substring(0, 1);
                string tooltip = sd.Key;
                Color fillc = Color.Yellow;

                EDDiscovery2.DB.MaterialCommodities mc = EDDiscovery2.DB.MaterialCommodities.GetCachedMaterial(sd.Key);
                if (mc != null)
                {
                    abv = mc.shortname;
                    fillc = mc.colour;
                    tooltip = mc.name + " (" + mc.shortname + ") " + mc.type + " " + sd.Value.ToString("0.0") + "%";

                    if (noncommon && mc.type.IndexOf("common", StringComparison.InvariantCultureIgnoreCase) >= 0)
                        continue;
                }

                CreateMaterialImage(pc, matpos, matsize, abv, tooltip + "\n\n" + "All " + matclicktext, tooltip, fillc, Color.Black);

                maximum = new Point(Math.Max(maximum.X, matpos.X + matsize.Width), Math.Max(maximum.Y, matpos.Y + matsize.Height));

                if (++noperline == 4)
                {
                    matpos = new Point(startpos.X, matpos.Y + matsize.Height + materialspacer);
                    noperline = 0;
                }
                else
                    matpos.X += matsize.Width + materialspacer;
            }

            return maximum;
        }

        void CreateMaterialImage(List<PictureBoxHotspot.ImageElement> pc, Point matpos, Size matsize, string text, string mattag, string mattip, Color matcolour , Color textcolour)
        {
            System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
            colormap.OldColor = Color.White;    // this is the marker colour to replace
            colormap.NewColor = matcolour;

            Bitmap mat = ControlHelpers.ReplaceColourInBitmap(EDDiscovery.Properties.Resources.materialmarkerorangefilled, new System.Drawing.Imaging.ColorMap[] { colormap });

            ControlHelpers.DrawTextCentreIntoBitmap(ref mat, text, stdfont, textcolour);

            PictureBoxHotspot.ImageElement ie = new PictureBoxHotspot.ImageElement(  
                            new Rectangle(matpos.X, matpos.Y, matsize.Width, matsize.Height), mat, mattag, mattip);

            pc.Add(ie);
        }

        Point CreateImageLabel(List<PictureBoxHotspot.ImageElement> c, Image i, Point postopright, Size size, string label,
                                    string ttext , int labelhoff, bool visible, bool fromEDSM)
        {
            //System.Diagnostics.Debug.WriteLine("    " + label + " " + postopright + " size " + size + " hoff " + labelhoff + " laby " + (postopright.Y + size.Height + labelhoff));
            if (fromEDSM)
                ttext = "From EDSM" + Environment.NewLine + ttext;

            PictureBoxHotspot.ImageElement ie = new PictureBoxHotspot.ImageElement(new Rectangle(postopright.X, postopright.Y, size.Width, size.Height), i, ttext, ttext);

            Point max = new Point(postopright.X + size.Width, postopright.Y + size.Height);

            if (label != null)
            {
                Font font = stdfont;
                if (fromEDSM)
                    font = stdfontUnderline;
                    //label = "("+label+")";

                Point labposcenthorz = new Point(postopright.X + size.Width / 2, postopright.Y + size.Height + labelhoff);

                PictureBoxHotspot.ImageElement lab = new PictureBoxHotspot.ImageElement();
                Size maxsize = new Size(300, 20);

                lab.TextCentreAutosize(labposcenthorz, maxsize, label, font, discoveryform.theme.LabelColor, this.BackColor );

                if (lab.pos.X < postopright.X)
                {
                    int offset = postopright.X - lab.pos.X;
                    ie.Translate(offset, 0);
                    lab.Translate(offset, 0);
                }

                if (visible)
                    c.Add(lab);

                max = new Point(Math.Max(lab.pos.X + lab.pos.Width, max.X), lab.pos.Y + lab.pos.Height);
            }
            
            if (visible)
                c.Add(ie);

            //System.Diagnostics.Debug.WriteLine(" ... to " + label + " " + max + " size " + (new Size(max.X-postopright.X,max.Y-postopright.Y)));
            return max;
        }

        void RepositionTree(List<PictureBoxHotspot.ImageElement> pc, int xoff, int yoff)
        {
            foreach (PictureBoxHotspot.ImageElement c in pc)
                c.Translate(xoff, yoff);
        }

        void SetSize(int stars)
        {
            starsize = new Size(stars, stars);
            planetsize = new Size(starsize.Width * 3 / 4, starsize.Height * 3 / 4);
            moonsize = new Size(starsize.Width * 2 / 4, starsize.Height * 2 / 4);
            materialsize = new Size(24, 24);
            itemsepar = new Size(stars/16, stars/16);
            topmargin = 10;
            leftmargin = 0;
        }

#endregion

#region User interaction

        private void ClickElement(object sender, MouseEventArgs e, PictureBoxHotspot.ImageElement i , object tag)
        {
            if (i != null)
                ShowInfo((string)tag, i.pos.Location.X < panelStars.Width / 2);
            else
                HideInfo();
        }

        private void checkBoxMaterials_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "Materials", checkBoxMaterials.Checked);
            DrawSystem(last_sn);
        }

        private void checkBoxMaterialsRare_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "MaterialsRare", checkBoxMaterialsRare.Checked);
            if (checkBoxMaterials.Checked == false)
                checkBoxMaterials.Checked = true;       // will trigger above, and cause a redraw.
            else
                DrawSystem(last_sn);
        }

        private void checkBoxMoons_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "Moons", checkBoxMoons.Checked);
            DrawSystem(last_sn);
        }

        bool userchangesize = true;
        void SetSizeCheckBoxes(int size)
        {
            userchangesize = false;
            checkBoxLarge.Checked = (size == 128);
            checkBoxMedium.Checked = (size == 96);
            checkBoxSmall.Checked = (size == 64);
            checkBoxTiny.Checked = (size == 48);

            if (!checkBoxLarge.Checked && !checkBoxMedium.Checked && !checkBoxSmall.Checked && !checkBoxTiny.Checked)
            {
                checkBoxSmall.Checked = true;
                size = 64;
            }
            userchangesize = true;
            SetSize(size);
            SQLiteDBClass.PutSettingInt(DbSave + "Size", size);
            DrawSystem(last_sn);
        }

        private void checkBoxLarge_CheckedChanged(object sender, EventArgs e)
        {
            if (userchangesize)
                SetSizeCheckBoxes(128);
        }

        private void checkBoxMedium_CheckedChanged(object sender, EventArgs e)
        {
            if (userchangesize)
                SetSizeCheckBoxes(96);
        }

        private void checkBoxSmall_CheckedChanged(object sender, EventArgs e)
        {
            if (userchangesize)
                SetSizeCheckBoxes(64);
        }

        private void checkBoxTiny_CheckedChanged(object sender, EventArgs e)
        {
            if (userchangesize)
                SetSizeCheckBoxes(48);
        }

        private void toolStripMenuItemToolbar_Click(object sender, EventArgs e)
        {
            panelControls.Visible = !panelControls.Visible;
        }

        void ShowInfo(string text , bool onright )
        {
            richTextBoxInfo.Text = text;
            richTextBoxInfo.Tag = onright;
            richTextBoxInfo.Visible = true;
            richTextBoxInfo.Show();
            PositionInfo();
        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "EDSM", checkBoxEDSM.Checked);
            DrawSystem(last_sn);
        }

        void HideInfo()
        {
            richTextBoxInfo.Visible = false;
        }

        void PositionInfo()
        {
            if (richTextBoxInfo.Visible)
            {
                if (richTextBoxInfo.Tag != null && ((bool)richTextBoxInfo.Tag) == true)
                    richTextBoxInfo.Location = new Point(panelStars.Width / 2 + panelStars.Width / 16, 10);
                else
                    richTextBoxInfo.Location = new Point(panelStars.Width / 16, 10);

                int h = Math.Min(richTextBoxInfo.EstimateVerticalSizeFromText(), panelStars.Height - 20);

                richTextBoxInfo.Size = new Size(panelStars.Width * 6 / 16, h);
                richTextBoxInfo.PerformLayout();    // not sure why i need this..
            }
        }

        private void panelStars_Click(object sender, EventArgs e)
        {
            HideInfo();
        }

#endregion

    }
}

