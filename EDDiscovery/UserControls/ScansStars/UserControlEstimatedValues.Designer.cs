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
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustomDGV = new ExtendedControls.ExtScrollBar();
            this.dataGridViewEstimatedValues = new BaseUtils.DataGridViewColumnControl();
            this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.checkBoxShowZeros = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowImpossible = new ExtendedControls.ExtCheckBox();
            this.labelControlText = new System.Windows.Forms.Label();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.BodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EDSM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mapped = new System.Windows.Forms.DataGridViewImageColumn();
            this.WasMapped = new System.Windows.Forms.DataGridViewImageColumn();
            this.WasDiscovered = new System.Windows.Forms.DataGridViewImageColumn();
            this.EstBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MappedValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstMappedEff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstDiscMapped = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEstimatedValues)).BeginInit();
            this.flowLayoutPanelTop.SuspendLayout();
            this.extPanelRollUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomDGV);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewEstimatedValues);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(1247, 542);
            this.dataViewScrollerPanel.TabIndex = 25;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustomDGV
            // 
            this.vScrollBarCustomDGV.AlwaysHideScrollBar = false;
            this.vScrollBarCustomDGV.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomDGV.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomDGV.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomDGV.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomDGV.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomDGV.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomDGV.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustomDGV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomDGV.HideScrollBar = true;
            this.vScrollBarCustomDGV.LargeChange = 0;
            this.vScrollBarCustomDGV.Location = new System.Drawing.Point(1228, 0);
            this.vScrollBarCustomDGV.Maximum = -1;
            this.vScrollBarCustomDGV.Minimum = 0;
            this.vScrollBarCustomDGV.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomDGV.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomDGV.Name = "vScrollBarCustomDGV";
            this.vScrollBarCustomDGV.Size = new System.Drawing.Size(19, 542);
            this.vScrollBarCustomDGV.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomDGV.SmallChange = 1;
            this.vScrollBarCustomDGV.TabIndex = 24;
            this.vScrollBarCustomDGV.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomDGV.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomDGV.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomDGV.ThumbDrawAngle = 0F;
            this.vScrollBarCustomDGV.Value = -1;
            this.vScrollBarCustomDGV.ValueLimited = -1;
            // 
            // dataGridViewEstimatedValues
            // 
            this.dataGridViewEstimatedValues.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewEstimatedValues.AllowUserToAddRows = false;
            this.dataGridViewEstimatedValues.AllowUserToDeleteRows = false;
            this.dataGridViewEstimatedValues.AllowUserToResizeRows = false;
            this.dataGridViewEstimatedValues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEstimatedValues.AutoSortByColumnName = false;
            this.dataGridViewEstimatedValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEstimatedValues.ColumnReorder = true;
            this.dataGridViewEstimatedValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BodyName,
            this.BodyType,
            this.EDSM,
            this.Mapped,
            this.WasMapped,
            this.WasDiscovered,
            this.EstBase,
            this.MappedValue,
            this.FirstMappedEff,
            this.FirstDiscMapped,
            this.EstValue});
            this.dataGridViewEstimatedValues.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewEstimatedValues.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEstimatedValues.Name = "dataGridViewEstimatedValues";
            this.dataGridViewEstimatedValues.PerColumnWordWrapControl = true;
            this.dataGridViewEstimatedValues.RowHeaderMenuStrip = null;
            this.dataGridViewEstimatedValues.RowHeadersVisible = false;
            this.dataGridViewEstimatedValues.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEstimatedValues.SingleRowSelect = true;
            this.dataGridViewEstimatedValues.Size = new System.Drawing.Size(1228, 542);
            this.dataGridViewEstimatedValues.TabIndex = 23;
            this.dataGridViewEstimatedValues.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewEstimatedValues_SortCompare);
            // 
            // flowLayoutPanelTop
            // 
            this.flowLayoutPanelTop.AutoSize = true;
            this.flowLayoutPanelTop.Controls.Add(this.edsmSpanshButton);
            this.flowLayoutPanelTop.Controls.Add(this.checkBoxShowZeros);
            this.flowLayoutPanelTop.Controls.Add(this.extCheckBoxShowImpossible);
            this.flowLayoutPanelTop.Controls.Add(this.labelControlText);
            this.flowLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
            this.flowLayoutPanelTop.Size = new System.Drawing.Size(1247, 30);
            this.flowLayoutPanelTop.TabIndex = 5;
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(8, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 71;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowZeros
            // 
            this.checkBoxShowZeros.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxShowZeros.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxShowZeros.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxShowZeros.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxShowZeros.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxShowZeros.Image = global::EDDiscovery.Icons.Controls.greenzero;
            this.checkBoxShowZeros.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxShowZeros.ImageIndeterminate = null;
            this.checkBoxShowZeros.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxShowZeros.ImageUnchecked = global::EDDiscovery.Icons.Controls.redzero;
            this.checkBoxShowZeros.Location = new System.Drawing.Point(40, 1);
            this.checkBoxShowZeros.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.checkBoxShowZeros.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxShowZeros.Name = "checkBoxShowZeros";
            this.checkBoxShowZeros.Size = new System.Drawing.Size(28, 28);
            this.checkBoxShowZeros.TabIndex = 5;
            this.checkBoxShowZeros.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxShowZeros, "Green will show all bodies even with zero value, red will only show bodies with v" +
        "alue");
            this.checkBoxShowZeros.UseVisualStyleBackColor = true;
            // 
            // extCheckBoxShowImpossible
            // 
            this.extCheckBoxShowImpossible.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowImpossible.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowImpossible.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxShowImpossible.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowImpossible.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowImpossible.Image = global::EDDiscovery.Icons.Controls.HighValueTick;
            this.extCheckBoxShowImpossible.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxShowImpossible.ImageIndeterminate = null;
            this.extCheckBoxShowImpossible.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowImpossible.ImageUnchecked = null;
            this.extCheckBoxShowImpossible.Location = new System.Drawing.Point(76, 1);
            this.extCheckBoxShowImpossible.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxShowImpossible.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxShowImpossible.Name = "extCheckBoxShowImpossible";
            this.extCheckBoxShowImpossible.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowImpossible.TabIndex = 5;
            this.extCheckBoxShowImpossible.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowImpossible, "Show all values, even ones which are impossible to get");
            this.extCheckBoxShowImpossible.UseVisualStyleBackColor = true;
            // 
            // labelControlText
            // 
            this.labelControlText.AutoSize = true;
            this.labelControlText.Location = new System.Drawing.Point(115, 0);
            this.labelControlText.Name = "labelControlText";
            this.labelControlText.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.labelControlText.Size = new System.Drawing.Size(43, 18);
            this.labelControlText.TabIndex = 6;
            this.labelControlText.Text = "<code>";
            // 
            // extPanelRollUp
            // 
            this.extPanelRollUp.AutoHeight = false;
            this.extPanelRollUp.AutoHeightWidthDisable = false;
            this.extPanelRollUp.AutoSize = true;
            this.extPanelRollUp.AutoWidth = false;
            this.extPanelRollUp.Controls.Add(this.flowLayoutPanelTop);
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
            this.extPanelRollUp.Size = new System.Drawing.Size(1247, 30);
            this.extPanelRollUp.TabIndex = 25;
            this.extPanelRollUp.UnrollHoverDelay = 1000;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // BodyName
            // 
            this.BodyName.FillWeight = 75F;
            this.BodyName.HeaderText = "Body Name";
            this.BodyName.MinimumWidth = 50;
            this.BodyName.Name = "BodyName";
            // 
            // BodyType
            // 
            this.BodyType.HeaderText = "Body Type";
            this.BodyType.Name = "BodyType";
            // 
            // EDSM
            // 
            this.EDSM.FillWeight = 50F;
            this.EDSM.HeaderText = "Source";
            this.EDSM.Name = "EDSM";
            // 
            // Mapped
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = null;
            this.Mapped.DefaultCellStyle = dataGridViewCellStyle1;
            this.Mapped.FillWeight = 30F;
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
            this.WasMapped.FillWeight = 30F;
            this.WasMapped.HeaderText = "Already Mapped";
            this.WasMapped.Name = "WasMapped";
            this.WasMapped.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // WasDiscovered
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.WasDiscovered.DefaultCellStyle = dataGridViewCellStyle3;
            this.WasDiscovered.FillWeight = 30F;
            this.WasDiscovered.HeaderText = "Already Discovered";
            this.WasDiscovered.Name = "WasDiscovered";
            this.WasDiscovered.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // EstBase
            // 
            this.EstBase.FillWeight = 50F;
            this.EstBase.HeaderText = "Base Value";
            this.EstBase.Name = "EstBase";
            // 
            // MappedValue
            // 
            this.MappedValue.FillWeight = 50F;
            this.MappedValue.HeaderText = "Mapped";
            this.MappedValue.Name = "MappedValue";
            // 
            // FirstMappedEff
            // 
            this.FirstMappedEff.FillWeight = 50F;
            this.FirstMappedEff.HeaderText = "First Mapped";
            this.FirstMappedEff.Name = "FirstMappedEff";
            // 
            // FirstDiscMapped
            // 
            this.FirstDiscMapped.FillWeight = 50F;
            this.FirstDiscMapped.HeaderText = "First Discovered Mapped";
            this.FirstDiscMapped.Name = "FirstDiscMapped";
            // 
            // EstValue
            // 
            this.EstValue.FillWeight = 50F;
            this.EstValue.HeaderText = "Current Value";
            this.EstValue.MinimumWidth = 50;
            this.EstValue.Name = "EstValue";
            // 
            // UserControlEstimatedValues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.extPanelRollUp);
            this.Name = "UserControlEstimatedValues";
            this.Size = new System.Drawing.Size(1247, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEstimatedValues)).EndInit();
            this.flowLayoutPanelTop.ResumeLayout(false);
            this.flowLayoutPanelTop.PerformLayout();
            this.extPanelRollUp.ResumeLayout(false);
            this.extPanelRollUp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustomDGV;
        private BaseUtils.DataGridViewColumnControl dataGridViewEstimatedValues;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox checkBoxShowZeros;
        private ExtendedControls.ExtCheckBox extCheckBoxShowImpossible;
        private System.Windows.Forms.Label labelControlText;
        private EDSMSpanshButton edsmSpanshButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn EDSM;
        private System.Windows.Forms.DataGridViewImageColumn Mapped;
        private System.Windows.Forms.DataGridViewImageColumn WasMapped;
        private System.Windows.Forms.DataGridViewImageColumn WasDiscovered;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn MappedValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstMappedEff;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstDiscMapped;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstValue;
    }
}
