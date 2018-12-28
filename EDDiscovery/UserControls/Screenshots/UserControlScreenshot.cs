/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System.IO;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScreenshot : UserControlCommonBase
    {
        string ImagePath = null;
        Point ImageSize;

        public UserControlScreenshot()
        {
            InitializeComponent();
            pictureBox.Visible = false;
        }

        public override void Init()
        {
            discoveryform.screenshotconverter.OnScreenShot += ScreenShot;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            discoveryform.screenshotconverter.OnScreenShot -= ScreenShot;
            uctg.OnTravelSelectionChanged -= Display;
        }

        public void ScreenShot(string path, Point size)
        {
            ImagePath = path;
            ImageSize = size;

            FitToWindow();
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                if (he.EntryType == EliteDangerousCore.JournalTypeEnum.Screenshot)
                {
                    JournalScreenshot ss = (JournalScreenshot)he.journalEntry;

                    JObject jo = ss.GetJson();
                    if (jo["EDDOutputFile"] != null && File.Exists(jo["EDDOutputFile"].Str()))
                    {
                        string store_name = jo["EDDOutputFile"].Str();
                        Point size = new Point(jo["EDDOutputWidth"].Int(), jo["EDDOutputHeight"].Int());

                        ScreenShot(store_name, size);
                    }
                    else if (jo["EDDInputFile"] != null && File.Exists(jo["EDDInputFile"].Str()))
                    {
                        string filename = jo["EDDInputFile"].Str();
                        ScreenShot(filename, new Point(ss.Width, ss.Height));
                    }
                    else
                    {
                        string filename = discoveryform.screenshotconverter.GetScreenshotPath(ss);

                        if (File.Exists(filename))
                        {
                            ScreenShot(filename, new Point(ss.Width, ss.Height));
                        }
                    }
                }
            }
        }

        void FitToWindow()
        {
            //System.Diagnostics.Debug.WriteLine("Screen shot " + ImagePath);

            double ratiopicture = (double)ImageSize.X / (double)ImageSize.Y;

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

            try
            {
                pictureBox.Location = new Point((boxwidth - imagewidth) / 2, (boxheight - imageheight) / 2);
                pictureBox.Size = new Size(imagewidth, imageheight);

                pictureBox.ImageLocation = ImagePath;                       // this could except, so protect..
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Visible = true;
            }
            catch
            {
                pictureBox.Visible = false;
            }
        }

        private void UserControlScreenshot_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }
    }
}
