namespace DialogTest
{
    partial class TestJournalRead
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
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.textBoxFile = new ExtendedControls.TextBoxBorder();
            this.SuspendLayout();
            // 
            // buttonExt1
            // 
            this.buttonExt1.Location = new System.Drawing.Point(22, 69);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(75, 23);
            this.buttonExt1.TabIndex = 0;
            this.buttonExt1.Text = "Read";
            this.buttonExt1.UseVisualStyleBackColor = true;
            this.buttonExt1.Click += new System.EventHandler(this.buttonExt1_Click);
            // 
            // textBoxFile
            // 
            this.textBoxFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFile.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFile.BorderColorScaling = 0.5F;
            this.textBoxFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFile.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFile.Location = new System.Drawing.Point(22, 23);
            this.textBoxFile.Multiline = false;
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.ReadOnly = false;
            this.textBoxFile.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFile.SelectionLength = 0;
            this.textBoxFile.SelectionStart = 0;
            this.textBoxFile.Size = new System.Drawing.Size(250, 20);
            this.textBoxFile.TabIndex = 1;
            this.textBoxFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFile.WordWrap = true;
            // 
            // TestJournalRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.buttonExt1);
            this.Name = "TestJournalRead";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ButtonExt buttonExt1;
        private ExtendedControls.TextBoxBorder textBoxFile;
    }
}