/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
namespace EDDiscovery
{
    partial class Form2DMap
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripComboExpo = new ExtendedControls.ToolStripComboBoxCustom();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxTime = new ExtendedControls.ToolStripComboBoxCustom();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomtoFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStars = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.imageViewer = new ExtendedControls.ImageViewer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.toolStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboExpo,
            this.toolStripSeparator1,
            this.toolStripComboBoxTime,
            this.toolStripSeparator2,
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomOut,
            this.toolStripButtonZoomtoFit,
            this.toolStripSeparator3,
            this.toolStripButtonStars,
            this.toolStripButtonSave});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(510, 29);
            this.toolStrip.TabIndex = 1;
            // 
            // toolStripComboExpo
            // 
            this.toolStripComboExpo.ArrowWidth = 1;
            this.toolStripComboExpo.BorderColor = System.Drawing.Color.White;
            this.toolStripComboExpo.ButtonColorScaling = 0.5F;
            this.toolStripComboExpo.DataSource = null;
            this.toolStripComboExpo.DisplayMember = "";
            this.toolStripComboExpo.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.toolStripComboExpo.DropDownHeight = 106;
            this.toolStripComboExpo.DropDownWidth = 150;
            this.toolStripComboExpo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboExpo.ItemHeight = 13;
            this.toolStripComboExpo.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.toolStripComboExpo.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.toolStripComboExpo.Name = "toolStripComboExpo";
            this.toolStripComboExpo.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.toolStripComboExpo.ScrollBarColor = System.Drawing.Color.LightGray;
            this.toolStripComboExpo.ScrollBarWidth = 16;
            this.toolStripComboExpo.SelectedIndex = -1;
            this.toolStripComboExpo.SelectedItem = null;
            this.toolStripComboExpo.Size = new System.Drawing.Size(150, 29);
            this.toolStripComboExpo.ValueMember = "";
            this.toolStripComboExpo.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxExpo_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripComboBoxTime
            // 
            this.toolStripComboBoxTime.ArrowWidth = 1;
            this.toolStripComboBoxTime.BorderColor = System.Drawing.Color.White;
            this.toolStripComboBoxTime.ButtonColorScaling = 0.5F;
            this.toolStripComboBoxTime.DataSource = null;
            this.toolStripComboBoxTime.DisplayMember = "";
            this.toolStripComboBoxTime.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.toolStripComboBoxTime.DropDownHeight = 106;
            this.toolStripComboBoxTime.DropDownWidth = 140;
            this.toolStripComboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBoxTime.ItemHeight = 13;
            this.toolStripComboBoxTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.toolStripComboBoxTime.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.toolStripComboBoxTime.Name = "toolStripComboBoxTime";
            this.toolStripComboBoxTime.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxTime.ScrollBarColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxTime.ScrollBarWidth = 16;
            this.toolStripComboBoxTime.SelectedIndex = -1;
            this.toolStripComboBoxTime.SelectedItem = null;
            this.toolStripComboBoxTime.Size = new System.Drawing.Size(200, 29);
            this.toolStripComboBoxTime.ValueMember = "";
            this.toolStripComboBoxTime.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxTime_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = global::EDDiscovery.Icons.Controls.Map2D_ZoomIn;
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(26, 26);
            this.toolStripButtonZoomIn.ToolTipText = "Zoom in";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = global::EDDiscovery.Icons.Controls.Map2D_ZoomOut;
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(26, 26);
            this.toolStripButtonZoomOut.ToolTipText = "Zoom out";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripButtonZoomtoFit
            // 
            this.toolStripButtonZoomtoFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomtoFit.Image = global::EDDiscovery.Icons.Controls.Map2D_ZoomToFit;
            this.toolStripButtonZoomtoFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomtoFit.Name = "toolStripButtonZoomtoFit";
            this.toolStripButtonZoomtoFit.Size = new System.Drawing.Size(26, 26);
            this.toolStripButtonZoomtoFit.ToolTipText = "Zoom to best fit";
            this.toolStripButtonZoomtoFit.Click += new System.EventHandler(this.toolStripButtonZoomtoFit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 29);
            // 
            // toolStripButtonStars
            // 
            this.toolStripButtonStars.CheckOnClick = true;
            this.toolStripButtonStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStars.Image = global::EDDiscovery.Icons.Controls.Map2D_ShowAllStars;
            this.toolStripButtonStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStars.Name = "toolStripButtonStars";
            this.toolStripButtonStars.Size = new System.Drawing.Size(26, 26);
            this.toolStripButtonStars.Text = "Stars";
            this.toolStripButtonStars.ToolTipText = "Show all stars";
            this.toolStripButtonStars.Click += new System.EventHandler(this.toolStripButtonStars_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::EDDiscovery.Icons.Controls.Map2D_Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(26, 26);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.toolStrip);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(982, 32);
            this.panelTop.TabIndex = 2;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseUp);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(925, 5);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 30;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(955, 5);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 31;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // imageViewer
            // 
            this.imageViewer.AutoScroll = true;
            this.imageViewer.AutoSize = false;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 32);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(982, 706);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.ZoomIncrement = 10;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "PNG Image|*.png|Bitmap Image|*.bmp|JPEG Image|*.jpg";
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.imageViewer);
            this.panelOuter.Controls.Add(this.panelTop);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(984, 740);
            this.panelOuter.TabIndex = 0;
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStripCustom.Location = new System.Drawing.Point(0, 740);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(984, 22);
            this.statusStripCustom.TabIndex = 32;
            // 
            // Form2DMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.statusStripCustom);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "Form2DMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2D Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2dClosing);
            this.Load += new System.EventHandler(this.Form2dLoad);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ToolStripComboBoxCustom toolStripComboExpo;
        private ExtendedControls.ToolStripComboBoxCustom toolStripComboBoxTime;
        private ExtendedControls.ImageViewer imageViewer;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomtoFit;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButtonStars;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}