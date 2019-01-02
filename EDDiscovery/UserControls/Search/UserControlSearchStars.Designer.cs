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
    partial class UserControlSearchStars       
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
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCentreDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.findSystemsUserControl = new EDDiscovery.UserControls.FindSystemsUserControl();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 124);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 592);
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
            this.vScrollBarCustom.Location = new System.Drawing.Point(784, 21);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 571);
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
            this.ColumnStar,
            this.ColumnCentreDistance,
            this.ColumnCurrentDistance,
            this.ColumnPosition});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.Size = new System.Drawing.Size(784, 592);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // ColumnStar
            // 
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.MinimumWidth = 50;
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnCentreDistance
            // 
            this.ColumnCentreDistance.FillWeight = 40F;
            this.ColumnCentreDistance.HeaderText = "Centre Distance";
            this.ColumnCentreDistance.MinimumWidth = 50;
            this.ColumnCentreDistance.Name = "ColumnCentreDistance";
            this.ColumnCentreDistance.ReadOnly = true;
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
            // findSystemsUserControl
            // 
            this.findSystemsUserControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.findSystemsUserControl.Location = new System.Drawing.Point(0, 0);
            this.findSystemsUserControl.Name = "findSystemsUserControl";
            this.findSystemsUserControl.Size = new System.Drawing.Size(804, 124);
            this.findSystemsUserControl.TabIndex = 32;
            // 
            // UserControlSearchStars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.findSystemsUserControl);
            this.Name = "UserControlSearchStars";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom;
        private EDDiscovery.UserControls.Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private FindSystemsUserControl findSystemsUserControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCentreDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
    }
}
