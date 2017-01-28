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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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
using EDDiscovery.EliteDangerous.JournalEvents;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScreenshot : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;

        string ImagePath = null;
        Point ImageSize;

        public UserControlScreenshot()
        {
            InitializeComponent();
            Name = "Screen Shot";
            pictureBox.Visible = false;
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;
            discoveryform.ImageHandler.OnScreenShot += ScreenShot;
            discoveryform.TravelControl.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            discoveryform.ImageHandler.OnScreenShot -= ScreenShot;
            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
        }

        public void ScreenShot(string path, Point size)
        {
            ImagePath = path;
            ImageSize = size;

            FitToWindow();
        }

        public override void Display(HistoryEntry he, HistoryList hl)
        {
            if (he != null)
            {
                if (he.EntryType == EliteDangerous.JournalTypeEnum.Screenshot)
                {
                    JournalScreenshot ss = (JournalScreenshot)he.journalEntry;

                    JObject jo = JObject.Parse(ss.EventDataString);
                    if (jo["EDDOutputFile"] != null && File.Exists(JSONHelper.GetStringDef(jo["EDDOutputFile"])))
                    {
                        string store_name = JSONHelper.GetStringDef(jo["EDDOutputFile"]);
                        Point size = new Point(JSONHelper.GetInt(jo["EDDOutputWidth"]), JSONHelper.GetInt(jo["EDDOutputHeight"]));

                        ScreenShot(store_name, size);
                    }
                    else if (jo["EDDInputFile"] != null && File.Exists(JSONHelper.GetStringDef(jo["EDDInputFile"])))
                    {
                        string filename = JSONHelper.GetStringDef(jo["EDDInputFile"]);
                        ScreenShot(filename, new Point(ss.Width, ss.Height));
                    }
                    else
                    {
                        string filename = discoveryform.ImageHandler.GetScreenshotPath(ss);

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
