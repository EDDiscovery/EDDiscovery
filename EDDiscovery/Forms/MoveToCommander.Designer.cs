namespace EDDiscovery2
{
    partial class MoveToCommander
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
            this.comboBoxCommanders = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonTransfer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxCommanders
            // 
            this.comboBoxCommanders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommanders.FormattingEnabled = true;
            this.comboBoxCommanders.Location = new System.Drawing.Point(12, 39);
            this.comboBoxCommanders.Name = "comboBoxCommanders";
            this.comboBoxCommanders.Size = new System.Drawing.Size(165, 21);
            this.comboBoxCommanders.TabIndex = 0;
            this.comboBoxCommanders.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommanders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Move selected history to commander.";
            // 
            // buttonTransfer
            // 
            this.buttonTransfer.Location = new System.Drawing.Point(253, 12);
            this.buttonTransfer.Name = "buttonTransfer";
            this.buttonTransfer.Size = new System.Drawing.Size(75, 23);
            this.buttonTransfer.TabIndex = 3;
            this.buttonTransfer.Text = "Transfer";
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Click += new System.EventHandler(this.buttonTransfer_Click);
            // 
            // MoveToCommander
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 113);
            this.Controls.Add(this.buttonTransfer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCommanders);
            this.Name = "MoveToCommander";
            this.Text = "MoveToCommander";
            this.Load += new System.EventHandler(this.MoveToCommander_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCommanders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonTransfer;
    }
}