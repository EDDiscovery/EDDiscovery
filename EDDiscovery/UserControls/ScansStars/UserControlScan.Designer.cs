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
            EliteDangerousCore.SystemDisplay systemDisplay1 = new EliteDangerousCore.SystemDisplay();
            this.extCheckBoxStar = new ExtendedControls.ExtCheckBox();
            this.buttonSize = new ExtendedControls.ExtButton();
            this.extButtonHighValue = new ExtendedControls.ExtButton();
            this.extButtonNewBookmark = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.scanDisplayBodyFiltersButton = new EDDiscovery.UserControls.ScanDisplayBodyFiltersButton();
            this.scanDisplayConfigureButton = new EDDiscovery.UserControls.ScanDisplayConfigureButton();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
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
            this.extCheckBoxStar.Location = new System.Drawing.Point(4, 1);
            this.extCheckBoxStar.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.extCheckBoxStar.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxStar.Name = "extCheckBoxStar";
            this.extCheckBoxStar.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxStar.TabIndex = 2;
            this.extCheckBoxStar.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxStar, "Select another system to view");
            this.extCheckBoxStar.UseVisualStyleBackColor = false;
            this.extCheckBoxStar.Click += new System.EventHandler(this.extCheckBoxStar_Click);
            // 
            // buttonSize
            // 
            this.buttonSize.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.buttonSize.Location = new System.Drawing.Point(112, 1);
            this.buttonSize.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
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
            this.extButtonHighValue.Location = new System.Drawing.Point(148, 1);
            this.extButtonHighValue.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.extButtonHighValue.Name = "extButtonHighValue";
            this.extButtonHighValue.Size = new System.Drawing.Size(28, 28);
            this.extButtonHighValue.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonHighValue, "Set High Value Limit");
            this.extButtonHighValue.UseVisualStyleBackColor = false;
            this.extButtonHighValue.Click += new System.EventHandler(this.extButtonHighValue_Click);
            // 
            // extButtonNewBookmark
            // 
            this.extButtonNewBookmark.Image = global::EDDiscovery.Icons.Controls.Bookmarks;
            this.extButtonNewBookmark.Location = new System.Drawing.Point(111, 3);
            this.extButtonNewBookmark.Name = "extButtonNewBookmark";
            this.extButtonNewBookmark.Size = new System.Drawing.Size(28, 28);
            this.extButtonNewBookmark.TabIndex = 38;
            this.toolTip.SetToolTip(this.extButtonNewBookmark, "Create a new bookmark for the current system");
            this.extButtonNewBookmark.Click += new System.EventHandler(this.extButtonNewBookmark_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(184, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export");
            this.buttonExtExcel.UseVisualStyleBackColor = false;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // scanDisplayBodyFiltersButton
            // 
            this.scanDisplayBodyFiltersButton.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.scanDisplayBodyFiltersButton.Location = new System.Drawing.Point(40, 1);
            this.scanDisplayBodyFiltersButton.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.scanDisplayBodyFiltersButton.Name = "scanDisplayBodyFiltersButton";
            this.scanDisplayBodyFiltersButton.SettingsSplittingChar = ';';
            this.scanDisplayBodyFiltersButton.Size = new System.Drawing.Size(28, 28);
            this.scanDisplayBodyFiltersButton.TabIndex = 32;
            this.toolTip.SetToolTip(this.scanDisplayBodyFiltersButton, "Configure scan display body filters");
            this.scanDisplayBodyFiltersButton.UseVisualStyleBackColor = true;
            // 
            // scanDisplayConfigureButton
            // 
            this.scanDisplayConfigureButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.scanDisplayConfigureButton.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.scanDisplayConfigureButton.Location = new System.Drawing.Point(76, 1);
            this.scanDisplayConfigureButton.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.scanDisplayConfigureButton.Name = "scanDisplayConfigureButton";
            this.scanDisplayConfigureButton.SettingsSplittingChar = ';';
            this.scanDisplayConfigureButton.Size = new System.Drawing.Size(28, 28);
            this.scanDisplayConfigureButton.TabIndex = 31;
            this.toolTip.SetToolTip(this.scanDisplayConfigureButton, "Configure scan display");
            this.scanDisplayConfigureButton.UseVisualStyleBackColor = true;
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(748, 34);
            this.rollUpPanelTop.TabIndex = 4;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extCheckBoxStar);
            this.panelControls.Controls.Add(this.scanDisplayBodyFiltersButton);
            this.panelControls.Controls.Add(this.scanDisplayConfigureButton);
            this.panelControls.Controls.Add(this.buttonSize);
            this.panelControls.Controls.Add(this.extButtonHighValue);
            this.panelControls.Controls.Add(this.buttonExtExcel);
            this.panelControls.Controls.Add(this.edsmSpanshButton);
            this.panelControls.Controls.Add(this.extButtonNewBookmark);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(748, 34);
            this.panelControls.TabIndex = 32;
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(220, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 30;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // panelStars
            // 
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.Location = new System.Drawing.Point(0, 34);
            this.panelStars.Name = "panelStars";
            this.panelStars.Size = new System.Drawing.Size(748, 648);
            systemDisplay1.BackColor = System.Drawing.Color.Black;
            systemDisplay1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            systemDisplay1.FontUnderlined = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            systemDisplay1.HideFullMaterials = false;
            systemDisplay1.LabelColor = System.Drawing.Color.DarkOrange;
            systemDisplay1.LargerFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            systemDisplay1.NoPlanetStarsOnSameLine = true;
            systemDisplay1.ShowAllG = true;
            systemDisplay1.ShowDist = true;
            systemDisplay1.ShowHabZone = true;
            systemDisplay1.ShowMaterials = true;
            systemDisplay1.ShowMoons = true;
            systemDisplay1.ShowOnlyMaterialsRare = false;
            systemDisplay1.ShowOverlays = true;
            systemDisplay1.ShowPlanetClasses = true;
            systemDisplay1.ShowPlanetMass = true;
            systemDisplay1.ShowStarAge = true;
            systemDisplay1.ShowStarClasses = true;
            systemDisplay1.ShowStarMass = true;
            systemDisplay1.ShowWebBodies = false;
            systemDisplay1.ValueLimit = 50000;
            this.panelStars.SystemDisplay = systemDisplay1;
            this.panelStars.TabIndex = 5;
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
        private ExtendedControls.ExtButton extButtonNewBookmark;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private ScanDisplayUserControl panelStars;
        private ExtendedControls.ExtButton buttonSize;
        private ExtendedControls.ExtButton extButtonHighValue;
        private ExtendedControls.ExtCheckBox extCheckBoxStar;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private EDSMSpanshButton edsmSpanshButton;
        private ScanDisplayConfigureButton scanDisplayConfigureButton;
        private ScanDisplayBodyFiltersButton scanDisplayBodyFiltersButton;
    }
}
