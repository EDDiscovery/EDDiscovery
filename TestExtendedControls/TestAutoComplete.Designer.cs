namespace DialogTest
{
    partial class TestAutoComplete
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
            this.autoCompleteTextBox1 = new ExtendedControls.AutoCompleteTextBox();
            this.SuspendLayout();
            // 
            // autoCompleteTextBox1
            // 
            this.autoCompleteTextBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.autoCompleteTextBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.autoCompleteTextBox1.BorderColor = System.Drawing.Color.Transparent;
            this.autoCompleteTextBox1.BorderColorScaling = 0.5F;
            this.autoCompleteTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoCompleteTextBox1.ControlBackground = System.Drawing.SystemColors.Control;
            this.autoCompleteTextBox1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.autoCompleteTextBox1.DropDownBorderColor = System.Drawing.Color.Green;
            this.autoCompleteTextBox1.DropDownHeight = 200;
            this.autoCompleteTextBox1.DropDownItemHeight = 20;
            this.autoCompleteTextBox1.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.autoCompleteTextBox1.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.autoCompleteTextBox1.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.autoCompleteTextBox1.DropDownWidth = 0;
            this.autoCompleteTextBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoCompleteTextBox1.Location = new System.Drawing.Point(40, 37);
            this.autoCompleteTextBox1.Multiline = false;
            this.autoCompleteTextBox1.Name = "autoCompleteTextBox1";
            this.autoCompleteTextBox1.ReadOnly = false;
            this.autoCompleteTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.autoCompleteTextBox1.SelectionLength = 0;
            this.autoCompleteTextBox1.SelectionStart = 0;
            this.autoCompleteTextBox1.Size = new System.Drawing.Size(316, 23);
            this.autoCompleteTextBox1.TabIndex = 0;
            this.autoCompleteTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.autoCompleteTextBox1.WordWrap = true;
            // 
            // TestAutoComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 334);
            this.Controls.Add(this.autoCompleteTextBox1);
            this.Name = "TestAutoComplete";
            this.Text = "TestAutoComplete";
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.AutoCompleteTextBox autoCompleteTextBox1;
    }
}