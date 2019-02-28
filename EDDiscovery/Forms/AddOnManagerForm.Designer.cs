/*
 * Copyright © 2017 EDDiscovery development team
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
    partial class AddOnManagerForm
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
            if (disposing)
            {
                components?.Dispose();
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelVScroll = new ExtendedControls.ExtPanelScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.buttonMore = new ExtendedControls.ExtButton();
            this.richTextBoxScrollDescription = new ExtendedControls.ExtRichTextBox();
            this.panelOK = new System.Windows.Forms.Panel();
            this.buttonExtGlobals = new ExtendedControls.ExtButton();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.panelTop.SuspendLayout();
            this.panelOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1228, 26);
            this.panelTop.TabIndex = 30;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(1205, 0);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(1175, 0);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 3);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(43, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "<code>";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.splitContainer1);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 26);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Padding = new System.Windows.Forms.Padding(3);
            this.panelOuter.Size = new System.Drawing.Size(1228, 581);
            this.panelOuter.TabIndex = 32;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelVScroll);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxScrollDescription);
            this.splitContainer1.Size = new System.Drawing.Size(1220, 573);
            this.splitContainer1.SplitterDistance = 358;
            this.splitContainer1.TabIndex = 1;
            // 
            // panelVScroll
            // 
            this.panelVScroll.Controls.Add(this.vScrollBarCustom1);
            this.panelVScroll.Controls.Add(this.buttonMore);
            this.panelVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScroll.Location = new System.Drawing.Point(0, 0);
            this.panelVScroll.Name = "panelVScroll";
            this.panelVScroll.ScrollBarWidth = 20;
            this.panelVScroll.Size = new System.Drawing.Size(1220, 358);
            this.panelVScroll.TabIndex = 2;
            this.panelVScroll.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 10;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(1200, 0);
            this.vScrollBarCustom1.Maximum = -309;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 358);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -309;
            this.vScrollBarCustom1.ValueLimited = -309;
            // 
            // buttonMore
            // 
            this.buttonMore.Location = new System.Drawing.Point(8, 15);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(24, 24);
            this.buttonMore.TabIndex = 5;
            this.buttonMore.Text = "+";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // richTextBoxScrollDescription
            // 
            this.richTextBoxScrollDescription.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxScrollDescription.BorderColorScaling = 0.5F;
            this.richTextBoxScrollDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxScrollDescription.HideScrollBar = true;
            this.richTextBoxScrollDescription.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxScrollDescription.Name = "richTextBoxScrollDescription";
            this.richTextBoxScrollDescription.ReadOnly = false;
            this.richTextBoxScrollDescription.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.richTextBoxScrollDescription.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBoxScrollDescription.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBoxScrollDescription.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxScrollDescription.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBoxScrollDescription.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBoxScrollDescription.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxScrollDescription.ScrollBarLineTweak = 0;
            this.richTextBoxScrollDescription.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBoxScrollDescription.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBoxScrollDescription.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBoxScrollDescription.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBoxScrollDescription.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBoxScrollDescription.ScrollBarWidth = 20;
            this.richTextBoxScrollDescription.ShowLineCount = false;
            this.richTextBoxScrollDescription.Size = new System.Drawing.Size(1220, 211);
            this.richTextBoxScrollDescription.TabIndex = 1;
            this.richTextBoxScrollDescription.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxScrollDescription.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelOK
            // 
            this.panelOK.Controls.Add(this.buttonExtGlobals);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(0, 607);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(1228, 30);
            this.panelOK.TabIndex = 10;
            // 
            // buttonExtGlobals
            // 
            this.buttonExtGlobals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtGlobals.Location = new System.Drawing.Point(3, 3);
            this.buttonExtGlobals.Name = "buttonExtGlobals";
            this.buttonExtGlobals.Size = new System.Drawing.Size(100, 23);
            this.buttonExtGlobals.TabIndex = 7;
            this.buttonExtGlobals.Text = "Globals";
            this.buttonExtGlobals.UseVisualStyleBackColor = true;
            this.buttonExtGlobals.Click += new System.EventHandler(this.buttonExtGlobals_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(1116, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 637);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(1228, 22);
            this.statusStripCustom.TabIndex = 31;
            // 
            // AddOnManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 659);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "AddOnManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<code>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadManagerForm_FormClosing);
            this.Shown += new System.EventHandler(this.DownloadManager_Shown);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelVScroll.ResumeLayout(false);
            this.panelOK.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ExtButton buttonOK;
        private ExtendedControls.ExtRichTextBox richTextBoxScrollDescription;
        private ExtendedControls.ExtPanelScroll panelVScroll;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ExtendedControls.ExtButton buttonExtGlobals;
        private ExtendedControls.ExtButton buttonMore;
    }
}