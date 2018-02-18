namespace EDDiscovery.UserControls
{
    partial class UserControlSurfaceBookmarks
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
            this.dataGridViewMarks = new System.Windows.Forms.DataGridView();
            this.BodyName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SurfaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SurfaceDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelSurface = new ExtendedControls.LabelExt();
            this.buttonSave = new ExtendedControls.ButtonExt();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarks)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMarks
            // 
            this.dataGridViewMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BodyName,
            this.SurfaceName,
            this.SurfaceDesc,
            this.Latitude,
            this.Longitude,
            this.Valid});
            this.dataGridViewMarks.Location = new System.Drawing.Point(6, 29);
            this.dataGridViewMarks.Name = "dataGridViewMarks";
            this.dataGridViewMarks.Size = new System.Drawing.Size(646, 165);
            this.dataGridViewMarks.TabIndex = 3;
            this.dataGridViewMarks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMarks_CellEndEdit);
            this.dataGridViewMarks.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewMarks_CellValidating);
            this.dataGridViewMarks.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewMarks_UserDeletingRow);
            // 
            // BodyName
            // 
            this.BodyName.HeaderText = "Planetary Body";
            this.BodyName.Name = "BodyName";
            this.BodyName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BodyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SurfaceName
            // 
            this.SurfaceName.HeaderText = "Name";
            this.SurfaceName.Name = "SurfaceName";
            // 
            // SurfaceDesc
            // 
            this.SurfaceDesc.HeaderText = "Description";
            this.SurfaceDesc.Name = "SurfaceDesc";
            // 
            // Latitude
            // 
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            // 
            // Longitude
            // 
            this.Longitude.HeaderText = "Longitude";
            this.Longitude.Name = "Longitude";
            // 
            // Valid
            // 
            this.Valid.HeaderText = "Saveable";
            this.Valid.Name = "Valid";
            this.Valid.ReadOnly = true;
            // 
            // labelSurface
            // 
            this.labelSurface.AutoSize = true;
            this.labelSurface.Location = new System.Drawing.Point(3, 0);
            this.labelSurface.Name = "labelSurface";
            this.labelSurface.Size = new System.Drawing.Size(149, 13);
            this.labelSurface.TabIndex = 2;
            this.labelSurface.Text = "Surface Bookmarks In System";
            this.labelSurface.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(158, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // UserControlSurfaceBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGridViewMarks);
            this.Controls.Add(this.labelSurface);
            this.Name = "UserControlSurfaceBookmarks";
            this.Size = new System.Drawing.Size(657, 200);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMarks;
        private ExtendedControls.LabelExt labelSurface;
        private System.Windows.Forms.DataGridViewComboBoxColumn BodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SurfaceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SurfaceDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Valid;
        private ExtendedControls.ButtonExt buttonSave;
    }
}
