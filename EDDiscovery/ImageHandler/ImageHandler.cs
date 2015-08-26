using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EDDiscovery.DB;
using EDDiscovery;

namespace EDDiscovery2.ImageHandler
{
    public partial class ImageHandler : UserControl
    {
        private SQLiteDBClass db;
        private EDDiscoveryForm _discoveryForm;
        public ImageHandler()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            db = new SQLiteDBClass();
            string OutputDirdefault = "";
            string ScreenshotsDirdefault = "";

            comboBoxFormat.SelectedIndex =  db.GetSettingInt("ImageHandlerFormatNr", 0);

            checkBoxAutoConvert.Checked = db.GetSettingBool("ImageHandlerAutoconvert", false);
            textBoxOutputDir.Text = db.GetSettingString("ImageHandlerOutputDir", OutputDirdefault);
            textBoxScreenshotsDir.Text = db.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonChnageEDScreenshot_Click(object sender, EventArgs e)
        {

        }

        private void buttonImageStore_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
            

           // db.PutSettingString("CommanderName", textBoxCmdrName.Text);
        }


        private void watcher(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                string output_folder = textBoxOutputDir.Text;

                string new_name = null;
                //new_name = sys_name;

                //sometimes the picture doesn't load into the picture box so waiting 1 sec in case this due to the file not being closed quick enough in ED 
                System.Threading.Thread.Sleep(1000);
                this.pictureBox1.ImageLocation = e.FullPath;
                this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                new_name = new_name + "(" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ")";

                //just in case we manage to take more than 1 pic in a second, add x's until the name is unique (the fix above may make this pointless)
                while (File.Exists(output_folder + "\\" + new_name + pic_ext))
                {
                    new_name = new_name + "x";
                }

                Bitmap ED_PIC = new Bitmap(e.FullPath);

                if (pic_ext == ".jpg")
                {
                    ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in imageConvert:" + ex.Message); 
            }
        }

        private string pic_ext
        {
            get
            {
                return "." + comboBoxFormat.SelectedText.ToLower();
            }
        }

        private void ImageHandler_Load(object sender, EventArgs e)
        {

        }
    }
}
