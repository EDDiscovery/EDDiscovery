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
    partial class UserControlLocalMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLocalMap));
            this.labelExtMin = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExtMax = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.slideMaxItems = new System.Windows.Forms.TrackBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.extAstroPlot = new ExtendedControls.Controls.ExtAstroPlot();
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).BeginInit();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelExtMin
            // 
            this.labelExtMin.AutoSize = true;
            this.labelExtMin.Location = new System.Drawing.Point(0, 1);
            this.labelExtMin.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelExtMin.Name = "labelExtMin";
            this.labelExtMin.Size = new System.Drawing.Size(24, 13);
            this.labelExtMin.TabIndex = 3;
            this.labelExtMin.Text = "Min";
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
            this.textMinRadius.EndButtonEnable = true;
            this.textMinRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textMinRadius.EndButtonImage")));
            this.textMinRadius.EndButtonVisible = false;
            this.textMinRadius.Format = "0.#######";
            this.textMinRadius.InErrorCondition = false;
            this.textMinRadius.Location = new System.Drawing.Point(32, 1);
            this.textMinRadius.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.textMinRadius.Maximum = 100000D;
            this.textMinRadius.Minimum = 0D;
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(40, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.Value = 0D;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.ValueChanged += new System.EventHandler(this.TextMinRadius_ValueChanged);
            // 
            // labelExtMax
            // 
            this.labelExtMax.AutoSize = true;
            this.labelExtMax.Location = new System.Drawing.Point(80, 1);
            this.labelExtMax.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelExtMax.Name = "labelExtMax";
            this.labelExtMax.Size = new System.Drawing.Size(27, 13);
            this.labelExtMax.TabIndex = 3;
            this.labelExtMax.Text = "Max";
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
            this.textMaxRadius.EndButtonEnable = true;
            this.textMaxRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textMaxRadius.EndButtonImage")));
            this.textMaxRadius.EndButtonVisible = false;
            this.textMaxRadius.Format = "0.#######";
            this.textMaxRadius.InErrorCondition = false;
            this.textMaxRadius.Location = new System.Drawing.Point(115, 1);
            this.textMaxRadius.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.textMaxRadius.Maximum = 100000D;
            this.textMaxRadius.Minimum = 0D;
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(40, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.TextMaxRadius_ValueChanged);
            // 
            // slideMaxItems
            // 
            this.slideMaxItems.AutoSize = false;
            this.slideMaxItems.LargeChange = 50;
            this.slideMaxItems.Location = new System.Drawing.Point(163, 1);
            this.slideMaxItems.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.slideMaxItems.Maximum = 500;
            this.slideMaxItems.Minimum = 50;
            this.slideMaxItems.Name = "slideMaxItems";
            this.slideMaxItems.Size = new System.Drawing.Size(90, 24);
            this.slideMaxItems.SmallChange = 10;
            this.slideMaxItems.TabIndex = 4;
            this.slideMaxItems.TickFrequency = 50;
            this.slideMaxItems.Value = 250;
            this.slideMaxItems.Scroll += new System.EventHandler(this.SlideMaxItems_Scroll);
            this.slideMaxItems.MouseHover += new System.EventHandler(this.SlideMaxItems_MouseHover);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel.Controls.Add(this.labelExtMin);
            this.flowLayoutPanel.Controls.Add(this.textMinRadius);
            this.flowLayoutPanel.Controls.Add(this.labelExtMax);
            this.flowLayoutPanel.Controls.Add(this.textMaxRadius);
            this.flowLayoutPanel.Controls.Add(this.slideMaxItems);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(380, 26);
            this.flowLayoutPanel.TabIndex = 5;
            // 
            // extAstroPlot
            // 
            this.extAstroPlot.AxesLength = 10;
            this.extAstroPlot.AxesThickness = 3;
            this.extAstroPlot.AxesWidget = true;
            this.extAstroPlot.Azimuth = 0.3D;
            this.extAstroPlot.BackColor = System.Drawing.Color.Black;
            this.extAstroPlot.Camera = new double[] {
        -1.693927420185106D,
        1.7731212399680372D,
        -5.4760068447290351D};
            this.extAstroPlot.CoordsCenter = new double[] {
        0D,
        0D,
        0D};
            this.extAstroPlot.CurrentColor = System.Drawing.Color.Red;
            this.extAstroPlot.Distance = 6D;
            this.extAstroPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extAstroPlot.Elevation = 0.3D;
            this.extAstroPlot.Focus = 900D;
            this.extAstroPlot.ForeColor = System.Drawing.Color.White;
            this.extAstroPlot.FramesRadius = 20D;
            this.extAstroPlot.FramesThickness = 1;
            this.extAstroPlot.FramesWidget = true;
            this.extAstroPlot.HotSpotSize = 10;
            this.extAstroPlot.LargeDotSize = 16;
            this.extAstroPlot.Location = new System.Drawing.Point(0, 26);
            this.extAstroPlot.MediumDotSize = 12;
            this.extAstroPlot.MouseSensitivity_Movement = 150;
            this.extAstroPlot.MouseSensitivity_Wheel = 300D;
            this.extAstroPlot.Name = "extAstroPlot";
            this.extAstroPlot.Size = new System.Drawing.Size(380, 380);
            this.extAstroPlot.SmallDotSize = 8;
            this.extAstroPlot.TabIndex = 30;
            this.extAstroPlot.UnVisitedColor = System.Drawing.Color.Yellow;
            this.extAstroPlot.VisitedColor = System.Drawing.Color.Aqua;
            // 
            // UserControlLocalMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.extAstroPlot);
            this.Controls.Add(this.flowLayoutPanel);
            this.Name = "UserControlLocalMap";
            this.Size = new System.Drawing.Size(380, 406);            
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).EndInit();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelExtMin;
        private ExtendedControls.NumberBoxDouble textMinRadius;
        private System.Windows.Forms.Label labelExtMax;
        private ExtendedControls.NumberBoxDouble textMaxRadius;
        private TrackBar slideMaxItems;
        private ToolTip toolTip;
        private FlowLayoutPanel flowLayoutPanel;
        private ExtendedControls.Controls.ExtAstroPlot extAstroPlot;
    }
}
