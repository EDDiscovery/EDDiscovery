﻿/*
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
    partial class UserControlEngineers
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
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonFilterEngineer = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxMoreInfo = new ExtendedControls.ExtCheckBox();
            this.panelEngineers = new ExtendedControls.ExtPanelScroll();
            this.extScrollBar1 = new ExtendedControls.ExtScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop.SuspendLayout();
            this.panelEngineers.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.buttonFilterEngineer);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Controls.Add(this.extCheckBoxMoreInfo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(750, 30);
            this.panelTop.TabIndex = 3;
            // 
            // buttonFilterEngineer
            // 
            this.buttonFilterEngineer.Image = global::EDDiscovery.Icons.Controls.EngineerCraft;
            this.buttonFilterEngineer.Location = new System.Drawing.Point(8, 1);
            this.buttonFilterEngineer.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterEngineer.Name = "buttonFilterEngineer";
            this.buttonFilterEngineer.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterEngineer.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonFilterEngineer, "Select which engineers are shown");
            this.buttonFilterEngineer.UseVisualStyleBackColor = true;
            this.buttonFilterEngineer.Click += new System.EventHandler(this.buttonFilterEngineer_Click);
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(48, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 31;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Turn on and off word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extCheckBoxMoreInfo
            // 
            this.extCheckBoxMoreInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxMoreInfo.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxMoreInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxMoreInfo.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxMoreInfo.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxMoreInfo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxMoreInfo.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxMoreInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxMoreInfo.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxMoreInfo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxMoreInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxMoreInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxMoreInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxMoreInfo.Image = global::EDDiscovery.Icons.Controls.RoundExpanded;
            this.extCheckBoxMoreInfo.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxMoreInfo.ImageIndeterminate = null;
            this.extCheckBoxMoreInfo.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxMoreInfo.ImageUnchecked = global::EDDiscovery.Icons.Controls.RoundExpand;
            this.extCheckBoxMoreInfo.Location = new System.Drawing.Point(88, 1);
            this.extCheckBoxMoreInfo.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxMoreInfo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxMoreInfo.Name = "extCheckBoxMoreInfo";
            this.extCheckBoxMoreInfo.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxMoreInfo.TabIndex = 31;
            this.extCheckBoxMoreInfo.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxMoreInfo, "Increase size of engineers grid and show more engineer details");
            this.extCheckBoxMoreInfo.UseVisualStyleBackColor = false;
            // 
            // panelEngineers
            // 
            this.panelEngineers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEngineers.Controls.Add(this.extScrollBar1);
            this.panelEngineers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEngineers.FlowControlsLeftToRight = false;
            this.panelEngineers.Location = new System.Drawing.Point(0, 30);
            this.panelEngineers.Name = "panelEngineers";
            this.panelEngineers.Size = new System.Drawing.Size(750, 490);
            this.panelEngineers.TabIndex = 4;
            this.panelEngineers.VerticalScrollBarDockRight = true;
            // 
            // extScrollBar1
            // 
            this.extScrollBar1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar1.ArrowColorScaling = 0.5F;
            this.extScrollBar1.ArrowDownDrawAngle = 270F;
            this.extScrollBar1.ArrowUpDrawAngle = 90F;
            this.extScrollBar1.BorderColor = System.Drawing.Color.White;
            this.extScrollBar1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar1.HideScrollBar = false;
            this.extScrollBar1.LargeChange = 10;
            this.extScrollBar1.Location = new System.Drawing.Point(732, 0);
            this.extScrollBar1.Maximum = -478;
            this.extScrollBar1.Minimum = 0;
            this.extScrollBar1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar1.Name = "extScrollBar1";
            this.extScrollBar1.Size = new System.Drawing.Size(16, 488);
            this.extScrollBar1.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar1.SmallChange = 1;
            this.extScrollBar1.TabIndex = 0;
            this.extScrollBar1.Text = "extScrollBar1";
            this.extScrollBar1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar1.ThumbColorScaling = 0.5F;
            this.extScrollBar1.ThumbDrawAngle = 0F;
            this.extScrollBar1.Value = -478;
            this.extScrollBar1.ValueLimited = -478;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlEngineers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEngineers);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlEngineers";
            this.Size = new System.Drawing.Size(750, 520);
            this.panelTop.ResumeLayout(false);
            this.panelEngineers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtButton buttonFilterEngineer;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtPanelScroll panelEngineers;
        private ExtendedControls.ExtScrollBar extScrollBar1;
        private ExtendedControls.ExtCheckBox extCheckBoxMoreInfo;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
