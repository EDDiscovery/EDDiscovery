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
            this.splitContainerVertical = new System.Windows.Forms.SplitContainer();
            this.pictureBoxList = new ExtendedControls.ExtPictureBox();
            this.splitContainerRightHorz = new System.Windows.Forms.SplitContainer();
            this.userControlSynthesis = new EDDiscovery.UserControls.UserControlSynthesis();
            this.panelSpecialButs = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonTechBroker = new ExtendedControls.ExtButton();
            this.buttonSpecialEffects = new ExtendedControls.ExtButton();
            this.userControlEngineering = new EDDiscovery.UserControls.UserControlEngineering();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMaxFSDInjectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBodyMaterialsWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlyCapacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useEDSMDataInSystemAvailabilityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useHistoricMaterialCountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleListPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).BeginInit();
            this.splitContainerRightHorz.Panel1.SuspendLayout();
            this.splitContainerRightHorz.Panel2.SuspendLayout();
            this.splitContainerRightHorz.SuspendLayout();
            this.panelSpecialButs.SuspendLayout();
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
            this.splitContainerVertical.Panel1MinSize = 1;
            // 
            // splitContainerVertical.Panel2
            // 
            this.splitContainerVertical.Panel2.Controls.Add(this.splitContainerRightHorz);
            this.splitContainerVertical.Panel2MinSize = 1;
            this.splitContainerVertical.Size = new System.Drawing.Size(1142, 626);
            this.splitContainerVertical.SplitterDistance = 119;
            this.splitContainerVertical.TabIndex = 0;
            // 
            // pictureBoxList
            // 
            this.pictureBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxList.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxList.Name = "pictureBoxList";
            this.pictureBoxList.Size = new System.Drawing.Size(119, 626);
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
            this.splitContainerRightHorz.Panel1.Controls.Add(this.userControlSynthesis);
            this.splitContainerRightHorz.Panel1.Controls.Add(this.panelSpecialButs);
            // 
            // splitContainerRightHorz.Panel2
            // 
            this.splitContainerRightHorz.Panel2.Controls.Add(this.userControlEngineering);
            this.splitContainerRightHorz.Size = new System.Drawing.Size(1019, 626);
            this.splitContainerRightHorz.SplitterDistance = 234;
            this.splitContainerRightHorz.TabIndex = 0;
            // 
            // userControlSynthesis
            // 
            this.userControlSynthesis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSynthesis.Location = new System.Drawing.Point(0, 29);
            this.userControlSynthesis.Name = "userControlSynthesis";
            this.userControlSynthesis.Size = new System.Drawing.Size(1019, 205);
            this.userControlSynthesis.TabIndex = 0;
            // 
            // panelSpecialButs
            // 
            this.panelSpecialButs.AutoSize = true;
            this.panelSpecialButs.Controls.Add(this.buttonTechBroker);
            this.panelSpecialButs.Controls.Add(this.buttonSpecialEffects);
            this.panelSpecialButs.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpecialButs.Location = new System.Drawing.Point(0, 0);
            this.panelSpecialButs.Name = "panelSpecialButs";
            this.panelSpecialButs.Size = new System.Drawing.Size(1019, 29);
            this.panelSpecialButs.TabIndex = 2;
            // 
            // buttonTechBroker
            // 
            this.buttonTechBroker.Location = new System.Drawing.Point(0, 3);
            this.buttonTechBroker.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonTechBroker.Name = "buttonTechBroker";
            this.buttonTechBroker.Size = new System.Drawing.Size(150, 23);
            this.buttonTechBroker.TabIndex = 1;
            this.buttonTechBroker.Text = "Tech Broker Unlocks";
            this.buttonTechBroker.UseVisualStyleBackColor = true;
            this.buttonTechBroker.Click += new System.EventHandler(this.buttonTechBroker_Click);
            // 
            // buttonSpecialEffects
            // 
            this.buttonSpecialEffects.Location = new System.Drawing.Point(156, 3);
            this.buttonSpecialEffects.Name = "buttonSpecialEffects";
            this.buttonSpecialEffects.Size = new System.Drawing.Size(121, 23);
            this.buttonSpecialEffects.TabIndex = 1;
            this.buttonSpecialEffects.Text = "Special Effects";
            this.buttonSpecialEffects.UseVisualStyleBackColor = true;
            this.buttonSpecialEffects.Click += new System.EventHandler(this.buttonSpecialEffects_Click);
            // 
            // userControlEngineering
            // 
            this.userControlEngineering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlEngineering.Location = new System.Drawing.Point(0, -1);
            this.userControlEngineering.Name = "userControlEngineering";
            this.userControlEngineering.Size = new System.Drawing.Size(1016, 386);
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
            this.useHistoricMaterialCountsToolStripMenuItem,
            this.toggleListPositionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuConfig";
            this.contextMenuStrip.Size = new System.Drawing.Size(369, 158);
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
            // toggleListPositionToolStripMenuItem
            // 
            this.toggleListPositionToolStripMenuItem.Name = "toggleListPositionToolStripMenuItem";
            this.toggleListPositionToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.toggleListPositionToolStripMenuItem.Text = "Toggle Shopping List Position";
            this.toggleListPositionToolStripMenuItem.Click += new System.EventHandler(this.ToggleListPositionToolStripMenuItem_Click);
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
            this.splitContainerRightHorz.Panel1.PerformLayout();
            this.splitContainerRightHorz.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).EndInit();
            this.splitContainerRightHorz.ResumeLayout(false);
            this.panelSpecialButs.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainerVertical;
        private ExtendedControls.ExtPictureBox pictureBoxList;
        private System.Windows.Forms.SplitContainer splitContainerRightHorz;
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
        private System.Windows.Forms.FlowLayoutPanel panelSpecialButs;
        private System.Windows.Forms.ToolStripMenuItem toggleListPositionToolStripMenuItem;
    }
}
