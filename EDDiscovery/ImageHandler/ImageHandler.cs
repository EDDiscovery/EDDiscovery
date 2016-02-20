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
using EDDiscovery2.DB;

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

            try
            {
                comboBoxFormat.SelectedIndex = db.GetSettingInt("ImageHandlerFormatNr", 0);
            }
            catch { }

            try
            {
                comboBoxFileNameFormat.SelectedIndex = db.GetSettingInt("comboBoxFileNameFormat", 0);
            }
            catch {}

            textBoxFileNameExample.Text = CreateFileName("Sol", "Screenshot_0000.bmp");

            checkBoxAutoConvert.Checked = db.GetSettingBool("ImageHandlerAutoconvert", false);
            checkBoxRemove.Checked = db.GetSettingBool("checkBoxRemove", false);

            textBoxOutputDir.Text = db.GetSettingString("ImageHandlerOutputDir", OutputDirdefault);
            textBoxScreenshotsDir.Text = db.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);

            checkBoxCropImage.Checked = db.GetSettingBool("ImageHandlerCropImage", false);
            groupBoxCropSettings.Enabled = checkBoxCropImage.Checked;
            numericUpDownTop.Value = db.GetSettingInt("ImageHandlerCropTop", 0);
            numericUpDownLeft.Value = db.GetSettingInt("ImageHandlerCropLeft", 0);
            numericUpDownWidth.Value = db.GetSettingInt("ImageHandlerCropWidth", 0);
            numericUpDownHeight.Value = db.GetSettingInt("ImageHandlerCropHeight", 0);
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
                string cur_sysname = "";

                string new_name = null;
                ISystem cursys = null;

                if (!checkBoxAutoConvert.Checked)
                {
                    return;
                }

                new_name = "unknown";
                cursys = _discoveryForm.TravelControl.GetCurrentSystem();

                if (cursys!=null)
                {
                    cur_sysname = cursys.name;
                }


                if (!Directory.Exists(textBoxOutputDir.Text))
                    Directory.CreateDirectory(textBoxOutputDir.Text);

                //sometimes the picture doesn't load into the picture box so waiting 1 sec in case this due to the file not being closed quick enough in ED
                System.Threading.Thread.Sleep(1000);

                new_name = CreateFileName(cur_sysname, e.FullPath);
                new_name = new_name.Replace("*", "_star");                  // fix SAG A, fix other possible file chars
                new_name = new_name.Replace("/", "_slash");
                new_name = new_name.Replace("\\", "_slash");
                new_name = new_name.Replace(":", "_colon");

                while (File.Exists(output_folder + "\\" + new_name + pic_ext))
                {
                    new_name = new_name + "x";
                }

                //var bmp = System.Drawing.Bitmap.FromFile(e.FullPath);
                var bmp = new System.Drawing.Bitmap(e.FullPath);

                /* MKW - crop image */
                if( checkBoxCropImage.Checked ) {
                    Rectangle crop = new Rectangle();
                    
                    crop.X = int.Parse(numericUpDownLeft.Text); /* Left is read-only */
                    crop.Y = int.Parse(numericUpDownTop.Text);  /* Top is read-only */
                    crop.Width = int.Parse(numericUpDownWidth.Text);
                    crop.Height = int.Parse(numericUpDownHeight.Text);

                    /* check that crop settings are within the image, otherwise adjust. */
                    if ((crop.Width <= 0) || (crop.Width > bmp.Width))
                    {
                        crop.X = 0;
                        crop.Width = bmp.Width;
                    }
                    else if (crop.Left + crop.Width > bmp.Width)
                    {
                        crop.X = bmp.Width - crop.Width;
                    }
                    if ((crop.Height <= 0) || (crop.Height > bmp.Height))
                    {
                        crop.Y = 0;
                        crop.Height = bmp.Height;
                    }
                    else if (crop.Top + crop.Height > bmp.Height)
                    {
                        crop.Y = bmp.Height - crop.Height;
                    }

                    /* Only crop if we need to */
                    if ((crop.Width != bmp.Width) || (crop.Height != bmp.Height))
                    {                        
                        bmp = bmp.Clone(crop, System.Drawing.Imaging.PixelFormat.DontCare);
                    }
                }

                string pngName = "";

                if (pic_ext.Equals(".jpg"))
                {
                    pngName = output_folder + "\\" + new_name + pic_ext;
                    bmp.Save(pngName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (pic_ext.Equals(".tiff"))
                {
                    pngName = output_folder + "\\" + new_name + pic_ext;
                    bmp.Save(pngName, System.Drawing.Imaging.ImageFormat.Tiff);
                }
                else if (pic_ext.Equals(".bmp"))
                {
                    pngName = output_folder + "\\" + new_name + pic_ext;
                    if (!textBoxOutputDir.Text.Equals(textBoxScreenshotsDir.Text))  // Dont save bmp format in screenshot dir....
                        bmp.Save(pngName, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    pngName = output_folder + "\\" + new_name + pic_ext;
                    bmp.Save(pngName, System.Drawing.Imaging.ImageFormat.Png);
                }

                bmp.Save(pngName, System.Drawing.Imaging.ImageFormat.Png);
                FileInfo fi = new FileInfo(e.FullPath);
                File.SetCreationTime(pngName, fi.CreationTime);

                bmp.Dispose();


                if (!checkBoxRemove.Checked && checkBoxPreview.Checked)
                {
                    this.pictureBox1.ImageLocation = e.FullPath;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                if (checkBoxRemove.Checked) // Remove original picture
                {
                    File.Delete(e.FullPath);
                }

                Invoke((MethodInvoker)delegate { TravelHistoryControl.LogText("Converted " + e.Name + " to " + new_name + pic_ext + Environment.NewLine ); });
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                MessageBox.Show("Exception in imageConvert:" + ex.Message);
            }
        }

        private string CreateFileName(string cur_sysname, string orignalfile)
        {
            int nr=0;

            if (Created)
            {
                Invoke((MethodInvoker)delegate
                {
                    nr = comboBoxFileNameFormat.SelectedIndex;
                });
            }

            switch (nr)
            {
                case 0:
                    return cur_sysname + " (" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ")";

                case 1:
                    {
                        string time = DateTime.Now.ToString().Replace(":", "-");
                        return cur_sysname + " (" + time + ")";
                    }

                default:
                    return Path.GetFileNameWithoutExtension(orignalfile);

            }


        }

        private string pic_ext
        {
            get
            {
                int nr=0;
                Invoke((MethodInvoker)delegate
                {
                    nr = comboBoxFormat.SelectedIndex;
                });

                switch (nr)
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

        private void checkBoxRemove_CheckedChanged(object sender, EventArgs e)
        {
            db.PutSettingBool("checkBoxRemove", checkBoxRemove.Checked);
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            db.PutSettingInt("comboBoxFileNameFormat", comboBoxFileNameFormat.SelectedIndex);

            textBoxFileNameExample.Text = CreateFileName("Sol", "Screenshot_0000.bmp");
        }

        private void textBoxScreenshotsDir_Leave(object sender, EventArgs e)
        {
            db.PutSettingString("ImageHandlerScreenshotsDir", textBoxScreenshotsDir.Text);
        }

        private void textBoxOutputDir_Leave(object sender, EventArgs e)
        {
            db.PutSettingString("ImageHandlerOutputDir", textBoxOutputDir.Text);
        }

        private void checkBoxCropImage_CheckedChanged(object sender, EventArgs e)
        {
            try {
                CheckBox cb = sender as CheckBox;
                groupBoxCropSettings.Enabled = cb.Checked;
                db.PutSettingBool("ImageHandlerCropImage", cb.Checked);
            } catch( Exception ex ) {
                System.Diagnostics.Trace.WriteLine("Exception checkBoxCropImage_CheckedChanged: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private void numericUpDownTop_Leave(object sender, EventArgs e)
        {
            try {
                NumericUpDown ud = sender as NumericUpDown;
                db.PutSettingInt("ImageHandlerCropTop", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception numericUpDownTop_Leave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private void numericUpDownLeft_Leave(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown ud = sender as NumericUpDown;
                db.PutSettingInt("ImageHandlerCropLeft", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception numericUpDownLeft_Leave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private void numericUpDownWidth_Leave(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown ud = sender as NumericUpDown;
                db.PutSettingInt("ImageHandlerCropWidth", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception numericUpDownWidth_Leave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        private void numericUpDownHeight_Leave(object sender, EventArgs e)
        {
            try
            {
                NumericUpDown ud = sender as NumericUpDown;
                db.PutSettingInt("ImageHandlerCropHeight", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception numericUpDownHeight_Leave: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }
    }
}
