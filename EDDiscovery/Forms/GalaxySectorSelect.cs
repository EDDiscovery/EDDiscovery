using BaseUtils;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class GalaxySectorSelect : Form
    {
        Map2d galaxy;

        private string initialsel;
        private List<int> initiallist;

        public string Selection
        {
            get
            {
                return GridId.ToString(imageViewer.Selection);
            }

            set
            {
                imageViewer.Selection = GridId.FromString(value);
            }
        }

        public enum ActionToDo { None, Add, Remove };
        public ActionToDo Action = ActionToDo.None;

        public List<int> AllRemoveSectors;          // For remove, all sectors not wanted
        public List<int> Removed;                   // For remove, new sectors not wanted

        List<Tuple<string, string>> defaultsel = new List<Tuple<string, string>>()
        {
            new Tuple<string,string>("Custom",""),
            new Tuple<string,string>("Reset","Reset"),
            new Tuple<string,string>("All","All"),
            new Tuple<string,string>("None",""),
            new Tuple<string, string>("Bubble","608,609,610,611,612,708,709,710,711,712,808,809,810,811,812,908,909,910,911,912,1008,1009,1010,1011,1012"),
            new Tuple<string, string>("Bubble+Colonia","608,609,610,611,612,708,709,710,711,712,808,809,810,811,812,908,909,910,911,912,1008,1009,1010,1011,1012,1108,1109,1110,1207,1208,1209,1306,1307,1308,1405,1406,1407,1504,1505,1603,1604,1703"),
        };

        public GalaxySectorSelect()
        {
            InitializeComponent();

            imageViewer.MouseMove += ImageViewer_MouseMove;
            imageViewer.onChange += ChangedSel;
        }

        public bool Init(string cellset)
        {
            string datapath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Maps");
            if (Directory.Exists(datapath))
            {
                galaxy = Map2d.LoadImage(Path.Combine(datapath, "Galaxy_L_Grid.json"));
                if (galaxy != null)
                {
                    imageViewer.Image = new Bitmap(galaxy.FilePath);
                    imageViewer.ZoomToFit();
                    imageViewer.Init(galaxy);

                    comboBoxSelections.Items.AddRange((from x in defaultsel select x.Item1));

                    initialsel = Selection = cellset;
                    initiallist = new List<int>(imageViewer.Selection);     // copy of..

                    SetComboBox();

                    // later EDDTheme.Instance.ApplyToForm(this);

                    return true;
                }
            }

            return false;
        }

        void SetComboBox()
        {
            string sel = Selection;
            comboBoxSelections.Enabled = false;
            int index = defaultsel.FindIndex(x => x.Item2 == sel);
            if (index != -1)
                comboBoxSelections.SelectedIndex = index;
            else
                comboBoxSelections.SelectedIndex = 0;
            comboBoxSelections.Enabled = true;
        }

        private void comboBoxSelections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSelections.Enabled)
            {
                int selitem = comboBoxSelections.SelectedIndex;
                string sel = defaultsel[selitem].Item2;
                if (sel == "Reset")
                    Selection = initialsel;
                else if ( sel != "Custom")
                    Selection = sel;
            }
        }

        private void ChangedSel()
        {
            SetComboBox();
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            labelX.Text = (imageViewer.CurrentCellID != -1) ? imageViewer.CurrentX.ToStringInvariant() : "-";
            labelZ.Text = (imageViewer.CurrentCellID != -1) ? imageViewer.CurrentZ.ToStringInvariant() : "-";
            labelID.Text = (imageViewer.CurrentCellID != -1) ? imageViewer.CurrentCellID.ToStringInvariant() : "-";
        }

        public void SetSelection(List<int> grid)
        {
            Invalidate();
        }

        private void buttonExtSet_Click(object sender, EventArgs e)
        {
            List<int> currentsel = imageViewer.Selection;

            System.Diagnostics.Debug.WriteLine("Initial Sectors " + string.Join(",", initiallist));
            System.Diagnostics.Debug.WriteLine("Cur Sectors " + string.Join(",", currentsel));
            bool added = false;

            foreach (int i in currentsel)
            {
                if (!initiallist.Contains(i))       // if initial list does not include a current sel
                {
                    added = true;
                    break;
                }
            }

            if (added)                            // we added some..
            {
                if (ExtendedControls.MessageBoxTheme.Show(this, "You have added new sectors!" + Environment.NewLine +
                        "This will require a complete re-download of the EDSM data" + Environment.NewLine +
                        "Confirm you wish to do this?", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Action = ActionToDo.Add;
                }
                else
                    return;
            }
            else
            {
                Removed = new List<int>();

                foreach (int i in initiallist)
                {
                    if (!currentsel.Contains(i))       // if initial list does not include a current sel
                        Removed.Add(i);
                }

                if (Removed.Count>0)
                {
                    AllRemoveSectors = (from int i in GridId.AllId() where !currentsel.Contains(i) select i).ToList();

                    //if (ExtendedControls.MessageBoxTheme.Show(this, "You have removed sectors!" + Environment.NewLine +
                    //        "This will require the DB to be cleaned of entries, which will take time" + Environment.NewLine +
                    //        "Confirm you wish to do this?", "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    //{
                    //    Action = ActionToDo.Remove;
                    //}
                    //else
                    //    return;

                    Action = ActionToDo.Remove;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    public class ImageViewerWithGrid : ExtendedControls.ImageViewer
    {
        public int CurrentCellID = -1;
        public int CurrentX = -1;
        public int CurrentZ = -1;

        public Action onChange;

        private int[] xlines;
        private int[] zlines;
        private Map2d galaxy;

        private List<int> includedgridid;
        public List<int> Selection { get { return includedgridid; } set { includedgridid = value; Invalidate(); } }

        public void Init(Map2d g)
        {
            galaxy = g;
            xlines = GridId.XLines(galaxy.TopRight.X);
            xlines[0] = galaxy.TopLeft.X;
            zlines = GridId.ZLines(galaxy.TopLeft.Y);      // in numeric incr order
            zlines[0] = galaxy.BottomLeft.Y;

            ClickToZoom = false;
            includedgridid = new List<int>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (galaxy != null)
            {
                SolidBrush red = new SolidBrush(Color.Gray);
                SolidBrush sel = new SolidBrush(Color.FromArgb(128, Color.Red));
                Pen pen = new Pen(red);

                Rectangle imagesourceregion = GetSourceImageRegion();  // This is what we are displaying

                Point lypostop = galaxy.LYPos(new Point(imagesourceregion.Left, imagesourceregion.Top)); // convert the top of what displayed to lys.

                int lywidth = imagesourceregion.Width * galaxy.LYWidth / galaxy.PixelWidth;     // work out the width of the displayed in ly.
                int lyheight = imagesourceregion.Height * galaxy.LYHeight / galaxy.PixelHeight;

                Rectangle imageviewport = GetImageViewPort();       // these are the pixels on the screen in which imagesourceregion is squashed into

                int[] xpos = new int[xlines.Length];
                int[] zpos = new int[zlines.Length];

                for (int x = 0; x < xlines.Length; x++)
                {
                    int xly = xlines[x];
                    int pos = xly - lypostop.X;  // where it is relative to left of image
                    float offset = (float)pos / lywidth;        // fraction across the image
                    int px = imageviewport.Left + (int)(imageviewport.Width * offset);  // shift to image viewport pixels
                    xpos[x] = px;

                    e.Graphics.DrawLine(pen, new Point(px, imageviewport.Top), new Point(px, imageviewport.Bottom));
                }

                for (int z = 0; z < zlines.Length; z++)     // in numberic order, so bottom of map first
                {
                    int zly = zlines[z];
                    int pos = lypostop.Y - zly;  // where it is relative to top of image, remembering top is highest
                    float offset = (float)pos / lyheight;        // fraction across the image
                    int px = imageviewport.Top + (int)(imageviewport.Height * offset);  // shift to image viewport pixels
                    zpos[z] = px;

                    e.Graphics.DrawLine(pen, new Point(imageviewport.Left, px), new Point(imageviewport.Right, px));
                }

                //for (int x = -40000; x < 40000; x += 250)    System.Diagnostics.Debug.WriteLine("{0} {1}", x, GridId.Id(x, 0));

                for (int x = 0; x < xlines.Length - 1; x++)
                {
                    for (int z = 0; z < zlines.Length - 1; z++)
                    {
                        int id = GridId.IdFromComponents(x, z);

                        if (!includedgridid.Contains(id))
                        {
                            int width = xpos[x + 1] - xpos[x] - 2;
                            int height = zpos[z] - zpos[z + 1] - 2;       // from bottom of map upwards

                            //System.Diagnostics.Debug.WriteLine("{0},{1} => {2} {3} {4} {5}", x, z, xpos[x], zpos[z], width, height);
                            e.Graphics.FillRectangle(sel, xpos[x] + 1, zpos[z + 1] + 1, width, height);
                        }
                    }
                }

                sel.Dispose();
                pen.Dispose();
                red.Dispose();
            }
        }

        int CellFromMousePos(Point mouse, out int xly, out int zly)
        {
            Rectangle imageviewport = GetImageViewPort();
            int mx = mouse.X - imageviewport.X;
            int my = mouse.Y - imageviewport.Y;
            xly = zly = 0;

            if (mx >= 0 && mx < imageviewport.Width && my >= 0 && my <= imageviewport.Height)
            {
                float fractx = (float)mx / imageviewport.Width;
                float fracty = (float)my / imageviewport.Height;

                Rectangle imagesourceregion = GetSourceImageRegion();  // This is what we are displaying
                Point lypostop = galaxy.LYPos(new Point(imagesourceregion.Left, imagesourceregion.Top)); // convert the top of what displayed to lys.

                int lywidth = imagesourceregion.Width * galaxy.LYWidth / galaxy.PixelWidth;     // work out the width of the displayed in ly.
                int lyheight = imagesourceregion.Height * galaxy.LYHeight / galaxy.PixelHeight;

                xly = (int)(lypostop.X + fractx * lywidth);
                zly = (int)(lypostop.Y - fracty * lyheight);

                int id = GridId.Id(xly, zly);

               // System.Diagnostics.Debug.WriteLine("ID {0} {1} {2}" ,id, xly, zly);

                return id;
            }
            else
                return -1;
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!IsPanning && rightclickcell == -1)
            {
                int x, y;
                int id = CellFromMousePos(e.Location, out x, out y);
                if (id != -1)
                {
                    if (includedgridid.Contains(id))
                        includedgridid.Remove(id);
                    else
                    {
                        includedgridid.Add(id);
                        includedgridid.Sort();
                    }
                    Invalidate();
                    onChange?.Invoke();
                }
            }
        }

        int rightclickcell = -1;
        bool rightclickon;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.CurrentCellID = CellFromMousePos(e.Location, out CurrentX, out CurrentZ);

            if (e.Button == MouseButtons.Right)
            {
                if (CurrentCellID != -1)
                {
                    if (rightclickcell == -1)
                    {
                        rightclickon = !includedgridid.Contains(CurrentCellID);
                    }

                    if (rightclickcell != CurrentCellID)
                    {
                        if (rightclickon)
                        {
                            if (!includedgridid.Contains(CurrentCellID))
                            {
                                includedgridid.Add(CurrentCellID);
                                includedgridid.Sort();
                                Invalidate();
                                onChange?.Invoke();
                            }
                        }
                        else
                        {
                            if (includedgridid.Contains(CurrentCellID))
                            { 
                                includedgridid.Remove(CurrentCellID);
                                Invalidate();
                                onChange?.Invoke();
                            }
                        }
                    }

                    rightclickcell = CurrentCellID;
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            rightclickcell = -1;
            base.OnMouseUp(e);
        }
    }
}
