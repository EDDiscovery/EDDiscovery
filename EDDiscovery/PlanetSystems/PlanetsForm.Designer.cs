namespace EDDiscovery2.PlanetSystems
{
    partial class PlanetsForm
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
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.CausesValidation = false;
            this.textBoxSystemName.Location = new System.Drawing.Point(69, 12);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = true;
            this.textBoxSystemName.Size = new System.Drawing.Size(192, 20);
            this.textBoxSystemName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "System";
            // 
            // PlanetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 390);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSystemName);
            this.Name = "PlanetsForm";
            this.Text = "PlanetsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Label label1;
    }
}