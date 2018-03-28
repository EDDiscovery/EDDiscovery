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
            this.labelExt1 = new System.Windows.Forms.Label();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.labelExt3 = new System.Windows.Forms.Label();
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.panelTop = new System.Windows.Forms.Panel();
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
            this.background = new System.Windows.Forms.PictureBox();
            this.rendererMap = new nzy3D.Plot3D.Rendering.View.Renderer3D();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).BeginInit();
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
            this.textMinRadius.Size = new System.Drawing.Size(40, 20);
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
            this.textMaxRadius.Size = new System.Drawing.Size(40, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Controls.Add(this.slideMaxItems);
            this.panelTop.Controls.Add(this.labelExt1);
            this.panelTop.Controls.Add(this.textMinRadius);
            this.panelTop.Controls.Add(this.labelExt3);
            this.panelTop.Controls.Add(this.textMaxRadius);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(380, 26);
            this.panelTop.TabIndex = 25;
            this.toolTip.SetToolTip(this.panelTop, "Rotate with 1st mouse button; Zoom with ScrollWheel; Pan with middle button");
            // 
            // slideMaxItems
            // 
            this.slideMaxItems.LargeChange = 50;
            this.slideMaxItems.Location = new System.Drawing.Point(152, 1);
            this.slideMaxItems.Maximum = 500;
            this.slideMaxItems.Minimum = 50;
            this.slideMaxItems.Name = "slideMaxItems";
            this.slideMaxItems.Size = new System.Drawing.Size(90, 45);
            this.slideMaxItems.SmallChange = 10;
            this.slideMaxItems.TabIndex = 4;
            this.slideMaxItems.TickFrequency = 50;
            this.slideMaxItems.Value = 250;
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
            // 
            // toolStripMenuZoom125
            // 
            this.toolStripMenuZoom125.Name = "toolStripMenuZoom125";
            this.toolStripMenuZoom125.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom125.Text = "1.25:1";
            // 
            // toolStripMenuZoom15
            // 
            this.toolStripMenuZoom15.Name = "toolStripMenuZoom15";
            this.toolStripMenuZoom15.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom15.Text = "1.5:1";
            // 
            // toolStripMenuZoom175
            // 
            this.toolStripMenuZoom175.Name = "toolStripMenuZoom175";
            this.toolStripMenuZoom175.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom175.Text = "1.75:1";
            // 
            // toolStripMenuZoom2
            // 
            this.toolStripMenuZoom2.Name = "toolStripMenuZoom2";
            this.toolStripMenuZoom2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom2.Text = "2:1";
            // 
            // toolStripMenuZoom25
            // 
            this.toolStripMenuZoom25.Name = "toolStripMenuZoom25";
            this.toolStripMenuZoom25.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom25.Text = "2.5:1";
            // 
            // toolStripMenuZoom3
            // 
            this.toolStripMenuZoom3.Name = "toolStripMenuZoom3";
            this.toolStripMenuZoom3.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom3.Text = "3:1";
            // 
            // toolStripMenuZoom35
            // 
            this.toolStripMenuZoom35.Name = "toolStripMenuZoom35";
            this.toolStripMenuZoom35.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom35.Text = "3.5:1";
            // 
            // toolStripMenuZoom4
            // 
            this.toolStripMenuZoom4.Name = "toolStripMenuZoom4";
            this.toolStripMenuZoom4.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom4.Text = "4:1";
            // 
            // toolStripMenuZoom5
            // 
            this.toolStripMenuZoom5.Name = "toolStripMenuZoom5";
            this.toolStripMenuZoom5.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom5.Text = "5:1";
            // 
            // toolStripMenuZoom6
            // 
            this.toolStripMenuZoom6.Name = "toolStripMenuZoom6";
            this.toolStripMenuZoom6.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom6.Text = "6:1";
            // 
            // toolStripMenuZoom8
            // 
            this.toolStripMenuZoom8.Name = "toolStripMenuZoom8";
            this.toolStripMenuZoom8.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom8.Text = "8:1";
            // 
            // toolStripMenuZoom10
            // 
            this.toolStripMenuZoom10.Name = "toolStripMenuZoom10";
            this.toolStripMenuZoom10.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuZoom10.Text = "10:1";
            // 
            // toolStripMenuZoomCenterMap
            // 
            this.toolStripMenuZoomCenterMap.Name = "toolStripMenuZoomCenterMap";
            this.toolStripMenuZoomCenterMap.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuZoomCenterMap.Text = "Center Map";
            // 
            // toolStripMenuReset
            // 
            this.toolStripMenuReset.Name = "toolStripMenuReset";
            this.toolStripMenuReset.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuReset.Text = "Reset";
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
            // 
            // background
            // 
            this.background.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Name = "background";
            this.background.Size = new System.Drawing.Size(380, 406);
            this.background.TabIndex = 30;
            this.background.TabStop = false;
            // 
            // rendererMap
            // 
            this.rendererMap.BackColor = System.Drawing.Color.Black;
            this.rendererMap.Location = new System.Drawing.Point(16, 40);
            this.rendererMap.Name = "rendererMap";
            this.rendererMap.Size = new System.Drawing.Size(350, 350);
            this.rendererMap.TabIndex = 31;
            this.rendererMap.VSync = false;
            // 
            // UserControlLocalMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.rendererMap);
            this.Controls.Add(this.background);
            this.Name = "UserControlLocalMap";
            this.Size = new System.Drawing.Size(380, 406);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slideMaxItems)).EndInit();
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
        private PictureBox background;
        private nzy3D.Plot3D.Rendering.View.Renderer3D rendererMap;
    }
}
