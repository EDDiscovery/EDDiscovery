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
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static EDDiscovery.Icons.Controls;
using static EliteDangerousCore.DB.SQLiteDBClass;
using static System.Math;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCompass : UserControlCommonBase
    {

        string DbLatSave { get { return "CompassLatTarget" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        string DbLongSave { get { return "CompassLongTarget" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        string DbHideSave { get { return "CompassAutoHide" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        double? latitude = null;
        double? longitude = null;
        double? heading = null;
        double? bodyRadius = null;
        double? targetBearing = null;
        double? targetDistance = null;
        bool autoHideTargetCoords = false;

        #region Init

        public UserControlCompass()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            discoveryform.OnNewEntry += Display;
            discoveryform.OnNewUIEvent += OnNewUIEvent;
            textBoxTargetLatitude.Text = GetSettingString(DbLatSave, "");
            textBoxTargetLongitude.Text = GetSettingString(DbLongSave, "");
            autoHideTargetCoords = GetSettingBool(DbHideSave, false);
            checkBoxHideTransparent.Checked = autoHideTargetCoords;
        }

        public override void Closing()
        {
            PutSettingString(DbLatSave, textBoxTargetLatitude.Text);
            PutSettingString(DbLongSave, textBoxTargetLongitude.Text);
            PutSettingBool(DbHideSave, autoHideTargetCoords);
            discoveryform.OnNewEntry -= Display;
            discoveryform.OnNewUIEvent -= OnNewUIEvent;
        }

        #endregion

        #region Display

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            labelExtTargetLong.BackColor = labelTargetLat.BackColor = curcol;
            textBoxTargetLatitude.BackColor = textBoxTargetLongitude.BackColor = curcol;
            BackColor = curcol;
            if (on && autoHideTargetCoords && textBoxTargetLatitude.Visible)
            {
                labelExtTargetLong.Visible = labelTargetLat.Visible = false;
                textBoxTargetLatitude.Visible = textBoxTargetLongitude.Visible = checkBoxHideTransparent.Visible = false;
                pictureBoxCompass.Top = textBoxTargetLongitude.Top;
            }
            if (!on && autoHideTargetCoords && !textBoxTargetLatitude.Visible)
            {
                labelExtTargetLong.Visible = labelTargetLat.Visible = true;
                textBoxTargetLatitude.Visible = textBoxTargetLongitude.Visible = checkBoxHideTransparent.Visible = true;
                pictureBoxCompass.Top = textBoxTargetLongitude.Top + textBoxTargetLongitude.Height + 3;
            }
            Display();
        }

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            Display(discoveryform.history.GetLast, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl)
        {
            if (he != null)
            {
                JournalScan sd = discoveryform.history.GetScans(he.System.Name).Where(sc => sc.BodyName == he.WhereAmI).FirstOrDefault();
                bodyRadius = sd?.nRadius;

                switch (he.journalEntry.EventTypeID)
                {
                    case JournalTypeEnum.Screenshot:
                        JournalScreenshot js = he.journalEntry as JournalScreenshot;
                        latitude = js.nLatitude;
                        longitude = js.nLongitude;
                        break;
                    case JournalTypeEnum.Touchdown:
                        JournalTouchdown jt = he.journalEntry as JournalTouchdown;
                        if (jt.PlayerControlled.HasValue && jt.PlayerControlled.Value)
                        {
                            latitude = jt.Latitude;
                            longitude = jt.Longitude;
                        }
                        break;
                    case JournalTypeEnum.Location:
                        JournalLocation jl = he.journalEntry as JournalLocation;
                        latitude = jl.Latitude;
                        longitude = jl.Longitude;
                        break;
                    case JournalTypeEnum.Liftoff:
                        JournalLiftoff jlo = he.journalEntry as JournalLiftoff;
                        if (jlo.PlayerControlled.HasValue && jlo.PlayerControlled.Value)
                        {
                            latitude = jlo.Latitude;
                            longitude = jlo.Longitude;
                        }
                        break;
                    case JournalTypeEnum.LeaveBody:
                        latitude = null;
                        longitude = null;
                        break;
                    default:
                        return;
                }
                Display();
            }
        }


        private void OnNewUIEvent(UIEvent uievent)       // UI event in, see if we want to hide.  UI events come before any onNew
        {
            EliteDangerousCore.UIEvents.UIPosition up = uievent as EliteDangerousCore.UIEvents.UIPosition;
            
            latitude = up?.Location?.Latitude;
            longitude = up?.Location?.Longitude;
            heading = up?.Heading;
            Display();
        }
        
        private void Display()
        {
            double? targetlat = null;
            double? targetlong = null;
            double x, y;
            if (Double.TryParse(textBoxTargetLatitude.Text, out x))
                targetlat = x;
            if (Double.TryParse(textBoxTargetLongitude.Text, out y))
                targetlong = y;

            targetBearing = CalculateBearing(targetlat, targetlong);
            targetDistance = CalculateDistance(targetlat, targetlong);
            DrawCompassImage();
            pictureBoxCompass.Render(true);
        }

        private string TargetDistanceFormatted()
        {
            if (!targetDistance.HasValue) return "distance unknown";
            if (targetDistance.Value < 1000) return $"{targetDistance.Value.ToString("N0")} m";
            if (targetDistance.Value < 1000000) return $"{(targetDistance.Value / 1000).ToString("N0")} km";
            return $"{(targetDistance.Value / 1000000).ToString("N0")} Mm";
        }

        private void DrawCompassImage()
        {
            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Transparent : BackColor;
            Font displayFont = discoveryform.theme.GetFont;
            PictureBoxHotspot.ImageElement compass;
            const int margin = 3;
            
            pictureBoxCompass.ClearImageList();
            if (heading.HasValue)
            {
                SizeF labelSize;
                using (Graphics tempGraph = Graphics.FromHwnd(Handle))
                {
                    labelSize = tempGraph.MeasureString("000", displayFont);
                }
                int bigTickSize = (int)labelSize.Height;
                int littleTickSize = (int)(labelSize.Height / 2);
                int scaleWidth = (int)labelSize.Width * 17;   // 9 labels + 8 gaps
                Size s = new Size(scaleWidth, bigTickSize + (int)labelSize.Width * 3 + margin * 3);
                int iconTop = bigTickSize + (int)labelSize.Height + margin * 2;
                int labelTop = bigTickSize + (int)labelSize.Height + margin * 2 + (int)labelSize.Width;
                Bitmap scale = new Bitmap(s.Width, s.Height);
                int left = (int)heading.Value - 90;
                bool steerLeft = false;
                bool steerRight = false;
                if (targetBearing.HasValue)
                {
                    int right = (int)heading.Value + 90;
                    int leftCorrected = left < 0 ? 360 + left : left;
                    int rightCorrected = right % 360;
                    int target = (int)targetBearing.Value;
                    if (rightCorrected >= 180)
                    {
                        steerRight = (target > rightCorrected && target <= 360) || (target < leftCorrected - 90);
                        steerLeft = target < leftCorrected && target >= leftCorrected - 90;
                    }
                    else
                    {
                        steerRight = target > rightCorrected && target <= rightCorrected + 90;
                        steerLeft = target > rightCorrected + 90 && target < leftCorrected;
                    }
                }
                using (Graphics gr = Graphics.FromImage(scale))
                {
                    string targetLabel = targetBearing.HasValue ? $"{targetBearing.Value.ToString("N0")}° ({TargetDistanceFormatted()})" : "";
                    SizeF targetLabelSize = gr.MeasureString(targetLabel, displayFont);
                    int tickGap = scaleWidth / 179;
                    gr.FillRectangle(new SolidBrush(backcolour), new Rectangle(0, 0, scale.Width, scale.Height));
                    Pen bigTick = new Pen(textcolour, 2);
                    Pen littleTick = new Pen(textcolour, 1);
                    for (int i = left; i <= (int)heading.Value + 90; i++)
                    {
                        int x = margin + ((i - left) * tickGap);
                        if (i % 5 == 0)
                        {
                            if (i % 20 == 0)
                            {
                                string label = i < 0 ? (i + 360).ToString() : (i % 360).ToString();
                                gr.DrawLine(bigTick, new Point(x, margin), new Point(x, bigTickSize));
                                gr.DrawString(label, displayFont, new SolidBrush(textcolour), x - margin, bigTickSize + margin * 2, StringFormat.GenericDefault);
                            }
                            else gr.DrawLine(littleTick, new Point(x, margin), new Point(x, littleTickSize));
                        }
                        if (i == (int)heading.Value)
                        {
                            using (Pen ahead = new Pen(textcolour, 4))
                            {
                                gr.DrawLine(ahead, new Point(x, 0), new Point(x, (int)labelSize.Width));
                            }
                        }
                        if (targetBearing.HasValue && (i + 360) % 360 == (int)targetBearing.Value)
                        {
                            gr.DrawImage(Map3D_Navigation_CenterOnSystem, new Rectangle(x - (int)labelSize.Width / 2, iconTop, (int)labelSize.Width, (int)labelSize.Width));
                            gr.DrawString(targetLabel, displayFont, new SolidBrush(textcolour), Max(x - (int)targetLabelSize.Width / 2, margin), labelTop, StringFormat.GenericDefault);
                        }
                    }
                    if (steerLeft)
                    {
                        gr.DrawImage(TabStrip_ArrowLeft, new Rectangle(margin, iconTop, (int)labelSize.Width, (int)labelSize.Width));
                        gr.DrawString(targetLabel, displayFont, new SolidBrush(textcolour), margin, labelTop, StringFormat.GenericDefault);
                    }
                    if (steerRight)
                    {
                        gr.DrawImage(TabStrip_ArrowRight, new Rectangle(scaleWidth - margin * 4 - (int)labelSize.Width, iconTop, (int)labelSize.Width, (int)labelSize.Width));
                        gr.DrawString(targetLabel, displayFont, new SolidBrush(textcolour), scaleWidth - margin * 4 - (int)targetLabelSize.Width, labelTop, StringFormat.GenericDefault);
                    }
                    bigTick.Dispose();
                    littleTick.Dispose();
                }
                compass = pictureBoxCompass.AddImage(new Rectangle(new Point(margin, margin), s), scale);
            }
            else
            {
                Size s = new Size(1920, 1080);
                compass = pictureBoxCompass.AddTextAutoSize(new Point(0, 2), s, "Surface coordinates unavailable", displayFont, textcolour, backcolour, 1.0F);
            }
            RevertToNormalSize();
            RequestTemporaryResize(new Size(Max(compass.img.Width + pictureBoxCompass.Left, textBoxTargetLatitude.Visible ? checkBoxHideTransparent.Right + 3 : 0), 
                compass.img.Height + (textBoxTargetLatitude.Visible ? textBoxTargetLatitude.Height + 6 : 0)));
        }

        private double? CalculateBearing(double? targetLat, double? targetLong)
        {
            if (!(latitude.HasValue && longitude.HasValue && targetLat.HasValue && targetLong.HasValue))
                return null;
            // turn degrees to radians
            double long1 = longitude.Value * PI / 180;
            double lat1 = latitude.Value * PI / 180;
            double long2 = targetLong.Value * PI / 180;
            double lat2 = targetLat.Value * PI / 180;

            double y = Sin(long2 - long1) * Cos(lat2);
            double x = Cos(lat1) * Sin(lat2) -
                        Sin(lat1) * Cos(lat2) * Cos(long2 - long1);
            
            // back to degrees again
            double bearing = Atan2(y, x) / PI * 180;

            //bearing in game HUD is 0-360, not -180 to 180
            return bearing > 0 ? bearing : 360 + bearing;
        }

        private double? CalculateDistance(double? targetLat, double? targetLong)
        {
            if (!(latitude.HasValue && longitude.HasValue && targetLat.HasValue && targetLong.HasValue && bodyRadius.HasValue))
                return null;

            double lat1 = latitude.Value * PI / 180;
            double lat2 = targetLat.Value * PI / 180;
            double deltaLong = (targetLong.Value - longitude.Value) * PI / 180;
            double deltaLat = (targetLat.Value - latitude.Value) * PI / 180;

            double a = Sin(deltaLat / 2) * Sin(deltaLat / 2) + Cos(lat1) * Cos(lat2) * Sin(deltaLat) * Sin(deltaLat);
            double c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));

            return bodyRadius.Value * c;
        }
        #endregion

        private void textBox_Validated(object sender, EventArgs e)
        {
            Display();
        }
        
        private void checkBoxHideTransparent_CheckedChanged(object sender, EventArgs e)
        {
            autoHideTargetCoords = ((CheckBoxCustom)sender).Checked;
            SetTransparency(IsTransparent, BackColor);
        }
    }
}
