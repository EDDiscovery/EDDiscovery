namespace UnitTest
{
    partial class UnitTests
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
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.panelTest = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.extButtonDrawn1 = new ExtendedControls.ExtButtonDrawn();
            this.panelTest.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLog.Font = new System.Drawing.Font("Cascadia Mono", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1505, 969);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // panelTest
            // 
            this.panelTest.Controls.Add(this.richTextBoxLog);
            this.panelTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTest.Location = new System.Drawing.Point(0, 24);
            this.panelTest.Name = "panelTest";
            this.panelTest.Size = new System.Drawing.Size(1505, 969);
            this.panelTest.TabIndex = 4;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.extButtonDrawn1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1505, 24);
            this.panelTop.TabIndex = 5;
            // 
            // extButtonDrawn1
            // 
            this.extButtonDrawn1.AutoEllipsis = false;
            this.extButtonDrawn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawn1.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawn1.BorderWidth = 1;
            this.extButtonDrawn1.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawn1.Image = null;
            this.extButtonDrawn1.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.extButtonDrawn1.Location = new System.Drawing.Point(1469, 4);
            this.extButtonDrawn1.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawn1.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawn1.MouseSelectedColorEnable = true;
            this.extButtonDrawn1.Name = "extButtonDrawn1";
            this.extButtonDrawn1.Selectable = true;
            this.extButtonDrawn1.Size = new System.Drawing.Size(18, 18);
            this.extButtonDrawn1.TabIndex = 0;
            this.extButtonDrawn1.Text = "extButtonDrawn1";
            this.extButtonDrawn1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawn1.UseMnemonic = true;
            this.extButtonDrawn1.Click += new System.EventHandler(this.extButtonDrawnClose_Click);
            // 
            // UnitTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1505, 993);
            this.Controls.Add(this.panelTest);
            this.Controls.Add(this.panelTop);
            this.Name = "UnitTests";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Unit Test";
            this.panelTest.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Panel panelTest;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn extButtonDrawn1;
    }
}

