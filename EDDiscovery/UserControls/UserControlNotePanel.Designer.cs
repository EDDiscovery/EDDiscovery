namespace EDDiscovery.UserControls
{
    partial class UserControlNotePanel
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
            this.pictureBox = new ExtendedControls.PictureBoxHotspot();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miGMPNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.miSystemNotes = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGMPNotes,
            this.miSystemNotes});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(240, 48);
            // 
            // miGMPNotes
            // 
            this.miGMPNotes.Checked = true;
            this.miGMPNotes.CheckOnClick = true;
            this.miGMPNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miGMPNotes.Name = "miGMPNotes";
            this.miGMPNotes.Size = new System.Drawing.Size(239, 22);
            this.miGMPNotes.Text = "Display galactic mapping notes";
            this.miGMPNotes.Click += new System.EventHandler(this.miGMPNotes_Click);
            // 
            // miSystemNotes
            // 
            this.miSystemNotes.Checked = true;
            this.miSystemNotes.CheckOnClick = true;
            this.miSystemNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miSystemNotes.Name = "miSystemNotes";
            this.miSystemNotes.Size = new System.Drawing.Size(239, 22);
            this.miSystemNotes.Text = "Display system notes";
            this.miSystemNotes.Click += new System.EventHandler(this.miSystemNotes_Click);
            // 
            // UserControlNotePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlNotePanel";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.PictureBoxHotspot pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miSystemNotes;
        private System.Windows.Forms.ToolStripMenuItem miGMPNotes;
    }
}
