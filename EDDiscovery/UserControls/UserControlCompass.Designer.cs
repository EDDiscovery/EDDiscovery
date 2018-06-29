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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelTargetLat = new ExtendedControls.LabelExt();
            this.labelExtTargetLong = new ExtendedControls.LabelExt();
            this.checkBoxHideTransparent = new ExtendedControls.CheckBoxCustom();
            this.numberBoxTargetLatitude = new ExtendedControls.NumberBoxDouble();
            this.numberBoxTargetLongitude = new ExtendedControls.NumberBoxDouble();
            this.comboBoxBookmarks = new ExtendedControls.ComboBoxCustom();
            this.labelBookmark = new ExtendedControls.LabelExt();
            this.buttonNewBookmark = new ExtendedControls.ButtonExt();
            this.compassControl = new ExtendedControls.CompassControl();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // labelTargetLat
            // 
            this.labelTargetLat.AutoSize = true;
            this.labelTargetLat.Location = new System.Drawing.Point(8, 11);
            this.labelTargetLat.Name = "labelTargetLat";
            this.labelTargetLat.Size = new System.Drawing.Size(38, 13);
            this.labelTargetLat.TabIndex = 4;
            this.labelTargetLat.Text = "Target";
            this.labelTargetLat.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // labelExtTargetLong
            // 
            this.labelExtTargetLong.AutoSize = true;
            this.labelExtTargetLong.Location = new System.Drawing.Point(113, 7);
            this.labelExtTargetLong.Name = "labelExtTargetLong";
            this.labelExtTargetLong.Size = new System.Drawing.Size(10, 13);
            this.labelExtTargetLong.TabIndex = 6;
            this.labelExtTargetLong.Text = ",";
            this.labelExtTargetLong.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // checkBoxHideTransparent
            // 
            this.checkBoxHideTransparent.AutoSize = true;
            this.checkBoxHideTransparent.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHideTransparent.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHideTransparent.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHideTransparent.FontNerfReduction = 0.5F;
            this.checkBoxHideTransparent.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxHideTransparent.Location = new System.Drawing.Point(231, 9);
            this.checkBoxHideTransparent.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHideTransparent.Name = "checkBoxHideTransparent";
            this.checkBoxHideTransparent.Size = new System.Drawing.Size(120, 17);
            this.checkBoxHideTransparent.TabIndex = 8;
            this.checkBoxHideTransparent.Text = "Hide In Transparent";
            this.checkBoxHideTransparent.TickBoxReductionSize = 10;
            this.checkBoxHideTransparent.UseVisualStyleBackColor = true;
            this.checkBoxHideTransparent.CheckedChanged += new System.EventHandler(this.checkBoxHideTransparent_CheckedChanged);
            // 
            // numberBoxTargetLatitude
            // 
            this.numberBoxTargetLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLatitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLatitude.BorderColorScaling = 0.5F;
            this.numberBoxTargetLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLatitude.ClearOnFirstChar = false;
            this.numberBoxTargetLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLatitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLatitude.Format = "N4";
            this.numberBoxTargetLatitude.InErrorCondition = false;
            this.numberBoxTargetLatitude.Location = new System.Drawing.Point(52, 8);
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
            this.numberBoxTargetLatitude.Value = 0D;
            this.numberBoxTargetLatitude.WordWrap = true;
            this.numberBoxTargetLatitude.ValueChanged += new System.EventHandler(this.numberBoxTargetLatitude_ValueChanged);
            // 
            // numberBoxTargetLongitude
            // 
            this.numberBoxTargetLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxTargetLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxTargetLongitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxTargetLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxTargetLongitude.BorderColorScaling = 0.5F;
            this.numberBoxTargetLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxTargetLongitude.ClearOnFirstChar = false;
            this.numberBoxTargetLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxTargetLongitude.DelayBeforeNotification = 0;
            this.numberBoxTargetLongitude.Format = "N4";
            this.numberBoxTargetLongitude.InErrorCondition = false;
            this.numberBoxTargetLongitude.Location = new System.Drawing.Point(129, 7);
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
            this.numberBoxTargetLongitude.Value = 0D;
            this.numberBoxTargetLongitude.WordWrap = true;
            this.numberBoxTargetLongitude.ValueChanged += new System.EventHandler(this.numberBoxTargetLongitude_ValueChanged);
            // 
            // comboBoxBookmarks
            // 
            this.comboBoxBookmarks.ArrowWidth = 1;
            this.comboBoxBookmarks.BorderColor = System.Drawing.Color.White;
            this.comboBoxBookmarks.ButtonColorScaling = 0.5F;
            this.comboBoxBookmarks.DataSource = null;
            this.comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxBookmarks.DisplayMember = "";
            this.comboBoxBookmarks.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxBookmarks.DropDownHeight = 200;
            this.comboBoxBookmarks.DropDownWidth = 200;
            this.comboBoxBookmarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxBookmarks.ItemHeight = 13;
            this.comboBoxBookmarks.Location = new System.Drawing.Point(116, 34);
            this.comboBoxBookmarks.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxBookmarks.Name = "comboBoxBookmarks";
            this.comboBoxBookmarks.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxBookmarks.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxBookmarks.ScrollBarWidth = 16;
            this.comboBoxBookmarks.SelectedIndex = -1;
            this.comboBoxBookmarks.SelectedItem = null;
            this.comboBoxBookmarks.SelectedValue = null;
            this.comboBoxBookmarks.Size = new System.Drawing.Size(200, 21);
            this.comboBoxBookmarks.TabIndex = 11;
            this.comboBoxBookmarks.Text = "comboBoxCustom1";
            this.comboBoxBookmarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxBookmarks.ValueMember = "";
            this.comboBoxBookmarks.SelectedIndexChanged += new System.EventHandler(this.comboBoxBookmarks_SelectedIndexChanged);
            // 
            // labelBookmark
            // 
            this.labelBookmark.AutoSize = true;
            this.labelBookmark.Location = new System.Drawing.Point(9, 37);
            this.labelBookmark.Name = "labelBookmark";
            this.labelBookmark.Size = new System.Drawing.Size(97, 13);
            this.labelBookmark.TabIndex = 12;
            this.labelBookmark.Text = "System Bookmarks";
            this.labelBookmark.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // buttonNewBookmark
            // 
            this.buttonNewBookmark.Image = global::EDDiscovery.Icons.Controls.Map3D_Bookmarks_ShowBookmarks;
            this.buttonNewBookmark.Location = new System.Drawing.Point(189, 0);
            this.buttonNewBookmark.Name = "buttonNewBookmark";
            this.buttonNewBookmark.Size = new System.Drawing.Size(32, 32);
            this.buttonNewBookmark.TabIndex = 13;
            this.buttonNewBookmark.UseVisualStyleBackColor = true;
            this.buttonNewBookmark.Click += new System.EventHandler(this.buttonNewBookmark_Click);
            // 
            // compassControl
            // 
            this.compassControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.compassControl.AutoSetStencilTicks = false;
            this.compassControl.BackColor = System.Drawing.Color.LightBlue;
            this.compassControl.Bearing = 0D;
            this.compassControl.Bug = double.NaN;
            this.compassControl.BugColor = System.Drawing.Color.White;
            this.compassControl.BugSizePixels = 10;
            this.compassControl.CentreTickColor = System.Drawing.Color.Green;
            this.compassControl.CentreTickHeightPercentage = 60;
            this.compassControl.CompassHeightPercentage = 60;
            this.compassControl.DisableMessage = "";
            this.compassControl.Distance = double.NaN;
            this.compassControl.DistanceFormat = "{0:0.##}";
            this.compassControl.Location = new System.Drawing.Point(11, 66);
            this.compassControl.Name = "compassControl";
            this.compassControl.ShowNegativeDegrees = false;
            this.compassControl.Size = new System.Drawing.Size(340, 80);
            this.compassControl.SlewRateDegreesSec = 10;
            this.compassControl.SlewToBearing = 0D;
            this.compassControl.StencilColor = System.Drawing.Color.Red;
            this.compassControl.StencilMajorTicksAt = 20;
            this.compassControl.StencilMinorTicksAt = 5;
            this.compassControl.TabIndex = 14;
            this.compassControl.TickHeightPercentage = 60;
            this.compassControl.WidthDegrees = 180;
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.compassControl);
            this.Controls.Add(this.buttonNewBookmark);
            this.Controls.Add(this.labelBookmark);
            this.Controls.Add(this.comboBoxBookmarks);
            this.Controls.Add(this.numberBoxTargetLongitude);
            this.Controls.Add(this.numberBoxTargetLatitude);
            this.Controls.Add(this.checkBoxHideTransparent);
            this.Controls.Add(this.labelExtTargetLong);
            this.Controls.Add(this.labelTargetLat);
            this.Name = "UserControlCompass";
            this.Size = new System.Drawing.Size(361, 157);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.LabelExt labelTargetLat;
        private ExtendedControls.LabelExt labelExtTargetLong;
        private ExtendedControls.CheckBoxCustom checkBoxHideTransparent;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLatitude;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLongitude;
        private ExtendedControls.ComboBoxCustom comboBoxBookmarks;
        private ExtendedControls.LabelExt labelBookmark;
        private ExtendedControls.ButtonExt buttonNewBookmark;
        private ExtendedControls.CompassControl compassControl;
    }
}
