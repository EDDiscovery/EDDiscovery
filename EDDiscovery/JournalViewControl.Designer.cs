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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
namespace EDDiscovery
{
    partial class JournalViewControl
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
            this.userControlJournalGrid = new EDDiscovery.UserControls.UserControlJournalGrid();
            this.SuspendLayout();
            // 
            // userControlJournalGrid
            // 
            this.userControlJournalGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlJournalGrid.Location = new System.Drawing.Point(0, 0);
            this.userControlJournalGrid.Name = "userControlJournalGrid";
            this.userControlJournalGrid.Size = new System.Drawing.Size(912, 582);
            this.userControlJournalGrid.TabIndex = 0;
            // 
            // JournalViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlJournalGrid);
            this.Name = "JournalViewControl";
            this.Size = new System.Drawing.Size(912, 582);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserControlJournalGrid userControlJournalGrid;
    }
}
