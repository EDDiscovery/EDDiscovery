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
    partial class UserControlWebBrowser
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
            this.extCheckBoxStar = new ExtendedControls.ExtCheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxAutoTrack = new ExtendedControls.ExtCheckBox();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extCheckBoxBack = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxAllowedList = new ExtendedControls.ExtCheckBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // extCheckBoxStar
            // 
            this.extCheckBoxStar.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxStar.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxStar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxStar.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxStar.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxStar.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxStar.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxStar.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxStar.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxStar.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxStar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxStar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxStar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBoxStar.Image = global::EDDiscovery.Icons.Controls.Scan_Star;
            this.extCheckBoxStar.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxStar.ImageIndeterminate = null;
            this.extCheckBoxStar.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxStar.ImageUnchecked = null;
            this.extCheckBoxStar.Location = new System.Drawing.Point(32, 1);
            this.extCheckBoxStar.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            this.extCheckBoxStar.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxStar.Name = "extCheckBoxStar";
            this.extCheckBoxStar.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxStar.TabIndex = 2;
            this.extCheckBoxStar.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxStar, "Select another system to view");
            this.extCheckBoxStar.UseVisualStyleBackColor = false;
            this.extCheckBoxStar.Click += new System.EventHandler(this.extCheckBoxStar_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            // 
            // checkBoxAutoTrack
            // 
            this.checkBoxAutoTrack.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxAutoTrack.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxAutoTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxAutoTrack.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxAutoTrack.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxAutoTrack.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoTrack.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoTrack.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxAutoTrack.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxAutoTrack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxAutoTrack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxAutoTrack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxAutoTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxAutoTrack.Image = global::EDDiscovery.Icons.Controls.ImportSphere;
            this.checkBoxAutoTrack.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAutoTrack.ImageIndeterminate = null;
            this.checkBoxAutoTrack.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxAutoTrack.ImageUnchecked = null;
            this.checkBoxAutoTrack.Location = new System.Drawing.Point(72, 1);
            this.checkBoxAutoTrack.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.checkBoxAutoTrack.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoTrack.Name = "checkBoxAutoTrack";
            this.checkBoxAutoTrack.Size = new System.Drawing.Size(28, 28);
            this.checkBoxAutoTrack.TabIndex = 4;
            this.checkBoxAutoTrack.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxAutoTrack, "Track system in history panel");
            this.checkBoxAutoTrack.UseVisualStyleBackColor = false;
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoSize = true;
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(748, 30);
            this.rollUpPanelTop.TabIndex = 4;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extCheckBoxBack);
            this.panelControls.Controls.Add(this.extCheckBoxStar);
            this.panelControls.Controls.Add(this.checkBoxAutoTrack);
            this.panelControls.Controls.Add(this.extCheckBoxAllowedList);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(748, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extCheckBoxBack
            // 
            this.extCheckBoxBack.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxBack.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxBack.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxBack.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxBack.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxBack.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxBack.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxBack.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxBack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBoxBack.Image = global::EDDiscovery.Icons.Controls.backbutton;
            this.extCheckBoxBack.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxBack.ImageIndeterminate = null;
            this.extCheckBoxBack.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxBack.ImageUnchecked = null;
            this.extCheckBoxBack.Location = new System.Drawing.Point(0, 1);
            this.extCheckBoxBack.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            this.extCheckBoxBack.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxBack.Name = "extCheckBoxBack";
            this.extCheckBoxBack.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxBack.TabIndex = 2;
            this.extCheckBoxBack.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxBack, "Back");
            this.extCheckBoxBack.UseVisualStyleBackColor = false;
            this.extCheckBoxBack.Click += new System.EventHandler(this.extCheckBoxClickBack);
            // 
            // extCheckBoxAllowedList
            // 
            this.extCheckBoxAllowedList.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxAllowedList.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxAllowedList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxAllowedList.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxAllowedList.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxAllowedList.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxAllowedList.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxAllowedList.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxAllowedList.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxAllowedList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxAllowedList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxAllowedList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxAllowedList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxAllowedList.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extCheckBoxAllowedList.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxAllowedList.ImageIndeterminate = null;
            this.extCheckBoxAllowedList.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxAllowedList.ImageUnchecked = null;
            this.extCheckBoxAllowedList.Location = new System.Drawing.Point(112, 1);
            this.extCheckBoxAllowedList.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxAllowedList.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxAllowedList.Name = "extCheckBoxAllowedList";
            this.extCheckBoxAllowedList.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxAllowedList.TabIndex = 4;
            this.extCheckBoxAllowedList.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxAllowedList, "URLs");
            this.extCheckBoxAllowedList.UseVisualStyleBackColor = false;
            this.extCheckBoxAllowedList.Click += new System.EventHandler(this.extCheckBoxAllowedList_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 30);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(748, 652);
            this.webBrowser.TabIndex = 5;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            this.webBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser_NewWindow);
            // 
            // UserControlWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlWebBrowser";
            this.Size = new System.Drawing.Size(748, 682);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private ExtendedControls.ExtCheckBox extCheckBoxStar;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private System.Windows.Forms.WebBrowser webBrowser;
        private ExtendedControls.ExtCheckBox extCheckBoxBack;
        private ExtendedControls.ExtCheckBox checkBoxAutoTrack;
        private ExtendedControls.ExtCheckBox extCheckBoxAllowedList;
    }
}
