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
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.TextBoxBorder();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.TextBoxBorder();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExt2dtop = new ExtendedControls.ButtonExt();
            this.buttonExt2dfront = new ExtendedControls.ButtonExt();
            this.buttonExt2dside = new ExtendedControls.ButtonExt();
            this.chartBubble = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).BeginInit();
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
            this.buttonExt2dtop.Location = new System.Drawing.Point(215, 0);
            this.buttonExt2dtop.Name = "buttonExt2dtop";
            this.buttonExt2dtop.Size = new System.Drawing.Size(40, 26);
            this.buttonExt2dtop.TabIndex = 6;
            this.buttonExt2dtop.Text = "Top";
            this.buttonExt2dtop.UseVisualStyleBackColor = true;
            this.buttonExt2dtop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dtop_MouseDown);
            // 
            // buttonExt2dfront
            // 
            this.buttonExt2dfront.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dfront.Location = new System.Drawing.Point(255, 0);
            this.buttonExt2dfront.Name = "buttonExt2dfront";
            this.buttonExt2dfront.Size = new System.Drawing.Size(40, 26);
            this.buttonExt2dfront.TabIndex = 7;
            this.buttonExt2dfront.Text = "Front";
            this.buttonExt2dfront.UseVisualStyleBackColor = true;
            this.buttonExt2dfront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dfront_MouseDown);
            // 
            // buttonExt2dside
            // 
            this.buttonExt2dside.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dside.Location = new System.Drawing.Point(295, 0);
            this.buttonExt2dside.Name = "buttonExt2dside";
            this.buttonExt2dside.Size = new System.Drawing.Size(40, 26);
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
            chartArea1.AxisY.Title = "Z";
            chartArea1.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea1.AxisY2.Title = "Y";
            chartArea1.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartXZY";
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
            chartArea2.AxisY.Title = "Y";
            chartArea2.AxisY.TitleForeColor = System.Drawing.Color.White;
            chartArea2.AxisY2.Title = "Z";
            chartArea2.AxisY2.TitleForeColor = System.Drawing.Color.White;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.Name = "ChartXYZ";
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
            chartArea3.Name = "ChartYZX";
            chartArea3.Visible = false;
            this.chartBubble.ChartAreas.Add(chartArea1);
            this.chartBubble.ChartAreas.Add(chartArea2);
            this.chartBubble.ChartAreas.Add(chartArea3);
            this.chartBubble.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartBubble.Location = new System.Drawing.Point(0, 26);
            this.chartBubble.Margin = new System.Windows.Forms.Padding(0);
            this.chartBubble.Name = "chartBubble";
            series1.ChartArea = "ChartXZY";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerSize = 4;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "CurrentFront";
            series1.YValuesPerPoint = 2;
            series2.ChartArea = "ChartXZY";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series2.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series2.MarkerBorderColor = System.Drawing.Color.Teal;
            series2.MarkerColor = System.Drawing.Color.RoyalBlue;
            series2.MarkerSize = 2;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "VisitedFront";
            series2.YValuesPerPoint = 2;
            series3.ChartArea = "ChartXZY";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series3.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series3.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series3.MarkerColor = System.Drawing.Color.Gold;
            series3.MarkerSize = 2;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "UnvisitedFront";
            series3.YValuesPerPoint = 2;
            series4.ChartArea = "ChartXYZ";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series4.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.MarkerSize = 4;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "CurrentTop";
            series4.YValuesPerPoint = 2;
            series5.ChartArea = "ChartXYZ";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series5.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series5.MarkerBorderColor = System.Drawing.Color.Teal;
            series5.MarkerColor = System.Drawing.Color.RoyalBlue;
            series5.MarkerSize = 2;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "VisitedTop";
            series5.YValuesPerPoint = 2;
            series6.ChartArea = "ChartXYZ";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series6.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series6.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series6.MarkerColor = System.Drawing.Color.Gold;
            series6.MarkerSize = 2;
            series6.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series6.Name = "UnvisitedTop";
            series6.YValuesPerPoint = 2;
            series7.ChartArea = "ChartYZX";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series7.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series7.MarkerColor = System.Drawing.Color.Red;
            series7.MarkerSize = 2;
            series7.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series7.Name = "CurrentSide";
            series7.YValuesPerPoint = 2;
            series8.ChartArea = "ChartYZX";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series8.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series8.MarkerBorderColor = System.Drawing.Color.Teal;
            series8.MarkerColor = System.Drawing.Color.RoyalBlue;
            series8.MarkerSize = 2;
            series8.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series8.Name = "VisitedSide";
            series8.YValuesPerPoint = 2;
            series9.ChartArea = "ChartYZX";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series9.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series9.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series9.MarkerColor = System.Drawing.Color.Gold;
            series9.MarkerSize = 2;
            series9.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series9.Name = "UnvisitedSide";
            series9.YValuesPerPoint = 2;
            this.chartBubble.Series.Add(series1);
            this.chartBubble.Series.Add(series2);
            this.chartBubble.Series.Add(series3);
            this.chartBubble.Series.Add(series4);
            this.chartBubble.Series.Add(series5);
            this.chartBubble.Series.Add(series6);
            this.chartBubble.Series.Add(series7);
            this.chartBubble.Series.Add(series8);
            this.chartBubble.Series.Add(series9);
            this.chartBubble.Size = new System.Drawing.Size(335, 317);
            this.chartBubble.SuppressExceptions = true;
            this.chartBubble.TabIndex = 28;
            this.chartBubble.Text = "Nearest Systems Plot";
            // 
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.chartBubble);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(335, 343);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBubble)).EndInit();
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
    }
}
