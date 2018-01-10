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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.TextBoxBorder();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.TextBoxBorder();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExt2dtop = new ExtendedControls.ButtonExt();
            this.buttonExt2dfront = new ExtendedControls.ButtonExt();
            this.buttonExt2dside = new ExtendedControls.ButtonExt();
            this.chartXY = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartXZ = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartYZ = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartYZ)).BeginInit();
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
            this.textMinRadius.Size = new System.Drawing.Size(40, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.TextChanged += new System.EventHandler(this.textMinRadius_TextChanged);
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(81, 6);
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
            this.textMaxRadius.Location = new System.Drawing.Point(109, 3);
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(40, 20);
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
            this.buttonExt2dtop.Location = new System.Drawing.Point(165, 0);
            this.buttonExt2dtop.Name = "buttonExt2dtop";
            this.buttonExt2dtop.Size = new System.Drawing.Size(55, 26);
            this.buttonExt2dtop.TabIndex = 6;
            this.buttonExt2dtop.Text = "2D Top";
            this.buttonExt2dtop.UseVisualStyleBackColor = true;
            this.buttonExt2dtop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dtop_MouseDown);
            // 
            // buttonExt2dfront
            // 
            this.buttonExt2dfront.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dfront.Location = new System.Drawing.Point(220, 0);
            this.buttonExt2dfront.Name = "buttonExt2dfront";
            this.buttonExt2dfront.Size = new System.Drawing.Size(60, 26);
            this.buttonExt2dfront.TabIndex = 7;
            this.buttonExt2dfront.Text = "2D Front";
            this.buttonExt2dfront.UseVisualStyleBackColor = true;
            this.buttonExt2dfront.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dfront_MouseDown);
            // 
            // buttonExt2dside
            // 
            this.buttonExt2dside.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExt2dside.Location = new System.Drawing.Point(280, 0);
            this.buttonExt2dside.Name = "buttonExt2dside";
            this.buttonExt2dside.Size = new System.Drawing.Size(55, 26);
            this.buttonExt2dside.TabIndex = 8;
            this.buttonExt2dside.Text = "2D Side";
            this.buttonExt2dside.UseVisualStyleBackColor = true;
            this.buttonExt2dside.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExt2dside_MouseDown);
            // 
            // chartXY
            // 
            this.chartXY.BackColor = System.Drawing.Color.Black;
            this.chartXY.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartXY.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea1.Area3DStyle.Inclination = 45;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX.LineColor = System.Drawing.Color.White;
            chartArea1.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY.LineColor = System.Drawing.Color.White;
            chartArea1.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartAreaXY";
            this.chartXY.ChartAreas.Add(chartArea1);
            this.chartXY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartXY.Location = new System.Drawing.Point(0, 26);
            this.chartXY.Margin = new System.Windows.Forms.Padding(0);
            this.chartXY.Name = "chartXY";
            series1.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series1.ChartArea = "ChartAreaXY";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.Color = System.Drawing.Color.Transparent;
            series1.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series1.MarkerColor = System.Drawing.Color.Red;
            series1.MarkerSize = 4;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Current";
            series1.YValuesPerPoint = 2;
            series2.ChartArea = "ChartAreaXY";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series2.Color = System.Drawing.Color.Transparent;
            series2.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series2.IsVisibleInLegend = false;
            series2.MarkerBorderColor = System.Drawing.Color.Teal;
            series2.MarkerColor = System.Drawing.Color.RoyalBlue;
            series2.MarkerSize = 2;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Visited";
            series2.YValuesPerPoint = 2;
            series3.ChartArea = "ChartAreaXY";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series3.Color = System.Drawing.Color.Transparent;
            series3.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series3.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series3.MarkerColor = System.Drawing.Color.Gold;
            series3.MarkerSize = 2;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Unvisited";
            series3.YValuesPerPoint = 2;
            this.chartXY.Series.Add(series1);
            this.chartXY.Series.Add(series2);
            this.chartXY.Series.Add(series3);
            this.chartXY.Size = new System.Drawing.Size(335, 317);
            this.chartXY.SuppressExceptions = true;
            this.chartXY.TabIndex = 27;
            this.chartXY.Text = "Nearest Systems Plot";
            title1.ForeColor = System.Drawing.Color.White;
            title1.Name = "XY";
            title1.Text = "Nearest Systems Plot X,Y + Z ";
            this.chartXY.Titles.Add(title1);
            this.chartXY.Visible = false;
            // 
            // chartXZ
            // 
            this.chartXZ.BackColor = System.Drawing.Color.Black;
            this.chartXZ.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartXZ.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea2.Area3DStyle.Inclination = 45;
            chartArea2.Area3DStyle.IsRightAngleAxes = false;
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisY.LineColor = System.Drawing.Color.White;
            chartArea2.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea2.BackColor = System.Drawing.Color.Black;
            chartArea2.BackSecondaryColor = System.Drawing.Color.White;
            chartArea2.Name = "ChartArea1";
            this.chartXZ.ChartAreas.Add(chartArea2);
            this.chartXZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartXZ.Location = new System.Drawing.Point(0, 26);
            this.chartXZ.Margin = new System.Windows.Forms.Padding(0);
            this.chartXZ.Name = "chartXZ";
            series4.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series4.Color = System.Drawing.Color.Transparent;
            series4.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.MarkerSize = 4;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "Current";
            series4.YValuesPerPoint = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series5.Color = System.Drawing.Color.Transparent;
            series5.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series5.IsVisibleInLegend = false;
            series5.MarkerBorderColor = System.Drawing.Color.Teal;
            series5.MarkerColor = System.Drawing.Color.RoyalBlue;
            series5.MarkerSize = 2;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "Visited";
            series5.YValuesPerPoint = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series6.Color = System.Drawing.Color.Transparent;
            series6.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series6.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series6.MarkerColor = System.Drawing.Color.Gold;
            series6.MarkerSize = 2;
            series6.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series6.Name = "Unvisited";
            series6.YValuesPerPoint = 2;
            this.chartXZ.Series.Add(series4);
            this.chartXZ.Series.Add(series5);
            this.chartXZ.Series.Add(series6);
            this.chartXZ.Size = new System.Drawing.Size(335, 317);
            this.chartXZ.SuppressExceptions = true;
            this.chartXZ.TabIndex = 28;
            this.chartXZ.Text = "Nearest Systems Plot";
            title2.ForeColor = System.Drawing.Color.White;
            title2.Name = "XZ";
            title2.Text = "Nearest Systems Plot X,Z + Y";
            this.chartXZ.Titles.Add(title2);
            // 
            // chartYZ
            // 
            this.chartYZ.BackColor = System.Drawing.Color.Black;
            this.chartYZ.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartYZ.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea3.Area3DStyle.Inclination = 45;
            chartArea3.Area3DStyle.IsRightAngleAxes = false;
            chartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisX.LineColor = System.Drawing.Color.White;
            chartArea3.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisY.LineColor = System.Drawing.Color.White;
            chartArea3.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea3.BackColor = System.Drawing.Color.Black;
            chartArea3.BackSecondaryColor = System.Drawing.Color.White;
            chartArea3.Name = "ChartAreaXY";
            this.chartYZ.ChartAreas.Add(chartArea3);
            this.chartYZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartYZ.Location = new System.Drawing.Point(0, 26);
            this.chartYZ.Margin = new System.Windows.Forms.Padding(0);
            this.chartYZ.Name = "chartYZ";
            series7.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series7.ChartArea = "ChartAreaXY";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series7.Color = System.Drawing.Color.Transparent;
            series7.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=4";
            series7.MarkerColor = System.Drawing.Color.Red;
            series7.MarkerSize = 4;
            series7.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series7.Name = "Current";
            series7.YValuesPerPoint = 2;
            series8.ChartArea = "ChartAreaXY";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series8.Color = System.Drawing.Color.Transparent;
            series8.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series8.IsVisibleInLegend = false;
            series8.MarkerBorderColor = System.Drawing.Color.Teal;
            series8.MarkerColor = System.Drawing.Color.RoyalBlue;
            series8.MarkerSize = 2;
            series8.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series8.Name = "Visited";
            series8.YValuesPerPoint = 2;
            series9.ChartArea = "ChartAreaXY";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series9.Color = System.Drawing.Color.Transparent;
            series9.CustomProperties = "BubbleMinSize=2, BubbleMaxSize=8";
            series9.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series9.MarkerColor = System.Drawing.Color.Gold;
            series9.MarkerSize = 2;
            series9.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series9.Name = "Unvisited";
            series9.YValuesPerPoint = 2;
            this.chartYZ.Series.Add(series7);
            this.chartYZ.Series.Add(series8);
            this.chartYZ.Series.Add(series9);
            this.chartYZ.Size = new System.Drawing.Size(335, 317);
            this.chartYZ.SuppressExceptions = true;
            this.chartYZ.TabIndex = 29;
            this.chartYZ.Text = "Nearest Systems Plot";
            title3.ForeColor = System.Drawing.Color.White;
            title3.Name = "YZ";
            title3.Text = "Nearest Systems Plot Y,Z + X";
            this.chartYZ.Titles.Add(title3);
            this.chartYZ.Visible = false;
            // 
            // UserControlPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.chartXZ);
            this.Controls.Add(this.chartXY);
            this.Controls.Add(this.chartYZ);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlPlot";
            this.Size = new System.Drawing.Size(335, 343);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartYZ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelExt1;
        private ExtendedControls.TextBoxBorder textMinRadius;
        private System.Windows.Forms.Label labelExt3;
        private ExtendedControls.TextBoxBorder textMaxRadius;
        private Panel panelTop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXY;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXZ;
        private ExtendedControls.ButtonExt buttonExt2dtop;
        private ExtendedControls.ButtonExt buttonExt2dfront;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartYZ;
        private ExtendedControls.ButtonExt buttonExt2dside;
    }
}
