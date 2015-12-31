namespace EDDiscovery2.Prospecting
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnDec = new System.Windows.Forms.DataGridViewButtonColumn();
            this.materialBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnNr,
            this.ColumnInc,
            this.ColumnDec});
            this.dataGridView1.Location = new System.Drawing.Point(6, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(249, 382);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Material";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnNr
            // 
            this.ColumnNr.HeaderText = "Nr";
            this.ColumnNr.MaxInputLength = 10;
            this.ColumnNr.Name = "ColumnNr";
            this.ColumnNr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNr.Width = 30;
            // 
            // ColumnInc
            // 
            this.ColumnInc.HeaderText = "Inc";
            this.ColumnInc.Name = "ColumnInc";
            this.ColumnInc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnInc.Text = "+";
            this.ColumnInc.UseColumnTextForButtonValue = true;
            this.ColumnInc.Width = 30;
            // 
            // ColumnDec
            // 
            this.ColumnDec.HeaderText = "Dec";
            this.ColumnDec.Name = "ColumnDec";
            this.ColumnDec.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDec.Text = "-";
            this.ColumnDec.Width = 30;
            // 
            // materialBindingSource
            // 
            this.materialBindingSource.DataSource = typeof(EDDiscovery2.Material);
            // 
            // AddMaterialNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "AddMaterialNodeControl";
            this.Size = new System.Drawing.Size(258, 436);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.materialBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNr;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnInc;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnDec;
        private System.Windows.Forms.BindingSource materialBindingSource;
    }
}
