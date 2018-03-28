/*
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
 * EDDiscovery is not affiliated with Frontier Developments plc.
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
using ExtendedControls;
using System.Drawing.Drawing2D;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        Size starsize, beltsize, planetsize, moonsize, materialsize;
        Size itemsepar;
        int leftmargin;
        int topmargin;
        const int materialspacer = 4;

        Font stdfont = new Font("Microsoft Sans Serif", 8.25F);
        Font stdfontUnderline = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);

        private string DbSave { get { return "ScanPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        HistoryEntry last_he = null;
        Point last_maxdisplayarea;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            rtbNodeInfo.Visible = false;
            toolTip.ShowAlways = true;
        }

        public override void Init()
        {
            progchange = true;
            checkBoxMaterials.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Materials", true);
            checkBoxMaterialsRare.Checked = SQLiteDBClass.GetSettingBool(DbSave + "MaterialsRare", false);
            checkBoxMoons.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Moons", true);
            checkBoxEDSM.Checked = SQLiteDBClass.GetSettingBool(DbSave + "EDSM", false);
            chkShowOverlays.Checked = SQLiteDBClass.GetSettingBool(DbSave + "BodyOverlays", false);
            progchange = false;

            int size = SQLiteDBClass.GetSettingInt(DbSave + "Size", 64);
            SetSizeCheckBoxes(size);

            uctg.OnTravelSelectionChanged += Display;
            discoveryform.OnNewEntry += NewEntry;

            imagebox.ClickElement += ClickElement;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }
		
		#endregion
		
		
		#region Transparency
        Color transparencycolor = Color.Green;
        public override Color ColorTransparency { get { return transparencycolor; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            imagebox.BackColor = this.BackColor = panelStars.BackColor = panelStars.vsc.SliderColor = panelStars.vsc.BackColor = panelControls.BackColor = curcol;
			rollUpPanelTop.BackColor = curcol;
			rollUpPanelTop.ShowHiddenMarker = !on;
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            PositionInfo();
            //System.Diagnostics.Debug.WriteLine("Resize panel stars {0} {1}", DisplayRectangle, panelStars.Size);

            if (last_he != null)
            {
                int newspace = panelStars.Width - panelStars.ScrollBarWidth;

                if (newspace < last_maxdisplayarea.X || newspace > last_maxdisplayarea.X + starsize.Width * 2)
                {
                    DrawSystem();
                }
            }
        }

        #endregion

        #region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.EntryType == JournalTypeEnum.Scan))
            {
                last_he = he;
                DrawSystem();
            }
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl)            // when user clicks around..
        {
            if (he != null && (last_he == null || he.System != last_he.System))
            {
                last_he = he;
                DrawSystem();
            }
        }

        void DrawSystem()   // draw last_sn, last_he
        {
            HideInfo();

            imagebox.ClearImageList();  // does not clear the image, render will do that
            lblSystemInfo.Text = "";

            if (last_he == null)
            {
                SetControlText("No System");
                imagebox.Render();
                return;
            }

            StarScan.SystemNode last_sn = discoveryform.history.starscan.FindSystem(last_he.System, checkBoxEDSM.Checked);

            SetControlText((last_sn == null) ? "No Scan" : last_sn.system.Name);

            if (last_sn != null)     // 
            {
                BuildSystemInfo(last_sn);

                Point curpos = new Point(leftmargin, topmargin);
                last_maxdisplayarea = curpos;
                List<PictureBoxHotspot.ImageElement> starcontrols = new List<PictureBoxHotspot.ImageElement>();

                //for( int i = 0; i < 1000; i +=100)  CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, new Point(i, 0), new Size(24, 24), i.ToString(), "");

                foreach (StarScan.ScanNode starnode in last_sn.starnodes.Values)        // always has scan nodes
                {
                    //System.Diagnostics.Debug.WriteLine("Draw " + starnode.type);
                    int offset = 0;
                    Point maxstarpos = DrawNode(starcontrols, starnode,
                                (starnode.type == StarScan.ScanNodeType.barycentre) ? Icons.Controls.Scan_Bodies_Barycentre : JournalScan.GetStarImageNotScanned(),
                                curpos, starsize, ref offset, false, (planetsize.Height * 6 / 4 - starsize.Height) / 2, true);       // the last part nerfs the label down to the right position

                    Point maxitemspos = maxstarpos;

                    curpos = new Point(maxitemspos.X + itemsepar.Width, curpos.Y);   // move to the right
                    curpos.Y += starsize.Height / 2 - planetsize.Height * 3 / 4;     // slide down for planet vs star difference in size

                    Point firstcolumn = curpos;

                    if (starnode.children != null)
                    {
                        Queue<StarScan.ScanNode> belts = null;
                        if (starnode.ScanData != null && (!starnode.ScanData.IsEDSMBody || checkBoxEDSM.Checked))
                        {
                            belts = new Queue<StarScan.ScanNode>(starnode.children.Values.Where(s => s.type == StarScan.ScanNodeType.belt));
                        }
                        else
                        {
                            belts = new Queue<StarScan.ScanNode>();
                        }

                        StarScan.ScanNode lastbelt = belts.Count != 0 ? belts.Dequeue() : null;

                        foreach (StarScan.ScanNode planetnode in starnode.children.Values.Where(s => s.type == StarScan.ScanNodeType.body || s.type == StarScan.ScanNodeType.star))
                        {
                           // System.Diagnostics.Debug.WriteLine("Draw " + planetnode.type);

                            while (lastbelt != null && planetnode.ScanData != null && (lastbelt.BeltData == null || lastbelt.BeltData.OuterRad < planetnode.ScanData.nSemiMajorAxis))
                            {
                                if (curpos.X + planetsize.Width > panelStars.Width - panelStars.ScrollBarWidth)
                                {
                                    curpos = new Point(firstcolumn.X, maxitemspos.Y + planetsize.Height);
                                }

                                DrawNode(starcontrols, lastbelt, Icons.Controls.Scan_Bodies_Belt,
                                         new Point(curpos.X + (planetsize.Width - beltsize.Width) / 2, curpos.Y), beltsize, ref offset, false);

                                curpos = new Point(curpos.X + planetsize.Width, curpos.Y);
                                lastbelt = belts.Count != 0 ? belts.Dequeue() : null;
                            }

                            bool nonedsmscans = planetnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                            //System.Diagnostics.Debug.WriteLine("Planet Node " + planetnode.ownname + " has scans " + nonedsmscans);

                            if (nonedsmscans || checkBoxEDSM.Checked)
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

                        while (lastbelt != null)
                        {
                            if (curpos.X + planetsize.Width > panelStars.Width - panelStars.ScrollBarWidth)
                            {
                                curpos = new Point(firstcolumn.X, maxitemspos.Y + planetsize.Height);
                            }

                            DrawNode(starcontrols, lastbelt, Icons.Controls.Scan_Bodies_Belt,
                                     new Point(curpos.X + (planetsize.Width - beltsize.Width) / 2, curpos.Y), beltsize, ref offset, false);

                            curpos = new Point(curpos.X + planetsize.Width, curpos.Y);
                            lastbelt = belts.Count != 0 ? belts.Dequeue() : null;
                        }
                    }

                    last_maxdisplayarea = new Point(Math.Max(last_maxdisplayarea.X, maxitemspos.X), Math.Max(last_maxdisplayarea.Y, maxitemspos.Y));
                    curpos = new Point(leftmargin, maxitemspos.Y + itemsepar.Height);
                }

                imagebox.AddRange(starcontrols);
            }

            imagebox.Render();      // replaces image..
        }

        private void BuildSystemInfo(StarScan.SystemNode system)
        {
            //systems are small... if they get too big and iterating repeatedly is a problem we'll have to move to a node-by-node approach, and move away from a single-line label
            lblSystemInfo.Text = BuildScanValue(system);
            
        }

        private string BuildScanValue(StarScan.SystemNode system)
        {
            var value = 0;

            foreach (var body in system.Bodies)
            {
                if (body?.ScanData?.EstimatedValue != null)
                {
                    value += body.ScanData.EstimatedValue;
                }
            }

            return $"Approx value: {value:N0}";
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<PictureBoxHotspot.ImageElement> pc, StarScan.ScanNode planetnode, Point curpos)
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

                foreach (StarScan.ScanNode moonnode in planetnode.children.Values.Where(n => n.type != StarScan.ScanNodeType.barycentre))
                {
                    bool nonedsmscans = moonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                    if (nonedsmscans || checkBoxEDSM.Checked)
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
                                bool nonedsmsubmoonscans = submoonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                                if (nonedsmsubmoonscans || checkBoxEDSM.Checked)
                                {
                                    int offsetsm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                                    Point sbmax = DrawNode(pc, submoonnode, JournalScan.GetMoonImageNotScanned(), submoonpos, moonsize, ref offsetsm);

                                    maxtreepos = new Point(Math.Max(maxtreepos.X, sbmax.X), Math.Max(maxtreepos.Y, sbmax.Y));

                                    submoonpos = new Point(submoonpos.X, maxtreepos.Y + itemsepar.Height);
                                }
                            }
                        }

                        moonpos = new Point(moonpos.X, maxtreepos.Y + itemsepar.Height);
                    }
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
                                    Size size, ref int offset, bool aligndown = false, int labelvoff = 0,
                                    bool toplevel = false)
        {
            string tip;
            Point endpoint = curpos;
            int quarterheight = size.Height / 4;
            int alignv = aligndown ? quarterheight : 0;

            JournalScan sc = sn.ScanData;

            //System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + curpos + " " + size + " hoff " + offset + " EDSM " + ((sc!= null) ? sc.IsEDSMBody.ToString() : ""));

            if (sc != null && (!sc.IsEDSMBody || checkBoxEDSM.Checked))     // if got one, and its our scan, or we are showing EDSM
            {
                tip = sc.DisplayString();

                if (sc.IsStar && toplevel)
                {
                    var starLabel = sn.customname ?? sn.ownname;

                    var habZone = sc.GetHabZoneStringLs();
                    if (!string.IsNullOrEmpty(habZone))
                    {
                        starLabel += $" ({habZone})";
                    }

                    endpoint = CreateImageLabel(pc, sc.GetStarTypeImage(),
                                                new Point(curpos.X + offset, curpos.Y + alignv),      // WE are basing it on a 1/4 + 1 + 1/4 grid, this is not being made bigger, move off
                                                size, starLabel, tip, alignv + labelvoff, sc.IsEDSMBody, false);          // and the label needs to be a quarter height below it..

                    offset += size.Width / 2;       // return the middle used was this..
                }
                else //else not a top-level star
                {
                    bool indicatematerials = sc.HasMaterials && !checkBoxMaterials.Checked;

                    Image nodeimage = sc.IsStar ? sc.GetStarTypeImage() : sc.GetPlanetClassImage();

                    if (ImageRequiresAnOverlay(sc, indicatematerials))
                    {
                        Bitmap bmp = new Bitmap(size.Width * 2, quarterheight * 6);

                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.DrawImage(nodeimage, size.Width / 2, quarterheight, size.Width, size.Height);

                            if (sc.IsLandable)
                                g.DrawImage(Icons.Controls.Scan_Bodies_Landable, new Rectangle(quarterheight, 0, quarterheight * 6, quarterheight * 6));

                            if (sc.HasRings)
                                g.DrawImage(sc.Rings.Count() > 1 ? Icons.Controls.Scan_Bodies_RingGap : Icons.Controls.Scan_Bodies_RingOnly,
                                                new Rectangle(-2, quarterheight, size.Width * 2, size.Height));

                            if (chkShowOverlays.Checked)
                            {
                                bool valuable = sc.EstimatedValue > 50000;
                                int overlaystotal = (sc.Terraformable ? 1 : 0) + (sc.HasMeaningfulVolcanism ? 1 : 0) + (valuable ? 1 : 0);
                                int ovsize = (overlaystotal>1) ? quarterheight : (quarterheight*3/2);
                                int pos = 0;

                                if (sc.Terraformable)
                                {
                                    g.DrawImage(Icons.Controls.Scan_Bodies_Terraformable, new Rectangle(0, pos, ovsize, ovsize));
                                    pos += ovsize + 1;
                                }

                                if (sc.HasMeaningfulVolcanism) //this renders below the terraformable icon if present
                                {
                                    g.DrawImage(Icons.Controls.Scan_Bodies_Volcanism, new Rectangle(0, pos, ovsize, ovsize));
                                    pos += ovsize + 1;
                                }

                                if (valuable)
                                    g.DrawImage(Icons.Controls.Scan_Bodies_HighValue, new Rectangle(0, pos, ovsize, ovsize)); 
                            }

                            if (indicatematerials)
                            {
                                Image mm = Icons.Controls.Scan_Bodies_MaterialMore;
                                g.DrawImage(mm, new Rectangle(bmp.Width - mm.Width, bmp.Height - mm.Height, mm.Width, mm.Height));
                            }
                        }

                        var nodeLabel = sn.customname ?? sn.ownname;
                        if (sn.ScanData.IsLandable && sn.ScanData.nSurfaceGravity != null)
                        {
                            nodeLabel += $" ({(sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2):N2}g)";
                        }

                        endpoint = CreateImageLabel(pc, bmp, curpos, new Size(bmp.Width, bmp.Height), nodeLabel, tip, labelvoff, sc.IsEDSMBody);
                        offset = size.Width;                                        // return that the middle is now this
                    }
                    else
                    {
                        endpoint = CreateImageLabel(pc, nodeimage, new Point(curpos.X + offset, curpos.Y + alignv), size,
                                                    sn.customname ?? sn.ownname, tip, alignv + labelvoff, sc.IsEDSMBody, false);
                        offset += size.Width / 2;
                    }

                    if (sc.HasMaterials && checkBoxMaterials.Checked)
                    {
                        Point matpos = new Point(endpoint.X + 4, curpos.Y);
                        Point endmat = CreateMaterialNodes(pc, sc, matpos, materialsize);
                        endpoint = new Point(Math.Max(endpoint.X, endmat.X), Math.Max(endpoint.Y, endmat.Y)); // record new right point..
                    }
                }
            }
            else if (sn.type == StarScan.ScanNodeType.belt)
            {
                if (sn.BeltData != null)
                    tip = sn.BeltData.RingInformationMoons(true);
                else
                    tip = sn.ownname + "\n\nNo scan data available";

                if (sn.children != null && sn.children.Count != 0)
                {
                    foreach (StarScan.ScanNode snc in sn.children.Values)
                    {
                        if (snc.ScanData != null)
                        {
                            tip += "\n\n" + snc.ScanData.DisplayString();
                        }
                    }
                }

                endpoint = CreateImageLabel(pc, Icons.Controls.Scan_Bodies_Belt,
                    new Point(curpos.X, curpos.Y + alignv), new Size(size.Width, size.Height), sn.ownname,
                                                    tip, alignv + labelvoff, false, false);
                offset += size.Width;
            }
            else
            {
                if (sn.type == StarScan.ScanNodeType.barycentre)
                    tip = "Barycentre of " + sn.ownname;
                else
                    tip = sn.ownname + "\n\nNo scan data available";

                endpoint = CreateImageLabel(pc, notscanned, new Point(curpos.X + offset, curpos.Y + alignv), size, sn.customname ?? sn.ownname, tip, alignv + labelvoff, false, false);
                offset += size.Width / 2;       // return the middle used was this..
            }

            return endpoint;
        }

        private static bool ImageRequiresAnOverlay(JournalScan sc, bool indicatematerials)
        {
            return sc.IsLandable || 
                sc.HasRings || 
                indicatematerials || 
                sc.Terraformable ||
                sc.HasMeaningfulVolcanism ||
                sc.EstimatedValue > 50000;
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

                MaterialCommodityDB mc = MaterialCommodityDB.GetCachedMaterial(sd.Key);
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

        void CreateMaterialImage(List<PictureBoxHotspot.ImageElement> pc, Point matpos, Size matsize, string text, string mattag, string mattip, Color matcolour, Color textcolour)
        {
            System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
            colormap.OldColor = Color.White;    // this is the marker colour to replace
            colormap.NewColor = matcolour;

            Bitmap mat = BaseUtils.BitMapHelpers.ReplaceColourInBitmap((Bitmap)Icons.Controls.Scan_Bodies_Material, new System.Drawing.Imaging.ColorMap[] { colormap });

            BaseUtils.BitMapHelpers.DrawTextCentreIntoBitmap(ref mat, text, stdfont, textcolour);

            PictureBoxHotspot.ImageElement ie = new PictureBoxHotspot.ImageElement(
                            new Rectangle(matpos.X, matpos.Y, matsize.Width, matsize.Height), mat, mattag, mattip);

            pc.Add(ie);
        }

        Point CreateImageLabel(List<PictureBoxHotspot.ImageElement> c, Image i, Point postopright, Size size, string label,
                                    string ttext, int labelhoff, bool fromEDSM, bool imgowned = true)
        {
            //System.Diagnostics.Debug.WriteLine("    " + label + " " + postopright + " size " + size + " hoff " + labelhoff + " laby " + (postopright.Y + size.Height + labelhoff));
            if (fromEDSM)
                ttext = "From EDSM" + Environment.NewLine + ttext;

            PictureBoxHotspot.ImageElement ie = new PictureBoxHotspot.ImageElement(new Rectangle(postopright.X, postopright.Y, size.Width, size.Height), i, ttext, ttext, imgowned);

            Point max = new Point(postopright.X + size.Width, postopright.Y + size.Height);

            if (label != null)
            {
                Font font = stdfont;
                if (fromEDSM)
                    font = stdfontUnderline;

                Point labposcenthorz = new Point(postopright.X + size.Width / 2, postopright.Y + size.Height + labelhoff);

                PictureBoxHotspot.ImageElement lab = new PictureBoxHotspot.ImageElement();
                Size maxsize = new Size(300, 30);

                lab.TextCentreAutosize(labposcenthorz, maxsize, label, font, discoveryform.theme.LabelColor, this.BackColor);

                if (lab.pos.X < postopright.X)
                {
                    int offset = postopright.X - lab.pos.X;
                    ie.Translate(offset, 0);
                    lab.Translate(offset, 0);
                }

                c.Add(lab);

                max = new Point(Math.Max(lab.pos.X + lab.pos.Width, max.X), lab.pos.Y + lab.pos.Height);
            }

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
            beltsize = new Size(starsize.Width * 1 / 2, starsize.Height);
            planetsize = new Size(starsize.Width * 3 / 4, starsize.Height * 3 / 4);
            moonsize = new Size(starsize.Width * 2 / 4, starsize.Height * 2 / 4);
            materialsize = new Size(24, 24);
            itemsepar = new Size(stars / 16, stars / 16);
            topmargin = 10;
            leftmargin = 0;
        }

        #endregion

        #region User interaction

        private void ClickElement(object sender, MouseEventArgs e, PictureBoxHotspot.ImageElement i, object tag)
        {
            if (i != null)
                ShowInfo((string)tag, i.pos.Location.X < panelStars.Width / 2);
            else
                HideInfo();
        }

        bool progchange = false;

        private void checkBoxMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "Materials", checkBoxMaterials.Checked);
                DrawSystem();
            }
        }

        private void checkBoxMaterialsRare_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "MaterialsRare", checkBoxMaterialsRare.Checked);

                progchange = true;
                checkBoxMaterials.Checked = true;
                progchange = false;

                DrawSystem();
            }
        }

        private void checkBoxMoons_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "Moons", checkBoxMoons.Checked);
                DrawSystem();
            }
        }

        private void SetSizeCheckBoxes(int size)
        {
            progchange = true;
            checkBoxLarge.Checked = (size == 128);
            checkBoxMedium.Checked = (size == 96);
            checkBoxSmall.Checked = (size == 64);
            checkBoxTiny.Checked = (size == 48);

            if (!checkBoxLarge.Checked && !checkBoxMedium.Checked && !checkBoxSmall.Checked && !checkBoxTiny.Checked)
            {
                checkBoxSmall.Checked = true;
                size = 64;
            }
            SetSize(size);
            SQLiteDBClass.PutSettingInt(DbSave + "Size", size);
            progchange = false;

            DrawSystem();
        }

        private void checkBoxLarge_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
                SetSizeCheckBoxes(128);
        }

        private void checkBoxMedium_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
                SetSizeCheckBoxes(96);
        }

        private void checkBoxSmall_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
                SetSizeCheckBoxes(64);
        }

        private void checkBoxTiny_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
                SetSizeCheckBoxes(48);
        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "EDSM", checkBoxEDSM.Checked);
                DrawSystem();
            }
        }

        private void chkShowOverlays_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "BodyOverlays", chkShowOverlays.Checked);
                DrawSystem();
            }
        }

        private void toolStripMenuItemToolbar_Click(object sender, EventArgs e)
        {
            panelControls.Visible = !panelControls.Visible;
            lblSystemInfo.Left = panelControls.Visible ? panelControls.Width : 0; // move approx value to left if controls hidden
        }

        void ShowInfo(string text, bool onright)
        {
            rtbNodeInfo.Text = text;
            rtbNodeInfo.Tag = onright;
            rtbNodeInfo.Visible = true;
            rtbNodeInfo.Show();
            PositionInfo();
        }



        void HideInfo()
        {
            rtbNodeInfo.Visible = false;
        }

        void PositionInfo()
        {
            if (rtbNodeInfo.Visible)
            {
                if (rtbNodeInfo.Tag != null && ((bool)rtbNodeInfo.Tag) == true)
                    rtbNodeInfo.Location = new Point(panelStars.Width / 2 + panelStars.Width / 16, 10);
                else
                    rtbNodeInfo.Location = new Point(panelStars.Width / 16, 10);

                int h = Math.Min(rtbNodeInfo.EstimateVerticalSizeFromText(), panelStars.Height - 20);

                rtbNodeInfo.Size = new Size(panelStars.Width * 6 / 16, h);
                rtbNodeInfo.PerformLayout();    // not sure why i need this..
            }
        }

        private void panelStars_Click(object sender, EventArgs e)
        {
            HideInfo();
        }

        #endregion

        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "All", "Stars only",
                                    "Planets only", //2
                                    "Exploration List stars", //3
                                    "Exploration List Planets", //4
                                    "Sold Exploration Data", // 5
                                        });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWrite csv = new BaseUtils.CSVWrite();
                csv.SetCSVDelimiter(frm.Comma);

                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(frm.Path))
                    {
                        if (frm.SelectedIndex == 5)
                        {
                            int count;
                            List<HistoryEntry> data = HistoryList.FilterByJournalEvent(discoveryform.history.ToList(), "Sell Exploration Data", out count);
                            data = (from he in data where he.EventTimeLocal >= frm.StartTime && he.EventTimeLocal <= frm.EndTime orderby he.EventTimeUTC descending select he).ToList();

                            List<HistoryEntry> scans = HistoryList.FilterByJournalEvent(discoveryform.history.ToList(), "Scan", out count);

                            if (frm.IncludeHeader)
                            {
                                writer.Write(csv.Format("Time"));
                                writer.Write(csv.Format("System"));
                                writer.Write(csv.Format("Star type"));
                                writer.Write(csv.Format("Planet type", false));
                                writer.WriteLine();
                            }

                            foreach (HistoryEntry he in data)
                            {
                                JournalSellExplorationData jsed = he.journalEntry as JournalSellExplorationData;
                                if (jsed == null || jsed.Discovered == null)
                                    continue;
                                foreach (String system in jsed.Discovered)
                                {
                                    writer.Write(csv.Format(jsed.EventTimeLocal));
                                    writer.Write(csv.Format(system));

                                    EDStar star = EDStar.Unknown;
                                    EDPlanet planet = EDPlanet.Unknown;

                                    foreach (HistoryEntry scanhe in scans)
                                    {
                                        JournalScan scan = scanhe.journalEntry as JournalScan;
                                        if (scan.BodyName.Equals(system, StringComparison.OrdinalIgnoreCase))
                                        {
                                            star = scan.StarTypeID;
                                            planet = scan.PlanetTypeID;
                                            break;
                                        }
                                    }
                                    writer.Write(csv.Format((star != EDStar.Unknown) ? Enum.GetName(typeof(EDStar), star) : ""));
                                    writer.Write(csv.Format((planet != EDPlanet.Unknown) ? Enum.GetName(typeof(EDPlanet), planet) : "", false));
                                    writer.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            List<JournalScan> scans = null;

                            if (frm.SelectedIndex < 3)
                            {
                                var entries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTime, frm.EndTime);
                                scans = entries.ConvertAll<JournalScan>(x => (JournalScan)x);
                            }
                            else
                            {
                                string explorepath = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "Exploration");
                                if (!System.IO.Directory.Exists(explorepath))
                                    System.IO.Directory.CreateDirectory(explorepath);

                                OpenFileDialog dlg = new OpenFileDialog();
                                dlg.InitialDirectory = explorepath;
                                dlg.DefaultExt = "json";
                                dlg.AddExtension = true;
                                dlg.Filter = "Explore file| *.json";

                                scans = new List<JournalScan>();

                                if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
                                {

                                    ExplorationSetClass _currentExplorationSet = new ExplorationSetClass();
                                    _currentExplorationSet.Clear();
                                    _currentExplorationSet.Load(dlg.FileName);

                                    foreach (string system in _currentExplorationSet.Systems)
                                    {
                                        List<long> edsmidlist = SystemClassDB.GetEdsmIdsFromName(system);

                                        if (edsmidlist.Count > 0)
                                        {
                                            for (int ii = 0; ii < edsmidlist.Count; ii++)
                                            {
                                                List<JournalScan> sysscans = EDSMClass.GetBodiesList((int)edsmidlist[ii]);
                                                if (sysscans != null)
                                                    scans.AddRange(sysscans);
                                            }
                                        }
                                    }
                                }
                                else
                                    return;
                            }

                            bool ShowStars = frm.SelectedIndex < 2 || frm.SelectedIndex == 3;
                            bool ShowPlanets = frm.SelectedIndex == 0 || frm.SelectedIndex == 2 || frm.SelectedIndex == 4;

                            if (frm.IncludeHeader)
                            {
                                // Write header

                                writer.Write(csv.Format("Time"));
                                writer.Write(csv.Format("BodyName"));
                                writer.Write(csv.Format("Estimated Value"));
                                writer.Write(csv.Format("DistanceFromArrivalLS"));
                                if (ShowStars)
                                {
                                    writer.Write(csv.Format("StarType"));
                                    writer.Write(csv.Format("StellarMass"));
                                    writer.Write(csv.Format("AbsoluteMagnitude"));
                                    writer.Write(csv.Format("Age MY"));
                                    writer.Write(csv.Format("Luminosity"));
                                }
                                writer.Write(csv.Format("Radius"));
                                writer.Write(csv.Format("RotationPeriod"));
                                writer.Write(csv.Format("SurfaceTemperature"));

                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format("TidalLock"));
                                    writer.Write(csv.Format("TerraformState"));
                                    writer.Write(csv.Format("PlanetClass"));
                                    writer.Write(csv.Format("Atmosphere"));
                                    writer.Write(csv.Format("Iron"));
                                    writer.Write(csv.Format("Silicates"));
                                    writer.Write(csv.Format("SulphurDioxide"));
                                    writer.Write(csv.Format("CarbonDioxide"));
                                    writer.Write(csv.Format("Nitrogen"));
                                    writer.Write(csv.Format("Oxygen"));
                                    writer.Write(csv.Format("Water"));
                                    writer.Write(csv.Format("Argon"));
                                    writer.Write(csv.Format("Ammonia"));
                                    writer.Write(csv.Format("Methane"));
                                    writer.Write(csv.Format("Hydrogen"));
                                    writer.Write(csv.Format("Helium"));
                                    writer.Write(csv.Format("Volcanism"));
                                    writer.Write(csv.Format("SurfaceGravity"));
                                    writer.Write(csv.Format("SurfacePressure"));
                                    writer.Write(csv.Format("Landable"));
                                    writer.Write(csv.Format("EarthMasses"));
                                }
                                // Common orbital param
                                writer.Write(csv.Format("SemiMajorAxis"));
                                writer.Write(csv.Format("Eccentricity"));
                                writer.Write(csv.Format("OrbitalInclination"));
                                writer.Write(csv.Format("Periapsis"));
                                writer.Write(csv.Format("OrbitalPeriod"));
                                writer.Write(csv.Format("AxialTilt"));


                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format("Carbon"));
                                    writer.Write(csv.Format("Iron"));
                                    writer.Write(csv.Format("Nickel"));
                                    writer.Write(csv.Format("Phosphorus"));
                                    writer.Write(csv.Format("Sulphur"));
                                    writer.Write(csv.Format("Arsenic"));
                                    writer.Write(csv.Format("Chromium"));
                                    writer.Write(csv.Format("Germanium"));
                                    writer.Write(csv.Format("Manganese"));
                                    writer.Write(csv.Format("Selenium"));
                                    writer.Write(csv.Format("Vanadium"));
                                    writer.Write(csv.Format("Zinc"));
                                    writer.Write(csv.Format("Zirconium"));
                                    writer.Write(csv.Format("Cadmium"));
                                    writer.Write(csv.Format("Mercury"));
                                    writer.Write(csv.Format("Molybdenum"));
                                    writer.Write(csv.Format("Niobium"));
                                    writer.Write(csv.Format("Tin"));
                                    writer.Write(csv.Format("Tungsten"));
                                    writer.Write(csv.Format("Antimony"));
                                    writer.Write(csv.Format("Polonium"));
                                    writer.Write(csv.Format("Ruthenium"));
                                    writer.Write(csv.Format("Technetium"));
                                    writer.Write(csv.Format("Tellurium"));
                                    writer.Write(csv.Format("Yttrium"));
                                }

                                writer.WriteLine();
                            }

                            foreach (JournalScan je in scans)
                            {
                                JournalScan scan = je as JournalScan;

                                if (ShowPlanets == false)  // Then only show stars.
                                    if (String.IsNullOrEmpty(scan.StarType))
                                        continue;

                                if (ShowStars == false)   // Then only show planets
                                    if (String.IsNullOrEmpty(scan.PlanetClass))
                                        continue;

                                writer.Write(csv.Format(scan.EventTimeUTC));
                                writer.Write(csv.Format(scan.BodyName));
                                writer.Write(csv.Format(scan.EstimatedValue));
                                writer.Write(csv.Format(scan.DistanceFromArrivalLS));

                                if (ShowStars)
                                {
                                    writer.Write(csv.Format(scan.StarType));
                                    writer.Write(csv.Format((scan.nStellarMass.HasValue) ? scan.nStellarMass.Value : 0));
                                    writer.Write(csv.Format((scan.nAbsoluteMagnitude.HasValue) ? scan.nAbsoluteMagnitude.Value : 0));
                                    writer.Write(csv.Format((scan.nAge.HasValue) ? scan.nAge.Value : 0));
                                    writer.Write(csv.Format(scan.Luminosity));
                                }


                                writer.Write(csv.Format(scan.nRadius.HasValue ? scan.nRadius.Value : 0));
                                writer.Write(csv.Format(scan.nRotationPeriod.HasValue ? scan.nRotationPeriod.Value : 0));
                                writer.Write(csv.Format(scan.nSurfaceTemperature.HasValue ? scan.nSurfaceTemperature.Value : 0));

                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format(scan.nTidalLock.HasValue ? scan.nTidalLock.Value : false));
                                    writer.Write(csv.Format((scan.TerraformState != null) ? scan.TerraformState : ""));
                                    writer.Write(csv.Format((scan.PlanetClass != null) ? scan.PlanetClass : ""));
                                    writer.Write(csv.Format((scan.Atmosphere != null) ? scan.Atmosphere : ""));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Iron")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Silicates")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("SulphurDioxide")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("CarbonDioxide")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Nitrogen")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Oxygen")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Water")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Argon")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Ammonia")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Methane")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Hydrogen")));
                                    writer.Write(csv.Format(scan.GetAtmosphereComponent("Helium")));
                                    writer.Write(csv.Format((scan.Volcanism != null) ? scan.Volcanism : ""));
                                    writer.Write(csv.Format(scan.nSurfaceGravity.HasValue ? scan.nSurfaceGravity.Value : 0));
                                    writer.Write(csv.Format(scan.nSurfacePressure.HasValue ? scan.nSurfacePressure.Value : 0));
                                    writer.Write(csv.Format(scan.nLandable.HasValue ? scan.nLandable.Value : false));
                                    writer.Write(csv.Format((scan.nMassEM.HasValue) ? scan.nMassEM.Value : 0));
                                }
                                // Common orbital param
                                writer.Write(csv.Format(scan.nSemiMajorAxis.HasValue ? scan.nSemiMajorAxis.Value : 0));
                                writer.Write(csv.Format(scan.nEccentricity.HasValue ? scan.nEccentricity.Value : 0));
                                writer.Write(csv.Format(scan.nOrbitalInclination.HasValue ? scan.nOrbitalInclination.Value : 0));
                                writer.Write(csv.Format(scan.nPeriapsis.HasValue ? scan.nPeriapsis.Value : 0));
                                writer.Write(csv.Format(scan.nOrbitalPeriod.HasValue ? scan.nOrbitalPeriod.Value : 0));
                                writer.Write(csv.Format(scan.nAxialTilt.HasValue ? scan.nAxialTilt : null));

                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format(scan.GetMaterial("Carbon")));
                                    writer.Write(csv.Format(scan.GetMaterial("Iron")));
                                    writer.Write(csv.Format(scan.GetMaterial("Nickel")));
                                    writer.Write(csv.Format(scan.GetMaterial("Phosphorus")));
                                    writer.Write(csv.Format(scan.GetMaterial("Sulphur")));
                                    writer.Write(csv.Format(scan.GetMaterial("Arsenic")));
                                    writer.Write(csv.Format(scan.GetMaterial("Chromium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Germanium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Manganese")));
                                    writer.Write(csv.Format(scan.GetMaterial("Selenium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Vanadium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Zinc")));
                                    writer.Write(csv.Format(scan.GetMaterial("Zirconium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Cadmium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Mercury")));
                                    writer.Write(csv.Format(scan.GetMaterial("Molybdenum")));
                                    writer.Write(csv.Format(scan.GetMaterial("Niobium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Tin")));
                                    writer.Write(csv.Format(scan.GetMaterial("Tungsten")));
                                    writer.Write(csv.Format(scan.GetMaterial("Antimony")));
                                    writer.Write(csv.Format(scan.GetMaterial("Polonium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Ruthenium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Technetium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Tellurium")));
                                    writer.Write(csv.Format(scan.GetMaterial("Yttrium")));
                                }
                                writer.WriteLine();
                            }
                        }

                        writer.Close();

                        if (frm.AutoOpen)
                            System.Diagnostics.Process.Start(frm.Path);
                    }
                }
                catch
                {
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        #endregion
    }
}

