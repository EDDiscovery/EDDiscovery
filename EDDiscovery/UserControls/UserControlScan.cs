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

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;
        private int displaynumber = 0;
        Size starsize,planetsize,moonsize,materialsize,beltsize;
        const int itemsepar = 0;
        const int leftmargin = 30;

        StarScan.SystemNode last_sn = null;
        Point last_maxdisplayarea;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
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
            SetSize(64);
        }

        #endregion

        #region Display

        public void Display(HistoryEntry he, HistoryList hl)
        {
            panelStars.RemoveAllControls(new List<Control> { richTextBoxInfo });

            last_sn = null;

            if (he != null)
            {
                StarScan.SystemNode sn = hl.starscan.FindSystem(he.System);

                if (sn != null)
                {
                    last_sn = sn;
                    DrawSystem(sn);
                }
            }
        }


        void DrawSystem(StarScan.SystemNode sn)
        {
            
            Point curpos = new Point(leftmargin, 0);
            last_maxdisplayarea = curpos;

            List<Control> starcontrols = new List<Control>();

            //for( int i = 0; i < 1000; i +=100)  CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, new Point(i, 0), new Size(24, 24), i.ToString(), "");

            foreach (StarScan.ScanNode starnode in sn.starnodes.Values)        // always has scan nodes
            {
                Point maxstarpos;

                bool belts = false;

                //System.Diagnostics.Debug.WriteLine("Star Cp " + curpos);

                if (starnode.type == StarScan.ScanNodeType.star)
                {
                    Image star;
                    string tip;

                    if ( starnode.scandata != null)
                    {
                        star = starnode.scandata.GetStarTypeImage().Item1;
                        tip = starnode.scandata.DisplayString(true);
                        belts = starnode.scandata.HasRings;
                    }
                    else
                    {
                        star = JournalScan.GetStarImageNotScanned();
                        tip = "Star " + starnode.fullname;
                    }

                    maxstarpos = CreateImageLabel(starcontrols, star, curpos, starsize, starnode.ownname, tip);
                }
                else
                    maxstarpos = CreateImageLabel(starcontrols, EDDiscovery.Properties.Resources.Barycentre, curpos, starsize, starnode.ownname, "Barycentre of " + starnode.fullname);

                curpos = new Point(maxstarpos.X + itemsepar, curpos.Y);

                Point maxitemspos = maxstarpos;

                if (belts)
                {
                    System.Diagnostics.Debug.WriteLine("Belts Cp " + curpos);

                    maxitemspos = CreateStarBelts(starcontrols, starnode.scandata, curpos, beltsize, starnode.fullname);
                    curpos = new Point(maxitemspos.X + itemsepar, curpos.Y);   // move to the right
                }
                
                curpos.Y += (starsize.Height - planetsize.Height) / 2;            // slide down for planet vs star difference in size

                //System.Diagnostics.Debug.WriteLine("Moon Cp " + curpos);

                Point firstcolumn = curpos;

                if (starnode.children != null)
                {
                    foreach (StarScan.ScanNode planetnode in starnode.children.Values)
                    {
                        List<Control> pc = new List<Control>();

                        Point maxpos = CreatePlanetTree(pc, planetnode, curpos);

                        //System.Diagnostics.Debug.WriteLine("Planet " + planetnode.ownname + " " + curpos + " " + maxpos + " max " + (panelStars.Width - panelStars.ScrollBarWidth));

                        if ( maxpos.X > panelStars.Width - panelStars.ScrollBarWidth)          // uh oh too wide..
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
                curpos = new Point(leftmargin, maxitemspos.Y + itemsepar);
            }

            panelStars.Controls.AddRange(starcontrols.ToArray());
            System.Diagnostics.Debug.WriteLine("Display area " + last_maxdisplayarea);
        }

        Point CreateStarBelts(List<Control> pc, JournalScan sc, Point curpos, Size beltsize, string starname)
        {
            Point endbelt = curpos;

            for( int i = 0; i < sc.Rings.Length; i++ )
            {
                string name = sc.Rings[i].Name;
                if (name.Length > starname.Length && name.Substring(0, starname.Length).Equals(starname))
                    name = name.Substring(starname.Length).Trim();

                endbelt = CreateImageLabel(pc, EDDiscovery.Properties.Resources.Belt, curpos, beltsize, name,
                                                    sc.RingInformationMoons(i));

                curpos = new Point(endbelt.X + itemsepar, curpos.Y);
            }

            return endbelt;
        }

        Point CreatePlanetTree( List<Control> pc, StarScan.ScanNode planetnode , Point curpos )
        {
            Image planet;
            string tip;
            Point maxtreepos = curpos;

            System.Diagnostics.Debug.WriteLine("Planet Cp " + curpos);
            if (planetnode.scandata != null)
            {
                planet = planetnode.scandata.GetPlanetClassImage();
                tip = planetnode.scandata.DisplayString(true) + "\n" + planetnode.fullname + ":" + planetnode.ownname;

                if (planetnode.scandata.HasRings)
                {
                    Bitmap bmp = new Bitmap(planetsize.Width * 2, planetsize.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(planet, planetsize.Width / 2, 0, planetsize.Width, planetsize.Height);
                        g.DrawImage(EDDiscovery.Properties.Resources.Ring_Only_512, new Rectangle(-2, 0, bmp.Width, bmp.Height));
                    }

                    maxtreepos = CreateImageLabel(pc, bmp, curpos, new Size(bmp.Width, bmp.Height), planetnode.ownname, tip);
                }
                else
                {
                    maxtreepos = CreateImageLabel(pc, planet, new Point(curpos.X + planetsize.Width / 2, curpos.Y), planetsize, planetnode.ownname, tip);
                }
            }
            else
            {
                planet = JournalScan.GetPlanetImageNotScanned();
                tip = "Planet " + planetnode.fullname;
                maxtreepos = CreateImageLabel(pc, planet, new Point(curpos.X + planetsize.Width / 2, curpos.Y), planetsize, planetnode.ownname, tip);
            }

            maxtreepos = new Point(Math.Max(curpos.X + planetsize.Width * 2, maxtreepos.X), maxtreepos.Y);      // min 2 widths

            System.Diagnostics.Debug.WriteLine("..after planet " + maxtreepos);

            //based on planesize-moonwidth/2, with space for a double moon width
            Point moonpos = new Point(curpos.X + planetsize.Width - moonsize.Width/2, maxtreepos.Y + itemsepar);

            System.Diagnostics.Debug.WriteLine("moonpos " + maxtreepos);

            if (planetnode.children != null)
            {
                foreach (StarScan.ScanNode moonnode in planetnode.children.Values)
                {
                    Image moon;
                    string mtip;
                    Point moonposend = moonpos;

                    if (moonnode.scandata != null)
                    {
                        moon = moonnode.scandata.GetPlanetClassImage();
                        mtip = moonnode.scandata.DisplayString(true) + "\n" + moonnode.fullname + ":" + moonnode.ownname;

                        if ( moonnode.scandata.HasRings )
                        {
                            Bitmap bmp = new Bitmap(moonsize.Width * 2, moonsize.Height);
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                g.DrawImage(moon, moonsize.Width / 2, 0, moonsize.Width, moonsize.Height);
                                g.DrawImage(EDDiscovery.Properties.Resources.Ring_Only_512, new Rectangle(-2, 0, bmp.Width, bmp.Height));
                            }

                            moonposend = CreateImageLabel(pc, bmp, new Point(moonpos.X - moonsize.Width/2, moonpos.Y), new Size(bmp.Width, bmp.Height), moonnode.ownname, tip);
                        }
                        else
                            moonposend = CreateImageLabel(pc, moon, moonpos, moonsize, moonnode.ownname, mtip);

                    }
                    else
                    {
                        moon = JournalScan.GetMoonImageNotScanned();
                        mtip = "Moon " + moonnode.fullname;
                        moonposend = CreateImageLabel(pc, moon, moonpos, moonsize, moonnode.ownname, mtip);
                    }

                    if (moonnode.scandata != null && moonnode.scandata.HasMaterials)
                    {
                        Point matpos = new Point(moonposend.X + 4, moonpos.Y);

                        Point endmat = CreateMaterialNodes(pc, moonnode.scandata, matpos, materialsize);

                        int maxy = Math.Max(moonposend.Y, endmat.Y);        // materials may not drop below moon if on one line

                        moonpos = new Point(moonpos.X, maxy + itemsepar);
                        maxtreepos = new Point(Math.Max(maxtreepos.X, endmat.X), Math.Max(maxtreepos.Y, maxy));
                    }
                    else
                    {
                        moonpos = new Point(moonpos.X, moonposend.Y + itemsepar);
                        maxtreepos = new Point(Math.Max(maxtreepos.X, moonposend.X), Math.Max(maxtreepos.Y, moonposend.Y));
                    }
                }
            }

            return maxtreepos;
        }



        Point CreateMaterialNodes(List<Control> pc, JournalScan sn, Point matpos, Size matsize)
        {
            Point startpos = matpos;
            Point maximum = matpos;
            int noperline = 0;

            foreach (KeyValuePair<string,double> sd in sn.Materials)
            {
                string abv = sd.Key.Substring(0, 1);
                string tooltip = sd.Key;
                Color fillc = Color.Yellow;

                EDDiscovery2.DB.MaterialCommodities mc = EDDiscovery2.DB.MaterialCommodities.GetCachedMaterial(sd.Key);
                if ( mc != null )
                {
                    abv = mc.shortname;
                    fillc = mc.colour;
                    tooltip = mc.name + " (" + mc.shortname + ") " + mc.type + " " + sd.Value.ToString("0.0") + "%";
                }

               // System.Diagnostics.Debug.WriteLine("Item " + mc.name + " abv " + mc.shortname + " at "  + matpos);
                DrawnPanelNoTheme dp = new DrawnPanelNoTheme();
                dp.ImageSelected = DrawnPanel.ImageType.Text;
                dp.DrawnImage = EDDiscovery.Properties.Resources.materialmarker;
                dp.ImageText = abv;
                dp.Location = matpos;
                dp.Size = matsize;
                dp.ForeColor = Color.Black;

                System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
                colormap.OldColor = Color.White;    // this is the marker colour to replace
                colormap.NewColor = fillc;
                System.Diagnostics.Debug.WriteLine("Map white to " + fillc);
                dp.SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap });

                toolTip.SetToolTip(dp, tooltip);

                pc.Add(dp);

                maximum = new Point(Math.Max(maximum.X, matpos.X + matsize.Width), Math.Max(maximum.Y, matpos.Y + matsize.Height));
                
                if (++noperline == 4)
                {
                    matpos = new Point(startpos.X, matpos.Y + matsize.Height + 4);
                    noperline = 0;
                }
                else
                    matpos.X += matsize.Width + 4;
            }

            return maximum;
        }


        Point CreateImageLabel(List<Control> c, Image i, Point postopright, Size size, string label, string ttext)
        {
            System.Diagnostics.Debug.WriteLine(postopright + " " + label);
            PictureBox pb = new PictureBox();
            pb.Image = i;
            pb.Size = size;
            pb.Location = postopright;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            //pb.BackColor = Color.Red;     // use to show boundaries
            pb.Tag = ttext;
            pb.Click += Pb_Click;
            c.Add(pb);

            if ( ttext != null )
                toolTip.SetToolTip(pb, ttext);

            Point max = new Point(postopright.X + size.Width, postopright.Y + size.Height);

            if (label != null)
            {
                DrawnPanel dp = new ExtendedControls.DrawnPanel();
                dp.ImageSelected = DrawnPanel.ImageType.Text;
                dp.ImageText = label;
                int labelhalfwidth = size.Width * 3 / 4;

                dp.Location = new Point(postopright.X + size.Width / 2 - labelhalfwidth, postopright.Y + size.Height);
                dp.Size = new Size(labelhalfwidth * 2, 20);
                dp.ForeColor = discoveryform.theme.LabelColor;
                c.Add(dp);

                max = new Point( Math.Max(postopright.X + size.Width / 2 + labelhalfwidth, max.X), postopright.Y + size.Height + 20);
            }

            return max;
        }

        void RepositionTree(List<Control> pc, int xoff, int yoff)
        {
            foreach (Control c in pc)
                c.Location = new Point(c.Location.X + xoff, c.Location.Y + yoff);
        }

        void SetSize(int stars)
        {
            starsize = new Size(stars, stars);
            beltsize = new Size(stars/2, stars);
            planetsize = new Size(starsize.Width * 3 / 4, starsize.Height * 3 / 4);
            moonsize = new Size(starsize.Width * 2 / 4, starsize.Height * 2 / 4);
            materialsize = new Size(24, 24);
        }

        #endregion


        private void Pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ShowInfo((string)pb.Tag, pb.Location.X < panelStars.Width/2 );
        }

#region Layout

        public override void LoadLayout()
        {
        }

        public override void Closing()
        {
            travelhistorycontrol.OnTravelSelectionChanged -= Display;
        }

#endregion

        void ShowInfo(string text , bool onright )
        {
            richTextBoxInfo.Text = text;
            richTextBoxInfo.Tag = onright;
            richTextBoxInfo.Visible = true;
            richTextBoxInfo.Show();
            PositionInfo();
        }

        void HideInfo()
        {
            richTextBoxInfo.Visible = false;
        }

        void PositionInfo()
        {
            if (richTextBoxInfo.Visible)
            {
                if (((bool)richTextBoxInfo.Tag) == true)
                    richTextBoxInfo.Location = new Point(panelStars.Width / 2 + panelStars.Width / 16, 10);
                else
                    richTextBoxInfo.Location = new Point(panelStars.Width / 16, 10);

                richTextBoxInfo.Size = new Size(panelStars.Width * 6 / 16, panelStars.Height - 20);
            }
        }

        private void panelStars_Click(object sender, EventArgs e)
        {
            HideInfo();
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            PositionInfo();
            System.Diagnostics.Debug.WriteLine("Resize panel stars {0} {1}", DisplayRectangle, panelStars.Size);

            if (last_sn != null )
            {
                int newspace = panelStars.Width - panelStars.ScrollBarWidth;

                if (newspace < last_maxdisplayarea.X || newspace > last_maxdisplayarea.X + starsize.Width * 2)
                {
                    panelStars.RemoveAllControls(new List<Control> { richTextBoxInfo });
                    DrawSystem(last_sn);
                }
            }
        }
    }
}

#if false



#endif 