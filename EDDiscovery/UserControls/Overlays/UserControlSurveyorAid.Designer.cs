﻿/*
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
    partial class UserControlSurveyorAid
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxSurveyorAid = new ExtendedControls.PictureBoxHotspot();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.planetaryClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammoniaWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.earthlikeWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bodyFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terraformableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasVolcanismToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasRingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hideAlreadyMappedBodiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyorAid)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSurveyorAid
            // 
            this.pictureBoxSurveyorAid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSurveyorAid.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSurveyorAid.Name = "pictureBoxSurveyorAid";
            this.pictureBoxSurveyorAid.Size = new System.Drawing.Size(478, 229);
            this.pictureBoxSurveyorAid.TabIndex = 0;
            this.pictureBoxSurveyorAid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSurveyorAid_MouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planetaryClassesToolStripMenuItem,
            this.bodyFeaturesToolStripMenuItem,
            this.toolStripSeparator1,
            this.hideAlreadyMappedBodiesToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(226, 98);
            // 
            // planetaryClassesToolStripMenuItem
            // 
            this.planetaryClassesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ammoniaWorldToolStripMenuItem,
            this.earthlikeWorldToolStripMenuItem,
            this.waterWorldToolStripMenuItem});
            this.planetaryClassesToolStripMenuItem.Name = "planetaryClassesToolStripMenuItem";
            this.planetaryClassesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.planetaryClassesToolStripMenuItem.Text = "Planetary Classes";
            // 
            // ammoniaWorldToolStripMenuItem
            // 
            this.ammoniaWorldToolStripMenuItem.Checked = true;
            this.ammoniaWorldToolStripMenuItem.CheckOnClick = true;
            this.ammoniaWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ammoniaWorldToolStripMenuItem.Name = "ammoniaWorldToolStripMenuItem";
            this.ammoniaWorldToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ammoniaWorldToolStripMenuItem.Text = "Ammonia World";
            this.ammoniaWorldToolStripMenuItem.Click += new System.EventHandler(this.ammoniaWorldToolStripMenuItem_Click);
            // 
            // earthlikeWorldToolStripMenuItem
            // 
            this.earthlikeWorldToolStripMenuItem.Checked = true;
            this.earthlikeWorldToolStripMenuItem.CheckOnClick = true;
            this.earthlikeWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.earthlikeWorldToolStripMenuItem.Name = "earthlikeWorldToolStripMenuItem";
            this.earthlikeWorldToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.earthlikeWorldToolStripMenuItem.Text = "Earthlike World";
            this.earthlikeWorldToolStripMenuItem.Click += new System.EventHandler(this.earthlikeWorldToolStripMenuItem_Click);
            // 
            // waterWorldToolStripMenuItem
            // 
            this.waterWorldToolStripMenuItem.Checked = true;
            this.waterWorldToolStripMenuItem.CheckOnClick = true;
            this.waterWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.waterWorldToolStripMenuItem.Name = "waterWorldToolStripMenuItem";
            this.waterWorldToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.waterWorldToolStripMenuItem.Text = "Water World";
            this.waterWorldToolStripMenuItem.Click += new System.EventHandler(this.waterWorldToolStripMenuItem_Click);
            // 
            // bodyFeaturesToolStripMenuItem
            // 
            this.bodyFeaturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terraformableToolStripMenuItem,
            this.hasVolcanismToolStripMenuItem,
            this.hasRingsToolStripMenuItem});
            this.bodyFeaturesToolStripMenuItem.Name = "bodyFeaturesToolStripMenuItem";
            this.bodyFeaturesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.bodyFeaturesToolStripMenuItem.Text = "Body Features";
            // 
            // terraformableToolStripMenuItem
            // 
            this.terraformableToolStripMenuItem.Checked = true;
            this.terraformableToolStripMenuItem.CheckOnClick = true;
            this.terraformableToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.terraformableToolStripMenuItem.Name = "terraformableToolStripMenuItem";
            this.terraformableToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.terraformableToolStripMenuItem.Text = "Terraformable";
            this.terraformableToolStripMenuItem.Click += new System.EventHandler(this.terraformableToolStripMenuItem_Click);
            // 
            // hasVolcanismToolStripMenuItem
            // 
            this.hasVolcanismToolStripMenuItem.Checked = true;
            this.hasVolcanismToolStripMenuItem.CheckOnClick = true;
            this.hasVolcanismToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasVolcanismToolStripMenuItem.Name = "hasVolcanismToolStripMenuItem";
            this.hasVolcanismToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasVolcanismToolStripMenuItem.Text = "Has Volcanism";
            this.hasVolcanismToolStripMenuItem.Click += new System.EventHandler(this.hasVolcanismToolStripMenuItem_Click);
            // 
            // hasRingsToolStripMenuItem
            // 
            this.hasRingsToolStripMenuItem.Checked = true;
            this.hasRingsToolStripMenuItem.CheckOnClick = true;
            this.hasRingsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasRingsToolStripMenuItem.Name = "hasRingsToolStripMenuItem";
            this.hasRingsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasRingsToolStripMenuItem.Text = "Has Rings";
            this.hasRingsToolStripMenuItem.Click += new System.EventHandler(this.hasRingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // hideAlreadyMappedBodiesToolStripMenuItem
            // 
            this.hideAlreadyMappedBodiesToolStripMenuItem.Checked = true;
            this.hideAlreadyMappedBodiesToolStripMenuItem.CheckOnClick = true;
            this.hideAlreadyMappedBodiesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideAlreadyMappedBodiesToolStripMenuItem.Name = "hideAlreadyMappedBodiesToolStripMenuItem";
            this.hideAlreadyMappedBodiesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.hideAlreadyMappedBodiesToolStripMenuItem.Text = "Hide already mapped bodies";
            this.hideAlreadyMappedBodiesToolStripMenuItem.Click += new System.EventHandler(this.hideAlreadyMappedBodiesToolStripMenuItem_Click);
            // 
            // UserControlSurveyorAid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxSurveyorAid);
            this.Name = "UserControlSurveyorAid";
            this.Size = new System.Drawing.Size(478, 229);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyorAid)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.PictureBoxHotspot pictureBoxSurveyorAid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem planetaryClassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ammoniaWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem earthlikeWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waterWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bodyFeaturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terraformableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hasVolcanismToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hasRingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem hideAlreadyMappedBodiesToolStripMenuItem;
    }
}
