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
    partial class UserControlSurveyor
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
            this.extButtonPlanets = new ExtendedControls.ExtButton();
            this.extButtonStars = new ExtendedControls.ExtButton();
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extPictureBoxScrollSystemDetails = new ExtendedControls.ExtPictureBoxScroll();
            this.extScrollBarSystemDetails = new ExtendedControls.ExtScrollBar();
            this.extPictureBoxSystemDetails = new ExtendedControls.ExtPictureBox();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonSearches = new ExtendedControls.ExtButton();
            this.extButtonAlignment = new ExtendedControls.ExtButton();
            this.extButtonFSS = new ExtendedControls.ExtButton();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.extButtonSetRoute = new ExtendedControls.ExtButton();
            this.extButtonControlRoute = new ExtendedControls.ExtButton();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extPictureBoxTitle = new ExtendedControls.ExtPictureBox();
            this.extPictureBoxRoute = new ExtendedControls.ExtPictureBox();
            this.extPictureBoxFuel = new ExtendedControls.ExtPictureBox();
            this.extPictureBoxScanSummary = new ExtendedControls.ExtPictureBox();
            this.extPictureBoxTarget = new ExtendedControls.ExtPictureBox();
            this.extPictureBoxScrollSystemDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxSystemDetails)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxRoute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxFuel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxScanSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // extButtonPlanets
            // 
            this.extButtonPlanets.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonPlanets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonPlanets.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.extButtonPlanets.Location = new System.Drawing.Point(8, 1);
            this.extButtonPlanets.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonPlanets.Name = "extButtonPlanets";
            this.extButtonPlanets.Size = new System.Drawing.Size(28, 28);
            this.extButtonPlanets.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonPlanets, "Planet Filter");
            this.extButtonPlanets.UseVisualStyleBackColor = false;
            this.extButtonPlanets.Click += new System.EventHandler(this.extButtonPlanets_Click);
            // 
            // extButtonStars
            // 
            this.extButtonStars.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonStars.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonStars.Image = global::EDDiscovery.Icons.Controls.Scan_Star;
            this.extButtonStars.Location = new System.Drawing.Point(88, 1);
            this.extButtonStars.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonStars.Name = "extButtonStars";
            this.extButtonStars.Size = new System.Drawing.Size(28, 28);
            this.extButtonStars.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonStars, "Star Filter");
            this.extButtonStars.UseVisualStyleBackColor = false;
            this.extButtonStars.Click += new System.EventHandler(this.extButtonStars_Click);
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(128, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Display Settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(408, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 34;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Word Wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extPictureBoxScrollSystemDetails
            // 
            this.extPictureBoxScrollSystemDetails.Controls.Add(this.extScrollBarSystemDetails);
            this.extPictureBoxScrollSystemDetails.Controls.Add(this.extPictureBoxSystemDetails);
            this.extPictureBoxScrollSystemDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPictureBoxScrollSystemDetails.Location = new System.Drawing.Point(0, 150);
            this.extPictureBoxScrollSystemDetails.Name = "extPictureBoxScrollSystemDetails";
            this.extPictureBoxScrollSystemDetails.ScrollBarEnabled = true;
            this.extPictureBoxScrollSystemDetails.Size = new System.Drawing.Size(721, 253);
            this.extPictureBoxScrollSystemDetails.TabIndex = 1;
            this.extPictureBoxScrollSystemDetails.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarSystemDetails
            // 
            this.extScrollBarSystemDetails.AlwaysHideScrollBar = false;
            this.extScrollBarSystemDetails.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarSystemDetails.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarSystemDetails.ArrowDownDrawAngle = 270F;
            this.extScrollBarSystemDetails.ArrowUpDrawAngle = 90F;
            this.extScrollBarSystemDetails.BorderColor = System.Drawing.Color.White;
            this.extScrollBarSystemDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarSystemDetails.HideScrollBar = true;
            this.extScrollBarSystemDetails.LargeChange = 253;
            this.extScrollBarSystemDetails.Location = new System.Drawing.Point(705, 0);
            this.extScrollBarSystemDetails.Maximum = 199;
            this.extScrollBarSystemDetails.Minimum = 0;
            this.extScrollBarSystemDetails.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarSystemDetails.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarSystemDetails.Name = "extScrollBarSystemDetails";
            this.extScrollBarSystemDetails.Size = new System.Drawing.Size(16, 253);
            this.extScrollBarSystemDetails.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarSystemDetails.SmallChange = 16;
            this.extScrollBarSystemDetails.TabIndex = 1;
            this.extScrollBarSystemDetails.Text = "extScrollBar1";
            this.extScrollBarSystemDetails.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarSystemDetails.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarSystemDetails.ThumbDrawAngle = 0F;
            this.extScrollBarSystemDetails.Value = 0;
            this.extScrollBarSystemDetails.ValueLimited = 0;
            // 
            // extPictureBoxSystemDetails
            // 
            this.extPictureBoxSystemDetails.Location = new System.Drawing.Point(0, 0);
            this.extPictureBoxSystemDetails.Name = "extPictureBoxSystemDetails";
            this.extPictureBoxSystemDetails.Size = new System.Drawing.Size(705, 200);
            this.extPictureBoxSystemDetails.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxSystemDetails.TabIndex = 0;
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoHeight = true;
            this.rollUpPanelTop.AutoHeightWidthDisable = false;
            this.rollUpPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rollUpPanelTop.AutoWidth = false;
            this.rollUpPanelTop.Controls.Add(this.panelControls);
            this.rollUpPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelTop.HiddenMarkerWidth = 400;
            this.rollUpPanelTop.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelTop.Name = "rollUpPanelTop";
            this.rollUpPanelTop.PinState = true;
            this.rollUpPanelTop.RolledUpHeight = 5;
            this.rollUpPanelTop.RollUpAnimationTime = 500;
            this.rollUpPanelTop.RollUpDelay = 1000;
            this.rollUpPanelTop.SecondHiddenMarkerWidth = 0;
            this.rollUpPanelTop.ShowHiddenMarker = true;
            this.rollUpPanelTop.Size = new System.Drawing.Size(721, 30);
            this.rollUpPanelTop.TabIndex = 15;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonPlanets);
            this.panelControls.Controls.Add(this.extButtonSearches);
            this.panelControls.Controls.Add(this.extButtonStars);
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonAlignment);
            this.panelControls.Controls.Add(this.extButtonFSS);
            this.panelControls.Controls.Add(this.edsmSpanshButton);
            this.panelControls.Controls.Add(this.extButtonSetRoute);
            this.panelControls.Controls.Add(this.extButtonControlRoute);
            this.panelControls.Controls.Add(this.extButtonFont);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(721, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extButtonSearches
            // 
            this.extButtonSearches.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSearches.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSearches.Image = global::EDDiscovery.Icons.Controls.Find;
            this.extButtonSearches.Location = new System.Drawing.Point(48, 1);
            this.extButtonSearches.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonSearches.Name = "extButtonSearches";
            this.extButtonSearches.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearches.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonSearches, "Select discoveries to search for, taken from the search panel scan searches");
            this.extButtonSearches.UseVisualStyleBackColor = false;
            this.extButtonSearches.Click += new System.EventHandler(this.extButtonSearches_Click);
            // 
            // extButtonAlignment
            // 
            this.extButtonAlignment.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonAlignment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonAlignment.Image = global::EDDiscovery.Icons.Controls.AlignCentre;
            this.extButtonAlignment.Location = new System.Drawing.Point(168, 1);
            this.extButtonAlignment.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonAlignment.Name = "extButtonAlignment";
            this.extButtonAlignment.Size = new System.Drawing.Size(28, 28);
            this.extButtonAlignment.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonAlignment, "Alignment");
            this.extButtonAlignment.UseVisualStyleBackColor = false;
            this.extButtonAlignment.Click += new System.EventHandler(this.extButtonAlignment_Click);
            // 
            // extButtonFSS
            // 
            this.extButtonFSS.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFSS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFSS.Image = global::EDDiscovery.Icons.Controls.FSSBodySignals;
            this.extButtonFSS.Location = new System.Drawing.Point(208, 1);
            this.extButtonFSS.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonFSS.Name = "extButtonFSS";
            this.extButtonFSS.Size = new System.Drawing.Size(28, 28);
            this.extButtonFSS.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonFSS, "FSS Signal Selection");
            this.extButtonFSS.UseVisualStyleBackColor = false;
            this.extButtonFSS.Click += new System.EventHandler(this.extButtonFSS_Click);
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(248, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 35;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // extButtonSetRoute
            // 
            this.extButtonSetRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSetRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSetRoute.Image = global::EDDiscovery.Icons.Controls.Route;
            this.extButtonSetRoute.Location = new System.Drawing.Point(288, 1);
            this.extButtonSetRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonSetRoute.Name = "extButtonSetRoute";
            this.extButtonSetRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonSetRoute.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonSetRoute, "Select route to follow");
            this.extButtonSetRoute.UseVisualStyleBackColor = false;
            this.extButtonSetRoute.Click += new System.EventHandler(this.extButtonSetRoute_Click);
            // 
            // extButtonControlRoute
            // 
            this.extButtonControlRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonControlRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonControlRoute.Image = global::EDDiscovery.Icons.Controls.RouteConfig;
            this.extButtonControlRoute.Location = new System.Drawing.Point(328, 1);
            this.extButtonControlRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonControlRoute.Name = "extButtonControlRoute";
            this.extButtonControlRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonControlRoute.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonControlRoute, "Route Settings");
            this.extButtonControlRoute.UseVisualStyleBackColor = false;
            this.extButtonControlRoute.Click += new System.EventHandler(this.extButtonControlRoute_Click);
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(368, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonFont, "Font");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extPictureBoxTitle
            // 
            this.extPictureBoxTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxTitle.Location = new System.Drawing.Point(0, 30);
            this.extPictureBoxTitle.Name = "extPictureBoxTitle";
            this.extPictureBoxTitle.Size = new System.Drawing.Size(721, 24);
            this.extPictureBoxTitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxTitle.TabIndex = 2;
            // 
            // extPictureBoxRoute
            // 
            this.extPictureBoxRoute.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxRoute.Location = new System.Drawing.Point(0, 54);
            this.extPictureBoxRoute.Name = "extPictureBoxRoute";
            this.extPictureBoxRoute.Size = new System.Drawing.Size(721, 24);
            this.extPictureBoxRoute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxRoute.TabIndex = 16;
            // 
            // extPictureBoxFuel
            // 
            this.extPictureBoxFuel.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxFuel.Location = new System.Drawing.Point(0, 102);
            this.extPictureBoxFuel.Name = "extPictureBoxFuel";
            this.extPictureBoxFuel.Size = new System.Drawing.Size(721, 24);
            this.extPictureBoxFuel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxFuel.TabIndex = 17;
            // 
            // extPictureBoxScanSummary
            // 
            this.extPictureBoxScanSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxScanSummary.Location = new System.Drawing.Point(0, 126);
            this.extPictureBoxScanSummary.Name = "extPictureBoxScanSummary";
            this.extPictureBoxScanSummary.Size = new System.Drawing.Size(721, 24);
            this.extPictureBoxScanSummary.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxScanSummary.TabIndex = 18;
            // 
            // extPictureBoxTarget
            // 
            this.extPictureBoxTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxTarget.Location = new System.Drawing.Point(0, 78);
            this.extPictureBoxTarget.Name = "extPictureBoxTarget";
            this.extPictureBoxTarget.Size = new System.Drawing.Size(721, 24);
            this.extPictureBoxTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.extPictureBoxTarget.TabIndex = 19;
            // 
            // UserControlSurveyor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPictureBoxScrollSystemDetails);
            this.Controls.Add(this.extPictureBoxScanSummary);
            this.Controls.Add(this.extPictureBoxFuel);
            this.Controls.Add(this.extPictureBoxTarget);
            this.Controls.Add(this.extPictureBoxRoute);
            this.Controls.Add(this.extPictureBoxTitle);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlSurveyor";
            this.Size = new System.Drawing.Size(721, 403);
            this.Resize += new System.EventHandler(this.UserControlSurveyor_Resize);
            this.extPictureBoxScrollSystemDetails.ResumeLayout(false);
            this.extPictureBoxScrollSystemDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxSystemDetails)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxRoute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxFuel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxScanSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxTarget)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPictureBox extPictureBoxSystemDetails;
        private ExtendedControls.ExtPictureBoxScroll extPictureBoxScrollSystemDetails;
        private ExtendedControls.ExtScrollBar extScrollBarSystemDetails;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonPlanets;
        private ExtendedControls.ExtButton extButtonStars;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtButton extButtonFSS;
        private ExtendedControls.ExtButton extButtonAlignment;
        private ExtendedControls.ExtButton extButtonSetRoute;
        private ExtendedControls.ExtButton extButtonControlRoute;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtButton extButtonSearches;
        private ExtendedControls.ExtPictureBox extPictureBoxTitle;
        private ExtendedControls.ExtPictureBox extPictureBoxRoute;
        private ExtendedControls.ExtPictureBox extPictureBoxFuel;
        private ExtendedControls.ExtPictureBox extPictureBoxScanSummary;
        private ExtendedControls.ExtPictureBox extPictureBoxTarget;
        private EDSMSpanshButton edsmSpanshButton;
    }
}
