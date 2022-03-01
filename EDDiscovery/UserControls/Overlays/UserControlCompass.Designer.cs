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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlCompass));
            this.labelTargetLat = new ExtendedControls.ExtLabel();
            this.checkBoxHideTransparent = new ExtendedControls.ExtCheckBox();
            this.numberBoxTargetLatitude = new ExtendedControls.NumberBoxDouble();
            this.numberBoxTargetLongitude = new ExtendedControls.NumberBoxDouble();
            this.comboBoxBookmarks = new ExtendedControls.ExtComboBox();
            this.labelBookmark = new ExtendedControls.ExtLabel();
            this.buttonNewBookmark = new ExtendedControls.ExtButton();
            this.compassControl = new ExtendedControls.CompassControl();
            this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelBookmarks = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelTop.SuspendLayout();
            this.flowLayoutPanelBookmarks.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTargetLat
            // 
            this.labelTargetLat.AutoSize = true;
            this.labelTargetLat.Location = new System.Drawing.Point(3, 13);
            this.labelTargetLat.Margin = new System.Windows.Forms.Padding(3, 13, 3, 0);
            this.labelTargetLat.Name = "labelTargetLat";
            this.labelTargetLat.Size = new System.Drawing.Size(38, 13);
            this.labelTargetLat.TabIndex = 4;
            this.labelTargetLat.Text = "Target";
            this.labelTargetLat.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // checkBoxHideTransparent
            // 
            this.checkBoxHideTransparent.AutoSize = true;
            this.checkBoxHideTransparent.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHideTransparent.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxHideTransparent.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHideTransparent.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHideTransparent.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxHideTransparent.ImageIndeterminate = null;
            this.checkBoxHideTransparent.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxHideTransparent.ImageUnchecked = null;
            this.checkBoxHideTransparent.Location = new System.Drawing.Point(205, 10);
            this.checkBoxHideTransparent.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.checkBoxHideTransparent.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHideTransparent.Name = "checkBoxHideTransparent";
            this.checkBoxHideTransparent.Size = new System.Drawing.Size(120, 17);
            this.checkBoxHideTransparent.TabIndex = 8;
            this.checkBoxHideTransparent.Text = "Hide In Transparent";
            this.checkBoxHideTransparent.TickBoxReductionRatio = 0.75F;
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
            this.numberBoxTargetLatitude.EndButtonEnable = true;
            this.numberBoxTargetLatitude.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxTargetLatitude.EndButtonImage")));
            this.numberBoxTargetLatitude.EndButtonVisible = false;
            this.numberBoxTargetLatitude.Format = "N4";
            this.numberBoxTargetLatitude.InErrorCondition = false;
            this.numberBoxTargetLatitude.Location = new System.Drawing.Point(47, 10);
            this.numberBoxTargetLatitude.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
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
            this.numberBoxTargetLongitude.EndButtonEnable = true;
            this.numberBoxTargetLongitude.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxTargetLongitude.EndButtonImage")));
            this.numberBoxTargetLongitude.EndButtonVisible = false;
            this.numberBoxTargetLongitude.Format = "N4";
            this.numberBoxTargetLongitude.InErrorCondition = false;
            this.numberBoxTargetLongitude.Location = new System.Drawing.Point(107, 10);
            this.numberBoxTargetLongitude.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
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
            this.comboBoxBookmarks.BorderColor = System.Drawing.Color.White;
            this.comboBoxBookmarks.ButtonColorScaling = 0.5F;
            this.comboBoxBookmarks.DataSource = null;
            this.comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxBookmarks.DisplayMember = "";
            this.comboBoxBookmarks.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxBookmarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxBookmarks.Location = new System.Drawing.Point(106, 3);
            this.comboBoxBookmarks.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxBookmarks.Name = "comboBoxBookmarks";
            this.comboBoxBookmarks.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxBookmarks.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxBookmarks.SelectedIndex = -1;
            this.comboBoxBookmarks.SelectedItem = null;
            this.comboBoxBookmarks.SelectedValue = null;
            this.comboBoxBookmarks.Size = new System.Drawing.Size(247, 21);
            this.comboBoxBookmarks.TabIndex = 11;
            this.comboBoxBookmarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxBookmarks.ValueMember = "";
            this.comboBoxBookmarks.SelectedIndexChanged += new System.EventHandler(this.comboBoxBookmarks_SelectedIndexChanged);
            // 
            // labelBookmark
            // 
            this.labelBookmark.AutoSize = true;
            this.labelBookmark.Location = new System.Drawing.Point(3, 3);
            this.labelBookmark.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelBookmark.Name = "labelBookmark";
            this.labelBookmark.Size = new System.Drawing.Size(97, 13);
            this.labelBookmark.TabIndex = 12;
            this.labelBookmark.Text = "System Bookmarks";
            this.labelBookmark.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // buttonNewBookmark
            // 
            this.buttonNewBookmark.Image = global::EDDiscovery.Icons.Controls.Map3D_Bookmarks_ShowBookmarks;
            this.buttonNewBookmark.Location = new System.Drawing.Point(167, 3);
            this.buttonNewBookmark.Name = "buttonNewBookmark";
            this.buttonNewBookmark.Size = new System.Drawing.Size(32, 32);
            this.buttonNewBookmark.TabIndex = 13;
            this.buttonNewBookmark.UseVisualStyleBackColor = true;
            this.buttonNewBookmark.Click += new System.EventHandler(this.buttonNewBookmark_Click);
            // 
            // compassControl
            // 
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
            this.compassControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.compassControl.GlideSlope = double.NaN;
            this.compassControl.Location = new System.Drawing.Point(0, 65);
            this.compassControl.Name = "compassControl";
            this.compassControl.ShowNegativeDegrees = false;
            this.compassControl.Size = new System.Drawing.Size(670, 80);
            this.compassControl.SlewRateDegreesSec = 10;
            this.compassControl.SlewToBearing = 0D;
            this.compassControl.StencilColor = System.Drawing.Color.Red;
            this.compassControl.StencilMajorTicksAt = 20;
            this.compassControl.StencilMinorTicksAt = 5;
            this.compassControl.TabIndex = 14;
            this.compassControl.TickHeightPercentage = 60;
            this.compassControl.WidthDegrees = 180;
            // 
            // flowLayoutPanelTop
            // 
            this.flowLayoutPanelTop.AutoSize = true;
            this.flowLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelTop.Controls.Add(this.labelTargetLat);
            this.flowLayoutPanelTop.Controls.Add(this.numberBoxTargetLatitude);
            this.flowLayoutPanelTop.Controls.Add(this.numberBoxTargetLongitude);
            this.flowLayoutPanelTop.Controls.Add(this.buttonNewBookmark);
            this.flowLayoutPanelTop.Controls.Add(this.checkBoxHideTransparent);
            this.flowLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
            this.flowLayoutPanelTop.Size = new System.Drawing.Size(670, 38);
            this.flowLayoutPanelTop.TabIndex = 15;
            this.flowLayoutPanelTop.WrapContents = false;
            // 
            // flowLayoutPanelBookmarks
            // 
            this.flowLayoutPanelBookmarks.AutoSize = true;
            this.flowLayoutPanelBookmarks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelBookmarks.Controls.Add(this.labelBookmark);
            this.flowLayoutPanelBookmarks.Controls.Add(this.comboBoxBookmarks);
            this.flowLayoutPanelBookmarks.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelBookmarks.Location = new System.Drawing.Point(0, 38);
            this.flowLayoutPanelBookmarks.Name = "flowLayoutPanelBookmarks";
            this.flowLayoutPanelBookmarks.Size = new System.Drawing.Size(670, 27);
            this.flowLayoutPanelBookmarks.TabIndex = 16;
            this.flowLayoutPanelBookmarks.WrapContents = false;
            // 
            // UserControlCompass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.compassControl);
            this.Controls.Add(this.flowLayoutPanelBookmarks);
            this.Controls.Add(this.flowLayoutPanelTop);
            this.Name = "UserControlCompass";
            this.Size = new System.Drawing.Size(670, 448);
            this.flowLayoutPanelTop.ResumeLayout(false);
            this.flowLayoutPanelTop.PerformLayout();
            this.flowLayoutPanelBookmarks.ResumeLayout(false);
            this.flowLayoutPanelBookmarks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtLabel labelTargetLat;
        private ExtendedControls.ExtCheckBox checkBoxHideTransparent;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLatitude;
        private ExtendedControls.NumberBoxDouble numberBoxTargetLongitude;
        private ExtendedControls.ExtComboBox comboBoxBookmarks;
        private ExtendedControls.ExtLabel labelBookmark;
        private ExtendedControls.ExtButton buttonNewBookmark;
        private ExtendedControls.CompassControl compassControl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBookmarks;
    }
}
