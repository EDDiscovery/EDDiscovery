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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlScanGrid));
            this.dataViewScrollerPanel2 = new ExtendedControls.DataViewScrollerPanel();
            this.dataGridViewScangrid = new System.Windows.Forms.DataGridView();
            this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
            this.img = new System.Windows.Forms.DataGridViewImageColumn();
            this.BodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel2
            // 
            this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewScangrid);
            this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
            this.dataViewScrollerPanel2.ScrollBarWidth = 20;
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.TabIndex = 25;
            this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewScangrid
            // 
            this.dataGridViewScangrid.AllowUserToAddRows = false;
            this.dataGridViewScangrid.AllowUserToDeleteRows = false;
            this.dataGridViewScangrid.AllowUserToResizeRows = false;
            this.dataGridViewScangrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScangrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScangrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.img,
            this.BodyName,
            this.BodyClass,
            this.BodyDetails});
            this.dataGridViewScangrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewScangrid.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewScangrid.Name = "dataGridViewScangrid";
            this.dataGridViewScangrid.RowHeadersVisible = false;
            this.dataGridViewScangrid.RowTemplate.Height = 35;
            this.dataGridViewScangrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewScangrid.Size = new System.Drawing.Size(552, 572);
            this.dataGridViewScangrid.TabIndex = 23;
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
            this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 551);
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
            // img
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.img.DefaultCellStyle = dataGridViewCellStyle1;
            this.img.FillWeight = 5F;
            this.img.HeaderText = "";
            this.img.Name = "img";
            this.img.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.img.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // BodyName
            // 
            this.BodyName.FillWeight = 15F;
            this.BodyName.HeaderText = "Name";
            this.BodyName.MinimumWidth = 20;
            this.BodyName.Name = "BodyName";
            // 
            // BodyClass
            // 
            this.BodyClass.FillWeight = 25F;
            this.BodyClass.HeaderText = "Class";
            this.BodyClass.MinimumWidth = 20;
            this.BodyClass.Name = "BodyClass";
            // 
            // BodyDetails
            // 
            this.BodyDetails.FillWeight = 50F;
            this.BodyDetails.HeaderText = "Details";
            this.BodyDetails.MinimumWidth = 30;
            this.BodyDetails.Name = "BodyDetails";
            // 
            // UserControlScanGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Name = "UserControlScanGrid";
            this.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewScangrid;
        private System.Windows.Forms.DataGridViewImageColumn img;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyDetails;
    }
}
