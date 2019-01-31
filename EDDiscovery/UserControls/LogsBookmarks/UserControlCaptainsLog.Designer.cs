namespace EDDiscovery.UserControls
{
    partial class UserControlCaptainsLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlCaptainsLog));
            this.tabStrip = new ExtendedControls.TabStrip();
            this.SuspendLayout();
            // 
            // tabStrip
            // 
            this.tabStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStrip.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tabStrip.DropDownBorderColor = System.Drawing.Color.Green;
            this.tabStrip.DropDownHeight = 500;
            this.tabStrip.DropDownItemSeperatorColor = System.Drawing.Color.Purple;
            this.tabStrip.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tabStrip.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tabStrip.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tabStrip.DropDownWidth = 400;
            this.tabStrip.EmptyColor = System.Drawing.Color.Empty;
            this.tabStrip.EmptyColorScaling = 0.5F;
            this.tabStrip.EmptyPanelIcon = ((System.Drawing.Image)(resources.GetObject("tabStrip.EmptyPanelIcon")));
            this.tabStrip.Location = new System.Drawing.Point(0, 0);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.SelectedBackColor = System.Drawing.Color.Transparent;
            this.tabStrip.SelectedIndex = -1;
            this.tabStrip.ShowPopOut = false;
            this.tabStrip.Size = new System.Drawing.Size(676, 443);
            this.tabStrip.StripMode = ExtendedControls.TabStrip.StripModeType.StripTopOpen;
            this.tabStrip.TabFieldSpacing = 8;
            this.tabStrip.TabIndex = 1;
            // 
            // UserControlCaptainsLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabStrip);
            this.Name = "UserControlCaptainsLog";
            this.Size = new System.Drawing.Size(676, 443);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.TabStrip tabStrip;
    }
}
