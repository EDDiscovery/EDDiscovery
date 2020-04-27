using EliteDangerousCore.DB;
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
    public class ScreenShotConverter
    {
        public string ScreenshotsDir;
        public string OutputDir;
        public bool AutoConvert; 
        public bool RemoveOriginal;
        public bool MarkHiRes;
        public bool CopyToClipboard; 
        public enum CropResizeOptions { Off, Crop, Resize };
        public bool KeepMasterConvertedImage;
        public CropResizeOptions CropResize1, CropResize2;
        public Rectangle CropResizeArea1 = new Rectangle();
        public Rectangle CropResizeArea2 = new Rectangle();
        public ScreenShotImageConverter.InputTypes InputFileExtension { get; set; } = ScreenShotImageConverter.InputTypes.bmp;
        public ScreenShotImageConverter.OutputTypes OutputFileExtension { get; set; } = ScreenShotImageConverter.OutputTypes.png;

        public int FileNameFormat;
        public int FolderNameFormat;

        private EDDiscoveryForm discoveryform;
        private ScreenshotDirectoryWatcher Watcher;

        public delegate void ScreenShot(string path, Point size);
        public event ScreenShot OnScreenShot;

        public ScreenShotConverter(EDDiscoveryForm frm)
        {
            discoveryform = frm;

            string screenshotsDirdefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
            string outputDirdefault = Path.Combine(screenshotsDirdefault, "Converted");

            ScreenshotsDir = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ImageHandlerScreenshotsDir", screenshotsDirdefault );
            if (!Directory.Exists(ScreenshotsDir))
                ScreenshotsDir = screenshotsDirdefault;

            OutputDir = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ImageHandlerOutputDir", outputDirdefault);
            if (!Directory.Exists(OutputDir))
                OutputDir = outputDirdefault;

            AutoConvert = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ImageHandlerAutoconvert", false);
            RemoveOriginal = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("checkBoxRemove", false);
            MarkHiRes = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("checkBoxHires", false);
            CopyToClipboard = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ImageHandlerClipboard", false);

            try
            {       // just in case
                if (EliteDangerousCore.DB.UserDatabase.Instance.KeyExists("ImageHandlerCropImage"))
                {
                    CropResize1 = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ImageHandlerCropImage", false) ? CropResizeOptions.Crop : CropResizeOptions.Off;
                    EliteDangerousCore.DB.UserDatabase.Instance.DeleteKey("ImageHandlerCropImage");
                }
                else
                    CropResize1 = (CropResizeOptions)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropResizeImage1", 0);

                CropResize2 = (CropResizeOptions)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropResizeImage2", 0);
                CropResizeArea1 = new Rectangle(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropLeft", 0), EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropTop", 0),
                                        EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropWidth", 1920), EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropHeight", 1024));
                CropResizeArea2 = new Rectangle(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropLeft2", 0), EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropTop2", 0),
                                        EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropWidth2", 1920), EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerCropHeight2", 1024));

                InputFileExtension = (ScreenShotImageConverter.InputTypes)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("comboBoxScanFor", 0);
                OutputFileExtension = (ScreenShotImageConverter.OutputTypes)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ImageHandlerFormatNr", 0);

                KeepMasterConvertedImage = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ImageHandlerKeepFullSized", false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
            }

            FileNameFormat = Math.Min(Math.Max(0, EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("comboBoxFileNameFormat", 0)), ScreenShotImageConverter.FileNameFormats.Length - 1);
            FolderNameFormat = Math.Min(Math.Max(0, EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("comboBoxSubFolder", 0)), ScreenShotImageConverter.SubFolderSelections.Length - 1);
        }

        public void SaveSettings()
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ImageHandlerOutputDir", OutputDir);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ImageHandlerScreenshotsDir", ScreenshotsDir);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ImageHandlerAutoconvert", AutoConvert );      // names are all over the place.. historical
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("checkBoxRemove", RemoveOriginal );
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("checkBoxHires", MarkHiRes );
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ImageHandlerClipboard", CopyToClipboard);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ImageHandlerKeepFullSized", KeepMasterConvertedImage);

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropResizeImage1", (int)CropResize1);      // fires the checked handler which sets the readonly mode of the controls
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropTop", CropResizeArea1.Top);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropLeft", CropResizeArea1.Left);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropWidth", CropResizeArea1.Width);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropHeight", CropResizeArea1.Height);

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropResizeImage2", (int)CropResize2);      // fires the checked handler which sets the readonly mode of the controls
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropTop2", CropResizeArea2.Top);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropLeft2", CropResizeArea2.Left);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropWidth2", CropResizeArea2.Width);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerCropHeight2", CropResizeArea2.Height);

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("comboBoxScanFor", (int)InputFileExtension);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ImageHandlerFormatNr", (int)OutputFileExtension);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("comboBoxFileNameFormat", FileNameFormat);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("comboBoxSubFolder", FolderNameFormat);

        }
   

        public bool Start()
        {
            Stop();

            Watcher = new ScreenshotDirectoryWatcher(discoveryform, CallWithConverter);   // pass function to get the convert going
            Watcher.OnScreenshot += ConvertCompleted;  // and function for it to call when its over..

            return Watcher.Start(ScreenshotsDir, InputFileExtension.ToString());       // you can restart a watcher without stopping it..
        }

        public void Stop()
        {
            if (Watcher != null)
            {
                Watcher.Stop();
                Watcher.Dispose();
                Watcher = null;
            }
        }

        private void CallWithConverter(Action<ScreenShotImageConverter> cb)           // called by Watcher with a function to run in the UI main thread..
        {
            if (discoveryform.InvokeRequired) // on discovery form UI thread, action..
            {
                discoveryform.Invoke(new Action(() => CallWithConverter(cb)));
            }
            else
            {
                if (AutoConvert)
                {
                    ScreenShotImageConverter p = new ScreenShotImageConverter();
                    p.InputFolder = ScreenshotsDir;
                    p.OutputFolder = OutputDir;
                    p.FolderFormatIndex = FolderNameFormat;
                    p.FilenameFormatIndex = FileNameFormat;
                    p.HighRes = MarkHiRes;
                    p.KeepMasterConvertedImage = KeepMasterConvertedImage;
                    p.CropResizeImage1 = CropResize1;
                    p.CropResizeArea1 = CropResizeArea1;
                    p.CropResizeImage2 = CropResize2;
                    p.CropResizeArea2 = CropResizeArea2;
                    p.RemoveOriginal = RemoveOriginal;
                    p.OutputFileExtension = OutputFileExtension;
                    p.InputFileExtension = InputFileExtension;
                    p.CopyToClipboard = CopyToClipboard;

                    System.Diagnostics.Debug.Assert(Application.MessageLoop);

                    cb(p);                                  // call the processor the system wants. Function needs an image converter.  Back to processScreenshot
                }
            }
        }

        private void ConvertCompleted(ScreenShotImageConverter cp) // Called by the watcher when a convert had completed, in UI thread
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

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
                        discoveryform.LogLineHighlight("Copying image to clipboard failed");
                    }
                }
            }

            OnScreenShot?.Invoke(cp.OutputFilename, cp.FinalSize);
        }


        public string GetScreenshotPath(JournalScreenshot ss)       // helper to find path for screenshots
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


    }
}
