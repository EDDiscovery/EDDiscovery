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
    partial class UserControlLog
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
            this.richTextBox_History = new ExtendedControls.RichTextBoxScroll();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_History
            // 
            this.richTextBox_History.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBox_History.BorderColorScaling = 0.5F;
            this.richTextBox_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_History.HideScrollBar = true;
            this.richTextBox_History.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_History.Name = "richTextBox_History";
            this.richTextBox_History.ReadOnly = false;
            this.richTextBox_History.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBox_History.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBox_History.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBox_History.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBox_History.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBox_History.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBox_History.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBox_History.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBox_History.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBox_History.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBox_History.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBox_History.ScrollBarWidth = 20;
            this.richTextBox_History.ShowLineCount = false;
            this.richTextBox_History.Size = new System.Drawing.Size(496, 224);
            this.richTextBox_History.TabIndex = 1;
            this.richTextBox_History.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBox_History.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy,
            this.clearLogToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearLogToolStripMenuItem.Text = "Clear Log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemCopy.Text = "Copy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // UserControlLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox_History);
            this.Name = "UserControlLog";
            this.Size = new System.Drawing.Size(496, 224);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal ExtendedControls.RichTextBoxScroll richTextBox_History;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
    }
}
