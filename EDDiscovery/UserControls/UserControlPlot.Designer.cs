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
            this.buttonExportToImage = new ExtendedControls.ButtonExt();
            this.checkBoxLegend = new ExtendedControls.CheckBoxCustom();
            this.checkBoxDotSize = new ExtendedControls.CheckBoxCustom();
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
            this.plotView = new OxyPlot.WindowsForms.PlotView();
            this.reportView = new ExtendedControls.RichTextBoxScroll();
            this.buttonExportReport = new ExtendedControls.ButtonExt();
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
            this.textMinRadius.Location = new System.Drawing.Point(5, 3);
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
            this.textMaxRadius.Location = new System.Drawing.Point(46, 3);
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
            this.panelTop.Controls.Add(this.buttonExportReport);
            this.panelTop.Controls.Add(this.buttonExportToImage);
            this.panelTop.Controls.Add(this.checkBoxLegend);
            this.panelTop.Controls.Add(this.checkBoxDotSize);
            this.panelTop.Controls.Add(this.comboBoxView);
            this.panelTop.Controls.Add(this.textMinRadius);
            this.panelTop.Controls.Add(this.textMaxRadius);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(333, 26);
            this.panelTop.TabIndex = 25;
            // 
            // buttonExportToImage
            // 
            this.buttonExportToImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportToImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportToImage.Image = global::EDDiscovery.Icons.Controls.Map3D_Recorder_Save;
            this.buttonExportToImage.Location = new System.Drawing.Point(212, 0);
            this.buttonExportToImage.Name = "buttonExportToImage";
            this.buttonExportToImage.Size = new System.Drawing.Size(24, 24);
            this.buttonExportToImage.TabIndex = 29;
            this.toolTip1.SetToolTip(this.buttonExportToImage, "Export to Excel");
            this.buttonExportToImage.UseVisualStyleBackColor = true;
            this.buttonExportToImage.Click += new System.EventHandler(this.buttonExportToImage_Click);
            // 
            // checkBoxLegend
            // 
            this.checkBoxLegend.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxLegend.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxLegend.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLegend.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxLegend.CheckBoxInnerColor = System.Drawing.Color.Transparent;
            this.checkBoxLegend.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxLegend.Checked = true;
            this.checkBoxLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLegend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxLegend.FontNerfReduction = 0.5F;
            this.checkBoxLegend.Image = global::EDDiscovery.Icons.Controls.Map3D_Stars_Menu;
            this.checkBoxLegend.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxLegend.Location = new System.Drawing.Point(182, 1);
            this.checkBoxLegend.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxLegend.Name = "checkBoxLegend";
            this.checkBoxLegend.Size = new System.Drawing.Size(24, 24);
            this.checkBoxLegend.TabIndex = 11;
            this.checkBoxLegend.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxLegend, "Toggle the Title and Legend visibility");
            this.checkBoxLegend.UseVisualStyleBackColor = false;
            // 
            // checkBoxDotSize
            // 
            this.checkBoxDotSize.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDotSize.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxDotSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxDotSize.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxDotSize.CheckBoxInnerColor = System.Drawing.Color.Transparent;
            this.checkBoxDotSize.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxDotSize.Checked = true;
            this.checkBoxDotSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDotSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxDotSize.FontNerfReduction = 0.5F;
            this.checkBoxDotSize.Image = global::EDDiscovery.Icons.Controls.Plot_Dots;
            this.checkBoxDotSize.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxDotSize.Location = new System.Drawing.Point(152, 1);
            this.checkBoxDotSize.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxDotSize.Name = "checkBoxDotSize";
            this.checkBoxDotSize.Size = new System.Drawing.Size(24, 24);
            this.checkBoxDotSize.TabIndex = 10;
            this.checkBoxDotSize.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxDotSize, "Use the third axis value to mimics depth (nearest systems larger, farther systems" +
        " smaller)");
            this.checkBoxDotSize.UseVisualStyleBackColor = false;
            this.checkBoxDotSize.CheckedChanged += new System.EventHandler(this.checkBoxDotSize_CheckedChanged);
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
            this.comboBoxView.Location = new System.Drawing.Point(88, 3);
            this.comboBoxView.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxView.Name = "comboBoxView";
            this.comboBoxView.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarWidth = 16;
            this.comboBoxView.SelectedIndex = -1;
            this.comboBoxView.SelectedItem = null;
            this.comboBoxView.SelectedValue = null;
            this.comboBoxView.Size = new System.Drawing.Size(58, 21);
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
            // plotView
            // 
            this.plotView.Cursor = System.Windows.Forms.Cursors.Cross;
            this.plotView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotView.ForeColor = System.Drawing.Color.Black;
            this.plotView.Location = new System.Drawing.Point(0, 26);
            this.plotView.Name = "plotView";
            this.plotView.PanCursor = System.Windows.Forms.Cursors.NoMove2D;
            this.plotView.Size = new System.Drawing.Size(333, 315);
            this.plotView.TabIndex = 31;
            this.plotView.Text = "plotView1";
            this.plotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // reportView
            // 
            this.reportView.BorderColor = System.Drawing.Color.Transparent;
            this.reportView.BorderColorScaling = 0.5F;
            this.reportView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportView.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportView.HideScrollBar = true;
            this.reportView.Location = new System.Drawing.Point(0, 26);
            this.reportView.Name = "reportView";
            this.reportView.ReadOnly = false;
            this.reportView.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.reportView.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.reportView.ScrollBarBackColor = System.Drawing.Color.Black;
            this.reportView.ScrollBarBorderColor = System.Drawing.Color.White;
            this.reportView.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.reportView.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.reportView.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.reportView.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.reportView.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.reportView.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.reportView.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.reportView.ScrollBarWidth = 20;
            this.reportView.ShowLineCount = false;
            this.reportView.Size = new System.Drawing.Size(333, 315);
            this.reportView.TabIndex = 32;
            this.reportView.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.reportView.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // buttonExportReport
            // 
            this.buttonExportReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportReport.Image = global::EDDiscovery.Icons.Controls.Map2D_Save;
            this.buttonExportReport.Location = new System.Drawing.Point(242, 0);
            this.buttonExportReport.Name = "buttonExportReport";
            this.buttonExportReport.Size = new System.Drawing.Size(24, 24);
            this.buttonExportReport.TabIndex = 30;
            this.toolTip1.SetToolTip(this.buttonExportReport, "Export to Excel");
            this.buttonExportReport.UseVisualStyleBackColor = true;
            this.buttonExportReport.Click += new System.EventHandler(this.buttonExportReport_Click);
            // 
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.reportView);
            this.Controls.Add(this.dataGridList);
            this.Controls.Add(this.plotView);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(333, 341);
            this.panelTop.ResumeLayout(false);
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
        private ExtendedControls.CheckBoxCustom checkBoxDotSize;
        private ToolTip toolTip1;
        private DataGridView dataGridList;
        private DataGridViewTextBoxColumn sysName;
        private DataGridViewTextBoxColumn sysX;
        private DataGridViewTextBoxColumn sysY;
        private DataGridViewTextBoxColumn sysZ;
        private DataGridViewTextBoxColumn sysVisits;
        private ExtendedControls.CheckBoxCustom checkBoxLegend;
        private ExtendedControls.ButtonExt buttonExportToImage;
        private OxyPlot.WindowsForms.PlotView plotView;
        private ExtendedControls.RichTextBoxScroll reportView;
        private ExtendedControls.ButtonExt buttonExportReport;
    }
}
