using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.ScreenShots
{
    class ScreenShotConverter
    {
        public string ScreenshotsDir;           // textBoxScreenShotsDir
        public string inputextension;                // comboBoxScanFor
        public string textBoxOutputDir;
        public string textBoxScreenshotsDir;
        public int comboBoxSubFolder;
        public int comboBoxFileNameFormat;
        public bool checkBoxHires, checkBoxCropImage, checkBoxRemove;
        public int cropx, cropy, cropwidth, cropheight;
        public string outputextension;      // comboBoxFormat.Text
        bool copytoclipboard; //  checkBoxCopyClipboard.Checked;

        private ScreenshotDirectoryWatcher Watcher;
        public delegate void ScreenShot(string path, Point size);
        public event ScreenShot OnScreenShot;

        private string ScreenshotsDirdefault, OutputDirdefault;

        void Restore(IDiscoveryController ctrl)
        {
            ScreenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            OutputDirdefault = Path.Combine(ScreenshotsDirdefault, "Converted");
            this.Watcher = new ScreenshotDirectoryWatcher(ctrl, CallWithConverter);   // pass function to get the convert going
            this.Watcher.OnScreenshot += ConvertCompleted;
        }

        public bool StartWatcher()
        {
            return Watcher.Start(ScreenshotsDir, inputextension);
        }

        public string GetScreenshotPath(JournalScreenshot ss)
        {
            string filename = ss.Filename;
            string defaultInputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");

            if (filename.StartsWith("\\ED_Pictures\\"))
            {
                filename = filename.Substring(13);
                string filepath = Path.Combine(ScreenshotsDir ?? defaultInputDir, filename);

                if (!File.Exists(filepath))
                {
                    filepath = Path.Combine(defaultInputDir, filename);
                }

                if (File.Exists(filepath))
                {
                    filename = filepath;
                }
            }

            return filename;
        }

        private void ConvertCompleted(ScreenShotImageConverter cp) // Called by the watcher when a convert had completed.
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
                        //_discoveryForm.LogLineHighlight("Copying image to clipboard failed");
                    }
                }
            }

            OnScreenShot?.Invoke(cp.OutputFilename, cp.FinalSize);
        }

        private ScreenShotImageConverter CreateConverter() // Called on UI thread
        {
            ScreenShotImageConverter p = new ScreenShotImageConverter();
            p.OutputFolder = textBoxOutputDir;
            if (String.IsNullOrWhiteSpace(p.OutputFolder))
                p.OutputFolder = OutputDirdefault;
            p.InputFolder = textBoxScreenshotsDir;
            p.FolderFormatIndex = comboBoxSubFolder;
            p.FilenameFormatIndex = comboBoxFileNameFormat;
            p.HighRes = checkBoxHires;
            p.CropImage = checkBoxCropImage;
            p.Preview = false;
            p.RemoveInputFile = checkBoxRemove;
            p.CropArea.X = cropx;
            p.CropArea.Y = cropy;
            p.CropArea.Width = cropwidth;
            p.CropArea.Height = cropheight;
            p.OutputExtension = "." + outputextension;
            p.InputExtension = inputextension;
            p.CopyToClipboard = copytoclipboard;
            return p;
        }

        private void CallWithConverter(Action<ScreenShotImageConverter> cb)           // called by Watcher with a function to run in the main thread..
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<Action<ScreenShotImageConverter>>(CallWithConverter), cb);
            }
            else
            {
                if (checkBoxAutoConvert.Checked)
                {
                    cb(CreateConverter());                                  // call it. Function needs an image converter.  Back to processScreenshot
                }
            }
        }
    }
}
