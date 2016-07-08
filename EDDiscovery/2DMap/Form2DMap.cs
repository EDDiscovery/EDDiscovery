using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EMK.LightGeometry;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class FormSagCarinaMission : Form
    {
        public List<FGEImage> fgeimages;
        private FGEImage currentFGEImage;
        public readonly EDDiscoveryForm _eddiscoveryForm;
        
        private DateTime startDate, endDate;
        public bool Test = false;

        private DateTimePicker pickerStart, pickerStop;
        ToolStripControlHost host1, host2;
        List<Point3D> starpositions = null;

        public bool Nowindowreposition { get; set; } = false;

        public FormSagCarinaMission(EDDiscoveryForm frm)
        {
            _eddiscoveryForm = frm;
            InitializeComponent();
        }


        bool initdone = false;
        private void FormSagCarinaMission_Load(object sender, EventArgs e)
        {
            var top = SQLiteDBClass.GetSettingInt("Map2DFormTop", -1);

            if (top >= 0 && Nowindowreposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt("Map2DFormLeft", 0);
                var height = SQLiteDBClass.GetSettingInt("Map2DFormHeight", 800);
                var width = SQLiteDBClass.GetSettingInt("Map2DFormWidth", 800);
                this.Location = new Point(left, top);
                this.Size = new Size(width, height);
                //Console.WriteLine("Restore map " + this.Top + "," + this.Left + "," + this.Width + "," + this.Height);
            }

            initdone = false;
            pickerStart = new DateTimePicker();
            pickerStop = new DateTimePicker();
            host1 = new ToolStripControlHost(pickerStart);
            toolStrip1.Items.Add(host1);
            host2 = new ToolStripControlHost(pickerStop);
            toolStrip1.Items.Add(host2);
            pickerStart.Value = DateTime.Today.AddMonths(-1);

            this.pickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            this.pickerStop.ValueChanged += new System.EventHandler(this.dateTimePickerStop_ValueChanged);

            startDate = new DateTime(2010, 1, 1);
            AddImages();

            toolStripComboBox1.Items.Clear();

            foreach (FGEImage img in fgeimages)
            {
                toolStripComboBox1.Items.Add(img.FileName);
            }
            
            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBoxTime.SelectedIndex = 0;
            initdone = true;
            ShowSelectedImage();
        }

        private void FormSagCarinaMission_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Visible)
            {
                SQLiteDBClass.PutSettingInt("Map2DFormWidth", this.Width);
                SQLiteDBClass.PutSettingInt("Map2DFormHeight", this.Height);
                SQLiteDBClass.PutSettingInt("Map2DFormTop", this.Top);
                SQLiteDBClass.PutSettingInt("Map2DFormLeft", this.Left);
                //Console.WriteLine("Save map " + this.Top + "," + this.Left + "," + this.Width + "," + this.Height);
            }
            imageViewer1.Image.Dispose();
            imageViewer1.Image = null;
            fgeimages = null;
            starpositions = null;
            GC.Collect();
        }

        private void AddImages()
        {
            fgeimages = new List<FGEImage>();
            string datapath = Path.Combine(Tools.GetAppDataDirectory(), "Maps");
            if (Directory.Exists(datapath))
            {
                fgeimages = FGEImage.LoadImages(datapath);
                fgeimages.AddRange(FGEImage.LoadFixedImages(datapath));
            }
        }

        private void ShowImage(FGEImage fgeimg)
        {
            //currentImage = (Bitmap)Image.FromFile(fgeimg.Name, true);
            if (fgeimg != null && initdone)
            {
                //panel1.BackgroundImage = new Bitmap(fgeimg.FilePath);
                imageViewer1.Image = new Bitmap(fgeimg.FilePath);
                imageViewer1.ZoomToFit();
                currentFGEImage = fgeimg;

                if (toolStripButtonStars.Checked)
                    DrawStars();

                DrawTravelHistory();
            }
        }


        private void DrawTravelHistory()
        {
            if (_eddiscoveryForm.TravelControl.visitedSystems == null)
                return;

            DateTime start = startDate;

            int currentcmdr = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;

            var history = from systems in _eddiscoveryForm.TravelControl.visitedSystems where systems.Time > start && systems.Time<endDate  && systems.curSystem!=null && systems.curSystem.HasCoordinate == true  orderby systems.Time  select systems;
            List<VisitedSystemsClass> listHistory = history.ToList<VisitedSystemsClass>();
            Graphics gfx = Graphics.FromImage(imageViewer1.Image);
            
            if (listHistory.Count > 1)
            {
                Pen pen = new Pen(Color.FromArgb(listHistory[1].MapColour), 2);
                if (pen.Color.A == 0)
                    pen.Color = Color.FromArgb(255, pen.Color);
                for (int ii = 1; ii < listHistory.Count; ii++)
                {
                    if (listHistory[ii].MapColour != listHistory[ii-1].MapColour)
                    {
                        pen = new Pen(Color.FromArgb(listHistory[ii].MapColour), 2);
                        if (pen.Color.A == 0)
                            pen.Color = Color.FromArgb(255, pen.Color);
                        
                    }
                    DrawLine(gfx, pen, listHistory[ii - 1].curSystem, listHistory[ii].curSystem);
                }
            }

            Point test1  = currentFGEImage.TransformCoordinate(currentFGEImage.BottomLeft);
            Point test2 = currentFGEImage.TransformCoordinate(currentFGEImage.TopRight);


            if (Test)
            TestGrid(gfx);
        }

        private void DrawStars()
        {
            if ( starpositions == null )
                starpositions = SystemClass.GetStarPositions();

            Pen pen = new Pen(Color.White, 2);
            Graphics gfx = Graphics.FromImage(imageViewer1.Image);

            foreach (Point3D si in starpositions)
            {
                DrawPoint(gfx, pen, si.X,si.Z );
            }

            pen = new Pen(Color.White, 2);
        }

        private void DrawLine(Graphics gfx, Pen pen, ISystem sys1, ISystem sys2)
        {
            gfx.DrawLine(pen, Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.x, (int)sys1.z))), Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys2.x, (int)sys2.z))));
        }

        private void DrawPoint(Graphics gfx, Pen pen, ISystem sys1, ISystem sys2)
        {
            Point point = Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.x, (int)sys1.z)));
            gfx.FillRectangle(pen.Brush, point.X, point.Y, 1, 1);
        }

        private void DrawPoint(Graphics gfx, Pen pen, double x, double z)
        {
            Point point = Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)x, (int)z)));
            gfx.FillRectangle(pen.Brush, point.X, point.Y, 1, 1);
        }

        private void TestGrid(Graphics gfx)
        {
            Pen pointPen = new Pen(Color.LawnGreen, 3);

            for (int x = currentFGEImage.BottomLeft.X; x<= currentFGEImage.BottomRight.X; x+= 1000)
                for (int z = currentFGEImage.BottomLeft.Y; z<= currentFGEImage.TopLeft.Y; z+= 1000)
                    gfx.DrawLine(pointPen, currentFGEImage.TransformCoordinate(new Point(x,z)), currentFGEImage.TransformCoordinate(new Point(x+10, z)));
        }


        private Point Transform2Screen(Point point)
        {
            //Point np = new Point(point.X / 4, point.Y / 4);

            return point;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //DrawTravelHistory();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedImage();
        }

        private void ShowSelectedImage()
        {
            string str = toolStripComboBox1.SelectedItem.ToString();

            FGEImage img = fgeimages.FirstOrDefault(i => i.FileName == str);
            ShowImage(img);
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nr = toolStripComboBoxTime.SelectedIndex;
            /*
            Distant Worlds Expedition
            FGE Expedition start
            Last Week
            Last Month
            Last Year
            All
            */

            endDate = DateTime.Today.AddDays(1);
            if (nr == 0)
                startDate = new DateTime(2016, 1, 14);
            else if (nr == 1)
                startDate = new DateTime(2015, 8, 1);
            else if (nr == 2)
                startDate = DateTime.Now.AddDays(-7);
            else if (nr == 3)
                startDate = DateTime.Now.AddMonths(-1);
            else if (nr == 4)
                startDate = DateTime.Now.AddYears(-1);
            else if (nr == 5)
                startDate = new DateTime(2010, 8, 1);
            else if (nr == 6)  // Custom
                startDate = new DateTime(2010, 8, 1);


            if (nr == 6)
            {
                host1.Visible = true;
                host2.Visible = true;
                endDate = pickerStop.Value;
                startDate = pickerStart.Value;
            }
            else
            {
                host1.Visible = false;
                host2.Visible = false;
                endDate = DateTime.Today.AddDays(1);
            }





            ShowSelectedImage();
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            imageViewer1.ZoomIn();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            startDate = pickerStart.Value;
            ShowSelectedImage();
        }

        private void dateTimePickerStop_ValueChanged(object sender, EventArgs e)
        {
            endDate = pickerStop.Value;
            ShowSelectedImage();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        imageViewer1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        imageViewer1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        imageViewer1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
        }

        private void toolStripButtonStars_Click(object sender, EventArgs e)
        {
            ShowSelectedImage();
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            imageViewer1.ZoomOut();
        }

        private void toolStripButtonZoomtoFit_Click(object sender, EventArgs e)
        {
            imageViewer1.ZoomToFit();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
