/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScreenshot : UserControlCommonBase
    {
        public UserControlScreenshot()
        {
            InitializeComponent();
            pictureBox.Visible = false;
        }

        public override void Init()
        {
            discoveryform.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= TGChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += TGChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += TGChanged;
        }

        public override void InitialDisplay()
        {
            TGChanged(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        public override void Closing()
        {
            discoveryform.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            uctg.OnTravelSelectionChanged -= TGChanged;
        }

        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
            Display(file, size);
        }

        private void TGChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null && he.journalEntry is JournalScreenshot)
            {
                var ss = (JournalScreenshot)he.journalEntry;
                if (!String.IsNullOrEmpty(ss.EDDOutputFile) && File.Exists(ss.EDDOutputFile))
                {
                    Display(ss.EDDOutputFile, new Size(ss.EDDOutputWidth, ss.EDDOutputHeight));
                }
                else
                {
                    var filename = GetScreenshotPath(ss.Filename);

                    if (filename != null && File.Exists(filename) && ss.Width != 0 && ss.Height != 0)
                    {
                        Display(filename, new Size(ss.Width, ss.Height));
                    }
                }
            }
        }

        private string GetScreenshotPath(string filepart)
        {
            var filenameout = filepart;

            if (filepart.StartsWith("\\ED_Pictures\\"))     // if its an ss record, try and find it either in watchedfolder or in default loc
            {
                filepart = filepart.Substring(13);
                filenameout = Path.Combine(discoveryform.screenshotconverter.InputFolder, filepart);

                if (!File.Exists(filenameout))
                {
                    string defaultInputDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Frontier Developments", "Elite Dangerous");
                    filenameout = Path.Combine(defaultInputDir, filepart);
                }
            }

            return filenameout;
        }

        private void Display(string file, Size s)
        {
            try
            {
                if (file.HasChars() && File.Exists(file))
                {
                    pictureBox.ImageLocation = file;                       // this could except, so protect..
                    imagesize = s;
                    FitToWindow();
                }
                else
                {
                    pictureBox.Visible = false;
                    pictureBox.ImageLocation = null;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot assign " + file + " to screenshot img " + ex);
            }
        }

        Size imagesize;

        void FitToWindow()
        {
            if (ClientRectangle.Width > 15 && ClientRectangle.Height > 15)
            {
                //System.Diagnostics.Debug.WriteLine("Screen shot " + ImagePath);

                double ratiopicture = (double)imagesize.Width / (double)imagesize.Height;

                int boxwidth = ClientRectangle.Width;
                int boxheight = ClientRectangle.Height;

                int imagewidth = boxwidth;
                int imageheight = (int)((double)imagewidth / ratiopicture);

                if (imageheight > boxheight)        // if width/ratio > available height, scale down width
                {
                    double scaledownwidth = (double)imageheight / (double)boxheight;
                    imagewidth = (int)((double)imagewidth / scaledownwidth);
                }

                imageheight = (int)((double)imagewidth / ratiopicture);
                pictureBox.Location = new Point((boxwidth - imagewidth) / 2, (boxheight - imageheight) / 2);
                pictureBox.Size = new Size(imagewidth, imageheight);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Visible = true;
            }
            else
                pictureBox.Visible = false;
        }

        private void UserControlScreenshot_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }
    }
}
