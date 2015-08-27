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
        private FileSystemWatcher watchfolder = null;

        public ImageHandler()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            db = new SQLiteDBClass();
            
            string ScreenshotsDirdefault = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Frontier Developments\\Elite Dangerous";
            string OutputDirdefault = ScreenshotsDirdefault + "\\Converted";

            comboBoxFormat.SelectedIndex =  db.GetSettingInt("ImageHandlerFormatNr", 0);

            checkBoxAutoConvert.Checked = db.GetSettingBool("ImageHandlerAutoconvert", false);
            textBoxOutputDir.Text = db.GetSettingString("ImageHandlerOutputDir", OutputDirdefault);
            textBoxScreenshotsDir.Text = db.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);
            StartWatcher();
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            db.PutSettingInt("ImageHandlerFormatNr", comboBoxFormat.SelectedIndex);
        }

        private void buttonChnageEDScreenshot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = "Select ED screenshot folder";
            dlg.SelectedPath = textBoxScreenshotsDir.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxScreenshotsDir.Text = dlg.SelectedPath;
                db.PutSettingString("ImageHandlerScreenshotsDir", textBoxScreenshotsDir.Text);
                StartWatcher();
            }
        }

        private void buttonImageStore_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = "Select converted screenshot folder";
            dlg.SelectedPath = textBoxOutputDir.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputDir.Text = dlg.SelectedPath;
                db.PutSettingString("ImageHandlerOutputDir", textBoxOutputDir.Text);
            }
        }

        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
           db.PutSettingBool("ImageHandlerAutoconvert", checkBoxAutoConvert.Checked);
        }


        public void StartWatcher()
        {
            try
            {
                if (watchfolder != null)
                {
                    watchfolder.EnableRaisingEvents = false;
                    watchfolder = null;
                }

                watchfolder = new System.IO.FileSystemWatcher();
                watchfolder.Path = textBoxScreenshotsDir.Text;

                watchfolder.Filter = "*.BMP";
                watchfolder.NotifyFilter = NotifyFilters.FileName;
                watchfolder.Created += watcher;
                watchfolder.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in imageWatcher:" + ex.Message);
            }
        }

        private void watcher(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                string output_folder = textBoxOutputDir.Text;

                string new_name = null;
                new_name = "unknown";

                SystemClass cursys = _discoveryForm.TravelControl.GetCurrentSystem();

                if (cursys!=null)
                {
                    new_name = cursys.name;
                }


                if (checkBoxAutoConvert.Checked)
                {

                    if (!Directory.Exists(textBoxOutputDir.Text))
                        Directory.CreateDirectory(textBoxOutputDir.Text);

                    //sometimes the picture doesn't load into the picture box so waiting 1 sec in case this due to the file not being closed quick enough in ED 
                    System.Threading.Thread.Sleep(1000);
                    this.pictureBox1.ImageLocation = e.FullPath;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    new_name = new_name + " (" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ")";

                    //just in case we manage to take more than 1 pic in a second, add x's until the name is unique (the fix above may make this pointless)
                    while (File.Exists(output_folder + "\\" + new_name + pic_ext))
                    {
                        new_name = new_name + "x";
                    }

                    Bitmap ED_PIC = new Bitmap(e.FullPath);

                    if (pic_ext.Equals(".jpg"))
                    {
                        ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else if (pic_ext.Equals(".tiff"))
                    {
                        ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Tiff);
                    }
                    else if (pic_ext.Equals(".bmp"))
                    {
                        if (!textBoxOutputDir.Text.Equals(textBoxScreenshotsDir.Text))  // Dont save bmp format in screenshot dir....
                            ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    else
                    {
                        ED_PIC.Save(output_folder + "\\" + new_name + pic_ext, System.Drawing.Imaging.ImageFormat.Png);
                    }
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
                switch (comboBoxFormat.SelectedIndex)
                {
                    case 0:
                        return ".png";

                    case 1:
                        return ".jpg";

                    case 2:
                        return ".bmp";

                    case 3:
                        return ".tiff";

                    default:
                        return ".png";
                }
            }
        }

        private void ImageHandler_Load(object sender, EventArgs e)
        {

        }
    }
}
