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
    partial class UserControlSurveyor
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
            this.pictureBoxSurveyor = new ExtendedControls.ExtPictureBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.planetaryClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammoniaWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.earthlikeWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bodyFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terraformableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasVolcanismToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasRingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowRadiusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasSignalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPlanetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllStarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBeltClustersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showMoreInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAlreadyMappedBodiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkEDSMForInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dontHideInFSSModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textAlignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyor)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSurveyor
            // 
            this.pictureBoxSurveyor.ContextMenuStrip = this.contextMenuStrip;
            this.pictureBoxSurveyor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSurveyor.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSurveyor.Name = "pictureBoxSurveyor";
            this.pictureBoxSurveyor.Size = new System.Drawing.Size(478, 229);
            this.pictureBoxSurveyor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxSurveyor.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planetaryClassesToolStripMenuItem,
            this.bodyFeaturesToolStripMenuItem,
            this.showAllPlanetsToolStripMenuItem,
            this.showAllStarsToolStripMenuItem,
            this.showBeltClustersToolStripMenuItem,
            this.toolStripSeparator1,
            this.showMoreInformationToolStripMenuItem,
            this.hideAlreadyMappedBodiesToolStripMenuItem,
            this.checkEDSMForInformationToolStripMenuItem,
            this.autoHideToolStripMenuItem,
            this.dontHideInFSSModeToolStripMenuItem,
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem,
            this.showValuesToolStripMenuItem,
            this.toolStripSeparator2,
            this.wordWrapToolStripMenuItem,
            this.textAlignToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(226, 346);
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
            this.ammoniaWorldToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.ammoniaWorldToolStripMenuItem.Text = "Ammonia World";
            // 
            // earthlikeWorldToolStripMenuItem
            // 
            this.earthlikeWorldToolStripMenuItem.Checked = true;
            this.earthlikeWorldToolStripMenuItem.CheckOnClick = true;
            this.earthlikeWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.earthlikeWorldToolStripMenuItem.Name = "earthlikeWorldToolStripMenuItem";
            this.earthlikeWorldToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.earthlikeWorldToolStripMenuItem.Text = "Earthlike World";
            // 
            // waterWorldToolStripMenuItem
            // 
            this.waterWorldToolStripMenuItem.Checked = true;
            this.waterWorldToolStripMenuItem.CheckOnClick = true;
            this.waterWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.waterWorldToolStripMenuItem.Name = "waterWorldToolStripMenuItem";
            this.waterWorldToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.waterWorldToolStripMenuItem.Text = "Water World";
            // 
            // bodyFeaturesToolStripMenuItem
            // 
            this.bodyFeaturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terraformableToolStripMenuItem,
            this.hasVolcanismToolStripMenuItem,
            this.hasRingsToolStripMenuItem,
            this.lowRadiusToolStripMenuItem,
            this.hasSignalsToolStripMenuItem});
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
            // 
            // hasVolcanismToolStripMenuItem
            // 
            this.hasVolcanismToolStripMenuItem.Checked = true;
            this.hasVolcanismToolStripMenuItem.CheckOnClick = true;
            this.hasVolcanismToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasVolcanismToolStripMenuItem.Name = "hasVolcanismToolStripMenuItem";
            this.hasVolcanismToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasVolcanismToolStripMenuItem.Text = "Has Volcanism";
            // 
            // hasRingsToolStripMenuItem
            // 
            this.hasRingsToolStripMenuItem.Checked = true;
            this.hasRingsToolStripMenuItem.CheckOnClick = true;
            this.hasRingsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasRingsToolStripMenuItem.Name = "hasRingsToolStripMenuItem";
            this.hasRingsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasRingsToolStripMenuItem.Text = "Has Rings";
            // 
            // lowRadiusToolStripMenuItem
            // 
            this.lowRadiusToolStripMenuItem.Checked = true;
            this.lowRadiusToolStripMenuItem.CheckOnClick = true;
            this.lowRadiusToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lowRadiusToolStripMenuItem.Name = "lowRadiusToolStripMenuItem";
            this.lowRadiusToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.lowRadiusToolStripMenuItem.Text = "Low Radius";
            // 
            // hasSignalsToolStripMenuItem
            // 
            this.hasSignalsToolStripMenuItem.Checked = true;
            this.hasSignalsToolStripMenuItem.CheckOnClick = true;
            this.hasSignalsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasSignalsToolStripMenuItem.Name = "hasSignalsToolStripMenuItem";
            this.hasSignalsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasSignalsToolStripMenuItem.Text = "Has Signals";
            this.hasSignalsToolStripMenuItem.Click += new System.EventHandler(this.hasSignalsToolStripMenuItem_Click);
            // 
            // showAllPlanetsToolStripMenuItem
            // 
            this.showAllPlanetsToolStripMenuItem.Checked = true;
            this.showAllPlanetsToolStripMenuItem.CheckOnClick = true;
            this.showAllPlanetsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAllPlanetsToolStripMenuItem.Name = "showAllPlanetsToolStripMenuItem";
            this.showAllPlanetsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showAllPlanetsToolStripMenuItem.Text = "Show All Planets";
            // 
            // showAllStarsToolStripMenuItem
            // 
            this.showAllStarsToolStripMenuItem.Checked = true;
            this.showAllStarsToolStripMenuItem.CheckOnClick = true;
            this.showAllStarsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAllStarsToolStripMenuItem.Name = "showAllStarsToolStripMenuItem";
            this.showAllStarsToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showAllStarsToolStripMenuItem.Text = "Show All Stars";
            // 
            // showBeltClustersToolStripMenuItem
            // 
            this.showBeltClustersToolStripMenuItem.Checked = true;
            this.showBeltClustersToolStripMenuItem.CheckOnClick = true;
            this.showBeltClustersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showBeltClustersToolStripMenuItem.Name = "showBeltClustersToolStripMenuItem";
            this.showBeltClustersToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showBeltClustersToolStripMenuItem.Text = "Show Belt Clusters";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // showMoreInformationToolStripMenuItem
            // 
            this.showMoreInformationToolStripMenuItem.Checked = true;
            this.showMoreInformationToolStripMenuItem.CheckOnClick = true;
            this.showMoreInformationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMoreInformationToolStripMenuItem.Name = "showMoreInformationToolStripMenuItem";
            this.showMoreInformationToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showMoreInformationToolStripMenuItem.Text = "Show More Information";
            // 
            // hideAlreadyMappedBodiesToolStripMenuItem
            // 
            this.hideAlreadyMappedBodiesToolStripMenuItem.Checked = true;
            this.hideAlreadyMappedBodiesToolStripMenuItem.CheckOnClick = true;
            this.hideAlreadyMappedBodiesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hideAlreadyMappedBodiesToolStripMenuItem.Name = "hideAlreadyMappedBodiesToolStripMenuItem";
            this.hideAlreadyMappedBodiesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.hideAlreadyMappedBodiesToolStripMenuItem.Text = "Hide already mapped bodies";
            // 
            // wordWrapToolStripMenuItem
            // 
            this.wordWrapToolStripMenuItem.Checked = true;
            this.wordWrapToolStripMenuItem.CheckOnClick = true;
            this.wordWrapToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
            this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.wordWrapToolStripMenuItem.Text = "Word Wrap";
            // 
            // checkEDSMForInformationToolStripMenuItem
            // 
            this.checkEDSMForInformationToolStripMenuItem.Checked = true;
            this.checkEDSMForInformationToolStripMenuItem.CheckOnClick = true;
            this.checkEDSMForInformationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEDSMForInformationToolStripMenuItem.Name = "checkEDSMForInformationToolStripMenuItem";
            this.checkEDSMForInformationToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.checkEDSMForInformationToolStripMenuItem.Text = "Check EDSM for information";
            // 
            // autoHideToolStripMenuItem
            // 
            this.autoHideToolStripMenuItem.Checked = true;
            this.autoHideToolStripMenuItem.CheckOnClick = true;
            this.autoHideToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoHideToolStripMenuItem.Name = "autoHideToolStripMenuItem";
            this.autoHideToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.autoHideToolStripMenuItem.Text = "Auto Hide";
            // 
            // dontHideInFSSModeToolStripMenuItem
            // 
            this.dontHideInFSSModeToolStripMenuItem.Checked = true;
            this.dontHideInFSSModeToolStripMenuItem.CheckOnClick = true;
            this.dontHideInFSSModeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dontHideInFSSModeToolStripMenuItem.Name = "dontHideInFSSModeToolStripMenuItem";
            this.dontHideInFSSModeToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.dontHideInFSSModeToolStripMenuItem.Text = "Don\'t Hide in FSS Mode";
            // 
            // showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem
            // 
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Checked = true;
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.CheckOnClick = true;
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Name = "showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem";
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem.Text = "Show System Info Always";
            // 
            // showValuesToolStripMenuItem
            // 
            this.showValuesToolStripMenuItem.Checked = true;
            this.showValuesToolStripMenuItem.CheckOnClick = true;
            this.showValuesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showValuesToolStripMenuItem.Name = "showValuesToolStripMenuItem";
            this.showValuesToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.showValuesToolStripMenuItem.Text = "Show values";
            // 
            // textAlignToolStripMenuItem
            // 
            this.textAlignToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftToolStripMenuItem,
            this.centerToolStripMenuItem,
            this.rightToolStripMenuItem});
            this.textAlignToolStripMenuItem.Name = "textAlignToolStripMenuItem";
            this.textAlignToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.textAlignToolStripMenuItem.Text = "Alignment";
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.leftToolStripMenuItem.Text = "Left";
            // 
            // centerToolStripMenuItem
            // 
            this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            this.centerToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.centerToolStripMenuItem.Text = "Center";
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.rightToolStripMenuItem.Text = "Right";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(222, 6);
            // 
            // UserControlSurveyor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxSurveyor);
            this.Name = "UserControlSurveyor";
            this.Size = new System.Drawing.Size(478, 229);
            this.Resize += new System.EventHandler(this.UserControlSurveyor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurveyor)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtPictureBox pictureBoxSurveyor;
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
        private System.Windows.Forms.ToolStripMenuItem textAlignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowRadiusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoHideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkEDSMForInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSystemInfoOnScreenWhenInTransparentModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dontHideInFSSModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hasSignalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPlanetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllStarsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBeltClustersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showMoreInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
