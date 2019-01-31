namespace EDDiscovery.UserControls
{
    partial class UserControlContainerGrid
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
            this.rollUpPanelMenu = new ExtendedControls.ExtPanelRollUp();
            this.buttonExtTile = new ExtendedControls.ExtButton();
            this.buttonExtDelete = new ExtendedControls.ExtButton();
            this.panelPlayfield = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonExtPopOut = new ExtendedControls.ExtButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rollUpPanelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // rollUpPanelMenu
            // 
            this.rollUpPanelMenu.Controls.Add(this.buttonExtTile);
            this.rollUpPanelMenu.Controls.Add(this.buttonExtDelete);
            this.rollUpPanelMenu.Controls.Add(this.buttonExtPopOut);
            this.rollUpPanelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelMenu.HiddenMarkerWidth = 0;
            this.rollUpPanelMenu.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelMenu.Name = "rollUpPanelMenu";
            this.rollUpPanelMenu.PinState = true;
            this.rollUpPanelMenu.RolledUpHeight = 5;
            this.rollUpPanelMenu.RollUpAnimationTime = 500;
            this.rollUpPanelMenu.RollUpDelay = 1000;
            this.rollUpPanelMenu.ShowHiddenMarker = true;
            this.rollUpPanelMenu.Size = new System.Drawing.Size(912, 32);
            this.rollUpPanelMenu.TabIndex = 1;
            this.rollUpPanelMenu.UnrolledHeight = 32;
            this.rollUpPanelMenu.UnrollHoverDelay = 1000;
            // 
            // buttonExtTile
            // 
            this.buttonExtTile.BorderColorScaling = 1.25F;
            this.buttonExtTile.ButtonColorScaling = 0.5F;
            this.buttonExtTile.ButtonDisabledScaling = 0.5F;
            this.buttonExtTile.Image = global::EDDiscovery.Icons.Controls.UCContainer_Tile;
            this.buttonExtTile.Location = new System.Drawing.Point(63, 3);
            this.buttonExtTile.Name = "buttonExtTile";
            this.buttonExtTile.Size = new System.Drawing.Size(24, 24);
            this.buttonExtTile.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonExtTile, "Tile the grid panels");
            this.buttonExtTile.UseVisualStyleBackColor = true;
            this.buttonExtTile.Click += new System.EventHandler(this.buttonExtTile_Click);
            // 
            // buttonExtDelete
            // 
            this.buttonExtDelete.BorderColorScaling = 1.25F;
            this.buttonExtDelete.ButtonColorScaling = 0.5F;
            this.buttonExtDelete.ButtonDisabledScaling = 0.5F;
            this.buttonExtDelete.Image = global::EDDiscovery.Icons.Controls.UCContainer_Remove;
            this.buttonExtDelete.Location = new System.Drawing.Point(33, 3);
            this.buttonExtDelete.Name = "buttonExtDelete";
            this.buttonExtDelete.Size = new System.Drawing.Size(24, 24);
            this.buttonExtDelete.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonExtDelete, "Remove selected panel");
            this.buttonExtDelete.UseVisualStyleBackColor = true;
            this.buttonExtDelete.Click += new System.EventHandler(this.buttonExtDelete_Click);
            // 
            // panelPlayfield
            // 
            this.panelPlayfield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayfield.Location = new System.Drawing.Point(0, 32);
            this.panelPlayfield.Name = "panelPlayfield";
            this.panelPlayfield.Size = new System.Drawing.Size(912, 612);
            this.panelPlayfield.TabIndex = 2;
            this.panelPlayfield.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelPlayfield_MouseClick);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ShowAlways = true;
            // 
            // buttonExtPopOut
            // 
            this.buttonExtPopOut.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtPopOut.BorderColorScaling = 1.25F;
            this.buttonExtPopOut.ButtonColorScaling = 0.5F;
            this.buttonExtPopOut.ButtonDisabledScaling = 0.5F;
            this.buttonExtPopOut.Image = global::EDDiscovery.Icons.Controls.UCContainer_Panels;
            this.buttonExtPopOut.Location = new System.Drawing.Point(3, 3);
            this.buttonExtPopOut.Name = "buttonExtPopOut";
            this.buttonExtPopOut.Size = new System.Drawing.Size(24, 24);
            this.buttonExtPopOut.TabIndex = 2;
            this.toolTip1.SetToolTip(this.buttonExtPopOut, "Click to select a pop out panel to display");
            this.buttonExtPopOut.UseVisualStyleBackColor = false;
            this.buttonExtPopOut.Click += new System.EventHandler(this.buttonExtPopOut_Click);
            // 
            // UserControlContainerGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelPlayfield);
            this.Controls.Add(this.rollUpPanelMenu);
            this.Name = "UserControlContainerGrid";
            this.Size = new System.Drawing.Size(912, 644);
            this.rollUpPanelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtPanelRollUp rollUpPanelMenu;
        private System.Windows.Forms.Panel panelPlayfield;
        private ExtendedControls.ExtButton buttonExtDelete;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtTile;
        private ExtendedControls.ExtButton buttonExtPopOut;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
