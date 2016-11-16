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
        Size starsize,planetsize,moonsize,compoundindicatorsize,materialsize;
        const int itemsepar = 0;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            travelhistorycontrol.OnTravelSelectionChanged += Display;
            toolTip.ShowAlways = true;

            richTextBoxInfo.Visible = false;


            SetSize(64);
        }

        #endregion

        public void Display(HistoryEntry he, HistoryList hl)
        {
            panelStars.RemoveAllControls( new List<Control> { richTextBoxInfo });

            if (he != null)
            {
                StarScan.SystemNode sn = hl.starscan.FindSystem(he.System);

                if (sn != null)
                    DrawSystem(sn);
            }
        }


        void DrawSystem(StarScan.SystemNode sn)
        {
            Point curpos = new Point(0, 30);
            List<Control> starcontrols = new List<Control>();

            for( int i = 0; i < 1000; i +=100)
            {
                CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, new Point(i, 0), new Size(24, 24), i.ToString(), "");
            }

            foreach (StarScan.ScanNode starnode in sn.starnodes.Values)        // always has scan nodes
            {
                Point maxstarpos;

                if (starnode.type == StarScan.ScanNodeType.star)
                {
                    Image star = (starnode.scandata != null) ? starnode.scandata.GetStarTypeImage() : JournalScan.GetStarImageNotScanned();
                    string tip = (starnode.scandata != null) ? starnode.scandata.DisplayString(true) : ("Star " + starnode.fullname);
                    string starname = starnode.ownname.Length == 0 ? starnode.fullname : starnode.ownname;

                    maxstarpos = CreateStarPlanet(starcontrols, star, curpos, starsize, starname, tip);
                }
                else
                    maxstarpos = CreateStarPlanet(starcontrols, EDDiscovery.Properties.Resources.ImageStarDiscWhite, curpos, compoundindicatorsize, starnode.ownname, "Orbiting barycentre of " + starnode.fullname);

                curpos.X = maxstarpos.X + itemsepar;

                Point planetfirstcolumn = curpos;

                Point maxplanetpos = maxstarpos;

                if (starnode.children != null)
                {
                    foreach (StarScan.ScanNode planetnode in starnode.children.Values)
                    {
                        List<Control> pc = new List<Control>();

                        Point maxpos = CreatePlanetTree(pc, planetnode, curpos);

                        System.Diagnostics.Debug.WriteLine("Planet " + planetnode.ownname + " " + curpos + " " + maxpos + " max " + (panelStars.Width - panelStars.ScrollBarWidth));

                        if ( maxpos.X > panelStars.Width - panelStars.ScrollBarWidth)          // uh oh too wide..
                        {
                            int xoffset = planetfirstcolumn.X - curpos.X;
                            int yoffset = maxplanetpos.Y - curpos.Y;

                            RepositionTree(pc, xoffset, yoffset);        // shift co-ords of all you've drawn

                            maxpos = new Point(maxpos.X + xoffset, maxpos.Y + yoffset);
                            curpos = new Point(maxpos.X, curpos.Y + yoffset);
                        }
                        else
                            curpos = new Point(maxpos.X, curpos.Y);

                        maxplanetpos = new Point(Math.Max(maxplanetpos.X, maxpos.X), Math.Max(maxplanetpos.Y, maxpos.Y));
                        
                        starcontrols.AddRange(pc.ToArray());
                    }
                }

                curpos = new Point(0, maxplanetpos.Y + itemsepar);
            }

            panelStars.Controls.AddRange(starcontrols.ToArray());
        }

        void RepositionTree( List<Control> pc, int xoff, int yoff )
        {
            foreach( Control c in pc)
                c.Location = new Point(c.Location.X + xoff, c.Location.Y + yoff);
        }

        Point CreatePlanetTree( List<Control> pc, StarScan.ScanNode planetnode , Point curpos )
        {
            Image planet = (planetnode.scandata != null) ? planetnode.scandata.GetPlanetClassImage() : JournalScan.GetPlanetImageNotScanned();
            string tip = (planetnode.scandata != null) ? planetnode.scandata.DisplayString(true) : ("Planet " + planetnode.fullname);

            Point maxtreepos = CreateStarPlanet(pc, planet, curpos, planetsize, planetnode.ownname, tip);
            Point moonpos = new Point(curpos.X + (planetsize.Width - moonsize.Width)/2, maxtreepos.Y + itemsepar);

            if (planetnode.children != null)
            {
                foreach (StarScan.ScanNode moonnode in planetnode.children.Values)
                {
                    Image moon = (moonnode.scandata != null) ? moonnode.scandata.GetPlanetClassImage() : JournalScan.GetMoonImageNotScanned();
                    string mtip = (moonnode.scandata != null) ? moonnode.scandata.DisplayString(true) : ("Moon " + moonnode.fullname);

                    Point moonposend = CreateStarPlanet(pc, moon, moonpos, moonsize, moonnode.ownname, mtip);

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

                System.Diagnostics.Debug.WriteLine("Item " + mc.name + " abv " + mc.shortname + " at "  + matpos);
                DrawnPanel dp = new DrawnPanel();
                dp.ImageSelected = DrawnPanel.ImageType.Text;
                dp.DrawnImage = EDDiscovery.Properties.Resources.powerplayjoin;
                dp.ImageText = abv;
                dp.Location = matpos;
                dp.Size = matsize;
                dp.ForeColor = Color.White;

                System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
                colormap.OldColor = Color.White;
                colormap.NewColor = fillc;
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


        Point CreateStarPlanet(List<Control> c, Image i, Point postopright, Size size, string label, string ttext)
        {
            System.Diagnostics.Debug.WriteLine(postopright + " " + label);
            PictureBox pb = new PictureBox();
            pb.Image = i;
            pb.Size = size;
            pb.Location = postopright;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Tag = ttext;
            pb.Click += Pb_Click;
            c.Add(pb);
            toolTip.SetToolTip(pb, ttext);

            DrawnPanel dp = new ExtendedControls.DrawnPanel();
            dp.ImageSelected = DrawnPanel.ImageType.Text;
            dp.ImageText = label;
            int labelhalfwidth = size.Width * 3 / 4;

            dp.Location = new Point(postopright.X + size.Width/2 - labelhalfwidth, postopright.Y + size.Height);
            dp.Size = new Size(labelhalfwidth*2, 20);
            dp.ForeColor = discoveryform.theme.LabelColor;
            c.Add(dp);

            return new Point(postopright.X + size.Width / 2 + labelhalfwidth, postopright.Y + size.Height + 20);
            //return new Point(postopright.X + size.Width, postopright.Y + size.Height + 20);
        }

        void SetSize(int stars)
        {
            starsize = new Size(stars, stars);
            planetsize = new Size(starsize.Width * 3 / 4, starsize.Height * 3 / 4);
            moonsize = new Size(starsize.Width * 2 / 4, starsize.Height * 2 / 4);
            compoundindicatorsize = new Size(starsize.Width * 2 / 4, starsize.Height * 2 / 4);
            materialsize = new Size(24, 24);
        }



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

        private void panelStars_Resize(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Resize panel stars {0}" , panelStars.Size);
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
        }
    }
}

