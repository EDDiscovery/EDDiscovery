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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom125 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom175 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom25 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom35 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoom10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuZoomCenterMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.extAstroPlot = new ExtendedControls.Controls.ExtAstroPlot();
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.textMinRadius.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
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
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
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
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuZoom,
            this.toolStripMenuZoomCenterMap,
            this.toolStripMenuReset,
            this.toolStripSeparator,
            this.aboutToolStripAbout});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(137, 98);
            // 
            // toolStripMenuZoom
            // 
            this.toolStripMenuZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuZoom1,
            this.toolStripMenuZoom125,
            this.toolStripMenuZoom15,
            this.toolStripMenuZoom175,
            this.toolStripMenuZoom2,
            this.toolStripMenuZoom25,
            this.toolStripMenuZoom3,
            this.toolStripMenuZoom35,
            this.toolStripMenuZoom4,
            this.toolStripMenuZoom5,
            this.toolStripMenuZoom6,
            this.toolStripMenuZoom8,
            this.toolStripMenuZoom10});
            this.toolStripMenuZoom.Name = "toolStripMenuZoom";
            this.toolStripMenuZoom.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuZoom.Text = "Zoom";
            // 
            // toolStripMenuZoom1
            // 
            this.toolStripMenuZoom1.Name = "toolStripMenuZoom1";
            this.toolStripMenuZoom1.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom1.Text = "1:1";
            this.toolStripMenuZoom1.Click += new System.EventHandler(this.toolStripMenuZoom1_Click);
            // 
            // toolStripMenuZoom125
            // 
            this.toolStripMenuZoom125.Name = "toolStripMenuZoom125";
            this.toolStripMenuZoom125.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom125.Text = "1.25:1";
            this.toolStripMenuZoom125.Click += new System.EventHandler(this.toolStripMenuZoom125_Click);
            // 
            // toolStripMenuZoom15
            // 
            this.toolStripMenuZoom15.Name = "toolStripMenuZoom15";
            this.toolStripMenuZoom15.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom15.Text = "1.5:1";
            this.toolStripMenuZoom15.Click += new System.EventHandler(this.toolStripMenuZoom15_Click);
            // 
            // toolStripMenuZoom175
            // 
            this.toolStripMenuZoom175.Name = "toolStripMenuZoom175";
            this.toolStripMenuZoom175.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom175.Text = "1.75:1";
            this.toolStripMenuZoom175.Click += new System.EventHandler(this.toolStripMenuZoom175_Click);
            // 
            // toolStripMenuZoom2
            // 
            this.toolStripMenuZoom2.Name = "toolStripMenuZoom2";
            this.toolStripMenuZoom2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom2.Text = "2:1";
            this.toolStripMenuZoom2.Click += new System.EventHandler(this.toolStripMenuZoom2_Click);
            // 
            // toolStripMenuZoom25
            // 
            this.toolStripMenuZoom25.Name = "toolStripMenuZoom25";
            this.toolStripMenuZoom25.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom25.Text = "2.5:1";
            this.toolStripMenuZoom25.Click += new System.EventHandler(this.toolStripMenuZoom25_Click);
            // 
            // toolStripMenuZoom3
            // 
            this.toolStripMenuZoom3.Name = "toolStripMenuZoom3";
            this.toolStripMenuZoom3.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom3.Text = "3:1";
            this.toolStripMenuZoom3.Click += new System.EventHandler(this.toolStripMenuZoom3_Click);
            // 
            // toolStripMenuZoom35
            // 
            this.toolStripMenuZoom35.Name = "toolStripMenuZoom35";
            this.toolStripMenuZoom35.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom35.Text = "3.5:1";
            this.toolStripMenuZoom35.Click += new System.EventHandler(this.toolStripMenuZoom35_Click);
            // 
            // toolStripMenuZoom4
            // 
            this.toolStripMenuZoom4.Name = "toolStripMenuZoom4";
            this.toolStripMenuZoom4.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom4.Text = "4:1";
            this.toolStripMenuZoom4.Click += new System.EventHandler(this.toolStripMenuZoom4_Click);
            // 
            // toolStripMenuZoom5
            // 
            this.toolStripMenuZoom5.Name = "toolStripMenuZoom5";
            this.toolStripMenuZoom5.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom5.Text = "5:1";
            this.toolStripMenuZoom5.Click += new System.EventHandler(this.toolStripMenuZoom5_Click);
            // 
            // toolStripMenuZoom6
            // 
            this.toolStripMenuZoom6.Name = "toolStripMenuZoom6";
            this.toolStripMenuZoom6.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom6.Text = "6:1";
            this.toolStripMenuZoom6.Click += new System.EventHandler(this.toolStripMenuZoom6_Click);
            // 
            // toolStripMenuZoom8
            // 
            this.toolStripMenuZoom8.Name = "toolStripMenuZoom8";
            this.toolStripMenuZoom8.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom8.Text = "8:1";
            this.toolStripMenuZoom8.Click += new System.EventHandler(this.toolStripMenuZoom8_Click);
            // 
            // toolStripMenuZoom10
            // 
            this.toolStripMenuZoom10.Name = "toolStripMenuZoom10";
            this.toolStripMenuZoom10.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom10.Text = "10:1";
            this.toolStripMenuZoom10.Click += new System.EventHandler(this.toolStripMenuZoom10_Click);
            // 
            // toolStripMenuZoomCenterMap
            // 
            this.toolStripMenuZoomCenterMap.Name = "toolStripMenuZoomCenterMap";
            this.toolStripMenuZoomCenterMap.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuZoomCenterMap.Text = "Center Map";
            this.toolStripMenuZoomCenterMap.Click += new System.EventHandler(this.toolStripMenuZoomCenterMap_Click);
            // 
            // toolStripMenuReset
            // 
            this.toolStripMenuReset.Name = "toolStripMenuReset";
            this.toolStripMenuReset.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuReset.Text = "Reset";
            this.toolStripMenuReset.Click += new System.EventHandler(this.toolStripMenuReset_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(133, 6);
            // 
            // aboutToolStripAbout
            // 
            this.aboutToolStripAbout.Name = "aboutToolStripAbout";
            this.aboutToolStripAbout.Size = new System.Drawing.Size(136, 22);
            this.aboutToolStripAbout.Text = "About";
            this.aboutToolStripAbout.Click += new System.EventHandler(this.aboutToolStripAbout_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.labelExtMin);
            this.flowLayoutPanel1.Controls.Add(this.textMinRadius);
            this.flowLayoutPanel1.Controls.Add(this.labelExtMax);
            this.flowLayoutPanel1.Controls.Add(this.textMaxRadius);
            this.flowLayoutPanel1.Controls.Add(this.slideMaxItems);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(380, 26);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // extAstroPlot
            // 
            this.extAstroPlot.AxesLength = 10;
            this.extAstroPlot.AxesThickness = 3;
            this.extAstroPlot.AxesWidget = true;
            this.extAstroPlot.Azimuth = 0.3D;
            this.extAstroPlot.BackColor = System.Drawing.Color.Black;
            this.extAstroPlot.Camera = new double[] {
        0D,
        0D,
        0D};
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
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UserControlLocalMap";
            this.Size = new System.Drawing.Size(380, 406);
            this.Load += new System.EventHandler(this.UserControlMap_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UserControlMap_MouseClick);
            this.Resize += new System.EventHandler(this.UserControlMap_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
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
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuZoom;
        private ToolStripMenuItem toolStripMenuZoom1;
        private ToolStripMenuItem toolStripMenuZoom125;
        private ToolStripMenuItem toolStripMenuZoom15;
        private ToolStripMenuItem toolStripMenuZoom175;
        private ToolStripMenuItem toolStripMenuZoom2;
        private ToolStripMenuItem toolStripMenuZoom25;
        private ToolStripMenuItem toolStripMenuZoom3;
        private ToolStripMenuItem toolStripMenuZoom35;
        private ToolStripMenuItem toolStripMenuZoom4;
        private ToolStripMenuItem toolStripMenuZoom5;
        private ToolStripMenuItem toolStripMenuZoom6;
        private ToolStripMenuItem toolStripMenuZoom8;
        private ToolStripMenuItem toolStripMenuZoom10;
        private ToolStripMenuItem toolStripMenuZoomCenterMap;
        private ToolStripMenuItem toolStripMenuReset;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem aboutToolStripAbout;
        private FlowLayoutPanel flowLayoutPanel1;
        private ExtendedControls.Controls.ExtAstroPlot extAstroPlot;
    }
}
