namespace EDDiscovery2
{
    partial class FormProspecting
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
            this.addMaterialNodeControl1 = new EDDiscovery2.AddMaterialNodeControl();
            this.SuspendLayout();
            // 
            // addMaterialNodeControl1
            // 
            this.addMaterialNodeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addMaterialNodeControl1.Location = new System.Drawing.Point(12, 32);
            this.addMaterialNodeControl1.Name = "addMaterialNodeControl1";
            this.addMaterialNodeControl1.Size = new System.Drawing.Size(783, 154);
            this.addMaterialNodeControl1.TabIndex = 0;
            // 
            // FormProspecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 214);
            this.Controls.Add(this.addMaterialNodeControl1);
            this.Name = "FormProspecting";
            this.Text = "Material node";
            this.ResumeLayout(false);

        }

        #endregion

        private AddMaterialNodeControl addMaterialNodeControl1;
    }
}