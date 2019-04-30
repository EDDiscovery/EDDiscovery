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
    partial class SplashForm
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
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel_eddiscovery
            // 
            this.panel_eddiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_eddiscovery.BackColor = System.Drawing.Color.Black;
            this.panel_eddiscovery.BackgroundImage = global::EDDiscovery.Properties.Resources.Logo;
            this.panel_eddiscovery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_eddiscovery.Location = new System.Drawing.Point(12, 53);
            this.panel_eddiscovery.Name = "panel_eddiscovery";
            this.panel_eddiscovery.Size = new System.Drawing.Size(293, 115);
            this.panel_eddiscovery.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(12, 171);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 33);
            this.label1.TabIndex = 20;
            this.label1.Text = "Welcome to EDD";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_version
            // 
            this.label_version.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_version.Location = new System.Drawing.Point(12, 9);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(293, 41);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "EDDiscovery";
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(317, 213);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel_eddiscovery);
            this.ForeColor = System.Drawing.Color.Orange;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery Loading";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_eddiscovery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_version;
    }
}