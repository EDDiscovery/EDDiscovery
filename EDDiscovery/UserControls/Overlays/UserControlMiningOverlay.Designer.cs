/*
 * Copyright © 2017 EDDiscovery development team
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
    partial class UserControlMiningOverlay
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
            this.pictureBox = new ExtendedControls.ExtPictureBox();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.extCheckBoxChartBase = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxZeroRefined = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.extComboBoxChartOptions = new ExtendedControls.ExtComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.extPanelRollUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox.Location = new System.Drawing.Point(0, 34);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(894, 353);
            this.pictureBox.TabIndex = 1;
            // 
            // extPanelRollUp
            // 
            this.extPanelRollUp.AutoHeight = false;
            this.extPanelRollUp.AutoHeightWidthDisable = false;
            this.extPanelRollUp.AutoWidth = false;
            this.extPanelRollUp.Controls.Add(this.extCheckBoxChartBase);
            this.extPanelRollUp.Controls.Add(this.extCheckBoxZeroRefined);
            this.extPanelRollUp.Controls.Add(this.buttonExtExcel);
            this.extPanelRollUp.Controls.Add(this.extComboBoxChartOptions);
            this.extPanelRollUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelRollUp.HiddenMarkerWidth = 40;
            this.extPanelRollUp.Location = new System.Drawing.Point(0, 0);
            this.extPanelRollUp.Name = "extPanelRollUp";
            this.extPanelRollUp.PinState = true;
            this.extPanelRollUp.RolledUpHeight = 5;
            this.extPanelRollUp.RollUpAnimationTime = 500;
            this.extPanelRollUp.RollUpDelay = 1000;
            this.extPanelRollUp.SecondHiddenMarkerWidth = 0;
            this.extPanelRollUp.ShowHiddenMarker = true;
            this.extPanelRollUp.Size = new System.Drawing.Size(894, 34);
            this.extPanelRollUp.TabIndex = 2;
            this.extPanelRollUp.UnrollHoverDelay = 1000;
            // 
            // extCheckBoxChartBase
            // 
            this.extCheckBoxChartBase.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxChartBase.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxChartBase.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxChartBase.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxChartBase.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxChartBase.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxChartBase.Image = global::EDDiscovery.Icons.Controls.AsteroidTotal;
            this.extCheckBoxChartBase.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxChartBase.ImageIndeterminate = null;
            this.extCheckBoxChartBase.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxChartBase.ImageUnchecked = null;
            this.extCheckBoxChartBase.Location = new System.Drawing.Point(268, 3);
            this.extCheckBoxChartBase.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxChartBase.Name = "extCheckBoxChartBase";
            this.extCheckBoxChartBase.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxChartBase.TabIndex = 62;
            this.extCheckBoxChartBase.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxChartBase, "Select chart between showing % chance for asteroids containing only this material" +
        ", and % chance over all asteroids");
            this.extCheckBoxChartBase.UseVisualStyleBackColor = false;
            // 
            // extCheckBoxZeroRefined
            // 
            this.extCheckBoxZeroRefined.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxZeroRefined.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxZeroRefined.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxZeroRefined.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxZeroRefined.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxZeroRefined.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxZeroRefined.ImageIndeterminate = null;
            this.extCheckBoxZeroRefined.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxZeroRefined.ImageUnchecked = null;
            this.extCheckBoxZeroRefined.Location = new System.Drawing.Point(235, 3);
            this.extCheckBoxZeroRefined.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxZeroRefined.Name = "extCheckBoxZeroRefined";
            this.extCheckBoxZeroRefined.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxZeroRefined.TabIndex = 61;
            this.extCheckBoxZeroRefined.Text = ">0";
            this.extCheckBoxZeroRefined.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extCheckBoxZeroRefined.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxZeroRefined, "Display items with zero refined items");
            this.extCheckBoxZeroRefined.UseVisualStyleBackColor = true;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(303, 3);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 60;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // extComboBoxChartOptions
            // 
            this.extComboBoxChartOptions.BorderColor = System.Drawing.Color.White;
            this.extComboBoxChartOptions.ButtonColorScaling = 0.5F;
            this.extComboBoxChartOptions.DataSource = null;
            this.extComboBoxChartOptions.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxChartOptions.DisplayMember = "";
            this.extComboBoxChartOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxChartOptions.Location = new System.Drawing.Point(4, 4);
            this.extComboBoxChartOptions.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxChartOptions.Name = "extComboBoxChartOptions";
            this.extComboBoxChartOptions.SelectedIndex = -1;
            this.extComboBoxChartOptions.SelectedItem = null;
            this.extComboBoxChartOptions.SelectedValue = null;
            this.extComboBoxChartOptions.Size = new System.Drawing.Size(225, 21);
            this.extComboBoxChartOptions.TabIndex = 3;
            this.extComboBoxChartOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.extComboBoxChartOptions, "Select chart options");
            this.extComboBoxChartOptions.ValueMember = "";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlMiningOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.extPanelRollUp);
            this.Name = "UserControlMiningOverlay";
            this.Size = new System.Drawing.Size(894, 946);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.extPanelRollUp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPictureBox pictureBox;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private ExtendedControls.ExtComboBox extComboBoxChartOptions;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtCheckBox extCheckBoxZeroRefined;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox extCheckBoxChartBase;
    }
}
