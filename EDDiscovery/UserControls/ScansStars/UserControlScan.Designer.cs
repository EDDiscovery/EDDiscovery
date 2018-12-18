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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.showSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelShowSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSystemInfo = new System.Windows.Forms.Label();
            this.panelControls = new System.Windows.Forms.Panel();
            this.checkBoxCustomHideFullMats = new ExtendedControls.CheckBoxCustom();
            this.chkShowOverlays = new ExtendedControls.CheckBoxCustom();
            this.checkBoxTiny = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMaterials = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMaterialsRare = new ExtendedControls.CheckBoxCustom();
            this.checkBoxEDSM = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMoons = new ExtendedControls.CheckBoxCustom();
            this.checkBoxLarge = new ExtendedControls.CheckBoxCustom();
            this.checkBoxSmall = new ExtendedControls.CheckBoxCustom();
            this.checkBoxMedium = new ExtendedControls.CheckBoxCustom();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rollUpPanelTop = new ExtendedControls.RollUpPanel();
            this.panelStars = new EDDiscovery.UserControls.ScanDisplay();
            this.contextMenuStrip.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.rollUpPanelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemToolbar,
            this.showSystemToolStripMenuItem,
            this.cancelShowSystemToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(184, 70);
            // 
            // toolStripMenuItemToolbar
            // 
            this.toolStripMenuItemToolbar.Name = "toolStripMenuItemToolbar";
            this.toolStripMenuItemToolbar.Size = new System.Drawing.Size(183, 22);
            this.toolStripMenuItemToolbar.Text = "Show/Hide Toolbar";
            this.toolStripMenuItemToolbar.Click += new System.EventHandler(this.toolStripMenuItemToolbar_Click);
            // 
            // showSystemToolStripMenuItem
            // 
            this.showSystemToolStripMenuItem.Name = "showSystemToolStripMenuItem";
            this.showSystemToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.showSystemToolStripMenuItem.Text = "Show System";
            this.showSystemToolStripMenuItem.Click += new System.EventHandler(this.showSystemToolStripMenuItem_Click);
            // 
            // cancelShowSystemToolStripMenuItem
            // 
            this.cancelShowSystemToolStripMenuItem.Name = "cancelShowSystemToolStripMenuItem";
            this.cancelShowSystemToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.cancelShowSystemToolStripMenuItem.Text = "Cancel Show System";
            this.cancelShowSystemToolStripMenuItem.Click += new System.EventHandler(this.cancelShowSystemToolStripMenuItem_Click);
            // 
            // lblSystemInfo
            // 
            this.lblSystemInfo.AutoSize = true;
            this.lblSystemInfo.ContextMenuStrip = this.contextMenuStrip;
            this.lblSystemInfo.Location = new System.Drawing.Point(403, 10);
            this.lblSystemInfo.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblSystemInfo.Name = "lblSystemInfo";
            this.lblSystemInfo.Size = new System.Drawing.Size(43, 13);
            this.lblSystemInfo.TabIndex = 30;
            this.lblSystemInfo.Text = "<code>";
            this.toolTip.SetToolTip(this.lblSystemInfo, "Right click on panel to show/hide the toolbar");
            // 
            // panelControls
            // 
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.ContextMenuStrip = this.contextMenuStrip;
            this.panelControls.Controls.Add(this.checkBoxCustomHideFullMats);
            this.panelControls.Controls.Add(this.chkShowOverlays);
            this.panelControls.Controls.Add(this.checkBoxTiny);
            this.panelControls.Controls.Add(this.checkBoxMaterials);
            this.panelControls.Controls.Add(this.checkBoxMaterialsRare);
            this.panelControls.Controls.Add(this.checkBoxEDSM);
            this.panelControls.Controls.Add(this.checkBoxMoons);
            this.panelControls.Controls.Add(this.checkBoxLarge);
            this.panelControls.Controls.Add(this.checkBoxSmall);
            this.panelControls.Controls.Add(this.checkBoxMedium);
            this.panelControls.Controls.Add(this.buttonExtExcel);
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Margin = new System.Windows.Forms.Padding(0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(403, 35);
            this.panelControls.TabIndex = 5;
            this.toolTip.SetToolTip(this.panelControls, "Right click on panel to show/hide the toolbar");
            // 
            // checkBoxCustomHideFullMats
            // 
            this.checkBoxCustomHideFullMats.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCustomHideFullMats.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCustomHideFullMats.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCustomHideFullMats.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxCustomHideFullMats.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomHideFullMats.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomHideFullMats.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxCustomHideFullMats.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxCustomHideFullMats.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxCustomHideFullMats.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxCustomHideFullMats.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxCustomHideFullMats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCustomHideFullMats.FontNerfReduction = 0.5F;
            this.checkBoxCustomHideFullMats.Image = global::EDDiscovery.Icons.Controls.Scan_HideFullMaterials;
            this.checkBoxCustomHideFullMats.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomHideFullMats.Location = new System.Drawing.Point(64, 0);
            this.checkBoxCustomHideFullMats.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomHideFullMats.Name = "checkBoxCustomHideFullMats";
            this.checkBoxCustomHideFullMats.Size = new System.Drawing.Size(32, 32);
            this.checkBoxCustomHideFullMats.TabIndex = 31;
            this.checkBoxCustomHideFullMats.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCustomHideFullMats, "Show/Hide materials which have reached their storage limit");
            this.checkBoxCustomHideFullMats.UseVisualStyleBackColor = false;
            this.checkBoxCustomHideFullMats.CheckedChanged += new System.EventHandler(this.checkBoxCustomHideFullMats_CheckedChanged);
            // 
            // chkShowOverlays
            // 
            this.chkShowOverlays.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkShowOverlays.BackColor = System.Drawing.Color.Transparent;
            this.chkShowOverlays.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chkShowOverlays.CheckBoxColor = System.Drawing.Color.White;
            this.chkShowOverlays.CheckBoxInnerColor = System.Drawing.Color.White;
            this.chkShowOverlays.CheckColor = System.Drawing.Color.DarkBlue;
            this.chkShowOverlays.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkShowOverlays.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.chkShowOverlays.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.chkShowOverlays.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.chkShowOverlays.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.chkShowOverlays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkShowOverlays.FontNerfReduction = 0.5F;
            this.chkShowOverlays.Image = global::EDDiscovery.Icons.Controls.Scan_ShowOverlays;
            this.chkShowOverlays.ImageButtonDisabledScaling = 0.5F;
            this.chkShowOverlays.Location = new System.Drawing.Point(322, 0);
            this.chkShowOverlays.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.chkShowOverlays.Name = "chkShowOverlays";
            this.chkShowOverlays.Size = new System.Drawing.Size(32, 32);
            this.chkShowOverlays.TabIndex = 31;
            this.chkShowOverlays.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.chkShowOverlays, "Show/Hide body status icons");
            this.chkShowOverlays.UseVisualStyleBackColor = false;
            this.chkShowOverlays.CheckedChanged += new System.EventHandler(this.chkShowOverlays_CheckedChanged);
            // 
            // checkBoxTiny
            // 
            this.checkBoxTiny.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxTiny.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxTiny.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_SizeTiny;
            this.checkBoxTiny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxTiny.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxTiny.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxTiny.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxTiny.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxTiny.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxTiny.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxTiny.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxTiny.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxTiny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxTiny.FontNerfReduction = 0.5F;
            this.checkBoxTiny.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxTiny.Location = new System.Drawing.Point(243, 0);
            this.checkBoxTiny.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxTiny.Name = "checkBoxTiny";
            this.checkBoxTiny.Size = new System.Drawing.Size(32, 32);
            this.checkBoxTiny.TabIndex = 2;
            this.checkBoxTiny.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxTiny, "Image size teeny tiny");
            this.checkBoxTiny.UseVisualStyleBackColor = false;
            this.checkBoxTiny.CheckedChanged += new System.EventHandler(this.checkBoxTiny_CheckedChanged);
            // 
            // checkBoxMaterials
            // 
            this.checkBoxMaterials.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMaterials.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMaterials.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_ShowAllMaterials;
            this.checkBoxMaterials.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMaterials.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMaterials.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMaterials.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMaterials.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMaterials.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxMaterials.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxMaterials.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxMaterials.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxMaterials.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMaterials.FontNerfReduction = 0.5F;
            this.checkBoxMaterials.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMaterials.Location = new System.Drawing.Point(0, 0);
            this.checkBoxMaterials.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMaterials.Name = "checkBoxMaterials";
            this.checkBoxMaterials.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMaterials.TabIndex = 2;
            this.checkBoxMaterials.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMaterials, "Show/Hide Materials");
            this.checkBoxMaterials.UseVisualStyleBackColor = false;
            this.checkBoxMaterials.CheckedChanged += new System.EventHandler(this.checkBoxMaterials_CheckedChanged);
            // 
            // checkBoxMaterialsRare
            // 
            this.checkBoxMaterialsRare.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMaterialsRare.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMaterialsRare.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_ShowRareMaterials;
            this.checkBoxMaterialsRare.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMaterialsRare.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMaterialsRare.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMaterialsRare.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMaterialsRare.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMaterialsRare.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxMaterialsRare.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxMaterialsRare.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxMaterialsRare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxMaterialsRare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMaterialsRare.FontNerfReduction = 0.5F;
            this.checkBoxMaterialsRare.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMaterialsRare.Location = new System.Drawing.Point(32, 0);
            this.checkBoxMaterialsRare.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMaterialsRare.Name = "checkBoxMaterialsRare";
            this.checkBoxMaterialsRare.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMaterialsRare.TabIndex = 2;
            this.checkBoxMaterialsRare.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMaterialsRare, "Show rare materials only");
            this.checkBoxMaterialsRare.UseVisualStyleBackColor = false;
            this.checkBoxMaterialsRare.CheckedChanged += new System.EventHandler(this.checkBoxMaterialsRare_CheckedChanged);
            // 
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.FontNerfReduction = 0.5F;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.Scan_FetchEDSMBodies;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.Location = new System.Drawing.Point(284, 0);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(32, 32);
            this.checkBoxEDSM.TabIndex = 3;
            this.checkBoxEDSM.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "Show/Hide Body data from EDSM");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);
            // 
            // checkBoxMoons
            // 
            this.checkBoxMoons.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMoons.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMoons.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_ShowMoons;
            this.checkBoxMoons.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMoons.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMoons.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMoons.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMoons.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMoons.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxMoons.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxMoons.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxMoons.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxMoons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMoons.FontNerfReduction = 0.5F;
            this.checkBoxMoons.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMoons.Location = new System.Drawing.Point(103, 0);
            this.checkBoxMoons.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMoons.Name = "checkBoxMoons";
            this.checkBoxMoons.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMoons.TabIndex = 2;
            this.checkBoxMoons.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMoons, "Show/Hide Moons");
            this.checkBoxMoons.UseVisualStyleBackColor = false;
            this.checkBoxMoons.CheckedChanged += new System.EventHandler(this.checkBoxMoons_CheckedChanged);
            // 
            // checkBoxLarge
            // 
            this.checkBoxLarge.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLarge.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxLarge.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.checkBoxLarge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxLarge.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxLarge.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxLarge.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxLarge.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxLarge.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxLarge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxLarge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxLarge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxLarge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxLarge.FontNerfReduction = 0.5F;
            this.checkBoxLarge.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxLarge.Location = new System.Drawing.Point(147, 0);
            this.checkBoxLarge.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxLarge.Name = "checkBoxLarge";
            this.checkBoxLarge.Size = new System.Drawing.Size(32, 32);
            this.checkBoxLarge.TabIndex = 2;
            this.checkBoxLarge.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxLarge, "Image size large");
            this.checkBoxLarge.UseVisualStyleBackColor = false;
            this.checkBoxLarge.CheckedChanged += new System.EventHandler(this.checkBoxLarge_CheckedChanged);
            // 
            // checkBoxSmall
            // 
            this.checkBoxSmall.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSmall.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxSmall.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_SizeSmall;
            this.checkBoxSmall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxSmall.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxSmall.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxSmall.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxSmall.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxSmall.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxSmall.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxSmall.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxSmall.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxSmall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxSmall.FontNerfReduction = 0.5F;
            this.checkBoxSmall.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxSmall.Location = new System.Drawing.Point(211, 0);
            this.checkBoxSmall.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxSmall.Name = "checkBoxSmall";
            this.checkBoxSmall.Size = new System.Drawing.Size(32, 32);
            this.checkBoxSmall.TabIndex = 2;
            this.checkBoxSmall.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxSmall, "Image size small");
            this.checkBoxSmall.UseVisualStyleBackColor = false;
            this.checkBoxSmall.CheckedChanged += new System.EventHandler(this.checkBoxSmall_CheckedChanged);
            // 
            // checkBoxMedium
            // 
            this.checkBoxMedium.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMedium.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxMedium.BackgroundImage = global::EDDiscovery.Icons.Controls.Scan_SizeMedium;
            this.checkBoxMedium.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMedium.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxMedium.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMedium.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMedium.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxMedium.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxMedium.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxMedium.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxMedium.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxMedium.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxMedium.FontNerfReduction = 0.5F;
            this.checkBoxMedium.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMedium.Location = new System.Drawing.Point(179, 0);
            this.checkBoxMedium.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMedium.Name = "checkBoxMedium";
            this.checkBoxMedium.Size = new System.Drawing.Size(32, 32);
            this.checkBoxMedium.TabIndex = 2;
            this.checkBoxMedium.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMedium, "Image size medium");
            this.checkBoxMedium.UseVisualStyleBackColor = false;
            this.checkBoxMedium.CheckedChanged += new System.EventHandler(this.checkBoxMedium_CheckedChanged);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(360, 0);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(32, 32);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
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
            this.rollUpPanelTop.ContextMenuStrip = this.contextMenuStrip;
            this.rollUpPanelTop.Controls.Add(this.panelControls);
            this.rollUpPanelTop.Controls.Add(this.lblSystemInfo);
            this.rollUpPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelTop.HiddenMarkerWidth = 0;
            this.rollUpPanelTop.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelTop.Name = "rollUpPanelTop";
            this.rollUpPanelTop.PinState = true;
            this.rollUpPanelTop.RolledUpHeight = 5;
            this.rollUpPanelTop.RollUpAnimationTime = 500;
            this.rollUpPanelTop.RollUpDelay = 1000;
            this.rollUpPanelTop.SecondHiddenMarkerWidth = 0;
            this.rollUpPanelTop.ShowHiddenMarker = true;
            this.rollUpPanelTop.Size = new System.Drawing.Size(748, 32);
            this.rollUpPanelTop.TabIndex = 4;
            this.toolTip.SetToolTip(this.rollUpPanelTop, "Right click on panel to show/hide the toolbar");
            this.rollUpPanelTop.UnrolledHeight = 32;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelStars
            // 
            this.panelStars.CheckEDSM = false;
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.HideFullMaterials = false;
            this.panelStars.Location = new System.Drawing.Point(0, 32);
            this.panelStars.Name = "panelStars";
            this.panelStars.ShowMaterials = false;
            this.panelStars.ShowMaterialsRare = false;
            this.panelStars.ShowMoons = false;
            this.panelStars.ShowOverlays = false;
            this.panelStars.Size = new System.Drawing.Size(748, 650);
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
            this.contextMenuStrip.ResumeLayout(false);
            this.panelControls.ResumeLayout(false);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.CheckBoxCustom checkBoxMaterials;
        private ExtendedControls.CheckBoxCustom checkBoxMoons;
        private ExtendedControls.CheckBoxCustom checkBoxSmall;
        private ExtendedControls.CheckBoxCustom checkBoxMedium;
        private ExtendedControls.CheckBoxCustom checkBoxLarge;
        private ExtendedControls.CheckBoxCustom checkBoxTiny;
        private ExtendedControls.CheckBoxCustom checkBoxMaterialsRare;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemToolbar;
        private ExtendedControls.CheckBoxCustom checkBoxEDSM;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private System.Windows.Forms.Label lblSystemInfo;
        private ExtendedControls.CheckBoxCustom chkShowOverlays;
        private System.Windows.Forms.Panel panelControls;
        private ExtendedControls.RollUpPanel rollUpPanelTop;
        private ExtendedControls.CheckBoxCustom checkBoxCustomHideFullMats;
        private System.Windows.Forms.ToolStripMenuItem showSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelShowSystemToolStripMenuItem;
        private ScanDisplay panelStars;
    }
}
