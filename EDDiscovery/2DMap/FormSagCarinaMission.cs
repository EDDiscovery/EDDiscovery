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
            if (Directory.Exists("FGE"))
            {
                if (File.Exists("FGE\\SC-00.jpg"))
                {
                    FGEImage fgeimg = new FGEImage("FGE\\SC-00.jpg");

                    fgeimg.TopLeft = new Point(-3000, 6000);
                    fgeimg.pxTopLeft = new Point(329, 144);

                    fgeimg.TopRight = new Point(3000, 6000);
                    fgeimg.pxTopRight = new Point(2625, 129);

                    fgeimg.BottomLeft = new Point(-3000, 0);
                    fgeimg.pxBottomLeft = new Point(175, 2635);

                    fgeimg.BottomRight = new Point(3000, 0);
                    fgeimg.pxBottomRight = new Point(2871, 2616);

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
            DateTime start = new DateTime(2015, 8, 1);

            var history = from systems in _eddiscoveryForm.TravelControl.visitedSystems where systems.time > start && systems.curSystem!=null && systems.curSystem.HasCoordinate == true  orderby systems.time  select systems;
            List<SystemPosition> listHistory = history.ToList<SystemPosition>();

            if (listHistory.Count > 1)
            {
                for (int ii = 1; ii < listHistory.Count; ii++)
                {
                    DrawLine(listHistory[ii - 1].curSystem, listHistory[ii].curSystem);
                }
            }

            Point test1  = currentFGEImage.TransformCoordinate(currentFGEImage.BottomLeft);
            Point test2 = currentFGEImage.TransformCoordinate(currentFGEImage.TopRight);
        }

        private void DrawLine(SystemClass sys1, SystemClass sys2)
        {
            Graphics gfx = panel1.CreateGraphics();

            gfx.DrawLine(Pens.Red, Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys1.x, (int)sys1.z))), Transform2Screen(currentFGEImage.TransformCoordinate(new Point((int)sys2.x, (int)sys2.z))));
        }

        private Point Transform2Screen(Point point)
        {
            Point np = new Point(point.X / 4, point.Y / 4);

            return np;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawTravelHistory();
        }
    }
}
