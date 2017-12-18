namespace ExtendedControls
{
    partial class MessageBoxTheme
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
            this.labelCaption = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExt3 = new ExtendedControls.ButtonExt();
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.buttonExt2 = new ExtendedControls.ButtonExt();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new System.Drawing.Point(10, 9);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(35, 13);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "label1";
            this.labelCaption.Visible = false;
            this.labelCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.labelCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelCaption);
            this.panel1.Controls.Add(this.buttonExt3);
            this.panel1.Controls.Add(this.buttonExt1);
            this.panel1.Controls.Add(this.buttonExt2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 361);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // buttonExt3
            // 
            this.buttonExt3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExt3.Location = new System.Drawing.Point(116, 326);
            this.buttonExt3.Name = "buttonExt3";
            this.buttonExt3.Size = new System.Drawing.Size(75, 24);
            this.buttonExt3.TabIndex = 4;
            this.buttonExt3.Text = "Retry";
            this.buttonExt3.UseVisualStyleBackColor = true;
            this.buttonExt3.Click += new System.EventHandler(this.buttonExt_Click);
            // 
            // buttonExt1
            // 
            this.buttonExt1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExt1.Location = new System.Drawing.Point(297, 326);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(75, 24);
            this.buttonExt1.TabIndex = 2;
            this.buttonExt1.Text = "OK";
            this.buttonExt1.UseVisualStyleBackColor = true;
            this.buttonExt1.Click += new System.EventHandler(this.buttonExt_Click);
            // 
            // buttonExt2
            // 
            this.buttonExt2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExt2.Location = new System.Drawing.Point(207, 326);
            this.buttonExt2.Name = "buttonExt2";
            this.buttonExt2.Size = new System.Drawing.Size(75, 24);
            this.buttonExt2.TabIndex = 3;
            this.buttonExt2.Text = "Cancel";
            this.buttonExt2.UseVisualStyleBackColor = true;
            this.buttonExt2.Click += new System.EventHandler(this.buttonExt_Click);
            // 
            // MessageBoxTheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.panel1);
            this.Name = "MessageBoxTheme";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageBox";
            this.Shown += new System.EventHandler(this.MessageBoxTheme_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelCaption;
        private ExtendedControls.ButtonExt buttonExt1;
        private ExtendedControls.ButtonExt buttonExt2;
        private ExtendedControls.ButtonExt buttonExt3;
        private System.Windows.Forms.Panel panel1;
    }
}