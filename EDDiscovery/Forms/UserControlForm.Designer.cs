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
namespace EDDiscovery.Forms
{
    partial class UserControlForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlForm));
            this.label_index = new ExtendedControls.ExtLabel();
            this.labelControlText = new ExtendedControls.ExtLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.panel_ontop = new ExtendedControls.ExtButtonDrawn();
            this.panel_transparent = new ExtendedControls.ExtButtonDrawn();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_taskbaricon = new ExtendedControls.ExtButtonDrawn();
            this.panel_showtitle = new ExtendedControls.ExtButtonDrawn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.statusStripBottom = new ExtendedControls.ExtStatusStrip();
            this.extPanelResizerTop = new ExtendedControls.ExtPanelResizer();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(2, 2);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(43, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "<code>";
            this.label_index.TextBackColor = System.Drawing.Color.Transparent;
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // labelControlText
            // 
            this.labelControlText.AutoSize = true;
            this.labelControlText.Location = new System.Drawing.Point(110, 2);
            this.labelControlText.Name = "labelControlText";
            this.labelControlText.Size = new System.Drawing.Size(43, 13);
            this.labelControlText.TabIndex = 23;
            this.labelControlText.Text = "<code>";
            this.labelControlText.TextBackColor = System.Drawing.Color.Transparent;
            this.labelControlText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelControlText_MouseDown);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(586, -2);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 24;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_minimize, "Minimise");
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // panel_ontop
            // 
            this.panel_ontop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ontop.AutoEllipsis = false;
            this.panel_ontop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_ontop.Image = null;
            this.panel_ontop.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Floating;
            this.panel_ontop.Location = new System.Drawing.Point(562, -2);
            this.panel_ontop.MouseOverColor = System.Drawing.Color.White;
            this.panel_ontop.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_ontop.MouseSelectedColorEnable = true;
            this.panel_ontop.Name = "panel_ontop";
            this.panel_ontop.Padding = new System.Windows.Forms.Padding(6);
            this.panel_ontop.PanelDisabledScaling = 0.25F;
            this.panel_ontop.Selectable = true;
            this.panel_ontop.Size = new System.Drawing.Size(24, 24);
            this.panel_ontop.TabIndex = 24;
            this.panel_ontop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_ontop, "Toggle window on top of others");
            this.panel_ontop.UseMnemonic = true;
            this.panel_ontop.Click += new System.EventHandler(this.panel_ontop_Click);
            // 
            // panel_transparent
            // 
            this.panel_transparent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_transparent.AutoEllipsis = false;
            this.panel_transparent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_transparent.Image = null;
            this.panel_transparent.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Transparent;
            this.panel_transparent.Location = new System.Drawing.Point(490, -2);
            this.panel_transparent.MouseOverColor = System.Drawing.Color.White;
            this.panel_transparent.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_transparent.MouseSelectedColorEnable = true;
            this.panel_transparent.Name = "panel_transparent";
            this.panel_transparent.Padding = new System.Windows.Forms.Padding(6);
            this.panel_transparent.PanelDisabledScaling = 0.25F;
            this.panel_transparent.Selectable = true;
            this.panel_transparent.Size = new System.Drawing.Size(24, 24);
            this.panel_transparent.TabIndex = 24;
            this.panel_transparent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_transparent, resources.GetString("panel_transparent.ToolTip"));
            this.panel_transparent.UseMnemonic = true;
            this.panel_transparent.Click += new System.EventHandler(this.panel_transparency_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(610, -2);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 25;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_close, "Close");
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_taskbaricon
            // 
            this.panel_taskbaricon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_taskbaricon.AutoEllipsis = false;
            this.panel_taskbaricon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_taskbaricon.Image = null;
            this.panel_taskbaricon.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.WindowInTaskBar;
            this.panel_taskbaricon.Location = new System.Drawing.Point(538, -2);
            this.panel_taskbaricon.MouseOverColor = System.Drawing.Color.White;
            this.panel_taskbaricon.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_taskbaricon.MouseSelectedColorEnable = true;
            this.panel_taskbaricon.Name = "panel_taskbaricon";
            this.panel_taskbaricon.Padding = new System.Windows.Forms.Padding(6);
            this.panel_taskbaricon.PanelDisabledScaling = 0.25F;
            this.panel_taskbaricon.Selectable = true;
            this.panel_taskbaricon.Size = new System.Drawing.Size(24, 24);
            this.panel_taskbaricon.TabIndex = 24;
            this.panel_taskbaricon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_taskbaricon, "Toggle show taskbar icon for this window");
            this.panel_taskbaricon.UseMnemonic = true;
            this.panel_taskbaricon.Click += new System.EventHandler(this.panel_taskbaricon_Click);
            // 
            // panel_showtitle
            // 
            this.panel_showtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_showtitle.AutoEllipsis = false;
            this.panel_showtitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_showtitle.Image = null;
            this.panel_showtitle.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Captioned;
            this.panel_showtitle.Location = new System.Drawing.Point(514, -2);
            this.panel_showtitle.MouseOverColor = System.Drawing.Color.White;
            this.panel_showtitle.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_showtitle.MouseSelectedColorEnable = true;
            this.panel_showtitle.Name = "panel_showtitle";
            this.panel_showtitle.Padding = new System.Windows.Forms.Padding(6);
            this.panel_showtitle.PanelDisabledScaling = 0.25F;
            this.panel_showtitle.Selectable = true;
            this.panel_showtitle.Size = new System.Drawing.Size(24, 24);
            this.panel_showtitle.TabIndex = 26;
            this.panel_showtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.panel_showtitle, "Toggle title visibility for this window when transparent");
            this.panel_showtitle.UseMnemonic = true;
            this.panel_showtitle.Click += new System.EventHandler(this.panel_showtitle_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_showtitle);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Controls.Add(this.labelControlText);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.panel_ontop);
            this.panelTop.Controls.Add(this.panel_taskbaricon);
            this.panelTop.Controls.Add(this.panel_transparent);
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(634, 22);
            this.panelTop.TabIndex = 27;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseUp);
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.Location = new System.Drawing.Point(0, 536);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Size = new System.Drawing.Size(634, 22);
            this.statusStripBottom.TabIndex = 26;
            // 
            // extPanelResizerTop
            // 
            this.extPanelResizerTop.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.extPanelResizerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelResizerTop.Location = new System.Drawing.Point(0, 0);
            this.extPanelResizerTop.Movement = System.Windows.Forms.DockStyle.Top;
            this.extPanelResizerTop.Name = "extPanelResizerTop";
            this.extPanelResizerTop.Size = new System.Drawing.Size(634, 3);
            this.extPanelResizerTop.TabIndex = 28;
            // 
            // UserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 558);
            this.Controls.Add(this.statusStripBottom);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.extPanelResizerTop);
            this.Name = "UserControlForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserControlForm_FormClosing);
            this.Shown += new System.EventHandler(this.UserControlForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlForm_Layout);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtLabel label_index;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtStatusStrip statusStripBottom;
        private ExtendedControls.ExtLabel labelControlText;
        private ExtendedControls.ExtButtonDrawn panel_ontop;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ExtButtonDrawn panel_transparent;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn panel_taskbaricon;
        private ExtendedControls.ExtButtonDrawn panel_showtitle;
        private ExtendedControls.ExtPanelResizer extPanelResizerTop;
    }
}