/*
 * Copyright © 2017 EDDiscovery development team
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
using BaseUtils;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class GalaxySectorSelect : ExtendedControls.DraggableForm
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

        private static List<Tuple<string, string>> DefaultGalaxyOptions = new List<Tuple<string, string>>()
        {
            new Tuple<string,string>("Custom".T(EDTx.GalaxySectorSelect_Custom),""),
            new Tuple<string,string>("Reset".T(EDTx.GalaxySectorSelect_Reset),"Reset"),
            new Tuple<string,string>("All".T(EDTx.GalaxySectorSelect_All),"All"),
            new Tuple<string,string>("None".T(EDTx.GalaxySectorSelect_None),""),
            new Tuple<string, string>("Bubble".T(EDTx.GalaxySectorSelect_Bubble),"810"),
            new Tuple<string, string>("Extended Bubble".T(EDTx.GalaxySectorSelect_ExtendedBubble),"608,609,610,611,612,708,709,710,711,712,808,809,810,811,812,908,909,910,911,912,1008,1009,1010,1011,1012"),
            new Tuple<string, string>("Bubble+Colonia".T(EDTx.GalaxySectorSelect_BC),"608,609,610,611,612,708,709,710,711,712,808,809,810,811,812,908,909,910,911,912,1008,1009,1010,1011,1012,1108,1109,1110,1207,1208,1209,1306,1307,1308,1405,1406,1407,1504,1505,1603,1604,1703"),
        };

        public GalaxySectorSelect()
        {
            InitializeComponent();

            imageViewer.MouseMove += ImageViewer_MouseMove;
            imageViewer.onChange += ChangedSel;
        }

        public bool Init(string cellset)
        {
            string datapath = EDDOptions.Instance.MapsAppDirectory();
            if (Directory.Exists(datapath))
            {
                galaxy = Map2d.LoadImage(Path.Combine(datapath, "Galaxy_L_Grid.json"));
                if (galaxy != null)
                {
                    imageViewer.Image = new Bitmap(galaxy.FilePath);
                    imageViewer.ZoomToFit();
                    imageViewer.Init(galaxy);
                    imageViewer.MinZoom = 1;

                    comboBoxSelections.Items.AddRange((from x in DefaultGalaxyOptions select x.Item1));

                    initialsel = Selection = cellset;
                    initiallist = new List<int>(imageViewer.Selection);     // copy of..

                    EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
                    bool winborder = theme.ApplyDialog(this);
                    statusStripCustom.Visible = panel_close.Visible = panel_minimize.Visible = !winborder;

                    BaseUtils.Translator.Instance.Translate(this, new Control[] { labelX, labelXName, labelZ, labelZName, labelID });

                    SetComboBox();

                    imageViewer.BackColor = Color.FromArgb(5, 5, 5);

                    return true;
                }
            }

            return false;
        }


        private void GalaxySectorSelect_Resize(object sender, EventArgs e)
        {
            imageViewer.ZoomToFit();
        }

        void SetComboBox()
        {
            string sel = Selection;
            comboBoxSelections.Enabled = false;
            int index = DefaultGalaxyOptions.FindIndex(x => x.Item2 == sel);
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
                string sel = DefaultGalaxyOptions[selitem].Item2;
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
            bool added = currentsel.Except(initiallist).Any();  // if initial list does not include a current sel

            if (added)                            // we added some..
            {
                if (ExtendedControls.MessageBoxTheme.Show(this, 
                    ("You have added new sectors!" + Environment.NewLine + "This will require a complete re-download of the EDSM data" + Environment.NewLine + "Confirm you wish to do this?").T(EDTx.GalaxySectorSelect_RD), 
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (!EDDConfig.Instance.EDSMEDDBDownload)
                        ExtendedControls.MessageBoxTheme.Show(this, ("Synchronisation to star data disabled in settings." + Environment.NewLine + "Reenable to allow star data to be updated").T(EDTx.GalaxySectorSelect_NoSync), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Action = ActionToDo.Add;
                }
                else
                    return;
            }
            else
            {
                Removed = initiallist.Except(currentsel).ToList();  // if initial list does not include a current sel

                if (Removed.Any())
                {
                    AllRemoveSectors = (from int i in GridId.AllId() where !currentsel.Contains(i) select i).ToList();

                    if (ExtendedControls.MessageBoxTheme.Show(this, 
                        ("You have removed sectors!" + Environment.NewLine + "This will require the DB to be cleaned of entries, which will take time" + Environment.NewLine + "Confirm you wish to do this?").T(EDTx.GalaxySectorSelect_RS), 
                        "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        Action = ActionToDo.Remove;
                    }
                    else
                        return;

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

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panelTop_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        // Present a menu to ask how much data to download..

        public static Tuple<string,string> SelectGalaxyMenu(Form parent)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            var list = DefaultGalaxyOptions.Where(x => x.Item1 != "Custom" && x.Item1 != "Reset").Select(x => x.Item1).ToList();

            int width = 500;
            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "ED Discovery downloads star data from EDSM/EDDB which is used to give you additional data.  Select how much data you want to store.  The more of the galaxy you select, the bigger the storage needed".T(EDTx.GalaxySectorSelect_GALSELEX), 
                            new Point(10, 30), new Size(width-50, 70), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Select:".T(EDTx.GalaxySectorSelect_Select), new Point(10, 100), new Size(130, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Entry", "All",
                        new Point(140, 100), new Size(width-140-100, 24),
                        "Select the data set".T(EDTx.GalaxySectorSelect_GALSELEN), list));

            f.AddOK(new Point(width - 40 - 80, 150), "Press to Accept".T(EDTx.GalaxySectorSelect_PresstoAccept));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                f.ReturnResult(DialogResult.OK);
            };
            
            DialogResult res = f.ShowDialogCentred(parent, parent.Icon, "Select EDSM Galaxy Data".T(EDTx.GalaxySectorSelect_GALSELTitle));

            string sel = f.Get("Entry");
            int index = DefaultGalaxyOptions.FindIndex((x) => { return x.Item1 == sel; });
            return DefaultGalaxyOptions[ index ];

        }

    }

    /// 
    /// Image view grid
    /// 

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

                int id = GridId.Id((double)xly, (double)zly);

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

                    //System.Diagnostics.Debug.WriteLine("Grid list now " + string.Join(",",includedgridid));
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
