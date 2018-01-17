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
namespace ActionLanguage
{
    partial class ActionProgramEditForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelVScroll = new ExtendedControls.PanelVScroll();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.buttonMore = new ExtendedControls.ButtonExt();
            this.panelName = new System.Windows.Forms.Panel();
            this.buttonExtDelete = new ExtendedControls.ButtonExt();
            this.textBoxBorderName = new ExtendedControls.TextBoxBorder();
            this.labelSet = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOK = new System.Windows.Forms.Panel();
            this.buttonExtDisk = new ExtendedControls.ButtonExt();
            this.buttonExtLoad = new ExtendedControls.ButtonExt();
            this.buttonExtSave = new ExtendedControls.ButtonExt();
            this.buttonExtEdit = new ExtendedControls.ButtonExt();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertEntryAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whitespaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeWhitespaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStripCustom = new ExtendedControls.StatusStripCustom();
            this.panelOuter.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.panelName.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelVScroll);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(3, 60);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Padding = new System.Windows.Forms.Padding(3);
            this.panelOuter.Size = new System.Drawing.Size(862, 388);
            this.panelOuter.TabIndex = 9;
            // 
            // panelVScroll
            // 
            this.panelVScroll.Controls.Add(this.vScrollBarCustom1);
            this.panelVScroll.Controls.Add(this.buttonMore);
            this.panelVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelVScroll.Location = new System.Drawing.Point(3, 3);
            this.panelVScroll.Name = "panelVScroll";
            this.panelVScroll.ScrollBarWidth = 20;
            this.panelVScroll.Size = new System.Drawing.Size(854, 380);
            this.panelVScroll.TabIndex = 8;
            this.panelVScroll.VerticalScrollBarDockRight = true;
            this.panelVScroll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelVScroll_MouseDown);
            this.panelVScroll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelVScroll_MouseMove);
            this.panelVScroll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelVScroll_MouseUp);
            this.panelVScroll.Resize += new System.EventHandler(this.panelVScroll_Resize);
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 32;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(834, 0);
            this.vScrollBarCustom1.Maximum = -314;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 380);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -314;
            this.vScrollBarCustom1.ValueLimited = -314;
            // 
            // buttonMore
            // 
            this.buttonMore.BorderColorScaling = 1.25F;
            this.buttonMore.ButtonColorScaling = 0.5F;
            this.buttonMore.ButtonDisabledScaling = 0.5F;
            this.buttonMore.Location = new System.Drawing.Point(6, 6);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(24, 24);
            this.buttonMore.TabIndex = 5;
            this.buttonMore.Text = "+";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.buttonExtDelete);
            this.panelName.Controls.Add(this.textBoxBorderName);
            this.panelName.Controls.Add(this.labelSet);
            this.panelName.Controls.Add(this.labelName);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelName.Location = new System.Drawing.Point(3, 24);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(862, 36);
            this.panelName.TabIndex = 8;
            // 
            // buttonExtDelete
            // 
            this.buttonExtDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtDelete.BorderColorScaling = 1.25F;
            this.buttonExtDelete.ButtonColorScaling = 0.5F;
            this.buttonExtDelete.ButtonDisabledScaling = 0.5F;
            this.buttonExtDelete.Location = new System.Drawing.Point(833, 4);
            this.buttonExtDelete.Name = "buttonExtDelete";
            this.buttonExtDelete.Size = new System.Drawing.Size(25, 23);
            this.buttonExtDelete.TabIndex = 25;
            this.buttonExtDelete.Text = "X";
            this.buttonExtDelete.UseVisualStyleBackColor = true;
            this.buttonExtDelete.Click += new System.EventHandler(this.buttonExtDelete_Click);
            // 
            // textBoxBorderName
            // 
            this.textBoxBorderName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderName.BorderColorScaling = 0.5F;
            this.textBoxBorderName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderName.Location = new System.Drawing.Point(152, 4);
            this.textBoxBorderName.Multiline = false;
            this.textBoxBorderName.Name = "textBoxBorderName";
            this.textBoxBorderName.ReadOnly = false;
            this.textBoxBorderName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderName.SelectionLength = 0;
            this.textBoxBorderName.SelectionStart = 0;
            this.textBoxBorderName.Size = new System.Drawing.Size(154, 20);
            this.textBoxBorderName.TabIndex = 0;
            this.textBoxBorderName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorderName.WordWrap = true;
            // 
            // labelSet
            // 
            this.labelSet.AutoSize = true;
            this.labelSet.Location = new System.Drawing.Point(53, 7);
            this.labelSet.Name = "labelSet";
            this.labelSet.Size = new System.Drawing.Size(33, 13);
            this.labelSet.TabIndex = 23;
            this.labelSet.Text = "<set>";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 7);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 23;
            this.labelName.Text = "Name:";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(862, 24);
            this.panelTop.TabIndex = 29;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(839, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(809, 0);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(27, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "N/A";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panelOK
            // 
            this.panelOK.Controls.Add(this.buttonExtDisk);
            this.panelOK.Controls.Add(this.buttonExtLoad);
            this.panelOK.Controls.Add(this.buttonExtSave);
            this.panelOK.Controls.Add(this.buttonExtEdit);
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(3, 448);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(862, 30);
            this.panelOK.TabIndex = 9;
            // 
            // buttonExtDisk
            // 
            this.buttonExtDisk.BorderColorScaling = 1.25F;
            this.buttonExtDisk.ButtonColorScaling = 0.5F;
            this.buttonExtDisk.ButtonDisabledScaling = 0.5F;
            this.buttonExtDisk.Location = new System.Drawing.Point(275, 4);
            this.buttonExtDisk.Name = "buttonExtDisk";
            this.buttonExtDisk.Size = new System.Drawing.Size(75, 23);
            this.buttonExtDisk.TabIndex = 11;
            this.buttonExtDisk.Text = "As File";
            this.buttonExtDisk.UseVisualStyleBackColor = true;
            this.buttonExtDisk.Click += new System.EventHandler(this.buttonExtDisk_Click);
            // 
            // buttonExtLoad
            // 
            this.buttonExtLoad.BorderColorScaling = 1.25F;
            this.buttonExtLoad.ButtonColorScaling = 0.5F;
            this.buttonExtLoad.ButtonDisabledScaling = 0.5F;
            this.buttonExtLoad.Location = new System.Drawing.Point(170, 4);
            this.buttonExtLoad.Name = "buttonExtLoad";
            this.buttonExtLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonExtLoad.TabIndex = 10;
            this.buttonExtLoad.Text = "Load";
            this.buttonExtLoad.UseVisualStyleBackColor = true;
            this.buttonExtLoad.Click += new System.EventHandler(this.buttonExtLoad_Click);
            // 
            // buttonExtSave
            // 
            this.buttonExtSave.BorderColorScaling = 1.25F;
            this.buttonExtSave.ButtonColorScaling = 0.5F;
            this.buttonExtSave.ButtonDisabledScaling = 0.5F;
            this.buttonExtSave.Location = new System.Drawing.Point(88, 4);
            this.buttonExtSave.Name = "buttonExtSave";
            this.buttonExtSave.Size = new System.Drawing.Size(75, 23);
            this.buttonExtSave.TabIndex = 9;
            this.buttonExtSave.Text = "Save";
            this.buttonExtSave.UseVisualStyleBackColor = true;
            this.buttonExtSave.Click += new System.EventHandler(this.buttonExtSave_Click);
            // 
            // buttonExtEdit
            // 
            this.buttonExtEdit.BorderColorScaling = 1.25F;
            this.buttonExtEdit.ButtonColorScaling = 0.5F;
            this.buttonExtEdit.ButtonDisabledScaling = 0.5F;
            this.buttonExtEdit.Location = new System.Drawing.Point(6, 4);
            this.buttonExtEdit.Name = "buttonExtEdit";
            this.buttonExtEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonExtEdit.TabIndex = 8;
            this.buttonExtEdit.Text = "Text Edit";
            this.buttonExtEdit.UseVisualStyleBackColor = true;
            this.buttonExtEdit.Click += new System.EventHandler(this.buttonExtEdit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(686, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BorderColorScaling = 1.25F;
            this.buttonOK.ButtonColorScaling = 0.5F;
            this.buttonOK.ButtonDisabledScaling = 0.5F;
            this.buttonOK.Location = new System.Drawing.Point(783, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.insertEntryAboveToolStripMenuItem,
            this.whitespaceToolStripMenuItem,
            this.removeWhitespaceToolStripMenuItem,
            this.editCommentToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(201, 158);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // insertEntryAboveToolStripMenuItem
            // 
            this.insertEntryAboveToolStripMenuItem.Name = "insertEntryAboveToolStripMenuItem";
            this.insertEntryAboveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.insertEntryAboveToolStripMenuItem.Text = "Insert Entry above";
            this.insertEntryAboveToolStripMenuItem.Click += new System.EventHandler(this.insertEntryAboveToolStripMenuItem_Click);
            // 
            // whitespaceToolStripMenuItem
            // 
            this.whitespaceToolStripMenuItem.Name = "whitespaceToolStripMenuItem";
            this.whitespaceToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.whitespaceToolStripMenuItem.Text = "Insert whitespace below";
            this.whitespaceToolStripMenuItem.Click += new System.EventHandler(this.whitespaceToolStripMenuItem_Click);
            // 
            // removeWhitespaceToolStripMenuItem
            // 
            this.removeWhitespaceToolStripMenuItem.Name = "removeWhitespaceToolStripMenuItem";
            this.removeWhitespaceToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.removeWhitespaceToolStripMenuItem.Text = "Remove whitespace";
            this.removeWhitespaceToolStripMenuItem.Click += new System.EventHandler(this.removeWhitespaceToolStripMenuItem_Click);
            // 
            // editCommentToolStripMenuItem
            // 
            this.editCommentToolStripMenuItem.Name = "editCommentToolStripMenuItem";
            this.editCommentToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.editCommentToolStripMenuItem.Text = "Edit Comment";
            this.editCommentToolStripMenuItem.Click += new System.EventHandler(this.editCommentToolStripMenuItem_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(3, 478);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(862, 22);
            this.statusStripCustom.TabIndex = 28;
            this.statusStripCustom.Text = "statusStripCustom1";
            // 
            // ActionProgramEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 500);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelName);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "ActionProgramEditForm";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ActionProgramForm";
            this.Shown += new System.EventHandler(this.ActionProgramForm_Shown);
            this.panelOuter.ResumeLayout(false);
            this.panelVScroll.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.PanelVScroll panelVScroll;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.ButtonExt buttonMore;
        private ExtendedControls.StatusStripCustom statusStripCustom;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelName;
        private ExtendedControls.TextBoxBorder textBoxBorderName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.ButtonExt buttonExtDelete;
        private System.Windows.Forms.Label labelSet;
        private ExtendedControls.ButtonExt buttonExtLoad;
        private ExtendedControls.ButtonExt buttonExtSave;
        private ExtendedControls.ButtonExt buttonExtEdit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whitespaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeWhitespaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertEntryAboveToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ButtonExt buttonExtDisk;
        private System.Windows.Forms.ToolStripMenuItem editCommentToolStripMenuItem;
    }
}