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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea25 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea26 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea27 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series73 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series74 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series75 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series76 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series77 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series78 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series79 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series80 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series81 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.TextBoxBorder();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.TextBoxBorder();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExt2dtop = new ExtendedControls.ButtonExt();
            this.buttonExt2dfront = new ExtendedControls.ButtonExt();
            this.buttonExt2dside = new ExtendedControls.ButtonExt();
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
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
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
            this.textMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMinRadius.BorderColorScaling = 0.5F;
            this.textMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMinRadius.Location = new System.Drawing.Point(28, 3);
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(35, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.TextChanged += new System.EventHandler(this.textMinRadius_TextChanged);
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(79, 6);
            this.labelExt3.Name = "labelExt3";
            this.labelExt3.Size = new System.Drawing.Size(27, 13);
            this.labelExt3.TabIndex = 3;
            this.labelExt3.Text = "Max";
            // 
            // textMaxRadius
            // 
            this.textMaxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textMaxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMaxRadius.BorderColorScaling = 0.5F;
            this.textMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMaxRadius.Location = new System.Drawing.Point(107, 3);
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(35, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.TextChanged += new System.EventHandler(this.textMaxRadius_TextChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Controls.Add(this.buttonExt2dtop);
            this.panelTop.Controls.Add(this.buttonExt2dfront);
            this.panelTop.Controls.Add(this.buttonExt2dside);
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
            // buttonExt2dtop
            // 
            this.buttonExt2dtop.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dtop.Location = new System.Drawing.Point(185, 0);
            this.buttonExt2dtop.Name = "buttonExt2dtop";
            this.buttonExt2dtop.Size = new System.Drawing.Size(50, 26);
            this.buttonExt2dtop.TabIndex = 6;
            this.buttonExt2dtop.Text = "Top";
            this.buttonExt2dtop.UseVisualStyleBackColor = true;
            this.buttonExt2dtop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dtop_MouseDown);
            // 
            // buttonExt2dfront
            // 
            this.buttonExt2dfront.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dfront.Location = new System.Drawing.Point(235, 0);
            this.buttonExt2dfront.Name = "buttonExt2dfront";
            this.buttonExt2dfront.Size = new System.Drawing.Size(50, 26);
            this.buttonExt2dfront.TabIndex = 7;
            this.buttonExt2dfront.Text = "Front";
            this.buttonExt2dfront.UseVisualStyleBackColor = true;
            this.buttonExt2dfront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dfront_MouseDown);
            // 
            // buttonExt2dside
            // 
            this.buttonExt2dside.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dside.Location = new System.Drawing.Point(285, 0);
            this.buttonExt2dside.Name = "buttonExt2dside";
            this.buttonExt2dside.Size = new System.Drawing.Size(50, 26);
            this.buttonExt2dside.TabIndex = 8;
            this.buttonExt2dside.Text = "Side";
            this.buttonExt2dside.UseVisualStyleBackColor = true;
            this.buttonExt2dside.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dside_MouseDown);
            // 
            // chartBubble
            // 
            this.chartBubble.BackColor = System.Drawing.Color.Black;
            this.chartBubble.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBubble.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea25.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea25.AxisX.LineColor = System.Drawing.Color.White;
            chartArea25.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea25.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea25.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea25.AxisX.MinorGrid.Interval = double.NaN;
            chartArea25.AxisX.MinorGrid.LineColor = System.Drawing.Color.Empty;
            chartArea25.AxisX.Title = "X";
            chartArea25.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea25.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea25.AxisY.LineColor = System.Drawing.Color.White;
            chartArea25.AxisY.MajorGrid.Interval = 0D;
            chartArea25.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea25.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea25.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea25.AxisY.MinorGrid.LineColor = System.Drawing.Color.White;
            chartArea25.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea25.AxisY.Title = "Z";
            chartArea25.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea25.AxisY2.Title = "Y";
            chartArea25.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea25.BackColor = System.Drawing.Color.Black;
            chartArea25.Name = "ChartXZY";
            chartArea26.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea26.AxisX.LineColor = System.Drawing.Color.White;
            chartArea26.AxisX.MajorGrid.Interval = 0D;
            chartArea26.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea26.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea26.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea26.AxisX.Title = "X";
            chartArea26.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea26.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea26.AxisY.LineColor = System.Drawing.Color.White;
            chartArea26.AxisY.MajorGrid.Interval = 0D;
            chartArea26.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea26.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea26.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea26.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea26.AxisY.Title = "Y";
            chartArea26.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea26.AxisY2.Title = "Z";
            chartArea26.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea26.BackColor = System.Drawing.Color.Black;
            chartArea26.Name = "ChartXYZ";
            chartArea26.Visible = false;
            chartArea27.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea27.AxisX.LineColor = System.Drawing.Color.White;
            chartArea27.AxisX.MajorGrid.Interval = 0D;
            chartArea27.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea27.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea27.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea27.AxisX.Title = "Y";
            chartArea27.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea27.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea27.AxisY.LineColor = System.Drawing.Color.White;
            chartArea27.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea27.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea27.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea27.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea27.AxisY.Title = "Z";
            chartArea27.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea27.AxisY2.Title = "X";
            chartArea27.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea27.BackColor = System.Drawing.Color.Black;
            chartArea27.Name = "ChartYZX";
            chartArea27.Visible = false;
            this.chartBubble.ChartAreas.Add(chartArea25);
            this.chartBubble.ChartAreas.Add(chartArea26);
            this.chartBubble.ChartAreas.Add(chartArea27);
            this.chartBubble.Location = new System.Drawing.Point(0, 26);
            this.chartBubble.Margin = new System.Windows.Forms.Padding(0);
            this.chartBubble.Name = "chartBubble";
            series73.ChartArea = "ChartXZY";
            series73.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series73.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series73.MarkerColor = System.Drawing.Color.Red;
            series73.MarkerSize = 4;
            series73.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series73.Name = "CurrentFront";
            series73.YValuesPerPoint = 2;
            series74.ChartArea = "ChartXZY";
            series74.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series74.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series74.MarkerBorderColor = System.Drawing.Color.Teal;
            series74.MarkerColor = System.Drawing.Color.RoyalBlue;
            series74.MarkerSize = 2;
            series74.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series74.Name = "VisitedFront";
            series74.YValuesPerPoint = 2;
            series75.ChartArea = "ChartXZY";
            series75.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series75.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series75.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series75.MarkerColor = System.Drawing.Color.Gold;
            series75.MarkerSize = 2;
            series75.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series75.Name = "UnvisitedFront";
            series75.YValuesPerPoint = 2;
            series76.ChartArea = "ChartXYZ";
            series76.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series76.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series76.MarkerColor = System.Drawing.Color.Red;
            series76.MarkerSize = 4;
            series76.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series76.Name = "CurrentTop";
            series76.YValuesPerPoint = 2;
            series77.ChartArea = "ChartXYZ";
            series77.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series77.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series77.MarkerBorderColor = System.Drawing.Color.Teal;
            series77.MarkerColor = System.Drawing.Color.RoyalBlue;
            series77.MarkerSize = 2;
            series77.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series77.Name = "VisitedTop";
            series77.YValuesPerPoint = 2;
            series78.ChartArea = "ChartXYZ";
            series78.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series78.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series78.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series78.MarkerColor = System.Drawing.Color.Gold;
            series78.MarkerSize = 2;
            series78.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series78.Name = "UnvisitedTop";
            series78.YValuesPerPoint = 2;
            series79.ChartArea = "ChartYZX";
            series79.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series79.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series79.MarkerColor = System.Drawing.Color.Red;
            series79.MarkerSize = 2;
            series79.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series79.Name = "CurrentSide";
            series79.YValuesPerPoint = 2;
            series80.ChartArea = "ChartYZX";
            series80.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series80.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series80.MarkerBorderColor = System.Drawing.Color.Teal;
            series80.MarkerColor = System.Drawing.Color.RoyalBlue;
            series80.MarkerSize = 2;
            series80.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series80.Name = "VisitedSide";
            series80.YValuesPerPoint = 2;
            series81.ChartArea = "ChartYZX";
            series81.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series81.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series81.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series81.MarkerColor = System.Drawing.Color.Gold;
            series81.MarkerSize = 2;
            series81.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series81.Name = "UnvisitedSide";
            series81.YValuesPerPoint = 2;
            this.chartBubble.Series.Add(series73);
            this.chartBubble.Series.Add(series74);
            this.chartBubble.Series.Add(series75);
            this.chartBubble.Series.Add(series76);
            this.chartBubble.Series.Add(series77);
            this.chartBubble.Series.Add(series78);
            this.chartBubble.Series.Add(series79);
            this.chartBubble.Series.Add(series80);
            this.chartBubble.Series.Add(series81);
            this.chartBubble.Size = new System.Drawing.Size(335, 317);
            this.chartBubble.SuppressExceptions = true;
            this.chartBubble.TabIndex = 28;
            this.chartBubble.Text = "Nearest Systems Plot";
            this.chartBubble.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartBubble_MouseDown);
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
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "1:1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem125
            // 
            this.toolStripMenuItem125.Name = "toolStripMenuItem125";
            this.toolStripMenuItem125.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem125.Text = "1.25:1";
            this.toolStripMenuItem125.Click += new System.EventHandler(this.toolStripMenuItem125_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem15.Text = "1.5:1";
            this.toolStripMenuItem15.Click += new System.EventHandler(this.toolStripMenuItem15_Click);
            // 
            // toolStripMenuItem175
            // 
            this.toolStripMenuItem175.Name = "toolStripMenuItem175";
            this.toolStripMenuItem175.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem175.Text = "1.75:1";
            this.toolStripMenuItem175.Click += new System.EventHandler(this.toolStripMenuItem175_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "2:1";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem25.Text = "2.5:1";
            this.toolStripMenuItem25.Click += new System.EventHandler(this.toolStripMenuItem25_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "3:1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem35
            // 
            this.toolStripMenuItem35.Name = "toolStripMenuItem35";
            this.toolStripMenuItem35.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem35.Text = "3.5:1";
            this.toolStripMenuItem35.Click += new System.EventHandler(this.toolStripMenuItem35_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
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
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.chartBubble);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(335, 343);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelExt1;
        private ExtendedControls.TextBoxBorder textMinRadius;
        private System.Windows.Forms.Label labelExt3;
        private ExtendedControls.TextBoxBorder textMaxRadius;
        private Panel panelTop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBubble;
        private ExtendedControls.ButtonExt buttonExt2dtop;
        private ExtendedControls.ButtonExt buttonExt2dfront;
        private ExtendedControls.ButtonExt buttonExt2dside;
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
    }
}
