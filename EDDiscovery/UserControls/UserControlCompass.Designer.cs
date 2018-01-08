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
namespace EDDiscovery.UserControls
{
    partial class UserControlCompass
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxCurrentLatitude = new ExtendedControls.TextBoxBorder();
            this.textBoxCurrentLongitude = new ExtendedControls.TextBoxBorder();
            this.textBoxTargetLatitude = new ExtendedControls.TextBoxBorder();
            this.textBoxTargetLongitude = new ExtendedControls.TextBoxBorder();
            this.labelExtBearing = new ExtendedControls.LabelExt();
            this.labelExtCurrLat = new ExtendedControls.LabelExt();
            this.labelExt1 = new ExtendedControls.LabelExt();
            this.labelTargetLat = new ExtendedControls.LabelExt();
            this.labelExtTargetLong = new ExtendedControls.LabelExt();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // textBoxCurrentLatitude
            // 
            this.textBoxCurrentLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCurrentLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCurrentLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCurrentLatitude.BorderColorScaling = 0.5F;
            this.textBoxCurrentLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCurrentLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCurrentLatitude.Location = new System.Drawing.Point(91, 42);
            this.textBoxCurrentLatitude.Multiline = false;
            this.textBoxCurrentLatitude.Name = "textBoxCurrentLatitude";
            this.textBoxCurrentLatitude.ReadOnly = false;
            this.textBoxCurrentLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCurrentLatitude.SelectionLength = 0;
            this.textBoxCurrentLatitude.SelectionStart = 0;
            this.textBoxCurrentLatitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxCurrentLatitude.TabIndex = 3;
            this.textBoxCurrentLatitude.Text = "0";
            this.textBoxCurrentLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxCurrentLatitude, "Current Latitude - take a screenshot to auto update");
            this.textBoxCurrentLatitude.WordWrap = true;
            this.textBoxCurrentLatitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // textBoxCurrentLongitude
            // 
            this.textBoxCurrentLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCurrentLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCurrentLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCurrentLongitude.BorderColorScaling = 0.5F;
            this.textBoxCurrentLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCurrentLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCurrentLongitude.Location = new System.Drawing.Point(249, 42);
            this.textBoxCurrentLongitude.Multiline = false;
            this.textBoxCurrentLongitude.Name = "textBoxCurrentLongitude";
            this.textBoxCurrentLongitude.ReadOnly = false;
            this.textBoxCurrentLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCurrentLongitude.SelectionLength = 0;
            this.textBoxCurrentLongitude.SelectionStart = 0;
            this.textBoxCurrentLongitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxCurrentLongitude.TabIndex = 4;
            this.textBoxCurrentLongitude.Text = "0";
            this.textBoxCurrentLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxCurrentLongitude, "Current Longitude - take a screenshot to auto update");
            this.textBoxCurrentLongitude.WordWrap = true;
            this.textBoxCurrentLongitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // textBoxTargetLatitude
            // 
            this.textBoxTargetLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTargetLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTargetLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetLatitude.BorderColorScaling = 0.5F;
            this.textBoxTargetLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTargetLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTargetLatitude.Location = new System.Drawing.Point(91, 16);
            this.textBoxTargetLatitude.Multiline = false;
            this.textBoxTargetLatitude.Name = "textBoxTargetLatitude";
            this.textBoxTargetLatitude.ReadOnly = false;
            this.textBoxTargetLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTargetLatitude.SelectionLength = 0;
            this.textBoxTargetLatitude.SelectionStart = 0;
            this.textBoxTargetLatitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxTargetLatitude.TabIndex = 1;
            this.textBoxTargetLatitude.Text = "0";
            this.textBoxTargetLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTargetLatitude, "Latitude of your destination");
            this.textBoxTargetLatitude.WordWrap = true;
            this.textBoxTargetLatitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // textBoxTargetLongitude
            // 
            this.textBoxTargetLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTargetLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTargetLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetLongitude.BorderColorScaling = 0.5F;
            this.textBoxTargetLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTargetLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTargetLongitude.Location = new System.Drawing.Point(249, 16);
            this.textBoxTargetLongitude.Multiline = false;
            this.textBoxTargetLongitude.Name = "textBoxTargetLongitude";
            this.textBoxTargetLongitude.ReadOnly = false;
            this.textBoxTargetLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTargetLongitude.SelectionLength = 0;
            this.textBoxTargetLongitude.SelectionStart = 0;
            this.textBoxTargetLongitude.Size = new System.Drawing.Size(55, 20);
            this.textBoxTargetLongitude.TabIndex = 2;
            this.textBoxTargetLongitude.Text = "0";
            this.textBoxTargetLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTargetLongitude, "Longitude of your destination");
            this.textBoxTargetLongitude.WordWrap = true;
            this.textBoxTargetLongitude.Validated += new System.EventHandler(this.textBox_Validated);
            // 
            // labelExtBearing
            // 
            this.labelExtBearing.AutoSize = true;
            this.labelExtBearing.Location = new System.Drawing.Point(97, 0);
            this.labelExtBearing.Name = "labelExtBearing";
            this.labelExtBearing.Size = new System.Drawing.Size(49, 13);
            this.labelExtBearing.TabIndex = 8;
            this.labelExtBearing.Text = "Bearing -";
            this.toolTip1.SetToolTip(this.labelExtBearing, "Bearing from current location to target location");
            // 
            // labelExtCurrLat
            // 
            this.labelExtCurrLat.AutoSize = true;
            this.labelExtCurrLat.Location = new System.Drawing.Point(3, 42);
            this.labelExtCurrLat.Name = "labelExtCurrLat";
            this.labelExtCurrLat.Size = new System.Drawing.Size(82, 13);
            this.labelExtCurrLat.TabIndex = 1;
            this.labelExtCurrLat.Text = "Current Latitude";
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(152, 42);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(91, 13);
            this.labelExt1.TabIndex = 2;
            this.labelExt1.Text = "Current Longitude";
            // 
            // labelTargetLat
            // 
            this.labelTargetLat.AutoSize = true;
            this.labelTargetLat.Location = new System.Drawing.Point(6, 16);
            this.labelTargetLat.Name = "labelTargetLat";
            this.labelTargetLat.Size = new System.Drawing.Size(79, 13);
            this.labelTargetLat.TabIndex = 4;
            this.labelTargetLat.Text = "Target Latitude";
            // 
            // labelExtTargetLong
            // 
            this.labelExtTargetLong.AutoSize = true;
            this.labelExtTargetLong.Location = new System.Drawing.Point(155, 16);
            this.labelExtTargetLong.Name = "labelExtTargetLong";
            this.labelExtTargetLong.Size = new System.Drawing.Size(88, 13);
            this.labelExtTargetLong.TabIndex = 6;
            this.labelExtTargetLong.Text = "Target Longitude";
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelExtBearing);
            this.Controls.Add(this.textBoxTargetLongitude);
            this.Controls.Add(this.labelExtTargetLong);
            this.Controls.Add(this.textBoxTargetLatitude);
            this.Controls.Add(this.labelTargetLat);
            this.Controls.Add(this.textBoxCurrentLongitude);
            this.Controls.Add(this.labelExt1);
            this.Controls.Add(this.labelExtCurrLat);
            this.Controls.Add(this.textBoxCurrentLatitude);
            this.Name = "UserControlCompass";
            this.Size = new System.Drawing.Size(312, 67);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.TextBoxBorder textBoxCurrentLatitude;
        private ExtendedControls.LabelExt labelExtCurrLat;
        private ExtendedControls.LabelExt labelExt1;
        private ExtendedControls.TextBoxBorder textBoxCurrentLongitude;
        private ExtendedControls.LabelExt labelTargetLat;
        private ExtendedControls.TextBoxBorder textBoxTargetLatitude;
        private ExtendedControls.LabelExt labelExtTargetLong;
        private ExtendedControls.TextBoxBorder textBoxTargetLongitude;
        private ExtendedControls.LabelExt labelExtBearing;
    }
}
