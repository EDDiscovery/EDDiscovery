namespace DialogTest
{
    partial class TestTabStrip
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabStrip1 = new ExtendedControls.TabStrip();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tabStrip1);
            this.panel1.Location = new System.Drawing.Point(45, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 100);
            this.panel1.TabIndex = 0;
            // 
            // tabStrip1
            // 
            this.tabStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabStrip1.Location = new System.Drawing.Point(43, 18);
            this.tabStrip1.Name = "tabStrip1";
            this.tabStrip1.SelectedIndex = -1;
            this.tabStrip1.ShowPopOut = true;
            this.tabStrip1.Size = new System.Drawing.Size(484, 46);
            this.tabStrip1.StripAtTop = false;
            this.tabStrip1.TabIndex = 0;
            // 
            // TestTabStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 546);
            this.Controls.Add(this.panel1);
            this.Name = "TestTabStrip";
            this.Text = "TestTabControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.TabStrip tabStrip1;
    }
}