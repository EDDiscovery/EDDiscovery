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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.panelTop = new System.Windows.Forms.Panel();
            this.checkBoxDotSize = new ExtendedControls.CheckBoxCustom();
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            // chartBubble
            // 
            this.chartBubble.BackColor = System.Drawing.Color.Black;
            this.chartBubble.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartBubble.BorderSkin.BorderColor = System.Drawing.Color.White;
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
            chartArea1.AxisY2.Title = "Y";
            chartArea1.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartXYZ-Top";
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea2.AxisX.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MajorGrid.Interval = 0D;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.Title = "X";
            chartArea2.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea2.AxisY.LineColor = System.Drawing.Color.White;
            chartArea2.AxisY.MajorGrid.Interval = 0D;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea2.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea2.AxisY.Title = "Z";
            chartArea2.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea2.AxisY2.Title = "Z";
            chartArea2.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.Name = "ChartXZY-Front";
            chartArea2.Visible = false;
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea3.AxisX.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.MajorGrid.Interval = 0D;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea3.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX.Title = "Y";
            chartArea3.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.DarkGray;
            chartArea3.AxisY.LineColor = System.Drawing.Color.White;
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea3.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea3.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea3.AxisY.Title = "Z";
            chartArea3.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea3.AxisY2.Title = "X";
            chartArea3.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea3.BackColor = System.Drawing.Color.Black;
            chartArea3.Name = "ChartYZX-Side";
            chartArea3.Visible = false;
            this.chartBubble.ChartAreas.Add(chartArea1);
            this.chartBubble.ChartAreas.Add(chartArea2);
            this.chartBubble.ChartAreas.Add(chartArea3);
            this.chartBubble.Location = new System.Drawing.Point(19, 41);
            this.chartBubble.Margin = new System.Windows.Forms.Padding(0);
            this.chartBubble.Name = "chartBubble";
            series1.ChartArea = "ChartXYZ-Top";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerSize = 4;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "CurrentFront";
            series1.YValuesPerPoint = 2;
            series2.ChartArea = "ChartXYZ-Top";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series2.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series2.MarkerBorderColor = System.Drawing.Color.Teal;
            series2.MarkerColor = System.Drawing.Color.RoyalBlue;
            series2.MarkerSize = 2;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "VisitedFront";
            series2.YValuesPerPoint = 2;
            series3.ChartArea = "ChartXYZ-Top";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series3.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series3.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series3.MarkerColor = System.Drawing.Color.Gold;
            series3.MarkerSize = 2;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "UnvisitedFront";
            series3.YValuesPerPoint = 2;
            series4.ChartArea = "ChartXZY-Front";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series4.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.MarkerSize = 4;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "CurrentTop";
            series4.YValuesPerPoint = 2;
            series5.ChartArea = "ChartXZY-Front";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series5.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series5.MarkerBorderColor = System.Drawing.Color.Teal;
            series5.MarkerColor = System.Drawing.Color.RoyalBlue;
            series5.MarkerSize = 2;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "VisitedTop";
            series5.YValuesPerPoint = 2;
            series6.ChartArea = "ChartXZY-Front";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series6.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series6.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series6.MarkerColor = System.Drawing.Color.Gold;
            series6.MarkerSize = 2;
            series6.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series6.Name = "UnvisitedTop";
            series6.YValuesPerPoint = 2;
            series7.ChartArea = "ChartYZX-Side";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series7.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series7.MarkerColor = System.Drawing.Color.Red;
            series7.MarkerSize = 2;
            series7.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series7.Name = "CurrentSide";
            series7.YValuesPerPoint = 2;
            series8.ChartArea = "ChartYZX-Side";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series8.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series8.MarkerBorderColor = System.Drawing.Color.Teal;
            series8.MarkerColor = System.Drawing.Color.RoyalBlue;
            series8.MarkerSize = 2;
            series8.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series8.Name = "VisitedSide";
            series8.YValuesPerPoint = 2;
            series9.ChartArea = "ChartYZX-Side";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series9.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series9.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series9.MarkerColor = System.Drawing.Color.Gold;
            series9.MarkerSize = 2;
            series9.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series9.Name = "UnvisitedSide";
            series9.YValuesPerPoint = 2;
            series10.ChartArea = "ChartXZY-Front";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series10.MarkerColor = System.Drawing.Color.Purple;
            series10.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series10.Name = "PreviousFront";
            series10.YValuesPerPoint = 2;
            series11.ChartArea = "ChartXYZ-Top";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series11.MarkerColor = System.Drawing.Color.Purple;
            series11.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series11.Name = "PreviousTop";
            series11.YValuesPerPoint = 2;
            series12.ChartArea = "ChartYZX-Side";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series12.MarkerColor = System.Drawing.Color.Purple;
            series12.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series12.Name = "PreviousSide";
            series12.YValuesPerPoint = 2;
            this.chartBubble.Series.Add(series1);
            this.chartBubble.Series.Add(series2);
            this.chartBubble.Series.Add(series3);
            this.chartBubble.Series.Add(series4);
            this.chartBubble.Series.Add(series5);
            this.chartBubble.Series.Add(series6);
            this.chartBubble.Series.Add(series7);
            this.chartBubble.Series.Add(series8);
            this.chartBubble.Series.Add(series9);
            this.chartBubble.Series.Add(series10);
            this.chartBubble.Series.Add(series11);
            this.chartBubble.Series.Add(series12);
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
        private ExtendedControls.CheckBoxCustom checkBoxDotSize;
        private ToolTip toolTip1;
    }
}
