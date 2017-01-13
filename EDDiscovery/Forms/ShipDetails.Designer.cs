namespace EDDiscovery.Forms
{
    partial class ShipDetails
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
            this.currentCargo = new System.Windows.Forms.TextBox();
            this.tankSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.unladenMass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fsdOptimalMass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fsdLinearConstant = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.fsdPowerConstant = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fsdDrive = new System.Windows.Forms.ComboBox();
            this.btnUpdateFSD = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.fsdMaxFuelPerJump = new System.Windows.Forms.TextBox();
            this.fsdJumpRange = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnCancel = new ExtendedControls.ButtonExt();
            this.btnOK = new ExtendedControls.ButtonExt();
            this.TankWarning = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Cargo";
            // 
            // currentCargo
            // 
            this.currentCargo.Location = new System.Drawing.Point(171, 28);
            this.currentCargo.Name = "currentCargo";
            this.currentCargo.Size = new System.Drawing.Size(121, 20);
            this.currentCargo.TabIndex = 3;
            // 
            // tankSize
            // 
            this.tankSize.Location = new System.Drawing.Point(171, 54);
            this.tankSize.Name = "tankSize";
            this.tankSize.Size = new System.Drawing.Size(121, 20);
            this.tankSize.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tank Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "FSD Drive";
            // 
            // unladenMass
            // 
            this.unladenMass.Location = new System.Drawing.Point(171, 106);
            this.unladenMass.Name = "unladenMass";
            this.unladenMass.Size = new System.Drawing.Size(121, 20);
            this.unladenMass.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Unladen Mass";
            // 
            // fsdOptimalMass
            // 
            this.fsdOptimalMass.Location = new System.Drawing.Point(171, 159);
            this.fsdOptimalMass.Name = "fsdOptimalMass";
            this.fsdOptimalMass.Size = new System.Drawing.Size(121, 20);
            this.fsdOptimalMass.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "FSD Optimal Mass ";
            // 
            // fsdLinearConstant
            // 
            this.fsdLinearConstant.Location = new System.Drawing.Point(171, 185);
            this.fsdLinearConstant.Name = "fsdLinearConstant";
            this.fsdLinearConstant.Size = new System.Drawing.Size(121, 20);
            this.fsdLinearConstant.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "FSD Linear constant";
            // 
            // fsdPowerConstant
            // 
            this.fsdPowerConstant.Location = new System.Drawing.Point(171, 211);
            this.fsdPowerConstant.Name = "fsdPowerConstant";
            this.fsdPowerConstant.Size = new System.Drawing.Size(121, 20);
            this.fsdPowerConstant.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "FSD Power Constant";
            // 
            // fsdDrive
            // 
            this.fsdDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fsdDrive.FormattingEnabled = true;
            this.fsdDrive.Location = new System.Drawing.Point(171, 132);
            this.fsdDrive.Name = "fsdDrive";
            this.fsdDrive.Size = new System.Drawing.Size(121, 21);
            this.fsdDrive.TabIndex = 16;
            // 
            // btnUpdateFSD
            // 
            this.btnUpdateFSD.Location = new System.Drawing.Point(298, 132);
            this.btnUpdateFSD.Name = "btnUpdateFSD";
            this.btnUpdateFSD.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateFSD.TabIndex = 17;
            this.btnUpdateFSD.Text = "Update";
            this.btnUpdateFSD.UseVisualStyleBackColor = true;
            this.btnUpdateFSD.Click += new System.EventHandler(this.btnUpdateFSD_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "FSD Max Fuel Per Jump";
            // 
            // fsdMaxFuelPerJump
            // 
            this.fsdMaxFuelPerJump.Location = new System.Drawing.Point(171, 237);
            this.fsdMaxFuelPerJump.Name = "fsdMaxFuelPerJump";
            this.fsdMaxFuelPerJump.Size = new System.Drawing.Size(121, 20);
            this.fsdMaxFuelPerJump.TabIndex = 19;
            // 
            // fsdJumpRange
            // 
            this.fsdJumpRange.Location = new System.Drawing.Point(171, 263);
            this.fsdJumpRange.Name = "fsdJumpRange";
            this.fsdJumpRange.Size = new System.Drawing.Size(121, 20);
            this.fsdJumpRange.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "FSD Jump Range";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(298, 261);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 22;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderColorScaling = 1.25F;
            this.btnCancel.ButtonColorScaling = 0.5F;
            this.btnCancel.ButtonDisabledScaling = 0.5F;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(214, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BorderColorScaling = 1.25F;
            this.btnOK.ButtonColorScaling = 0.5F;
            this.btnOK.ButtonDisabledScaling = 0.5F;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(298, 294);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TankWarning
            // 
            this.TankWarning.Location = new System.Drawing.Point(171, 80);
            this.TankWarning.Name = "TankWarning";
            this.TankWarning.Size = new System.Drawing.Size(121, 20);
            this.TankWarning.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Tank warning";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(207, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "This screen tries to ESTIMATE fuel values";
            // 
            // ShipDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 325);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.TankWarning);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.fsdJumpRange);
            this.Controls.Add(this.fsdMaxFuelPerJump);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnUpdateFSD);
            this.Controls.Add(this.fsdDrive);
            this.Controls.Add(this.fsdPowerConstant);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fsdLinearConstant);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.fsdOptimalMass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.unladenMass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tankSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.currentCargo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ShipDetails";
            this.Text = "Ship Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ButtonExt btnOK;
        private ExtendedControls.ButtonExt btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox currentCargo;
        private System.Windows.Forms.TextBox tankSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox unladenMass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fsdOptimalMass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox fsdLinearConstant;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox fsdPowerConstant;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox fsdDrive;
        private System.Windows.Forms.Button btnUpdateFSD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox fsdMaxFuelPerJump;
        private System.Windows.Forms.TextBox fsdJumpRange;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.TextBox TankWarning;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
    }
}