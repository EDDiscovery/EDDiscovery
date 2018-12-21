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
            this.label_index = new ExtendedControls.LabelExt();
            this.labelControlText = new ExtendedControls.LabelExt();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.panel_ontop = new ExtendedControls.DrawnPanel();
            this.panel_transparent = new ExtendedControls.DrawnPanel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_taskbaricon = new ExtendedControls.DrawnPanel();
            this.panel_showtitle = new ExtendedControls.DrawnPanel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.statusStripBottom = new ExtendedControls.StatusStripCustom();
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
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(582, -2);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 24;
            this.panel_minimize.TabStop = false;
            this.toolTip1.SetToolTip(this.panel_minimize, "Minimise");
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // panel_ontop
            // 
            this.panel_ontop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ontop.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Floating;
            this.panel_ontop.Location = new System.Drawing.Point(558, -2);
            this.panel_ontop.Name = "panel_ontop";
            this.panel_ontop.Padding = new System.Windows.Forms.Padding(6);
            this.panel_ontop.Size = new System.Drawing.Size(24, 24);
            this.panel_ontop.TabIndex = 24;
            this.toolTip1.SetToolTip(this.panel_ontop, "Toggle window on top of others");
            this.panel_ontop.Click += new System.EventHandler(this.panel_ontop_Click);
            // 
            // panel_transparent
            // 
            this.panel_transparent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_transparent.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Transparent;
            this.panel_transparent.Location = new System.Drawing.Point(486, -2);
            this.panel_transparent.Name = "panel_transparent";
            this.panel_transparent.Padding = new System.Windows.Forms.Padding(6);
            this.panel_transparent.Size = new System.Drawing.Size(24, 24);
            this.panel_transparent.TabIndex = 24;
            this.toolTip1.SetToolTip(this.panel_transparent, resources.GetString("panel_transparent.ToolTip"));
            this.panel_transparent.Click += new System.EventHandler(this.panel_transparency_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(606, -2);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 25;
            this.panel_close.TabStop = false;
            this.toolTip1.SetToolTip(this.panel_close, "Close");
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_taskbaricon
            // 
            this.panel_taskbaricon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_taskbaricon.ImageSelected = ExtendedControls.DrawnPanel.ImageType.WindowInTaskBar;
            this.panel_taskbaricon.Location = new System.Drawing.Point(534, -2);
            this.panel_taskbaricon.Name = "panel_taskbaricon";
            this.panel_taskbaricon.Padding = new System.Windows.Forms.Padding(6);
            this.panel_taskbaricon.Size = new System.Drawing.Size(24, 24);
            this.panel_taskbaricon.TabIndex = 24;
            this.toolTip1.SetToolTip(this.panel_taskbaricon, "Toggle show taskbar icon for this window");
            this.panel_taskbaricon.Click += new System.EventHandler(this.panel_taskbaricon_Click);
            // 
            // panel_showtitle
            // 
            this.panel_showtitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_showtitle.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Captioned;
            this.panel_showtitle.Location = new System.Drawing.Point(510, -2);
            this.panel_showtitle.Name = "panel_showtitle";
            this.panel_showtitle.Padding = new System.Windows.Forms.Padding(6);
            this.panel_showtitle.Size = new System.Drawing.Size(24, 24);
            this.panel_showtitle.TabIndex = 26;
            this.toolTip1.SetToolTip(this.panel_showtitle, "Toggle title visibility for this window when transparent");
            this.panel_showtitle.Click += new System.EventHandler(this.panel_showtitle_Click);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.Controls.Add(this.panel_showtitle);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Controls.Add(this.labelControlText);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.panel_ontop);
            this.panelTop.Controls.Add(this.panel_taskbaricon);
            this.panelTop.Controls.Add(this.panel_transparent);
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Location = new System.Drawing.Point(2, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(630, 22);
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
            this.statusStripBottom.Text = "statusStripCustom1";
            // 
            // UserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 558);
            this.Controls.Add(this.statusStripBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlForm";
            this.Text = "<code>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserControlForm_FormClosing);
            this.Shown += new System.EventHandler(this.UserControlForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlForm_Layout);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.LabelExt label_index;
        private ExtendedControls.DrawnPanel panel_minimize;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.StatusStripCustom statusStripBottom;
        private ExtendedControls.LabelExt labelControlText;
        private ExtendedControls.DrawnPanel panel_ontop;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.DrawnPanel panel_transparent;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_taskbaricon;
        private ExtendedControls.DrawnPanel panel_showtitle;
    }
}