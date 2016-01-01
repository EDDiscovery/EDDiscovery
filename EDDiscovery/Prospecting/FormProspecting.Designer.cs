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
            this.addMaterialNodeControl1 = new EDDiscovery2.Prospecting.AddMaterialNodeControl();
            this.SuspendLayout();
            // 
            // addMaterialNodeControl1
            // 
            this.addMaterialNodeControl1.Location = new System.Drawing.Point(12, 32);
            this.addMaterialNodeControl1.Name = "addMaterialNodeControl1";
            this.addMaterialNodeControl1.Size = new System.Drawing.Size(258, 436);
            this.addMaterialNodeControl1.TabIndex = 0;
            // 
            // FormProspecting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 592);
            this.Controls.Add(this.addMaterialNodeControl1);
            this.Name = "FormProspecting";
            this.Text = "Material node";
            this.ResumeLayout(false);

        }

        #endregion

        private AddMaterialNodeControl addMaterialNodeControl1;
    }
}