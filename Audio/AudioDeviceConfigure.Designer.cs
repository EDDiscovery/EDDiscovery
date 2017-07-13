namespace AudioExtensions
{ 
    partial class AudioDeviceConfigure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioDeviceConfigure));
            this.comboBoxCustomDevice = new ExtendedControls.ComboBoxCustom();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxCustomDevice
            // 
            this.comboBoxCustomDevice.ArrowWidth = 1;
            this.comboBoxCustomDevice.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomDevice.ButtonColorScaling = 0.5F;
            this.comboBoxCustomDevice.DataSource = null;
            this.comboBoxCustomDevice.DisplayMember = "";
            this.comboBoxCustomDevice.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomDevice.DropDownHeight = 106;
            this.comboBoxCustomDevice.DropDownWidth = 248;
            this.comboBoxCustomDevice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomDevice.ItemHeight = 13;
            this.comboBoxCustomDevice.Location = new System.Drawing.Point(14, 55);
            this.comboBoxCustomDevice.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomDevice.Name = "comboBoxCustomDevice";
            this.comboBoxCustomDevice.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomDevice.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomDevice.ScrollBarWidth = 16;
            this.comboBoxCustomDevice.SelectedIndex = -1;
            this.comboBoxCustomDevice.SelectedItem = null;
            this.comboBoxCustomDevice.SelectedValue = null;
            this.comboBoxCustomDevice.Size = new System.Drawing.Size(428, 23);
            this.comboBoxCustomDevice.TabIndex = 0;
            this.comboBoxCustomDevice.Text = "comboBoxCustom1";
            this.comboBoxCustomDevice.ValueMember = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Device";
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(367, 105);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
            this.buttonExtOK.TabIndex = 2;
            this.buttonExtOK.Text = "OK";
            this.buttonExtOK.UseVisualStyleBackColor = true;
            this.buttonExtOK.Click += new System.EventHandler(this.buttonExtOK_Click);
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExtCancel.Location = new System.Drawing.Point(273, 105);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 3;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonExtOK);
            this.panel1.Controls.Add(this.buttonExtCancel);
            this.panel1.Controls.Add(this.comboBoxCustomDevice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 150);
            this.panel1.TabIndex = 4;
            // 
            // AudioDeviceConfigure
            // 
            this.AcceptButton = this.buttonExtOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(455, 150);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AudioDeviceConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio Device Select";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ComboBoxCustom comboBoxCustomDevice;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ButtonExt buttonExtOK;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private System.Windows.Forms.Panel panel1;
    }
}