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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExportToImage = new ExtendedControls.ButtonExt();
            this.checkBoxLegend = new ExtendedControls.CheckBoxCustom();
            this.checkBoxDotSize = new ExtendedControls.CheckBoxCustom();
            this.comboBoxView = new ExtendedControls.ComboBoxCustom();
            this.chartBubblePlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomToolStripMenuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem125 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem175 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem35 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripReset = new System.Windows.Forms.ToolStripMenuItem();
            this.background = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridList = new System.Windows.Forms.DataGridView();
            this.sysName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sysVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubblePlot)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).BeginInit();
            this.SuspendLayout();
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(3, 6);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(24, 13);
            this.labelExt1.TabIndex = 3;
            this.labelExt1.Text = "Min";
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
            this.textMinRadius.Location = new System.Drawing.Point(28, 3);
            this.textMinRadius.Maximum = 100000D;
            this.textMinRadius.Minimum = 0D;
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(30, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.Value = 0D;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(69, 6);
            this.labelExt3.Name = "labelExt3";
            this.labelExt3.Size = new System.Drawing.Size(27, 13);
            this.labelExt3.TabIndex = 3;
            this.labelExt3.Text = "Max";
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
            this.textMaxRadius.Location = new System.Drawing.Point(97, 3);
            this.textMaxRadius.Maximum = 100000D;
            this.textMaxRadius.Minimum = 0D;
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(30, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Controls.Add(this.buttonExportToImage);
            this.panelTop.Controls.Add(this.checkBoxLegend);
            this.panelTop.Controls.Add(this.checkBoxDotSize);
            this.panelTop.Controls.Add(this.comboBoxView);
            this.panelTop.Controls.Add(this.labelExt1);
            this.panelTop.Controls.Add(this.textMinRadius);
            this.panelTop.Controls.Add(this.labelExt3);
            this.panelTop.Controls.Add(this.textMaxRadius);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(335, 26);
            this.panelTop.TabIndex = 25;
            // 
            // buttonExportToImage
            // 
            this.buttonExportToImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExportToImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportToImage.Image = global::EDDiscovery.Icons.Controls.Map3D_Recorder_Save;
            this.buttonExportToImage.Location = new System.Drawing.Point(248, 1);
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
            this.checkBoxLegend.Location = new System.Drawing.Point(221, 1);
            this.checkBoxLegend.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxLegend.Name = "checkBoxLegend";
            this.checkBoxLegend.Size = new System.Drawing.Size(24, 24);
            this.checkBoxLegend.TabIndex = 11;
            this.checkBoxLegend.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxLegend, "Toggle the Legend");
            this.checkBoxLegend.UseVisualStyleBackColor = false;
            this.checkBoxLegend.CheckedChanged += new System.EventHandler(this.checkBoxLegend_CheckedChanged);
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
            this.checkBoxDotSize.Location = new System.Drawing.Point(194, 1);
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
            this.comboBoxView.Location = new System.Drawing.Point(136, 3);
            this.comboBoxView.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxView.Name = "comboBoxView";
            this.comboBoxView.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarWidth = 16;
            this.comboBoxView.SelectedIndex = -1;
            this.comboBoxView.SelectedItem = null;
            this.comboBoxView.SelectedValue = null;
            this.comboBoxView.Size = new System.Drawing.Size(49, 21);
            this.comboBoxView.TabIndex = 9;
            this.comboBoxView.Text = "comboBoxCustom1";
            this.comboBoxView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.comboBoxView, "Orientation of the Plot");
            this.comboBoxView.ValueMember = "";
            this.comboBoxView.SelectedIndexChanged += new System.EventHandler(this.comboBoxView_SelectedIndexChanged);
            // 
            // chartBubblePlot
            // 
            this.chartBubblePlot.BackColor = System.Drawing.Color.Black;
            this.chartBubblePlot.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBubblePlot.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisX.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX.MinorGrid.Interval = double.NaN;
            chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Empty;
            chartArea1.AxisX.Title = "X";
            chartArea1.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea1.AxisY.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MajorGrid.Interval = 0D;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea1.AxisY.Title = "Y";
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY2.Title = "Z";
            chartArea1.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "Plot";
            this.chartBubblePlot.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.ForeColor = System.Drawing.Color.White;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend";
            this.chartBubblePlot.Legends.Add(legend1);
            this.chartBubblePlot.Location = new System.Drawing.Point(19, 41);
            this.chartBubblePlot.Margin = new System.Windows.Forms.Padding(0);
            this.chartBubblePlot.Name = "chartBubblePlot";
            series1.ChartArea = "Plot";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.CustomProperties = "BubbleMinSize=5, BubbleMaxSize=5";
            series1.Legend = "Legend";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerSize = 4;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Current";
            series1.YValuesPerPoint = 2;
            series2.ChartArea = "Plot";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series2.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series2.Legend = "Legend";
            series2.MarkerBorderColor = System.Drawing.Color.Blue;
            series2.MarkerColor = System.Drawing.Color.RoyalBlue;
            series2.MarkerSize = 4;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Visited";
            series2.YValuesPerPoint = 2;
            series3.ChartArea = "Plot";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series3.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series3.Legend = "Legend";
            series3.MarkerBorderColor = System.Drawing.Color.RoyalBlue;
            series3.MarkerColor = System.Drawing.Color.Purple;
            series3.MarkerSize = 4;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Previous";
            series3.YValuesPerPoint = 2;
            series4.ChartArea = "Plot";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series4.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series4.Legend = "Legend";
            series4.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series4.MarkerColor = System.Drawing.Color.Gold;
            series4.MarkerSize = 4;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "Not Visited";
            series4.YValuesPerPoint = 2;
            this.chartBubblePlot.Series.Add(series1);
            this.chartBubblePlot.Series.Add(series2);
            this.chartBubblePlot.Series.Add(series3);
            this.chartBubblePlot.Series.Add(series4);
            this.chartBubblePlot.Size = new System.Drawing.Size(301, 283);
            this.chartBubblePlot.SuppressExceptions = true;
            this.chartBubblePlot.TabIndex = 28;
            this.chartBubblePlot.Text = "Nearest Systems Plot";
            this.chartBubblePlot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartBubble_MouseDown);
            this.chartBubblePlot.MouseEnter += new System.EventHandler(this.chartBubble_MouseEnter);
            this.chartBubblePlot.MouseLeave += new System.EventHandler(this.chartBubble_MouseLeave);
            this.chartBubblePlot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartBubble_MouseMove);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToolStripMenuZoom,
            this.resetToolStripReset});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(107, 48);
            // 
            // zoomToolStripMenuZoom
            // 
            this.zoomToolStripMenuZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem125,
            this.toolStripMenuItem15,
            this.toolStripMenuItem175,
            this.toolStripMenuItem2,
            this.toolStripMenuItem25,
            this.toolStripMenuItem3,
            this.toolStripMenuItem35,
            this.toolStripMenuItem4});
            this.zoomToolStripMenuZoom.Name = "zoomToolStripMenuZoom";
            this.zoomToolStripMenuZoom.Size = new System.Drawing.Size(106, 22);
            this.zoomToolStripMenuZoom.Text = "Zoom";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem1.Text = "1:1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem125
            // 
            this.toolStripMenuItem125.Name = "toolStripMenuItem125";
            this.toolStripMenuItem125.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem125.Text = "1.25:1";
            this.toolStripMenuItem125.Click += new System.EventHandler(this.toolStripMenuItem125_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem15.Text = "1.5:1";
            this.toolStripMenuItem15.Click += new System.EventHandler(this.toolStripMenuItem15_Click);
            // 
            // toolStripMenuItem175
            // 
            this.toolStripMenuItem175.Name = "toolStripMenuItem175";
            this.toolStripMenuItem175.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem175.Text = "1.75:1";
            this.toolStripMenuItem175.Click += new System.EventHandler(this.toolStripMenuItem175_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem2.Text = "2:1";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem25.Text = "2.5:1";
            this.toolStripMenuItem25.Click += new System.EventHandler(this.toolStripMenuItem25_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem3.Text = "3:1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem35
            // 
            this.toolStripMenuItem35.Name = "toolStripMenuItem35";
            this.toolStripMenuItem35.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem35.Text = "3.5:1";
            this.toolStripMenuItem35.Click += new System.EventHandler(this.toolStripMenuItem35_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem4.Text = "4:1";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // resetToolStripReset
            // 
            this.resetToolStripReset.Name = "resetToolStripReset";
            this.resetToolStripReset.Size = new System.Drawing.Size(106, 22);
            this.resetToolStripReset.Text = "Reset";
            this.resetToolStripReset.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // background
            // 
            this.background.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(335, 343);
            this.background.TabIndex = 29;
            this.background.TabStop = false;
            this.background.MouseDown += new System.Windows.Forms.MouseEventHandler(this.background_MouseDown);
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
            this.dataGridList.Size = new System.Drawing.Size(335, 317);
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
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.dataGridList);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.chartBubblePlot);
            this.Controls.Add(this.background);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(335, 343);
            this.Resize += new System.EventHandler(this.UserControlPlot_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubblePlot)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelExt1;
        private ExtendedControls.NumberBoxDouble textMinRadius;
        private System.Windows.Forms.Label labelExt3;
        private ExtendedControls.NumberBoxDouble textMaxRadius;
        private Panel panelTop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBubblePlot;
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
        private PictureBox background;
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
    }
}
