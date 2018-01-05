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
    partial class UserControlScanGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataViewScrollerPanel2 = new ExtendedControls.DataViewScrollerPanel();
            this.dataGridViewScangrid = new System.Windows.Forms.DataGridView();
            this.ImageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBriefing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTotalValue = new ExtendedControls.LabelExt();
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel2
            // 
            this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewScangrid);
            this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 26);
            this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
            this.dataViewScrollerPanel2.ScrollBarWidth = 20;
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 546);
            this.dataViewScrollerPanel2.TabIndex = 25;
            this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewScangrid
            // 
            this.dataGridViewScangrid.AllowUserToAddRows = false;
            this.dataGridViewScangrid.AllowUserToDeleteRows = false;
            this.dataGridViewScangrid.AllowUserToResizeRows = false;
            this.dataGridViewScangrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScangrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewScangrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScangrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImageColumn,
            this.colName,
            this.colClass,
            this.Distance,
            this.colBriefing});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewScangrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewScangrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScangrid.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewScangrid.Name = "dataGridViewScangrid";
            this.dataGridViewScangrid.ReadOnly = true;
            this.dataGridViewScangrid.RowHeadersVisible = false;
            this.dataGridViewScangrid.RowTemplate.Height = 36;
            this.dataGridViewScangrid.RowTemplate.ReadOnly = true;
            this.dataGridViewScangrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewScangrid.Size = new System.Drawing.Size(552, 546);
            this.dataGridViewScangrid.TabIndex = 23;
            this.dataGridViewScangrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewScangrid_RowPostPaint);
            // 
            // ImageColumn
            // 
            this.ImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ImageColumn.HeaderText = "";
            this.ImageColumn.MinimumWidth = 36;
            this.ImageColumn.Name = "ImageColumn";
            this.ImageColumn.ReadOnly = true;
            this.ImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ImageColumn.Width = 36;
            // 
            // colName
            // 
            this.colName.FillWeight = 37.56873F;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 20;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colClass
            // 
            this.colClass.FillWeight = 46.96092F;
            this.colClass.HeaderText = "Class";
            this.colClass.MinimumWidth = 20;
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            // 
            // Distance
            // 
            this.Distance.FillWeight = 23.39628F;
            this.Distance.HeaderText = "Distance";
            this.Distance.Name = "Distance";
            this.Distance.ReadOnly = true;
            // 
            // colBriefing
            // 
            this.colBriefing.FillWeight = 93.92184F;
            this.colBriefing.HeaderText = "Briefing";
            this.colBriefing.MinimumWidth = 20;
            this.colBriefing.Name = "colBriefing";
            this.colBriefing.ReadOnly = true;
            // 
            // vScrollBarCustom2
            // 
            this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom2.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom2.HideScrollBar = true;
            this.vScrollBarCustom2.LargeChange = 0;
            this.vScrollBarCustom2.Location = new System.Drawing.Point(552, 21);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 525);
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 24;
            this.vScrollBarCustom2.Text = "vScrollBarCustom2";
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTotalValue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 26);
            this.panel1.TabIndex = 26;
            // 
            // labelTotalValue
            // 
            this.labelTotalValue.AutoSize = true;
            this.labelTotalValue.Location = new System.Drawing.Point(3, 5);
            this.labelTotalValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelTotalValue.Name = "labelTotalValue";
            this.labelTotalValue.Size = new System.Drawing.Size(85, 13);
            this.labelTotalValue.TabIndex = 0;
            this.labelTotalValue.Text = "Scan data value";
            // 
            // UserControlScanGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Controls.Add(this.panel1);
            this.Name = "UserControlScanGrid";
            this.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewScangrid;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBriefing;
        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.LabelExt labelTotalValue;
    }
}
