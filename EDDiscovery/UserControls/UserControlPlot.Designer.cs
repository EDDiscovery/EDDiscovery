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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series22 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series23 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series24 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series25 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series26 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series27 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.panelTop = new System.Windows.Forms.Panel();
            this.comboBoxView = new ExtendedControls.ComboBoxCustom();
            this.chartBubble = new System.Windows.Forms.DataVisualization.Charting.Chart();
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
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.background)).BeginInit();
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
            this.textMinRadius.Size = new System.Drawing.Size(35, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.Value = 0D;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(75, 6);
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
            this.textMaxRadius.Location = new System.Drawing.Point(103, 3);
            this.textMaxRadius.Maximum = 100000D;
            this.textMaxRadius.Minimum = 0D;
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(35, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
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
            // comboBoxView
            // 
            this.comboBoxView.ArrowWidth = 1;
            this.comboBoxView.BorderColor = System.Drawing.Color.White;
            this.comboBoxView.ButtonColorScaling = 0.5F;
            this.comboBoxView.DataSource = null;
            this.comboBoxView.DisplayMember = "";
            this.comboBoxView.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxView.DropDownHeight = 106;
            this.comboBoxView.DropDownWidth = 75;
            this.comboBoxView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxView.ItemHeight = 13;
            this.comboBoxView.Location = new System.Drawing.Point(153, 3);
            this.comboBoxView.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxView.Name = "comboBoxView";
            this.comboBoxView.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxView.ScrollBarWidth = 16;
            this.comboBoxView.SelectedIndex = -1;
            this.comboBoxView.SelectedItem = null;
            this.comboBoxView.SelectedValue = null;
            this.comboBoxView.Size = new System.Drawing.Size(60, 21);
            this.comboBoxView.TabIndex = 9;
            this.comboBoxView.Text = "comboBoxCustom1";
            this.comboBoxView.ValueMember = "";
            this.comboBoxView.SelectedIndexChanged += new System.EventHandler(this.comboBoxView_SelectedIndexChanged);
            // 
            // chartBubble
            // 
            this.chartBubble.BackColor = System.Drawing.Color.Black;
            this.chartBubble.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBubble.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea7.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea7.AxisX.LineColor = System.Drawing.Color.White;
            chartArea7.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea7.AxisX.MinorGrid.Interval = double.NaN;
            chartArea7.AxisX.MinorGrid.LineColor = System.Drawing.Color.Empty;
            chartArea7.AxisX.Title = "X";
            chartArea7.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea7.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea7.AxisY.LineColor = System.Drawing.Color.White;
            chartArea7.AxisY.MajorGrid.Interval = 0D;
            chartArea7.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea7.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea7.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea7.AxisY.MinorGrid.LineColor = System.Drawing.Color.White;
            chartArea7.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea7.AxisY.Title = "Y";
            chartArea7.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea7.AxisY2.Title = "Y";
            chartArea7.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea7.BackColor = System.Drawing.Color.Black;
            chartArea7.Name = "ChartXYZ-Top";
            chartArea8.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea8.AxisX.LineColor = System.Drawing.Color.White;
            chartArea8.AxisX.MajorGrid.Interval = 0D;
            chartArea8.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea8.AxisX.Title = "X";
            chartArea8.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea8.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea8.AxisY.LineColor = System.Drawing.Color.White;
            chartArea8.AxisY.MajorGrid.Interval = 0D;
            chartArea8.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea8.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea8.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea8.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea8.AxisY.Title = "Z";
            chartArea8.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea8.AxisY2.Title = "Z";
            chartArea8.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea8.BackColor = System.Drawing.Color.Black;
            chartArea8.Name = "ChartXZY-Front";
            chartArea8.Visible = false;
            chartArea9.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea9.AxisX.LineColor = System.Drawing.Color.White;
            chartArea9.AxisX.MajorGrid.Interval = 0D;
            chartArea9.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea9.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea9.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea9.AxisX.Title = "Y";
            chartArea9.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea9.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea9.AxisY.LineColor = System.Drawing.Color.White;
            chartArea9.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea9.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea9.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea9.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea9.AxisY.Title = "Z";
            chartArea9.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea9.AxisY2.Title = "X";
            chartArea9.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea9.BackColor = System.Drawing.Color.Black;
            chartArea9.Name = "ChartYZX-Side";
            chartArea9.Visible = false;
            this.chartBubble.ChartAreas.Add(chartArea7);
            this.chartBubble.ChartAreas.Add(chartArea8);
            this.chartBubble.ChartAreas.Add(chartArea9);
            this.chartBubble.Location = new System.Drawing.Point(19, 41);
            this.chartBubble.Margin = new System.Windows.Forms.Padding(0);
            this.chartBubble.Name = "chartBubble";
            series19.ChartArea = "ChartXYZ-Top";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series19.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series19.MarkerColor = System.Drawing.Color.Red;
            series19.MarkerSize = 4;
            series19.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series19.Name = "CurrentFront";
            series19.YValuesPerPoint = 2;
            series20.ChartArea = "ChartXYZ-Top";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series20.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series20.MarkerBorderColor = System.Drawing.Color.Teal;
            series20.MarkerColor = System.Drawing.Color.RoyalBlue;
            series20.MarkerSize = 2;
            series20.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series20.Name = "VisitedFront";
            series20.YValuesPerPoint = 2;
            series21.ChartArea = "ChartXYZ-Top";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series21.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series21.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series21.MarkerColor = System.Drawing.Color.Gold;
            series21.MarkerSize = 2;
            series21.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series21.Name = "UnvisitedFront";
            series21.YValuesPerPoint = 2;
            series22.ChartArea = "ChartXZY-Front";
            series22.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series22.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series22.MarkerColor = System.Drawing.Color.Red;
            series22.MarkerSize = 4;
            series22.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series22.Name = "CurrentTop";
            series22.YValuesPerPoint = 2;
            series23.ChartArea = "ChartXZY-Front";
            series23.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series23.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series23.MarkerBorderColor = System.Drawing.Color.Teal;
            series23.MarkerColor = System.Drawing.Color.RoyalBlue;
            series23.MarkerSize = 2;
            series23.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series23.Name = "VisitedTop";
            series23.YValuesPerPoint = 2;
            series24.ChartArea = "ChartXZY-Front";
            series24.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series24.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series24.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series24.MarkerColor = System.Drawing.Color.Gold;
            series24.MarkerSize = 2;
            series24.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series24.Name = "UnvisitedTop";
            series24.YValuesPerPoint = 2;
            series25.ChartArea = "ChartYZX-Side";
            series25.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series25.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series25.MarkerColor = System.Drawing.Color.Red;
            series25.MarkerSize = 2;
            series25.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series25.Name = "CurrentSide";
            series25.YValuesPerPoint = 2;
            series26.ChartArea = "ChartYZX-Side";
            series26.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series26.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series26.MarkerBorderColor = System.Drawing.Color.Teal;
            series26.MarkerColor = System.Drawing.Color.RoyalBlue;
            series26.MarkerSize = 2;
            series26.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series26.Name = "VisitedSide";
            series26.YValuesPerPoint = 2;
            series27.ChartArea = "ChartYZX-Side";
            series27.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series27.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series27.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series27.MarkerColor = System.Drawing.Color.Gold;
            series27.MarkerSize = 2;
            series27.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series27.Name = "UnvisitedSide";
            series27.YValuesPerPoint = 2;
            this.chartBubble.Series.Add(series19);
            this.chartBubble.Series.Add(series20);
            this.chartBubble.Series.Add(series21);
            this.chartBubble.Series.Add(series22);
            this.chartBubble.Series.Add(series23);
            this.chartBubble.Series.Add(series24);
            this.chartBubble.Series.Add(series25);
            this.chartBubble.Series.Add(series26);
            this.chartBubble.Series.Add(series27);
            this.chartBubble.Size = new System.Drawing.Size(301, 283);
            this.chartBubble.SuppressExceptions = true;
            this.chartBubble.TabIndex = 28;
            this.chartBubble.Text = "Nearest Systems Plot";
            this.chartBubble.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartBubble_MouseDown);
            this.chartBubble.MouseEnter += new System.EventHandler(this.chartBubble_MouseEnter);
            this.chartBubble.MouseLeave += new System.EventHandler(this.chartBubble_MouseLeave);
            this.chartBubble.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartBubble_MouseMove);
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
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.chartBubble);
            this.Controls.Add(this.background);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(335, 343);
            this.Resize += new System.EventHandler(this.UserControlPlot_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelExt1;
        private ExtendedControls.NumberBoxDouble textMinRadius;
        private System.Windows.Forms.Label labelExt3;
        private ExtendedControls.NumberBoxDouble textMaxRadius;
        private Panel panelTop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBubble;
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
    }
}
