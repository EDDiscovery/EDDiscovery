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
    partial class UserControlEDSM
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
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewEDSM = new System.Windows.Forms.DataGridView();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExtDBLookup = new ExtendedControls.ButtonExt();
            this.buttonExtEDSMSphere = new ExtendedControls.ButtonExt();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.textBoxRadius = new ExtendedControls.TextBoxBorder();
            this.textBoxSystemName = new ExtendedControls.TextBoxBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEDSM)).BeginInit();
            this.historyContextMenu.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewEDSM);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 80);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(804, 636);
            this.dataViewScrollerPanel1.TabIndex = 7;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(784, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 615);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 7;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewEDSM
            // 
            this.dataGridViewEDSM.AllowUserToAddRows = false;
            this.dataGridViewEDSM.AllowUserToDeleteRows = false;
            this.dataGridViewEDSM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEDSM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEDSM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnStar,
            this.ColumnDistance,
            this.ColumnPosition,
            this.ColumnID});
            this.dataGridViewEDSM.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewEDSM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEDSM.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEDSM.Name = "dataGridViewEDSM";
            this.dataGridViewEDSM.RowHeadersVisible = false;
            this.dataGridViewEDSM.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEDSM.Size = new System.Drawing.Size(784, 636);
            this.dataGridViewEDSM.TabIndex = 0;
            this.dataGridViewEDSM.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewEDSM_SortCompare);
            this.dataGridViewEDSM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewEDSM_MouseDown);
            // 
            // ColumnStar
            // 
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.MinimumWidth = 50;
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.FillWeight = 23F;
            this.ColumnDistance.HeaderText = "Distance";
            this.ColumnDistance.MinimumWidth = 50;
            this.ColumnDistance.Name = "ColumnDistance";
            this.ColumnDistance.ReadOnly = true;
            // 
            // ColumnPosition
            // 
            this.ColumnPosition.FillWeight = 75F;
            this.ColumnPosition.HeaderText = "Position";
            this.ColumnPosition.MinimumWidth = 50;
            this.ColumnPosition.Name = "ColumnPosition";
            this.ColumnPosition.ReadOnly = true;
            // 
            // ColumnID
            // 
            this.ColumnID.FillWeight = 25F;
            this.ColumnID.HeaderText = "ID";
            this.ColumnID.MinimumWidth = 50;
            this.ColumnID.Name = "ColumnID";
            this.ColumnID.ReadOnly = true;
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapGotoStartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(187, 48);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonExtDBLookup);
            this.panelTop.Controls.Add(this.buttonExtEDSMSphere);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.textBoxRadius);
            this.panelTop.Controls.Add(this.textBoxSystemName);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.labelFilter);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(804, 80);
            this.panelTop.TabIndex = 8;
            // 
            // buttonExtDBLookup
            // 
            this.buttonExtDBLookup.BorderColorScaling = 1.25F;
            this.buttonExtDBLookup.ButtonColorScaling = 0.5F;
            this.buttonExtDBLookup.ButtonDisabledScaling = 0.5F;
            this.buttonExtDBLookup.Location = new System.Drawing.Point(258, 7);
            this.buttonExtDBLookup.Name = "buttonExtDBLookup";
            this.buttonExtDBLookup.Size = new System.Drawing.Size(176, 23);
            this.buttonExtDBLookup.TabIndex = 31;
            this.buttonExtDBLookup.Text = "From DB Find Systems ";
            this.buttonExtDBLookup.UseVisualStyleBackColor = true;
            this.buttonExtDBLookup.Click += new System.EventHandler(this.buttonExtDBLookup_Click);
            // 
            // buttonExtEDSMSphere
            // 
            this.buttonExtEDSMSphere.BorderColorScaling = 1.25F;
            this.buttonExtEDSMSphere.ButtonColorScaling = 0.5F;
            this.buttonExtEDSMSphere.ButtonDisabledScaling = 0.5F;
            this.buttonExtEDSMSphere.Location = new System.Drawing.Point(258, 50);
            this.buttonExtEDSMSphere.Name = "buttonExtEDSMSphere";
            this.buttonExtEDSMSphere.Size = new System.Drawing.Size(176, 23);
            this.buttonExtEDSMSphere.TabIndex = 31;
            this.buttonExtEDSMSphere.Text = "From EDSM Sphere Systems";
            this.buttonExtEDSMSphere.UseVisualStyleBackColor = true;
            this.buttonExtEDSMSphere.Click += new System.EventHandler(this.buttonExtEDSMSphere_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BorderColorScaling = 1.25F;
            this.buttonExtExcel.ButtonColorScaling = 0.5F;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Properties.Resources.excel;
            this.buttonExtExcel.Location = new System.Drawing.Point(447, 8);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 30;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // textBoxRadius
            // 
            this.textBoxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxRadius.BorderColorScaling = 0.5F;
            this.textBoxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxRadius.Location = new System.Drawing.Point(87, 50);
            this.textBoxRadius.Multiline = false;
            this.textBoxRadius.Name = "textBoxRadius";
            this.textBoxRadius.ReadOnly = false;
            this.textBoxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxRadius.SelectionLength = 0;
            this.textBoxRadius.SelectionStart = 0;
            this.textBoxRadius.Size = new System.Drawing.Size(48, 20);
            this.textBoxRadius.TabIndex = 1;
            this.textBoxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRadius.WordWrap = true;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSystemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystemName.BorderColorScaling = 0.5F;
            this.textBoxSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSystemName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSystemName.Location = new System.Drawing.Point(87, 6);
            this.textBoxSystemName.Multiline = false;
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = false;
            this.textBoxSystemName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSystemName.SelectionLength = 0;
            this.textBoxSystemName.SelectionStart = 0;
            this.textBoxSystemName.Size = new System.Drawing.Size(148, 20);
            this.textBoxSystemName.TabIndex = 1;
            this.textBoxSystemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxSystemName, "Enter text to search in any fields for an item");
            this.textBoxSystemName.WordWrap = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Radius ly";
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(13, 8);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(60, 13);
            this.labelFilter.TabIndex = 24;
            this.labelFilter.Text = "Name Filter";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // UserControlEDSM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlEDSM";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEDSM)).EndInit();
            this.historyContextMenu.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private System.Windows.Forms.DataGridView dataGridViewEDSM;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.TextBoxBorder textBoxSystemName;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private ExtendedControls.ButtonExt buttonExtDBLookup;
        private ExtendedControls.ButtonExt buttonExtEDSMSphere;
        private ExtendedControls.TextBoxBorder textBoxRadius;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnID;
    }
}
