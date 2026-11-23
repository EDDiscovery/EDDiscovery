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
    partial class UserControlSpanel
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
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extButtonColumns = new ExtendedControls.ExtButton();
            this.extButtonColumnOrder = new ExtendedControls.ExtButton();
            this.extButtonHabZones = new ExtendedControls.ExtButton();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.buttonField = new ExtendedControls.ExtButton();
            this.extButtonScanShow = new ExtendedControls.ExtButton();
            this.extButtoScanPos = new ExtendedControls.ExtButton();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.buttonExt12 = new ExtendedControls.ExtButton();
            this.buttonExt11 = new ExtendedControls.ExtButton();
            this.buttonExt10 = new ExtendedControls.ExtButton();
            this.buttonExt9 = new ExtendedControls.ExtButton();
            this.buttonExt8 = new ExtendedControls.ExtButton();
            this.buttonExt7 = new ExtendedControls.ExtButton();
            this.buttonExt6 = new ExtendedControls.ExtButton();
            this.buttonExt5 = new ExtendedControls.ExtButton();
            this.buttonExt4 = new ExtendedControls.ExtButton();
            this.buttonExt3 = new ExtendedControls.ExtButton();
            this.buttonExt2 = new ExtendedControls.ExtButton();
            this.buttonExt1 = new ExtendedControls.ExtButton();
            this.buttonExt0 = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 30);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(892, 650);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.ClickElement += new ExtendedControls.ExtPictureBox.OnElement(this.pictureBox_ClickElement);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(892, 30);
            this.rollUpPanelTop.TabIndex = 16;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonColumns);
            this.panelControls.Controls.Add(this.extButtonColumnOrder);
            this.panelControls.Controls.Add(this.extButtonHabZones);
            this.panelControls.Controls.Add(this.buttonFilter);
            this.panelControls.Controls.Add(this.buttonField);
            this.panelControls.Controls.Add(this.extButtonScanShow);
            this.panelControls.Controls.Add(this.extButtoScanPos);
            this.panelControls.Controls.Add(this.extButtonFont);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(892, 30);
            this.panelControls.TabIndex = 32;
            this.panelControls.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(8, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Configure overall settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            this.extButtonShowControl.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonColumns
            // 
            this.extButtonColumns.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonColumns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonColumns.Image = global::EDDiscovery.Icons.Controls.Columns;
            this.extButtonColumns.Location = new System.Drawing.Point(48, 1);
            this.extButtonColumns.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonColumns.Name = "extButtonColumns";
            this.extButtonColumns.Size = new System.Drawing.Size(28, 28);
            this.extButtonColumns.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonColumns, "Configure columns shown");
            this.extButtonColumns.UseVisualStyleBackColor = false;
            this.extButtonColumns.Click += new System.EventHandler(this.extButtonColumns_Click);
            this.extButtonColumns.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonColumnOrder
            // 
            this.extButtonColumnOrder.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonColumnOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonColumnOrder.Image = global::EDDiscovery.Icons.Controls.ColumnOrder;
            this.extButtonColumnOrder.Location = new System.Drawing.Point(88, 1);
            this.extButtonColumnOrder.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonColumnOrder.Name = "extButtonColumnOrder";
            this.extButtonColumnOrder.Size = new System.Drawing.Size(28, 28);
            this.extButtonColumnOrder.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonColumnOrder, "Configure column order");
            this.extButtonColumnOrder.UseVisualStyleBackColor = false;
            this.extButtonColumnOrder.Click += new System.EventHandler(this.extButtonColumnOrder_Click);
            this.extButtonColumnOrder.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonHabZones
            // 
            this.extButtonHabZones.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonHabZones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonHabZones.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.extButtonHabZones.Location = new System.Drawing.Point(128, 1);
            this.extButtonHabZones.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonHabZones.Name = "extButtonHabZones";
            this.extButtonHabZones.Size = new System.Drawing.Size(28, 28);
            this.extButtonHabZones.TabIndex = 35;
            this.toolTip.SetToolTip(this.extButtonHabZones, "Configure hab zone information");
            this.extButtonHabZones.UseVisualStyleBackColor = false;
            this.extButtonHabZones.Click += new System.EventHandler(this.extButtonHabZones_Click);
            this.extButtonHabZones.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(168, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 36;
            this.toolTip.SetToolTip(this.buttonFilter, "Select what journal events to show");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            this.buttonFilter.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // buttonField
            // 
            this.buttonField.Image = global::EDDiscovery.Icons.Controls.FieldFilter;
            this.buttonField.Location = new System.Drawing.Point(208, 1);
            this.buttonField.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonField.Name = "buttonField";
            this.buttonField.Size = new System.Drawing.Size(28, 28);
            this.buttonField.TabIndex = 37;
            this.toolTip.SetToolTip(this.buttonField, "Filter out events");
            this.buttonField.UseVisualStyleBackColor = true;
            this.buttonField.Click += new System.EventHandler(this.buttonField_Click);
            this.buttonField.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonScanShow
            // 
            this.extButtonScanShow.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonScanShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonScanShow.Image = global::EDDiscovery.Icons.Controls.Scan;
            this.extButtonScanShow.Location = new System.Drawing.Point(248, 1);
            this.extButtonScanShow.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonScanShow.Name = "extButtonScanShow";
            this.extButtonScanShow.Size = new System.Drawing.Size(28, 28);
            this.extButtonScanShow.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonScanShow, "Configure scan display");
            this.extButtonScanShow.UseVisualStyleBackColor = false;
            this.extButtonScanShow.Click += new System.EventHandler(this.extButtonScanShow_Click);
            this.extButtonScanShow.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtoScanPos
            // 
            this.extButtoScanPos.BackColor = System.Drawing.SystemColors.Control;
            this.extButtoScanPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtoScanPos.Image = global::EDDiscovery.Icons.Controls.ScanPosition;
            this.extButtoScanPos.Location = new System.Drawing.Point(288, 1);
            this.extButtoScanPos.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtoScanPos.Name = "extButtoScanPos";
            this.extButtoScanPos.Size = new System.Drawing.Size(28, 28);
            this.extButtoScanPos.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtoScanPos, "Set position of scan display");
            this.extButtoScanPos.UseVisualStyleBackColor = false;
            this.extButtoScanPos.Click += new System.EventHandler(this.extButtoScanPos_Click);
            this.extButtoScanPos.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(328, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonFont, "Font");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            this.extButtonFont.MouseEnter += new System.EventHandler(this.panelControls_MouseEnter);
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(368, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 34;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Word Wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // buttonExt12
            // 
            this.buttonExt12.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt12.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt12.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt12.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt12.Location = new System.Drawing.Point(384, 48);
            this.buttonExt12.Name = "buttonExt12";
            this.buttonExt12.Size = new System.Drawing.Size(24, 24);
            this.buttonExt12.TabIndex = 1;
            this.buttonExt12.Tag = "10";
            this.buttonExt12.UseVisualStyleBackColor = false;
            this.buttonExt12.Visible = false;
            this.buttonExt12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt12.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt12.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt11
            // 
            this.buttonExt11.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt11.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt11.Location = new System.Drawing.Point(354, 48);
            this.buttonExt11.Name = "buttonExt11";
            this.buttonExt11.Size = new System.Drawing.Size(24, 24);
            this.buttonExt11.TabIndex = 1;
            this.buttonExt11.Tag = "10";
            this.buttonExt11.UseVisualStyleBackColor = false;
            this.buttonExt11.Visible = false;
            this.buttonExt11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt11.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt11.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt10
            // 
            this.buttonExt10.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt10.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt10.Location = new System.Drawing.Point(324, 48);
            this.buttonExt10.Name = "buttonExt10";
            this.buttonExt10.Size = new System.Drawing.Size(24, 24);
            this.buttonExt10.TabIndex = 1;
            this.buttonExt10.Tag = "10";
            this.buttonExt10.UseVisualStyleBackColor = false;
            this.buttonExt10.Visible = false;
            this.buttonExt10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt10.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt9
            // 
            this.buttonExt9.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt9.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt9.Location = new System.Drawing.Point(290, 48);
            this.buttonExt9.Name = "buttonExt9";
            this.buttonExt9.Size = new System.Drawing.Size(24, 24);
            this.buttonExt9.TabIndex = 1;
            this.buttonExt9.Tag = "9";
            this.buttonExt9.UseVisualStyleBackColor = false;
            this.buttonExt9.Visible = false;
            this.buttonExt9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt9.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt8
            // 
            this.buttonExt8.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt8.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt8.Location = new System.Drawing.Point(260, 48);
            this.buttonExt8.Name = "buttonExt8";
            this.buttonExt8.Size = new System.Drawing.Size(24, 24);
            this.buttonExt8.TabIndex = 1;
            this.buttonExt8.Tag = "8";
            this.buttonExt8.UseVisualStyleBackColor = false;
            this.buttonExt8.Visible = false;
            this.buttonExt8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt8.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt7
            // 
            this.buttonExt7.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt7.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt7.Location = new System.Drawing.Point(230, 48);
            this.buttonExt7.Name = "buttonExt7";
            this.buttonExt7.Size = new System.Drawing.Size(24, 24);
            this.buttonExt7.TabIndex = 1;
            this.buttonExt7.Tag = "7";
            this.buttonExt7.UseVisualStyleBackColor = false;
            this.buttonExt7.Visible = false;
            this.buttonExt7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt6
            // 
            this.buttonExt6.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt6.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt6.Location = new System.Drawing.Point(200, 48);
            this.buttonExt6.Name = "buttonExt6";
            this.buttonExt6.Size = new System.Drawing.Size(24, 24);
            this.buttonExt6.TabIndex = 1;
            this.buttonExt6.Tag = "6";
            this.buttonExt6.UseVisualStyleBackColor = false;
            this.buttonExt6.Visible = false;
            this.buttonExt6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt5
            // 
            this.buttonExt5.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt5.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt5.Location = new System.Drawing.Point(164, 48);
            this.buttonExt5.Name = "buttonExt5";
            this.buttonExt5.Size = new System.Drawing.Size(24, 24);
            this.buttonExt5.TabIndex = 1;
            this.buttonExt5.Tag = "5";
            this.buttonExt5.UseVisualStyleBackColor = false;
            this.buttonExt5.Visible = false;
            this.buttonExt5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt4
            // 
            this.buttonExt4.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt4.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt4.Location = new System.Drawing.Point(133, 48);
            this.buttonExt4.Name = "buttonExt4";
            this.buttonExt4.Size = new System.Drawing.Size(24, 24);
            this.buttonExt4.TabIndex = 1;
            this.buttonExt4.Tag = "4";
            this.buttonExt4.UseVisualStyleBackColor = false;
            this.buttonExt4.Visible = false;
            this.buttonExt4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt3
            // 
            this.buttonExt3.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt3.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt3.Location = new System.Drawing.Point(103, 48);
            this.buttonExt3.Name = "buttonExt3";
            this.buttonExt3.Size = new System.Drawing.Size(24, 24);
            this.buttonExt3.TabIndex = 1;
            this.buttonExt3.Tag = "3";
            this.buttonExt3.UseVisualStyleBackColor = false;
            this.buttonExt3.Visible = false;
            this.buttonExt3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt2
            // 
            this.buttonExt2.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt2.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt2.Location = new System.Drawing.Point(73, 48);
            this.buttonExt2.Name = "buttonExt2";
            this.buttonExt2.Size = new System.Drawing.Size(24, 24);
            this.buttonExt2.TabIndex = 1;
            this.buttonExt2.Tag = "2";
            this.buttonExt2.UseVisualStyleBackColor = false;
            this.buttonExt2.Visible = false;
            this.buttonExt2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt1
            // 
            this.buttonExt1.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt1.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt1.Location = new System.Drawing.Point(43, 48);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(24, 24);
            this.buttonExt1.TabIndex = 1;
            this.buttonExt1.Tag = "1";
            this.buttonExt1.UseVisualStyleBackColor = false;
            this.buttonExt1.Visible = false;
            this.buttonExt1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // buttonExt0
            // 
            this.buttonExt0.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt0.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonExt0.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonExt0.Image = global::EDDiscovery.Icons.Controls.ResizeColumn;
            this.buttonExt0.Location = new System.Drawing.Point(8, 48);
            this.buttonExt0.Name = "buttonExt0";
            this.buttonExt0.Size = new System.Drawing.Size(24, 24);
            this.buttonExt0.TabIndex = 1;
            this.buttonExt0.Tag = "0";
            this.buttonExt0.UseVisualStyleBackColor = false;
            this.buttonExt0.Visible = false;
            this.buttonExt0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.divider_MouseDown);
            this.buttonExt0.MouseMove += new System.Windows.Forms.MouseEventHandler(this.divider_MouseMove);
            this.buttonExt0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divider_MouseUp);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlSpanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonExt12);
            this.Controls.Add(this.buttonExt11);
            this.Controls.Add(this.buttonExt10);
            this.Controls.Add(this.buttonExt9);
            this.Controls.Add(this.buttonExt8);
            this.Controls.Add(this.buttonExt7);
            this.Controls.Add(this.buttonExt6);
            this.Controls.Add(this.buttonExt5);
            this.Controls.Add(this.buttonExt4);
            this.Controls.Add(this.buttonExt3);
            this.Controls.Add(this.buttonExt2);
            this.Controls.Add(this.buttonExt1);
            this.Controls.Add(this.buttonExt0);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlSpanel";
            this.Size = new System.Drawing.Size(892, 680);
            this.Resize += new System.EventHandler(this.UserControlSpanel_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPictureBox pictureBox;
        private ExtendedControls.ExtButton buttonExt0;
        private ExtendedControls.ExtButton buttonExt1;
        private ExtendedControls.ExtButton buttonExt2;
        private ExtendedControls.ExtButton buttonExt3;
        private ExtendedControls.ExtButton buttonExt4;
        private ExtendedControls.ExtButton buttonExt5;
        private ExtendedControls.ExtButton buttonExt6;
        private ExtendedControls.ExtButton buttonExt7;
        private ExtendedControls.ExtButton buttonExt8;
        private ExtendedControls.ExtButton buttonExt9;
        private ExtendedControls.ExtButton buttonExt10;
        private ExtendedControls.ExtButton buttonExt11;
        private ExtendedControls.ExtButton buttonExt12;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtButton extButtonColumns;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtButton extButtonColumnOrder;
        private ExtendedControls.ExtButton extButtonScanShow;
        private ExtendedControls.ExtButton extButtoScanPos;
        private ExtendedControls.ExtButton extButtonHabZones;
        private ExtendedControls.ExtButton buttonFilter;
        private ExtendedControls.ExtButton buttonField;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
    }
}
