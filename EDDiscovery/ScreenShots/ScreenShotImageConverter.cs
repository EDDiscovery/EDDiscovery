﻿/*
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
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Except for naming, same code as before. Just removed dependency on discovery form.

namespace EDDiscovery.ScreenShots
{
    public class ScreenShotImageConverter
    {
        // config

        public string InputFolder = "";
        public string OutputFolder = "";

        public int FolderFormatIndex = 0;
        public static string[] SubFolderSelections = new string[]
        {
            "None",         //0
            "System Name",
            "YYYY-MM-DD",
            "DD-MM-YYYY",
            "MM-DD-YYYY",
            "YYYY-MM-DD Sysname",   //5
            "DD-MM-YYYY Sysname",
            "MM-DD-YYYY Sysname",
            "CMDRName",
            "CMDRName Sysname",
            "YYYY-MM-DD CMDRName Sysname",  //10
            "CMDRName\\Sysname"
        };

        public int FilenameFormatIndex = 0;
        public static string[] FileNameFormats = new string[]
        {
            "Sysname (YYYYMMDD-HHMMSS)",            //0
            "Sysname (Windows dateformat)",
            "YYYY-MM-DD HH-MM-SS Sysname",
            "DD-MM-YYYY HH-MM-SS Sysname",
            "MM-DD-YYYY HH-MM-SS Sysname",          //4
            "HH-MM-SS Sysname",
            "HH-MM-SS",
            "Sysname",
            "Keep original",                        // 8
            "Sysname BodyName (YYYYMMDD-HHMMSS)",       //9
            "Sysname BodyName (Windows dateformat)",
            "YYYY-MM-DD HH-MM-SS Sysname BodyName",     //11
            "DD-MM-YYYY HH-MM-SS Sysname BodyName",
            "MM-DD-YYYY HH-MM-SS Sysname BodyName",     //13
            "HH-MM-SS Sysname BodyName",        //14
            "Sysname BodyName",                 //15
        };

        public bool HighRes = false;
        public bool CropImage = false;
        public Rectangle CropArea = new Rectangle();
        public bool RemoveInputFile = false;

        public enum OutputTypes { png, jpg, bmp, tiff };
        public OutputTypes OutputFileExtension { get; set; } = OutputTypes.png;

        public enum InputTypes { bmp, jpg, png };
        public InputTypes InputFileExtension { get; set; } = InputTypes.bmp;

        public bool CopyToClipboard = false;


        // on item
        public string InputFilename;
        public string SystemName;
        public JournalScreenshot JournalScreenShot;
        public int CommanderID;

        // passing back
        public string OutputFilename;
        public bool Converted;
        public Point FinalSize;

        // privates

        private DateTime Timestamp;

        public bool Convert(Bitmap bmp, Action<string> logit) // can call independent of watcher, pass in bmp to convert
        {
            OutputFilename = null;
            FinalSize = Point.Empty;
            Converted = false;

            if (!UpdateOutputFolderWithSubFolder())  // add on any sub folder options to output folder
            {
                logit(string.Format(("Cannot convert {0} into the same folder as they are stored into" + Environment.NewLine + "Pick a different conversion folder or a different output format").Tx(this,"FolderErr"), InputFileExtension.ToString()));
                return false;
            }

            using (Bitmap croppedbmp = ConvertImage(bmp))   // do the convert to BMP
            {
                if (OutputFileExtension == OutputTypes.jpg)
                {
                    croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (OutputFileExtension == OutputTypes.tiff)
                {
                    croppedbmp.Save(OutputFilename, System.Drawing.Imaging.ImageFormat.Tiff);
                }
                else if (OutputFileExtension == OutputTypes.bmp)
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

            if (Converted)
            {
                File.SetCreationTime(OutputFilename, Timestamp);

                if (JournalScreenShot != null)
                {
                    JournalScreenShot.SetConvertedFilename(InputFilename, OutputFilename, FinalSize.X, FinalSize.Y);
                }

                System.Diagnostics.Debug.WriteLine("Convert " + InputFilename + " at " + SystemName + " to " + OutputFilename);

                logit(string.Format("Converted {0} to {1}".Tx(this,"CNV"), Path.GetFileName(InputFilename) , Path.GetFileName(OutputFilename)));
            }

            return Converted;
        }

        private Bitmap ConvertImage(Bitmap bmp)
        {
            int index = 0;

            string bodyname = (JournalScreenShot.Body == null) ? "" : (JournalScreenShot.Body.Equals(SystemName, StringComparison.InvariantCultureIgnoreCase) ? "" : JournalScreenShot.Body);

            do                                          // add _N on the filename for index>0, to make them unique.
            {

                OutputFilename = Path.Combine(OutputFolder, CreateFileName(SystemName, bodyname, InputFilename, FilenameFormatIndex, HighRes, Timestamp) + (index == 0 ? "" : "_" + index) + "." + OutputFileExtension.ToString());
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




        // given a file, get the screenshot as a bmp

        public Bitmap GetScreenshot(ref string filename, Action<string> logit)
        {
            Bitmap bmp = GetScreenshot(filename, SystemName, ref CommanderID, ref JournalScreenShot, ref OutputFilename, ref FinalSize, ref Timestamp, ref filename, logit);
            InputFilename = filename;
            return bmp;
        }

        private Bitmap GetScreenshot(string inputfile, string cur_sysname, ref int cmdrid, ref JournalScreenshot ss, ref string store_name, ref Point finalsize, ref DateTime timestamp, ref string readfilename, Action<string> logit)
        {
            Bitmap bmp = null;

            for (int tries = 60; tries > 0; tries--)          // wait 30 seconds and then try it anyway.. 32K hires shots take a while to write.
            {
                if (TryGetScreenshot(inputfile, cur_sysname, ref cmdrid, ref ss, ref store_name, ref finalsize, ref timestamp, out bmp, out readfilename, logit, false))
                {
                    break;
                }

                System.Threading.Thread.Sleep(500);     // every 500ms see if we can read the file, if we can, go, else wait..
            }

            if (bmp == null)
            {
                TryGetScreenshot(inputfile, cur_sysname, ref cmdrid, ref ss, ref store_name, ref finalsize, ref timestamp, out bmp, out readfilename, logit, true);
            }

            return bmp;
        }


        private bool TryGetScreenshot(string filename, string cur_sysname, ref int cmdrid, ref JournalScreenshot ss, ref string store_name, ref Point finalsize, ref DateTime timestamp, out Bitmap bmp, out string readfilename, Action<string> logit, bool throwOnError = false)
        {
            string defaultInputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");

            if (filename.StartsWith("\\ED_Pictures\\"))
            {
                filename = filename.Substring(13);
                string filepath = Path.Combine(InputFolder ?? defaultInputDir, filename);

                if (!File.Exists(filepath))
                {
                    filepath = Path.Combine(defaultInputDir, filename);
                }

                if (File.Exists(filepath))
                {
                    filename = filepath;
                }
            }

            bmp = null;

            try
            {
                using (FileStream testfile = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))        // throws if can't open
                {
                    timestamp = new FileInfo(filename).CreationTimeUtc;
                    MemoryStream memstrm = new MemoryStream(); // will be owned by bitmap
                    testfile.CopyTo(memstrm);
                    memstrm.Seek(0, SeekOrigin.Begin);
                    bmp = new Bitmap(memstrm);

                    if (ss == null)
                    {
                        ss = JournalScreenshot.GetScreenshot(filename, bmp.Size.Width, bmp.Size.Height, timestamp, cur_sysname == "Unknown System" ? null : cur_sysname, cmdrid);
                    }

                    if (ss != null)
                    {
                        string ss_infile = null;
                        string ss_outfile = null;
                        int ss_width = 0;
                        int ss_height = 0;
                        ss.GetConvertedFilename(out ss_infile, out ss_outfile, out ss_width, out ss_height);
                        JObject jo = ss.GetJson();
                        if (ss_outfile != null && File.Exists(ss_outfile))
                        {
                            store_name = ss_outfile;
                            finalsize = new Point(ss_width, ss_height);
                        }
                    }

                    readfilename = filename;
                    cmdrid = ss.CommanderId;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                }

                if (throwOnError)
                {
                    logit(string.Format("Unable to open screenshot '{0}': {1}".Tx(this,"ERRF") , filename , ex.Message));
                    throw;
                }

                readfilename = null;
                return false;
            }
        }

        // helpers for above

        private bool UpdateOutputFolderWithSubFolder()
        {
            if (String.IsNullOrWhiteSpace(OutputFolder))
            {
                OutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous", "Converted");
            }

            switch (FolderFormatIndex)
            {
                case 1:     // system name
                    OutputFolder = Path.Combine(OutputFolder, SystemName.SafeFileString());
                    break;

                case 2:     // "YYYY-MM-DD"
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("yyyy-MM-dd"));
                    break;
                case 3:     // "DD-MM-YYYY"
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("dd-MM-yyyy"));
                    break;
                case 4:     // "MM-DD-YYYY"
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("MM-dd-yyyy"));
                    break;

                case 5:  //"YYYY-MM-DD Sysname",
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("yyyy-MM-dd") + " " + SystemName.SafeFileString());
                    break;

                case 6:  //"DD-MM-YYYY Sysname",
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("dd-MM-yyyy") + " " + SystemName.SafeFileString());
                    break;

                case 7: //"MM-DD-YYYY Sysname"
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("MM-dd-yyyy") + " " + SystemName.SafeFileString());
                    break;

                case 8: // CMDR name
                    OutputFolder = Path.Combine(OutputFolder, (EDCommander.GetCommander(CommanderID)?.Name ?? $"CmdrId{CommanderID}").SafeFileString());
                    break;

                case 9: // CMDR name at sysname
                    OutputFolder = Path.Combine(OutputFolder, (EDCommander.GetCommander(CommanderID)?.Name ?? $"CmdrId{CommanderID}").SafeFileString() + " at " + SystemName.SafeFileString());
                    break;

                case 10: // YYYY - MM - DD CMDR name at sysname
                    OutputFolder = Path.Combine(OutputFolder, Timestamp.ToString("yyyy-MM-dd") + " " +
                          (EDCommander.GetCommander(CommanderID)?.Name ?? $"CmdrId{CommanderID}").SafeFileString() + " at " + SystemName.SafeFileString());
                    break;

                case 11: // CMDR Name \ SystemName
                    OutputFolder = Path.Combine(OutputFolder, (EDCommander.GetCommander(CommanderID)?.Name ?? $"CmdrId{CommanderID}").SafeFileString(), SystemName.SafeFileString());
                    break;
            }

            if (!Directory.Exists(OutputFolder))
                Directory.CreateDirectory(OutputFolder);

            return !(OutputFolder.Equals(InputFolder) && OutputFileExtension.ToString().Equals(InputFileExtension.ToString()));
        }


        public static string CreateFileName(string cur_sysname, string cur_bodyname, string inputfile, int formatindex, bool hires, DateTime timestamp)
        {
            cur_sysname = cur_sysname.SafeFileString();
            cur_bodyname = cur_bodyname.SafeFileString();

            string postfix = (hires && Path.GetFileName(inputfile).Contains("HighRes")) ? " (HighRes)" : "";
            string bodyinsert = (formatindex >= 9 && formatindex <= 15 && cur_bodyname.Length > 0) ? ("-" + cur_bodyname) : "";

            switch (formatindex)
            {
                case 0:
                case 9:
                    return cur_sysname + bodyinsert + " (" + timestamp.ToString("yyyyMMdd-HHmmss") + ")" + postfix;

                case 1:
                case 10:
                    {
                        string time = timestamp.ToString();
                        time = time.Replace(":", "-");
                        time = time.Replace("/", "-");          // Rob found it was outputting 21/2/2020 on mine, so we need more replaces
                        time = time.Replace("\\", "-");
                        return cur_sysname + bodyinsert + " (" + time + ")" + postfix;
                    }
                case 2:
                case 11:
                    {
                        string time = timestamp.ToString("yyyy-MM-dd HH-mm-ss");
                        return time + " " + cur_sysname + bodyinsert + postfix;
                    }
                case 3:
                case 12:
                    {
                        string time = timestamp.ToString("dd-MM-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + bodyinsert + postfix;
                    }
                case 4:
                case 13:
                    {
                        string time = timestamp.ToString("MM-dd-yyyy HH-mm-ss");
                        return time + " " + cur_sysname + bodyinsert + postfix;
                    }

                case 5:
                case 14:
                    {
                        string time = timestamp.ToString("HH-mm-ss");
                        return time + " " + cur_sysname + bodyinsert + postfix;
                    }

                case 6:
                    {
                        string time = timestamp.ToString("HH-mm-ss");
                        return time + postfix;
                    }

                case 7:
                case 15:
                    {
                        return cur_sysname + bodyinsert + postfix;
                    }

                default:
                    return Path.GetFileNameWithoutExtension(inputfile);
            }
        }
    }
}

