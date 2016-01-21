namespace EDDiscovery2
{
    partial class AddMaterialNodeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMaterialNodeControl));
            this.dataGridViewPlanet = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddPlanet = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.materialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlanet)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPlanet
            // 
            this.dataGridViewPlanet.AllowUserToAddRows = false;
            this.dataGridViewPlanet.AllowUserToDeleteRows = false;
            this.dataGridViewPlanet.AllowUserToResizeRows = false;
            this.dataGridViewPlanet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPlanet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlanet.Location = new System.Drawing.Point(0, 51);
            this.dataGridViewPlanet.Name = "dataGridViewPlanet";
            this.dataGridViewPlanet.Size = new System.Drawing.Size(517, 318);
            this.dataGridViewPlanet.TabIndex = 0;
            this.dataGridViewPlanet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPlanet_CellContentClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddPlanet,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(520, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAddPlanet
            // 
            this.toolStripButtonAddPlanet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddPlanet.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPlanet.Image")));
            this.toolStripButtonAddPlanet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPlanet.Name = "toolStripButtonAddPlanet";
            this.toolStripButtonAddPlanet.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddPlanet.Text = "Add Planet";
            this.toolStripButtonAddPlanet.Click += new System.EventHandler(this.toolStripButtonAddPlanet_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton1.Text = "Save";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // materialBindingSource
            // 
            this.materialBindingSource.DataSource = typeof(EDDiscovery2.Material);
            // 
            // AddMaterialNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridViewPlanet);
            this.Name = "AddMaterialNodeControl";
            this.Size = new System.Drawing.Size(520, 372);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlanet)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPlanet;
        private System.Windows.Forms.BindingSource materialBindingSource;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPlanet;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
