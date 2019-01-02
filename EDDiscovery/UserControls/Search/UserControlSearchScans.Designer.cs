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
    partial class UserControlSearchScans     
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
            this.dataViewScrollerPanel = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom = new ExtendedControls.VScrollBarCustom();
            this.dataGridView = new EDDiscovery.UserControls.Search.DataGridViewStarResults();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.conditionFilterUC = new ExtendedConditionsForms.ConditionFilterUC();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.comboBoxSearches = new ExtendedControls.ComboBoxCustom();
            this.buttonDelete = new ExtendedControls.ButtonExt();
            this.buttonSave = new ExtendedControls.ButtonExt();
            this.buttonFind = new ExtendedControls.ButtonExt();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 338);
            this.dataViewScrollerPanel.TabIndex = 7;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = false;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(784, 34);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 304);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 7;
            this.vScrollBarCustom.Text = "vScrollBarCustom1";
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.CheckEDSM = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnStar,
            this.ColumnInformation,
            this.ColumnCurrentDistance,
            this.ColumnPosition});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.Size = new System.Drawing.Size(784, 338);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // ColumnDate
            // 
            this.ColumnDate.FillWeight = 60F;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.MinimumWidth = 50;
            this.ColumnDate.Name = "ColumnDate";
            // 
            // ColumnStar
            // 
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.MinimumWidth = 50;
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 200F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.MinimumWidth = 50;
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // ColumnCurrentDistance
            // 
            this.ColumnCurrentDistance.FillWeight = 40F;
            this.ColumnCurrentDistance.HeaderText = "Current Distance";
            this.ColumnCurrentDistance.MinimumWidth = 50;
            this.ColumnCurrentDistance.Name = "ColumnCurrentDistance";
            this.ColumnCurrentDistance.ReadOnly = true;
            // 
            // ColumnPosition
            // 
            this.ColumnPosition.FillWeight = 75F;
            this.ColumnPosition.HeaderText = "Position";
            this.ColumnPosition.MinimumWidth = 50;
            this.ColumnPosition.Name = "ColumnPosition";
            this.ColumnPosition.ReadOnly = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 33);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.conditionFilterUC);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataViewScrollerPanel);
            this.splitContainer.Size = new System.Drawing.Size(804, 683);
            this.splitContainer.SplitterDistance = 341;
            this.splitContainer.TabIndex = 8;
            // 
            // conditionFilterUC
            // 
            this.conditionFilterUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionFilterUC.Location = new System.Drawing.Point(0, 0);
            this.conditionFilterUC.Name = "conditionFilterUC";
            this.conditionFilterUC.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.conditionFilterUC.Size = new System.Drawing.Size(804, 341);
            this.conditionFilterUC.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonExtExcel);
            this.panel1.Controls.Add(this.comboBoxSearches);
            this.panel1.Controls.Add(this.buttonDelete);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonFind);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 33);
            this.panel1.TabIndex = 1;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.TravelGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(538, 3);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 37;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // comboBoxSearches
            // 
            this.comboBoxSearches.ArrowWidth = 1;
            this.comboBoxSearches.BorderColor = System.Drawing.Color.White;
            this.comboBoxSearches.ButtonColorScaling = 0.5F;
            this.comboBoxSearches.DataSource = null;
            this.comboBoxSearches.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSearches.DisplayMember = "";
            this.comboBoxSearches.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSearches.DropDownHeight = 400;
            this.comboBoxSearches.DropDownWidth = 400;
            this.comboBoxSearches.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSearches.ItemHeight = 13;
            this.comboBoxSearches.Location = new System.Drawing.Point(109, 3);
            this.comboBoxSearches.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSearches.Name = "comboBoxSearches";
            this.comboBoxSearches.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSearches.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSearches.ScrollBarWidth = 16;
            this.comboBoxSearches.SelectedIndex = -1;
            this.comboBoxSearches.SelectedItem = null;
            this.comboBoxSearches.SelectedValue = null;
            this.comboBoxSearches.Size = new System.Drawing.Size(199, 21);
            this.comboBoxSearches.TabIndex = 1;
            this.comboBoxSearches.Text = "comboBoxCustom1";
            this.comboBoxSearches.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxSearches.ValueMember = "";
            // 
            // buttonDelete
            // 
            this.buttonDelete.AutoSize = true;
            this.buttonDelete.Location = new System.Drawing.Point(420, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 23);
            this.buttonDelete.TabIndex = 0;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = true;
            this.buttonSave.Location = new System.Drawing.Point(314, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonFind
            // 
            this.buttonFind.AutoSize = true;
            this.buttonFind.Location = new System.Drawing.Point(3, 3);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(100, 23);
            this.buttonFind.TabIndex = 0;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // UserControlSearchScans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panel1);
            this.Name = "UserControlSearchScans";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom;
        private EDDiscovery.UserControls.Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ExtendedConditionsForms.ConditionFilterUC conditionFilterUC;
        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.ButtonExt buttonFind;
        private ExtendedControls.ComboBoxCustom comboBoxSearches;
        private ExtendedControls.ButtonExt buttonSave;
        private ExtendedControls.ButtonExt buttonDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
        private ExtendedControls.ButtonExt buttonExtExcel;
    }
}
