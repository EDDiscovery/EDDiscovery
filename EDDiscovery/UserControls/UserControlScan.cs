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

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;
        private int displaynumber = 0;
        Size starsize,planetsize,moonsize,materialsize,beltsize;
        Size itemsepar;
        int leftmargin;
        int topmargin;
        const int materialspacer = 4;
        const int labelnerf = 4;

        private string DbSave { get { return "ScanPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

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

            checkBoxMaterials.Checked = SQLiteDBClass.GetSettingBool(DbSave+"Materials", true);
            checkBoxMaterialsRare.Checked = SQLiteDBClass.GetSettingBool(DbSave+"MaterialsRare", false);
            checkBoxMoons.Checked = SQLiteDBClass.GetSettingBool(DbSave+"Moons", true);
            int size = SQLiteDBClass.GetSettingInt(DbSave+"Size", 64);
            SetSizeCheckBoxes(size);
        }

        public override void LoadLayout()
        {
        }

        public override void Closing()
        {
            travelhistorycontrol.OnTravelSelectionChanged -= Display;
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

        public void Display(HistoryEntry he, HistoryList hl)
        {
            StarScan.SystemNode newnode = (he != null) ? hl.starscan.FindSystem(he.System) : null;

            if (newnode != last_sn)
            {
                last_sn = newnode;
                DrawSystem(last_sn);
            }
        }

        void DrawSystem(StarScan.SystemNode sn)
        {
            panelStars.RemoveAllControls(new List<Control> { richTextBoxInfo });        // clean up
            HideInfo();

            last_sn = sn;                                                               // remember in case we need to draw

            if (sn == null)
                return;
         
            Point curpos = new Point(leftmargin, topmargin);
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
                        tip = starnode.fullname + "\n\nNo scan data available";
                    }

                    maxstarpos = CreateImageLabel(starcontrols, star, curpos, starsize, starnode.ownname, tip,0);
                }
                else
                    maxstarpos = CreateImageLabel(starcontrols, EDDiscovery.Properties.Resources.Barycentre, curpos, starsize, starnode.ownname, "Barycentre of " + starnode.fullname,0);

                curpos = new Point(maxstarpos.X + itemsepar.Width, curpos.Y);

                Point maxitemspos = maxstarpos;

                if (belts)
                {
                    //System.Diagnostics.Debug.WriteLine("Belts Cp " + curpos);

                    maxitemspos = CreateStarBelts(starcontrols, starnode.scandata, curpos, beltsize, starnode.fullname);
                    curpos = new Point(maxitemspos.X + itemsepar.Width, curpos.Y);   // move to the right
                }
                
                curpos.Y += starsize.Height/2 - planetsize.Height*3/4;            // slide down for planet vs star difference in size

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
                curpos = new Point(leftmargin, maxitemspos.Y + itemsepar.Height);
            }

            panelStars.Controls.AddRange(starcontrols.ToArray());
            //System.Diagnostics.Debug.WriteLine("Display area " + last_maxdisplayarea);
        }

        Point CreateStarBelts(List<Control> pc, JournalScan sc, Point curpos, Size beltsize, string starname)
        {
            Point endbelt = curpos;

            for( int i = 0; i < sc.Rings.Length; i++ )
            {
                string name = sc.Rings[i].Name;
                if (name.Length > starname.Length && name.Substring(0, starname.Length).Equals(starname))
                    name = name.Substring(starname.Length).Trim();

                curpos.X += 4;      // a little spacing, image is tight

                endbelt = CreateImageLabel(pc, EDDiscovery.Properties.Resources.Belt, curpos, beltsize, name,
                                                    sc.RingInformationMoons(i),0);

                curpos = new Point(endbelt.X + itemsepar.Width, curpos.Y);
            }

            return endbelt;
        }

        // return right bottom of area used from curpos
        Point CreatePlanetTree(List<Control> pc, StarScan.ScanNode planetnode , Point curpos )
        {
            // PLANETWIDTH|PLANETWIDTH  (if drawing a full planet with rings/landing)
            // or
            // MOONWIDTH|MOONWIDTH      (if drawing a single width planet)
                                                                          // this offset, ONLY used if a single width planet, allows for two moons
            int offset = moonsize.Width - planetsize.Width / 2;           // centre is moon width, back off by planetwidth/2 to place the left edge of the planet

            Point maxtreepos = CreatePlanetMoonRingLanding(pc, planetnode.scandata, JournalScan.GetPlanetImageNotScanned(), 
                                curpos, planetsize, planetnode.ownname, ref offset , true);        // offset passes in the suggested offset, returns the centre offset

            if (planetnode.children != null && checkBoxMoons.Checked)
            {
                offset -= moonsize.Width;               // offset is centre of planet image, back off by a moon width to allow for 2 moon widths centred

                Point moonpos = new Point(curpos.X + offset, maxtreepos.Y + itemsepar.Height);    // moon pos

                foreach (StarScan.ScanNode moonnode in planetnode.children.Values)
                {
                    int offsetm = moonsize.Width / 2;                // pass in normal offset if not double width item (half moon from moonpos.x)

                    Point max = CreatePlanetMoonRingLanding(pc, moonnode.scandata, JournalScan.GetMoonImageNotScanned(), moonpos, moonsize, moonnode.ownname, ref offsetm , false);

                    moonpos = new Point(moonpos.X, max.Y + itemsepar.Height);
                    maxtreepos = new Point(Math.Max(maxtreepos.X, max.X), Math.Max(maxtreepos.Y, max.Y));
                }
            }

            return maxtreepos;
        }

        // return right bottom of area used from curpos
        Point CreatePlanetMoonRingLanding(List<Control> pc, JournalScan sc, Image notscanned, Point curpos, Size size, string ownname, ref int offset , bool alignhorz)
        {
            Image planet;
            string tip;
            Point endpoint = curpos;
            int hoff = size.Height / 4;
            int alignhoff = (alignhorz) ? hoff : 0;

            if (sc != null)
            {
                bool indicatematerials = sc.HasMaterials && !checkBoxMaterials.Checked;

                planet = sc.GetPlanetClassImage();
                tip = sc.DisplayString(true);

                if ( sc.IsLandable || sc.HasRings || indicatematerials )
                {
                    Bitmap bmp = new Bitmap(size.Width * 2, hoff * 6);

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(planet, size.Width/2, hoff, size.Width, size.Height);

                        if ( sc.IsLandable)
                            g.DrawImage(EDDiscovery.Properties.Resources.planet_landing, new Rectangle(hoff,0, hoff*6, hoff*6) );

                        if (sc.HasRings)
                            g.DrawImage(sc.Rings.Count()>1 ? EDDiscovery.Properties.Resources.RingGap512 : EDDiscovery.Properties.Resources.Ring_Only_512, 
                                            new Rectangle(-2, hoff, size.Width * 2, size.Height));

                        if (indicatematerials)
                        {
                            Image mm = EDDiscovery.Properties.Resources.materiamoreindicator;
                            g.DrawImage(mm, new Rectangle(bmp.Width-mm.Width, bmp.Height-mm.Height, mm.Width, mm.Height));
                        }
                    }

                    endpoint = CreateImageLabel(pc, bmp, curpos, new Size(bmp.Width, bmp.Height), ownname, tip , 0);
                    offset = size.Width;                                        // return that the middle is now this
                }
                else
                {        
                    endpoint = CreateImageLabel(pc, planet, new Point(curpos.X + offset, curpos.Y + alignhoff), size, ownname, tip , alignhoff);
                    offset += size.Width / 2;
                }

                if (sc.HasMaterials && checkBoxMaterials.Checked)
                {
                    Point matpos = new Point(endpoint.X + 4, curpos.Y);
                    Point endmat = CreateMaterialNodes(pc, sc, matpos, materialsize);
                    endpoint = new Point(Math.Max(endpoint.X, endmat.X), Math.Max(endpoint.Y, endmat.Y)); // record new right point..
                }
            }
            else
            {
                planet = notscanned;
                tip = ownname + "\n\nNo scan data available";
                // again, at moonsize.w/2, to allow for a double moon size with rings
                endpoint = CreateImageLabel(pc, planet, new Point(curpos.X + offset, curpos.Y + alignhoff), planetsize, ownname, tip , alignhoff);
                offset += size.Width / 2;       // return the middle used was this..
            }

            return endpoint;
        }


        Point CreateMaterialNodes(List<Control> pc, JournalScan sn, Point matpos, Size matsize)
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

        void CreateMaterialImage(List<Control> pc, Point matpos, Size matsize, string text, string mattag, string mattip, Color matcolour , Color textcolour)
        {
            DrawnPanelNoTheme dp = new DrawnPanelNoTheme();
            dp.ImageSelected = DrawnPanel.ImageType.Text;
            dp.DrawnImage = EDDiscovery.Properties.Resources.materialmarkerorangefilled;
            dp.ImageText = text;
            dp.Location = matpos;
            dp.Size = matsize;
            dp.ForeColor = textcolour;
            dp.Tag = mattag;
            dp.Click += MaterialClick;

            System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();
            colormap.OldColor = Color.White;    // this is the marker colour to replace
            colormap.NewColor = matcolour;
            dp.SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap });

            toolTip.SetToolTip(dp, mattip);

            pc.Add(dp);
        }

        Point CreateImageLabel(List<Control> c, Image i, Point postopright, Size size, string label, string ttext , int labelhoff)
        {
            //System.Diagnostics.Debug.WriteLine(label + " " + postopright + " size " + size);

            PictureBox pb = new PictureBox();
            pb.Image = i;
            pb.Size = size;
            pb.Location = postopright;
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            //pb.BackColor = Color.DarkGray;     // use to show boundaries during debugging
            pb.Tag = ttext;
            pb.Click += ImageClick;
            c.Add(pb);

            if ( ttext != null )
                toolTip.SetToolTip(pb, ttext);

            Point max = new Point(postopright.X + size.Width, postopright.Y + size.Height);

            if (label != null)
            {
                DrawnPanel dp = new ExtendedControls.DrawnPanel();
                dp.ImageSelected = DrawnPanel.ImageType.Text;
                dp.ImageText = label;
                //dp.BackColor = Color.DarkBlue;        // use to show label areas
                int labelwidth;

                using (Graphics gr = CreateGraphics())
                {
                    SizeF sizef = gr.MeasureString(label, dp.Font);
                    labelwidth = (int)(sizef.Width + 4);
                }

                dp.Location = new Point(postopright.X + size.Width / 2 - labelwidth/2, postopright.Y + size.Height - labelnerf + labelhoff );

                if (dp.Location.X<0)
                {
                    pb.Location = new Point(pb.Location.X + -dp.Location.X, pb.Location.Y);
                    dp.Location = new Point(0, dp.Location.Y);
                }

                dp.Size = new Size(labelwidth, 20);
                dp.ForeColor = discoveryform.theme.LabelColor;
                c.Add(dp);

                max = new Point( Math.Max(dp.Location.X + labelwidth, max.X), dp.Location.Y + 20);
            }

            //System.Diagnostics.Debug.WriteLine(" ... to " + label + " " + max + " size " + (new Size(max.X-postopright.X,max.Y-postopright.Y)));
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
            itemsepar = new Size(2, 2);
            topmargin = 10;
            leftmargin = 0;
        }

        #endregion

        #region User interaction

        private void ImageClick(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            ShowInfo((string)pb.Tag, pb.Location.X < panelStars.Width / 2);
        }

        private void MaterialClick(object sender, EventArgs e)
        {
            DrawnPanelNoTheme dp = sender as DrawnPanelNoTheme;
            ShowInfo((string)dp.Tag, dp.Location.X < panelStars.Width / 2);
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

