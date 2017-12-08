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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerVertical = new ExtendedControls.SplitContainerCustom();
            this.pictureBoxList = new ExtendedControls.PictureBoxHotspot();
            this.splitContainerRightHorz = new ExtendedControls.SplitContainerCustom();
            this.userControlSynthesis = new EDDiscovery.UserControls.UserControlSynthesis();
            this.userControlEngineering = new EDDiscovery.UserControls.UserControlEngineering();
            this.contextMenuConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMaxFSDInjectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllMaterialsWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).BeginInit();
            this.splitContainerVertical.Panel1.SuspendLayout();
            this.splitContainerVertical.Panel2.SuspendLayout();
            this.splitContainerVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).BeginInit();
            this.splitContainerRightHorz.Panel1.SuspendLayout();
            this.splitContainerRightHorz.Panel2.SuspendLayout();
            this.splitContainerRightHorz.SuspendLayout();
            this.contextMenuConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // splitContainerVertical
            // 
            this.splitContainerVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.splitContainerVertical.Size = new System.Drawing.Size(971, 572);
            this.splitContainerVertical.SplitterDistance = 120;
            this.splitContainerVertical.TabIndex = 0;
            // 
            // pictureBoxList
            // 
            this.pictureBoxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxList.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxList.Name = "pictureBoxList";
            this.pictureBoxList.Size = new System.Drawing.Size(120, 572);
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
            // 
            // splitContainerRightHorz.Panel2
            // 
            this.splitContainerRightHorz.Panel2.Controls.Add(this.userControlEngineering);
            this.splitContainerRightHorz.Size = new System.Drawing.Size(847, 572);
            this.splitContainerRightHorz.SplitterDistance = 214;
            this.splitContainerRightHorz.TabIndex = 0;
            // 
            // userControlSynthesis
            // 
            this.userControlSynthesis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlSynthesis.Location = new System.Drawing.Point(0, 0);
            this.userControlSynthesis.Name = "userControlSynthesis";
            this.userControlSynthesis.Size = new System.Drawing.Size(844, 211);
            this.userControlSynthesis.TabIndex = 0;
            // 
            // userControlEngineering
            // 
            this.userControlEngineering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlEngineering.Location = new System.Drawing.Point(0, -1);
            this.userControlEngineering.Name = "userControlEngineering";
            this.userControlEngineering.Size = new System.Drawing.Size(844, 352);
            this.userControlEngineering.TabIndex = 0;
            // 
            // contextMenuConfig
            // 
            this.contextMenuConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMaxFSDInjectionsToolStripMenuItem,
            this.showAllMaterialsWhenLandedToolStripMenuItem,
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem});
            this.contextMenuConfig.Name = "contextMenuConfig";
            this.contextMenuConfig.Size = new System.Drawing.Size(369, 92);
            // 
            // showMaxFSDInjectionsToolStripMenuItem
            // 
            this.showMaxFSDInjectionsToolStripMenuItem.Checked = true;
            this.showMaxFSDInjectionsToolStripMenuItem.CheckOnClick = true;
            this.showMaxFSDInjectionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMaxFSDInjectionsToolStripMenuItem.Name = "showMaxFSDInjectionsToolStripMenuItem";
            this.showMaxFSDInjectionsToolStripMenuItem.Size = new System.Drawing.Size(315, 22);
            this.showMaxFSDInjectionsToolStripMenuItem.Text = "Show Max FSD Injections";
            this.showMaxFSDInjectionsToolStripMenuItem.Click += new System.EventHandler(this.showMaxFSDInjectionsToolStripMenuItem_Click);
            // 
            // showAllMaterialsWhenLandedToolStripMenuItem
            // 
            this.showAllMaterialsWhenLandedToolStripMenuItem.CheckOnClick = true;
            this.showAllMaterialsWhenLandedToolStripMenuItem.Name = "showAllMaterialsWhenLandedToolStripMenuItem";
            this.showAllMaterialsWhenLandedToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showAllMaterialsWhenLandedToolStripMenuItem.Text = "Show Body Materials When Landed";
            this.showAllMaterialsWhenLandedToolStripMenuItem.Click += new System.EventHandler(this.showAllMaterialsWhenLandedToolStripMenuItem_Click);
            // 
            // showAvailableMaterialsInListWhenLandedToolStripMenuItem
            // 
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.CheckOnClick = true;
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Name = "showAvailableMaterialsInListWhenLandedToolStripMenuItem";
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Size = new System.Drawing.Size(368, 22);
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Text = "Include Material %age on Landed Body in Shopping List";
            this.showAvailableMaterialsInListWhenLandedToolStripMenuItem.Click += new System.EventHandler(this.showAvailableMaterialsInListWhenLandedToolStripMenuItem_Click);
            // 
            // UserControlShoppingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerVertical);
            this.Name = "UserControlShoppingList";
            this.Size = new System.Drawing.Size(971, 572);
            this.splitContainerVertical.Panel1.ResumeLayout(false);
            this.splitContainerVertical.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVertical)).EndInit();
            this.splitContainerVertical.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxList)).EndInit();
            this.splitContainerRightHorz.Panel1.ResumeLayout(false);
            this.splitContainerRightHorz.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightHorz)).EndInit();
            this.splitContainerRightHorz.ResumeLayout(false);
            this.contextMenuConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.SplitContainerCustom splitContainerVertical;
        private ExtendedControls.PictureBoxHotspot pictureBoxList;
        private ExtendedControls.SplitContainerCustom splitContainerRightHorz;
        private UserControlSynthesis userControlSynthesis;
        private UserControlEngineering userControlEngineering;
        private System.Windows.Forms.ContextMenuStrip contextMenuConfig;
        private System.Windows.Forms.ToolStripMenuItem showMaxFSDInjectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllMaterialsWhenLandedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAvailableMaterialsInListWhenLandedToolStripMenuItem;
    }
}
