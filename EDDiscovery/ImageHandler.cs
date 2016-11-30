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
        private EDDiscoveryForm _discoveryForm;
        private FileSystemWatcher watchfolder = null;
        private bool initialized = false;

        public delegate void ScreenShot(string path, Point size);
        public event ScreenShot OnScreenShot;

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
            "HH-MM-SS Sysname",
            "HH-MM-SS",
            "Sysname",
            "Keep original"});
            this.comboBoxSubFolder.Items.AddRange(new string[] {
            "None",
            "System Name",
            "YYYY-MM-DD",
            "DD-MM-YYYY",
            "MM-DD-YYYY",
            "YYYY-MM-DD Sysname",
            "DD-MM-YYYY Sysname",
            "MM-DD-YYYY Sysname"
            });

            this.comboBoxScanFor.Items.AddRange(new string[] { "bmp -ED Launcher", "jpg -Steam" , "png -Steam" });
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
                comboBoxSubFolder.SelectedIndex = SQLiteDBClass.GetSettingInt("comboBoxSubFolder", 0);
            }
            catch { }

            try
            {
                comboBoxScanFor.Enabled = false;    // to prevent the select change from actually doing any work here
                comboBoxScanFor.SelectedIndex = SQLiteDBClass.GetSettingInt("comboBoxScanFor", 0);
                comboBoxScanFor.Enabled = true;
            }
            catch { }

            checkBoxAutoConvert.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerAutoconvert", false);
            checkBoxRemove.Checked = SQLiteDBClass.GetSettingBool("checkBoxRemove", false);
            checkBoxHires.Checked = SQLiteDBClass.GetSettingBool("checkBoxHires", false);

            textBoxOutputDir.Text = SQLiteDBClass.GetSettingString("ImageHandlerOutputDir", OutputDirdefault);
            textBoxScreenshotsDir.Text = SQLiteDBClass.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);

            checkBoxCopyClipboard.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerClipboard", false);
            checkBoxPreview.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerPreview", false);
            checkBoxCropImage.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerCropImage", false);      // fires the checked handler which sets the readonly mode of the controls
            numericUpDownTop.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropTop", 0);
            numericUpDownLeft.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropLeft", 0);
            numericUpDownWidth.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropWidth", 0);
            numericUpDownHeight.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropHeight", 0);

            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);

            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = checkBoxCropImage.Checked;

            this.initialized = true;
        }

        public bool StartWatcher()
        {
            if (watchfolder != null )                           // if there, delete
            {
                watchfolder.EnableRaisingEvents = false;
                watchfolder = null;
            }

            string watchedfolder = textBoxScreenshotsDir.Text;

            if (Directory.Exists(watchedfolder))
            {
                watchfolder = new System.IO.FileSystemWatcher();
                watchfolder.Path = watchedfolder;

                string ext = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));

                watchfolder.Filter = "*." + ext;
                watchfolder.NotifyFilter = NotifyFilters.FileName;
                watchfolder.Created += watcher;
                watchfolder.EnableRaisingEvents = true;

                _discoveryForm.LogLine("Scanning for " + ext + " screenshots in " + watchedfolder );
                return true;
            }
            else
                _discoveryForm.LogLineHighlight("Folder specified for image conversion does not exist, check settings in the Screenshots tab");

            return false;
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

            HistoryEntry he = _discoveryForm.history.GetLastFSD;
            string cur_sysname = ( he != null ) ? he.System.name : "Unknown System";

            Convert(e.FullPath,cur_sysname,checkboxremove,checkboxpreview);
        }

        // preparing for a convert stored function by hiving this out to a separate function..

        private void Convert(string inputfile, string cur_sysname, bool removeinputfile,bool previewinputfile) // can call independent of watcher
        {                                                                             
            try
            {
                string output_folder= "";
                int formatindex=0;
                bool hires=false;
                bool cropimage = false;
                Rectangle crop = new Rectangle();
                string extension = null;
                bool cannotexecute = false;
                string inputext = null;
                bool copyclipboard = false;

                Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
                {                                                   // I've tested that this is required..      
                                                                    // cropping also picked up dialog items so moved here..
                                                                    // other items are also picked up here in one go.
                    output_folder = textBoxOutputDir.Text;

                    switch( comboBoxSubFolder.SelectedIndex )
                    {
                        case 1:     // system name
                            output_folder += "\\" + Tools.SafeFileString(cur_sysname);
                            break;

                        case 2:     // "YYYY-MM-DD"
                            output_folder += "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                            break;
                        case 3:     // "DD-MM-YYYY"
                            output_folder += "\\" + DateTime.Now.ToString("dd-MM-yyyy");
                            break;
                        case 4:     // "MM-DD-YYYY"
                            output_folder += "\\" + DateTime.Now.ToString("MM-dd-yyyy");
                            break;

                        case 5:  //"YYYY-MM-DD Sysname",
                            output_folder += "\\" + DateTime.Now.ToString("yyyy-MM-dd") + " " + Tools.SafeFileString(cur_sysname);
                            break;

                        case 6:  //"DD-MM-YYYY Sysname",
                            output_folder += "\\" + DateTime.Now.ToString("dd-MM-yyyy") + " " + Tools.SafeFileString(cur_sysname);
                            break;

                        case 7: //"MM-DD-YYYY Sysname"
                            output_folder += "\\" + DateTime.Now.ToString("MM-dd-yyyy") + " " + Tools.SafeFileString(cur_sysname);
                            break;
                    }

                    if (!Directory.Exists(output_folder))
                        Directory.CreateDirectory(output_folder);

                    formatindex = comboBoxFileNameFormat.SelectedIndex;
                    hires = checkBoxHires.Checked;
                    cropimage = checkBoxCropImage.Checked;
                    crop.X = numericUpDownLeft.Value;      
                    crop.Y = numericUpDownTop.Value;  
                    crop.Width = numericUpDownWidth.Value;
                    crop.Height = numericUpDownHeight.Value;
                    extension = "." + comboBoxFormat.Text;
                    inputext = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));
                    copyclipboard = checkBoxCopyClipboard.Checked;

                    cannotexecute = output_folder.Equals(textBoxScreenshotsDir.Text) && comboBoxFormat.Text.Equals(inputext);
                });

                if ( cannotexecute )                                // cannot store BMPs into the Elite dangerous folder as it creates a circular condition
                {
                    MessageBox.Show("Cannot convert " + inputext + " into the same folder as they are stored into" + Environment.NewLine + Environment.NewLine + "Pick a different conversion folder or a different output format", "WARNING", MessageBoxButtons.OK);
                    return;
                }

                string store_name = null;
                int index = 0;
                do                                          // add _N on the filename for index>0, to make them unique.
                {
                    store_name = Path.Combine(output_folder, CreateFileName(cur_sysname, inputfile, formatindex, hires) + (index==0?"":"_"+index) + extension);
                    index++;
                } while (File.Exists(store_name));          // if name exists, pick another

                FileStream testfile = null;

                for (int tries = 60; tries-- > 0;)          // wait 30 seconds and then try it anyway.. 32K hires shots take a while to write.
                {
                    System.Threading.Thread.Sleep(500);     // every 500ms see if we can read the file, if we can, go, else wait..
                    try
                    {
                        //Console.WriteLine("Trying " + inputfile);
                        testfile = File.Open(inputfile, FileMode.Open, FileAccess.Read, FileShare.None);        // throws if can't open
                        //Console.WriteLine("Worked " + inputfile);
                        testfile.Close();
                        break;
                    }
                    catch
                    { }
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(inputfile);
                System.Drawing.Bitmap croppedbmp = null;

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

                if (copyclipboard)
                {
                    Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
                    {                                                   // I've tested that this is required..    
                        try
                        {
                            Clipboard.SetImage(croppedbmp);
                        }
                        catch
                        {
                            _discoveryForm.LogLineHighlight("Copying image to clipboard failed");
                        }
                    });
                }

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

                Point finalsize = new Point(croppedbmp.Size);

                bmp.Dispose();              // need to free the bmp before any more operations on the file..
                croppedbmp.Dispose();       // and ensure this one is freed of handles to the file.

                FileInfo fi = new FileInfo(inputfile);
                File.SetCreationTime(store_name, fi.CreationTime);

                if (previewinputfile)        // if preview, load in
                {
                    pictureBox.ImageLocation = store_name;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                if (OnScreenShot!=null)
                {
                    Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
                    {                                                   // and fire the event
                        OnScreenShot(store_name, finalsize);
                    });
                }

                if (removeinputfile)         // if remove, delete original picture
                {
                    File.Delete(inputfile);
                }

                Invoke((MethodInvoker)delegate {
                    _discoveryForm.LogLine("Converted " + Path.GetFileName(inputfile) + " to " + 
                        Path.GetFileName(store_name) ); });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                MessageBox.Show("Error in executing image conversion, try another screenshot, check output path settings. (Exception " + ex.Message + ")");
            }
        }

        private string CreateFileName(string cur_sysname, string orignalfile, int formatindex, bool hires)
        {
            cur_sysname = Tools.SafeFileString(cur_sysname);

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

                case 5:
                    {
                        string time = DateTime.Now.ToString("HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }

                case 6:
                    {
                        string time = DateTime.Now.ToString("HH-mm-ss");
                        return time + postfix;
                    }

                case 7:
                    {
                        return cur_sysname + postfix;
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
            if (initialized)
            {
                SQLiteDBClass.PutSettingBool("checkBoxRemove", checkBoxRemove.Checked);
            }
        }

        private void checkBox_hires_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingBool("checkBoxHires", checkBoxHires.Checked);
            }
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingInt("comboBoxFileNameFormat", comboBoxFileNameFormat.SelectedIndex);
            }
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked);
        }

        private void checkBoxCropImage_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = cb.Checked;
            if (initialized)
            {
                SQLiteDBClass.PutSettingBool("ImageHandlerCropImage", cb.Checked);
            }
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
            if (initialized)
            {
                SQLiteDBClass.PutSettingBool("ImageHandlerPreview", checkBoxPreview.Checked);
            }
            pictureBox.Image = null;
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingInt("ImageHandlerFormatNr", comboBoxFormat.SelectedIndex);
            }
        }

        private void checkBoxAutoConvert_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingBool("ImageHandlerAutoconvert", checkBoxAutoConvert.Checked);
            }
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

        private void textBoxScreenshotsDir_Leave(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString("ImageHandlerScreenshotsDir", textBoxScreenshotsDir.Text);

            if (!StartWatcher())
            {
                MessageBox.Show("Folder specified does not exist, image conversion is now off");
            }
        }

        private void textBoxScreenshotsDir_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxScreenshotsDir_Leave(sender, e);
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

        private void textBoxOutputDir_Leave(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingString("ImageHandlerOutputDir", textBoxOutputDir.Text);
        }

        private void textBoxOutputDir_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxOutputDir_Leave(sender, e);
            }
        }

        private void comboBoxScanFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.initialized && comboBoxScanFor.Enabled)            // BUG: stop StarWatcher starting too soon
            {
                SQLiteDBClass.PutSettingInt("comboBoxScanFor", comboBoxScanFor.SelectedIndex);
                StartWatcher();
            }
        }

        private void checkBoxCopyClipboard_CheckedChanged(object sender, EventArgs e)
        {
            if (this.initialized)
            {
                SQLiteDBClass.PutSettingBool("ImageHandlerClipboard", checkBoxCopyClipboard.Checked);
            }
        }

        private void comboBoxSubFolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.initialized)
            {
                SQLiteDBClass.PutSettingInt("comboBoxSubFolder", comboBoxSubFolder.SelectedIndex);
            }
        }
    }
}
