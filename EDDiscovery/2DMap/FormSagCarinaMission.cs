using EDDiscovery;
using EDDiscovery.DB;
using EDDiscovery2.DB;
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
        public List<FGEImage> fgeimages = new List<FGEImage>();
        private FGEImage currentFGEImage;
        public readonly EDDiscoveryForm _eddiscoveryForm;
        //        private Bitmap currentImage;

        private DateTime startDate, endDate;
        public bool Test = false;

        private DateTimePicker pickerStart, pickerStop;
        ToolStripControlHost host1, host2;


        public FormSagCarinaMission(EDDiscoveryForm frm)
        {
            _eddiscoveryForm = frm;




            InitializeComponent();
        }



        private void FormSagCarinaMission_Load(object sender, EventArgs e)
        {
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
            WindowState = FormWindowState.Maximized;

            toolStripComboBox1.Items.Clear();

            foreach (FGEImage img in fgeimages)
            {
                toolStripComboBox1.Items.Add(img.FileName);
            }
            toolStripComboBox1.SelectedIndex = 0;
            toolStripComboBoxTime.SelectedIndex = 0;

        }


        private void LoadImages(string datapath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(datapath);
            FileInfo[] allFiles = null;

            try
            {
                allFiles = dirInfo.GetFiles("*.json");
            }
            catch
            {
            }

            if (allFiles != null)
            {
                foreach (FileInfo fi in allFiles)
                {
                    JObject pfile = null;
                    string json = EDDiscoveryForm.LoadJsonFile(fi.FullName);


                    if (json != null)
                    {
                        FGEImage fgeimg;
                        pfile = (JObject)JObject.Parse(json);

                        if (File.Exists(fi.FullName.Replace(".json", ".png")))
                            fgeimg = new FGEImage(fi.FullName.Replace(".json", ".png"));
                        else
                            fgeimg = new FGEImage(fi.FullName.Replace(".json", ".jpg"));

                        fgeimg.TopLeft = new Point(pfile["x1"].Value<int>(), pfile["y1"].Value<int>());
                        fgeimg.pxTopLeft = new Point(pfile["px1"].Value<int>(), pfile["py1"].Value<int>());

                        fgeimg.TopRight = new Point(pfile["x2"].Value<int>(), pfile["y1"].Value<int>());
                        fgeimg.pxTopRight = new Point(pfile["px2"].Value<int>(), pfile["py1"].Value<int>());

                        fgeimg.BottomLeft = new Point(pfile["x1"].Value<int>(), pfile["y2"].Value<int>());
                        fgeimg.pxBottomLeft = new Point(pfile["px1"].Value<int>(), pfile["py2"].Value<int>());

                        fgeimg.BottomRight = new Point(pfile["x2"].Value<int>(), pfile["y2"].Value<int>());
                        fgeimg.pxBottomRight = new Point(pfile["px2"].Value<int>(), pfile["py2"].Value<int>());
                        fgeimages.Add(fgeimg);
                    }
                }
            }
        }



        private void AddImages()
        {
            LoadImages(Path.Combine(Tools.GetAppDataDirectory(), "Maps"));
            if (Directory.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps")))
            {
                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-01.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-01.jpg"));

                    fgeimg.TopLeft = new Point(-3000, 6000);
                    fgeimg.pxTopLeft = new Point(329, 144);

                    fgeimg.TopRight = new Point(3000, 6000);
                    fgeimg.pxTopRight = new Point(2625, 129);

                    fgeimg.BottomLeft = new Point(-3000, 0);
                    fgeimg.pxBottomLeft = new Point(175, 2635);

                    fgeimg.BottomRight = new Point(3000, 0);
                    fgeimg.pxBottomRight = new Point(2871, 2616);


                    fgeimg.Yaxispoints.Add(new Point(0, 2634));
                    fgeimg.Yaxispoints.Add(new Point(1000, 2158));
                    fgeimg.Yaxispoints.Add(new Point(2000, 1710));
                    fgeimg.Yaxispoints.Add(new Point(3000, 1288));
                    fgeimg.Yaxispoints.Add(new Point(4000, 887));
                    fgeimg.Yaxispoints.Add(new Point(5000, 503));
                    fgeimg.Yaxispoints.Add(new Point(6000, 144));

                    fgeimages.Add(fgeimg);

                    ShowImage(fgeimg);
                }

                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-02.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-02.jpg"));

                    fgeimg.TopLeft = new Point(-1000, 9000);
                    fgeimg.pxTopLeft = new Point(281, 169);

                    fgeimg.TopRight = new Point(5000, 9000);
                    fgeimg.pxTopRight = new Point(2688, 150);

                    fgeimg.BottomLeft = new Point(-1000, 3000);
                    fgeimg.pxBottomLeft = new Point(152, 2648);

                    fgeimg.BottomRight = new Point(5000, 3000);
                    fgeimg.pxBottomRight = new Point(2817, 2620);


                    fgeimg.Yaxispoints.Add(new Point(3000, 2643));
                    fgeimg.Yaxispoints.Add(new Point(4000, 2199));
                    fgeimg.Yaxispoints.Add(new Point(5000, 1767));
                    fgeimg.Yaxispoints.Add(new Point(6000, 1341));
                    fgeimg.Yaxispoints.Add(new Point(7000, 936));
                    fgeimg.Yaxispoints.Add(new Point(8000, 545));
                    fgeimg.Yaxispoints.Add(new Point(9000, 167));

                    fgeimages.Add(fgeimg);

                    //ShowImage(fgeimg);
                }

                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-03.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-03.jpg"));

                    fgeimg.TopLeft = new Point(3000, 8000);
                    fgeimg.pxTopLeft = new Point(319, 187);

                    fgeimg.TopRight = new Point(9000, 8000);
                    fgeimg.pxTopRight = new Point(2646, 166);

                    fgeimg.BottomLeft = new Point(3000, 2000);
                    fgeimg.pxBottomLeft = new Point(184, 2631);

                    fgeimg.BottomRight = new Point(9000, 2000);
                    fgeimg.pxBottomRight = new Point(2777, 2609);


                    fgeimg.Yaxispoints.Add(new Point(2000, 2631));
                    fgeimg.Yaxispoints.Add(new Point(3000, 2186));
                    fgeimg.Yaxispoints.Add(new Point(4000, 1757));
                    fgeimg.Yaxispoints.Add(new Point(5000, 1343));
                    fgeimg.Yaxispoints.Add(new Point(6000, 944));
                    fgeimg.Yaxispoints.Add(new Point(7000, 557));
                    fgeimg.Yaxispoints.Add(new Point(8000, 187));

                    fgeimages.Add(fgeimg);

                    //ShowImage(fgeimg);
                }

                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-04.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-04.jpg"));

                    fgeimg.TopLeft = new Point(8000, 8000);
                    fgeimg.pxTopLeft = new Point(253, 129);

                    fgeimg.TopRight = new Point(14000, 8000);
                    fgeimg.pxTopRight = new Point(2661, 112);

                    fgeimg.BottomLeft = new Point(8000, 2000);
                    fgeimg.pxBottomLeft = new Point(105, 2701);

                    fgeimg.BottomRight = new Point(14000, 2000);
                    fgeimg.pxBottomRight = new Point(2788, 2696);


                    fgeimg.Yaxispoints.Add(new Point(2000, 2701));
                    fgeimg.Yaxispoints.Add(new Point(3000, 2234));
                    fgeimg.Yaxispoints.Add(new Point(4000, 1781));
                    fgeimg.Yaxispoints.Add(new Point(5000, 1345));
                    fgeimg.Yaxispoints.Add(new Point(6000, 927));
                    fgeimg.Yaxispoints.Add(new Point(7000, 520));
                    fgeimg.Yaxispoints.Add(new Point(8000, 129));

                    fgeimages.Add(fgeimg);

                    //ShowImage(fgeimg);
                }



                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-L4.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-L4.jpg"));

                    fgeimg.TopLeft = new Point(0, 30000);
                    fgeimg.pxTopLeft = new Point(344, 106);

                    fgeimg.TopRight = new Point(30000, 30000);
                    fgeimg.pxTopRight = new Point(2511, 119);

                    fgeimg.BottomLeft = new Point(0, -5000);
                    fgeimg.pxBottomLeft = new Point(136, 2839);

                    fgeimg.BottomRight = new Point(30000, -5000);
                    fgeimg.pxBottomRight = new Point(2881, 2855);


                    fgeimg.Yaxispoints.Add(new Point(-5000, 2839));
                    fgeimg.Yaxispoints.Add(new Point(0000, 2392));
                    fgeimg.Yaxispoints.Add(new Point(5000, 1926));
                    fgeimg.Yaxispoints.Add(new Point(10000, 1523));
                    fgeimg.Yaxispoints.Add(new Point(15000, 1117));
                    fgeimg.Yaxispoints.Add(new Point(20000, 771));
                    fgeimg.Yaxispoints.Add(new Point(25000, 406));
                    fgeimg.Yaxispoints.Add(new Point(30000, 106));

                    fgeimages.Add(fgeimg);

                    ShowImage(fgeimg);
                }
                if (File.Exists(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-U4.jpg")))
                {
                    FGEImage fgeimg = new FGEImage(Path.Combine(Tools.GetAppDataDirectory(), "Maps\\SC-U4.jpg"));

                    fgeimg.TopLeft = new Point(0, 60000);
                    fgeimg.pxTopLeft = new Point(273, 445);

                    fgeimg.TopRight = new Point(30000, 60000);
                    fgeimg.pxTopRight = new Point(2496, 439);

                    fgeimg.BottomLeft = new Point(0, 30000);
                    fgeimg.pxBottomLeft = new Point(100, 2868);

                    fgeimg.BottomRight = new Point(30000, 30000);
                    fgeimg.pxBottomRight = new Point(2840, 2862);


                    fgeimg.Yaxispoints.Add(new Point(30000, 2868));
                    fgeimg.Yaxispoints.Add(new Point(35000, 2385));
                    fgeimg.Yaxispoints.Add(new Point(40000, 1944));
                    fgeimg.Yaxispoints.Add(new Point(45000, 1524));
                    fgeimg.Yaxispoints.Add(new Point(50000, 1143));
                    fgeimg.Yaxispoints.Add(new Point(55000, 773));
                    fgeimg.Yaxispoints.Add(new Point(60000, 445));

                    fgeimages.Add(fgeimg);

                    ShowImage(fgeimg);
                }

            }
        }

        private void ShowImage(FGEImage fgeimg)
        {
            //currentImage = (Bitmap)Image.FromFile(fgeimg.Name, true);
            if (fgeimg != null)
            {
                //panel1.BackgroundImage = new Bitmap(fgeimg.FilePath);
                imageViewer1.Image = new Bitmap(fgeimg.FilePath);
                imageViewer1.ZoomToFit();
                currentFGEImage = fgeimg;
                DrawTravelHistory();
            }
        }


        private void DrawTravelHistory()
        {
            DateTime start = startDate;

            

            foreach (var sys in _eddiscoveryForm.TravelControl.visitedSystems)
            {
                if (sys.curSystem == null)
                {
                    sys.curSystem = SystemData.GetSystem(sys.Name);

                }
            }

            int currentcmdr = EDDiscoveryForm.EDDConfig.CurrentCommander.Nr;

            var history = from systems in _eddiscoveryForm.TravelControl.visitedSystems where systems.time > start && systems.time<endDate  && systems.curSystem!=null && systems.curSystem.HasCoordinate == true  orderby systems.time  select systems;
            List<SystemPosition> listHistory = history.ToList<SystemPosition>();
            Graphics gfx = Graphics.FromImage(imageViewer1.Image);
            
            if (listHistory.Count > 1)
            {
                Pen pen = new Pen(Color.FromArgb(listHistory[1].vs.MapColour), 2);
                if (pen.Color.A == 0)
                    pen.Color = Color.FromArgb(255, pen.Color);
                for (int ii = 1; ii < listHistory.Count; ii++)
                {
                    if (listHistory[ii].vs.MapColour != listHistory[ii-1].vs.MapColour)
                    {
                        pen = new Pen(Color.FromArgb(listHistory[ii].vs.MapColour), 2);
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

        private void DrawLine(Graphics gfx, Pen pen, ISystem sys1, ISystem sys2)
        {
            gfx.DrawLine(pen, Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.x, (int)sys1.z))), Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys2.x, (int)sys2.z))));
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
