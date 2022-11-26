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
namespace EDDiscovery.UserControls
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonDrawnShowTitle = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDrawnMinimize = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDrawnOnTop = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDrawnTaskBarIcon = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDrawnTransparentMode = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDrawnClose = new ExtendedControls.ExtButtonDrawn();
            this.statusStripBottom = new ExtendedControls.ExtStatusStrip();
            this.panelControls = new System.Windows.Forms.Panel();
            this.extButtonDrawnHelp = new ExtendedControls.ExtButtonDrawn();
            this.label_title = new System.Windows.Forms.Label();
            this.labelControlText = new System.Windows.Forms.Label();
            this.extPanelResizerTop = new ExtendedControls.ExtPanelResizer();
            this.panelTitleControlText = new System.Windows.Forms.Panel();
            this.panelControls.SuspendLayout();
            this.panelTitleControlText.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extButtonDrawnShowTitle
            // 
            this.extButtonDrawnShowTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnShowTitle.AutoEllipsis = false;
            this.extButtonDrawnShowTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnShowTitle.Image = null;
            this.extButtonDrawnShowTitle.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Captioned;
            this.extButtonDrawnShowTitle.Location = new System.Drawing.Point(31, -2);
            this.extButtonDrawnShowTitle.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnShowTitle.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnShowTitle.MouseSelectedColorEnable = true;
            this.extButtonDrawnShowTitle.Name = "extButtonDrawnShowTitle";
            this.extButtonDrawnShowTitle.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnShowTitle.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnShowTitle.Selectable = true;
            this.extButtonDrawnShowTitle.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnShowTitle.TabIndex = 26;
            this.extButtonDrawnShowTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnShowTitle, "Toggle title visibility for this window when transparent");
            this.extButtonDrawnShowTitle.UseMnemonic = true;
            this.extButtonDrawnShowTitle.Click += new System.EventHandler(this.button_showtitle_Click);
            // 
            // extButtonDrawnMinimize
            // 
            this.extButtonDrawnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnMinimize.AutoEllipsis = false;
            this.extButtonDrawnMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnMinimize.Image = null;
            this.extButtonDrawnMinimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.extButtonDrawnMinimize.Location = new System.Drawing.Point(127, -2);
            this.extButtonDrawnMinimize.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnMinimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnMinimize.MouseSelectedColorEnable = true;
            this.extButtonDrawnMinimize.Name = "extButtonDrawnMinimize";
            this.extButtonDrawnMinimize.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnMinimize.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnMinimize.Selectable = false;
            this.extButtonDrawnMinimize.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnMinimize.TabIndex = 24;
            this.extButtonDrawnMinimize.TabStop = false;
            this.extButtonDrawnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnMinimize, "Minimise");
            this.extButtonDrawnMinimize.UseMnemonic = true;
            this.extButtonDrawnMinimize.Click += new System.EventHandler(this.button_minimize_Click);
            // 
            // extButtonDrawnOnTop
            // 
            this.extButtonDrawnOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnOnTop.AutoEllipsis = false;
            this.extButtonDrawnOnTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnOnTop.Image = null;
            this.extButtonDrawnOnTop.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Floating;
            this.extButtonDrawnOnTop.Location = new System.Drawing.Point(79, -2);
            this.extButtonDrawnOnTop.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnOnTop.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnOnTop.MouseSelectedColorEnable = true;
            this.extButtonDrawnOnTop.Name = "extButtonDrawnOnTop";
            this.extButtonDrawnOnTop.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnOnTop.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnOnTop.Selectable = true;
            this.extButtonDrawnOnTop.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnOnTop.TabIndex = 24;
            this.extButtonDrawnOnTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnOnTop, "Toggle window on top of others");
            this.extButtonDrawnOnTop.UseMnemonic = true;
            this.extButtonDrawnOnTop.Click += new System.EventHandler(this.button_ontop_Click);
            // 
            // extButtonDrawnTaskBarIcon
            // 
            this.extButtonDrawnTaskBarIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnTaskBarIcon.AutoEllipsis = false;
            this.extButtonDrawnTaskBarIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnTaskBarIcon.Image = null;
            this.extButtonDrawnTaskBarIcon.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.WindowInTaskBar;
            this.extButtonDrawnTaskBarIcon.Location = new System.Drawing.Point(55, -2);
            this.extButtonDrawnTaskBarIcon.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnTaskBarIcon.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnTaskBarIcon.MouseSelectedColorEnable = true;
            this.extButtonDrawnTaskBarIcon.Name = "extButtonDrawnTaskBarIcon";
            this.extButtonDrawnTaskBarIcon.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnTaskBarIcon.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnTaskBarIcon.Selectable = true;
            this.extButtonDrawnTaskBarIcon.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnTaskBarIcon.TabIndex = 24;
            this.extButtonDrawnTaskBarIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnTaskBarIcon, "Toggle show taskbar icon for this window");
            this.extButtonDrawnTaskBarIcon.UseMnemonic = true;
            this.extButtonDrawnTaskBarIcon.Click += new System.EventHandler(this.button_taskbaricon_Click);
            // 
            // extButtonDrawnTransparentMode
            // 
            this.extButtonDrawnTransparentMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnTransparentMode.AutoEllipsis = false;
            this.extButtonDrawnTransparentMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnTransparentMode.Image = null;
            this.extButtonDrawnTransparentMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Transparent;
            this.extButtonDrawnTransparentMode.Location = new System.Drawing.Point(7, -2);
            this.extButtonDrawnTransparentMode.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnTransparentMode.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnTransparentMode.MouseSelectedColorEnable = true;
            this.extButtonDrawnTransparentMode.Name = "extButtonDrawnTransparentMode";
            this.extButtonDrawnTransparentMode.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnTransparentMode.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnTransparentMode.Selectable = true;
            this.extButtonDrawnTransparentMode.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnTransparentMode.TabIndex = 24;
            this.extButtonDrawnTransparentMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnTransparentMode, resources.GetString("extButtonDrawnTransparentMode.ToolTip"));
            this.extButtonDrawnTransparentMode.UseMnemonic = true;
            this.extButtonDrawnTransparentMode.Click += new System.EventHandler(this.button_transparency_Click);
            // 
            // extButtonDrawnClose
            // 
            this.extButtonDrawnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnClose.AutoEllipsis = false;
            this.extButtonDrawnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnClose.Image = null;
            this.extButtonDrawnClose.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.extButtonDrawnClose.Location = new System.Drawing.Point(151, -2);
            this.extButtonDrawnClose.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnClose.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnClose.MouseSelectedColorEnable = true;
            this.extButtonDrawnClose.Name = "extButtonDrawnClose";
            this.extButtonDrawnClose.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnClose.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnClose.Selectable = false;
            this.extButtonDrawnClose.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnClose.TabIndex = 25;
            this.extButtonDrawnClose.TabStop = false;
            this.extButtonDrawnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.extButtonDrawnClose, "Close");
            this.extButtonDrawnClose.UseMnemonic = true;
            this.extButtonDrawnClose.Click += new System.EventHandler(this.button_close_Click);
            // 
            // statusStripBottom
            // 
            this.statusStripBottom.Location = new System.Drawing.Point(0, 652);
            this.statusStripBottom.Name = "statusStripBottom";
            this.statusStripBottom.Size = new System.Drawing.Size(812, 22);
            this.statusStripBottom.TabIndex = 26;
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.Controls.Add(this.extButtonDrawnShowTitle);
            this.panelControls.Controls.Add(this.extButtonDrawnMinimize);
            this.panelControls.Controls.Add(this.extButtonDrawnOnTop);
            this.panelControls.Controls.Add(this.extButtonDrawnTaskBarIcon);
            this.panelControls.Controls.Add(this.extButtonDrawnHelp);
            this.panelControls.Controls.Add(this.extButtonDrawnTransparentMode);
            this.panelControls.Controls.Add(this.extButtonDrawnClose);
            this.panelControls.Location = new System.Drawing.Point(637, 3);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(175, 22);
            this.panelControls.TabIndex = 27;
            this.panelControls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.panelControls.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // extButtonDrawnHelp
            // 
            this.extButtonDrawnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelp.AutoEllipsis = false;
            this.extButtonDrawnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelp.Image = null;
            this.extButtonDrawnHelp.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelp.Location = new System.Drawing.Point(103, -2);
            this.extButtonDrawnHelp.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelp.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelp.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelp.Name = "extButtonDrawnHelp";
            this.extButtonDrawnHelp.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelp.PanelDisabledScaling = 0.25F;
            this.extButtonDrawnHelp.Selectable = true;
            this.extButtonDrawnHelp.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelp.TabIndex = 24;
            this.extButtonDrawnHelp.Text = "?";
            this.extButtonDrawnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelp.UseMnemonic = true;
            this.extButtonDrawnHelp.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Location = new System.Drawing.Point(3, 2);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(62, 13);
            this.label_title.TabIndex = 23;
            this.label_title.Text = "<code title>";
            this.label_title.UseMnemonic = false;
            this.label_title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_title_MouseDown);
            // 
            // labelControlText
            // 
            this.labelControlText.AutoSize = true;
            this.labelControlText.Location = new System.Drawing.Point(101, 1);
            this.labelControlText.Name = "labelControlText";
            this.labelControlText.Size = new System.Drawing.Size(98, 13);
            this.labelControlText.TabIndex = 23;
            this.labelControlText.Text = "<code control text>";
            this.labelControlText.UseMnemonic = false;
            this.labelControlText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelControlText_MouseDown);
            // 
            // extPanelResizerTop
            // 
            this.extPanelResizerTop.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.extPanelResizerTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelResizerTop.Location = new System.Drawing.Point(0, 0);
            this.extPanelResizerTop.Movement = System.Windows.Forms.DockStyle.Top;
            this.extPanelResizerTop.Name = "extPanelResizerTop";
            this.extPanelResizerTop.Size = new System.Drawing.Size(812, 3);
            this.extPanelResizerTop.TabIndex = 28;
            // 
            // panelTitleControlText
            // 
            this.panelTitleControlText.Controls.Add(this.labelControlText);
            this.panelTitleControlText.Controls.Add(this.label_title);
            this.panelTitleControlText.Location = new System.Drawing.Point(0, 3);
            this.panelTitleControlText.Name = "panelTitleControlText";
            this.panelTitleControlText.Size = new System.Drawing.Size(569, 22);
            this.panelTitleControlText.TabIndex = 29;
            this.panelTitleControlText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            this.panelTitleControlText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MouseUp);
            // 
            // UserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 674);
            this.Controls.Add(this.statusStripBottom);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.panelTitleControlText);
            this.Controls.Add(this.extPanelResizerTop);
            this.Name = "UserControlForm";
            this.Shown += new System.EventHandler(this.UserControlForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlForm_Layout);
            this.panelControls.ResumeLayout(false);
            this.panelTitleControlText.ResumeLayout(false);
            this.panelTitleControlText.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnMinimize;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnClose;
        private ExtendedControls.ExtStatusStrip statusStripBottom;
        private System.Windows.Forms.Label labelControlText;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnOnTop;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnTransparentMode;
        private System.Windows.Forms.Panel panelControls;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnTaskBarIcon;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnShowTitle;
        private ExtendedControls.ExtPanelResizer extPanelResizerTop;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelp;
        private System.Windows.Forms.Panel panelTitleControlText;
    }
}