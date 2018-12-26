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
    partial class UserControlSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSearch));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabStrip = new ExtendedControls.TabStrip();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // tabStrip
            // 
            this.tabStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStrip.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tabStrip.DropDownBorderColor = System.Drawing.Color.Green;
            this.tabStrip.DropDownHeight = 500;
            this.tabStrip.DropDownItemSeperatorColor = System.Drawing.Color.Purple;
            this.tabStrip.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tabStrip.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tabStrip.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tabStrip.DropDownWidth = 400;
            this.tabStrip.EmptyColor = System.Drawing.Color.Empty;
            this.tabStrip.EmptyColorScaling = 0.5F;
            this.tabStrip.EmptyPanelIcon = ((System.Drawing.Image)(resources.GetObject("tabStrip.EmptyPanelIcon")));
            this.tabStrip.Location = new System.Drawing.Point(0, 0);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.SelectedIndex = -1;
            this.tabStrip.ShowPopOut = false;
            this.tabStrip.Size = new System.Drawing.Size(804, 716);
            this.tabStrip.StripMode = ExtendedControls.TabStrip.StripModeType.StripTopOpen;
            this.tabStrip.TabFieldSpacing = 8;
            this.tabStrip.TabIndex = 0;
            // 
            // UserControlSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabStrip);
            this.Name = "UserControlSearch";
            this.Size = new System.Drawing.Size(804, 716);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.TabStrip tabStrip;
    }
}
