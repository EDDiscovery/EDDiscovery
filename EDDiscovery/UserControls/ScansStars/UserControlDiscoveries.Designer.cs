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
    partial class UserControlDiscoveries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDiscoveries));
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonSearches = new ExtendedControls.ExtButton();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTime = new System.Windows.Forms.Label();
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.dataGridView = new EDDiscovery.UserControls.Search.DataGridViewStarResults();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSearches = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(399, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export");
            this.buttonExtExcel.UseVisualStyleBackColor = false;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            // 
            // extButtonSearches
            // 
            this.extButtonSearches.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSearches.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSearches.Image = global::EDDiscovery.Icons.Controls.Find;
            this.extButtonSearches.Location = new System.Drawing.Point(359, 1);
            this.extButtonSearches.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonSearches.Name = "extButtonSearches";
            this.extButtonSearches.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearches.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonSearches, "Select discoveries to search for, taken from the search panel scan searches");
            this.extButtonSearches.UseVisualStyleBackColor = false;
            this.extButtonSearches.Click += new System.EventHandler(this.extButtonSearches_Click);
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoHeight = false;
            this.rollUpPanelTop.AutoHeightWidthDisable = false;
            this.rollUpPanelTop.AutoSize = true;
            this.rollUpPanelTop.AutoWidth = false;
            this.rollUpPanelTop.Controls.Add(this.panelControls);
            this.rollUpPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelTop.HiddenMarkerWidth = 400;
            this.rollUpPanelTop.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelTop.Name = "rollUpPanelTop";
            this.rollUpPanelTop.PinState = true;
            this.rollUpPanelTop.RolledUpHeight = 5;
            this.rollUpPanelTop.RollUpAnimationTime = 500;
            this.rollUpPanelTop.RollUpDelay = 1000;
            this.rollUpPanelTop.SecondHiddenMarkerWidth = 0;
            this.rollUpPanelTop.ShowHiddenMarker = true;
            this.rollUpPanelTop.Size = new System.Drawing.Size(748, 30);
            this.rollUpPanelTop.TabIndex = 4;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.labelTime);
            this.panelControls.Controls.Add(this.comboBoxTime);
            this.panelControls.Controls.Add(this.labelSearch);
            this.panelControls.Controls.Add(this.textBoxSearch);
            this.panelControls.Controls.Add(this.extButtonSearches);
            this.panelControls.Controls.Add(this.buttonExtExcel);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(748, 30);
            this.panelControls.TabIndex = 32;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(0, 1);
            this.labelTime.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTime.ButtonColorScaling = 0.5F;
            this.comboBoxTime.DataSource = null;
            this.comboBoxTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTime.DisplayMember = "";
            this.comboBoxTime.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTime.Location = new System.Drawing.Point(38, 1);
            this.comboBoxTime.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.comboBoxTime.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTime.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTime.SelectedIndex = -1;
            this.comboBoxTime.SelectedItem = null;
            this.comboBoxTime.SelectedValue = null;
            this.comboBoxTime.Size = new System.Drawing.Size(100, 21);
            this.comboBoxTime.TabIndex = 0;
            this.comboBoxTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxTime.ValueMember = "";
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.CheckEDSM = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnStar,
            this.ColumnPosition,
            this.ColumnCurrentDistance,
            this.ColumnSearches,
            this.ColumnInformation,
            this.ColumnParent});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 30);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(748, 652);
            this.dataGridView.TabIndex = 5;
            // 
            // ColumnDate
            // 
            this.ColumnDate.FillWeight = 60F;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            // 
            // ColumnStar
            // 
            this.ColumnStar.FillWeight = 60F;
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnPosition
            // 
            this.ColumnPosition.FillWeight = 60F;
            this.ColumnPosition.HeaderText = "Position";
            this.ColumnPosition.Name = "ColumnPosition";
            this.ColumnPosition.ReadOnly = true;
            // 
            // ColumnCurrentDistance
            // 
            this.ColumnCurrentDistance.FillWeight = 60F;
            this.ColumnCurrentDistance.HeaderText = "Current Distance";
            this.ColumnCurrentDistance.Name = "ColumnCurrentDistance";
            this.ColumnCurrentDistance.ReadOnly = true;
            // 
            // ColumnSearches
            // 
            this.ColumnSearches.HeaderText = "Searches";
            this.ColumnSearches.Name = "ColumnSearches";
            this.ColumnSearches.ReadOnly = true;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 200F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // ColumnParent
            // 
            this.ColumnParent.FillWeight = 150F;
            this.ColumnParent.HeaderText = "Parent";
            this.ColumnParent.Name = "ColumnParent";
            this.ColumnParent.ReadOnly = true;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(146, 1);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 32;
            this.labelSearch.Text = "Search";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderColorScaling = 0.5F;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.ClearOnFirstChar = false;
            this.textBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSearch.EndButtonEnable = true;
            this.textBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxSearch.EndButtonImage")));
            this.textBoxSearch.EndButtonVisible = false;
            this.textBoxSearch.InErrorCondition = false;
            this.textBoxSearch.Location = new System.Drawing.Point(195, 1);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.textBoxSearch.Multiline = false;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.ReadOnly = false;
            this.textBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSearch.SelectionLength = 0;
            this.textBoxSearch.SelectionStart = 0;
            this.textBoxSearch.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearch.TabIndex = 31;
            this.textBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxSearch, resources.GetString("textBoxSearch.ToolTip"));
            this.textBoxSearch.WordWrap = true;
            // 
            // UserControlDiscoveries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlDiscoveries";
            this.Size = new System.Drawing.Size(748, 682);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private System.Windows.Forms.Label labelTime;
        internal ExtendedControls.ExtComboBox comboBoxTime;
        private ExtendedControls.ExtButton extButtonSearches;
        private Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSearches;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParent;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtTextBox textBoxSearch;
    }
}
