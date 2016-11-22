namespace EDDiscovery2
{
    partial class TabControlForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.statusStripCustom1 = new ExtendedControls.StatusStripCustom();
            this.label_index = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(598, 3);
            this.panel_close.MarginSize = 6;
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 20;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.ImageText = null;
            this.panel_minimize.Location = new System.Drawing.Point(568, 3);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 21;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // statusStripCustom1
            // 
            this.statusStripCustom1.Location = new System.Drawing.Point(0, 448);
            this.statusStripCustom1.Name = "statusStripCustom1";
            this.statusStripCustom1.Size = new System.Drawing.Size(624, 22);
            this.statusStripCustom1.TabIndex = 22;
            this.statusStripCustom1.Text = "statusStripCustom1";
            // 
            // label_index
            // 
            this.label_index.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(508, 13);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(27, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "N/A";
            // 
            // TabControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 470);
            this.Controls.Add(this.label_index);
            this.Controls.Add(this.statusStripCustom1);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.panel_close);
            this.Name = "TabControlForm";
            this.Text = "TabControlForm";
            this.Activated += new System.EventHandler(this.TabControlForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TabControlForm_FormClosing);
            this.Load += new System.EventHandler(this.TabControlForm_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.TabControlForm_Layout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private ExtendedControls.StatusStripCustom statusStripCustom1;
        private System.Windows.Forms.Label label_index;
    }
}