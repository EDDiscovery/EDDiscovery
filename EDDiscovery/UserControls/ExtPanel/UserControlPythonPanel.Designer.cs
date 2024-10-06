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
            this.SuspendLayout();
            // 
            // configurableUC
            // 
            this.configurableUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurableUC.Location = new System.Drawing.Point(0, 0);
            this.configurableUC.Name = "configurableUC";
            this.configurableUC.PanelBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configurableUC.Size = new System.Drawing.Size(496, 224);
            this.configurableUC.SwallowReturn = false;
            this.configurableUC.TabIndex = 0;
            // 
            // UserControlPythonPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.configurableUC);
            this.Name = "UserControlPythonPanel";
            this.Size = new System.Drawing.Size(496, 224);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ConfigurableUC configurableUC;
    }
}
