namespace EDDiscovery2
{ 
    partial class TextBoxEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextBoxEntry));
            this.textBox = new ExtendedControls.TextBoxBorder();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.BorderColor = System.Drawing.Color.Transparent;
            this.textBox.BorderColorScaling = 0.5F;
            this.textBox.Location = new System.Drawing.Point(12, 23);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(238, 20);
            this.textBox.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(102, 61);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(193, 61);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // TextBoxEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 90);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TextBoxEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TextBoxEntry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.TextBoxBorder textBox;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}