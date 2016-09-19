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
            this.cbSystemLink = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbLogSystemName = new System.Windows.Forms.TextBox();
            this.tbVisitedDate = new System.Windows.Forms.TextBox();
            this.tbLogCoordX = new System.Windows.Forms.TextBox();
            this.tbLogCoordY = new System.Windows.Forms.TextBox();
            this.tbLogCoordZ = new System.Windows.Forms.TextBox();
            this.tbManualSystemName = new ExtendedControls.AutoCompleteTextBox();
            this.tbSysCoordX = new System.Windows.Forms.TextBox();
            this.tbSysCoordY = new System.Windows.Forms.TextBox();
            this.tbSysCoordZ = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFindSystem = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEDSMLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Travel Log System Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date Visited:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New system name:";
            // 
            // cbSystemLink
            // 
            this.cbSystemLink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSystemLink.FormattingEnabled = true;
            this.cbSystemLink.Location = new System.Drawing.Point(151, 88);
            this.cbSystemLink.Name = "cbSystemLink";
            this.cbSystemLink.Size = new System.Drawing.Size(216, 21);
            this.cbSystemLink.TabIndex = 5;
            this.cbSystemLink.SelectedIndexChanged += new System.EventHandler(this.cbSystemLink_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Travel Log Coordinates:";
            // 
            // tbLogSystemName
            // 
            this.tbLogSystemName.Location = new System.Drawing.Point(151, 10);
            this.tbLogSystemName.Name = "tbLogSystemName";
            this.tbLogSystemName.ReadOnly = true;
            this.tbLogSystemName.Size = new System.Drawing.Size(216, 20);
            this.tbLogSystemName.TabIndex = 7;
            // 
            // tbVisitedDate
            // 
            this.tbVisitedDate.Location = new System.Drawing.Point(151, 36);
            this.tbVisitedDate.Name = "tbVisitedDate";
            this.tbVisitedDate.ReadOnly = true;
            this.tbVisitedDate.Size = new System.Drawing.Size(216, 20);
            this.tbVisitedDate.TabIndex = 8;
            // 
            // tbLogCoordX
            // 
            this.tbLogCoordX.Location = new System.Drawing.Point(151, 62);
            this.tbLogCoordX.Name = "tbLogCoordX";
            this.tbLogCoordX.ReadOnly = true;
            this.tbLogCoordX.Size = new System.Drawing.Size(68, 20);
            this.tbLogCoordX.TabIndex = 9;
            this.tbLogCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbLogCoordY
            // 
            this.tbLogCoordY.Location = new System.Drawing.Point(225, 62);
            this.tbLogCoordY.Name = "tbLogCoordY";
            this.tbLogCoordY.ReadOnly = true;
            this.tbLogCoordY.Size = new System.Drawing.Size(68, 20);
            this.tbLogCoordY.TabIndex = 10;
            this.tbLogCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbLogCoordZ
            // 
            this.tbLogCoordZ.Location = new System.Drawing.Point(299, 62);
            this.tbLogCoordZ.Name = "tbLogCoordZ";
            this.tbLogCoordZ.ReadOnly = true;
            this.tbLogCoordZ.Size = new System.Drawing.Size(68, 20);
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
            this.tbManualSystemName.Location = new System.Drawing.Point(151, 116);
            this.tbManualSystemName.Name = "tbManualSystemName";
            this.tbManualSystemName.Size = new System.Drawing.Size(135, 20);
            this.tbManualSystemName.TabIndex = 12;
            // 
            // tbSysCoordX
            // 
            this.tbSysCoordX.Location = new System.Drawing.Point(151, 142);
            this.tbSysCoordX.Name = "tbSysCoordX";
            this.tbSysCoordX.ReadOnly = true;
            this.tbSysCoordX.Size = new System.Drawing.Size(68, 20);
            this.tbSysCoordX.TabIndex = 13;
            this.tbSysCoordX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbSysCoordY
            // 
            this.tbSysCoordY.Location = new System.Drawing.Point(225, 142);
            this.tbSysCoordY.Name = "tbSysCoordY";
            this.tbSysCoordY.ReadOnly = true;
            this.tbSysCoordY.Size = new System.Drawing.Size(68, 20);
            this.tbSysCoordY.TabIndex = 14;
            this.tbSysCoordY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbSysCoordZ
            // 
            this.tbSysCoordZ.Location = new System.Drawing.Point(299, 142);
            this.tbSysCoordZ.Name = "tbSysCoordZ";
            this.tbSysCoordZ.ReadOnly = true;
            this.tbSysCoordZ.Size = new System.Drawing.Size(68, 20);
            this.tbSysCoordZ.TabIndex = 15;
            this.tbSysCoordZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "System Coordinates";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(292, 210);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(211, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFindSystem
            // 
            this.btnFindSystem.Location = new System.Drawing.Point(292, 115);
            this.btnFindSystem.Name = "btnFindSystem";
            this.btnFindSystem.Size = new System.Drawing.Size(75, 23);
            this.btnFindSystem.TabIndex = 19;
            this.btnFindSystem.Text = "Find";
            this.btnFindSystem.UseVisualStyleBackColor = true;
            this.btnFindSystem.Click += new System.EventHandler(this.btnFindSystem_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "EDSM Link:";
            // 
            // lblEDSMLink
            // 
            this.lblEDSMLink.AutoSize = true;
            this.lblEDSMLink.Location = new System.Drawing.Point(148, 178);
            this.lblEDSMLink.Name = "lblEDSMLink";
            this.lblEDSMLink.Size = new System.Drawing.Size(58, 13);
            this.lblEDSMLink.TabIndex = 21;
            this.lblEDSMLink.TabStop = true;
            this.lblEDSMLink.Text = "EDSMLink";
            this.lblEDSMLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEDSMLink_LinkClicked);
            // 
            // AssignTravelLogSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 245);
            this.Controls.Add(this.lblEDSMLink);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnFindSystem);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSysCoordZ);
            this.Controls.Add(this.tbSysCoordY);
            this.Controls.Add(this.tbSysCoordX);
            this.Controls.Add(this.tbManualSystemName);
            this.Controls.Add(this.tbLogCoordZ);
            this.Controls.Add(this.tbLogCoordY);
            this.Controls.Add(this.tbLogCoordX);
            this.Controls.Add(this.tbVisitedDate);
            this.Controls.Add(this.tbLogSystemName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSystemLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AssignTravelLogSystemForm";
            this.Text = "AssignTravelLogSystemForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSystemLink;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbLogSystemName;
        private System.Windows.Forms.TextBox tbVisitedDate;
        private System.Windows.Forms.TextBox tbLogCoordX;
        private System.Windows.Forms.TextBox tbLogCoordY;
        private System.Windows.Forms.TextBox tbLogCoordZ;
        private ExtendedControls.AutoCompleteTextBox tbManualSystemName;
        private System.Windows.Forms.TextBox tbSysCoordX;
        private System.Windows.Forms.TextBox tbSysCoordY;
        private System.Windows.Forms.TextBox tbSysCoordZ;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFindSystem;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lblEDSMLink;
    }
}