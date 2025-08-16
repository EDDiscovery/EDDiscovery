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
using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    partial class UserControlStarDistance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlStarDistance));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addToTrilaterationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addToExpeditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelExtMin = new System.Windows.Forms.Label();
            this.labelExtMax = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.textMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.textMinRadius = new ExtendedControls.NumberBoxDouble();
            this.checkBoxCube = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.flowLayoutStarDistances = new System.Windows.Forms.FlowLayoutPanel();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridViewNearest = new BaseUtils.DataGridViewColumnControl();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisited = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutStarDistances.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearest)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewSystemToolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem1,
            this.addToTrilaterationToolStripMenuItem1,
            this.addToExpeditionToolStripMenuItem});
            this.contextMenuStrip.Name = "closestContextMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(176, 114);
            // 
            // viewSystemToolStripMenuItem
            // 
            this.viewSystemToolStripMenuItem.Name = "viewSystemToolStripMenuItem";
            this.viewSystemToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.viewSystemToolStripMenuItem.Text = "View System";
            this.viewSystemToolStripMenuItem.Click += new System.EventHandler(this.viewSystemToolStripMenuItem_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem1
            // 
            this.viewOnEDSMToolStripMenuItem1.Name = "viewOnEDSMToolStripMenuItem1";
            this.viewOnEDSMToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.viewOnEDSMToolStripMenuItem1.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem1.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem1_Click);
            // 
            // addToTrilaterationToolStripMenuItem1
            // 
            this.addToTrilaterationToolStripMenuItem1.Name = "addToTrilaterationToolStripMenuItem1";
            this.addToTrilaterationToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.addToTrilaterationToolStripMenuItem1.Text = "Add to Trilateration";
            this.addToTrilaterationToolStripMenuItem1.Click += new System.EventHandler(this.addToTrilaterationToolStripMenuItem1_Click);
            // 
            // addToExpeditionToolStripMenuItem
            // 
            this.addToExpeditionToolStripMenuItem.Name = "addToExpeditionToolStripMenuItem";
            this.addToExpeditionToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.addToExpeditionToolStripMenuItem.Text = "Add to Expedition";
            this.addToExpeditionToolStripMenuItem.Click += new System.EventHandler(this.addToExpeditionToolStripMenuItem_Click);
            // 
            // labelExtMin
            // 
            this.labelExtMin.AutoSize = true;
            this.labelExtMin.Location = new System.Drawing.Point(3, 4);
            this.labelExtMin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelExtMin.Name = "labelExtMin";
            this.labelExtMin.Size = new System.Drawing.Size(24, 13);
            this.labelExtMin.TabIndex = 3;
            this.labelExtMin.Text = "Min";
            // 
            // labelExtMax
            // 
            this.labelExtMax.AutoSize = true;
            this.labelExtMax.Location = new System.Drawing.Point(91, 4);
            this.labelExtMax.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelExtMax.Name = "labelExtMax";
            this.labelExtMax.Size = new System.Drawing.Size(27, 13);
            this.labelExtMax.TabIndex = 3;
            this.labelExtMax.Text = "Max";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // textMaxRadius
            // 
            this.textMaxRadius.BackErrorColor = System.Drawing.Color.Red;
            this.textMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMaxRadius.BorderColor2 = System.Drawing.Color.Transparent;
            this.textMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMaxRadius.ClearOnFirstChar = false;
            this.textMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMaxRadius.DelayBeforeNotification = 500;
            this.textMaxRadius.EndButtonEnable = true;
            this.textMaxRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textMaxRadius.EndButtonImage")));
            this.textMaxRadius.EndButtonSize16ths = 10;
            this.textMaxRadius.EndButtonVisible = false;
            this.textMaxRadius.Format = "0.#######";
            this.textMaxRadius.InErrorCondition = false;
            this.textMaxRadius.Location = new System.Drawing.Point(124, 4);
            this.textMaxRadius.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textMaxRadius.Maximum = 100000D;
            this.textMaxRadius.Minimum = 0D;
            this.textMaxRadius.Multiline = false;
            this.textMaxRadius.Name = "textMaxRadius";
            this.textMaxRadius.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.textMaxRadius.ReadOnly = false;
            this.textMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMaxRadius.SelectionLength = 0;
            this.textMaxRadius.SelectionStart = 0;
            this.textMaxRadius.Size = new System.Drawing.Size(52, 20);
            this.textMaxRadius.TabIndex = 1;
            this.textMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMaxRadius.TextNoChange = "0";
            this.toolTip.SetToolTip(this.textMaxRadius, "Maximum star distance in ly");
            this.textMaxRadius.Value = 0D;
            this.textMaxRadius.WordWrap = true;
            this.textMaxRadius.ValueChanged += new System.EventHandler(this.textMaxRadius_ValueChanged);
            // 
            // textMinRadius
            // 
            this.textMinRadius.BackErrorColor = System.Drawing.Color.Red;
            this.textMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.textMinRadius.BorderColor2 = System.Drawing.Color.Transparent;
            this.textMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMinRadius.ClearOnFirstChar = false;
            this.textMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.textMinRadius.DelayBeforeNotification = 500;
            this.textMinRadius.EndButtonEnable = true;
            this.textMinRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textMinRadius.EndButtonImage")));
            this.textMinRadius.EndButtonSize16ths = 10;
            this.textMinRadius.EndButtonVisible = false;
            this.textMinRadius.Format = "0.#######";
            this.textMinRadius.InErrorCondition = false;
            this.textMinRadius.Location = new System.Drawing.Point(33, 4);
            this.textMinRadius.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textMinRadius.Maximum = 100000D;
            this.textMinRadius.Minimum = 0D;
            this.textMinRadius.Multiline = false;
            this.textMinRadius.Name = "textMinRadius";
            this.textMinRadius.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.textMinRadius.ReadOnly = false;
            this.textMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textMinRadius.SelectionLength = 0;
            this.textMinRadius.SelectionStart = 0;
            this.textMinRadius.Size = new System.Drawing.Size(52, 20);
            this.textMinRadius.TabIndex = 1;
            this.textMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textMinRadius.TextNoChange = "0";
            this.toolTip.SetToolTip(this.textMinRadius, "Minimum star distance in ly");
            this.textMinRadius.Value = 0D;
            this.textMinRadius.WordWrap = true;
            this.textMinRadius.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
            // 
            // checkBoxCube
            // 
            this.checkBoxCube.AutoSize = true;
            this.checkBoxCube.ButtonGradientDirection = 90F;
            this.checkBoxCube.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCube.CheckBoxGradientDirection = 225F;
            this.checkBoxCube.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCube.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCube.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCube.DisabledScaling = 0.5F;
            this.checkBoxCube.ImageIndeterminate = null;
            this.checkBoxCube.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCube.ImageUnchecked = null;
            this.checkBoxCube.Location = new System.Drawing.Point(182, 4);
            this.checkBoxCube.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.checkBoxCube.MouseOverScaling = 1.3F;
            this.checkBoxCube.MouseSelectedScaling = 1.3F;
            this.checkBoxCube.Name = "checkBoxCube";
            this.checkBoxCube.Size = new System.Drawing.Size(51, 17);
            this.checkBoxCube.TabIndex = 4;
            this.checkBoxCube.Text = "Cube";
            this.checkBoxCube.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCube, "Check to indicate use a cube instead of a sphere for distances");
            this.checkBoxCube.UseVisualStyleBackColor = true;
            this.checkBoxCube.CheckedChanged += new System.EventHandler(this.checkBoxCube_CheckedChanged);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(239, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // flowLayoutStarDistances
            // 
            this.flowLayoutStarDistances.AutoSize = true;
            this.flowLayoutStarDistances.Controls.Add(this.labelExtMin);
            this.flowLayoutStarDistances.Controls.Add(this.textMinRadius);
            this.flowLayoutStarDistances.Controls.Add(this.labelExtMax);
            this.flowLayoutStarDistances.Controls.Add(this.textMaxRadius);
            this.flowLayoutStarDistances.Controls.Add(this.checkBoxCube);
            this.flowLayoutStarDistances.Controls.Add(this.buttonExtExcel);
            this.flowLayoutStarDistances.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutStarDistances.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutStarDistances.Name = "flowLayoutStarDistances";
            this.flowLayoutStarDistances.Size = new System.Drawing.Size(352, 30);
            this.flowLayoutStarDistances.TabIndex = 5;
            this.flowLayoutStarDistances.WrapContents = false;
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewNearest);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(352, 542);
            this.dataViewScrollerPanel.TabIndex = 25;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(328, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 542);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderDrawAngle = 90F;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 24;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridViewNearest
            // 
            this.dataGridViewNearest.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewNearest.AllowUserToAddRows = false;
            this.dataGridViewNearest.AllowUserToDeleteRows = false;
            this.dataGridViewNearest.AllowUserToResizeRows = false;
            this.dataGridViewNearest.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewNearest.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewNearest.AutoSortByColumnName = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNearest.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewNearest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNearest.ColumnReorder = true;
            this.dataGridViewNearest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colDistance,
            this.colVisited});
            this.dataGridViewNearest.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewNearest.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewNearest.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewNearest.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewNearest.Name = "dataGridViewNearest";
            this.dataGridViewNearest.PerColumnWordWrapControl = true;
            this.dataGridViewNearest.RowHeaderMenuStrip = null;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNearest.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewNearest.RowHeadersVisible = false;
            this.dataGridViewNearest.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewNearest.SingleRowSelect = true;
            this.dataGridViewNearest.Size = new System.Drawing.Size(328, 542);
            this.dataGridViewNearest.TabIndex = 23;
            this.dataGridViewNearest.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewNearest_CellClick);
            this.dataGridViewNearest.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewNearest_CellDoubleClick);
            this.dataGridViewNearest.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewNearest_SortCompare);
            this.dataGridViewNearest.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewNearest_MouseDown);
            // 
            // colName
            // 
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 50;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colDistance
            // 
            this.colDistance.FillWeight = 25F;
            this.colDistance.HeaderText = "Distance";
            this.colDistance.MinimumWidth = 50;
            this.colDistance.Name = "colDistance";
            this.colDistance.ReadOnly = true;
            // 
            // colVisited
            // 
            this.colVisited.FillWeight = 25F;
            this.colVisited.HeaderText = "Visited";
            this.colVisited.Name = "colVisited";
            this.colVisited.ReadOnly = true;
            // 
            // UserControlStarDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.flowLayoutStarDistances);
            this.Name = "UserControlStarDistance";
            this.Size = new System.Drawing.Size(352, 572);
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutStarDistances.ResumeLayout(false);
            this.flowLayoutStarDistances.PerformLayout();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }               

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToTrilaterationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem1;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private BaseUtils.DataGridViewColumnControl dataGridViewNearest;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colDistance;
        private DataGridViewTextBoxColumn colVisited;
        private System.Windows.Forms.Label labelExtMin;
        private ExtendedControls.NumberBoxDouble textMinRadius;
        private System.Windows.Forms.Label labelExtMax;
        private ExtendedControls.NumberBoxDouble textMaxRadius;
        private ToolTip toolTip;
        private ExtendedControls.ExtCheckBox checkBoxCube;
        private ToolStripMenuItem addToExpeditionToolStripMenuItem;
        private FlowLayoutPanel flowLayoutStarDistances;
        private ToolStripMenuItem viewSystemToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ToolStripMenuItem viewOnSpanshToolStripMenuItem;
    }
}
