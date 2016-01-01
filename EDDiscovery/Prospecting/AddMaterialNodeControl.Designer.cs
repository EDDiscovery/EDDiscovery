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
            this.materialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPlanetType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColumnGravity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTerrain = new System.Windows.Forms.DataGridViewComboBoxColumn();
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
            this.dataGridViewPlanet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnPlanetType,
            this.ColumnGravity,
            this.ColumnTerrain});
            this.dataGridViewPlanet.Location = new System.Drawing.Point(0, 51);
            this.dataGridViewPlanet.Name = "dataGridViewPlanet";
            this.dataGridViewPlanet.Size = new System.Drawing.Size(517, 318);
            this.dataGridViewPlanet.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddPlanet});
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
            // materialBindingSource
            // 
            this.materialBindingSource.DataSource = typeof(EDDiscovery2.Material);
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            // 
            // ColumnPlanetType
            // 
            this.ColumnPlanetType.HeaderText = "Type";
            this.ColumnPlanetType.Name = "ColumnPlanetType";
            this.ColumnPlanetType.Width = 60;
            // 
            // ColumnGravity
            // 
            this.ColumnGravity.HeaderText = "Gravity";
            this.ColumnGravity.Name = "ColumnGravity";
            this.ColumnGravity.Width = 40;
            // 
            // ColumnTerrain
            // 
            this.ColumnTerrain.HeaderText = "Terrain";
            this.ColumnTerrain.Items.AddRange(new object[] {
            "Unknown",
            "V easy",
            "Easy",
            "Medium",
            "Hard",
            "V hard",
            ""});
            this.ColumnTerrain.Name = "ColumnTerrain";
            this.ColumnTerrain.Width = 40;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnPlanetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGravity;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnTerrain;
    }
}
