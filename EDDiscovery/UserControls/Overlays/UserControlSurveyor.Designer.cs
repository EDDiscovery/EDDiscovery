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
            this.checkBoxEDSM = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extPictureBoxScroll = new ExtendedControls.ExtPictureBoxScroll();
            this.extScrollBar = new ExtendedControls.ExtScrollBar();
            this.pictureBoxSurveyor = new ExtendedControls.ExtPictureBox();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonAlignment = new ExtendedControls.ExtButton();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.extButtonFSS = new ExtendedControls.ExtButton();
            this.extButtonSetRoute = new ExtendedControls.ExtButton();
            this.extButtonControlRoute = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extPictureBoxScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyor)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
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
            this.extButtonStars.Location = new System.Drawing.Point(48, 1);
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
            this.extButtonShowControl.Location = new System.Drawing.Point(88, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Display Settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            // 
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.EDSM;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.ImageIndeterminate = null;
            this.checkBoxEDSM.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.ImageUnchecked = null;
            this.checkBoxEDSM.Location = new System.Drawing.Point(248, 1);
            this.checkBoxEDSM.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(28, 28);
            this.checkBoxEDSM.TabIndex = 33;
            this.checkBoxEDSM.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "EDSM lookup toggle");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(288, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 34;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Word Wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extPictureBoxScroll
            // 
            this.extPictureBoxScroll.Controls.Add(this.extScrollBar);
            this.extPictureBoxScroll.Controls.Add(this.pictureBoxSurveyor);
            this.extPictureBoxScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPictureBoxScroll.Location = new System.Drawing.Point(0, 30);
            this.extPictureBoxScroll.Name = "extPictureBoxScroll";
            this.extPictureBoxScroll.ScrollBarEnabled = true;
            this.extPictureBoxScroll.Size = new System.Drawing.Size(721, 373);
            this.extPictureBoxScroll.TabIndex = 1;
            this.extPictureBoxScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBar
            // 
            this.extScrollBar.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar.ArrowColorScaling = 0.5F;
            this.extScrollBar.ArrowDownDrawAngle = 270F;
            this.extScrollBar.ArrowUpDrawAngle = 90F;
            this.extScrollBar.BorderColor = System.Drawing.Color.White;
            this.extScrollBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar.HideScrollBar = true;
            this.extScrollBar.LargeChange = 373;
            this.extScrollBar.Location = new System.Drawing.Point(705, 0);
            this.extScrollBar.Maximum = 199;
            this.extScrollBar.Minimum = 0;
            this.extScrollBar.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar.Name = "extScrollBar";
            this.extScrollBar.Size = new System.Drawing.Size(16, 373);
            this.extScrollBar.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar.SmallChange = 16;
            this.extScrollBar.TabIndex = 1;
            this.extScrollBar.Text = "extScrollBar1";
            this.extScrollBar.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar.ThumbColorScaling = 0.5F;
            this.extScrollBar.ThumbDrawAngle = 0F;
            this.extScrollBar.Value = 0;
            this.extScrollBar.ValueLimited = 0;
            // 
            // pictureBoxSurveyor
            // 
            this.pictureBoxSurveyor.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSurveyor.Name = "pictureBoxSurveyor";
            this.pictureBoxSurveyor.Size = new System.Drawing.Size(705, 200);
            this.pictureBoxSurveyor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSurveyor.TabIndex = 0;
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
            this.panelControls.Controls.Add(this.extButtonStars);
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonAlignment);
            this.panelControls.Controls.Add(this.extButtonFont);
            this.panelControls.Controls.Add(this.extButtonFSS);
            this.panelControls.Controls.Add(this.checkBoxEDSM);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Controls.Add(this.extButtonSetRoute);
            this.panelControls.Controls.Add(this.extButtonControlRoute);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(721, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extButtonAlignment
            // 
            this.extButtonAlignment.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonAlignment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonAlignment.Image = global::EDDiscovery.Icons.Controls.AlignCentre;
            this.extButtonAlignment.Location = new System.Drawing.Point(128, 1);
            this.extButtonAlignment.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonAlignment.Name = "extButtonAlignment";
            this.extButtonAlignment.Size = new System.Drawing.Size(28, 28);
            this.extButtonAlignment.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonAlignment, "Alignment");
            this.extButtonAlignment.UseVisualStyleBackColor = false;
            this.extButtonAlignment.Click += new System.EventHandler(this.extButtonAlignment_Click);
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(168, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonFont, "Font");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
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
            // extButtonSetRoute
            // 
            this.extButtonSetRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSetRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSetRoute.Image = global::EDDiscovery.Icons.Controls.Route;
            this.extButtonSetRoute.Location = new System.Drawing.Point(328, 1);
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
            this.extButtonControlRoute.Location = new System.Drawing.Point(368, 1);
            this.extButtonControlRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonControlRoute.Name = "extButtonControlRoute";
            this.extButtonControlRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonControlRoute.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonControlRoute, "Route Settings");
            this.extButtonControlRoute.UseVisualStyleBackColor = false;
            this.extButtonControlRoute.Click += new System.EventHandler(this.extButtonControlRoute_Click);
            // 
            // UserControlSurveyor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPictureBoxScroll);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlSurveyor";
            this.Size = new System.Drawing.Size(721, 403);
            this.Resize += new System.EventHandler(this.UserControlSurveyor_Resize);
            this.extPictureBoxScroll.ResumeLayout(false);
            this.extPictureBoxScroll.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyor)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtPictureBox pictureBoxSurveyor;
        private ExtendedControls.ExtPictureBoxScroll extPictureBoxScroll;
        private ExtendedControls.ExtScrollBar extScrollBar;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonPlanets;
        private ExtendedControls.ExtButton extButtonStars;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtCheckBox checkBoxEDSM;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtButton extButtonFSS;
        private ExtendedControls.ExtButton extButtonAlignment;
        private ExtendedControls.ExtButton extButtonSetRoute;
        private ExtendedControls.ExtButton extButtonControlRoute;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton extButtonFont;
    }
}
