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
    public partial class ScanDisplay : UserControl
    {
        public bool CheckEDSM { get; set; }
        public bool ShowMoons { get; set; }
        public bool ShowMaterials { get; set; }
        public bool ShowMaterialsRare { get; set; }
        public bool HideFullMaterials { get; set; }
        public bool ShowOverlays { get; set; }

        public int WidthAvailable { get { return this.Width - vScrollBarCustom.Width; } }   // available display width
        public Point DisplayAreaUsed { get; private set; }  // used area to display in
        public Size StarSize { get; private set; }  // size of stars

        private Size beltsize, planetsize, moonsize, materialsize;
        private Size itemsepar;
        private int leftmargin;
        private int topmargin;
        private const int materialspacer = 4;

        private Font stdfont = new Font("Microsoft Sans Serif", 8.25F);
        private Font stdfontUnderline = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Underline);

        #region Init
        public ScanDisplay()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            rtbNodeInfo.Visible = false;
            toolTip.ShowAlways = true;
            imagebox.ClickElement += ClickElement;
        }
	
        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            PositionInfo();
        }

        #endregion

        #region Display

        public StarScan.SystemNode DrawSystem(ISystem showing_system, MaterialCommoditiesList curmats, HistoryList hl )   // draw showing_system (may be null), showing_matcomds (may be null)
        {
            HideInfo();

            imagebox.ClearImageList();  // does not clear the image, render will do that
            
            if (showing_system == null)
            {
                imagebox.Render();
                return null;
            }

            StarScan.SystemNode last_sn = hl.starscan.FindSystem(showing_system, CheckEDSM, byname: true);

            if (last_sn != null)     // 
            {
                Point curpos = new Point(leftmargin, topmargin);
                DisplayAreaUsed = curpos;
                List<PictureBoxHotspot.ImageElement> starcontrols = new List<PictureBoxHotspot.ImageElement>();

                //for( int i = 0; i < 1000; i +=100)  CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, new Point(i, 0), new Size(24, 24), i.ToString(), "");

                foreach (StarScan.ScanNode starnode in last_sn.starnodes.Values)        // always has scan nodes
                {
                    //System.Diagnostics.Debug.WriteLine("Draw " + starnode.type);
                    int offset = 0;
                    Point maxstarpos = DrawNode(starcontrols, starnode,curmats,hl,
                                (starnode.type == StarScan.ScanNodeType.barycentre) ? Icons.Controls.Scan_Bodies_Barycentre : JournalScan.GetStarImageNotScanned(),
                                curpos, StarSize, ref offset, false, (planetsize.Height * 6 / 4 - StarSize.Height) / 2, true);       // the last part nerfs the label down to the right position

                    Point maxitemspos = maxstarpos;

                    curpos = new Point(maxitemspos.X + itemsepar.Width, curpos.Y);   // move to the right
                    curpos.Y += StarSize.Height / 2 - planetsize.Height * 3 / 4;     // slide down for planet vs star difference in size

                    Point firstcolumn = curpos;

                    if (starnode.children != null)
                    {
                        Queue<StarScan.ScanNode> belts = null;
                        if (starnode.ScanData != null && (!starnode.ScanData.IsEDSMBody || CheckEDSM))
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

                                DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt,
                                         new Point(curpos.X + (planetsize.Width - beltsize.Width) / 2, curpos.Y), beltsize, ref offset, false);

                                curpos = new Point(curpos.X + planetsize.Width, curpos.Y);
                                lastbelt = belts.Count != 0 ? belts.Dequeue() : null;
                            }

                            bool nonedsmscans = planetnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                            //System.Diagnostics.Debug.WriteLine("Planet Node " + planetnode.ownname + " has scans " + nonedsmscans);

                            if (nonedsmscans || CheckEDSM)
                            {
                                List<PictureBoxHotspot.ImageElement> pc = new List<PictureBoxHotspot.ImageElement>();

                                Point maxpos = CreatePlanetTree(pc, planetnode, curmats, hl, curpos);

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

                            DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt,
                                     new Point(curpos.X + (planetsize.Width - beltsize.Width) / 2, curpos.Y), beltsize, ref offset, false);

                            curpos = new Point(curpos.X + planetsize.Width, curpos.Y);
                            lastbelt = belts.Count != 0 ? belts.Dequeue() : null;
                        }
                    }

                    DisplayAreaUsed = new Point(Math.Max(DisplayAreaUsed.X, maxitemspos.X), Math.Max(DisplayAreaUsed.Y, maxitemspos.Y));
                    curpos = new Point(leftmargin, maxitemspos.Y + itemsepar.Height);
                }

                imagebox.AddRange(starcontrols);
            }

            imagebox.Render();      // replaces image..

            return last_sn;
        }

        private string BuildScanValue(StarScan.SystemNode system)
        {
            var value = 0;

            foreach (var body in system.Bodies)
            {
                if (body?.ScanData != null)
                {
                    if (CheckEDSM || !body.ScanData.IsEDSMBody)
                    {
                        value += body.ScanData.EstimatedValue;
                    }
                }
            }

            return string.Format("Approx value: {0:N0}".Tx(this,"AV"), value);
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<PictureBoxHotspot.ImageElement> pc, StarScan.ScanNode planetnode, MaterialCommoditiesList curmats, HistoryList hl, Point curpos)
        {
            // PLANETWIDTH|PLANETWIDTH  (if drawing a full planet with rings/landing)
            // or
            // MOONWIDTH|MOONWIDTH      (if drawing a single width planet)
            // this offset, ONLY used if a single width planet, allows for two moons
            int offset = moonsize.Width - planetsize.Width / 2;           // centre is moon width, back off by planetwidth/2 to place the left edge of the planet

            Point maxtreepos = DrawNode(pc, planetnode, curmats, hl, JournalScan.GetPlanetImageNotScanned(),
                                curpos, planetsize, ref offset, true);        // offset passes in the suggested offset, returns the centre offset

            if (planetnode.children != null && ShowMoons)
            {
                offset -= moonsize.Width;               // offset is centre of planet image, back off by a moon width to allow for 2 moon widths centred

                Point moonpos = new Point(curpos.X + offset, maxtreepos.Y + itemsepar.Height);    // moon pos

                foreach (StarScan.ScanNode moonnode in planetnode.children.Values.Where(n => n.type != StarScan.ScanNodeType.barycentre))
                {
                    bool nonedsmscans = moonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                    if (nonedsmscans || CheckEDSM)
                    {
                        int offsetm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                        Point mmax = DrawNode(pc, moonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), moonpos, moonsize, ref offsetm);

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

                                if (nonedsmsubmoonscans || CheckEDSM)
                                {
                                    int offsetsm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                                    Point sbmax = DrawNode(pc, submoonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), submoonpos, moonsize, ref offsetsm);

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
        Point DrawNode(List<PictureBoxHotspot.ImageElement> pc, StarScan.ScanNode sn, MaterialCommoditiesList curmats, HistoryList hl, 
                                    Image notscanned, Point curpos,
                                    Size size, ref int offset, bool aligndown = false, int labelvoff = 0,
                                    bool toplevel = false)
        {
            string tip;
            Point endpoint = curpos;
            int quarterheight = size.Height / 4;
            int alignv = aligndown ? quarterheight : 0;

            JournalScan sc = sn.ScanData;

            //System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + curpos + " " + size + " hoff " + offset + " EDSM " + ((sc!= null) ? sc.IsEDSMBody.ToString() : ""));

            if (sc != null && (!sc.IsEDSMBody || CheckEDSM))     // if got one, and its our scan, or we are showing EDSM
            {
                tip = sc.DisplayString(historicmatlist:curmats, currentmatlist:hl.GetLast?.MaterialCommodity);

                if ( sn.type == StarScan.ScanNodeType.ring)
                {

                }
                else  if (sc.IsStar && toplevel)
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
                    bool indicatematerials = sc.HasMaterials && !ShowMaterials;

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

                            if (ShowOverlays)
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

                    if (sc.HasMaterials && ShowMaterials)
                    {
                        Point matpos = new Point(endpoint.X + 4, curpos.Y);
                        Point endmat = CreateMaterialNodes(pc, sc, curmats, hl, matpos, materialsize);
                        endpoint = new Point(Math.Max(endpoint.X, endmat.X), Math.Max(endpoint.Y, endmat.Y)); // record new right point..
                    }
                }
            }
            else if (sn.type == StarScan.ScanNodeType.belt)
            {
                if (sn.BeltData != null)
                    tip = sn.BeltData.RingInformationMoons(true);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".Tx(this, "NSD");

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
                    tip = string.Format("Barycentre of {0}".Tx(this,"BC") , sn.ownname);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".Tx(this, "NSD");

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


        Point CreateMaterialNodes(List<PictureBoxHotspot.ImageElement> pc, JournalScan sn, MaterialCommoditiesList curmats, HistoryList hl, Point matpos, Size matsize)
        {
            Point startpos = matpos;
            Point maximum = matpos;
            int noperline = 0;

            bool noncommon = ShowMaterialsRare;

            string matclicktext = sn.DisplayMaterials(2, curmats, hl.GetLast?.MaterialCommodity);

            foreach (KeyValuePair<string, double> sd in sn.Materials)
            {
                string tooltip = sn.DisplayMaterial(sd.Key, sd.Value, curmats,hl.GetLast?.MaterialCommodity);

                Color fillc = Color.Yellow;
                string abv = sd.Key.Substring(0, 1);

                MaterialCommodityData mc = MaterialCommodityData.GetByFDName(sd.Key);

                if (mc != null)
                {
                    abv = mc.Shortname;
                    fillc = mc.Colour;

                    if (HideFullMaterials)                 // check full
                    {
                        int? limit = mc.MaterialLimit();
                        MaterialCommodities matnow = curmats.Find(mc);

                        // debug if (matnow != null && mc.shortname == "Fe")  matnow.count = 10000;
                            
                        if (matnow != null && matnow.Count >= limit)        // and limit
                            continue;
                    }

                    if (noncommon && mc.IsCommonMaterial )
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

                lab.TextCentreAutosize(labposcenthorz, maxsize, label, font, EDDTheme.Instance.LabelColor, this.BackColor);

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

        public void SetSize(int stars)
        {
            StarSize = new Size(stars, stars);
            beltsize = new Size(StarSize.Width * 1 / 2, StarSize.Height);
            planetsize = new Size(StarSize.Width * 3 / 4, StarSize.Height * 3 / 4);
            moonsize = new Size(StarSize.Width * 2 / 4, StarSize.Height * 2 / 4);
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

        void ShowInfo(string text, bool onright)
        {
            rtbNodeInfo.Text = text;
            rtbNodeInfo.Tag = onright;
            rtbNodeInfo.Visible = true;
            rtbNodeInfo.Show();
            PositionInfo();
        }

        public void HideInfo()
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

                rtbNodeInfo.Size = new Size(panelStars.Width * 7 / 16, h);
                rtbNodeInfo.PerformLayout();    // not sure why i need this..
            }
        }

        private void panelStars_Click(object sender, EventArgs e)
        {
            HideInfo();
        }


        #endregion
    }
}

