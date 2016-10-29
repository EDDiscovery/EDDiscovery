namespace EDDiscovery
{
    partial class JournalViewControl
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
            this.userControlJournalGrid = new EDDiscovery.UserControls.UserControlJournalGrid();
            this.SuspendLayout();
            // 
            // userControlJournalGrid
            // 
            this.userControlJournalGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlJournalGrid.Location = new System.Drawing.Point(0, 0);
            this.userControlJournalGrid.Name = "userControlJournalGrid";
            this.userControlJournalGrid.Size = new System.Drawing.Size(912, 582);
            this.userControlJournalGrid.TabIndex = 0;
            // 
            // JournalViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlJournalGrid);
            this.Name = "JournalViewControl";
            this.Size = new System.Drawing.Size(912, 582);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserControlJournalGrid userControlJournalGrid;
    }
}
