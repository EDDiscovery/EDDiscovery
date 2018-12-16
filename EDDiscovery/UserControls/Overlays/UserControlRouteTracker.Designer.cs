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
namespace EDDiscovery.UserControls
{
    partial class UserControlRouteTracker
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
            this.pictureBox = new ExtendedControls.PictureBoxHotspot();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showJumpsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showWaypointCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDeviationFromRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCopyWPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSetTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.contextMenuStrip;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRouteToolStripMenuItem,
            this.showJumpsToolStripMenuItem,
            this.showWaypointCoordinatesToolStripMenuItem,
            this.showDeviationFromRouteToolStripMenuItem,
            this.autoCopyWPToolStripMenuItem,
            this.autoSetTargetToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(225, 158);
            // 
            // setRouteToolStripMenuItem
            // 
            this.setRouteToolStripMenuItem.Name = "setRouteToolStripMenuItem";
            this.setRouteToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.setRouteToolStripMenuItem.Text = "Set route";
            this.setRouteToolStripMenuItem.Click += new System.EventHandler(this.setRouteToolStripMenuItem_Click);
            // 
            // showJumpsToolStripMenuItem
            // 
            this.showJumpsToolStripMenuItem.Checked = true;
            this.showJumpsToolStripMenuItem.CheckOnClick = true;
            this.showJumpsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showJumpsToolStripMenuItem.Name = "showJumpsToolStripMenuItem";
            this.showJumpsToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showJumpsToolStripMenuItem.Text = "Show Jumps To Go";
            this.showJumpsToolStripMenuItem.Click += new System.EventHandler(this.showJumpsToolStripMenuItem_Click);
            // 
            // showWaypointCoordinatesToolStripMenuItem
            // 
            this.showWaypointCoordinatesToolStripMenuItem.Checked = true;
            this.showWaypointCoordinatesToolStripMenuItem.CheckOnClick = true;
            this.showWaypointCoordinatesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showWaypointCoordinatesToolStripMenuItem.Name = "showWaypointCoordinatesToolStripMenuItem";
            this.showWaypointCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showWaypointCoordinatesToolStripMenuItem.Text = "Show Waypoint Coordinates";
            this.showWaypointCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.showWaypointCoordinatesToolStripMenuItem_Click);
            // 
            // showDeviationFromRouteToolStripMenuItem
            // 
            this.showDeviationFromRouteToolStripMenuItem.Checked = true;
            this.showDeviationFromRouteToolStripMenuItem.CheckOnClick = true;
            this.showDeviationFromRouteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDeviationFromRouteToolStripMenuItem.Name = "showDeviationFromRouteToolStripMenuItem";
            this.showDeviationFromRouteToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.showDeviationFromRouteToolStripMenuItem.Text = "Show Deviation from route";
            this.showDeviationFromRouteToolStripMenuItem.Click += new System.EventHandler(this.showDeviationFromRouteToolStripMenuItem_Click);
            // 
            // autoCopyWPToolStripMenuItem
            // 
            this.autoCopyWPToolStripMenuItem.Checked = true;
            this.autoCopyWPToolStripMenuItem.CheckOnClick = true;
            this.autoCopyWPToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoCopyWPToolStripMenuItem.Name = "autoCopyWPToolStripMenuItem";
            this.autoCopyWPToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.autoCopyWPToolStripMenuItem.Text = "Auto copy waypoint";
            this.autoCopyWPToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoCopyWPToolStripMenuItem_CheckedChanged);
            // 
            // autoSetTargetToolStripMenuItem
            // 
            this.autoSetTargetToolStripMenuItem.Checked = true;
            this.autoSetTargetToolStripMenuItem.CheckOnClick = true;
            this.autoSetTargetToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoSetTargetToolStripMenuItem.Name = "autoSetTargetToolStripMenuItem";
            this.autoSetTargetToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.autoSetTargetToolStripMenuItem.Text = "Auto set target";
            // 
            // UserControlRouteTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlRouteTracker";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.PictureBoxHotspot pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoCopyWPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSetTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showJumpsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showWaypointCoordinatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDeviationFromRouteToolStripMenuItem;
    }
}
