/*
 * Copyright © 2019 EDDiscovery development team
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ExtendedControls;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class ScanDisplayUserControl : UserControl
    {
        public bool CheckEDSM { get; set; }
        public bool ShowMoons { get; set; } = true;
        public bool ShowOverlays { get; set; } = true;
        public bool ShowMaterials { get; set; } = true;
        public bool ShowOnlyMaterialsRare { get; set; } = false;
        public bool HideFullMaterials { get; set; } = false;
        public bool ShowAllG { get; set; } = true;
        public bool ShowHabZone { get; set; } = true;
        public bool ShowPlanetClasses { get; set; } = true;
        public bool ShowStarClasses { get; set; } = true;
        public bool ShowDist { get; set; } = true;

        public int ValueLimit { get; set; } = 50000;

        public int WidthAvailable { get { return this.Width - vScrollBarCustom.Width; } }   // available display width
        public Point DisplayAreaUsed { get; private set; }  // used area to display in
        public Size StarSize { get; private set; }  // size of stars

        private Size beltsize, planetsize, moonsize, materialsize;
        private int starfirstplanetspacerx;        // distance between star and first planet
        private int starplanetgroupspacery;        // distance between each star/planet grouping 
        private int planetspacerx;       // distance between each planet in a row
        private int planetspacery;       // distance between each planet row
        private int moonspacerx;        // distance to move moon across
        private int moonspacery;        // distance to slide down moon vs planet
        private int materiallinespacerxy;   // extra distance to add around material output
        private int leftmargin;
        private int topmargin;

        private Font stdfont = EDDTheme.Instance.GetDialogFont;
        private Font largerfont = EDDTheme.Instance.GetFont;
        private Font stdfontUnderline = EDDTheme.Instance.GetDialogScaledFont(1f,FontStyle.Underline);

        #region Init
        public ScanDisplayUserControl()
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

        // draw scannode (may be null), 
        // curmats may be null
        public void DrawSystem(StarScan.SystemNode scannode, MaterialCommoditiesList curmats, HistoryList hl, string opttext = null, string[] filter=  null ) 
        {
            HideInfo();

            imagebox.ClearImageList();  // does not clear the image, render will do that
            
            if (scannode != null)
            {
                Point leftmiddle = new Point(leftmargin, topmargin + StarSize.Height/2);

                if ( opttext != null )
                {
                    ExtPictureBox.ImageElement lab = new ExtPictureBox.ImageElement();
                    lab.TextAutosize(new Point(leftmargin,0), new Size(500, 30), opttext, largerfont, EDDTheme.Instance.LabelColor, this.BackColor);
                    imagebox.Add(lab);
                    leftmiddle.Y += lab.Image.Height + 8;
                }

                DisplayAreaUsed = leftmiddle;
                List<ExtPictureBox.ImageElement> starcontrols = new List<ExtPictureBox.ImageElement>();

                bool displaybelts = filter == null || (filter.Contains("belt") || filter.Contains("All"));

                Point maxitemspos = new Point(0, 0);

                foreach (StarScan.ScanNode starnode in scannode.starnodes.Values)        // always has scan nodes
                {
                    if (filter != null && starnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                    {
                        System.Diagnostics.Debug.WriteLine("SDUC Rejected " + starnode.fullname);
                        continue;
                    }

                    // Draw star

                    Point maxstarpos = DrawNode(starcontrols, starnode, curmats, hl,
                                (starnode.type == StarScan.ScanNodeType.barycentre) ? Icons.Controls.Scan_Bodies_Barycentre : JournalScan.GetStarImageNotScanned(),
                                leftmiddle, false, out int unusedstarcentre, StarSize, true);       // the last part nerfs the label down to the right position

                    maxitemspos = new Point(Math.Max(maxitemspos.X, maxstarpos.X), Math.Max(maxitemspos.Y, maxstarpos.Y));

                    if (starnode.children != null)
                    {
                        leftmiddle = new Point(maxitemspos.X + starfirstplanetspacerx, leftmiddle.Y + StarSize.Height / 2 - planetsize.Height * 3 / 4);
                        Point firstcolumn = leftmiddle;

                        Queue<StarScan.ScanNode> belts;
                        if (starnode.ScanData != null && (!starnode.ScanData.IsEDSMBody || CheckEDSM))  // have scandata on star, and its not edsm or allowed edsm
                        {
                            belts = new Queue<StarScan.ScanNode>(starnode.children.Values.Where(s => s.type == StarScan.ScanNodeType.belt));    // find belts in children of star
                        }
                        else
                        {
                            belts = new Queue<StarScan.ScanNode>(); // empty array
                        }

                        StarScan.ScanNode lastbelt = belts.Count != 0 ? belts.Dequeue() : null;

                        double habzonestartls = starnode.ScanData?.HabitableZoneInner ?? 0;
                        double habzoneendls = starnode.ScanData?.HabitableZoneOuter ?? 0;

                        // process body and stars only

                        foreach (StarScan.ScanNode planetnode in starnode.children.Values.Where(s => s.type == StarScan.ScanNodeType.body || s.type == StarScan.ScanNodeType.star))
                        {
                            if (filter != null && planetnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                            {
                                System.Diagnostics.Debug.WriteLine("SDUC Rejected " + planetnode.fullname);
                                continue;
                            }

                            //System.Diagnostics.Debug.WriteLine("Draw " + planetnode.ownname + ":" + planetnode.type);

                            // if belt is before this, display belts here

                            while (displaybelts && lastbelt != null && planetnode.ScanData != null && (lastbelt.BeltData == null || lastbelt.BeltData.OuterRad < planetnode.ScanData.nSemiMajorAxis))
                            {
                                // if too far across, go back to star
                                if (leftmiddle.X + planetsize.Width > panelStars.Width - panelStars.ScrollBarWidth) // if too far across..
                                {
                                    leftmiddle = new Point(firstcolumn.X, maxitemspos.Y + planetspacery + planetsize.Height / 2); // move to left at maxy+space+h/2
                                }

                                string appendlabel = Environment.NewLine + $"{lastbelt.BeltData.OuterRad / JournalScan.oneLS_m:N1}ls";

                                Point maxbeltpos = DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt, leftmiddle,false,out int unusedbeltcentre, beltsize, appendlabeltext:appendlabel);

                                leftmiddle = new Point(maxbeltpos.X + planetspacerx, leftmiddle.Y);
                                lastbelt = belts.Count != 0 ? belts.Dequeue() : null;

                                maxitemspos = new Point(Math.Max(maxitemspos.X, maxbeltpos.X), Math.Max(maxitemspos.Y, maxbeltpos.Y));
                            }

                            bool nonedsmscans = planetnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                           //System.Diagnostics.Debug.WriteLine("Planet Node " + planetnode.ownname + " has scans " + nonedsmscans);

                            if (nonedsmscans || CheckEDSM)
                            {
                                List<ExtPictureBox.ImageElement> pc = new List<ExtPictureBox.ImageElement>();

                                bool habzone = ShowHabZone && planetnode.ScanData != null && planetnode.ScanData.nSemiMajorAxisAU.HasValue && 
                                                planetnode.ScanData.nSemiMajorAxisAU.Value * JournalScan.oneAU_LS >= habzonestartls && planetnode.ScanData.nSemiMajorAxisAU.Value * JournalScan.oneAU_LS <= habzoneendls;

                                Point maxplanetpos = CreatePlanetTree(pc, planetnode, curmats, hl, leftmiddle, filter, habzone );

                                //System.Diagnostics.Debug.WriteLine("Planet " + planetnode.ownname + " " + curpos + " " + maxpos + " max " + (panelStars.Width - panelStars.ScrollBarWidth));

                                if (maxplanetpos.X > panelStars.Width - panelStars.ScrollBarWidth)          // uh oh too wide..
                                {
                                    int xoff = firstcolumn.X - leftmiddle.X;                     // shift to firstcolumn.x, maxitemspos.Y+planetspacer
                                    int yoff = (maxitemspos.Y+planetspacery) - (leftmiddle.Y-planetsize.Height/2);

                                    RepositionTree(pc, xoff, yoff);        // shift co-ords of all you've drawn

                                    maxplanetpos = new Point(maxplanetpos.X + xoff, maxplanetpos.Y + yoff);     // remove the shift from maxpos

                                    leftmiddle = new Point(maxplanetpos.X + planetspacerx, leftmiddle.Y + yoff);   // and set the curpos to maxpos.x + spacer, remove the shift from curpos.y
                                }
                                else
                                    leftmiddle = new Point(maxplanetpos.X + planetspacerx, leftmiddle.Y);     // shift current pos right, plus a spacer

                                maxitemspos = new Point(Math.Max(maxitemspos.X, maxplanetpos.X), Math.Max(maxitemspos.Y, maxplanetpos.Y));

                                starcontrols.AddRange(pc.ToArray());
                            }
                        }

                        // do any futher belts after all planets

                        while (displaybelts && lastbelt != null)
                        {
                            if (leftmiddle.X + planetsize.Width > panelStars.Width - panelStars.ScrollBarWidth) // if too far across..
                            {
                                leftmiddle = new Point(firstcolumn.X, maxitemspos.Y + planetspacery + planetsize.Height / 2); // move to left at maxy+space+h/2
                            }

                            string appendlabel = Environment.NewLine + $"{lastbelt.BeltData.OuterRad / JournalScan.oneLS_m:N1}ls";

                            Point maxbeltpos = DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt, leftmiddle, false, out int unusedbelt2centre, beltsize, appendlabeltext:appendlabel);

                            leftmiddle = new Point(maxbeltpos.X + planetspacerx, leftmiddle.Y);
                            lastbelt = belts.Count != 0 ? belts.Dequeue() : null;

                            maxitemspos = new Point(Math.Max(maxitemspos.X, maxbeltpos.X), Math.Max(maxitemspos.Y, maxbeltpos.Y));
                        }

                        maxitemspos = leftmiddle = new Point(leftmargin, maxitemspos.Y + starplanetgroupspacery + StarSize.Height / 2);     // move back to left margin and move down to next position of star, allowing gap
                    }
                    else
                    {               // no planets, so just move across and plot another one
                        leftmiddle = new Point(maxitemspos.X + starfirstplanetspacerx, leftmiddle.Y);

                        if (leftmiddle.X + StarSize.Width > panelStars.Width - panelStars.ScrollBarWidth) // if too far across..
                        {
                            maxitemspos = leftmiddle = new Point(leftmargin, maxitemspos.Y + starplanetgroupspacery + StarSize.Height / 2); // move to left at maxy+space+h/2
                        }
                    }

                    DisplayAreaUsed = new Point(Math.Max(DisplayAreaUsed.X, maxitemspos.X), Math.Max(DisplayAreaUsed.Y, maxitemspos.Y));
                }

                imagebox.AddRange(starcontrols);
            }

            imagebox.Render();      // replaces image..
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<ExtPictureBox.ImageElement> pc, StarScan.ScanNode planetnode, MaterialCommoditiesList curmats, HistoryList hl, Point leftmiddle, string[] filter, bool habzone)
        {
            Color? backwash = null;
            if (habzone)
                backwash = Color.FromArgb(255, 0, 30, 0);

            Point maxtreepos = DrawNode(pc, planetnode, curmats, hl, JournalScan.GetPlanetImageNotScanned(),
                                leftmiddle, false, out int planetcentre, planetsize, backwash: backwash);        // offset passes in the suggested offset, returns the centre offset

            if (planetnode.children != null && ShowMoons)
            {
                Point moonposcentremid = new Point(planetcentre, maxtreepos.Y + moonspacery + moonsize.Height/2);    // moon pos, below planet, centre x coord

                foreach (StarScan.ScanNode moonnode in planetnode.children.Values.Where(n => n.type != StarScan.ScanNodeType.barycentre))
                {
                    if (filter != null && moonnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                        continue;

                    bool nonedsmscans = moonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                    if (nonedsmscans || CheckEDSM)
                    {
                        Point mmax = DrawNode(pc, moonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), moonposcentremid, true, out int mooncentre, moonsize);

                        maxtreepos = new Point(Math.Max(maxtreepos.X, mmax.X), Math.Max(maxtreepos.Y, mmax.Y));

                        if (moonnode.children != null)
                        {
                            Point submoonpos = new Point(mmax.X + moonspacerx, moonposcentremid.Y);     // first its left mid
                            bool xiscentre = false;

                            //for ( int i = 0; i < 1; i++) { StarScan.ScanNode submoonnode = moonnode; // debug
                            foreach (StarScan.ScanNode submoonnode in moonnode.children.Values)
                            {

                                if (filter != null && submoonnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                                    continue;

                                bool nonedsmsubmoonscans = submoonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                                if (nonedsmsubmoonscans || CheckEDSM)
                                {
                                    Point sbmax = DrawNode(pc, submoonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), submoonpos, xiscentre, out int xsubmooncentre, moonsize);

                                    if (xiscentre)
                                        submoonpos = new Point(submoonpos.X, sbmax.Y + moonspacery + moonsize.Height / 2);
                                    else
                                    {
                                        submoonpos = new Point(xsubmooncentre, sbmax.Y + moonspacery + moonsize.Height / 2);
                                        xiscentre = true;       // now go to centre placing
                                    }

                                    maxtreepos = new Point(Math.Max(maxtreepos.X, sbmax.X), Math.Max(maxtreepos.Y, sbmax.Y));
                                }
                            }
                        }

                        moonposcentremid = new Point(moonposcentremid.X, maxtreepos.Y + moonspacery + moonsize.Height/2);
                    }
                }
            }

            return maxtreepos;
        }


        // return right bottom of area used from curpos

        Point DrawNode(List<ExtPictureBox.ImageElement> pc,
                            StarScan.ScanNode sn,
                            MaterialCommoditiesList curmats,    // curmats may be null
                            HistoryList hl,
                            Image notscanned,               // image if sn is not known
                            Point position,                 // position is normally left/middle, unless xiscentre is set.
                            bool xiscentre,
                            out int ximagecentre,           // centre in x of image
                            Size size,                      // nominal size
                            bool toplevelstar = false,      // its top level, don't draw as planet
                            Color? backwash = null,         // optional back wash on image (planet nodes only)
                            string appendlabeltext = ""     // any label text to append
                )         
        {
            string tip;
            Point endpoint = position;
            ximagecentre = -1;

            JournalScan sc = sn.ScanData;

            if (sc != null && (!sc.IsEDSMBody || CheckEDSM))     // if got one, and its our scan, or we are showing EDSM
            {
                tip = sc.DisplayString(historicmatlist:curmats, currentmatlist:hl.GetLast?.MaterialCommodity);
                if (sn.Signals != null)
                    tip += "\n" + "Signals".T(EDTx.ScanDisplayUserControl_Signals)+":\n" + JournalSAASignalsFound.SignalList(sn.Signals,4, "\n");

                if ( sn.type == StarScan.ScanNodeType.ring)
                {

                }
                else  if (sc.IsStar && toplevelstar)
                {
                    var starlabel = sn.customname ?? sn.ownname;

                    if ( sc.nStellarMass.HasValue )
                        starlabel += Environment.NewLine + $"{sc.nStellarMass.Value:N2} SM";
                    if ( sc.nAge.HasValue)
                        starlabel += Environment.NewLine + $"{sc.nAge.Value:N0} MY";

                    if (ShowHabZone)
                    {
                        var habZone = sc.GetHabZoneStringLs();
                        if (habZone.HasChars())
                            starlabel += Environment.NewLine + $"{habZone}";
                    }

                    starlabel += appendlabeltext;

                    string overlaytext = null;
                    if (ShowStarClasses && sn.ScanData != null && sn.ScanData.StarTypeID != EDStar.H)
                        overlaytext = sn.ScanData.StarClassification;

                    Image starimage = sc.GetStarTypeImage();
                    bool owned = false;

                    if (overlaytext.HasChars())
                    {
                        Image clone = starimage.Clone() as Image;
                        using (Font f = new Font(EDDTheme.Instance.FontName, starimage.Width / 6.0f))
                        {
                            using (Graphics g = Graphics.FromImage(clone))
                            {
                                using (Brush b = new SolidBrush(Color.Black))
                                {
                                    g.DrawString(overlaytext, f, b, new Rectangle(0, 0, starimage.Width, starimage.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                                }
                            }
                        }

                        starimage = clone;
                        owned = true;
                    }

                    endpoint = CreateImageAndLabel(pc, starimage,
                                                    position,      // WE are basing it on a 1/4 + 1 + 1/4 grid, this is not being made bigger, move off
                                                    size, out ximagecentre , starlabel, tip, sc.IsEDSMBody, owned);          // and the label needs to be a quarter height below it..
                }
                else //else not a top-level star or its a planet
                {
                    Image nodeimage = sc.IsStar ? sc.GetStarTypeImage() : sc.GetPlanetClassImage();

                    bool valuable = sc.EstimatedValue >= ValueLimit;
                    int imageoverlays = ShowOverlays ? ((sc.Terraformable ? 1 : 0) + (sc.HasMeaningfulVolcanism ? 1 : 0) + (valuable ? 1 : 0) + (sc.Mapped ? 1 : 0) + (sn.Signals != null ? 1 : 0)) : 0;
                    bool materialsicon = sc.HasMaterials && !ShowMaterials;
                    string overlaytext = "";

                    if (ShowPlanetClasses)
                    {
                        overlaytext = sc.IsStar ? sc.StarClassification : Bodies.PlanetAbv(sc.PlanetTypeID);
                    }

                    var nodelabel = sn.customname ?? sn.ownname;

                    if (!sc.IsStar && (sn.ScanData.IsLandable || ShowAllG) && sn.ScanData.nSurfaceGravity != null)
                    {
                        nodelabel += Environment.NewLine + $"{(sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2):N2}g";
                    }

                    if ( ShowDist && sn.ScanData.nSemiMajorAxis.HasValue)
                    {
                        nodelabel += Environment.NewLine + $"{(sn.ScanData.nSemiMajorAxis / JournalScan.oneLS_m):N1}ls";
                    }

                    nodelabel += appendlabeltext;

                    bool requiresbigoverlay =   sc.IsLandable ||
                                                sc.HasRings ||
                                                imageoverlays > 0 ||
                                                materialsicon;

                    int quarterheight = size.Height / 4;

                    Size bmpsize = new Size(size.Width * (requiresbigoverlay ? 2 : 1), quarterheight * 6);

                    Bitmap bmp = new Bitmap(bmpsize.Width, bmpsize.Height);

                    //backwash = Color.FromArgb(255, 80, 0, 0); //debug

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        if (backwash.HasValue)
                        {
                            using (Brush b = new SolidBrush(backwash.Value))
                            {
                                g.FillRectangle(b, new Rectangle(0, 0, bmpsize.Width, bmpsize.Height));
                            }
                        }

                        g.DrawImage(nodeimage, bmpsize.Width / 2 - size.Width/2, bmpsize.Height/2 - size.Height/2, size.Width, size.Height);

                        if (sc.IsLandable)
                            g.DrawImage(Icons.Controls.Scan_Bodies_Landable, new Rectangle(quarterheight, 0, quarterheight * 6, quarterheight * 6));

                        if (sc.HasRings)
                            g.DrawImage(sc.Rings.Count() > 1 ? Icons.Controls.Scan_Bodies_RingGap : Icons.Controls.Scan_Bodies_RingOnly,
                                            new Rectangle(-2, quarterheight, size.Width * 2, size.Height));

                        if (imageoverlays>0)
                        {
                            int ovsize = (imageoverlays > 1) ? quarterheight : (quarterheight * 3 / 2);
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
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_HighValue, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (sc.Mapped)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Mapped, new Rectangle(0, pos, ovsize, ovsize));
                                pos += ovsize + 1;
                            }

                            if (sn.Signals != null)
                            {
                                g.DrawImage(Icons.Controls.Scan_Bodies_Signals, new Rectangle(0, pos, ovsize, ovsize));
                            }
                        }

                        if (materialsicon)
                        {
                            Image mm = Icons.Controls.Scan_Bodies_MaterialMore;
                            g.DrawImage(mm, new Rectangle(bmp.Width - mm.Width, bmp.Height - mm.Height, mm.Width, mm.Height));
                        }

                        if (overlaytext.HasChars())
                        {
                            using (Font f = new Font(EDDTheme.Instance.FontName, size.Width / 5.0f))
                            {
                                Color text = sc.IsStar ? Color.Black : Color.FromArgb(255,160,190,160);

                                using (Brush b = new SolidBrush(text))
                                {
                                    g.DrawString(overlaytext, f, b, new Rectangle(0, 0, bmpsize.Width, bmpsize.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                                }
                            }
                        }
                    }

                    Point postoplot = xiscentre ? new Point(position.X - bmpsize.Width / 2, position.Y) : position;

                    endpoint = CreateImageAndLabel(pc, bmp, postoplot, bmpsize, out ximagecentre, nodelabel, tip, sc.IsEDSMBody);

                    if (sc.HasMaterials && ShowMaterials)
                    {
                        Point matpos = new Point(endpoint.X + 4, position.Y);
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
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".T(EDTx.ScanDisplayUserControl_NSD);

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

                string nodelabel = sn.ownname;
                nodelabel += appendlabeltext;

                endpoint = CreateImageAndLabel(pc, Icons.Controls.Scan_Bodies_Belt, position, size, out ximagecentre, nodelabel, tip, false, false);
            }
            else
            {
                if (sn.type == StarScan.ScanNodeType.barycentre)
                    tip = string.Format("Barycentre of {0}".T(EDTx.ScanDisplayUserControl_BC) , sn.ownname);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".T(EDTx.ScanDisplayUserControl_NSD);

                string nodelabel = sn.customname ?? sn.ownname;
                nodelabel += appendlabeltext;

                endpoint = CreateImageAndLabel(pc, notscanned, position, size, out ximagecentre, nodelabel, tip, false, false);
            }

            System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + position + " " + size + " -> "+ endpoint);
            return endpoint;
        }

        // curmats may be null
        Point CreateMaterialNodes(List<ExtPictureBox.ImageElement> pc, JournalScan sn, MaterialCommoditiesList curmats, HistoryList hl, Point matpos, Size matsize)
        {
            Point startpos = matpos;
            Point maximum = matpos;
            int noperline = 0;

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
                        MaterialCommodities matnow = curmats?.Find(mc);  // allow curmats = null

                        // debug if (matnow != null && mc.shortname == "Fe")  matnow.count = 10000;
                            
                        if (matnow != null && limit != null && matnow.Count >= limit)        // and limit
                            continue;
                    }

                    if (ShowOnlyMaterialsRare && mc.IsCommonMaterial )
                        continue;
                }

                System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
                colormap.OldColor = Color.White;    // this is the marker colour to replace
                colormap.NewColor = fillc;

                Bitmap mat = BaseUtils.BitMapHelpers.ReplaceColourInBitmap((Bitmap)Icons.Controls.Scan_Bodies_Material, new System.Drawing.Imaging.ColorMap[] { colormap });

                BaseUtils.BitMapHelpers.DrawTextCentreIntoBitmap(ref mat, abv, stdfont, Color.Black);

                ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement(
                                new Rectangle(matpos.X, matpos.Y, matsize.Width, matsize.Height), mat, tooltip + "\n\n" + "All " + matclicktext, tooltip);

                pc.Add(ie);

                maximum = new Point(Math.Max(maximum.X, matpos.X + matsize.Width), Math.Max(maximum.Y, matpos.Y + matsize.Height));

                if (++noperline == 4)
                {
                    matpos = new Point(startpos.X, matpos.Y + matsize.Height + materiallinespacerxy);
                    noperline = 0;
                }
                else
                    matpos.X += matsize.Width + materiallinespacerxy;
            }

            return maximum;
        }


        // plot at leftmiddle the image of size, return bot left accounting for label 
        // label can be null. returns ximagecentre of image

        Point CreateImageAndLabel(List<ExtPictureBox.ImageElement> c, Image i, Point leftmiddle, Size size, out int ximagecentre, string label,
                                    string ttext, bool fromEDSM, bool imgowned = true)
        {
            //System.Diagnostics.Debug.WriteLine("    " + label + " " + postopright + " size " + size + " hoff " + labelhoff + " laby " + (postopright.Y + size.Height + labelhoff));

            ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement(new Rectangle(leftmiddle.X, leftmiddle.Y - size.Height/2, size.Width, size.Height), i, ttext, ttext, imgowned);

            Point max = new Point(leftmiddle.X + size.Width, leftmiddle.Y + size.Height/2);

            if (label != null)
            {
                Font font = fromEDSM ? stdfontUnderline : stdfont;

                Point labposcenthorz = new Point(leftmiddle.X + size.Width/2, leftmiddle.Y + size.Height/2);

                ExtPictureBox.ImageElement lab = new ExtPictureBox.ImageElement();

                Color backcolor = this.BackColor;       // override for debug
                lab.TextCentreAutosize(labposcenthorz, new Size(0,100), label, font, EDDTheme.Instance.LabelColor, backcolor, frmt: new StringFormat() { Alignment = StringAlignment.Center });

                if (lab.Location.X < ie.Location.X)
                {
                    int offset = ie.Location.X - lab.Location.X;
                    ie.Translate(offset, 0);
                    lab.Translate(offset, 0);
                }

                c.Add(lab);

                max = new Point(Math.Max(lab.Location.X + lab.Location.Width, max.X), lab.Location.Y + lab.Location.Height);
            }

            c.Add(ie);

            ximagecentre = ie.Location.X + ie.Location.Width / 2;

            //System.Diagnostics.Debug.WriteLine(" ... to " + label + " " + max + " size " + (new Size(max.X-postopright.X,max.Y-postopright.Y)));
            return max;
        }

        void RepositionTree(List<ExtPictureBox.ImageElement> pc, int xoff, int yoff)
        {
            foreach (ExtPictureBox.ImageElement c in pc)
                c.Translate(xoff, yoff);
        }

        public void SetSize(int stars)
        {
            StarSize = new Size(stars, stars);
            beltsize = new Size(StarSize.Width * 1 / 2, StarSize.Height);
            planetsize = new Size(StarSize.Width * 3 / 4, StarSize.Height * 3 / 4);
            moonsize = new Size(StarSize.Width * 2 / 4, StarSize.Height * 2 / 4);
            int matsize = stars >= 64 ? 24 : 16;
            materialsize = new Size(matsize, matsize);

            starfirstplanetspacerx = Math.Min(stars / 2, 16);      // 16/2=8 to 16
            starplanetgroupspacery = Math.Min(stars / 2, 24);      // 16/2=8 to 24
            planetspacerx = Math.Min(stars / 4, 16);       
            planetspacery = Math.Min(stars / 4, 16);
            moonspacerx = Math.Min(stars / 4, 8);
            moonspacery = Math.Min(stars / 4, 8);
            topmargin = 10;
            leftmargin = 8;
            materiallinespacerxy = 4;
        }

        #endregion

        #region User interaction

        private void ClickElement(object sender, MouseEventArgs e, ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null)
                ShowInfo((string)tag, i.Location.Location.X < panelStars.Width / 2);
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

        private void panelStars_MouseDown(object sender, MouseEventArgs e)
        {
            HideInfo();
        }

        public void HideInfo()
        {
            rtbNodeInfo.Visible = false;
        }

        void PositionInfo()
        {
            if (rtbNodeInfo.Visible)
            {
                int y = -panelStars.ScrollOffset;           // invert to get pixels down scrolled
                int width = panelStars.Width * 7 / 16;

                if (rtbNodeInfo.Tag != null && ((bool)rtbNodeInfo.Tag) == true)
                    rtbNodeInfo.Location = new Point(panelStars.Width - panelStars.ScrollBar.Width - 10 - width, y);
                else
                    rtbNodeInfo.Location = new Point(10, y);

                int h = Math.Min(rtbNodeInfo.EstimateVerticalSizeFromText(), panelStars.Height - 20);

                rtbNodeInfo.Size = new Size(width, h);
                rtbNodeInfo.PerformLayout();    // not sure why i need this..
            }
        }

        public void SetBackground(Color c)
        {
            panelStars.BackColor = imagebox.BackColor = vScrollBarCustom.SliderColor = vScrollBarCustom.BackColor = c;
        }


        #endregion
    }
}

