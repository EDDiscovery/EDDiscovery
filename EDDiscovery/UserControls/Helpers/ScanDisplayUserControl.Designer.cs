﻿/*
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
    partial class ScanDisplayUserControl
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
            this.panelStars = new ExtendedControls.ExtPanelScroll();
            this.rtbNodeInfo = new ExtendedControls.ExtRichTextBox();
            this.imagebox = new ExtendedControls.ExtPictureBox();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripBodies = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelStars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).BeginInit();
            this.contextMenuStripBodies.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStars
            // 
            this.panelStars.Controls.Add(this.vScrollBarCustom);
            this.panelStars.Controls.Add(this.rtbNodeInfo);
            this.panelStars.Controls.Add(this.imagebox);
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.FlowControlsLeftToRight = false;
            this.panelStars.Location = new System.Drawing.Point(0, 0);
            this.panelStars.Name = "panelStars";
            this.panelStars.Size = new System.Drawing.Size(748, 682);
            this.panelStars.TabIndex = 1;
            this.panelStars.VerticalScrollBarDockRight = true;
            this.panelStars.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelStars_MouseDown);
            // 
            // rtbNodeInfo
            // 
            this.rtbNodeInfo.BorderColor = System.Drawing.Color.Transparent;
            this.rtbNodeInfo.DetectUrls = true;
            this.rtbNodeInfo.HideScrollBar = true;
            this.rtbNodeInfo.Location = new System.Drawing.Point(472, 6);
            this.rtbNodeInfo.Name = "rtbNodeInfo";
            this.rtbNodeInfo.ReadOnly = false;
            this.rtbNodeInfo.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.rtbNodeInfo.ShowLineCount = false;
            this.rtbNodeInfo.Size = new System.Drawing.Size(200, 100);
            this.rtbNodeInfo.TabIndex = 3;
            this.rtbNodeInfo.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.rtbNodeInfo.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // imagebox
            // 
            this.imagebox.FillColor = System.Drawing.Color.Transparent;
            this.imagebox.FreezeTracking = false;
            this.imagebox.Location = new System.Drawing.Point(0, 0);
            this.imagebox.Name = "imagebox";
            this.imagebox.Size = new System.Drawing.Size(466, 554);
            this.imagebox.TabIndex = 4;
            this.imagebox.TabStop = false;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 20;
            this.vScrollBarCustom.Location = new System.Drawing.Point(729, 0);
            this.vScrollBarCustom.Maximum = -109;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(19, 682);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 2;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -109;
            this.vScrollBarCustom.ValueLimited = -109;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            // 
            // contextMenuStripBodies
            // 
            this.contextMenuStripBodies.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bookmarkToolStripMenuItem});
            this.contextMenuStripBodies.Name = "contextMenuStripBodies";
            this.contextMenuStripBodies.Size = new System.Drawing.Size(181, 48);
            // 
            // bookmarkToolStripMenuItem
            // 
            this.bookmarkToolStripMenuItem.Name = "bookmarkToolStripMenuItem";
            this.bookmarkToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.bookmarkToolStripMenuItem.Text = "Bookmark";
            // 
            // ScanDisplayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStars);
            this.Name = "ScanDisplayUserControl";
            this.Size = new System.Drawing.Size(748, 682);
            this.Resize += new System.EventHandler(this.UserControlScan_Resize);
            this.panelStars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagebox)).EndInit();
            this.contextMenuStripBodies.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtPanelScroll panelStars;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private ExtendedControls.ExtRichTextBox rtbNodeInfo;
        private ExtendedControls.ExtPictureBox imagebox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBodies;
        private System.Windows.Forms.ToolStripMenuItem bookmarkToolStripMenuItem;
    }
}
