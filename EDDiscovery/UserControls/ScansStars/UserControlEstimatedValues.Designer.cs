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
    partial class UserControlEstimatedValues
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataViewScrollerPanel2 = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom2 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewEstimatedValues = new System.Windows.Forms.DataGridView();
            this.BodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EDSM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mapped = new System.Windows.Forms.DataGridViewImageColumn();
            this.WasMapped = new System.Windows.Forms.DataGridViewImageColumn();
            this.WasDiscovered = new System.Windows.Forms.DataGridViewImageColumn();
            this.EstValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBoxEDSM = new ExtendedControls.ExtCheckBox();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEstimatedValues)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.extPanelRollUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel2
            // 
            this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewEstimatedValues);
            this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel2.LimitLargeChange = 2147483647;
            this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 542);
            this.dataViewScrollerPanel2.TabIndex = 25;
            this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom2.Location = new System.Drawing.Point(556, 0);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(16, 542);
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 24;
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
            // 
            // dataGridViewEstimatedValues
            // 
            this.dataGridViewEstimatedValues.AllowUserToAddRows = false;
            this.dataGridViewEstimatedValues.AllowUserToDeleteRows = false;
            this.dataGridViewEstimatedValues.AllowUserToResizeRows = false;
            this.dataGridViewEstimatedValues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEstimatedValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEstimatedValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BodyName,
            this.BodyType,
            this.EDSM,
            this.Mapped,
            this.WasMapped,
            this.WasDiscovered,
            this.EstValue});
            this.dataGridViewEstimatedValues.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewEstimatedValues.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEstimatedValues.Name = "dataGridViewEstimatedValues";
            this.dataGridViewEstimatedValues.RowHeadersVisible = false;
            this.dataGridViewEstimatedValues.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEstimatedValues.Size = new System.Drawing.Size(556, 542);
            this.dataGridViewEstimatedValues.TabIndex = 23;
            // 
            // BodyName
            // 
            this.BodyName.FillWeight = 55.37307F;
            this.BodyName.HeaderText = "Body Name";
            this.BodyName.MinimumWidth = 50;
            this.BodyName.Name = "BodyName";
            // 
            // BodyType
            // 
            this.BodyType.FillWeight = 54.70244F;
            this.BodyType.HeaderText = "Body Type";
            this.BodyType.Name = "BodyType";
            // 
            // EDSM
            // 
            this.EDSM.FillWeight = 25.86232F;
            this.EDSM.HeaderText = "EDSM";
            this.EDSM.Name = "EDSM";
            // 
            // Mapped
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.Mapped.DefaultCellStyle = dataGridViewCellStyle1;
            this.Mapped.FillWeight = 31.05361F;
            this.Mapped.HeaderText = "Mapped";
            this.Mapped.Name = "Mapped";
            this.Mapped.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Mapped.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // WasMapped
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.WasMapped.DefaultCellStyle = dataGridViewCellStyle2;
            this.WasMapped.FillWeight = 33.97367F;
            this.WasMapped.HeaderText = "Already Mapped";
            this.WasMapped.Name = "WasMapped";
            this.WasMapped.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // WasDiscovered
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.WasDiscovered.DefaultCellStyle = dataGridViewCellStyle3;
            this.WasDiscovered.FillWeight = 37.95792F;
            this.WasDiscovered.HeaderText = "Already Discovered";
            this.WasDiscovered.Name = "WasDiscovered";
            this.WasDiscovered.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EstValue
            // 
            this.EstValue.FillWeight = 15.82087F;
            this.EstValue.HeaderText = "Est Value";
            this.EstValue.MinimumWidth = 50;
            this.EstValue.Name = "EstValue";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.checkBoxEDSM);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(572, 30);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.Scan_FetchEDSMBodies;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.ImageIndeterminate = null;
            this.checkBoxEDSM.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.ImageUnchecked = null;
            this.checkBoxEDSM.Location = new System.Drawing.Point(8, 1);
            this.checkBoxEDSM.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(28, 28);
            this.checkBoxEDSM.TabIndex = 4;
            this.checkBoxEDSM.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "Get and show information from EDSM");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
            // 
            // extPanelRollUp
            // 
            this.extPanelRollUp.AutoSize = true;
            this.extPanelRollUp.Controls.Add(this.flowLayoutPanel1);
            this.extPanelRollUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelRollUp.HiddenMarkerWidth = 400;
            this.extPanelRollUp.Location = new System.Drawing.Point(0, 0);
            this.extPanelRollUp.Name = "extPanelRollUp";
            this.extPanelRollUp.PinState = true;
            this.extPanelRollUp.RolledUpHeight = 5;
            this.extPanelRollUp.RollUpAnimationTime = 500;
            this.extPanelRollUp.RollUpDelay = 1000;
            this.extPanelRollUp.SecondHiddenMarkerWidth = 0;
            this.extPanelRollUp.ShowHiddenMarker = true;
            this.extPanelRollUp.Size = new System.Drawing.Size(572, 30);
            this.extPanelRollUp.TabIndex = 25;
            this.extPanelRollUp.UnrollHoverDelay = 1000;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlEstimatedValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Controls.Add(this.extPanelRollUp);
            this.Name = "UserControlEstimatedValues";
            this.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEstimatedValues)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.extPanelRollUp.ResumeLayout(false);
            this.extPanelRollUp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel2;
        private ExtendedControls.ExtScrollBar vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewEstimatedValues;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ExtendedControls.ExtCheckBox checkBoxEDSM;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn EDSM;
        private System.Windows.Forms.DataGridViewImageColumn Mapped;
        private System.Windows.Forms.DataGridViewImageColumn WasMapped;
        private System.Windows.Forms.DataGridViewImageColumn WasDiscovered;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstValue;
    }
}
