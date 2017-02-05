/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Concurrent;
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
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using Newtonsoft.Json.Linq;

namespace EDDiscovery2.ImageHandler
{
    public partial class ImageHandler : UserControl
    {
        private EDDiscoveryForm _discoveryForm;
        private FileSystemWatcher watchfolder = null;
        private ConcurrentDictionary<string, System.Threading.Timer> ScreenshotTimers = new ConcurrentDictionary<string, System.Threading.Timer>();
        private bool initialized = false;
        private string EDPicturesDir;
        private int LastJournalCmdr = Int32.MinValue;
        private JournalLocOrJump LastJournalLoc;

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
            EDPicturesDir = ScreenshotsDirdefault;

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

            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);

            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = checkBoxCropImage.Checked;

            _discoveryForm.OnNewJournalEntry += NewJournalEntry;

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

        private void NewJournalEntry(JournalEntry je)
        {
            if (je is JournalLocOrJump)
            {
                LastJournalCmdr = je.CommanderId;
                LastJournalLoc = je as JournalLocOrJump;
            }
            else
            {
                if (je.CommanderId != LastJournalCmdr)
                {
                    LastJournalLoc = null;
                    LastJournalCmdr = je.CommanderId;
                }
            }

            if (je.EventTypeID == JournalTypeEnum.Screenshot)
            {
                if (!checkBoxAutoConvert.Checked)
                {
                    return;
                }

                JournalScreenshot ss = je as JournalScreenshot;
                string filename = GetScreenshotPath(ss);

                if (File.Exists(filename))
                {
                    ProcessScreenshot(filename, ss.System, ss, ss.CommanderId);
                }
            }
        }

        public string GetScreenshotPath(JournalScreenshot ss)
        {
            string filename = ss.Filename;
            if (filename.StartsWith("\\ED_Pictures\\"))
            {
                filename = filename.Substring(13);
                string ssdir = (string)Invoke(new Func<String>(() => textBoxScreenshotsDir.Text));
                string filepath = Path.Combine(textBoxScreenshotsDir.Text, filename);

                if (!File.Exists(filepath))
                {
                    filepath = Path.Combine(EDPicturesDir, filename);
                }

                if (File.Exists(filepath))
                {
                    filename = filepath;
                }
            }

            return filename;
        }

        private void watcher(object sender, System.IO.FileSystemEventArgs e)
        {
            if (!checkBoxAutoConvert.Checked)
            {
                return;
            }

            int cmdrid = LastJournalCmdr;

            if (e.FullPath.ToLowerInvariant().EndsWith(".bmp"))
            {
                if (!ScreenshotTimers.ContainsKey(e.FullPath))
                {
                    System.Threading.Timer timer = new System.Threading.Timer(s => ProcessScreenshot(e.FullPath, null, null, cmdrid), null, 5000, System.Threading.Timeout.Infinite);

                    // Destroy the timer if OnScreenshot was run between the above check and adding the timer to the dictionary
                    if (!ScreenshotTimers.TryAdd(e.FullPath, timer))
                    {
                        timer.Dispose();
                    }
                }
            }
            else
            {
                ProcessScreenshot(e.FullPath, null, null, cmdrid);
            }
        }

        private void ProcessScreenshot(string filename, string sysname, JournalScreenshot ss, int cmdrid)
        {
            System.Threading.Timer timer = null;

            // Don't run if OnScreenshot has already run for this image
            if ((ScreenshotTimers.TryGetValue(filename, out timer) && timer == null) || (!ScreenshotTimers.TryAdd(filename, null) && !ScreenshotTimers.TryUpdate(filename, null, timer)))
                return;

            if (timer != null)
                timer.Dispose();

            bool checkboxremove = false;
            bool checkboxpreview = false;

            Invoke((MethodInvoker)delegate
            {
                checkboxremove = checkBoxRemove.Checked;
                checkboxpreview = checkBoxPreview.Checked;
            });

            if (sysname == null)
            {
                if (LastJournalLoc != null)
                {
                    sysname = LastJournalLoc.StarSystem;
                }
                else if (cmdrid >= 0)
                {
                    LastJournalLoc = JournalEntry.GetLast<JournalLocOrJump>(cmdrid, DateTime.UtcNow);
                    if (LastJournalLoc != null)
                    {
                        sysname = LastJournalLoc.StarSystem;
                    }
                }
            }

            if (sysname == null)
            {
                HistoryEntry he = _discoveryForm.history.GetLastFSD;
                sysname = (he != null) ? he.System.name : "Unknown System";
            }

            Convert(filename, sysname, checkboxremove, checkboxpreview, ss, cmdrid);
        }

        // preparing for a convert stored function by hiving this out to a separate function..

        private void Convert(string inputfile, string cur_sysname, bool removeinputfile,bool previewinputfile, JournalScreenshot ss, int cmdrid) // can call independent of watcher
        {                                                                             
            try
            {
                string store_name = null;
                FileInfo fi = null;
                Point finalsize = Point.Empty;
                bool converted = false;

                if (ss != null)
                {
                    cmdrid = ss.CommanderId;
                }

                using (Bitmap bmp = GetScreenshot(inputfile, cur_sysname, cmdrid, ref ss, ref store_name, ref finalsize, ref fi))
                {
                    DateTime filetime = fi.CreationTimeUtc;

                    ConvertParams cp = GetConversionParams(cur_sysname, filetime);

                    if (cp.cannotexecute)
                    {
                        _discoveryForm.LogLineHighlight("Cannot convert " + cp.inputext + " into the same folder as they are stored into" + Environment.NewLine + Environment.NewLine + "Pick a different conversion folder or a different output format");
                        bmp.Dispose();
                        return;
                    }

                    cmdrid = ss.CommanderId;

                    if (store_name == null || cp.reconvert)
                    {
                        using (Bitmap croppedbmp = ConvertImage(cp, inputfile, bmp, cur_sysname, filetime, ss, cmdrid, ref store_name))
                        {
                            if (cp.copyclipboard)
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

                            if (cp.extension.Equals(".jpg"))
                            {
                                croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else if (cp.extension.Equals(".tiff"))
                            {
                                croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Tiff);
                            }
                            else if (cp.extension.Equals(".bmp"))
                            {
                                croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            else
                            {
                                croppedbmp.Save(store_name, System.Drawing.Imaging.ImageFormat.Png);
                            }

                            finalsize = new Point(croppedbmp.Size);
                            converted = true;
                        }
                    }
                }

                if (converted)
                {
                    File.SetCreationTime(store_name, fi.CreationTime);

                    if (previewinputfile)        // if preview, load in
                    {
                        pictureBox.ImageLocation = store_name;
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                    if (ss != null)
                    {
                        ss.SetConvertedFilename(inputfile, store_name, finalsize.X, finalsize.Y);
                        ss.Update();
                    }
                }

                if (OnScreenShot!=null)
                {
                    Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
                    {                                                   // and fire the event
                        OnScreenShot(store_name, finalsize);
                    });
                }

                if (converted)
                {
                    if (removeinputfile)         // if remove, delete original picture
                    {
                        System.Threading.Timer timer;
                        ScreenshotTimers.TryRemove(inputfile, out timer);
                        File.Delete(inputfile);
                    }

                    Invoke((MethodInvoker)delegate
                    {
                        _discoveryForm.LogLine("Converted " + Path.GetFileName(inputfile) + " to " +
                            Path.GetFileName(store_name));
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                MessageBox.Show("Error in executing image conversion, try another screenshot, check output path settings. (Exception " + ex.Message + ")");
            }
        }

        private Bitmap GetScreenshot(string inputfile, string cur_sysname, int cmdrid, ref JournalScreenshot ss, ref string store_name, ref Point finalsize, ref FileInfo fi)
        {
            FileStream testfile = null;
            MemoryStream memstrm = new MemoryStream();
            Bitmap bmp = null;

            for (int tries = 60; tries-- > 0;)          // wait 30 seconds and then try it anyway.. 32K hires shots take a while to write.
            {
                System.Threading.Thread.Sleep(500);     // every 500ms see if we can read the file, if we can, go, else wait..
                try
                {
                    //Console.WriteLine("Trying " + inputfile);
                    using (testfile = File.Open(inputfile, FileMode.Open, FileAccess.Read, FileShare.Read))        // throws if can't open
                    {
                        memstrm.SetLength(0);
                        testfile.CopyTo(memstrm);
                        memstrm.Seek(0, SeekOrigin.Begin);
                        bmp = new Bitmap(memstrm);
                    }
                    //Console.WriteLine("Worked " + inputfile);
                    break;
                }
                catch
                {
                }
            }

            try
            {
                //Console.WriteLine("Trying " + inputfile);
                using (testfile = File.Open(inputfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))        // throws if can't open
                {
                    memstrm.SetLength(0);
                    testfile.CopyTo(memstrm);
                    memstrm.Seek(0, SeekOrigin.Begin);
                    bmp = new Bitmap(memstrm);
                }
                //Console.WriteLine("Worked " + inputfile);
            }
            catch (Exception ex)
            {
                _discoveryForm.LogLineHighlight($"Unable to open screenshot '{inputfile}': {ex.Message}");
                throw;
            }

            try
            {
                fi = new FileInfo(inputfile);
                if (ss == null)
                {
                    ss = JournalScreenshot.GetScreenshot(inputfile, bmp.Size.Width, bmp.Size.Height, fi.CreationTimeUtc, cur_sysname == "Unknown System" ? null : cur_sysname, cmdrid);
                }

                if (ss != null)
                {
                    JObject jo = JObject.Parse(ss.EventDataString);
                    if (jo["EDDOutputFile"] != null && File.Exists(JSONHelper.GetStringDef(jo["EDDOutputFile"])))
                    {
                        store_name = JSONHelper.GetStringDef(jo["EDDOutputFile"]);
                        finalsize = new Point(JSONHelper.GetInt(jo["EDDOutputWidth"]), JSONHelper.GetInt(jo["EDDOutputHeight"]));
                    }
                }
            }
            catch
            {
                bmp.Dispose();
                throw;
            }

            return bmp;
        }

        private class ConvertParams
        {
            public string output_folder = "";
            public int formatindex = 0;
            public bool hires = false;
            public bool cropimage = false;
            public Rectangle crop = new Rectangle();
            public bool cannotexecute = false;
            public string inputext = null;
            public string extension = null;
            public bool copyclipboard = false;
            public bool reconvert = true;
        }

        private ConvertParams GetConversionParams(string cur_sysname, DateTime timestamp)
        {
            ConvertParams p = new ConvertParams();

            Invoke((MethodInvoker)delegate                      // pick it in a delegate as we are in another thread..
            {                                                   // I've tested that this is required..
                                                                // cropping also picked up dialog items so moved here..
                                                                // other items are also picked up here in one go.
                p.output_folder = textBoxOutputDir.Text;

                switch (comboBoxSubFolder.SelectedIndex)
                {
                    case 1:     // system name
                        p.output_folder += "\\" + Tools.SafeFileString(cur_sysname);
                        break;

                    case 2:     // "YYYY-MM-DD"
                        p.output_folder += "\\" + timestamp.ToString("yyyy-MM-dd");
                        break;
                    case 3:     // "DD-MM-YYYY"
                        p.output_folder += "\\" + timestamp.ToString("dd-MM-yyyy");
                        break;
                    case 4:     // "MM-DD-YYYY"
                        p.output_folder += "\\" + timestamp.ToString("MM-dd-yyyy");
                        break;

                    case 5:  //"YYYY-MM-DD Sysname",
                        p.output_folder += "\\" + timestamp.ToString("yyyy-MM-dd") + " " + Tools.SafeFileString(cur_sysname);
                        break;

                    case 6:  //"DD-MM-YYYY Sysname",
                        p.output_folder += "\\" + timestamp.ToString("dd-MM-yyyy") + " " + Tools.SafeFileString(cur_sysname);
                        break;

                    case 7: //"MM-DD-YYYY Sysname"
                        p.output_folder += "\\" + timestamp.ToString("MM-dd-yyyy") + " " + Tools.SafeFileString(cur_sysname);
                        break;
                }

                if (!Directory.Exists(p.output_folder))
                    Directory.CreateDirectory(p.output_folder);

                p.formatindex = comboBoxFileNameFormat.SelectedIndex;
                p.hires = checkBoxHires.Checked;
                p.cropimage = checkBoxCropImage.Checked;
                p.crop.X = numericUpDownLeft.Value;
                p.crop.Y = numericUpDownTop.Value;
                p.crop.Width = numericUpDownWidth.Value;
                p.crop.Height = numericUpDownHeight.Value;
                p.extension = "." + comboBoxFormat.Text;
                p.inputext = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));
                p.copyclipboard = checkBoxCopyClipboard.Checked;

                p.cannotexecute = p.output_folder.Equals(textBoxScreenshotsDir.Text) && comboBoxFormat.Text.Equals(p.inputext);
            });

            return p;
        }

        private Bitmap ConvertImage(ConvertParams cp, string inputfile, Bitmap bmp, string cur_sysname, DateTime timestamp, JournalScreenshot ss, int cmdrid, ref string store_name)
        {
            int index = 0;
            do                                          // add _N on the filename for index>0, to make them unique.
            {
                store_name = Path.Combine(cp.output_folder, CreateFileName(cur_sysname, inputfile, cp.formatindex, cp.hires, timestamp) + (index == 0 ? "" : "_" + index) + cp.extension);
                index++;
            } while (File.Exists(store_name));          // if name exists, pick another

            System.Drawing.Bitmap croppedbmp = null;

            if (cp.cropimage)
            {
                /* check that crop settings are within the image, otherwise adjust. */
                if ((cp.crop.Width <= 0) || (cp.crop.Width > bmp.Width))
                {
                    cp.crop.X = 0;
                    cp.crop.Width = bmp.Width;
                }
                else if (cp.crop.Left + cp.crop.Width > bmp.Width)
                {
                    cp.crop.X = bmp.Width - cp.crop.Width;
                }
                if ((cp.crop.Height <= 0) || (cp.crop.Height > bmp.Height))
                {
                    cp.crop.Y = 0;
                    cp.crop.Height = bmp.Height;
                }
                else if (cp.crop.Top + cp.crop.Height > bmp.Height)
                {
                    cp.crop.Y = bmp.Height - cp.crop.Height;
                }

                /* Only crop if we need to */
                if ((cp.crop.Width != bmp.Width) || (cp.crop.Height != bmp.Height))
                {                                                   // CLONE new one, which creates a new object
                    croppedbmp = bmp.Clone(cp.crop, System.Drawing.Imaging.PixelFormat.DontCare);
                }
                else
                    croppedbmp = bmp;           // just copy reference.. no need to crop.
            }
            else
                croppedbmp = bmp;               // just copy reference..

            return croppedbmp;
        }

        private string CreateFileName(string cur_sysname, string orignalfile, int formatindex, bool hires, DateTime timestamp)
        {
            cur_sysname = Tools.SafeFileString(cur_sysname);

            string postfix = (hires && Path.GetFileName(orignalfile).Contains("HighRes")) ? " (HighRes)" : "";

            switch (formatindex)
            {
                case 0:
                    return cur_sysname + " (" + timestamp.ToString("yyyyMMdd-HHmmss") + ")" + postfix;

                case 1:
                    {
                        string time = timestamp.ToString();
                        time = time.Replace(":", "-");
                        time = time.Replace("/", "-");          // Rob found it was outputting 21/2/2020 on mine, so we need more replaces
                        time = time.Replace("\\", "-");
                        return cur_sysname + " (" + time + ")" + postfix;
                    }
                case 2:
                    {
                        string time = timestamp.ToString("yyyy-MM-dd HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }
                case 3:
                    {
                        string time = timestamp.ToString("dd-MM-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }
                case 4:
                    {
                        string time = timestamp.ToString("MM-dd-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }

                case 5:
                    {
                        string time = timestamp.ToString("HH-mm-ss");
                        return time + " " + cur_sysname + postfix;
                    }

                case 6:
                    {
                        string time = timestamp.ToString("HH-mm-ss");
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
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingInt("comboBoxFileNameFormat", comboBoxFileNameFormat.SelectedIndex);
            }
            textBoxFileNameExample.Text = CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);
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
