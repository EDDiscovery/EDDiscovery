﻿using System;
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
        private EDDiscoveryForm _discoveryForm;
        private FileSystemWatcher watchfolder = null;

        public ImageHandler()
        {
            InitializeComponent();
            this.comboBoxFormat.Items.AddRange(new string[] { "png", "jpg", "bmp", "tiff" });
            this.comboBoxFileNameFormat.Items.AddRange(new string[] {
            "Sysname (YYYYMMDD-HHMMSS)",
            "Sysname (Windows dateformat)",
            "YYYY-MM-DD HH-MM-SS Sysname",
            "DD-MM-YYYY HH-MM-SS Sysname",
            "MM-DD-YYYY HH-MM-SS Sysname",
            "Keep original"});
            this.comboBoxScanFor.Items.AddRange(new string[] { "bmp -ED Launcher", "jpg -Steam" });
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            string ScreenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            string OutputDirdefault = Path.Combine(ScreenshotsDirdefault, "Converted");

            try
            {
                comboBoxFormat.SelectedIndex = SQLiteDBClass.GetSettingInt("ImageHandlerFormatNr", 0);
            }
            catch { }

            try
            {
                comboBoxFileNameFormat.SelectedIndex = SQLiteDBClass.GetSettingInt("comboBoxFileNameFormat", 0);
            }
            catch { }

            try
            {
                comboBoxScanFor.SelectedIndex = SQLiteDBClass.GetSettingInt("comboBoxScanFor", 0);
            }
            catch { }

            checkBoxAutoConvert.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerAutoconvert", false);
            checkBoxRemove.Checked = SQLiteDBClass.GetSettingBool("checkBoxRemove", false);
            checkBoxHires.Checked = SQLiteDBClass.GetSettingBool("checkBoxHires", false);

            textBoxOutputDir.Text = SQLiteDBClass.GetSettingString("ImageHandlerOutputDir", OutputDirdefault);
            textBoxScreenshotsDir.Text = SQLiteDBClass.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);

            checkBoxPreview.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerPreview", false);
            checkBoxCropImage.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerCropImage", false);      // fires the checked handler which sets the readonly mode of the controls
            numericUpDownTop.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropTop", 0);
            numericUpDownLeft.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropLeft", 0);
            numericUpDownWidth.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropWidth", 0);
            numericUpDownHeight.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropHeight", 0);

            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);

            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = checkBoxCropImage.Checked;
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

                watchfolder.Filter = "*." + comboBoxScanFor.Text.Substring(0,comboBoxScanFor.Text.IndexOf(" "));
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
            if (!checkBoxAutoConvert.Checked)
            {
                return;
            }

            bool checkboxremove = false;
            bool checkboxpreview = false;

            Invoke((MethodInvoker)delegate
            {
                checkboxremove = checkBoxRemove.Checked;
                checkboxpreview = checkBoxPreview.Checked;
            });

            string cur_sysname = "Unknown System";
            ISystem cursys = _discoveryForm.TravelControl.GetCurrentSystem();

            if (cursys != null)
                cur_sysname = cursys.name;

            Convert(e.FullPath,cur_sysname,checkboxremove,checkboxpreview);
        }

        // preparing for a convert stored function by hiving this out to a separate function..

        private void Convert(string inputfile, string cur_sysname, bool removeinputfile,bool previewinputfile) // can call independent of watcher
        {                                                                             
            try
            {
                string output_folder = textBoxOutputDir.Text;

                if (!Directory.Exists(textBoxOutputDir.Text))
                    Directory.CreateDirectory(textBoxOutputDir.Text);

                //sometimes the picture doesn't load into the picture box so waiting 1 sec in case this due to the file not being closed quick enough in ED
                System.Threading.Thread.Sleep(1000);

                int formatindex=0;
                bool hires=false;
                bool cropimage = false;
                Rectangle crop = new Rectangle();
                string extension = null;
                bool cannotexecute = false;
                string inputext = null;

                Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
                {                                                   // I've tested that this is required..      
                                                                    // cropping also picked up dialog items so moved here..
                                                                    // other items are also picked up here in one go.
                    formatindex = comboBoxFileNameFormat.SelectedIndex;
                    hires = checkBoxHires.Checked;
                    cropimage = checkBoxCropImage.Checked;
                    crop.X = numericUpDownLeft.Value;      
                    crop.Y = numericUpDownTop.Value;  
                    crop.Width = numericUpDownWidth.Value;
                    crop.Height = numericUpDownHeight.Value;
                    extension = "." + comboBoxFormat.Text;
                    inputext = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));
                    cannotexecute = textBoxOutputDir.Text.Equals(textBoxScreenshotsDir.Text) && comboBoxFormat.Text.Equals(inputext);
                });

                if ( cannotexecute )                                // cannot store BMPs into the Elite dangerous folder as it creates a circular condition
                {
                    MessageBox.Show("Cannot convert " + inputext + " into the same folder as they are stored into" + Environment.NewLine + Environment.NewLine + "Pick a different conversion folder or a different output format", "WARNING", MessageBoxButtons.OK);
                    return;
                }

                string store_name = null;
                int index = 0;
                do                                        // add _N on the filename for index>0, to make them unique.
                {
                    store_name = Path.Combine(output_folder, CreateFileName(cur_sysname, inputfile, formatindex, hires) + (index==0?"":"_"+index) + extension);
                    index++;
                } while (File.Exists(store_name));          // if name exists, pick another

                //var bmp = System.Drawing.Bitmap.FromFile(e.FullPath);
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(inputfile);
                System.Drawing.Bitmap croppedbmp = null;

                /* MKW - crop image */
                if (cropimage)
                {
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
                    {                                                   // CLONE new one, which creates a new object
                        croppedbmp = bmp.Clone(crop, System.Drawing.Imaging.PixelFormat.DontCare);
                    }
                    else
                        croppedbmp = bmp;           // just copy reference.. no need to crop.
                }
                else
                    croppedbmp = bmp;               // just copy reference..

                if (extension.Equals(".jpg"))
                {
                    croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (extension.Equals(".tiff"))
                {
                    croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Tiff);
                }
                else if (extension.Equals(".bmp"))
                {
                    croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else
                {
                    croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Png);
                }

                bmp.Dispose();              // need to free the bmp before any more operations on the file..
                croppedbmp.Dispose();       // and ensure this one is freed of handles to the file.

                FileInfo fi = new FileInfo(inputfile);
                File.SetCreationTime(store_name, fi.CreationTime);

                if (previewinputfile)        // if preview, load in
                {
                    pictureBox1.ImageLocation = store_name;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                if (removeinputfile)         // if remove, delete original picture
                {
                    File.Delete(inputfile);
                }

                Invoke((MethodInvoker)delegate {
                    TravelHistoryControl.LogText("Converted " + Path.GetFileName(inputfile) + " to " + 
                        Path.GetFileName(store_name) + Environment.NewLine ); });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                MessageBox.Show("Exception in imageConvert:" + ex.Message);
            }
        }
                                                            // thread safe - no picking up of dialog data.
        private string CreateFileName(string cur_sysname, string orignalfile, int formatindex, bool hires)
        {                                                       
            cur_sysname = cur_sysname.Replace("*", "_star");     // fix SAG A, fix other possible file chars
            cur_sysname = cur_sysname.Replace("/", "_slash");
            cur_sysname = cur_sysname.Replace("\\", "_slash");
            cur_sysname = cur_sysname.Replace(":", "_colon");
            cur_sysname = cur_sysname.Replace("?", "_qmark");

            string postfix = (hires && Path.GetFileName(orignalfile).Contains("HighRes")) ? " (HighRes)" : "";

            switch (formatindex)
            {
                case 0:
                    return cur_sysname + " (" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ")" + postfix;

                case 1:
                    {
                        string time = DateTime.Now.ToString();
                        time = time.Replace(":", "-");
                        time = time.Replace("/", "-");          // Rob found it was outputting 21/2/2020 on mine, so we need more replaces
                        time = time.Replace("\\", "-");
                        return cur_sysname + " (" + time + ")" + postfix;
                    }
                case 2:
                    {
                        string time = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }
                case 3:
                    {
                        string time = DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }
                case 4:
                    {
                        string time = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }

                default:
                    return Path.GetFileNameWithoutExtension(orignalfile);

            }


        }

        private void ImageHandler_Load(object sender, EventArgs e)
        {
        }

        private void checkBoxRemove_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("checkBoxRemove", checkBoxRemove.Checked);
        }

        private void checkBox_hires_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("checkBoxHires", checkBoxHires.Checked);
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt("comboBoxFileNameFormat", comboBoxFileNameFormat.SelectedIndex);
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);
        }

        private void textBoxScreenshotsDir_Leave(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString("ImageHandlerScreenshotsDir", textBoxScreenshotsDir.Text);
        }

        private void textBoxOutputDir_Leave(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString("ImageHandlerOutputDir", textBoxOutputDir.Text);
        }

        private void checkBoxCropImage_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = cb.Checked;
            SQLiteDBClass.PutSettingBool("ImageHandlerCropImage", cb.Checked);
        }

        private void numericUpDownTop_Leave(object sender, EventArgs e)
        {
            ExtendedControls.NumericUpDownCustom ud = sender as ExtendedControls.NumericUpDownCustom;
            SQLiteDBClass.PutSettingInt("ImageHandlerCropTop", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
        }

        private void numericUpDownLeft_Leave(object sender, EventArgs e)
        {
            ExtendedControls.NumericUpDownCustom ud = sender as ExtendedControls.NumericUpDownCustom;
            SQLiteDBClass.PutSettingInt("ImageHandlerCropLeft", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
        }

        private void numericUpDownWidth_Leave(object sender, EventArgs e)
        {
            ExtendedControls.NumericUpDownCustom ud = sender as ExtendedControls.NumericUpDownCustom;
            SQLiteDBClass.PutSettingInt("ImageHandlerCropWidth", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
        }

        private void numericUpDownHeight_Leave(object sender, EventArgs e)
        {
            ExtendedControls.NumericUpDownCustom ud = sender as ExtendedControls.NumericUpDownCustom;
            SQLiteDBClass.PutSettingInt("ImageHandlerCropHeight", (int)ud.Value); /* We constrain the updown from 0..1,000,000 */
        }

        private void checkBoxPreview_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("ImageHandlerPreview", checkBoxPreview.Checked);
            pictureBox1.Image = null;
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt("ImageHandlerFormatNr", comboBoxFormat.SelectedIndex);
        }

        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool("ImageHandlerAutoconvert", checkBoxAutoConvert.Checked);
        }

        private void buttonChnageEDScreenshot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = "Select ED screenshot folder";
            dlg.SelectedPath = textBoxScreenshotsDir.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxScreenshotsDir.Text = dlg.SelectedPath;
                SQLiteDBClass.PutSettingString("ImageHandlerScreenshotsDir", textBoxScreenshotsDir.Text);
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
                SQLiteDBClass.PutSettingString("ImageHandlerOutputDir", textBoxOutputDir.Text);
            }
        }

        private void comboBoxScanFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingInt("comboBoxScanFor", comboBoxScanFor.SelectedIndex);

            if ( watchfolder != null )      // if already watching, restart it
                StartWatcher();
        }
    }
}
