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
    partial class UserControlScan
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
            this.checkBoxEDSM = new ExtendedControls.ExtCheckBox();
            this.buttonSize = new ExtendedControls.ExtButton();
            this.extButtonHighValue = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonFilter = new ExtendedControls.ExtButton();
            this.extButtonDisplayFilters = new ExtendedControls.ExtButton();
            this.panelStars = new EDDiscovery.UserControls.ScanDisplayUserControl();
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
            this.extCheckBoxStar.Location = new System.Drawing.Point(0, 1);
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
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.Scan_FetchEDSMBodies;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.ImageIndeterminate = null;
            this.checkBoxEDSM.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.ImageUnchecked = null;
            this.checkBoxEDSM.Location = new System.Drawing.Point(160, 1);
            this.checkBoxEDSM.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(28, 28);
            this.checkBoxEDSM.TabIndex = 3;
            this.checkBoxEDSM.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "Show/Hide Body data from EDSM");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
            // 
            // buttonSize
            // 
            this.buttonSize.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.buttonSize.Location = new System.Drawing.Point(120, 1);
            this.buttonSize.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonSize.Name = "buttonSize";
            this.buttonSize.Size = new System.Drawing.Size(28, 28);
            this.buttonSize.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonSize, "Select image size");
            this.buttonSize.UseVisualStyleBackColor = false;
            this.buttonSize.Click += new System.EventHandler(this.buttonSize_Click);
            // 
            // extButtonHighValue
            // 
            this.extButtonHighValue.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonHighValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonHighValue.Image = global::EDDiscovery.Icons.Controls.Scan_Bodies_HighValue;
            this.extButtonHighValue.Location = new System.Drawing.Point(192, 1);
            this.extButtonHighValue.Margin = new System.Windows.Forms.Padding(0, 1, 4, 1);
            this.extButtonHighValue.Name = "extButtonHighValue";
            this.extButtonHighValue.Size = new System.Drawing.Size(28, 28);
            this.extButtonHighValue.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonHighValue, "Set High Value Limit");
            this.extButtonHighValue.UseVisualStyleBackColor = false;
            this.extButtonHighValue.Click += new System.EventHandler(this.extButtonHighValue_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(232, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = false;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
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
            this.panelControls.Controls.Add(this.extCheckBoxStar);
            this.panelControls.Controls.Add(this.extButtonFilter);
            this.panelControls.Controls.Add(this.extButtonDisplayFilters);
            this.panelControls.Controls.Add(this.buttonSize);
            this.panelControls.Controls.Add(this.checkBoxEDSM);
            this.panelControls.Controls.Add(this.extButtonHighValue);
            this.panelControls.Controls.Add(this.buttonExtExcel);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(748, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extButtonFilter
            // 
            this.extButtonFilter.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFilter.Image = global::EDDiscovery.Icons.Controls.TravelGrid_EventFilter;
            this.extButtonFilter.Location = new System.Drawing.Point(40, 1);
            this.extButtonFilter.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonFilter.Name = "extButtonFilter";
            this.extButtonFilter.Size = new System.Drawing.Size(28, 28);
            this.extButtonFilter.TabIndex = 29;
            this.extButtonFilter.UseVisualStyleBackColor = false;
            this.extButtonFilter.Click += new System.EventHandler(this.extButtonFilter_Click);
            // 
            // extButtonDisplayFilters
            // 
            this.extButtonDisplayFilters.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonDisplayFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonDisplayFilters.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonDisplayFilters.Location = new System.Drawing.Point(80, 1);
            this.extButtonDisplayFilters.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonDisplayFilters.Name = "extButtonDisplayFilters";
            this.extButtonDisplayFilters.Size = new System.Drawing.Size(28, 28);
            this.extButtonDisplayFilters.TabIndex = 29;
            this.extButtonDisplayFilters.UseVisualStyleBackColor = false;
            this.extButtonDisplayFilters.Click += new System.EventHandler(this.extButtonDisplayFilters_Click);
            // 
            // panelStars
            // 
            this.panelStars.CheckEDSM = false;
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.HideFullMaterials = false;
            this.panelStars.Location = new System.Drawing.Point(0, 30);
            this.panelStars.Name = "panelStars";
            this.panelStars.ShowAllG = true;
            this.panelStars.ShowDist = true;
            this.panelStars.ShowHabZone = true;
            this.panelStars.ShowMaterials = false;
            this.panelStars.ShowMoons = false;
            this.panelStars.ShowOnlyMaterialsRare = false;
            this.panelStars.ShowOverlays = false;
            this.panelStars.ShowPlanetClasses = true;
            this.panelStars.ShowStarClasses = true;
            this.panelStars.Size = new System.Drawing.Size(748, 652);
            this.panelStars.TabIndex = 5;
            this.panelStars.ValueLimit = 50000;
            
            // 
            // UserControlScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStars);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlScan";
            this.Size = new System.Drawing.Size(748, 682);
            this.Resize += new System.EventHandler(this.UserControlScan_Resize);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox checkBoxEDSM;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private ScanDisplayUserControl panelStars;
        private ExtendedControls.ExtButton buttonSize;
        private ExtendedControls.ExtButton extButtonHighValue;
        private ExtendedControls.ExtCheckBox extCheckBoxStar;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonFilter;
        private ExtendedControls.ExtButton extButtonDisplayFilters;
    }
}
