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
using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    partial class UserControlPlot
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
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.buttonExtReport = new ExtendedControls.ButtonExt();
            this.buttonExportPNG = new ExtendedControls.ButtonExt();
            this.comboBoxView = new ExtendedControls.ComboBoxCustom();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomToolStripMenuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem125 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem175 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem35 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.sysName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plotViewTop = new OxyPlot.WindowsForms.PlotView();
            this.plotViewSide = new OxyPlot.WindowsForms.PlotView();
            this.plotViewFront = new OxyPlot.WindowsForms.PlotView();
            this.reportView = new ExtendedControls.TextBoxBorder();
            this.panelTop.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            this.SuspendLayout();
            // 
            // textMinRadius
            // 
            this.textMinRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textMinRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textMinRadius.BackErrorColor = System.Drawing.Color.Red;
            this.textMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMinRadius.BorderColorScaling = 0.5F;
            this.textMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMinRadius.ClearOnFirstChar = false;
            this.textMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMinRadius.DelayBeforeNotification = 500;
            this.textMinRadius.Format = "0.#######";
            this.textMinRadius.InErrorCondition = false;
            this.textMinRadius.Location = new System.Drawing.Point(26, 3);
            this.textMinRadius.Maximum = 100000D;
            this.textMinRadius.Minimum = 0D;
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(36, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textMinRadius, "Minimum Range");
            this.textMinRadius.Value = 0D;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
            // 
            // textMaxRadius
            // 
            this.textMaxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textMaxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textMaxRadius.BackErrorColor = System.Drawing.Color.Red;
            this.textMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMaxRadius.BorderColorScaling = 0.5F;
            this.textMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMaxRadius.ClearOnFirstChar = false;
            this.textMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMaxRadius.DelayBeforeNotification = 500;
            this.textMaxRadius.Format = "0.#######";
            this.textMaxRadius.InErrorCondition = false;
            this.textMaxRadius.Location = new System.Drawing.Point(91, 3);
            this.textMaxRadius.Maximum = 100000D;
            this.textMaxRadius.Minimum = 0D;
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(36, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textMaxRadius, "Maximum Range");
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Controls.Add(this.labelExt1);
            this.panelTop.Controls.Add(this.labelExt3);
            this.panelTop.Controls.Add(this.buttonExtReport);
            this.panelTop.Controls.Add(this.buttonExportPNG);
            this.panelTop.Controls.Add(this.comboBoxView);
            this.panelTop.Controls.Add(this.textMinRadius);
            this.panelTop.Controls.Add(this.textMaxRadius);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(333, 26);
            this.panelTop.TabIndex = 25;
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.BackColor = System.Drawing.Color.Transparent;
            this.labelExt1.Location = new System.Drawing.Point(2, 7);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(24, 13);
            this.labelExt1.TabIndex = 31;
            this.labelExt1.Text = "Min";
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(63, 7);
            this.labelExt3.Name = "labelExt3";
            this.labelExt3.Size = new System.Drawing.Size(27, 13);
            this.labelExt3.TabIndex = 32;
            this.labelExt3.Text = "Max";
            // 
            // buttonExtReport
            // 
            this.buttonExtReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExtReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtReport.Image = global::EDDiscovery.Icons.Controls.StatsTime_Text;
            this.buttonExtReport.Location = new System.Drawing.Point(225, 1);
            this.buttonExtReport.Name = "buttonExtReport";
            this.buttonExtReport.Size = new System.Drawing.Size(24, 24);
            this.buttonExtReport.TabIndex = 30;
            this.toolTip1.SetToolTip(this.buttonExtReport, "Create a TXT report.");
            this.buttonExtReport.UseVisualStyleBackColor = true;
            this.buttonExtReport.Click += new System.EventHandler(this.buttonExtReport_Click);
            // 
            // buttonExportPNG
            // 
            this.buttonExportPNG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportPNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportPNG.Image = global::EDDiscovery.Icons.Controls.Map3D_Recorder_Save;
            this.buttonExportPNG.Location = new System.Drawing.Point(195, 1);
            this.buttonExportPNG.Name = "buttonExportPNG";
            this.buttonExportPNG.Size = new System.Drawing.Size(24, 24);
            this.buttonExportPNG.TabIndex = 29;
            this.toolTip1.SetToolTip(this.buttonExportPNG, "Export to PNG");
            this.buttonExportPNG.UseVisualStyleBackColor = true;
            this.buttonExportPNG.Click += new System.EventHandler(this.buttonExportToImage_Click);
            // 
            // comboBoxView
            // 
            this.comboBoxView.ArrowWidth = 1;
            this.comboBoxView.BorderColor = System.Drawing.Color.White;
            this.comboBoxView.ButtonColorScaling = 0.5F;
            this.comboBoxView.DataSource = null;
            this.comboBoxView.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxView.DisplayMember = "";
            this.comboBoxView.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxView.DropDownHeight = 106;
            this.comboBoxView.DropDownWidth = 75;
            this.comboBoxView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxView.ItemHeight = 13;
            this.comboBoxView.Location = new System.Drawing.Point(134, 3);
            this.comboBoxView.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxView.Name = "comboBoxView";
            this.comboBoxView.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarWidth = 16;
            this.comboBoxView.SelectedIndex = -1;
            this.comboBoxView.SelectedItem = null;
            this.comboBoxView.SelectedValue = null;
            this.comboBoxView.Size = new System.Drawing.Size(53, 21);
            this.comboBoxView.TabIndex = 9;
            this.comboBoxView.Text = "comboBoxCustom1";
            this.comboBoxView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.comboBoxView, "Orientation of the Plot");
            this.comboBoxView.ValueMember = "";
            this.comboBoxView.SelectedIndexChanged += new System.EventHandler(this.comboBoxView_SelectedIndexChanged);
            this.comboBoxView.TextChanged += new System.EventHandler(this.comboBoxView_TextChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuZoom,
            this.resetToolStripReset});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(68, 48);
            // 
            // zoomToolStripMenuZoom
            // 
            this.zoomToolStripMenuZoom.Name = "zoomToolStripMenuZoom";
            this.zoomToolStripMenuZoom.Size = new System.Drawing.Size(67, 22);
            // 
            // resetToolStripReset
            // 
            this.resetToolStripReset.Name = "resetToolStripReset";
            this.resetToolStripReset.Size = new System.Drawing.Size(67, 22);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem125
            // 
            this.toolStripMenuItem125.Name = "toolStripMenuItem125";
            this.toolStripMenuItem125.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem175
            // 
            this.toolStripMenuItem175.Name = "toolStripMenuItem175";
            this.toolStripMenuItem175.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem35
            // 
            this.toolStripMenuItem35.Name = "toolStripMenuItem35";
            this.toolStripMenuItem35.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(32, 19);
            // 
            // dataGridList
            // 
            this.dataGridList.AllowUserToAddRows = false;
            this.dataGridList.AllowUserToDeleteRows = false;
            this.dataGridList.AllowUserToOrderColumns = true;
            this.dataGridList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sysName,
            this.sysX,
            this.sysY,
            this.sysZ,
            this.sysVisits});
            this.dataGridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridList.Location = new System.Drawing.Point(0, 26);
            this.dataGridList.Name = "dataGridList";
            this.dataGridList.ReadOnly = true;
            this.dataGridList.RowHeadersVisible = false;
            this.dataGridList.Size = new System.Drawing.Size(333, 315);
            this.dataGridList.TabIndex = 30;
            this.dataGridList.Visible = false;
            this.dataGridList.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridList_SortCompare);
            // 
            // sysName
            // 
            this.sysName.HeaderText = "Name";
            this.sysName.Name = "sysName";
            this.sysName.ReadOnly = true;
            // 
            // sysX
            // 
            this.sysX.FillWeight = 15F;
            this.sysX.HeaderText = "X";
            this.sysX.Name = "sysX";
            this.sysX.ReadOnly = true;
            // 
            // sysY
            // 
            this.sysY.FillWeight = 15F;
            this.sysY.HeaderText = "Y";
            this.sysY.Name = "sysY";
            this.sysY.ReadOnly = true;
            // 
            // sysZ
            // 
            this.sysZ.FillWeight = 15F;
            this.sysZ.HeaderText = "Z";
            this.sysZ.Name = "sysZ";
            this.sysZ.ReadOnly = true;
            // 
            // sysVisits
            // 
            this.sysVisits.FillWeight = 20F;
            this.sysVisits.HeaderText = "Visits";
            this.sysVisits.Name = "sysVisits";
            this.sysVisits.ReadOnly = true;
            // 
            // plotViewTop
            // 
            this.plotViewTop.Cursor = System.Windows.Forms.Cursors.Cross;
            this.plotViewTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotViewTop.ForeColor = System.Drawing.Color.Black;
            this.plotViewTop.Location = new System.Drawing.Point(0, 26);
            this.plotViewTop.Name = "plotViewTop";
            this.plotViewTop.PanCursor = System.Windows.Forms.Cursors.NoMove2D;
            this.plotViewTop.Size = new System.Drawing.Size(333, 315);
            this.plotViewTop.TabIndex = 31;
            this.plotViewTop.Text = "plotView1";
            this.plotViewTop.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewTop.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewTop.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // plotViewSide
            // 
            this.plotViewSide.Cursor = System.Windows.Forms.Cursors.Cross;
            this.plotViewSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotViewSide.ForeColor = System.Drawing.Color.Black;
            this.plotViewSide.Location = new System.Drawing.Point(0, 26);
            this.plotViewSide.Name = "plotViewSide";
            this.plotViewSide.PanCursor = System.Windows.Forms.Cursors.NoMove2D;
            this.plotViewSide.Size = new System.Drawing.Size(333, 315);
            this.plotViewSide.TabIndex = 33;
            this.plotViewSide.Text = "plotView1";
            this.plotViewSide.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewSide.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewSide.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // plotViewFront
            // 
            this.plotViewFront.Cursor = System.Windows.Forms.Cursors.Cross;
            this.plotViewFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotViewFront.ForeColor = System.Drawing.Color.Black;
            this.plotViewFront.Location = new System.Drawing.Point(0, 26);
            this.plotViewFront.Name = "plotViewFront";
            this.plotViewFront.PanCursor = System.Windows.Forms.Cursors.NoMove2D;
            this.plotViewFront.Size = new System.Drawing.Size(333, 315);
            this.plotViewFront.TabIndex = 34;
            this.plotViewFront.Text = "plotView1";
            this.plotViewFront.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewFront.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewFront.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // reportView
            // 
            this.reportView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.reportView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.reportView.BackErrorColor = System.Drawing.Color.Red;
            this.reportView.BorderColor = System.Drawing.Color.Transparent;
            this.reportView.BorderColorScaling = 0.5F;
            this.reportView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reportView.ClearOnFirstChar = false;
            this.reportView.ControlBackground = System.Drawing.SystemColors.Control;
            this.reportView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.reportView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportView.InErrorCondition = false;
            this.reportView.Location = new System.Drawing.Point(0, 26);
            this.reportView.Multiline = true;
            this.reportView.Name = "reportView";
            this.reportView.ReadOnly = false;
            this.reportView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.reportView.SelectionLength = 0;
            this.reportView.SelectionStart = 0;
            this.reportView.Size = new System.Drawing.Size(333, 315);
            this.reportView.TabIndex = 2;
            this.reportView.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.reportView.WordWrap = true;
            // 
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.dataGridList);
            this.Controls.Add(this.plotViewTop);
            this.Controls.Add(this.plotViewFront);
            this.Controls.Add(this.plotViewSide);
            this.Controls.Add(this.reportView);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(333, 341);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.NumberBoxDouble textMinRadius;
        private ExtendedControls.NumberBoxDouble textMaxRadius;
        private Panel panelTop;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem zoomToolStripMenuZoom;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem125;
        private ToolStripMenuItem toolStripMenuItem15;
        private ToolStripMenuItem toolStripMenuItem175;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem25;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem35;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem resetToolStripReset;
        private ExtendedControls.ComboBoxCustom comboBoxView;
        private ToolTip toolTip1;
        private DataGridView dataGridList;
        private DataGridViewTextBoxColumn sysName;
        private DataGridViewTextBoxColumn sysX;
        private DataGridViewTextBoxColumn sysY;
        private DataGridViewTextBoxColumn sysZ;
        private DataGridViewTextBoxColumn sysVisits;
        private ExtendedControls.ButtonExt buttonExportPNG;
        private OxyPlot.WindowsForms.PlotView plotViewTop;
        private OxyPlot.WindowsForms.PlotView plotViewSide;
        private OxyPlot.WindowsForms.PlotView plotViewFront;
        private ExtendedControls.TextBoxBorder reportView;
        private ExtendedControls.ButtonExt buttonExtReport;
        private Label labelExt1;
        private Label labelExt3;
    }
}
