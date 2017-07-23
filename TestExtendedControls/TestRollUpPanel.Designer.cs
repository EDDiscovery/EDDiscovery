namespace DialogTest
{
    partial class TestRollUpPanel
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
            this.rolluppanel = new ExtendedControls.RollUpPanel();
            this.buttonExt3 = new ExtendedControls.ButtonExt();
            this.buttonExt2 = new ExtendedControls.ButtonExt();
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rolluppanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rolluppanel
            // 
            this.rolluppanel.Controls.Add(this.buttonExt3);
            this.rolluppanel.Controls.Add(this.buttonExt2);
            this.rolluppanel.Controls.Add(this.buttonExt1);
            this.rolluppanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.rolluppanel.HiddenMarkerColor = System.Drawing.Color.Red;
            this.rolluppanel.HiddenMarkerMouseOverColor = System.Drawing.Color.Green;
            this.rolluppanel.HiddenMarkerWidth = 0;
            this.rolluppanel.Location = new System.Drawing.Point(0, 0);
            this.rolluppanel.Name = "rolluppanel";
            this.rolluppanel.RolledUpHeight = 5;
            this.rolluppanel.RollPixelStep = 2;
            this.rolluppanel.RollUpDelay = 1000;
            this.rolluppanel.Size = new System.Drawing.Size(941, 40);
            this.rolluppanel.TabIndex = 0;
            this.rolluppanel.UnrolledHeight = 32;
            this.rolluppanel.UnrollHoverDelay = 1000;
            // 
            // buttonExt3
            // 
            this.buttonExt3.BorderColorScaling = 1.25F;
            this.buttonExt3.ButtonColorScaling = 0.5F;
            this.buttonExt3.ButtonDisabledScaling = 0.5F;
            this.buttonExt3.Location = new System.Drawing.Point(226, 6);
            this.buttonExt3.Name = "buttonExt3";
            this.buttonExt3.Size = new System.Drawing.Size(75, 23);
            this.buttonExt3.TabIndex = 1;
            this.buttonExt3.Text = "buttonExt1";
            this.buttonExt3.UseVisualStyleBackColor = true;
            // 
            // buttonExt2
            // 
            this.buttonExt2.BorderColorScaling = 1.25F;
            this.buttonExt2.ButtonColorScaling = 0.5F;
            this.buttonExt2.ButtonDisabledScaling = 0.5F;
            this.buttonExt2.Location = new System.Drawing.Point(117, 5);
            this.buttonExt2.Name = "buttonExt2";
            this.buttonExt2.Size = new System.Drawing.Size(75, 23);
            this.buttonExt2.TabIndex = 1;
            this.buttonExt2.Text = "buttonExt1";
            this.buttonExt2.UseVisualStyleBackColor = true;
            // 
            // buttonExt1
            // 
            this.buttonExt1.BorderColorScaling = 1.25F;
            this.buttonExt1.ButtonColorScaling = 0.5F;
            this.buttonExt1.ButtonDisabledScaling = 0.5F;
            this.buttonExt1.Location = new System.Drawing.Point(13, 6);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(75, 23);
            this.buttonExt1.TabIndex = 1;
            this.buttonExt1.Text = "buttonExt1";
            this.buttonExt1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(941, 478);
            this.panel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(55, 7);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(862, 402);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // TestRollUpPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 518);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rolluppanel);
            this.Name = "TestRollUpPanel";
            this.Text = "TestForm2";
            this.rolluppanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.RollUpPanel rolluppanel;
        private ExtendedControls.ButtonExt buttonExt3;
        private ExtendedControls.ButtonExt buttonExt2;
        private ExtendedControls.ButtonExt buttonExt1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
    }
}