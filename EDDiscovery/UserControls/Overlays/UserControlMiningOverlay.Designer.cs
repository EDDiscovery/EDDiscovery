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
            this.pictureBox = new ExtendedControls.ExtPictureBox();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.extCheckBoxZeroRefined = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.extComboBoxChartOptions = new ExtendedControls.ExtComboBox();
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
            // extCheckBoxZeroRefined
            // 
            this.extCheckBoxZeroRefined.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxZeroRefined.AutoSize = true;
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
            this.extCheckBoxZeroRefined.Size = new System.Drawing.Size(29, 23);
            this.extCheckBoxZeroRefined.TabIndex = 61;
            this.extCheckBoxZeroRefined.Text = ">0";
            this.extCheckBoxZeroRefined.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extCheckBoxZeroRefined.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxZeroRefined.UseVisualStyleBackColor = true;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(265, 3);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 60;
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
            this.extComboBoxChartOptions.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extComboBoxChartOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxChartOptions.Location = new System.Drawing.Point(4, 4);
            this.extComboBoxChartOptions.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxChartOptions.Name = "extComboBoxChartOptions";
            this.extComboBoxChartOptions.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extComboBoxChartOptions.ScrollBarColor = System.Drawing.Color.LightGray;
            this.extComboBoxChartOptions.SelectedIndex = -1;
            this.extComboBoxChartOptions.SelectedItem = null;
            this.extComboBoxChartOptions.SelectedValue = null;
            this.extComboBoxChartOptions.Size = new System.Drawing.Size(225, 21);
            this.extComboBoxChartOptions.TabIndex = 3;
            this.extComboBoxChartOptions.Text = "";
            this.extComboBoxChartOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxChartOptions.ValueMember = "";
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
            this.extPanelRollUp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPictureBox pictureBox;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private ExtendedControls.ExtComboBox extComboBoxChartOptions;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtCheckBox extCheckBoxZeroRefined;
    }
}
