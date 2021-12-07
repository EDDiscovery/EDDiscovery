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
    partial class UserControlOrganics
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
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extPictureBoxScroll = new ExtendedControls.ExtPictureBoxScroll();
            this.pictureBox = new ExtendedControls.ExtPictureBox();
            this.extScrollBar = new ExtendedControls.ExtScrollBar();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBar = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.ColDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStarSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBodyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColGenus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSpecies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLastScanType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelGrid = new System.Windows.Forms.Panel();
            this.flowLayoutPanelGridControl = new System.Windows.Forms.FlowLayoutPanel();
            this.labelStart = new System.Windows.Forms.Label();
            this.extDateTimePickerStartDate = new ExtendedControls.ExtDateTimePicker();
            this.labelEnd = new System.Windows.Forms.Label();
            this.extDateTimePickerEndDate = new ExtendedControls.ExtDateTimePicker();
            this.labelValue = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.extPictureBoxScroll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.flowLayoutPanelGridControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(8, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Display Settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(88, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 34;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Word Wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extPictureBoxScroll
            // 
            this.extPictureBoxScroll.Controls.Add(this.pictureBox);
            this.extPictureBoxScroll.Controls.Add(this.extScrollBar);
            this.extPictureBoxScroll.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPictureBoxScroll.Location = new System.Drawing.Point(0, 30);
            this.extPictureBoxScroll.Name = "extPictureBoxScroll";
            this.extPictureBoxScroll.ScrollBarEnabled = true;
            this.extPictureBoxScroll.Size = new System.Drawing.Size(1108, 257);
            this.extPictureBoxScroll.TabIndex = 1;
            this.extPictureBoxScroll.VerticalScrollBarDockRight = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1092, 257);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            // 
            // extScrollBar
            // 
            this.extScrollBar.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar.ArrowColorScaling = 0.5F;
            this.extScrollBar.ArrowDownDrawAngle = 270F;
            this.extScrollBar.ArrowUpDrawAngle = 90F;
            this.extScrollBar.BorderColor = System.Drawing.Color.White;
            this.extScrollBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar.HideScrollBar = true;
            this.extScrollBar.LargeChange = 257;
            this.extScrollBar.Location = new System.Drawing.Point(1092, 0);
            this.extScrollBar.Maximum = 256;
            this.extScrollBar.Minimum = 0;
            this.extScrollBar.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar.Name = "extScrollBar";
            this.extScrollBar.Size = new System.Drawing.Size(16, 257);
            this.extScrollBar.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar.SmallChange = 16;
            this.extScrollBar.TabIndex = 1;
            this.extScrollBar.Text = "extScrollBar1";
            this.extScrollBar.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar.ThumbColorScaling = 0.5F;
            this.extScrollBar.ThumbDrawAngle = 0F;
            this.extScrollBar.Value = 0;
            this.extScrollBar.ValueLimited = 0;
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBar);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 316);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(1108, 407);
            this.dataViewScrollerPanel.TabIndex = 26;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            this.dataViewScrollerPanel.Resize += new System.EventHandler(this.dataViewScrollerPanel_Resize);
            // 
            // vScrollBar
            // 
            this.vScrollBar.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBar.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBar.ArrowColorScaling = 0.5F;
            this.vScrollBar.ArrowDownDrawAngle = 270F;
            this.vScrollBar.ArrowUpDrawAngle = 90F;
            this.vScrollBar.BorderColor = System.Drawing.Color.White;
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBar.HideScrollBar = true;
            this.vScrollBar.LargeChange = 0;
            this.vScrollBar.Location = new System.Drawing.Point(1092, 0);
            this.vScrollBar.Maximum = -1;
            this.vScrollBar.Minimum = 0;
            this.vScrollBar.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBar.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(16, 407);
            this.vScrollBar.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBar.SmallChange = 1;
            this.vScrollBar.TabIndex = 24;
            this.vScrollBar.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBar.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBar.ThumbColorScaling = 0.5F;
            this.vScrollBar.ThumbDrawAngle = 0F;
            this.vScrollBar.Value = -1;
            this.vScrollBar.ValueLimited = -1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDate,
            this.ColStarSystem,
            this.ColBodyName,
            this.ColBodyType,
            this.ColGenus,
            this.ColSpecies,
            this.ColLastScanType,
            this.ColValue});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(1092, 407);
            this.dataGridView.TabIndex = 23;
            // 
            // ColDate
            // 
            this.ColDate.FillWeight = 50F;
            this.ColDate.HeaderText = "Date";
            this.ColDate.Name = "ColDate";
            // 
            // ColStarSystem
            // 
            this.ColStarSystem.HeaderText = "Star System";
            this.ColStarSystem.Name = "ColStarSystem";
            // 
            // ColBodyName
            // 
            this.ColBodyName.FillWeight = 75F;
            this.ColBodyName.HeaderText = "Body Name";
            this.ColBodyName.MinimumWidth = 50;
            this.ColBodyName.Name = "ColBodyName";
            // 
            // ColBodyType
            // 
            this.ColBodyType.HeaderText = "Body Type";
            this.ColBodyType.Name = "ColBodyType";
            // 
            // ColGenus
            // 
            this.ColGenus.FillWeight = 50F;
            this.ColGenus.HeaderText = "Genus";
            this.ColGenus.Name = "ColGenus";
            // 
            // ColSpecies
            // 
            this.ColSpecies.FillWeight = 50F;
            this.ColSpecies.HeaderText = "Species";
            this.ColSpecies.Name = "ColSpecies";
            // 
            // ColLastScanType
            // 
            this.ColLastScanType.FillWeight = 50F;
            this.ColLastScanType.HeaderText = "Last Scan Type";
            this.ColLastScanType.Name = "ColLastScanType";
            // 
            // ColValue
            // 
            this.ColValue.HeaderText = "Value";
            this.ColValue.Name = "ColValue";
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(1108, 30);
            this.rollUpPanelTop.TabIndex = 15;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonFont);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1108, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(48, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonFont, "Font");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            // 
            // panelGrid
            // 
            this.panelGrid.AutoSize = true;
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 0);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1108, 723);
            this.panelGrid.TabIndex = 2;
            this.panelGrid.Resize += new System.EventHandler(this.panelGrid_Resize);
            // 
            // flowLayoutPanelGridControl
            // 
            this.flowLayoutPanelGridControl.AutoSize = true;
            this.flowLayoutPanelGridControl.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanelGridControl.Controls.Add(this.labelStart);
            this.flowLayoutPanelGridControl.Controls.Add(this.extDateTimePickerStartDate);
            this.flowLayoutPanelGridControl.Controls.Add(this.labelEnd);
            this.flowLayoutPanelGridControl.Controls.Add(this.extDateTimePickerEndDate);
            this.flowLayoutPanelGridControl.Controls.Add(this.labelValue);
            this.flowLayoutPanelGridControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelGridControl.Location = new System.Drawing.Point(0, 287);
            this.flowLayoutPanelGridControl.Name = "flowLayoutPanelGridControl";
            this.flowLayoutPanelGridControl.Size = new System.Drawing.Size(1108, 29);
            this.flowLayoutPanelGridControl.TabIndex = 2;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(3, 6);
            this.labelStart.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(29, 13);
            this.labelStart.TabIndex = 1;
            this.labelStart.Text = "Start";
            // 
            // extDateTimePickerStartDate
            // 
            this.extDateTimePickerStartDate.BorderColor = System.Drawing.Color.Transparent;
            this.extDateTimePickerStartDate.BorderColorScaling = 0.5F;
            this.extDateTimePickerStartDate.Checked = false;
            this.extDateTimePickerStartDate.CustomFormat = "dd MMMM yyyy";
            this.extDateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.extDateTimePickerStartDate.Location = new System.Drawing.Point(38, 3);
            this.extDateTimePickerStartDate.Name = "extDateTimePickerStartDate";
            this.extDateTimePickerStartDate.SelectedColor = System.Drawing.Color.Yellow;
            this.extDateTimePickerStartDate.ShowCheckBox = true;
            this.extDateTimePickerStartDate.ShowUpDown = false;
            this.extDateTimePickerStartDate.Size = new System.Drawing.Size(250, 23);
            this.extDateTimePickerStartDate.TabIndex = 0;
            this.extDateTimePickerStartDate.TextBackColor = System.Drawing.Color.DarkBlue;
            this.extDateTimePickerStartDate.Value = new System.DateTime(2021, 12, 5, 11, 46, 34, 377);
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Location = new System.Drawing.Point(294, 6);
            this.labelEnd.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(26, 13);
            this.labelEnd.TabIndex = 2;
            this.labelEnd.Text = "End";
            // 
            // extDateTimePickerEndDate
            // 
            this.extDateTimePickerEndDate.BorderColor = System.Drawing.Color.Transparent;
            this.extDateTimePickerEndDate.BorderColorScaling = 0.5F;
            this.extDateTimePickerEndDate.Checked = false;
            this.extDateTimePickerEndDate.CustomFormat = "dd MMMM yyyy";
            this.extDateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.extDateTimePickerEndDate.Location = new System.Drawing.Point(326, 3);
            this.extDateTimePickerEndDate.Name = "extDateTimePickerEndDate";
            this.extDateTimePickerEndDate.SelectedColor = System.Drawing.Color.Yellow;
            this.extDateTimePickerEndDate.ShowCheckBox = true;
            this.extDateTimePickerEndDate.ShowUpDown = false;
            this.extDateTimePickerEndDate.Size = new System.Drawing.Size(250, 23);
            this.extDateTimePickerEndDate.TabIndex = 0;
            this.extDateTimePickerEndDate.TextBackColor = System.Drawing.Color.DarkBlue;
            this.extDateTimePickerEndDate.Value = new System.DateTime(2021, 12, 5, 11, 46, 34, 377);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(591, 6);
            this.labelValue.Margin = new System.Windows.Forms.Padding(12, 6, 3, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(25, 13);
            this.labelValue.TabIndex = 2;
            this.labelValue.Text = "0 cr";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 316);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(1108, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // UserControlOrganics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.flowLayoutPanelGridControl);
            this.Controls.Add(this.extPictureBoxScroll);
            this.Controls.Add(this.rollUpPanelTop);
            this.Controls.Add(this.panelGrid);
            this.Name = "UserControlOrganics";
            this.Size = new System.Drawing.Size(1108, 723);
            this.extPictureBoxScroll.ResumeLayout(false);
            this.extPictureBoxScroll.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.flowLayoutPanelGridControl.ResumeLayout(false);
            this.flowLayoutPanelGridControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPictureBox pictureBox;
        private ExtendedControls.ExtPictureBoxScroll extPictureBoxScroll;
        private ExtendedControls.ExtScrollBar extScrollBar;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBar;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStarSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBodyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColGenus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSpecies;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastScanType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelGridControl;
        private System.Windows.Forms.Label labelStart;
        private ExtendedControls.ExtDateTimePicker extDateTimePickerStartDate;
        private System.Windows.Forms.Label labelEnd;
        private ExtendedControls.ExtDateTimePicker extDateTimePickerEndDate;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Button button1;
    }
}
