using EDDiscovery;
using EDDiscovery.DB;
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


        public FormSagCarinaMission(EDDiscoveryForm frm)
        {
            _eddiscoveryForm = frm;
            InitializeComponent();
        }



        private void FormSagCarinaMission_Load(object sender, EventArgs e)
        {
            if (Directory.Exists("Maps"))
            {
                if (File.Exists("Maps\\SC-01.jpg"))
                {
                    FGEImage fgeimg = new FGEImage("Maps\\SC-01.jpg");

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
            }       
        }

        private void ShowImage(FGEImage fgeimg)
        {
            //currentImage = (Bitmap)Image.FromFile(fgeimg.Name, true);

            panel1.BackgroundImage = new Bitmap(fgeimg.Name);
            currentFGEImage = fgeimg;
            DrawTravelHistory();
            
        }


        private void DrawTravelHistory()
        {
            DateTime start = new DateTime(2014, 8, 1);

            var history = from systems in _eddiscoveryForm.TravelControl.visitedSystems where systems.time > start && systems.curSystem!=null && systems.curSystem.HasCoordinate == true  orderby systems.time  select systems;
            List<SystemPosition> listHistory = history.ToList<SystemPosition>();
            Graphics gfx = Graphics.FromImage(panel1.BackgroundImage);
            Pen pen = new Pen(Color.Red, 2);

            if (listHistory.Count > 1)
            {
                for (int ii = 1; ii < listHistory.Count; ii++)
                {
                    DrawLine(gfx, pen, listHistory[ii - 1].curSystem, listHistory[ii].curSystem);
                }
            }

            Point test1  = currentFGEImage.TransformCoordinate(currentFGEImage.BottomLeft);
            Point test2 = currentFGEImage.TransformCoordinate(currentFGEImage.TopRight);

            TestGrid(gfx);
        }

        private void DrawLine(Graphics gfx, Pen pen, SystemClass sys1, SystemClass sys2)
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
    }
}
