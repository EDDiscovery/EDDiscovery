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
    partial class UserControlShoppingList
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
            this.splitContainerVertical = new ExtendedControls.ExtSplitContainer();
            this.pictureBoxList = new ExtendedControls.ExtPictureBox();
            this.splitContainerRightHorz = new ExtendedControls.ExtSplitContainer();
            this.buttonTechBroker = new ExtendedControls.ExtButton();
            this.userControlSynthesis = new EDDiscovery.UserControls.UserControlSynthesis();
            this.userControlEngineering = new EDDiscovery.UserControls.UserControlEngineering();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMaxFSDInjectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBodyMaterialsWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlyCapacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useHistoricMaterialCountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSpecialEffects = new ExtendedControls.ExtButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).BeginInit();
            this.splitContainerRightHorz.Panel1.SuspendLayout();
            this.splitContainerRightHorz.Panel2.SuspendLayout();
            this.splitContainerRightHorz.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVertical.Location = new System.Drawing.Point(0, 0);
            this.splitContainerVertical.Name = "splitContainerVertical";
            // 
            // splitContainerVertical.Panel1
            // 
            this.splitContainerVertical.Panel1.Controls.Add(this.pictureBoxList);
            this.splitContainerVertical.Panel1MinSize = 100;
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.splitContainerRightHorz);
            this.splitContainerVertical.Size = new System.Drawing.Size(1142, 626);
            this.splitContainerVertical.SplitterDistance = 141;
            this.splitContainerVertical.TabIndex = 0;
            // 
            // pictureBoxList
            // 
            this.pictureBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxList.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxList.Name = "pictureBoxList";
            this.pictureBoxList.Size = new System.Drawing.Size(141, 626);
            this.pictureBoxList.TabIndex = 0;
            // 
            // splitContainerRightHorz
            // 
            this.splitContainerRightHorz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRightHorz.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRightHorz.Name = "splitContainerRightHorz";
            this.splitContainerRightHorz.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRightHorz.Panel1
            // 
            this.splitContainerRightHorz.Panel1.Controls.Add(this.buttonSpecialEffects);
            this.splitContainerRightHorz.Panel1.Controls.Add(this.buttonTechBroker);
            this.splitContainerRightHorz.Panel1.Controls.Add(this.userControlSynthesis);
            // 
            // splitContainerRightHorz.Panel2
            // 
            this.splitContainerRightHorz.Panel2.Controls.Add(this.userControlEngineering);
            this.splitContainerRightHorz.Size = new System.Drawing.Size(997, 626);
            this.splitContainerRightHorz.SplitterDistance = 234;
            this.splitContainerRightHorz.TabIndex = 0;
            // 
            // buttonTechBroker
            // 
            this.buttonTechBroker.Location = new System.Drawing.Point(3, 3);
            this.buttonTechBroker.Name = "buttonTechBroker";
            this.buttonTechBroker.Size = new System.Drawing.Size(150, 23);
            this.buttonTechBroker.TabIndex = 1;
            this.buttonTechBroker.Text = "Tech Broker Unlocks";
            this.buttonTechBroker.UseVisualStyleBackColor = true;
            this.buttonTechBroker.Click += new System.EventHandler(this.buttonTechBroker_Click);
            // 
            // userControlSynthesis
            // 
            this.userControlSynthesis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlSynthesis.Location = new System.Drawing.Point(0, 32);
            this.userControlSynthesis.Name = "userControlSynthesis";
            this.userControlSynthesis.Size = new System.Drawing.Size(994, 199);
            this.userControlSynthesis.TabIndex = 0;
            // 
            // userControlEngineering
            // 
            this.userControlEngineering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlEngineering.Location = new System.Drawing.Point(0, -1);
            this.userControlEngineering.Name = "userControlEngineering";
            this.userControlEngineering.Size = new System.Drawing.Size(994, 386);
            this.userControlEngineering.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMaxFSDInjectionsToolStripMenuItem,
            this.showBodyMaterialsWhenLandedToolStripMenuItem,
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem,
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem,
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem,
            this.useHistoricMaterialCountsToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuConfig";
            this.contextMenuStrip.Size = new System.Drawing.Size(369, 136);
            // 
            // showMaxFSDInjectionsToolStripMenuItem
            // 
            this.showMaxFSDInjectionsToolStripMenuItem.Checked = true;
            this.showMaxFSDInjectionsToolStripMenuItem.CheckOnClick = true;
            this.showMaxFSDInjectionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMaxFSDInjectionsToolStripMenuItem.Name = "showMaxFSDInjectionsToolStripMenuItem";
            this.showMaxFSDInjectionsToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showMaxFSDInjectionsToolStripMenuItem.Text = "Show Max FSD Injections";
            this.showMaxFSDInjectionsToolStripMenuItem.Click += new System.EventHandler(this.showMaxFSDInjectionsToolStripMenuItem_Click);
            // 
            // showBodyMaterialsWhenLandedToolStripMenuItem
            // 
            this.showBodyMaterialsWhenLandedToolStripMenuItem.CheckOnClick = true;
            this.showBodyMaterialsWhenLandedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlyCapacityToolStripMenuItem});
            this.showBodyMaterialsWhenLandedToolStripMenuItem.Name = "showBodyMaterialsWhenLandedToolStripMenuItem";
            this.showBodyMaterialsWhenLandedToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showBodyMaterialsWhenLandedToolStripMenuItem.Text = "Show Body Materials When Landed";
            this.showBodyMaterialsWhenLandedToolStripMenuItem.Click += new System.EventHandler(this.showAllMaterialsWhenLandedToolStripMenuItem_Click);
            // 
            // onlyCapacityToolStripMenuItem
            // 
            this.onlyCapacityToolStripMenuItem.CheckOnClick = true;
            this.onlyCapacityToolStripMenuItem.Name = "onlyCapacityToolStripMenuItem";
            this.onlyCapacityToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.onlyCapacityToolStripMenuItem.Text = "Hide when storage full";
            this.onlyCapacityToolStripMenuItem.Click += new System.EventHandler(this.onlyCapacityToolStripMenuItem_Click);
            // 
            // showAvailableMaterialsInListWhenLandedToolStripMenuItem
            // 
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.CheckOnClick = true;
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Name = "showAvailableMaterialsInListWhenLandedToolStripMenuItem";
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Text = "Include Material %age on Landed Body in Shopping List";
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Click += new System.EventHandler(this.showAvailableMaterialsInListWhenLandedToolStripMenuItem_Click);
            // 
            // showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem
            // 
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.CheckOnClick = true;
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Name = "showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem";
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Text = "Show System Availability in Shopping List in flight";
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Click += new System.EventHandler(this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem_Click);
            // 
            // useEDSMDataInSystemAvailabilityToolStripMenuItem
            // 
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem.CheckOnClick = true;
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem.Name = "useEDSMDataInSystemAvailabilityToolStripMenuItem";
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem.Text = "Use EDSM data in System Availability";
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem.Click += new System.EventHandler(this.useEDSMDataInSystemAvailabilityToolStripMenuItem_Click);
            // 
            // useHistoricMaterialCountsToolStripMenuItem
            // 
            this.useHistoricMaterialCountsToolStripMenuItem.CheckOnClick = true;
            this.useHistoricMaterialCountsToolStripMenuItem.Name = "useHistoricMaterialCountsToolStripMenuItem";
            this.useHistoricMaterialCountsToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.useHistoricMaterialCountsToolStripMenuItem.Text = "Use Historic Material Counts";
            this.useHistoricMaterialCountsToolStripMenuItem.Click += new System.EventHandler(this.useHistoricMaterialCountsToolStripMenuItem_Click);
            // 
            // buttonSpecialEffects
            // 
            this.buttonSpecialEffects.Location = new System.Drawing.Point(190, 3);
            this.buttonSpecialEffects.Name = "buttonSpecialEffects";
            this.buttonSpecialEffects.Size = new System.Drawing.Size(121, 23);
            this.buttonSpecialEffects.TabIndex = 1;
            this.buttonSpecialEffects.Text = "Special Effects";
            this.buttonSpecialEffects.UseVisualStyleBackColor = true;
            this.buttonSpecialEffects.Click += new System.EventHandler(this.buttonSpecialEffects_Click);
            // 
            // UserControlShoppingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerVertical);
            this.Name = "UserControlShoppingList";
            this.Size = new System.Drawing.Size(1142, 626);
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).EndInit();
            this.splitContainerVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxList)).EndInit();
            this.splitContainerRightHorz.Panel1.ResumeLayout(false);
            this.splitContainerRightHorz.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).EndInit();
            this.splitContainerRightHorz.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtSplitContainer splitContainerVertical;
        private ExtendedControls.ExtPictureBox pictureBoxList;
        private ExtendedControls.ExtSplitContainer splitContainerRightHorz;
        private UserControlSynthesis userControlSynthesis;
        private UserControlEngineering userControlEngineering;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showMaxFSDInjectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBodyMaterialsWhenLandedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAvailableMaterialsInListWhenLandedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useHistoricMaterialCountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useEDSMDataInSystemAvailabilityToolStripMenuItem;
        private ExtendedControls.ExtButton buttonTechBroker;
        private System.Windows.Forms.ToolStripMenuItem onlyCapacityToolStripMenuItem;
        private ExtendedControls.ExtButton buttonSpecialEffects;
    }
}
