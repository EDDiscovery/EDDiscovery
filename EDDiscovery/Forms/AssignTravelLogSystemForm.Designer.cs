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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignTravelLogSystemForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEDSMLink = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFindSystem = new ExtendedControls.ButtonExt();
            this.cbSystemLink = new ExtendedControls.ComboBoxCustom();
            this.btnCancel = new ExtendedControls.ButtonExt();
            this.btnOK = new ExtendedControls.ButtonExt();
            this.tbLogSystemName = new ExtendedControls.TextBoxBorder();
            this.tbVisitedDate = new ExtendedControls.TextBoxBorder();
            this.tbSysCoordZ = new ExtendedControls.TextBoxBorder();
            this.tbLogCoordX = new ExtendedControls.TextBoxBorder();
            this.tbSysCoordY = new ExtendedControls.TextBoxBorder();
            this.tbLogCoordY = new ExtendedControls.TextBoxBorder();
            this.tbSysCoordX = new ExtendedControls.TextBoxBorder();
            this.tbLogCoordZ = new ExtendedControls.TextBoxBorder();
            this.tbManualSystemName = new ExtendedControls.AutoCompleteTextBox();
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
            this.lblEDSMLink.Location = new System.Drawing.Point(186, 216);
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
            this.panel1.Size = new System.Drawing.Size(478, 282);
            this.panel1.TabIndex = 22;
            // 
            // btnFindSystem
            // 
            this.btnFindSystem.BorderColorScaling = 1.25F;
            this.btnFindSystem.ButtonColorScaling = 0.5F;
            this.btnFindSystem.ButtonDisabledScaling = 0.5F;
            this.btnFindSystem.Location = new System.Drawing.Point(370, 133);
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
            this.cbSystemLink.DisplayMember = "";
            this.cbSystemLink.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.cbSystemLink.DropDownHeight = 106;
            this.cbSystemLink.DropDownWidth = 216;
            this.cbSystemLink.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbSystemLink.ItemHeight = 13;
            this.cbSystemLink.Location = new System.Drawing.Point(189, 98);
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
            this.cbSystemLink.ValueMember = "";
            this.cbSystemLink.SelectedIndexChanged += new System.EventHandler(this.cbSystemLink_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderColorScaling = 1.25F;
            this.btnCancel.ButtonColorScaling = 0.5F;
            this.btnCancel.ButtonDisabledScaling = 0.5F;
            this.btnCancel.Location = new System.Drawing.Point(280, 246);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BorderColorScaling = 1.25F;
            this.btnOK.ButtonColorScaling = 0.5F;
            this.btnOK.ButtonDisabledScaling = 0.5F;
            this.btnOK.Location = new System.Drawing.Point(370, 246);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbLogSystemName
            // 
            this.tbLogSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogSystemName.BorderColorScaling = 0.5F;
            this.tbLogSystemName.Location = new System.Drawing.Point(189, 14);
            this.tbLogSystemName.Name = "tbLogSystemName";
            this.tbLogSystemName.ReadOnly = true;
            this.tbLogSystemName.Size = new System.Drawing.Size(256, 20);
            this.tbLogSystemName.TabIndex = 7;
            // 
            // tbVisitedDate
            // 
            this.tbVisitedDate.BorderColor = System.Drawing.Color.Transparent;
            this.tbVisitedDate.BorderColorScaling = 0.5F;
            this.tbVisitedDate.Location = new System.Drawing.Point(189, 40);
            this.tbVisitedDate.Name = "tbVisitedDate";
            this.tbVisitedDate.ReadOnly = true;
            this.tbVisitedDate.Size = new System.Drawing.Size(256, 20);
            this.tbVisitedDate.TabIndex = 8;
            // 
            // tbSysCoordZ
            // 
            this.tbSysCoordZ.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordZ.BorderColorScaling = 0.5F;
            this.tbSysCoordZ.Location = new System.Drawing.Point(370, 175);
            this.tbSysCoordZ.Name = "tbSysCoordZ";
            this.tbSysCoordZ.ReadOnly = true;
            this.tbSysCoordZ.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordZ.TabIndex = 15;
            this.tbSysCoordZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbLogCoordX
            // 
            this.tbLogCoordX.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordX.BorderColorScaling = 0.5F;
            this.tbLogCoordX.Location = new System.Drawing.Point(189, 66);
            this.tbLogCoordX.Name = "tbLogCoordX";
            this.tbLogCoordX.ReadOnly = true;
            this.tbLogCoordX.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordX.TabIndex = 9;
            this.tbLogCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbSysCoordY
            // 
            this.tbSysCoordY.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordY.BorderColorScaling = 0.5F;
            this.tbSysCoordY.Location = new System.Drawing.Point(280, 175);
            this.tbSysCoordY.Name = "tbSysCoordY";
            this.tbSysCoordY.ReadOnly = true;
            this.tbSysCoordY.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordY.TabIndex = 14;
            this.tbSysCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbLogCoordY
            // 
            this.tbLogCoordY.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordY.BorderColorScaling = 0.5F;
            this.tbLogCoordY.Location = new System.Drawing.Point(280, 66);
            this.tbLogCoordY.Name = "tbLogCoordY";
            this.tbLogCoordY.ReadOnly = true;
            this.tbLogCoordY.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordY.TabIndex = 10;
            this.tbLogCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbSysCoordX
            // 
            this.tbSysCoordX.BorderColor = System.Drawing.Color.Transparent;
            this.tbSysCoordX.BorderColorScaling = 0.5F;
            this.tbSysCoordX.Location = new System.Drawing.Point(189, 175);
            this.tbSysCoordX.Name = "tbSysCoordX";
            this.tbSysCoordX.ReadOnly = true;
            this.tbSysCoordX.Size = new System.Drawing.Size(75, 20);
            this.tbSysCoordX.TabIndex = 13;
            this.tbSysCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbLogCoordZ
            // 
            this.tbLogCoordZ.BorderColor = System.Drawing.Color.Transparent;
            this.tbLogCoordZ.BorderColorScaling = 0.5F;
            this.tbLogCoordZ.Location = new System.Drawing.Point(370, 66);
            this.tbLogCoordZ.Name = "tbLogCoordZ";
            this.tbLogCoordZ.ReadOnly = true;
            this.tbLogCoordZ.Size = new System.Drawing.Size(75, 20);
            this.tbLogCoordZ.TabIndex = 11;
            this.tbLogCoordZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbManualSystemName
            // 
            this.tbManualSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.tbManualSystemName.BorderColorScaling = 0.5F;
            this.tbManualSystemName.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tbManualSystemName.DropDownBorderColor = System.Drawing.Color.Green;
            this.tbManualSystemName.DropDownHeight = 200;
            this.tbManualSystemName.DropDownItemHeight = 20;
            this.tbManualSystemName.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tbManualSystemName.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tbManualSystemName.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tbManualSystemName.DropDownWidth = 0;
            this.tbManualSystemName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tbManualSystemName.Location = new System.Drawing.Point(189, 135);
            this.tbManualSystemName.Name = "tbManualSystemName";
            this.tbManualSystemName.Size = new System.Drawing.Size(166, 20);
            this.tbManualSystemName.TabIndex = 12;
            // 
            // AssignTravelLogSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 282);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AssignTravelLogSystemForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AssignTravelLogSystemForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.ComboBoxCustom cbSystemLink;
        private System.Windows.Forms.Label label4;
        private ExtendedControls.TextBoxBorder tbLogSystemName;
        private ExtendedControls.TextBoxBorder tbVisitedDate;
        private ExtendedControls.TextBoxBorder tbLogCoordX;
        private ExtendedControls.TextBoxBorder tbLogCoordY;
        private ExtendedControls.TextBoxBorder tbLogCoordZ;
        private ExtendedControls.AutoCompleteTextBox tbManualSystemName;
        private ExtendedControls.TextBoxBorder tbSysCoordX;
        private ExtendedControls.TextBoxBorder tbSysCoordY;
        private ExtendedControls.TextBoxBorder tbSysCoordZ;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.ButtonExt btnOK;
        private ExtendedControls.ButtonExt btnCancel;
        private ExtendedControls.ButtonExt btnFindSystem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblEDSMLink;
        private System.Windows.Forms.Panel panel1;
    }
}