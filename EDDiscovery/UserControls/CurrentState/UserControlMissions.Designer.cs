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
    partial class UserControlMissions
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
            this.missionListCurrent = new EDDiscovery.UserControls.Helpers.MissionListUserControl();
            this.panelPrev = new System.Windows.Forms.Panel();
            this.panelCurrent = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerMissions = new System.Windows.Forms.SplitContainer();
            this.missionListPrevious = new EDDiscovery.UserControls.Helpers.MissionListUserControl();
            this.panelPrev.SuspendLayout();
            this.panelCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMissions)).BeginInit();
            this.splitContainerMissions.Panel1.SuspendLayout();
            this.splitContainerMissions.Panel2.SuspendLayout();
            this.splitContainerMissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // missionListCurrent
            // 
            this.missionListCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.missionListCurrent.Location = new System.Drawing.Point(0, 0);
            this.missionListCurrent.Name = "missionListCurrent";
            this.missionListCurrent.Size = new System.Drawing.Size(800, 266);
            this.missionListCurrent.TabIndex = 1;
            // 
            // panelPrev
            // 
            this.panelPrev.Controls.Add(this.missionListPrevious);
            this.panelPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrev.Location = new System.Drawing.Point(0, 0);
            this.panelPrev.Name = "panelPrev";
            this.panelPrev.Size = new System.Drawing.Size(800, 302);
            this.panelPrev.TabIndex = 4;
            // 
            // panelCurrent
            // 
            this.panelCurrent.Controls.Add(this.missionListCurrent);
            this.panelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurrent.Location = new System.Drawing.Point(0, 0);
            this.panelCurrent.Name = "panelCurrent";
            this.panelCurrent.Size = new System.Drawing.Size(800, 266);
            this.panelCurrent.TabIndex = 3;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // splitContainerMissions
            // 
            this.splitContainerMissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMissions.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMissions.Name = "splitContainerMissions";
            this.splitContainerMissions.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMissions.Panel1
            // 
            this.splitContainerMissions.Panel1.Controls.Add(this.panelCurrent);
            // 
            // splitContainerMissions.Panel2
            // 
            this.splitContainerMissions.Panel2.Controls.Add(this.panelPrev);
            this.splitContainerMissions.Size = new System.Drawing.Size(800, 572);
            this.splitContainerMissions.SplitterDistance = 266;
            this.splitContainerMissions.TabIndex = 3;
            // 
            // missionListPrevious
            // 
            this.missionListPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.missionListPrevious.Location = new System.Drawing.Point(0, 0);
            this.missionListPrevious.Name = "missionListPrevious";
            this.missionListPrevious.Size = new System.Drawing.Size(800, 302);
            this.missionListPrevious.TabIndex = 2;
            // 
            // UserControlMissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMissions);
            this.Name = "UserControlMissions";
            this.Size = new System.Drawing.Size(800, 572);
            this.panelPrev.ResumeLayout(false);
            this.panelCurrent.ResumeLayout(false);
            this.splitContainerMissions.Panel1.ResumeLayout(false);
            this.splitContainerMissions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMissions)).EndInit();
            this.splitContainerMissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelPrev;
        private System.Windows.Forms.Panel panelCurrent;
        private System.Windows.Forms.SplitContainer splitContainerMissions;
        private Helpers.MissionListUserControl missionListCurrent;
        private Helpers.MissionListUserControl missionListPrevious;
    }
}
