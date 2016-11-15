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

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;
        private int displaynumber = 0;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            travelhistorycontrol.OnTravelSelectionChanged += Display;
            toolTip.ShowAlways = true;

            richTextBoxInfo.Visible = false;
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
            const int starsize = 64;
            const int compoundindicatorsize = starsize/2;
            const int planetsize = starsize*3/4;
            const int itemsepar = 24;
            const int moonsize = starsize*2/4;
            const int leftmargin = starsize;
            const int topmargin = starsize / 2;

            Point curpos = new Point(leftmargin, topmargin);

            foreach (StarScan.ScanNode starnode in sn.starnodes)        // always has scan nodes
            {
                if (starnode.starisnotcompound)
                {
                    Image star = (starnode.scandata != null) ? starnode.scandata.GetStarTypeImage() : JournalScan.GetStarImageNotScanned();
                    string tip = (starnode.scandata != null) ? starnode.scandata.DisplayString(true) : ("Star " + starnode.fullname);
                    string starname = starnode.ownname.Length == 0 ? starnode.fullname : starnode.ownname;

                    CreateNode(panelStars, star, curpos.X, curpos.Y, starsize, starsize, starname, tip);
                }
                else
                    CreateNode(panelStars, EDDiscovery.Properties.Resources.ImageStarDiscWhite, curpos.X , curpos.Y , compoundindicatorsize, compoundindicatorsize, starnode.ownname, "Orbiting barycentre of " + starnode.fullname);

                curpos.X += starsize + itemsepar;

                int maxy = curpos.Y + starsize + itemsepar;

                if (starnode.children != null)
                {
                    foreach (StarScan.ScanNode planetnode in starnode.children)
                    {
                        Image planet = (planetnode.scandata != null) ? planetnode.scandata.GetPlanetClassImage() : JournalScan.GetPlanetImageNotScanned();
                        string tip = (starnode.scandata != null) ? planetnode.scandata.DisplayString(true) : ("Planet " + starnode.fullname);

                        CreateNode(panelStars, planet, curpos.X, curpos.Y, planetsize, planetsize, planetnode.ownname, tip);

                        Point moonpos = curpos;
                        moonpos.Y += planetsize + itemsepar;

                        if (planetnode.children != null)
                        {
                            foreach (StarScan.ScanNode moonnode in planetnode.children)
                            {
                                Image moon = (moonnode.scandata != null) ? moonnode.scandata.GetPlanetClassImage() : JournalScan.GetMoonImageNotScanned();
                                string mtip = (moonnode.scandata != null) ? moonnode.scandata.DisplayString(true) : ("Moon " + starnode.fullname);

                                CreateNode(panelStars , moon, moonpos.X, moonpos.Y, moonsize, moonsize, moonnode.ownname, mtip);

                                moonpos.Y += moonsize + itemsepar;

                                maxy = Math.Max(maxy, moonpos.Y);
                            }
                        }

                        curpos.X += planetsize + itemsepar;
                    }
                }

                curpos = new Point(leftmargin, maxy);
            }
        }

        void CreateNode(Control c, Image i, int xpos, int ypos, int width , int height , string label , string ttext)
        {
            System.Diagnostics.Debug.WriteLine(xpos + "," + ypos + " " + label);
            PictureBox pb = new PictureBox();
            pb.Image = i;
            pb.Width = width;
            pb.Height = height;
            pb.Location = new Point(xpos-width/2, ypos-height/2);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Tag = ttext;
            pb.Click += Pb_Click;
            c.Controls.Add(pb);
            pb.Show();
            ExtendedControls.LabelExt lb = new ExtendedControls.LabelExt();
            lb.Text = label;
            lb.Location = new Point(xpos-width , ypos + height / 2);
            lb.Font = new Font("Microsoft Sans Serif", 8);
            lb.Size = new Size(width*2, 20);
            lb.CentreX = true;
            lb.ForeColor = discoveryform.theme.LabelColor;
            lb.TextBackColor = Color.Transparent;
            c.Controls.Add(lb);
            lb.Show();
            toolTip.SetToolTip(pb, ttext);
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
        }
    }
}

