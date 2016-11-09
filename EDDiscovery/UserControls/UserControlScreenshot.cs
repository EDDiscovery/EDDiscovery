using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScreenshot : UserControlCommonBase
    {
        private int displaynumber = 0;
        private TravelHistoryControl travelhistorycontrol;

        string ImagePath = null;
        Point ImageSize;

        public UserControlScreenshot()
        {
            InitializeComponent();
            pictureBox.Visible = false;
        }

        public override void Init(TravelHistoryControl thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            travelhistorycontrol = thc;
            displaynumber = vn;
            travelhistorycontrol._discoveryForm.ImageHandler.OnScreenShot += ScreenShot;
        }

        public override void Closing()
        {
            travelhistorycontrol._discoveryForm.ImageHandler.OnScreenShot -= ScreenShot;
        }

        public void ScreenShot(string path, Point size)
        {
            ImagePath = path;
            ImageSize = size;

            FitToWindow();
        }

        void FitToWindow()
        {
            //System.Diagnostics.Debug.WriteLine("Screen shot " + ImagePath);

            double ratiopicture = (double)ImageSize.X / (double)ImageSize.Y;

            int boxwidth = ClientRectangle.Width;
            int boxheight = ClientRectangle.Height;

            int imagewidth = boxwidth;
            int imageheight = (int)((double)imagewidth / ratiopicture);

            if (imageheight > boxheight)        // if width/ratio > available height, scale down width
            {
                double scaledownwidth = (double)imageheight / (double)boxheight;
                imagewidth = (int)((double)imagewidth / scaledownwidth);
            }

            imageheight = (int)((double)imagewidth / ratiopicture);

            pictureBox.Location = new Point((boxwidth - imagewidth)/2, (boxheight - imageheight) / 2);
            pictureBox.Size = new Size(imagewidth, imageheight);

            pictureBox.ImageLocation = ImagePath;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Visible = true;
        }

        private void UserControlScreenshot_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }
    }
}
