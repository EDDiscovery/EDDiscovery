/*
 * Copyright 2016 - 2022 EDDiscovery development team
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
 * 
 */

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScreenshot : UserControlCommonBase
    {
        private Size imagesize;
        private string autotext;
        private string imagetext = "Image";
        private string dbImages = "Images";
        private List<string> extimages = new List<string>();

        public UserControlScreenshot()
        {
            InitializeComponent();
            DBBaseName = "Screensaver";
            pictureBox.Visible = false;
            rollUpPanelTop.Resize += RollUpPanelTop_Resize;
        }

        public override void Init()
        {
            extButtonRemoveImage.Enabled = extButtonCopy.Enabled = false;

            DiscoveryForm.ScreenShotCaptured += Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            autotext = "Travel History Entry".Tx();

            string[] dbsaved = GetSetting(dbImages, "").Split('\u2188');
            foreach (var s in dbsaved)
            {
                if (File.Exists(s))     // make sure they are still there.
                    extimages.Add(s);
            }
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            UpdateComboBox();
        }

        private void Discoveryform_OnHistoryChange()
        {
            UpdateComboBox();
        }


        public override void Closing()
        {
            DiscoveryForm.ScreenShotCaptured -= Discoveryform_ScreenShotCaptured;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            PutSetting(dbImages, String.Join("\u2188", extimages));
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if ( he.journalEntry is JournalScreenshot )
            {
                UpdateComboBox();   // need to make sure box is updated
            }
        }
        private void Discoveryform_ScreenShotCaptured(string file, Size size)
        {
            if ( extComboBoxImage.SelectedIndex == 0 )  // if on auto
                Display(file, size);
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (he.journalEntry is JournalScreenshot)    // if screen shot
            {
                if (extComboBoxImage.SelectedIndex == 0)      // if on Auto..
                {
                    Display(he.journalEntry as JournalScreenshot);
                }
            }
        }

        // Display he
        private void Display(JournalScreenshot js)
        {
            var pathsize = js.GetScreenshotPath(DiscoveryForm.ScreenshotConverter.InputFolder);

            if (pathsize != null)
            {
                Display(pathsize.Item1, pathsize.Item2);
            }
            else
                Display("");
        }

        // Display file, give size if you know
        private void Display(string file, Size? s = null)
        {
            try
            {
                if (file.HasChars() && File.Exists(file))
                {
                    if ( s == null )        // if we don't have size, find it out
                    {
                        using (Image imgload = Image.FromFile(file))
                        {
                            s = imgload.Size;
                        }
                    }

                    pictureBox.ImageLocation = file;                       // this could except, so protect..
                    imagesize = s.Value;
                    FitToWindow();

                    SetControlText(file.ReplaceIfStartsWith(JournalScreenshot.DefaultInputDir(),"ED:"));

                    extButtonCopy.Enabled = true;
                    return;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot assign " + file + " to screenshot img " + ex);
            }

            extButtonCopy.Enabled = false;
            pictureBox.Visible = false;
            pictureBox.ImageLocation = null;
        }

        void FitToWindow()
        {
            if (ClientRectangle.Width > 15 && ClientRectangle.Height > 15 && !imagesize.IsEmpty)
            {
                System.Diagnostics.Debug.WriteLine($"Screen shot {imagesize}");

                double ratiopicture = (double)imagesize.Width / (double)imagesize.Height;

                int boxwidth = ClientRectangle.Width;
                int boxheight = ClientRectangle.Height - rollUpPanelTop.Height;

                int imagewidth = boxwidth;
                int imageheight = (int)((double)imagewidth / ratiopicture);

                if (imageheight > boxheight)        // if width/ratio > available height, scale down width
                {
                    double scaledownwidth = (double)imageheight / (double)boxheight;
                    imagewidth = (int)((double)imagewidth / scaledownwidth);
                }

                imageheight = (int)((double)imagewidth / ratiopicture);
                pictureBox.Location = new Point((boxwidth - imagewidth) / 2, (boxheight - imageheight) / 2 + rollUpPanelTop.Height);
                pictureBox.Size = new Size(imagewidth, imageheight);

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Visible = true;
            }
            else
                pictureBox.Visible = false;
        }

        private void RollUpPanelTop_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }

        private void UserControlScreenshot_Resize(object sender, EventArgs e)
        {
            FitToWindow();
        }

        private void UpdateComboBox()
        {
            var sslist = HistoryList.LatestFirst(DiscoveryForm.History.EntryOrder(), new System.Collections.Generic.HashSet<JournalTypeEnum>() { JournalTypeEnum.Screenshot });

            string cur = extComboBoxImage.Text;

            extComboBoxImage.Tag = sslist;
            extComboBoxImage.Items.Clear();
            extComboBoxImage.Items.Add(autotext);
            extComboBoxImage.Items.Add(imagetext);
            extComboBoxImage.Items.AddRange(extimages);
            foreach (var x in sslist)
            {
                var j = x.journalEntry as JournalScreenshot;
                var pathsize = j.GetScreenshotPath(DiscoveryForm.ScreenshotConverter.InputFolder);
                if (pathsize != null)
                {
                 //   System.Diagnostics.Debug.WriteLine($"Accept {pathsize.Item1} {x.WhereAmI} {File.Exists(pathsize.Item1)}");
                    try
                    {
                        var ct = File.GetCreationTimeUtc(pathsize.Item1);
                        var diff = j.EventTimeUTC - ct;

                        extComboBoxImage.Items.Add($"{EDDConfig.Instance.ConvertTimeToSelectedFromUTC(j.EventTimeUTC).ToString()} {x.WhereAmI}{(Math.Abs(diff.TotalSeconds)>120?" *": "")}");
                    }
                    catch
                    {

                    }
                }
                else
                {
                  //  System.Diagnostics.Debug.WriteLine($"Reject {j.Filename} {x.WhereAmI}");
                }
            }

            if (cur == "" || !extComboBoxImage.Items.Contains(cur))
                cur =  autotext;

            extComboBoxImage.Enabled = false;
            extComboBoxImage.SelectedItem = cur;
            extComboBoxImage.Enabled = true;
        }

        private void extComboBoxImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( extComboBoxImage.Enabled)
            {
                extButtonRemoveImage.Enabled = false;

                if (extComboBoxImage.SelectedIndex == 0)    // travel history, no action
                {
                }
                else if (extComboBoxImage.SelectedIndex == 1)   // image load
                {
                    OpenFileDialog dlg = new OpenFileDialog();

                    dlg.Filter = "Images|*.jpeg;*.png;*.jpg;*.bmp;*.tif;*.tiff;";
                    dlg.Title = imagetext;

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        var path = dlg.FileName;
                        if ( !extimages.Contains(path))     // don't double add
                            extimages.Add(path);

                        extComboBoxImage.Text = path;
                        UpdateComboBox();
                        Display(path);
                        extButtonRemoveImage.Enabled = true;
                    }
                }
                else 
                {
                    int hepos = 2 + extimages.Count;
                    if (extComboBoxImage.SelectedIndex < hepos)
                    {
                        Display(extimages[extComboBoxImage.SelectedIndex - 2]);
                        extButtonRemoveImage.Enabled = true;
                    }
                    else
                    {
                        var helist = extComboBoxImage.Tag as List<HistoryEntry>;
                        var he = helist[extComboBoxImage.SelectedIndex - hepos];
                        Display(he.journalEntry as JournalScreenshot);
                    }
                }
            }

        }

        private void extButtonRemoveImage_Click(object sender, EventArgs e)
        {
            int offset = extComboBoxImage.SelectedIndex - 2;

            if (offset >= 0 && offset < extimages.Count)        // if withing ext images range
            {
                extimages.RemoveAt(offset);
                
                extComboBoxImage.Enabled = false;
                extComboBoxImage.SelectedItem = 0;      // go back to travel history
                extComboBoxImage.Enabled = true;

                UpdateComboBox();       // and update 
                Display("");            // clear display
            }
        }

        private void extButtonCopy_Click(object sender, EventArgs e)
        {
            if ( pictureBox.ImageLocation.HasChars())
            {
                SetClipboardImage(pictureBox.ImageLocation);
            }
        }
    }
}
