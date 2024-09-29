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
    partial class UserControlScanGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extButtonHabZones = new ExtendedControls.ExtButton();
            this.dataViewScrollerPanel2 = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.colImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBriefing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustom2 = new ExtendedControls.ExtScrollBar();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.statusStripSummary = new ExtendedControls.ExtStatusStrip();
            this.toolStripStatusTotalValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripJumponiumProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.statusStripSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
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
            this.toolTip.SetToolTip(this.extButtonShowControl, "Configure overall settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
            // 
            // extButtonHabZones
            // 
            this.extButtonHabZones.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonHabZones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonHabZones.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            this.extButtonHabZones.Location = new System.Drawing.Point(48, 1);
            this.extButtonHabZones.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonHabZones.Name = "extButtonHabZones";
            this.extButtonHabZones.Size = new System.Drawing.Size(28, 28);
            this.extButtonHabZones.TabIndex = 35;
            this.toolTip.SetToolTip(this.extButtonHabZones, "Configure hab zone information");
            this.extButtonHabZones.UseVisualStyleBackColor = false;
            this.extButtonHabZones.Click += new System.EventHandler(this.extButtonHabZones_Click);
            // 
            // dataViewScrollerPanel2
            // 
            this.dataViewScrollerPanel2.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(1007, 519);
            this.dataViewScrollerPanel2.TabIndex = 25;
            this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colImage,
            this.colName,
            this.colClass,
            this.colDistance,
            this.colBriefing});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 36;
            this.dataGridView.RowTemplate.ReadOnly = true;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(988, 519);
            this.dataGridView.TabIndex = 23;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScangrid_CellDoubleClick);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewScangrid_RowPostPaint);
            // 
            // colImage
            // 
            this.colImage.FillWeight = 20F;
            this.colImage.HeaderText = "";
            this.colImage.MinimumWidth = 20;
            this.colImage.Name = "colImage";
            this.colImage.ReadOnly = true;
            this.colImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colName
            // 
            this.colName.FillWeight = 42.49234F;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 20;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colClass
            // 
            this.colClass.FillWeight = 53.11543F;
            this.colClass.HeaderText = "Class";
            this.colClass.MinimumWidth = 20;
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            // 
            // colDistance
            // 
            this.colDistance.FillWeight = 26.4625F;
            this.colDistance.HeaderText = "Distance";
            this.colDistance.MinimumWidth = 20;
            this.colDistance.Name = "colDistance";
            this.colDistance.ReadOnly = true;
            // 
            // colBriefing
            // 
            this.colBriefing.FillWeight = 106.2309F;
            this.colBriefing.HeaderText = "Information";
            this.colBriefing.MinimumWidth = 20;
            this.colBriefing.Name = "colBriefing";
            this.colBriefing.ReadOnly = true;
            // 
            // vScrollBarCustom2
            // 
            this.vScrollBarCustom2.AlwaysHideScrollBar = false;
            this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom2.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom2.HideScrollBar = true;
            this.vScrollBarCustom2.LargeChange = 0;
            this.vScrollBarCustom2.Location = new System.Drawing.Point(988, 0);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(19, 519);
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 24;
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
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
            this.rollUpPanelTop.Size = new System.Drawing.Size(1007, 30);
            this.rollUpPanelTop.TabIndex = 25;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonShowControl);
            this.panelControls.Controls.Add(this.extButtonHabZones);
            this.panelControls.Controls.Add(this.edsmSpanshButton);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1007, 30);
            this.panelControls.TabIndex = 32;
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(88, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 36;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // statusStripSummary
            // 
            this.statusStripSummary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusTotalValue,
            this.toolStripJumponiumProgressBar});
            this.statusStripSummary.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripSummary.Location = new System.Drawing.Point(0, 549);
            this.statusStripSummary.Name = "statusStripSummary";
            this.statusStripSummary.ShowItemToolTips = true;
            this.statusStripSummary.Size = new System.Drawing.Size(1007, 23);
            this.statusStripSummary.SizingGrip = false;
            this.statusStripSummary.TabIndex = 27;
            // 
            // toolStripStatusTotalValue
            // 
            this.toolStripStatusTotalValue.Name = "toolStripStatusTotalValue";
            this.toolStripStatusTotalValue.Size = new System.Drawing.Size(122, 18);
            this.toolStripStatusTotalValue.Text = "Estimated scans value";
            // 
            // toolStripJumponiumProgressBar
            // 
            this.toolStripJumponiumProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripJumponiumProgressBar.Maximum = 8;
            this.toolStripJumponiumProgressBar.Name = "toolStripJumponiumProgressBar";
            this.toolStripJumponiumProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripJumponiumProgressBar.Size = new System.Drawing.Size(100, 17);
            // 
            // UserControlScanGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Controls.Add(this.rollUpPanelTop);
            this.Controls.Add(this.statusStripSummary);
            this.Name = "UserControlScanGrid";
            this.Size = new System.Drawing.Size(1007, 572);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.statusStripSummary.ResumeLayout(false);
            this.statusStripSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel2;
        private ExtendedControls.ExtScrollBar vScrollBarCustom2;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtStatusStrip statusStripSummary;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTotalValue;
        private System.Windows.Forms.ToolStripProgressBar toolStripJumponiumProgressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBriefing;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtButton extButtonHabZones;
        private EDSMSpanshButton edsmSpanshButton;
    }
}
