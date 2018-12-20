/*
 * Copyright © 2016 EDDiscovery development team
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
    partial class TimedMessage
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
            this.labelMessage = new ExtendedControls.LabelExt();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(4, 4);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(366, 47);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "<code>";
            this.labelMessage.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // TimedMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 124);
            this.Controls.Add(this.labelMessage);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "TimedMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "<code>";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.LabelExt labelMessage;
    }
}