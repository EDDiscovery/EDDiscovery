using static ExtendedControls.ThemeStandard;

namespace EDDiscovery.UserControls
{
    partial class SurfaceBookmarksForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToCompassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelSurface = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewMarks = new System.Windows.Forms.DataGridView();
            this.BodyName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SurfaceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SurfaceDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarks)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToCompassToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 26);
            // 
            // sendToCompassToolStripMenuItem
            // 
            this.sendToCompassToolStripMenuItem.Name = "sendToCompassToolStripMenuItem";
            this.sendToCompassToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.sendToCompassToolStripMenuItem.Text = "Send to compass";
            this.sendToCompassToolStripMenuItem.Click += new System.EventHandler(this.sendToCompassToolStripMenuItem_Click);
            // 
            // labelSurface
            // 
            this.labelSurface.AutoSize = true;
            this.labelSurface.Location = new System.Drawing.Point(3, 4);
            this.labelSurface.Name = "labelSurface";
            this.labelSurface.Size = new System.Drawing.Size(149, 13);
            this.labelSurface.TabIndex = 2;
            this.labelSurface.Text = "Surface Bookmarks In System";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.labelSurface);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(657, 32);
            this.panelTop.TabIndex = 5;
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewMarks);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(657, 168);
            this.dataViewScrollerPanel1.TabIndex = 6;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 1;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(637, 21);
            this.vScrollBarCustom1.Maximum = 0;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 147);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 7;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = 0;
            this.vScrollBarCustom1.ValueLimited = 0;
            // 
            // dataGridViewMarks
            // 
            this.dataGridViewMarks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BodyName,
            this.SurfaceName,
            this.SurfaceDesc,
            this.Latitude,
            this.Longitude,
            this.Valid});
            this.dataGridViewMarks.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridViewMarks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMarks.Name = "dataGridViewMarks";
            this.dataGridViewMarks.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMarks.Size = new System.Drawing.Size(637, 168);
            this.dataGridViewMarks.TabIndex = 3;
            this.dataGridViewMarks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMarks_CellEndEdit);
            this.dataGridViewMarks.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewMarks_CellValidating);
            this.dataGridViewMarks.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewMarks_UserDeletingRow);
            // 
            // BodyName
            // 
            this.BodyName.FillWeight = 150F;
            this.BodyName.HeaderText = "Planetary Body";
            this.BodyName.Name = "BodyName";
            this.BodyName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BodyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SurfaceName
            // 
            this.SurfaceName.FillWeight = 150F;
            this.SurfaceName.HeaderText = "Name";
            this.SurfaceName.Name = "SurfaceName";
            // 
            // SurfaceDesc
            // 
            this.SurfaceDesc.FillWeight = 150F;
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
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 26);
            // 
            // SurfaceBookmarksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panelTop);
            this.Name = "SurfaceBookmarksForm";
            this.Size = new System.Drawing.Size(657, 200);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMarks;
        private System.Windows.Forms.Label labelSurface;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sendToCompassToolStripMenuItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn BodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SurfaceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SurfaceDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Valid;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
    }
}
