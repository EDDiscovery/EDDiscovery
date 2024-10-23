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
    partial class UserControlPythonPanel
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
            this.SuspendLayout();
            // 
            // configurableUC
            // 
            this.configurableUC.DialogResult = System.Windows.Forms.DialogResult.None;
            this.configurableUC.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.extRichTextBoxErrorLog.HideScrollBar = true;
            this.extRichTextBoxErrorLog.Location = new System.Drawing.Point(102, 64);
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
            this.extRichTextBoxErrorLog.Size = new System.Drawing.Size(200, 100);
            this.extRichTextBoxErrorLog.TabIndex = 1;
            this.extRichTextBoxErrorLog.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.extRichTextBoxErrorLog.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // UserControlPythonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extRichTextBoxErrorLog);
            this.Controls.Add(this.configurableUC);
            this.Name = "UserControlPythonPanel";
            this.Size = new System.Drawing.Size(496, 224);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ConfigurableUC configurableUC;
        private ExtendedControls.ExtRichTextBox extRichTextBoxErrorLog;
    }
}
