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
        private Font stdfontUnderline = EDDTheme.Instance.GetDialogScaledFont(1f, FontStyle.Underline);
        private Font largerfont = EDDTheme.Instance.GetFont;

        const int noderatiodivider = 8;     // in eight sizes
        const int nodeheightratio = 12;     
        const int nodeoverlaywidthratio = 20;

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
                Point leftmiddle = new Point(leftmargin, topmargin + StarSize.Height * nodeheightratio / 2 / noderatiodivider);  // half down (h/2 * ratio)

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
                       // System.Diagnostics.Debug.WriteLine("SDUC Rejected " + starnode.fullname);
                        continue;
                    }

                    // Draw star

                    Point maxstarpos = DrawNode(starcontrols, starnode, curmats, hl,
                                (starnode.type == StarScan.ScanNodeType.barycentre) ? Icons.Controls.Scan_Bodies_Barycentre : JournalScan.GetStarImageNotScanned(),
                                leftmiddle, false, out int unusedstarcentre, StarSize, DrawLevel.TopLevelStar);       // the last part nerfs the label down to the right position

                    maxitemspos = new Point(Math.Max(maxitemspos.X, maxstarpos.X), Math.Max(maxitemspos.Y, maxstarpos.Y));

                    if (starnode.children != null)
                    {
                        leftmiddle = new Point(maxitemspos.X + starfirstplanetspacerx, leftmiddle.Y);
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

                        List<StarScan.ScanNode> planetsinorder = starnode.children.Values.Where(s => s.type == StarScan.ScanNodeType.body || s.type == StarScan.ScanNodeType.star).ToList();

                        for(int pn = 0; pn < planetsinorder.Count; pn++)
                        {
                            StarScan.ScanNode planetnode = planetsinorder[pn];

                            if (filter != null && planetnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                            {
                                //System.Diagnostics.Debug.WriteLine("SDUC Rejected " + planetnode.fullname);
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

                                Point maxbeltpos = DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt, leftmiddle,false,out int unusedbeltcentre, beltsize, DrawLevel.PlanetLevel, appendlabeltext:appendlabel);

                                leftmiddle = new Point(maxbeltpos.X + planetspacerx, leftmiddle.Y);
                                lastbelt = belts.Count != 0 ? belts.Dequeue() : null;

                                maxitemspos = new Point(Math.Max(maxitemspos.X, maxbeltpos.X), Math.Max(maxitemspos.Y, maxbeltpos.Y));
                            }

                           //System.Diagnostics.Debug.WriteLine("Planet Node " + planetnode.ownname + " has scans " + nonedsmscans);

                            if (planetnode.DoesNodeHaveNonEDSMScansBelow() || CheckEDSM)
                            {
                                List<ExtPictureBox.ImageElement> pc = new List<ExtPictureBox.ImageElement>();

                                bool habzone = false;

                                if (ShowHabZone && planetnode.ScanData != null)
                                {
                                    double dist = planetnode.ScanData.DistanceAccountingForBarycentre / JournalScan.oneLS_m;  // m , converted to LS
                                    habzone =  dist >= habzonestartls && dist <= habzoneendls;
                                }

                                Point maxplanetpos = CreatePlanetTree(pc, planetnode, curmats, hl, leftmiddle, filter, habzone , out int centreplanet);

                                int imgh = planetsize.Height * nodeheightratio / noderatiodivider;
                                int barytop = leftmiddle.Y - imgh / 2 - 12;
                                DrawBarycentre(pc, planetsinorder, pn, pn - 1, new Point(centreplanet, leftmiddle.Y), new Point(leftmiddle.X - planetspacerx, barytop), true);
                                DrawBarycentre(pc, planetsinorder, pn, pn + 1, new Point(centreplanet, leftmiddle.Y), new Point(maxplanetpos.X, barytop), true);

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

                            Point maxbeltpos = DrawNode(starcontrols, lastbelt, curmats, hl, Icons.Controls.Scan_Bodies_Belt, leftmiddle, false, out int unusedbelt2centre, beltsize, DrawLevel.PlanetLevel, appendlabeltext:appendlabel);

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

        // draw the barycentre line

        void DrawBarycentre(List<ExtPictureBox.ImageElement> pc, List<StarScan.ScanNode> bodiesinorder, int cur, int partner , Point pos1, Point pos2 , bool above)
        {
            if (bodiesinorder[cur].ScanData?.IsOrbitingBaryCentre ?? false)      // if have scan data and its orbiting a bary centre
            {
                if (partner>=0 && partner < bodiesinorder.Count && bodiesinorder[partner].HasSameParents(bodiesinorder[cur]))
                {
                    //System.Diagnostics.Debug.WriteLine(bodiesinorder[cur].fullname + " same parents as " + bodiesinorder[partner].fullname + " and is barycentre");

                    ExtPictureBox.ImageElement image = new ExtPictureBox.ImageElement();
                    image.OwnerDraw((g, ie) =>
                    {
                        using (Pen p = new Pen(Color.FromArgb(128, 170, 170, 170), 2)) // gray
                        {
                            //System.Diagnostics.Debug.WriteLine("Bary " + ie.Location.Location + "->" + ie.AltLocation.Location + " " + above);
                            Point midpoint = above ? new Point(ie.Location.X, ie.AltLocation.Y) : new Point(ie.AltLocation.X, ie.Location.Y);
                            g.DrawLine(p, ie.Location.Location, midpoint );
                            g.DrawLine(p, midpoint, ie.AltLocation.Location );
                        }

                    }, new Rectangle(pos1.X,pos1.Y,0,0 ));     // set to zero size so won't hit
                    image.AltLocation = new Rectangle(pos2.X,pos2.Y,0,0);

                    pc.Insert(0,image);  // need to draw it under the images, so add first
                }
            }
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<ExtPictureBox.ImageElement> pc, StarScan.ScanNode planetnode, MaterialCommoditiesList curmats, HistoryList hl, Point leftmiddle, 
                                string[] filter, bool habzone, out int planetcentre )
        {
            Color? backwash = null;
            if ( habzone )
                backwash = Color.FromArgb(64, 0, 128, 0);       // transparent in case we have a non black background

            Point maxtreepos = DrawNode(pc, planetnode, curmats, hl, JournalScan.GetPlanetImageNotScanned(),
                                leftmiddle, false, out planetcentre, planetsize, DrawLevel.PlanetLevel, backwash: backwash);        // offset passes in the suggested offset, returns the centre offset

            if (planetnode.children != null && ShowMoons)
            {
                Point moonposcentremid = new Point(planetcentre, maxtreepos.Y + moonspacery + moonsize.Height/2);    // moon pos, below planet, centre x coord

                var moonnodes = planetnode.children.Values.Where(n => n.type != StarScan.ScanNodeType.barycentre).ToList();

                for( int mn = 0; mn < moonnodes.Count; mn++)
                {
                    StarScan.ScanNode moonnode = moonnodes[mn];

                    if (filter != null && moonnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                        continue;

                    bool nonedsmscans = moonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                    if (nonedsmscans || CheckEDSM)
                    {
                        Point mmax = DrawNode(pc, moonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), moonposcentremid, true, out int mooncentre, moonsize, DrawLevel.MoonLevel);

                        //                        int imgh = planetsize.Height * nodeheightratio / noderatiodivider;

                        int up = moonsize.Height/2 + moonspacery;       // go up by moon/2, plus spacer, which is what we use below to space them
                        int overlaywidth = moonsize.Height * nodeheightratio / noderatiodivider / 6;        // same as below in node
                        int left = moonposcentremid.X - moonsize.Width - overlaywidth - 4;      // worse case, tie into code in DrawNode for sizing

                        DrawBarycentre(pc, moonnodes, mn, mn - 1, new Point(mooncentre, moonposcentremid.Y), new Point(left, moonposcentremid.Y - up) , false);

                        maxtreepos = new Point(Math.Max(maxtreepos.X, mmax.X), Math.Max(maxtreepos.Y, mmax.Y));

                        if (moonnode.children != null)
                        {
                            Point submoonpos = new Point(mmax.X + moonspacerx, moonposcentremid.Y);     // first its left mid
                            bool xiscentre = false;

                            foreach (StarScan.ScanNode submoonnode in moonnode.children.Values)
                            {
                                if (filter != null && submoonnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                                    continue;

                                bool nonedsmsubmoonscans = submoonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                                if (nonedsmsubmoonscans || CheckEDSM)
                                {
                                    Point sbmax = DrawNode(pc, submoonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), submoonpos, xiscentre, out int xsubmooncentre, moonsize, DrawLevel.MoonLevel);

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

                        DrawBarycentre(pc, moonnodes, mn, mn + 1, new Point(mooncentre, moonposcentremid.Y), new Point(left, maxtreepos.Y), false);

                        moonposcentremid = new Point(moonposcentremid.X, maxtreepos.Y + moonspacery + moonsize.Height/2);
                        //System.Diagnostics.Debug.WriteLine("Next moon centre at " + moonposcentremid );
                    }
                }
            }

            return maxtreepos;
        }

        enum DrawLevel { TopLevelStar, PlanetLevel, MoonLevel };

        // return right bottom of area used from curpos

        Point DrawNode(     List<ExtPictureBox.ImageElement> pc,
                            StarScan.ScanNode sn,
                            MaterialCommoditiesList curmats,    // curmats may be null
                            HistoryList hl,
                            Image notscanned,               // image if sn is not known
                            Point position,                 // position is normally left/middle, unless xiscentre is set.
                            bool xiscentre,
                            out int ximagecentre,           // centre in x of image
                            Size size,                      // nominal size
                            DrawLevel drawtype,          // drawing..
                            Color? backwash = null,         // optional back wash on image (planet nodes only)
                            string appendlabeltext = ""     // any label text to append
                )         
        {
            string tip;
            Point endpoint = position;
            ximagecentre = -1;

            JournalScan sc = sn.ScanData;

            if (sc != null && (!sc.IsEDSMBody || CheckEDSM))     // has a scan and its our scan, or we are showing EDSM
            {
                if (sn.type != StarScan.ScanNodeType.ring)       // not rings
                {
                    tip = sc.DisplayString(historicmatlist: curmats, currentmatlist: hl.GetLast?.MaterialCommodity);
                    if (sn.Signals != null)
                        tip += "\n" + "Signals".T(EDTx.ScanDisplayUserControl_Signals) + ":\n" + JournalSAASignalsFound.SignalList(sn.Signals, 4, "\n");

                    Image nodeimage = sc.IsStar ? sc.GetStarTypeImage() : sc.GetPlanetClassImage();

                    string overlaytext = "";
                    var nodelabels = new string[2] { "", "" };

                    nodelabels[0] = sn.customname ?? sn.ownname;
                    if (sc.IsEDSMBody)
                        nodelabels[0] = "_" + nodelabels[0];

                    if ( sc.IsStar )
                    {
                        if (ShowStarClasses)
                            overlaytext = sc.StarClassification;

                        if (sc.nStellarMass.HasValue)
                            nodelabels[1] = nodelabels[1].AppendPrePad($"{sc.nStellarMass.Value:N2} SM",Environment.NewLine);

                        if (drawtype == DrawLevel.TopLevelStar)
                        {
                            if (sc.nAge.HasValue)
                                nodelabels[1] = nodelabels[1].AppendPrePad($"{sc.nAge.Value:N0} MY", Environment.NewLine);

                            if (ShowHabZone)
                            {
                                var habZone = sc.GetHabZoneStringLs();
                                if (habZone.HasChars())
                                    nodelabels[1] = nodelabels[1].AppendPrePad($"{habZone}", Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        if (ShowPlanetClasses)
                            overlaytext = Bodies.PlanetAbv(sc.PlanetTypeID);

                        if ((sn.ScanData.IsLandable || ShowAllG) && sn.ScanData.nSurfaceGravity != null)
                        {
                            nodelabels[1] = nodelabels[1].AppendPrePad( $"{(sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2):N2}g", Environment.NewLine);
                        }
                    }

                    if (ShowDist)
                    {
                        if ( sn.ScanData.IsOrbitingBaryCentre)          // if in orbit of barycentre
                        {
                            if ( drawtype != DrawLevel.MoonLevel)       // can't do moons
                            {
                                string s = $"{(sn.ScanData.DistanceFromArrivalLS):N1}ls";
                                if (sn.ScanData.nSemiMajorAxis.HasValue)
                                    s += "/" + sn.ScanData.SemiMajorAxisLSKM;

                                nodelabels[1] = nodelabels[1].AppendPrePad(s, Environment.NewLine);
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine("Detected barycentre moon " + sn.fullname);
                            }
                        }
                        else if ( sn.ScanData.nSemiMajorAxis.HasValue )
                        {
                            nodelabels[1] = nodelabels[1].AppendPrePad($"{(sn.ScanData.nSemiMajorAxis / JournalScan.oneLS_m):N1}ls", Environment.NewLine);
                        }
                    }

                    nodelabels[1] = nodelabels[1].AppendPrePad(appendlabeltext, Environment.NewLine);

                    bool valuable = sc.EstimatedValue >= ValueLimit;
                    int iconoverlays = ShowOverlays ? ((sc.Terraformable ? 1 : 0) + (sc.HasMeaningfulVolcanism ? 1 : 0) + (valuable ? 1 : 0) + (sc.Mapped ? 1 : 0) + (sn.Signals != null ? 1 : 0)) : 0;

//   if (sc.BodyName.Contains("4 b"))  iconoverlays = 0;

                    bool materialsicon = sc.HasMaterials && !ShowMaterials;
                    bool imageoverlays = sc.IsLandable || (sc.HasRings && drawtype != DrawLevel.TopLevelStar) || materialsicon;

                    int bitmapheight = size.Height * nodeheightratio / noderatiodivider;
                    int overlaywidth = bitmapheight / 6;
                    int imagewidtharea = (imageoverlays ? 2:1) * size.Width;            // area used by image+overlay if any
                    int iconwidtharea = (iconoverlays > 0 ? overlaywidth : 0);          // area used by icon width area on left

                    int bitmapwidth = iconwidtharea + imagewidtharea;                   // total width
                    int imageleft = iconwidtharea + imagewidtharea / 2 - size.Width/2;  // calculate where the left of the image is 
                    int imagetop = bitmapheight / 2 - size.Height / 2;                  // and the top

                    Bitmap bmp = new Bitmap(bitmapwidth, bitmapheight);
                    
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        //backwash = Color.FromArgb(128, 40, 40, 40);
                        if (backwash.HasValue)
                        {
                            using (Brush b = new SolidBrush(backwash.Value))
                            {
                                g.FillRectangle(b, new Rectangle(0, 0, bitmapwidth, bitmapheight));
                            }
                        }

                        g.DrawImage(nodeimage, imageleft, imagetop , size.Width, size.Height);

                        if (sc.IsLandable)
                        {
                            int offset = size.Height * 4 / 16;
                            int scale = 5;
                            g.DrawImage(Icons.Controls.Scan_Bodies_Landable, new Rectangle(imageleft + size.Width/2 - offset * scale / 2, 
                                                                                           imagetop + size.Height/2 - offset * scale / 2, offset * scale, offset * scale));
                        }

                        if (sc.HasRings && drawtype != DrawLevel.TopLevelStar)
                        {
                            g.DrawImage(sc.Rings.Count() > 1 ? Icons.Controls.Scan_Bodies_RingGap : Icons.Controls.Scan_Bodies_RingOnly,
                                            new Rectangle(imageleft-size.Width/2, imagetop, size.Width * 2, size.Height));
                        }

                        if (iconoverlays > 0)
                        {
                            int ovsize = bmp.Height / 6;
                            int pos = 4;

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
                                Color text = sc.IsStar ? Color.Black : Color.FromArgb(255,100,140,100);

                                using (Brush b = new SolidBrush(text))
                                {
                                    g.DrawString(overlaytext, f, b, new Rectangle(iconwidtharea, 0, bitmapwidth-iconwidtharea, bitmapheight), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                                }
                            }
                        }
                    }

                    // for xcentre, center the image around position.X, not the bitmap, the icon width area is off to the left
                    Point postoplot = xiscentre ? new Point(position.X - imagewidtharea / 2 - iconwidtharea, position.Y) : position;

                    //System.Diagnostics.Debug.WriteLine("Body " + sc.BodyName + " plot at "  + postoplot + " " + bmp.Size + " " + (postoplot.X+imageleft) + "," + (postoplot.Y-bmp.Height/2+imagetop));


                    endpoint = CreateImageAndLabel(pc, bmp, postoplot, bmp.Size, out ximagecentre, nodelabels, tip);
                    ximagecentre += iconwidtharea / 2;       // adjust for left icon width, since we have iconwidth + image..

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

                Size bmpsize = new Size(size.Width, planetsize.Height * nodeheightratio / noderatiodivider);

                endpoint = CreateImageAndLabel(pc, Icons.Controls.Scan_Bodies_Belt, position, bmpsize, out ximagecentre, new string[] { sn.ownname + appendlabeltext }, tip, false);
            }
            else
            {
                if (sn.type == StarScan.ScanNodeType.barycentre)
                    tip = string.Format("Barycentre of {0}".T(EDTx.ScanDisplayUserControl_BC) , sn.ownname);
                else
                    tip = sn.ownname + Environment.NewLine + Environment.NewLine + "No scan data available".T(EDTx.ScanDisplayUserControl_NSD);

                string nodelabel = sn.customname ?? sn.ownname;
                nodelabel += appendlabeltext;

                endpoint = CreateImageAndLabel(pc, notscanned, position, size, out ximagecentre, new string[] { nodelabel }, tip, false);
            }

        //    System.Diagnostics.Debug.WriteLine("Node " + sn.ownname + " " + position + " " + size + " -> "+ endpoint);
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

        Point CreateImageAndLabel(List<ExtPictureBox.ImageElement> c, Image i, Point leftmiddle, Size size, out int ximagecentre, string[] labels, string ttext, bool imgowned = true)
        {
            //System.Diagnostics.Debug.WriteLine("    " + label + " " + postopright + " size " + size + " hoff " + labelhoff + " laby " + (postopright.Y + size.Height + labelhoff));

            ExtPictureBox.ImageElement ie = new ExtPictureBox.ImageElement(new Rectangle(leftmiddle.X, leftmiddle.Y - size.Height/2, size.Width, size.Height), i, ttext, ttext, imgowned);

            Point max = new Point(leftmiddle.X + size.Width, leftmiddle.Y + size.Height/2);

            var labelie = new List<ExtPictureBox.ImageElement>();
            int laboff = 0;
            int vpos = leftmiddle.Y + size.Height / 2;

            foreach (string label in labels)
            {
                if (label.HasChars())
                {
                    Font f = stdfont;
                    int labcut = 0;
                    if (label[0] == '_')
                    {
                        f = stdfontUnderline;
                        labcut = 1;
                    }

                    Point labposcenthorz = new Point(leftmiddle.X + size.Width / 2, vpos);

                    ExtPictureBox.ImageElement labie = new ExtPictureBox.ImageElement();
                    Color backcolor = this.BackColor;       // override for debug

                    using (var frmt = new StringFormat() { Alignment = StringAlignment.Center })
                    {
                        labie.TextCentreAutosize(labposcenthorz, new Size(0, 1000), label.Substring(labcut), f, EDDTheme.Instance.LabelColor, backcolor, frmt:frmt );
                    }

                    labelie.Add(labie);

                    if (labie.Location.X < leftmiddle.X)
                        laboff = Math.Max(laboff, leftmiddle.X - labie.Location.X);
                    vpos += labie.Location.Height;
                }
            }


            foreach (var l in labelie)
            {
                l.Translate(laboff, 0);
                c.Add(l);
                max = new Point(Math.Max(max.X, l.Location.Right), Math.Max(max.Y, l.Location.Bottom));
                //System.Diagnostics.Debug.WriteLine("Label " + l.Location);
            }

            ie.Translate(laboff, 0);
            max = new Point(Math.Max(max.X, ie.Location.Right), Math.Max(max.Y, ie.Location.Bottom));
            c.Add(ie);

            //System.Diagnostics.Debug.WriteLine(".. Max " + max);

            ximagecentre = ie.Location.X + ie.Location.Width / 2;

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

