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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlCompass));
            this.labelTargetLat = new ExtendedControls.ExtLabelBitmap();
            this.numberBoxTargetLatitude = new ExtendedControls.NumberBoxDouble();
            this.numberBoxTargetLongitude = new ExtendedControls.NumberBoxDouble();
            this.comboBoxBookmarks = new ExtendedControls.ExtComboBox();
            this.buttonNewBookmark = new ExtendedControls.ExtButton();
            this.compassControl = new ExtendedControls.CompassControl();
            this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonBlank = new ExtendedControls.ExtButton();
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTargetLat
            // 
            this.labelTargetLat.AutoSize = true;
            this.labelTargetLat.Location = new System.Drawing.Point(3, 6);
            this.labelTargetLat.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.labelTargetLat.Name = "labelTargetLat";
            this.labelTargetLat.Size = new System.Drawing.Size(38, 13);
            this.labelTargetLat.TabIndex = 4;
            this.labelTargetLat.Text = "Target";
            this.labelTargetLat.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // numberBoxTargetLatitude
            // 
            this.numberBoxTargetLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLatitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLatitude.ClearOnFirstChar = false;
            this.numberBoxTargetLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLatitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLatitude.EndButtonEnable = true;
            this.numberBoxTargetLatitude.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxTargetLatitude.EndButtonImage")));
            this.numberBoxTargetLatitude.EndButtonVisible = false;
            this.numberBoxTargetLatitude.Format = "N4";
            this.numberBoxTargetLatitude.InErrorCondition = false;
            this.numberBoxTargetLatitude.Location = new System.Drawing.Point(47, 6);
            this.numberBoxTargetLatitude.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.numberBoxTargetLatitude.Maximum = 90D;
            this.numberBoxTargetLatitude.Minimum = -90D;
            this.numberBoxTargetLatitude.Multiline = false;
            this.numberBoxTargetLatitude.Name = "numberBoxTargetLatitude";
            this.numberBoxTargetLatitude.ReadOnly = false;
            this.numberBoxTargetLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxTargetLatitude.SelectionLength = 0;
            this.numberBoxTargetLatitude.SelectionStart = 0;
            this.numberBoxTargetLatitude.Size = new System.Drawing.Size(54, 20);
            this.numberBoxTargetLatitude.TabIndex = 9;
            this.numberBoxTargetLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.numberBoxTargetLatitude, "Latitude of target position, blank for none");
            this.numberBoxTargetLatitude.Value = 0D;
            this.numberBoxTargetLatitude.WordWrap = true;
            // 
            // numberBoxTargetLongitude
            // 
            this.numberBoxTargetLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLongitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLongitude.ClearOnFirstChar = false;
            this.numberBoxTargetLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLongitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLongitude.EndButtonEnable = true;
            this.numberBoxTargetLongitude.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxTargetLongitude.EndButtonImage")));
            this.numberBoxTargetLongitude.EndButtonVisible = false;
            this.numberBoxTargetLongitude.Format = "N4";
            this.numberBoxTargetLongitude.InErrorCondition = false;
            this.numberBoxTargetLongitude.Location = new System.Drawing.Point(107, 6);
            this.numberBoxTargetLongitude.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.numberBoxTargetLongitude.Maximum = 180D;
            this.numberBoxTargetLongitude.Minimum = -180D;
            this.numberBoxTargetLongitude.Multiline = false;
            this.numberBoxTargetLongitude.Name = "numberBoxTargetLongitude";
            this.numberBoxTargetLongitude.ReadOnly = false;
            this.numberBoxTargetLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxTargetLongitude.SelectionLength = 0;
            this.numberBoxTargetLongitude.SelectionStart = 0;
            this.numberBoxTargetLongitude.Size = new System.Drawing.Size(54, 20);
            this.numberBoxTargetLongitude.TabIndex = 10;
            this.numberBoxTargetLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.numberBoxTargetLongitude, "Longitude of target position, blank for none");
            this.numberBoxTargetLongitude.Value = 0D;
            this.numberBoxTargetLongitude.WordWrap = true;
            // 
            // comboBoxBookmarks
            // 
            this.comboBoxBookmarks.BorderColor = System.Drawing.Color.White;
            this.comboBoxBookmarks.DataSource = null;
            this.comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxBookmarks.DisplayMember = "";
            this.comboBoxBookmarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxBookmarks.Location = new System.Drawing.Point(167, 6);
            this.comboBoxBookmarks.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.comboBoxBookmarks.Name = "comboBoxBookmarks";
            this.comboBoxBookmarks.SelectedIndex = -1;
            this.comboBoxBookmarks.SelectedItem = null;
            this.comboBoxBookmarks.SelectedValue = null;
            this.comboBoxBookmarks.Size = new System.Drawing.Size(247, 21);
            this.comboBoxBookmarks.TabIndex = 11;
            this.comboBoxBookmarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxBookmarks, "Select known body feature or select bookmark");
            this.comboBoxBookmarks.ValueMember = "";
            this.comboBoxBookmarks.SelectedIndexChanged += new System.EventHandler(this.comboBoxBookmarks_SelectedIndexChanged);
            // 
            // buttonNewBookmark
            // 
            this.buttonNewBookmark.Image = global::EDDiscovery.Icons.Controls.Bookmarks;
            this.buttonNewBookmark.Location = new System.Drawing.Point(454, 4);
            this.buttonNewBookmark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.buttonNewBookmark.Name = "buttonNewBookmark";
            this.buttonNewBookmark.Size = new System.Drawing.Size(28, 28);
            this.buttonNewBookmark.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonNewBookmark, "Edit or create bookmark on system");
            this.buttonNewBookmark.UseVisualStyleBackColor = true;
            this.buttonNewBookmark.Click += new System.EventHandler(this.buttonNewBookmark_Click);
            // 
            // compassControl
            // 
            this.compassControl.AutoSetStencilTicks = true;
            this.compassControl.BackColor = System.Drawing.Color.LightBlue;
            this.compassControl.Bearing = 0D;
            this.compassControl.Bug = double.NaN;
            this.compassControl.BugColor = System.Drawing.Color.White;
            this.compassControl.BugSizePixels = 10;
            this.compassControl.CentreTickColor = System.Drawing.Color.Green;
            this.compassControl.DisableMessage = "";
            this.compassControl.Distance = double.NaN;
            this.compassControl.DistanceFormat = "{0:0.##}";
            this.compassControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compassControl.GlideSlope = double.NaN;
            this.compassControl.Location = new System.Drawing.Point(0, 35);
            this.compassControl.Name = "compassControl";
            this.compassControl.ShowNegativeDegrees = false;
            this.compassControl.Size = new System.Drawing.Size(915, 285);
            this.compassControl.SlewRateDegreesSec = 40;
            this.compassControl.SlewToBearing = 0D;
            this.compassControl.StencilColor = System.Drawing.Color.Red;
            this.compassControl.StencilMajorTicksAt = 20;
            this.compassControl.StencilMinorTicksAt = 5;
            this.compassControl.TabIndex = 14;
            this.compassControl.TextBandRatioToFont = 1D;
            this.compassControl.WidthDegrees = 180;
            // 
            // flowLayoutPanelTop
            // 
            this.flowLayoutPanelTop.AutoSize = true;
            this.flowLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelTop.Controls.Add(this.labelTargetLat);
            this.flowLayoutPanelTop.Controls.Add(this.numberBoxTargetLatitude);
            this.flowLayoutPanelTop.Controls.Add(this.numberBoxTargetLongitude);
            this.flowLayoutPanelTop.Controls.Add(this.comboBoxBookmarks);
            this.flowLayoutPanelTop.Controls.Add(this.extButtonBlank);
            this.flowLayoutPanelTop.Controls.Add(this.buttonNewBookmark);
            this.flowLayoutPanelTop.Controls.Add(this.extButtonShowControl);
            this.flowLayoutPanelTop.Controls.Add(this.extButtonFont);
            this.flowLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
            this.flowLayoutPanelTop.Size = new System.Drawing.Size(915, 35);
            this.flowLayoutPanelTop.TabIndex = 15;
            this.flowLayoutPanelTop.WrapContents = false;
            // 
            // extButtonBlank
            // 
            this.extButtonBlank.Image = global::EDDiscovery.Icons.Controls.Cross;
            this.extButtonBlank.Location = new System.Drawing.Point(420, 4);
            this.extButtonBlank.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.extButtonBlank.Name = "extButtonBlank";
            this.extButtonBlank.Size = new System.Drawing.Size(28, 28);
            this.extButtonBlank.TabIndex = 13;
            this.toolTip.SetToolTip(this.extButtonBlank, "Clear target latitude and longitude and selection");
            this.extButtonBlank.UseVisualStyleBackColor = true;
            this.extButtonBlank.Click += new System.EventHandler(this.extButtonBlank_Click);
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(488, 4);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 31;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Configure compass overlay");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(522, 4);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonFont, "Configure font of compass");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.compassControl);
            this.Controls.Add(this.flowLayoutPanelTop);
            this.Name = "UserControlCompass";
            this.Size = new System.Drawing.Size(915, 320);
            this.flowLayoutPanelTop.ResumeLayout(false);
            this.flowLayoutPanelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtLabelBitmap labelTargetLat;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLatitude;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLongitude;
        private ExtendedControls.ExtComboBox comboBoxBookmarks;
        private ExtendedControls.ExtButton buttonNewBookmark;
        private ExtendedControls.CompassControl compassControl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtButton extButtonBlank;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
