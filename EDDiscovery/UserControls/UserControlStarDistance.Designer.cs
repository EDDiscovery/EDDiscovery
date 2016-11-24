namespace EDDiscovery.UserControls
{
    partial class UserControlStarDistance
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
            this.closestContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToTrilaterationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataViewScrollerPanel2 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewNearest = new System.Windows.Forms.DataGridView();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.closestContextMenu.SuspendLayout();
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearest)).BeginInit();
            this.SuspendLayout();
            // 
            // closestContextMenu
            // 
            this.closestContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToTrilaterationToolStripMenuItem1,
            this.viewOnEDSMToolStripMenuItem1});
            this.closestContextMenu.Name = "closestContextMenu";
            this.closestContextMenu.Size = new System.Drawing.Size(178, 48);
            // 
            // addToTrilaterationToolStripMenuItem1
            // 
            this.addToTrilaterationToolStripMenuItem1.Name = "addToTrilaterationToolStripMenuItem1";
            this.addToTrilaterationToolStripMenuItem1.Size = new System.Drawing.Size(177, 22);
            this.addToTrilaterationToolStripMenuItem1.Text = "Add to Trilateration";
            this.addToTrilaterationToolStripMenuItem1.Click += new System.EventHandler(this.addToTrilaterationToolStripMenuItem1_Click);
            // 
            // viewOnEDSMToolStripMenuItem1
            // 
            this.viewOnEDSMToolStripMenuItem1.Name = "viewOnEDSMToolStripMenuItem1";
            this.viewOnEDSMToolStripMenuItem1.Size = new System.Drawing.Size(177, 22);
            this.viewOnEDSMToolStripMenuItem1.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem1.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem1_Click);
            // 
            // dataViewScrollerPanel2
            // 
            this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewNearest);
            this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
            this.dataViewScrollerPanel2.ScrollBarWidth = 20;
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.TabIndex = 25;
            this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom2
            // 
            this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom2.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom2.HideScrollBar = true;
            this.vScrollBarCustom2.LargeChange = 0;
            this.vScrollBarCustom2.Location = new System.Drawing.Point(552, 21);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 551);
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 24;
            this.vScrollBarCustom2.Text = "vScrollBarCustom2";
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
            // 
            // dataGridViewNearest
            // 
            this.dataGridViewNearest.AllowUserToAddRows = false;
            this.dataGridViewNearest.AllowUserToDeleteRows = false;
            this.dataGridViewNearest.AllowUserToResizeRows = false;
            this.dataGridViewNearest.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewNearest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNearest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Distance});
            this.dataGridViewNearest.ContextMenuStrip = this.closestContextMenu;
            this.dataGridViewNearest.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewNearest.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewNearest.Name = "dataGridViewNearest";
            this.dataGridViewNearest.RowHeadersVisible = false;
            this.dataGridViewNearest.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewNearest.Size = new System.Drawing.Size(552, 572);
            this.dataGridViewNearest.TabIndex = 23;
            // 
            // Col1
            // 
            this.Col1.HeaderText = "Name";
            this.Col1.MinimumWidth = 50;
            this.Col1.Name = "Col1";
            // 
            // Distance
            // 
            this.Distance.FillWeight = 25F;
            this.Distance.HeaderText = "Distance";
            this.Distance.MinimumWidth = 50;
            this.Distance.Name = "Distance";
            // 
            // UserControlStarDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Name = "UserControlStarDistance";
            this.Size = new System.Drawing.Size(572, 572);
            this.closestContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNearest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip closestContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToTrilaterationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem1;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewNearest;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
    }
}
