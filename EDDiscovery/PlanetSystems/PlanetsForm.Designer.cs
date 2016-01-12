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
            this.addMaterialNodeControl1 = new EDDiscovery2.AddMaterialNodeControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.CausesValidation = false;
            this.textBoxSystemName.Location = new System.Drawing.Point(69, 12);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.Size = new System.Drawing.Size(192, 20);
            this.textBoxSystemName.TabIndex = 0;
            this.textBoxSystemName.TextChanged += new System.EventHandler(this.textBoxSystemName_TextChanged);
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
            // addMaterialNodeControl1
            // 
            this.addMaterialNodeControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.addMaterialNodeControl1.Location = new System.Drawing.Point(0, 60);
            this.addMaterialNodeControl1.Name = "addMaterialNodeControl1";
            this.addMaterialNodeControl1.Size = new System.Drawing.Size(963, 330);
            this.addMaterialNodeControl1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Current";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PlanetsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 390);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.addMaterialNodeControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSystemName);
            this.Name = "PlanetsForm";
            this.Text = "PlanetsForm";
            this.Load += new System.EventHandler(this.PlanetsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Label label1;
        private AddMaterialNodeControl addMaterialNodeControl1;
        private System.Windows.Forms.Button button1;
    }
}