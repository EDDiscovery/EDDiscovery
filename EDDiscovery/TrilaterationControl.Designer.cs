namespace EDDiscovery
{
    partial class TrilaterationControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewDistances = new System.Windows.Forms.DataGridView();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCalculated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.labelTargetSystem = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.labelCoordinates = new System.Windows.Forms.Label();
            this.textBoxCoordinateX = new System.Windows.Forms.TextBox();
            this.labelCoordinateX = new System.Windows.Forms.Label();
            this.labelCoordinateY = new System.Windows.Forms.Label();
            this.textBoxCoordinateY = new System.Windows.Forms.TextBox();
            this.labelCoordinateZ = new System.Windows.Forms.Label();
            this.textBoxCoordinateZ = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.dataGridViewClosestSystems = new System.Windows.Forms.DataGridView();
            this.dataGridViewSuggestedSystems = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSuggestedSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDistances
            // 
            this.dataGridViewDistances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDistances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnCalculated,
            this.ColumnStatus});
            this.dataGridViewDistances.Location = new System.Drawing.Point(16, 69);
            this.dataGridViewDistances.Name = "dataGridViewDistances";
            this.dataGridViewDistances.Size = new System.Drawing.Size(500, 246);
            this.dataGridViewDistances.TabIndex = 0;
            this.dataGridViewDistances.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellEndEdit);
            this.dataGridViewDistances.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewDistances_CellValidating);
            this.dataGridViewDistances.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewDistances_EditingControlShowing);
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.Width = 200;
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.HeaderText = "Distance";
            this.ColumnDistance.Name = "ColumnDistance";
            this.ColumnDistance.Width = 75;
            // 
            // ColumnCalculated
            // 
            this.ColumnCalculated.HeaderText = "Calculated";
            this.ColumnCalculated.Name = "ColumnCalculated";
            this.ColumnCalculated.ReadOnly = true;
            this.ColumnCalculated.Width = 75;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Width = 105;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.Location = new System.Drawing.Point(16, 40);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = true;
            this.textBoxSystemName.Size = new System.Drawing.Size(146, 20);
            this.textBoxSystemName.TabIndex = 1;
            // 
            // labelTargetSystem
            // 
            this.labelTargetSystem.AutoSize = true;
            this.labelTargetSystem.Location = new System.Drawing.Point(13, 24);
            this.labelTargetSystem.Name = "labelTargetSystem";
            this.labelTargetSystem.Size = new System.Drawing.Size(78, 13);
            this.labelTargetSystem.TabIndex = 2;
            this.labelTargetSystem.Text = "Target System:";
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Enabled = false;
            this.buttonSubmit.Location = new System.Drawing.Point(389, 38);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(109, 23);
            this.buttonSubmit.TabIndex = 4;
            this.buttonSubmit.Text = "Submit Distances";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            // 
            // labelCoordinates
            // 
            this.labelCoordinates.AutoSize = true;
            this.labelCoordinates.Location = new System.Drawing.Point(174, 24);
            this.labelCoordinates.Name = "labelCoordinates";
            this.labelCoordinates.Size = new System.Drawing.Size(122, 13);
            this.labelCoordinates.TabIndex = 5;
            this.labelCoordinates.Text = "Trilaterated Coordinates:";
            // 
            // textBoxCoordinateX
            // 
            this.textBoxCoordinateX.Location = new System.Drawing.Point(191, 40);
            this.textBoxCoordinateX.Name = "textBoxCoordinateX";
            this.textBoxCoordinateX.ReadOnly = true;
            this.textBoxCoordinateX.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateX.TabIndex = 6;
            this.textBoxCoordinateX.Text = "?";
            this.textBoxCoordinateX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCoordinateX
            // 
            this.labelCoordinateX.AutoSize = true;
            this.labelCoordinateX.Location = new System.Drawing.Point(174, 43);
            this.labelCoordinateX.Name = "labelCoordinateX";
            this.labelCoordinateX.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateX.TabIndex = 7;
            this.labelCoordinateX.Text = "X:";
            // 
            // labelCoordinateY
            // 
            this.labelCoordinateY.AutoSize = true;
            this.labelCoordinateY.Location = new System.Drawing.Point(241, 43);
            this.labelCoordinateY.Name = "labelCoordinateY";
            this.labelCoordinateY.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateY.TabIndex = 9;
            this.labelCoordinateY.Text = "Y:";
            // 
            // textBoxCoordinateY
            // 
            this.textBoxCoordinateY.Location = new System.Drawing.Point(258, 40);
            this.textBoxCoordinateY.Name = "textBoxCoordinateY";
            this.textBoxCoordinateY.ReadOnly = true;
            this.textBoxCoordinateY.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateY.TabIndex = 8;
            this.textBoxCoordinateY.Text = "?";
            this.textBoxCoordinateY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelCoordinateZ
            // 
            this.labelCoordinateZ.AutoSize = true;
            this.labelCoordinateZ.Location = new System.Drawing.Point(308, 43);
            this.labelCoordinateZ.Name = "labelCoordinateZ";
            this.labelCoordinateZ.Size = new System.Drawing.Size(17, 13);
            this.labelCoordinateZ.TabIndex = 11;
            this.labelCoordinateZ.Text = "Z:";
            // 
            // textBoxCoordinateZ
            // 
            this.textBoxCoordinateZ.Location = new System.Drawing.Point(325, 40);
            this.textBoxCoordinateZ.Name = "textBoxCoordinateZ";
            this.textBoxCoordinateZ.ReadOnly = true;
            this.textBoxCoordinateZ.Size = new System.Drawing.Size(50, 20);
            this.textBoxCoordinateZ.TabIndex = 10;
            this.textBoxCoordinateZ.Text = "?";
            this.textBoxCoordinateZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelStatus.Location = new System.Drawing.Point(134, 4);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(241, 19);
            this.labelStatus.TabIndex = 12;
            this.labelStatus.Text = "Status";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewClosestSystems
            // 
            this.dataGridViewClosestSystems.AllowUserToAddRows = false;
            this.dataGridViewClosestSystems.AllowUserToDeleteRows = false;
            this.dataGridViewClosestSystems.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClosestSystems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewClosestSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClosestSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSuggestedSystem});
            this.dataGridViewClosestSystems.Location = new System.Drawing.Point(16, 321);
            this.dataGridViewClosestSystems.Name = "dataGridViewClosestSystems";
            this.dataGridViewClosestSystems.ReadOnly = true;
            this.dataGridViewClosestSystems.RowHeadersVisible = false;
            this.dataGridViewClosestSystems.Size = new System.Drawing.Size(225, 185);
            this.dataGridViewClosestSystems.TabIndex = 13;
            this.dataGridViewClosestSystems.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewClosestSystems_CellMouseDoubleClick);
            // 
            // dataGridViewSuggestedSystems
            // 
            this.dataGridViewSuggestedSystems.AllowUserToAddRows = false;
            this.dataGridViewSuggestedSystems.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSuggestedSystems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSuggestedSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSuggestedSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dataGridViewSuggestedSystems.Location = new System.Drawing.Point(283, 321);
            this.dataGridViewSuggestedSystems.Name = "dataGridViewSuggestedSystems";
            this.dataGridViewSuggestedSystems.ReadOnly = true;
            this.dataGridViewSuggestedSystems.RowHeadersVisible = false;
            this.dataGridViewSuggestedSystems.Size = new System.Drawing.Size(233, 185);
            this.dataGridViewSuggestedSystems.TabIndex = 14;
            this.dataGridViewSuggestedSystems.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSuggestedSystems_CellContentDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Suggested Systems";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // ColumnSuggestedSystem
            // 
            this.ColumnSuggestedSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnSuggestedSystem.HeaderText = "Closest Systems (coming soon)";
            this.ColumnSuggestedSystem.MinimumWidth = 100;
            this.ColumnSuggestedSystem.Name = "ColumnSuggestedSystem";
            this.ColumnSuggestedSystem.ReadOnly = true;
            // 
            // TrilaterationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewSuggestedSystems);
            this.Controls.Add(this.dataGridViewClosestSystems);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelCoordinateZ);
            this.Controls.Add(this.textBoxCoordinateZ);
            this.Controls.Add(this.labelCoordinateY);
            this.Controls.Add(this.textBoxCoordinateY);
            this.Controls.Add(this.labelCoordinateX);
            this.Controls.Add(this.textBoxCoordinateX);
            this.Controls.Add(this.labelCoordinates);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.labelTargetSystem);
            this.Controls.Add(this.textBoxSystemName);
            this.Controls.Add(this.dataGridViewDistances);
            this.Name = "TrilaterationControl";
            this.Size = new System.Drawing.Size(538, 522);
            this.VisibleChanged += new System.EventHandler(this.TrilaterationControl_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuggestedSystems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDistances;
        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Label labelTargetSystem;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalculated;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
        private System.Windows.Forms.Label labelCoordinates;
        private System.Windows.Forms.TextBox textBoxCoordinateX;
        private System.Windows.Forms.Label labelCoordinateX;
        private System.Windows.Forms.Label labelCoordinateY;
        private System.Windows.Forms.TextBox textBoxCoordinateY;
        private System.Windows.Forms.Label labelCoordinateZ;
        private System.Windows.Forms.TextBox textBoxCoordinateZ;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.DataGridView dataGridViewClosestSystems;
        private System.Windows.Forms.DataGridView dataGridViewSuggestedSystems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSuggestedSystem;
    }
}
