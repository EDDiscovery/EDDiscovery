/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using ExtendedControls;

namespace EDDiscovery
{
    partial class TrilaterationControl
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
            this.userControlTrilateration = new EDDiscovery.UserControls.UserControlTrilateration();
            this.SuspendLayout();
            // 
            // userControlTrilateration
            // 
            this.userControlTrilateration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlTrilateration.Location = new System.Drawing.Point(0, 0);
            this.userControlTrilateration.Name = "userControlTrilateration";
            this.userControlTrilateration.Size = new System.Drawing.Size(924, 681);
            this.userControlTrilateration.TabIndex = 0;
            // 
            // TrilaterationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlTrilateration);
            this.Name = "TrilaterationControl";
            this.Size = new System.Drawing.Size(924, 681);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserControlTrilateration userControlTrilateration;
    }
}
