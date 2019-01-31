/*
 * Copyright © 2016 EDDiscovery development team
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
namespace EDDiscovery.Forms
{
    partial class AssignTravelLogSystemForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEDSMLink = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFindSystem = new ExtendedControls.ExtButton();
            this.cbSystemLink = new ExtendedControls.ExtComboBox();
            this.btnCancel = new ExtendedControls.ExtButton();
            this.btnOK = new ExtendedControls.ExtButton();
            this.tbLogSystemName = new ExtendedControls.ExtTextBox();
            this.tbVisitedDate = new ExtendedControls.ExtTextBox();
            this.tbSysCoordZ = new ExtendedControls.ExtTextBox();
            this.tbLogCoordX = new ExtendedControls.ExtTextBox();
            this.tbSysCoordY = new ExtendedControls.ExtTextBox();
            this.tbLogCoordY = new ExtendedControls.ExtTextBox();
            this.tbSysCoordX = new ExtendedControls.ExtTextBox();
            this.tbLogCoordZ = new ExtendedControls.ExtTextBox();
            this.tbManualSystemName = new ExtendedControls.ExtTextBoxAutoComplete();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Travel Log System Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date Visited:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New system name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Travel Log Coordinates:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "System Coordinates";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "EDSM Link:";
            // 
            // lblEDSMLink
            // 
            this.lblEDSMLink.AutoSize = true;
            this.lblEDSMLink.Location = new System.Drawing.Point(249, 216);
            this.lblEDSMLink.Name = "lblEDSMLink";
            this.lblEDSMLink.Size = new System.Drawing.Size(58, 13);
            this.lblEDSMLink.TabIndex = 21;
            this.lblEDSMLink.TabStop = true;
            this.lblEDSMLink.Text = "EDSMLink";
            this.lblEDSMLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEDSMLink_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblEDSMLink);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnFindSystem);
            this.panel1.Controls.Add(this.cbSystemLink);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.tbLogSystemName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbVisitedDate);
            this.panel1.Controls.Add(this.tbSysCoordZ);
            this.panel1.Controls.Add(this.tbLogCoordX);
            this.panel1.Controls.Add(this.tbSysCoordY);
            this.panel1.Controls.Add(this.tbLogCoordY);
            this.panel1.Controls.Add(this.tbSysCoordX);
            this.panel1.Controls.Add(this.tbLogCoordZ);
            this.panel1.Controls.Add(this.tbManualSystemName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 287);
            this.panel1.TabIndex = 22;
            // 
            // btnFindSystem
            // 
            this.btnFindSystem.Location = new System.Drawing.Point(433, 133);
            this.btnFindSystem.Name = "btnFindSystem";
            this.btnFindSystem.Size = new System.Drawing.Size(75, 23);
            this.btnFindSystem.TabIndex = 19;
            this.btnFindSystem.Text = "Find";
            this.btnFindSystem.UseVisualStyleBackColor = true;
            this.btnFindSystem.Click += new System.EventHandler(this.btnFindSystem_Click);
            // 
            // cbSystemLink
            // 
            this.cbSystemLink.ArrowWidth = 1;
            this.cbSystemLink.BorderColor = System.Drawing.Color.White;
            this.cbSystemLink.ButtonColorScaling = 0.5F;
            this.cbSystemLink.DataSource = null;
            this.cbSystemLink.DisableBackgroundDisabledShadingGradient = false;
            this.cbSystemLink.DisplayMember = "";
            this.cbSystemLink.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.cbSystemLink.DropDownHeight = 106;
            this.cbSystemLink.DropDownWidth = 216;
            this.cbSystemLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbSystemLink.ItemHeight = 13;
            this.cbSystemLink.Location = new System.Drawing.Point(252, 98);
            this.cbSystemLink.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.cbSystemLink.Name = "cbSystemLink";
            this.cbSystemLink.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.cbSystemLink.ScrollBarColor = System.Drawing.Color.LightGray;
            this.cbSystemLink.ScrollBarWidth = 16;
            this.cbSystemLink.SelectedIndex = -1;
            this.cbSystemLink.SelectedItem = null;
            this.cbSystemLink.SelectedValue = null;
            this.cbSystemLink.Size = new System.Drawing.Size(256, 21);
            this.cbSystemLink.TabIndex = 5;
            this.cbSystemLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbSystemLink.ValueMember = "";
            this.cbSystemLink.SelectedIndexChanged += new System.EventHandler(this.cbSystemLink_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(343, 246);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(433, 246);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbLogSystemName
            // 
            this.tbLogSystemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbLogSystemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbLogSystemName.BackErrorColor = System.Drawing.Color.Red;
            this.tbLogSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogSystemName.BorderColorScaling = 0.5F;
            this.tbLogSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLogSystemName.ClearOnFirstChar = false;
            this.tbLogSystemName.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbLogSystemName.InErrorCondition = false;
            this.tbLogSystemName.Location = new System.Drawing.Point(252, 14);
            this.tbLogSystemName.Multiline = false;
            this.tbLogSystemName.Name = "tbLogSystemName";
            this.tbLogSystemName.ReadOnly = true;
            this.tbLogSystemName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbLogSystemName.SelectionLength = 0;
            this.tbLogSystemName.SelectionStart = 0;
            this.tbLogSystemName.Size = new System.Drawing.Size(256, 20);
            this.tbLogSystemName.TabIndex = 7;
            this.tbLogSystemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbLogSystemName.WordWrap = true;
            // 
            // tbVisitedDate
            // 
            this.tbVisitedDate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbVisitedDate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbVisitedDate.BackErrorColor = System.Drawing.Color.Red;
            this.tbVisitedDate.BorderColor = System.Drawing.Color.Transparent;
            this.tbVisitedDate.BorderColorScaling = 0.5F;
            this.tbVisitedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbVisitedDate.ClearOnFirstChar = false;
            this.tbVisitedDate.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbVisitedDate.InErrorCondition = false;
            this.tbVisitedDate.Location = new System.Drawing.Point(252, 40);
            this.tbVisitedDate.Multiline = false;
            this.tbVisitedDate.Name = "tbVisitedDate";
            this.tbVisitedDate.ReadOnly = true;
            this.tbVisitedDate.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbVisitedDate.SelectionLength = 0;
            this.tbVisitedDate.SelectionStart = 0;
            this.tbVisitedDate.Size = new System.Drawing.Size(256, 20);
            this.tbVisitedDate.TabIndex = 8;
            this.tbVisitedDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbVisitedDate.WordWrap = true;
            // 
            // tbSysCoordZ
            // 
            this.tbSysCoordZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbSysCoordZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbSysCoordZ.BackErrorColor = System.Drawing.Color.Red;
            this.tbSysCoordZ.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordZ.BorderColorScaling = 0.5F;
            this.tbSysCoordZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSysCoordZ.ClearOnFirstChar = false;
            this.tbSysCoordZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbSysCoordZ.InErrorCondition = false;
            this.tbSysCoordZ.Location = new System.Drawing.Point(433, 175);
            this.tbSysCoordZ.Multiline = false;
            this.tbSysCoordZ.Name = "tbSysCoordZ";
            this.tbSysCoordZ.ReadOnly = true;
            this.tbSysCoordZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSysCoordZ.SelectionLength = 0;
            this.tbSysCoordZ.SelectionStart = 0;
            this.tbSysCoordZ.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordZ.TabIndex = 15;
            this.tbSysCoordZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSysCoordZ.WordWrap = true;
            // 
            // tbLogCoordX
            // 
            this.tbLogCoordX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbLogCoordX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbLogCoordX.BackErrorColor = System.Drawing.Color.Red;
            this.tbLogCoordX.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordX.BorderColorScaling = 0.5F;
            this.tbLogCoordX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLogCoordX.ClearOnFirstChar = false;
            this.tbLogCoordX.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbLogCoordX.InErrorCondition = false;
            this.tbLogCoordX.Location = new System.Drawing.Point(252, 66);
            this.tbLogCoordX.Multiline = false;
            this.tbLogCoordX.Name = "tbLogCoordX";
            this.tbLogCoordX.ReadOnly = true;
            this.tbLogCoordX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbLogCoordX.SelectionLength = 0;
            this.tbLogCoordX.SelectionStart = 0;
            this.tbLogCoordX.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordX.TabIndex = 9;
            this.tbLogCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbLogCoordX.WordWrap = true;
            // 
            // tbSysCoordY
            // 
            this.tbSysCoordY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbSysCoordY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbSysCoordY.BackErrorColor = System.Drawing.Color.Red;
            this.tbSysCoordY.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordY.BorderColorScaling = 0.5F;
            this.tbSysCoordY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSysCoordY.ClearOnFirstChar = false;
            this.tbSysCoordY.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbSysCoordY.InErrorCondition = false;
            this.tbSysCoordY.Location = new System.Drawing.Point(343, 175);
            this.tbSysCoordY.Multiline = false;
            this.tbSysCoordY.Name = "tbSysCoordY";
            this.tbSysCoordY.ReadOnly = true;
            this.tbSysCoordY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSysCoordY.SelectionLength = 0;
            this.tbSysCoordY.SelectionStart = 0;
            this.tbSysCoordY.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordY.TabIndex = 14;
            this.tbSysCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSysCoordY.WordWrap = true;
            // 
            // tbLogCoordY
            // 
            this.tbLogCoordY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbLogCoordY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbLogCoordY.BackErrorColor = System.Drawing.Color.Red;
            this.tbLogCoordY.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordY.BorderColorScaling = 0.5F;
            this.tbLogCoordY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLogCoordY.ClearOnFirstChar = false;
            this.tbLogCoordY.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbLogCoordY.InErrorCondition = false;
            this.tbLogCoordY.Location = new System.Drawing.Point(343, 66);
            this.tbLogCoordY.Multiline = false;
            this.tbLogCoordY.Name = "tbLogCoordY";
            this.tbLogCoordY.ReadOnly = true;
            this.tbLogCoordY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbLogCoordY.SelectionLength = 0;
            this.tbLogCoordY.SelectionStart = 0;
            this.tbLogCoordY.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordY.TabIndex = 10;
            this.tbLogCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbLogCoordY.WordWrap = true;
            // 
            // tbSysCoordX
            // 
            this.tbSysCoordX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbSysCoordX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbSysCoordX.BackErrorColor = System.Drawing.Color.Red;
            this.tbSysCoordX.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordX.BorderColorScaling = 0.5F;
            this.tbSysCoordX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSysCoordX.ClearOnFirstChar = false;
            this.tbSysCoordX.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbSysCoordX.InErrorCondition = false;
            this.tbSysCoordX.Location = new System.Drawing.Point(252, 175);
            this.tbSysCoordX.Multiline = false;
            this.tbSysCoordX.Name = "tbSysCoordX";
            this.tbSysCoordX.ReadOnly = true;
            this.tbSysCoordX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbSysCoordX.SelectionLength = 0;
            this.tbSysCoordX.SelectionStart = 0;
            this.tbSysCoordX.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordX.TabIndex = 13;
            this.tbSysCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSysCoordX.WordWrap = true;
            // 
            // tbLogCoordZ
            // 
            this.tbLogCoordZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbLogCoordZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbLogCoordZ.BackErrorColor = System.Drawing.Color.Red;
            this.tbLogCoordZ.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordZ.BorderColorScaling = 0.5F;
            this.tbLogCoordZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbLogCoordZ.ClearOnFirstChar = false;
            this.tbLogCoordZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbLogCoordZ.InErrorCondition = false;
            this.tbLogCoordZ.Location = new System.Drawing.Point(433, 66);
            this.tbLogCoordZ.Multiline = false;
            this.tbLogCoordZ.Name = "tbLogCoordZ";
            this.tbLogCoordZ.ReadOnly = true;
            this.tbLogCoordZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbLogCoordZ.SelectionLength = 0;
            this.tbLogCoordZ.SelectionStart = 0;
            this.tbLogCoordZ.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordZ.TabIndex = 11;
            this.tbLogCoordZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbLogCoordZ.WordWrap = true;
            // 
            // tbManualSystemName
            // 
            this.tbManualSystemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.tbManualSystemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.tbManualSystemName.BackErrorColor = System.Drawing.Color.Red;
            this.tbManualSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.tbManualSystemName.BorderColorScaling = 0.5F;
            this.tbManualSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbManualSystemName.ClearOnFirstChar = false;
            this.tbManualSystemName.ControlBackground = System.Drawing.SystemColors.Control;
            this.tbManualSystemName.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tbManualSystemName.DropDownBorderColor = System.Drawing.Color.Green;
            this.tbManualSystemName.DropDownHeight = 200;
            this.tbManualSystemName.DropDownItemHeight = 13;
            this.tbManualSystemName.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tbManualSystemName.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tbManualSystemName.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tbManualSystemName.DropDownWidth = 0;
            this.tbManualSystemName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tbManualSystemName.InErrorCondition = false;
            this.tbManualSystemName.Location = new System.Drawing.Point(252, 135);
            this.tbManualSystemName.Multiline = false;
            this.tbManualSystemName.Name = "tbManualSystemName";
            this.tbManualSystemName.ReadOnly = false;
            this.tbManualSystemName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbManualSystemName.SelectionLength = 0;
            this.tbManualSystemName.SelectionStart = 0;
            this.tbManualSystemName.Size = new System.Drawing.Size(166, 20);
            this.tbManualSystemName.TabIndex = 12;
            this.tbManualSystemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbManualSystemName.WordWrap = true;
            // 
            // AssignTravelLogSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 287);
            this.Controls.Add(this.panel1);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "AssignTravelLogSystemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assign System";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.ExtComboBox cbSystemLink;
        private System.Windows.Forms.Label label4;
        private ExtendedControls.ExtTextBox tbLogSystemName;
        private ExtendedControls.ExtTextBox tbVisitedDate;
        private ExtendedControls.ExtTextBox tbLogCoordX;
        private ExtendedControls.ExtTextBox tbLogCoordY;
        private ExtendedControls.ExtTextBox tbLogCoordZ;
        private ExtendedControls.ExtTextBoxAutoComplete tbManualSystemName;
        private ExtendedControls.ExtTextBox tbSysCoordX;
        private ExtendedControls.ExtTextBox tbSysCoordY;
        private ExtendedControls.ExtTextBox tbSysCoordZ;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.ExtButton btnOK;
        private ExtendedControls.ExtButton btnCancel;
        private ExtendedControls.ExtButton btnFindSystem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblEDSMLink;
        private System.Windows.Forms.Panel panel1;
    }
}