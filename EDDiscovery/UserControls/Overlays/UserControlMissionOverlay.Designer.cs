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
    partial class UserControlMissionOverlay
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
            this.pictureBox = new ExtendedControls.ExtPictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.missionNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missionDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.factionInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rewardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.TabIndex = 1;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.missionNameToolStripMenuItem,
            this.missionDescriptionToolStripMenuItem,
            this.startDateToolStripMenuItem,
            this.endDateToolStripMenuItem,
            this.factionInformationToolStripMenuItem,
            this.rewardToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(180, 158);
            // 
            // missionNameToolStripMenuItem
            // 
            this.missionNameToolStripMenuItem.Checked = true;
            this.missionNameToolStripMenuItem.CheckOnClick = true;
            this.missionNameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.missionNameToolStripMenuItem.Name = "missionNameToolStripMenuItem";
            this.missionNameToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.missionNameToolStripMenuItem.Text = "Mission Name";
            // 
            // missionDescriptionToolStripMenuItem
            // 
            this.missionDescriptionToolStripMenuItem.Checked = true;
            this.missionDescriptionToolStripMenuItem.CheckOnClick = true;
            this.missionDescriptionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.missionDescriptionToolStripMenuItem.Name = "missionDescriptionToolStripMenuItem";
            this.missionDescriptionToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.missionDescriptionToolStripMenuItem.Text = "Mission Description";
            // 
            // startDateToolStripMenuItem
            // 
            this.startDateToolStripMenuItem.Checked = true;
            this.startDateToolStripMenuItem.CheckOnClick = true;
            this.startDateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startDateToolStripMenuItem.Name = "startDateToolStripMenuItem";
            this.startDateToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.startDateToolStripMenuItem.Text = "Start Date";
            // 
            // factionInformationToolStripMenuItem
            // 
            this.factionInformationToolStripMenuItem.Checked = true;
            this.factionInformationToolStripMenuItem.CheckOnClick = true;
            this.factionInformationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.factionInformationToolStripMenuItem.Name = "factionInformationToolStripMenuItem";
            this.factionInformationToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.factionInformationToolStripMenuItem.Text = "Faction Information";
            // 
            // rewardToolStripMenuItem
            // 
            this.rewardToolStripMenuItem.Checked = true;
            this.rewardToolStripMenuItem.CheckOnClick = true;
            this.rewardToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rewardToolStripMenuItem.Name = "rewardToolStripMenuItem";
            this.rewardToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.rewardToolStripMenuItem.Text = "Reward";
            // 
            // endDateToolStripMenuItem
            // 
            this.endDateToolStripMenuItem.Checked = true;
            this.endDateToolStripMenuItem.CheckOnClick = true;
            this.endDateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.endDateToolStripMenuItem.Name = "endDateToolStripMenuItem";
            this.endDateToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.endDateToolStripMenuItem.Text = "End Date";
            // 
            // UserControlMissionOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlMissionOverlay";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPictureBox pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem startDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem missionDescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem factionInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rewardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem missionNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endDateToolStripMenuItem;
    }
}
