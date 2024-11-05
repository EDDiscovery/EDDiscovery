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
    partial class UserControlZMQPanel
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
            this.configurableUC = new ExtendedControls.ConfigurableUC();
            this.extRichTextBoxErrorLog = new ExtendedControls.ExtRichTextBox();
            this.panelLog = new System.Windows.Forms.Panel();
            this.panelLogToolbar = new System.Windows.Forms.Panel();
            this.extButtonViewDialog = new ExtendedControls.ExtButton();
            this.panelLog.SuspendLayout();
            this.panelLogToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurableUC
            // 
            this.configurableUC.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.configurableUC.DialogResult = System.Windows.Forms.DialogResult.None;
            this.configurableUC.Location = new System.Drawing.Point(0, 0);
            this.configurableUC.Name = "configurableUC";
            this.configurableUC.PanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configurableUC.Size = new System.Drawing.Size(496, 224);
            this.configurableUC.SwallowReturn = true;
            this.configurableUC.TabIndex = 0;
            // 
            // extRichTextBoxErrorLog
            // 
            this.extRichTextBoxErrorLog.BorderColor = System.Drawing.Color.Transparent;
            this.extRichTextBoxErrorLog.BorderColorScaling = 0.5F;
            this.extRichTextBoxErrorLog.DetectUrls = true;
            this.extRichTextBoxErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extRichTextBoxErrorLog.HideScrollBar = true;
            this.extRichTextBoxErrorLog.Location = new System.Drawing.Point(0, 32);
            this.extRichTextBoxErrorLog.Name = "extRichTextBoxErrorLog";
            this.extRichTextBoxErrorLog.ReadOnly = false;
            this.extRichTextBoxErrorLog.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.extRichTextBoxErrorLog.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extRichTextBoxErrorLog.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.extRichTextBoxErrorLog.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.extRichTextBoxErrorLog.ScrollBarBorderColor = System.Drawing.Color.White;
            this.extRichTextBoxErrorLog.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extRichTextBoxErrorLog.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.extRichTextBoxErrorLog.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.extRichTextBoxErrorLog.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.extRichTextBoxErrorLog.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.extRichTextBoxErrorLog.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.extRichTextBoxErrorLog.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extRichTextBoxErrorLog.ShowLineCount = false;
            this.extRichTextBoxErrorLog.Size = new System.Drawing.Size(349, 188);
            this.extRichTextBoxErrorLog.TabIndex = 1;
            this.extRichTextBoxErrorLog.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.extRichTextBoxErrorLog.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelLog
            // 
            this.panelLog.Controls.Add(this.extRichTextBoxErrorLog);
            this.panelLog.Controls.Add(this.panelLogToolbar);
            this.panelLog.Location = new System.Drawing.Point(29, 230);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(349, 220);
            this.panelLog.TabIndex = 2;
            // 
            // panelLogToolbar
            // 
            this.panelLogToolbar.Controls.Add(this.extButtonViewDialog);
            this.panelLogToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelLogToolbar.Name = "panelLogToolbar";
            this.panelLogToolbar.Size = new System.Drawing.Size(349, 32);
            this.panelLogToolbar.TabIndex = 2;
            // 
            // extButtonViewDialog
            // 
            this.extButtonViewDialog.Location = new System.Drawing.Point(4, 4);
            this.extButtonViewDialog.Name = "extButtonViewDialog";
            this.extButtonViewDialog.Size = new System.Drawing.Size(100, 23);
            this.extButtonViewDialog.TabIndex = 0;
            this.extButtonViewDialog.Text = "View Dialog";
            this.extButtonViewDialog.UseVisualStyleBackColor = true;
            this.extButtonViewDialog.Click += new System.EventHandler(this.extButtonViewDialog_Click);
            // 
            // UserControlPythonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.configurableUC);
            this.Name = "UserControlPythonPanel";
            this.Size = new System.Drawing.Size(1101, 564);
            this.panelLog.ResumeLayout(false);
            this.panelLogToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ConfigurableUC configurableUC;
        private ExtendedControls.ExtRichTextBox extRichTextBoxErrorLog;
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.Panel panelLogToolbar;
        private ExtendedControls.ExtButton extButtonViewDialog;
    }
}
