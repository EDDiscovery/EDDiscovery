namespace EDDiscovery2
{ 
    partial class InfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(736, 559);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(86, 12);
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.Size = new System.Drawing.Size(650, 500);
            this.textBoxInfo.TabIndex = 2;
            this.textBoxInfo.Text = "";
            // 
            // InfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 594);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InfoForm";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.InfoForm_Layout);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RichTextBox textBoxInfo;
    }
}