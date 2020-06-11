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
    partial class UserControlMissions
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
            this.dataViewScrollerPanelCurrent = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewCurrent = new System.Windows.Forms.DataGridView();
            this.cColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColFromFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColDestSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColTargetFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cColInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomCur = new ExtendedControls.ExtScrollBar();
            this.panelPrev = new System.Windows.Forms.Panel();
            this.dataViewScrollerPanelPrev = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewPrevious = new System.Windows.Forms.DataGridView();
            this.PcolName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColFromFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColDestSys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColTargetFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomPrev = new ExtendedControls.ExtScrollBar();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.customDateTimePickerStart = new ExtendedControls.ExtDateTimePicker();
            this.labelTo = new System.Windows.Forms.Label();
            this.customDateTimePickerEnd = new ExtendedControls.ExtDateTimePicker();
            this.labelValue = new System.Windows.Forms.Label();
            this.panelCurrent = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerMissions = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanelCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrent)).BeginInit();
            this.panelPrev.SuspendLayout();
            this.dataViewScrollerPanelPrev.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrevious)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.panelCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMissions)).BeginInit();
            this.splitContainerMissions.Panel1.SuspendLayout();
            this.splitContainerMissions.Panel2.SuspendLayout();
            this.splitContainerMissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanelCurrent
            // 
            this.dataViewScrollerPanelCurrent.Controls.Add(this.dataGridViewCurrent);
            this.dataViewScrollerPanelCurrent.Controls.Add(this.vScrollBarCustomCur);
            this.dataViewScrollerPanelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelCurrent.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelCurrent.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelCurrent.Name = "dataViewScrollerPanelCurrent";
            this.dataViewScrollerPanelCurrent.Size = new System.Drawing.Size(800, 266);
            this.dataViewScrollerPanelCurrent.TabIndex = 0;
            this.dataViewScrollerPanelCurrent.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewCurrent
            // 
            this.dataGridViewCurrent.AllowDrop = true;
            this.dataGridViewCurrent.AllowUserToAddRows = false;
            this.dataGridViewCurrent.AllowUserToDeleteRows = false;
            this.dataGridViewCurrent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCurrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurrent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cColName,
            this.cColStartDate,
            this.cColEndDate,
            this.cColOrigin,
            this.cColFromFaction,
            this.cColDestSystem,
            this.cColTargetFaction,
            this.cColValue,
            this.cColInfo});
            this.dataGridViewCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCurrent.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCurrent.Name = "dataGridViewCurrent";
            this.dataGridViewCurrent.RowHeadersVisible = false;
            this.dataGridViewCurrent.RowHeadersWidth = 25;
            this.dataGridViewCurrent.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCurrent.Size = new System.Drawing.Size(784, 266);
            this.dataGridViewCurrent.TabIndex = 1;
            this.dataGridViewCurrent.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewCurrent_SortCompare);
            // 
            // cColName
            // 
            this.cColName.HeaderText = "Name";
            this.cColName.MinimumWidth = 50;
            this.cColName.Name = "cColName";
            this.cColName.ReadOnly = true;
            // 
            // cColStartDate
            // 
            this.cColStartDate.FillWeight = 75F;
            this.cColStartDate.HeaderText = "Start Date";
            this.cColStartDate.MinimumWidth = 50;
            this.cColStartDate.Name = "cColStartDate";
            this.cColStartDate.ReadOnly = true;
            // 
            // cColEndDate
            // 
            this.cColEndDate.FillWeight = 75F;
            this.cColEndDate.HeaderText = "End Date";
            this.cColEndDate.MinimumWidth = 50;
            this.cColEndDate.Name = "cColEndDate";
            this.cColEndDate.ReadOnly = true;
            // 
            // cColOrigin
            // 
            this.cColOrigin.FillWeight = 80F;
            this.cColOrigin.HeaderText = "Origin";
            this.cColOrigin.Name = "cColOrigin";
            this.cColOrigin.ReadOnly = true;
            // 
            // cColFromFaction
            // 
            this.cColFromFaction.FillWeight = 50F;
            this.cColFromFaction.HeaderText = "Faction";
            this.cColFromFaction.MinimumWidth = 50;
            this.cColFromFaction.Name = "cColFromFaction";
            this.cColFromFaction.ReadOnly = true;
            // 
            // cColDestSystem
            // 
            this.cColDestSystem.FillWeight = 80F;
            this.cColDestSystem.HeaderText = "Destination";
            this.cColDestSystem.MinimumWidth = 50;
            this.cColDestSystem.Name = "cColDestSystem";
            this.cColDestSystem.ReadOnly = true;
            // 
            // cColTargetFaction
            // 
            this.cColTargetFaction.FillWeight = 50F;
            this.cColTargetFaction.HeaderText = "Target Faction";
            this.cColTargetFaction.MinimumWidth = 50;
            this.cColTargetFaction.Name = "cColTargetFaction";
            this.cColTargetFaction.ReadOnly = true;
            // 
            // cColValue
            // 
            this.cColValue.HeaderText = "Value (cr)";
            this.cColValue.Name = "cColValue";
            // 
            // cColInfo
            // 
            this.cColInfo.FillWeight = 150F;
            this.cColInfo.HeaderText = "Info";
            this.cColInfo.MinimumWidth = 50;
            this.cColInfo.Name = "cColInfo";
            this.cColInfo.ReadOnly = true;
            // 
            // vScrollBarCustomCur
            // 
            this.vScrollBarCustomCur.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomCur.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomCur.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomCur.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomCur.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomCur.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomCur.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomCur.HideScrollBar = false;
            this.vScrollBarCustomCur.LargeChange = 0;
            this.vScrollBarCustomCur.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustomCur.Maximum = -1;
            this.vScrollBarCustomCur.Minimum = 0;
            this.vScrollBarCustomCur.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomCur.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomCur.Name = "vScrollBarCustomCur";
            this.vScrollBarCustomCur.Size = new System.Drawing.Size(16, 266);
            this.vScrollBarCustomCur.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomCur.SmallChange = 1;
            this.vScrollBarCustomCur.TabIndex = 0;
            this.vScrollBarCustomCur.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomCur.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomCur.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomCur.ThumbDrawAngle = 0F;
            this.vScrollBarCustomCur.Value = -1;
            this.vScrollBarCustomCur.ValueLimited = -1;
            // 
            // panelPrev
            // 
            this.panelPrev.Controls.Add(this.dataViewScrollerPanelPrev);
            this.panelPrev.Controls.Add(this.panelButtons);
            this.panelPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrev.Location = new System.Drawing.Point(0, 0);
            this.panelPrev.Name = "panelPrev";
            this.panelPrev.Size = new System.Drawing.Size(800, 302);
            this.panelPrev.TabIndex = 4;
            // 
            // dataViewScrollerPanelPrev
            // 
            this.dataViewScrollerPanelPrev.Controls.Add(this.dataGridViewPrevious);
            this.dataViewScrollerPanelPrev.Controls.Add(this.vScrollBarCustomPrev);
            this.dataViewScrollerPanelPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelPrev.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelPrev.Location = new System.Drawing.Point(0, 22);
            this.dataViewScrollerPanelPrev.Name = "dataViewScrollerPanelPrev";
            this.dataViewScrollerPanelPrev.Size = new System.Drawing.Size(800, 280);
            this.dataViewScrollerPanelPrev.TabIndex = 4;
            this.dataViewScrollerPanelPrev.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewPrevious
            // 
            this.dataGridViewPrevious.AllowDrop = true;
            this.dataGridViewPrevious.AllowUserToAddRows = false;
            this.dataGridViewPrevious.AllowUserToDeleteRows = false;
            this.dataGridViewPrevious.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPrevious.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPrevious.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PcolName,
            this.pColStart,
            this.pColEnd,
            this.pColOrigin,
            this.pColFromFaction,
            this.pColDestSys,
            this.pColTargetFaction,
            this.pColResult,
            this.pColInfo});
            this.dataGridViewPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPrevious.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPrevious.Name = "dataGridViewPrevious";
            this.dataGridViewPrevious.RowHeadersVisible = false;
            this.dataGridViewPrevious.RowHeadersWidth = 25;
            this.dataGridViewPrevious.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewPrevious.Size = new System.Drawing.Size(784, 280);
            this.dataGridViewPrevious.TabIndex = 2;
            this.dataGridViewPrevious.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewPrevious_SortCompare);
            // 
            // PcolName
            // 
            this.PcolName.HeaderText = "Name";
            this.PcolName.MinimumWidth = 50;
            this.PcolName.Name = "PcolName";
            this.PcolName.ReadOnly = true;
            // 
            // pColStart
            // 
            this.pColStart.FillWeight = 75F;
            this.pColStart.HeaderText = "Start Date";
            this.pColStart.MinimumWidth = 50;
            this.pColStart.Name = "pColStart";
            this.pColStart.ReadOnly = true;
            // 
            // pColEnd
            // 
            this.pColEnd.FillWeight = 75F;
            this.pColEnd.HeaderText = "End Date";
            this.pColEnd.MinimumWidth = 50;
            this.pColEnd.Name = "pColEnd";
            this.pColEnd.ReadOnly = true;
            // 
            // pColOrigin
            // 
            this.pColOrigin.FillWeight = 80F;
            this.pColOrigin.HeaderText = "Origin";
            this.pColOrigin.MinimumWidth = 50;
            this.pColOrigin.Name = "pColOrigin";
            this.pColOrigin.ReadOnly = true;
            // 
            // pColFromFaction
            // 
            this.pColFromFaction.FillWeight = 50F;
            this.pColFromFaction.HeaderText = "Faction";
            this.pColFromFaction.MinimumWidth = 50;
            this.pColFromFaction.Name = "pColFromFaction";
            this.pColFromFaction.ReadOnly = true;
            // 
            // pColDestSys
            // 
            this.pColDestSys.FillWeight = 80F;
            this.pColDestSys.HeaderText = "Destination";
            this.pColDestSys.MinimumWidth = 50;
            this.pColDestSys.Name = "pColDestSys";
            this.pColDestSys.ReadOnly = true;
            // 
            // pColTargetFaction
            // 
            this.pColTargetFaction.FillWeight = 50F;
            this.pColTargetFaction.HeaderText = "Target Faction";
            this.pColTargetFaction.MinimumWidth = 50;
            this.pColTargetFaction.Name = "pColTargetFaction";
            this.pColTargetFaction.ReadOnly = true;
            // 
            // pColResult
            // 
            this.pColResult.FillWeight = 50F;
            this.pColResult.HeaderText = "Result (cr)";
            this.pColResult.MinimumWidth = 25;
            this.pColResult.Name = "pColResult";
            this.pColResult.ReadOnly = true;
            // 
            // pColInfo
            // 
            this.pColInfo.FillWeight = 150F;
            this.pColInfo.HeaderText = "Info";
            this.pColInfo.MinimumWidth = 50;
            this.pColInfo.Name = "pColInfo";
            this.pColInfo.ReadOnly = true;
            // 
            // vScrollBarCustomPrev
            // 
            this.vScrollBarCustomPrev.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomPrev.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomPrev.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomPrev.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomPrev.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomPrev.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomPrev.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomPrev.HideScrollBar = false;
            this.vScrollBarCustomPrev.LargeChange = 0;
            this.vScrollBarCustomPrev.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustomPrev.Maximum = -1;
            this.vScrollBarCustomPrev.Minimum = 0;
            this.vScrollBarCustomPrev.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomPrev.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomPrev.Name = "vScrollBarCustomPrev";
            this.vScrollBarCustomPrev.Size = new System.Drawing.Size(16, 280);
            this.vScrollBarCustomPrev.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomPrev.SmallChange = 1;
            this.vScrollBarCustomPrev.TabIndex = 0;
            this.vScrollBarCustomPrev.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomPrev.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomPrev.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomPrev.ThumbDrawAngle = 0F;
            this.vScrollBarCustomPrev.Value = -1;
            this.vScrollBarCustomPrev.ValueLimited = -1;
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSize = true;
            this.panelButtons.Controls.Add(this.customDateTimePickerStart);
            this.panelButtons.Controls.Add(this.labelTo);
            this.panelButtons.Controls.Add(this.customDateTimePickerEnd);
            this.panelButtons.Controls.Add(this.labelValue);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 22);
            this.panelButtons.TabIndex = 2;
            // 
            // customDateTimePickerStart
            // 
            this.customDateTimePickerStart.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerStart.BorderColorScaling = 0.5F;
            this.customDateTimePickerStart.Checked = false;
            this.customDateTimePickerStart.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.customDateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerStart.Location = new System.Drawing.Point(0, 1);
            this.customDateTimePickerStart.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.customDateTimePickerStart.Name = "customDateTimePickerStart";
            this.customDateTimePickerStart.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerStart.ShowCheckBox = true;
            this.customDateTimePickerStart.ShowUpDown = false;
            this.customDateTimePickerStart.Size = new System.Drawing.Size(220, 20);
            this.customDateTimePickerStart.TabIndex = 0;
            this.customDateTimePickerStart.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.customDateTimePickerStart.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.customDateTimePickerStart.ValueChanged += new System.EventHandler(this.customDateTimePickerStart_ValueChanged);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(228, 1);
            this.labelTo.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(16, 13);
            this.labelTo.TabIndex = 1;
            this.labelTo.Text = "to";
            // 
            // customDateTimePickerEnd
            // 
            this.customDateTimePickerEnd.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerEnd.BorderColorScaling = 0.5F;
            this.customDateTimePickerEnd.Checked = false;
            this.customDateTimePickerEnd.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.customDateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerEnd.Location = new System.Drawing.Point(252, 1);
            this.customDateTimePickerEnd.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.customDateTimePickerEnd.Name = "customDateTimePickerEnd";
            this.customDateTimePickerEnd.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerEnd.ShowCheckBox = true;
            this.customDateTimePickerEnd.ShowUpDown = false;
            this.customDateTimePickerEnd.Size = new System.Drawing.Size(218, 20);
            this.customDateTimePickerEnd.TabIndex = 0;
            this.customDateTimePickerEnd.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.customDateTimePickerEnd.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.customDateTimePickerEnd.ValueChanged += new System.EventHandler(this.customDateTimePickerEnd_ValueChanged);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(478, 1);
            this.labelValue.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(43, 13);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "<code>";
            // 
            // panelCurrent
            // 
            this.panelCurrent.Controls.Add(this.dataViewScrollerPanelCurrent);
            this.panelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurrent.Location = new System.Drawing.Point(0, 0);
            this.panelCurrent.Name = "panelCurrent";
            this.panelCurrent.Size = new System.Drawing.Size(800, 266);
            this.panelCurrent.TabIndex = 3;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // splitContainerMissions
            // 
            this.splitContainerMissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMissions.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMissions.Name = "splitContainerMissions";
            this.splitContainerMissions.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMissions.Panel1
            // 
            this.splitContainerMissions.Panel1.Controls.Add(this.panelCurrent);
            // 
            // splitContainerMissions.Panel2
            // 
            this.splitContainerMissions.Panel2.Controls.Add(this.panelPrev);
            this.splitContainerMissions.Size = new System.Drawing.Size(800, 572);
            this.splitContainerMissions.SplitterDistance = 266;
            this.splitContainerMissions.TabIndex = 3;
            // 
            // UserControlMissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMissions);
            this.Name = "UserControlMissions";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanelCurrent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrent)).EndInit();
            this.panelPrev.ResumeLayout(false);
            this.panelPrev.PerformLayout();
            this.dataViewScrollerPanelPrev.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrevious)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.panelCurrent.ResumeLayout(false);
            this.splitContainerMissions.Panel1.ResumeLayout(false);
            this.splitContainerMissions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMissions)).EndInit();
            this.splitContainerMissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelCurrent;
        private System.Windows.Forms.DataGridView dataGridViewCurrent;
        private ExtendedControls.ExtScrollBar vScrollBarCustomCur;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelPrev;
        private System.Windows.Forms.DataGridView dataGridViewPrevious;
        private System.Windows.Forms.Panel panelCurrent;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelPrev;
        private ExtendedControls.ExtScrollBar vScrollBarCustomPrev;
        private System.Windows.Forms.SplitContainer splitContainerMissions;
        private ExtendedControls.ExtDateTimePicker customDateTimePickerStart;
        private System.Windows.Forms.Label labelTo;
        private ExtendedControls.ExtDateTimePicker customDateTimePickerEnd;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColOrigin;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColFromFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColDestSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColTargetFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cColInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PcolName;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColOrigin;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColFromFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColDestSys;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColTargetFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColInfo;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
    }
}
