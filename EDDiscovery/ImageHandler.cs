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
 * EDDiscovery is not affiliated with Frontier Developments plc.
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
        private bool initialized = false;
        private ScreenshotDirectoryWatcher Watcher;

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
            "MM-DD-YYYY Sysname",
            "CMDRName",
            "CMDRName Sysname",
	    "CMDRName, Sysname",
            "YYYY-MM-DD CMDRName Sysname"
            });

            this.comboBoxScanFor.Items.AddRange(new string[] { "bmp -ED Launcher", "jpg -Steam" , "png -Steam" });
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            this.Watcher = new ScreenshotDirectoryWatcher(_discoveryForm, CallWithConverter);
            this.Watcher.OnScreenshot += ConvertCompleted;

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
            if (!Directory.Exists(textBoxOutputDir.Text))
                textBoxOutputDir.Text = OutputDirdefault;

            textBoxScreenshotsDir.Text = SQLiteDBClass.GetSettingString("ImageHandlerScreenshotsDir", ScreenshotsDirdefault);
            if (!Directory.Exists(textBoxScreenshotsDir.Text))
                textBoxScreenshotsDir.Text = ScreenshotsDirdefault;

            checkBoxCopyClipboard.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerClipboard", false);
            checkBoxPreview.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerPreview", false);
            checkBoxCropImage.Checked = SQLiteDBClass.GetSettingBool("ImageHandlerCropImage", false);      // fires the checked handler which sets the readonly mode of the controls
            numericUpDownTop.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropTop", 0);
            numericUpDownLeft.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropLeft", 0);
            numericUpDownWidth.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropWidth", 0);
            numericUpDownHeight.Value = SQLiteDBClass.GetSettingInt("ImageHandlerCropHeight", 0);

            textBoxFileNameExample.Text = ImageConverter.CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);

            numericUpDownTop.Enabled = numericUpDownWidth.Enabled = numericUpDownLeft.Enabled = numericUpDownHeight.Enabled = checkBoxCropImage.Checked;

            this.initialized = true;
        }

        public bool StartWatcher()
        {
            string watchedfolder = textBoxScreenshotsDir.Text;
            string ext = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));
            return Watcher.Start(watchedfolder, ext);
        }

        public string GetScreenshotPath(JournalScreenshot ss)
        {
            return Watcher.GetScreenshotPath(ss);
        }

        private void ConvertCompleted(ImageConverter cp) // Called on UI thread
        {
            if (cp.Converted && cp.CopyToClipboard)
            {
                using (Image bmp = Bitmap.FromFile(cp.OutputFilename))
                {
                    try
                    {
                        Clipboard.SetImage(bmp);
                    }
                    catch
                    {
                        _discoveryForm.LogLineHighlight("Copying image to clipboard failed");
                    }
                }
            }

            if (cp.Preview)        // if preview, load in
            {
                pictureBox.ImageLocation = cp.OutputFilename;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            OnScreenShot?.Invoke(cp.OutputFilename, cp.FinalSize);
        }

        private ImageConverter CreateConverter() // Called on UI thread
        {
            ImageConverter p = new ImageConverter();
            p.OutputFolder = textBoxOutputDir.Text;
            p.InputFolder = textBoxScreenshotsDir.Text;
            p.FolderFormatIndex = comboBoxSubFolder.SelectedIndex;
            p.FilenameFormatIndex = comboBoxFileNameFormat.SelectedIndex;
            p.HighRes = checkBoxHires.Checked;
            p.CropImage = checkBoxCropImage.Checked;
            p.Preview = checkBoxPreview.Checked;
            p.RemoveInputFile = checkBoxRemove.Checked;
            p.CropArea.X = numericUpDownLeft.Value;
            p.CropArea.Y = numericUpDownTop.Value;
            p.CropArea.Width = numericUpDownWidth.Value;
            p.CropArea.Height = numericUpDownHeight.Value;
            p.OutputExtension = "." + comboBoxFormat.Text;
            p.InputExtension = comboBoxScanFor.Text.Substring(0, comboBoxScanFor.Text.IndexOf(" "));
            p.CopyToClipboard = checkBoxCopyClipboard.Checked;
            p.Controller = _discoveryForm;
            return p;
        }

        private void CallWithConverter(Action<ImageConverter> cb)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<Action<ImageConverter>>(CallWithConverter), cb);
            }
            else
            {
                if (checkBoxAutoConvert.Checked)
                {
                    cb(CreateConverter());
                }
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
            textBoxFileNameExample.Text = ImageConverter.CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                SQLiteDBClass.PutSettingInt("comboBoxFileNameFormat", comboBoxFileNameFormat.SelectedIndex);
            }
            textBoxFileNameExample.Text = ImageConverter.CreateFileName("Sol", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, checkBoxHires.Checked, DateTime.Now);
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
            pictureBox.Image?.Dispose();
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

    public class ScreenshotDirectoryWatcher : IDisposable
    {
        private IDiscoveryController _discoveryForm;
        private Action<Action<ImageConverter>> paramscallback;
        private FileSystemWatcher watchfolder = null;
        private string watchedfolder = null;
        private ConcurrentDictionary<string, System.Threading.Timer> ScreenshotTimers = new ConcurrentDictionary<string, System.Threading.Timer>();
        private string EDPicturesDir;
        private int LastJournalCmdr = Int32.MinValue;
        private JournalLocOrJump LastJournalLoc;

        public event Action<ImageConverter> OnScreenshot;

        public ScreenshotDirectoryWatcher(IDiscoveryController controller, Action<Action<ImageConverter>> paramscallback)
        {
            this.paramscallback = paramscallback;
            this._discoveryForm = controller;
            string ScreenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            string OutputDirdefault = Path.Combine(ScreenshotsDirdefault, "Converted");
            this.EDPicturesDir = ScreenshotsDirdefault;
            _discoveryForm.OnNewJournalEntry += NewJournalEntry;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                _discoveryForm.OnNewJournalEntry -= NewJournalEntry;
            }
        }

        public bool Start(string folder, string ext)
        {
            this.Stop();

            this.watchedfolder = folder;
            return StartWatcher(folder, ext);
        }

        public void Stop()
        {
            if (watchfolder != null)
            {
                watchfolder.Dispose();
                watchfolder = null;
            }

            this.watchedfolder = null;
        }

        private bool StartWatcher(string watchedfolder, string ext)
        {
            if (watchfolder != null)                           // if there, delete
            {
                watchfolder.EnableRaisingEvents = false;
                watchfolder = null;
            }

            if (Directory.Exists(watchedfolder))
            {
                watchfolder = new System.IO.FileSystemWatcher();
                watchfolder.Path = watchedfolder;

                watchfolder.Filter = "*." + ext;
                watchfolder.NotifyFilter = NotifyFilters.FileName;
                watchfolder.Created += watcher;
                watchfolder.EnableRaisingEvents = true;

                _discoveryForm.LogLine("Scanning for " + ext + " screenshots in " + watchedfolder);
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
                JournalScreenshot ss = je as JournalScreenshot;
                string filename = GetScreenshotPath(ss);

                if (File.Exists(filename))
                {
                    this.paramscallback?.Invoke(cp => ProcessScreenshot(filename, ss.System, ss, ss.CommanderId, cp));
                }
            }
        }

        public string GetScreenshotPath(JournalScreenshot ss)
        {
            string filename = ss.Filename;
            if (filename.StartsWith("\\ED_Pictures\\"))
            {
                filename = filename.Substring(13);
                string filepath = Path.Combine(watchedfolder ?? EDPicturesDir, filename);

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
            this.paramscallback?.Invoke(cp => ProcessFilesystemEvent(sender, e, cp));
        }

        private void ProcessFilesystemEvent(object sender, System.IO.FileSystemEventArgs e, ImageConverter cp)
        {
            int cmdrid = LastJournalCmdr;

            if (e.FullPath.ToLowerInvariant().EndsWith(".bmp"))
            {
                if (!ScreenshotTimers.ContainsKey(e.FullPath))
                {
                    System.Threading.Timer timer = new System.Threading.Timer(s => ProcessScreenshot(e.FullPath, null, null, cmdrid, cp), null, 5000, System.Threading.Timeout.Infinite);

                    // Destroy the timer if OnScreenshot was run between the above check and adding the timer to the dictionary
                    if (!ScreenshotTimers.TryAdd(e.FullPath, timer))
                    {
                        timer.Dispose();
                    }
                }
            }
            else
            {
                ProcessScreenshot(e.FullPath, null, null, cmdrid, cp);
            }
        }

        private void ProcessScreenshot(string filename, string sysname, JournalScreenshot ss, int cmdrid, ImageConverter cp)
        {

            System.Threading.Timer timer = null;

            // Don't run if OnScreenshot has already run for this image
            if ((ScreenshotTimers.TryGetValue(filename, out timer) && timer == null) || (!ScreenshotTimers.TryAdd(filename, null) && !ScreenshotTimers.TryUpdate(filename, null, timer)))
                return;

            if (timer != null)
                timer.Dispose();

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

            try
            {
                cp.InputFilename = filename;
                cp.SystemName = sysname;
                cp.JournalScreenShot = ss;
                cp.CommanderID = cmdrid;

                bool converted = cp.Convert();

                if (converted && cp.RemoveInputFile)         // if remove, delete original picture
                {
                    ScreenshotTimers.TryRemove(filename, out timer);

                    try
                    {
                        File.Delete(filename);
                    }
                    catch
                    {
                        System.Diagnostics.Trace.WriteLine($"Unable to remove file {filename}");
                    }
                }

                this.OnScreenshot?.Invoke(cp);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception watcher: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

                _discoveryForm.LogLineHighlight("Error in executing image conversion, try another screenshot, check output path settings. (Exception " + ex.Message + ")");
            }
        }
    }

    public class ImageConverter
    {
        public string OutputFolder = "";
        public string InputFolder = "";
        public int FolderFormatIndex = 0;
        public int FilenameFormatIndex = 0;
        public bool HighRes = false;
        public bool CropImage = false;
        public Rectangle CropArea = new Rectangle();
        public string InputExtension = null;
        public string OutputExtension = null;
        public bool CopyToClipboard = false;
        public bool ReConvert = true;
        public bool Preview = false;
        public bool RemoveInputFile = false;
        public string InputFilename;
        public string SystemName;
        public JournalScreenshot JournalScreenShot;
        public int CommanderID;
        public DateTime Timestamp;
        public string OutputFilename;
        public Point FinalSize;
        public bool Converted;
        public IDiscoveryController Controller;

        public bool Convert() // can call independent of watcher
        {
            OutputFilename = null;
            FinalSize = Point.Empty;
            Converted = false;

            FileInfo fi = null;

            if (JournalScreenShot != null)
            {
                CommanderID = JournalScreenShot.CommanderId;
            }

            using (Bitmap bmp = GetScreenshot(InputFilename, SystemName, CommanderID, ref JournalScreenShot, ref OutputFilename, ref FinalSize, ref fi))
            {
                CommanderID = JournalScreenShot.CommanderId;            // reload in case GetScreenshot changed it..

                Timestamp = fi.CreationTimeUtc;

                if (!GetOutputSubFolder())
                {
                    Controller.LogLineHighlight("Cannot convert " + InputExtension + " into the same folder as they are stored into" + Environment.NewLine + Environment.NewLine + "Pick a different conversion folder or a different output format");
                    return false;
                }

                if (OutputFilename == null || ReConvert)
                {
                    using (Bitmap croppedbmp = ConvertImage(bmp))
                    {
                        if (OutputExtension.Equals(".jpg"))
                        {
                            croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else if (OutputExtension.Equals(".tiff"))
                        {
                            croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Tiff);
                        }
                        else if (OutputExtension.Equals(".bmp"))
                        {
                            croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Bmp);
                        }
                        else
                        {
                            croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Png);
                        }

                        FinalSize = new Point(croppedbmp.Size);
                        Converted = true;
                    }
                }
            }

            if (Converted)
            {
                File.SetCreationTime(OutputFilename, fi.CreationTime);

                if (JournalScreenShot != null)
                {
                    JournalScreenShot.SetConvertedFilename(InputFilename, OutputFilename, FinalSize.X, FinalSize.Y);
                    JournalScreenShot.Update();
                }

                Controller.LogLine("Converted " + Path.GetFileName(InputFilename) + " to " + Path.GetFileName(OutputFilename));
            }

            return Converted;
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
                Controller.LogLineHighlight($"Unable to open screenshot '{inputfile}': {ex.Message}");
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

        private bool GetOutputSubFolder()
        {
            switch (FolderFormatIndex)
            {
                case 1:     // system name
                    OutputFolder += "\\" + Tools.SafeFileString(SystemName);
                    break;

                case 2:     // "YYYY-MM-DD"
                    OutputFolder += "\\" + Timestamp.ToString("yyyy-MM-dd");
                    break;
                case 3:     // "DD-MM-YYYY"
                    OutputFolder += "\\" + Timestamp.ToString("dd-MM-yyyy");
                    break;
                case 4:     // "MM-DD-YYYY"
                    OutputFolder += "\\" + Timestamp.ToString("MM-dd-yyyy");
                    break;

                case 5:  //"YYYY-MM-DD Sysname",
                    OutputFolder += "\\" + Timestamp.ToString("yyyy-MM-dd") + " " + Tools.SafeFileString(SystemName);
                    break;

                case 6:  //"DD-MM-YYYY Sysname",
                    OutputFolder += "\\" + Timestamp.ToString("dd-MM-yyyy") + " " + Tools.SafeFileString(SystemName);
                    break;

                case 7: //"MM-DD-YYYY Sysname"
                    OutputFolder += "\\" + Timestamp.ToString("MM-dd-yyyy") + " " + Tools.SafeFileString(SystemName);
                    break;

                case 8: // CMDR name
                    OutputFolder += "\\" + Tools.SafeFileString(CommanderID >= 0 ? EDDConfig.Instance.Commander(CommanderID).Name : "UnknownCmdr");
                    break;

                case 9: // CMDR name sysname
                    OutputFolder += "\\" + Tools.SafeFileString(CommanderID >= 0 ? EDDConfig.Instance.Commander(CommanderID).Name : "UnknownCmdr") + " at " + Tools.SafeFileString(SystemName);
                    break;

		case 10: // CMDR name, sysname
                    OutputFolder += "\\" + Tools.SafeFileString(CommanderID >= 0 ? EDDConfig.Instance.Commander(CommanderID).Name : "UnknownCmdr") + "\\" + Tools.SafeFileString(SystemName);
                    break;

                case 11: // YYYY - MM - DD CMDR name sysname
                    OutputFolder += "\\" + Timestamp.ToString("yyyy-MM-dd") + " " +
                                    Tools.SafeFileString(CommanderID >= 0 ? EDDConfig.Instance.Commander(CommanderID).Name : "UnknownCmdr") + " at " + Tools.SafeFileString(SystemName);
                    break;
            }

            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            return !(OutputFolder.Equals(InputFolder) && OutputExtension.Equals("." + InputExtension));
        }

        private Bitmap ConvertImage(Bitmap bmp)
        {
            int index = 0;
            do                                          // add _N on the filename for index>0, to make them unique.
            {
                OutputFilename = Path.Combine(OutputFolder, CreateFileName(SystemName, InputFilename, FilenameFormatIndex, HighRes, Timestamp) + (index == 0 ? "" : "_" + index) + OutputExtension);
                index++;
            } while (File.Exists(OutputFilename));          // if name exists, pick another

            System.Drawing.Bitmap croppedbmp = null;

            if (CropImage)
            {
                /* check that crop settings are within the image, otherwise adjust. */
                if ((CropArea.Width <= 0) || (CropArea.Width > bmp.Width))
                {
                    CropArea.X = 0;
                    CropArea.Width = bmp.Width;
                }
                else if (CropArea.Left + CropArea.Width > bmp.Width)
                {
                    CropArea.X = bmp.Width - CropArea.Width;
                }
                if ((CropArea.Height <= 0) || (CropArea.Height > bmp.Height))
                {
                    CropArea.Y = 0;
                    CropArea.Height = bmp.Height;
                }
                else if (CropArea.Top + CropArea.Height > bmp.Height)
                {
                    CropArea.Y = bmp.Height - CropArea.Height;
                }

                /* Only crop if we need to */
                if ((CropArea.Width != bmp.Width) || (CropArea.Height != bmp.Height))
                {                                                   // CLONE new one, which creates a new object
                    croppedbmp = bmp.Clone(CropArea, System.Drawing.Imaging.PixelFormat.DontCare);
                }
                else
                    croppedbmp = bmp;           // just copy reference.. no need to crop.
            }
            else
                croppedbmp = bmp;               // just copy reference..

            return croppedbmp;
        }

        public static string CreateFileName(string cur_sysname, string inputfile, int formatindex, bool hires, DateTime timestamp)
        {
            cur_sysname = Tools.SafeFileString(cur_sysname);

            string postfix = (hires && Path.GetFileName(inputfile).Contains("HighRes")) ? " (HighRes)" : "";

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
                    return Path.GetFileNameWithoutExtension(inputfile);
            }
        }
    }
}
