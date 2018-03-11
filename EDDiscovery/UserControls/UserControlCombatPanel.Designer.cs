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
    partial class UserControlCombatPanel
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.comboBoxCustomCampaign = new ExtendedControls.ComboBoxCustom();
            this.buttonExtEditCampaign = new ExtendedControls.ButtonExt();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reward = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelKills = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // comboBoxCustomCampaign
            // 
            this.comboBoxCustomCampaign.ArrowWidth = 1;
            this.comboBoxCustomCampaign.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomCampaign.ButtonColorScaling = 0.5F;
            this.comboBoxCustomCampaign.DataSource = null;
            this.comboBoxCustomCampaign.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomCampaign.DisplayMember = "";
            this.comboBoxCustomCampaign.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomCampaign.DropDownHeight = 300;
            this.comboBoxCustomCampaign.DropDownWidth = 400;
            this.comboBoxCustomCampaign.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomCampaign.ItemHeight = 13;
            this.comboBoxCustomCampaign.Location = new System.Drawing.Point(4, 4);
            this.comboBoxCustomCampaign.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomCampaign.Name = "comboBoxCustomCampaign";
            this.comboBoxCustomCampaign.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCampaign.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCampaign.ScrollBarWidth = 16;
            this.comboBoxCustomCampaign.SelectedIndex = -1;
            this.comboBoxCustomCampaign.SelectedItem = null;
            this.comboBoxCustomCampaign.SelectedValue = null;
            this.comboBoxCustomCampaign.Size = new System.Drawing.Size(180, 21);
            this.comboBoxCustomCampaign.TabIndex = 1;
            this.comboBoxCustomCampaign.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomCampaign.ValueMember = "";
            this.comboBoxCustomCampaign.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomCampaign_SelectedIndexChanged);
            // 
            // buttonExtEditCampaign
            // 
            this.buttonExtEditCampaign.Location = new System.Drawing.Point(190, 3);
            this.buttonExtEditCampaign.Name = "buttonExtEditCampaign";
            this.buttonExtEditCampaign.Size = new System.Drawing.Size(50, 24);
            this.buttonExtEditCampaign.TabIndex = 2;
            this.buttonExtEditCampaign.Text = "Edit";
            this.buttonExtEditCampaign.UseVisualStyleBackColor = true;
            this.buttonExtEditCampaign.Click += new System.EventHandler(this.buttonExtEditCampaign_Click);
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridView1);
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 96);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(394, 100);
            this.dataViewScrollerPanel1.TabIndex = 3;
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
            this.vScrollBarCustom1.LargeChange = 1;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(374, 21);
            this.vScrollBarCustom1.Maximum = 0;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 79);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 1;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = 0;
            this.vScrollBarCustom1.ValueLimited = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Event,
            this.Reward});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(374, 100);
            this.dataGridView1.TabIndex = 0;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            // 
            // Event
            // 
            this.Event.HeaderText = "Event";
            this.Event.Name = "Event";
            // 
            // Reward
            // 
            this.Reward.HeaderText = "Reward";
            this.Reward.Name = "Reward";
            // 
            // labelKills
            // 
            this.labelKills.AutoSize = true;
            this.labelKills.Location = new System.Drawing.Point(4, 34);
            this.labelKills.Name = "labelKills";
            this.labelKills.Size = new System.Drawing.Size(25, 13);
            this.labelKills.TabIndex = 4;
            this.labelKills.Text = "Kills";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(68, 34);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(34, 13);
            this.labelValue.TabIndex = 4;
            this.labelValue.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(132, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Crimes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Factions";
            // 
            // UserControlCombatPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelValue);
            this.Controls.Add(this.labelKills);
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.buttonExtEditCampaign);
            this.Controls.Add(this.comboBoxCustomCampaign);
            this.Name = "UserControlCombatPanel";
            this.Size = new System.Drawing.Size(496, 224);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ExtendedControls.ComboBoxCustom comboBoxCustomCampaign;
        private ExtendedControls.ButtonExt buttonExtEditCampaign;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reward;
        private System.Windows.Forms.Label labelKills;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
