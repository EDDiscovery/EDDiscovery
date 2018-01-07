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
    partial class UserControlRadar
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series22 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series23 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series24 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.TextBoxBorder();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.TextBoxBorder();
            this.panelTop = new System.Windows.Forms.Panel();
            this.chartXY = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkBoxSwitchCharts = new ExtendedControls.CheckBoxCustom();
            this.chartXZ = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXZ)).BeginInit();
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
            this.textMinRadius.Location = new System.Drawing.Point(33, 3);
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(50, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.TextChanged += new System.EventHandler(this.textMinRadius_TextChanged);
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(96, 6);
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
            this.textMaxRadius.Location = new System.Drawing.Point(129, 3);
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(50, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.TextChanged += new System.EventHandler(this.textMaxRadius_TextChanged);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.checkBoxSwitchCharts);
            this.panelTop.Controls.Add(this.labelExt1);
            this.panelTop.Controls.Add(this.textMinRadius);
            this.panelTop.Controls.Add(this.labelExt3);
            this.panelTop.Controls.Add(this.textMaxRadius);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(335, 31);
            this.panelTop.TabIndex = 25;
            // 
            // chartXY
            // 
            this.chartXY.BackColor = System.Drawing.Color.Black;
            this.chartXY.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartXY.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea7.Area3DStyle.Inclination = 45;
            chartArea7.Area3DStyle.IsRightAngleAxes = false;
            chartArea7.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea7.AxisX.LineColor = System.Drawing.Color.White;
            chartArea7.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea7.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea7.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea7.AxisY.LineColor = System.Drawing.Color.White;
            chartArea7.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea7.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea7.BackColor = System.Drawing.Color.Black;
            chartArea7.BackSecondaryColor = System.Drawing.Color.White;
            chartArea7.Name = "ChartArea1";
            this.chartXY.ChartAreas.Add(chartArea7);
            this.chartXY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartXY.Location = new System.Drawing.Point(0, 31);
            this.chartXY.Margin = new System.Windows.Forms.Padding(0);
            this.chartXY.Name = "chartXY";
            series19.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series19.ChartArea = "ChartArea1";
            series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series19.Color = System.Drawing.Color.Transparent;
            series19.CustomProperties = "BubbleMinSize=6, BubbleMaxSize=6";
            series19.LabelToolTip = "#LABEL";
            series19.MarkerColor = System.Drawing.Color.Gray;
            series19.MarkerSize = 6;
            series19.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series19.Name = "Current";
            series19.YValuesPerPoint = 2;
            series20.ChartArea = "ChartArea1";
            series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series20.Color = System.Drawing.Color.Transparent;
            series20.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=16";
            series20.IsVisibleInLegend = false;
            series20.LabelToolTip = "#LABEL";
            series20.MarkerBorderColor = System.Drawing.Color.Teal;
            series20.MarkerColor = System.Drawing.Color.RoyalBlue;
            series20.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series20.Name = "Visited";
            series20.YValuesPerPoint = 2;
            series21.ChartArea = "ChartArea1";
            series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series21.Color = System.Drawing.Color.Transparent;
            series21.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=16";
            series21.LabelToolTip = "#LABEL";
            series21.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series21.MarkerColor = System.Drawing.Color.Gold;
            series21.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series21.Name = "Unvisited";
            series21.YValuesPerPoint = 2;
            this.chartXY.Series.Add(series19);
            this.chartXY.Series.Add(series20);
            this.chartXY.Series.Add(series21);
            this.chartXY.Size = new System.Drawing.Size(335, 312);
            this.chartXY.SuppressExceptions = true;
            this.chartXY.TabIndex = 27;
            this.chartXY.Text = "chart2";
            // 
            // checkBoxSwitchCharts
            // 
            this.checkBoxSwitchCharts.AutoSize = true;
            this.checkBoxSwitchCharts.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxSwitchCharts.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxSwitchCharts.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxSwitchCharts.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxSwitchCharts.FontNerfReduction = 0.5F;
            this.checkBoxSwitchCharts.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxSwitchCharts.Location = new System.Drawing.Point(246, 0);
            this.checkBoxSwitchCharts.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxSwitchCharts.Name = "checkBoxSwitchCharts";
            this.checkBoxSwitchCharts.Size = new System.Drawing.Size(89, 31);
            this.checkBoxSwitchCharts.TabIndex = 4;
            this.checkBoxSwitchCharts.Text = "Switch Views";
            this.checkBoxSwitchCharts.TickBoxReductionSize = 10;
            this.checkBoxSwitchCharts.UseVisualStyleBackColor = true;
            this.checkBoxSwitchCharts.CheckedChanged += new System.EventHandler(this.checkBoxSwitchCharts_CheckedChanged);
            this.checkBoxSwitchCharts.CheckStateChanged += new System.EventHandler(this.checkBoxSwitchCharts_CheckStateChanged);
            // 
            // chartXZ
            // 
            this.chartXZ.BackColor = System.Drawing.Color.Black;
            this.chartXZ.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartXZ.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea8.Area3DStyle.Inclination = 45;
            chartArea8.Area3DStyle.IsRightAngleAxes = false;
            chartArea8.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea8.AxisX.LineColor = System.Drawing.Color.White;
            chartArea8.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea8.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea8.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea8.AxisY.LineColor = System.Drawing.Color.White;
            chartArea8.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea8.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea8.BackColor = System.Drawing.Color.Black;
            chartArea8.BackSecondaryColor = System.Drawing.Color.White;
            chartArea8.Name = "ChartArea1";
            this.chartXZ.ChartAreas.Add(chartArea8);
            this.chartXZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartXZ.Location = new System.Drawing.Point(0, 31);
            this.chartXZ.Margin = new System.Windows.Forms.Padding(0);
            this.chartXZ.Name = "chartXZ";
            series22.BackImageTransparentColor = System.Drawing.Color.Transparent;
            series22.ChartArea = "ChartArea1";
            series22.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series22.Color = System.Drawing.Color.Transparent;
            series22.CustomProperties = "BubbleMinSize=6, BubbleMaxSize=6";
            series22.LabelToolTip = "#LABEL";
            series22.MarkerColor = System.Drawing.Color.Gray;
            series22.MarkerSize = 6;
            series22.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series22.Name = "Current";
            series22.YValuesPerPoint = 2;
            series23.ChartArea = "ChartArea1";
            series23.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series23.Color = System.Drawing.Color.Transparent;
            series23.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=16";
            series23.IsVisibleInLegend = false;
            series23.LabelToolTip = "#LABEL";
            series23.MarkerBorderColor = System.Drawing.Color.Teal;
            series23.MarkerColor = System.Drawing.Color.RoyalBlue;
            series23.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series23.Name = "Visited";
            series23.YValuesPerPoint = 2;
            series24.ChartArea = "ChartArea1";
            series24.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series24.Color = System.Drawing.Color.Transparent;
            series24.CustomProperties = "BubbleMinSize=4, BubbleMaxSize=16";
            series24.LabelToolTip = "#LABEL";
            series24.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            series24.MarkerColor = System.Drawing.Color.Gold;
            series24.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series24.Name = "Unvisited";
            series24.YValuesPerPoint = 2;
            this.chartXZ.Series.Add(series22);
            this.chartXZ.Series.Add(series23);
            this.chartXZ.Series.Add(series24);
            this.chartXZ.Size = new System.Drawing.Size(335, 312);
            this.chartXZ.SuppressExceptions = true;
            this.chartXZ.TabIndex = 28;
            this.chartXZ.Text = "chart2";
            this.chartXZ.Visible = false;
            // 
            // UserControlRadar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartXY);
            this.Controls.Add(this.chartXZ);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlRadar";
            this.Size = new System.Drawing.Size(335, 343);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartXY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartXZ)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelExt1;
        private ExtendedControls.TextBoxBorder textMinRadius;
        private System.Windows.Forms.Label labelExt3;
        private ExtendedControls.TextBoxBorder textMaxRadius;
        private Panel panelTop;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXY;
        private ExtendedControls.CheckBoxCustom checkBoxSwitchCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartXZ;
    }
}
