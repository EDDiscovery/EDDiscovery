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
                        var planetcentres = new Dictionary<StarScan.ScanNode, Point>();

                        for (int pn = 0; pn < planetsinorder.Count; pn++)
                        {
                            StarScan.ScanNode planetnode = planetsinorder[pn];

                            if (filter != null && planetnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                            {
                                //System.Diagnostics.Debug.WriteLine("SDUC Rejected " + planetnode.fullname);
                                continue;
                            }

                            //System.Diagnostics.Debug.WriteLine("Draw " + planetnode.ownname + ":" + planetnode.type);

                            // if belt is before this, display belts here

                            while (displaybelts && lastbelt != null && planetnode.ScanData != null && (lastbelt.BeltData == null || planetnode.ScanData.IsOrbitingBaryCentre || lastbelt.BeltData.OuterRad < planetnode.ScanData.nSemiMajorAxis))
                            {
                                // if too far across, go back to star
                                if (leftmiddle.X + planetsize.Width > panelStars.Width - panelStars.ScrollBarWidth) // if too far across..
                                {
                                    leftmiddle = new Point(firstcolumn.X, maxitemspos.Y + planetspacery + planetsize.Height / 2); // move to left at maxy+space+h/2
                                }

                                string appendlabel = "";

                                if (lastbelt.BeltData != null)
                                {
                                    appendlabel.AppendPrePad($"{lastbelt.BeltData.OuterRad / JournalScan.oneLS_m:N1}ls", Environment.NewLine);
                                }

                                appendlabel = appendlabel.AppendPrePad("" + lastbelt.ScanData?.BodyID, Environment.NewLine);

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

                                if (ShowHabZone && planetnode.ScanData != null && !planetnode.ScanData.IsOrbitingBaryCentre && planetnode.ScanData.nSemiMajorAxis.HasValue)
                                {
                                    double dist = planetnode.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m;  // m , converted to LS
                                    habzone =  dist >= habzonestartls && dist <= habzoneendls;
                                }

                                Point maxplanetpos = CreatePlanetTree(pc, planetnode, curmats, hl, leftmiddle, filter, habzone , out int centreplanet);

                                Point pcnt = new Point(centreplanet, leftmiddle.Y);

                                if (maxplanetpos.X > panelStars.Width - panelStars.ScrollBarWidth)          // uh oh too wide..
                                {
                                    int xoff = firstcolumn.X - leftmiddle.X;                     // shift to firstcolumn.x, maxitemspos.Y+planetspacer
                                    int yoff = (maxitemspos.Y+planetspacery) - (leftmiddle.Y-planetsize.Height/2);

                                    RepositionTree(pc, xoff, yoff);        // shift co-ords of all you've drawn - this will include any bary points drawn in moons

                                    pcnt.X += xoff; pcnt.Y += yoff; // need to account for planet centre

                                    maxplanetpos = new Point(maxplanetpos.X + xoff, maxplanetpos.Y + yoff);     // remove the shift from maxpos

                                    leftmiddle = new Point(maxplanetpos.X + planetspacerx, leftmiddle.Y + yoff);   // and set the curpos to maxpos.x + spacer, remove the shift from curpos.y
                                }
                                else
                                    leftmiddle = new Point(maxplanetpos.X + planetspacerx, leftmiddle.Y);     // shift current pos right, plus a spacer

                                maxitemspos = new Point(Math.Max(maxitemspos.X, maxplanetpos.X), Math.Max(maxitemspos.Y, maxplanetpos.Y));

                                starcontrols.AddRange(pc.ToArray());

                                planetcentres[planetnode] = pcnt;
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

                        // make a tree of the planets with their barycentres from the Parents information
                        var barynodes = StarScan.ScanNode.PopulateBarycentres(planetsinorder);  // children always made, barynode tree

                        StarScan.ScanNode.DumpTree(barynodes, "TOP", 0);

                        List<ExtPictureBox.ImageElement> pcb = new List<ExtPictureBox.ImageElement>();

                        foreach (var k in barynodes.children)   // for all barynodes.. display
                        {
                            DisplayBarynode(k.Value, 0, planetcentres, planetsinorder, pcb, planetsize.Height / 2);     // done after the reposition so true positions set up.
                        }

                        starcontrols.InsertRange(0,pcb); // insert at start so drawn under
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
                var mooncentres = new Dictionary<StarScan.ScanNode, Point>();

                for ( int mn = 0; mn < moonnodes.Count; mn++)
                {
                    StarScan.ScanNode moonnode = moonnodes[mn];

                    if (filter != null && moonnode.IsBodyInFilter(filter, true) == false)       // if filter active, but no body or children in filter
                        continue;

                    bool nonedsmscans = moonnode.DoesNodeHaveNonEDSMScansBelow();     // is there any scans here, either at this node or below?

                    if (nonedsmscans || CheckEDSM)
                    {
                        Point mmax = DrawNode(pc, moonnode, curmats, hl, JournalScan.GetMoonImageNotScanned(), moonposcentremid, true, out int mooncentre, moonsize, DrawLevel.MoonLevel);

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

                        mooncentres[moonnode] = new Point(mooncentre, moonposcentremid.Y);

                        moonposcentremid = new Point(moonposcentremid.X, maxtreepos.Y + moonspacery + moonsize.Height/2);

                        //System.Diagnostics.Debug.WriteLine("Next moon centre at " + moonposcentremid );
                    }
                }

                //// now, taking the moon modes, create a barycentre tree with those inserted in 
                var barynodes = StarScan.ScanNode.PopulateBarycentres(moonnodes);  // children always made, barynode tree

                foreach (var k in barynodes.children)   // for all barynodes.. display
                {
                    DisplayBarynode(k.Value, 0, mooncentres, moonnodes, pc, moonsize.Width * 5 / 4, true);
                }
            }

            return maxtreepos;
        }

        void RepositionTree(List<ExtPictureBox.ImageElement> pc, int xoff, int yoff)
        {
            foreach (ExtPictureBox.ImageElement c in pc)
            {
                c.Translate(xoff, yoff);

                var joinlist = c.Tag as List<BaryPointInfo>;        // barypoints need adjusting too
                if (joinlist != null)
                {
                    foreach (var p in joinlist)
                    {
                        p.point = new Point(p.point.X + xoff, p.point.Y + yoff);       
                        p.toppos = new Point(p.toppos.X + xoff, p.toppos.Y + yoff);
                    }
                }
            }
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
            topmargin = planetspacery = 40;     // enough space for a decent number of barycentres
            moonspacerx = Math.Min(stars / 4, 8);
            moonspacery = Math.Min(stars / 4, 8);
            leftmargin = 8;
            materiallinespacerxy = 4;
        }

        #endregion

        #region User interaction

        private void ClickElement(object sender, MouseEventArgs e, ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null && tag is string)
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

