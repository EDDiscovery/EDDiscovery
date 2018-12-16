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
    partial class UserControlTrippanel
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
            this.showEDSMStartButtonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTravelledDistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFuelLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCurrentFSDRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAvgFSDRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showMaxFSDRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFSDRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pictureBox.Size = new System.Drawing.Size(728, 570);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.ClickElement += new ExtendedControls.PictureBoxHotspot.OnElement(this.pictureBox_ClickElement);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showEDSMStartButtonsToolStripMenuItem,
            this.showTargetToolStripMenuItem,
            this.showTravelledDistanceToolStripMenuItem,
            this.showFuelLevelToolStripMenuItem,
            this.showCurrentFSDRangeToolStripMenuItem,
            this.showAvgFSDRangeToolStripMenuItem,
            this.showMaxFSDRangeToolStripMenuItem,
            this.showFSDRangeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(211, 202);
            // 
            // showEDSMStartButtonsToolStripMenuItem
            // 
            this.showEDSMStartButtonsToolStripMenuItem.Checked = true;
            this.showEDSMStartButtonsToolStripMenuItem.CheckOnClick = true;
            this.showEDSMStartButtonsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showEDSMStartButtonsToolStripMenuItem.Name = "showEDSMStartButtonsToolStripMenuItem";
            this.showEDSMStartButtonsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showEDSMStartButtonsToolStripMenuItem.Text = "Show EDSM/Start Buttons";
            // 
            // showTargetToolStripMenuItem
            // 
            this.showTargetToolStripMenuItem.Checked = true;
            this.showTargetToolStripMenuItem.CheckOnClick = true;
            this.showTargetToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTargetToolStripMenuItem.Name = "showTargetToolStripMenuItem";
            this.showTargetToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showTargetToolStripMenuItem.Text = "Show Target";
            // 
            // showTravelledDistanceToolStripMenuItem
            // 
            this.showTravelledDistanceToolStripMenuItem.Checked = true;
            this.showTravelledDistanceToolStripMenuItem.CheckOnClick = true;
            this.showTravelledDistanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTravelledDistanceToolStripMenuItem.Name = "showTravelledDistanceToolStripMenuItem";
            this.showTravelledDistanceToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showTravelledDistanceToolStripMenuItem.Text = "Show Travelled Distance";
            // 
            // showFuelLevelToolStripMenuItem
            // 
            this.showFuelLevelToolStripMenuItem.Checked = true;
            this.showFuelLevelToolStripMenuItem.CheckOnClick = true;
            this.showFuelLevelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showFuelLevelToolStripMenuItem.Name = "showFuelLevelToolStripMenuItem";
            this.showFuelLevelToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showFuelLevelToolStripMenuItem.Text = "Show Fuel Level";
            // 
            // showCurrentFSDRangeToolStripMenuItem
            // 
            this.showCurrentFSDRangeToolStripMenuItem.Checked = true;
            this.showCurrentFSDRangeToolStripMenuItem.CheckOnClick = true;
            this.showCurrentFSDRangeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showCurrentFSDRangeToolStripMenuItem.Name = "showCurrentFSDRangeToolStripMenuItem";
            this.showCurrentFSDRangeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showCurrentFSDRangeToolStripMenuItem.Text = "Show Current FSD Range";
            // 
            // showAvgFSDRangeToolStripMenuItem
            // 
            this.showAvgFSDRangeToolStripMenuItem.Checked = true;
            this.showAvgFSDRangeToolStripMenuItem.CheckOnClick = true;
            this.showAvgFSDRangeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAvgFSDRangeToolStripMenuItem.Name = "showAvgFSDRangeToolStripMenuItem";
            this.showAvgFSDRangeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showAvgFSDRangeToolStripMenuItem.Text = "Show Avg FSD Range";
            // 
            // showMaxFSDRangeToolStripMenuItem
            // 
            this.showMaxFSDRangeToolStripMenuItem.Checked = true;
            this.showMaxFSDRangeToolStripMenuItem.CheckOnClick = true;
            this.showMaxFSDRangeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMaxFSDRangeToolStripMenuItem.Name = "showMaxFSDRangeToolStripMenuItem";
            this.showMaxFSDRangeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showMaxFSDRangeToolStripMenuItem.Text = "Show Max FSD Range";
            // 
            // showFSDRangeToolStripMenuItem
            // 
            this.showFSDRangeToolStripMenuItem.Checked = true;
            this.showFSDRangeToolStripMenuItem.CheckOnClick = true;
            this.showFSDRangeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showFSDRangeToolStripMenuItem.Name = "showFSDRangeToolStripMenuItem";
            this.showFSDRangeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showFSDRangeToolStripMenuItem.Text = "Show FSD Range";
            // 
            // UserControlTrippanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlTrippanel";
            this.Size = new System.Drawing.Size(728, 570);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.PictureBoxHotspot pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showEDSMStartButtonsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFuelLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCurrentFSDRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAvgFSDRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMaxFSDRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFSDRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTravelledDistanceToolStripMenuItem;
    }
}
