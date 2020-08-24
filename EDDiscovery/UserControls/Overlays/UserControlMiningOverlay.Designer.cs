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
            this.extPanelRollUp1 = new ExtendedControls.ExtPanelRollUp();
            this.extComboBoxChartOptions = new ExtendedControls.ExtComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.extPanelRollUp1.SuspendLayout();
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
            // extPanelRollUp1
            // 
            this.extPanelRollUp1.Controls.Add(this.extComboBoxChartOptions);
            this.extPanelRollUp1.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelRollUp1.HiddenMarkerWidth = 40;
            this.extPanelRollUp1.Location = new System.Drawing.Point(0, 0);
            this.extPanelRollUp1.Name = "extPanelRollUp1";
            this.extPanelRollUp1.PinState = true;
            this.extPanelRollUp1.RolledUpHeight = 5;
            this.extPanelRollUp1.RollUpAnimationTime = 500;
            this.extPanelRollUp1.RollUpDelay = 1000;
            this.extPanelRollUp1.SecondHiddenMarkerWidth = 0;
            this.extPanelRollUp1.ShowHiddenMarker = true;
            this.extPanelRollUp1.Size = new System.Drawing.Size(894, 34);
            this.extPanelRollUp1.TabIndex = 2;
            this.extPanelRollUp1.UnrollHoverDelay = 1000;
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
            this.extComboBoxChartOptions.Text = "extComboBox1";
            this.extComboBoxChartOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxChartOptions.ValueMember = "";
            // 
            // UserControlMiningOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.extPanelRollUp1);
            this.Name = "UserControlMiningOverlay";
            this.Size = new System.Drawing.Size(894, 946);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.extPanelRollUp1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPictureBox pictureBox;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp1;
        private ExtendedControls.ExtComboBox extComboBoxChartOptions;
    }
}
