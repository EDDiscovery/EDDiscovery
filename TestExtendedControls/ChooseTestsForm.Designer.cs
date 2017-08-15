namespace DialogTest
{
    partial class ChooseTestsForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAutoComplete = new System.Windows.Forms.Button();
            this.btnExtendedControls = new System.Windows.Forms.Button();
            this.btnRollUp = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAutoComplete, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExtendedControls, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRollUp, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(279, 189);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnAutoComplete
            // 
            this.btnAutoComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoComplete.Location = new System.Drawing.Point(3, 3);
            this.btnAutoComplete.Name = "btnAutoComplete";
            this.btnAutoComplete.Size = new System.Drawing.Size(273, 41);
            this.btnAutoComplete.TabIndex = 0;
            this.btnAutoComplete.Text = "Test Auto Complete";
            this.btnAutoComplete.UseVisualStyleBackColor = true;
            this.btnAutoComplete.Click += new System.EventHandler(this.btnAutoComplete_Click);
            // 
            // btnExtendedControls
            // 
            this.btnExtendedControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtendedControls.Location = new System.Drawing.Point(3, 50);
            this.btnExtendedControls.Name = "btnExtendedControls";
            this.btnExtendedControls.Size = new System.Drawing.Size(273, 41);
            this.btnExtendedControls.TabIndex = 1;
            this.btnExtendedControls.Text = "Test Extended Controls";
            this.btnExtendedControls.UseVisualStyleBackColor = true;
            this.btnExtendedControls.Click += new System.EventHandler(this.btnExtendedControls_Click);
            // 
            // btnRollUp
            // 
            this.btnRollUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRollUp.Location = new System.Drawing.Point(3, 97);
            this.btnRollUp.Name = "btnRollUp";
            this.btnRollUp.Size = new System.Drawing.Size(273, 41);
            this.btnRollUp.TabIndex = 3;
            this.btnRollUp.Text = "Test Roll Up Panel";
            this.btnRollUp.UseVisualStyleBackColor = true;
            this.btnRollUp.Click += new System.EventHandler(this.btnRollUp_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.Location = new System.Drawing.Point(3, 144);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(273, 42);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ChooseTestsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(304, 214);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ChooseTestsForm";
            this.Text = "Test Extended Controls";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAutoComplete;
        private System.Windows.Forms.Button btnExtendedControls;
        private System.Windows.Forms.Button btnRollUp;
        private System.Windows.Forms.Button btnExit;
    }
}
