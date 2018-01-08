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
using System;
using System.Drawing;
using static EliteDangerousCore.DB.SQLiteDBClass;
using static System.Math;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCompass : UserControlCommonBase
    {

        private HistoryEntry last_he;
        private string DbLatSave { get { return "CompassLatTarget" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbLongSave { get { return "CompassLongTarget" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        #region Init

        public UserControlCompass()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            discoveryform.OnNewEntry += Display;
            textBoxTargetLatitude.Text = GetSettingString(DbLatSave, "");
            textBoxTargetLongitude.Text = GetSettingString(DbLongSave, "");
        }

        public override void Closing()
        {
            PutSettingString(DbLatSave, textBoxTargetLatitude.Text);
            PutSettingString(DbLongSave, textBoxTargetLongitude.Text);
            discoveryform.OnNewEntry -= Display;
        }

        #endregion

        #region Display

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            labelExt1.BackColor = labelExtBearing.BackColor = labelExtCurrLat.BackColor = labelExtTargetLong.BackColor = labelTargetLat.BackColor = curcol;
            textBoxCurrentLatitude.BackColor = textBoxCurrentLongitude.BackColor = textBoxTargetLatitude.BackColor = textBoxTargetLongitude.BackColor = curcol;
            BackColor = curcol;
            Display();
        }

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            
            Display(discoveryform.history.GetLast, discoveryform.history);
        }
        
        private void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }
        
        private void Display()
        {
            if (last_he != null)
            {
                double? latitude = null;
                double? longitude = null;
                switch (last_he.journalEntry.EventTypeID)
                {
                    case JournalTypeEnum.Screenshot:
                        JournalScreenshot js = last_he.journalEntry as JournalScreenshot;
                        latitude = js.nLatitude;
                        longitude = js.nLongitude;
                        break;
                    case JournalTypeEnum.Touchdown:
                        JournalTouchdown jt = last_he.journalEntry as JournalTouchdown;
                        if (jt.PlayerControlled.HasValue && jt.PlayerControlled.Value)
                        {
                            latitude = jt.Latitude;
                            longitude = jt.Longitude;
                        }
                        break;
                    case JournalTypeEnum.Location:
                        JournalLocation jl = last_he.journalEntry as JournalLocation;
                        latitude = jl.Latitude;
                        longitude = jl.Longitude;
                        break;
                    case JournalTypeEnum.Liftoff:
                        JournalLiftoff jlo = last_he.journalEntry as JournalLiftoff;
                        if (jlo.PlayerControlled.HasValue && jlo.PlayerControlled.Value)
                        {
                            latitude = jlo.Latitude;
                            longitude = jlo.Longitude;
                        }
                        break;
                    case JournalTypeEnum.FSDJump:
                        // we want to blank the current location when we're definitely not on a planet - could add other journal types here?
                        break;
                    default:
                        // but leave what's there if we might be and what's there is valid
                        double i, j;
                        if (Double.TryParse(textBoxCurrentLatitude.Text, out i))
                            latitude = i;
                        if (Double.TryParse(textBoxCurrentLongitude.Text, out j))
                            longitude = j;
                        break;
                }
                textBoxCurrentLatitude.Text = latitude.HasValue ? latitude.Value.ToString("N4") : "";
                textBoxCurrentLongitude.Text = longitude.HasValue ? longitude.Value.ToString("N4") : "";

                double? targetlat = null;
                double? targetlong = null;
                double x, y;
                if (Double.TryParse(textBoxTargetLatitude.Text, out x))
                    targetlat = x;
                if (Double.TryParse(textBoxTargetLongitude.Text, out y))
                    targetlong = y;

                double? bearing = CalculateBearing(latitude, longitude, targetlat, targetlong);
                labelExtBearing.Text = bearing.HasValue ? $"Bearing: {bearing.Value.ToString("N0")}" : "Bearing -";
            }
        }

        private double? CalculateBearing(double? currLat, double? currLong, double? targetLat, double? targetLong)
        {
            if (!(currLat.HasValue && currLong.HasValue && targetLat.HasValue && targetLong.HasValue))
                return null;
            // turn degrees to radians
            double long1 = currLong.Value * PI / 180;
            double lat1 = currLat.Value * PI / 180;
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
        #endregion

        private void textBox_Validated(object sender, EventArgs e)
        {
            Display();
        }

    }
}
