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
    partial class UserControlScreenshot
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extCheckBrowse = new ExtendedControls.ExtCheckBox();
            this.extButtonSelect = new ExtendedControls.ExtButton();
            this.extButtonCopy = new ExtendedControls.ExtButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(100, 90);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(448, 293);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 10;
            this.pictureBox.TabStop = false;
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoHeight = false;
            this.rollUpPanelTop.AutoHeightWidthDisable = false;
            this.rollUpPanelTop.AutoSize = true;
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(810, 30);
            this.rollUpPanelTop.TabIndex = 11;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extCheckBrowse);
            this.panelControls.Controls.Add(this.extButtonSelect);
            this.panelControls.Controls.Add(this.extButtonCopy);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(810, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extCheckBrowse
            // 
            this.extCheckBrowse.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBrowse.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBrowse.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBrowse.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBrowse.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBrowse.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBrowse.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBrowse.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBrowse.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBrowse.Image = global::EDDiscovery.Icons.Controls.Scan_Star;
            this.extCheckBrowse.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBrowse.ImageIndeterminate = null;
            this.extCheckBrowse.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBrowse.ImageUnchecked = null;
            this.extCheckBrowse.Location = new System.Drawing.Point(0, 1);
            this.extCheckBrowse.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            this.extCheckBrowse.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBrowse.Name = "extCheckBrowse";
            this.extCheckBrowse.Size = new System.Drawing.Size(28, 28);
            this.extCheckBrowse.TabIndex = 2;
            this.extCheckBrowse.TickBoxReductionRatio = 0.75F;
            this.extCheckBrowse.UseVisualStyleBackColor = false;
            // 
            // extButtonSelect
            // 
            this.extButtonSelect.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSelect.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.extButtonSelect.Location = new System.Drawing.Point(40, 1);
            this.extButtonSelect.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonSelect.Name = "extButtonSelect";
            this.extButtonSelect.Size = new System.Drawing.Size(28, 28);
            this.extButtonSelect.TabIndex = 29;
            this.extButtonSelect.UseVisualStyleBackColor = false;
            // 
            // extButtonCopy
            // 
            this.extButtonCopy.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonCopy.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonCopy.Location = new System.Drawing.Point(80, 1);
            this.extButtonCopy.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonCopy.Name = "extButtonCopy";
            this.extButtonCopy.Size = new System.Drawing.Size(28, 28);
            this.extButtonCopy.TabIndex = 29;
            this.extButtonCopy.UseVisualStyleBackColor = false;
            // 
            // UserControlScreenshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rollUpPanelTop);
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlScreenshot";
            this.Size = new System.Drawing.Size(810, 574);
            this.Resize += new System.EventHandler(this.UserControlScreenshot_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtCheckBox extCheckBrowse;
        private ExtendedControls.ExtButton extButtonSelect;
        private ExtendedControls.ExtButton extButtonCopy;
    }
}
