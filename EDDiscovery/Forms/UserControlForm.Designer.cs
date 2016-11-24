namespace EDDiscovery.Forms
{
    partial class UserControlForm
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
            this.label_index = new System.Windows.Forms.Label();
            this.labelControlText = new System.Windows.Forms.Label();
            this.statusStripCustom1 = new ExtendedControls.StatusStripCustom();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_ontop = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.SuspendLayout();
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(2, 2);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(27, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "N/A";
            // 
            // labelControlText
            // 
            this.labelControlText.AutoSize = true;
            this.labelControlText.Location = new System.Drawing.Point(110, 2);
            this.labelControlText.Name = "labelControlText";
            this.labelControlText.Size = new System.Drawing.Size(27, 13);
            this.labelControlText.TabIndex = 23;
            this.labelControlText.Text = "N/A";
            // 
            // statusStripCustom1
            // 
            this.statusStripCustom1.Location = new System.Drawing.Point(0, 536);
            this.statusStripCustom1.Name = "statusStripCustom1";
            this.statusStripCustom1.Size = new System.Drawing.Size(634, 22);
            this.statusStripCustom1.TabIndex = 26;
            this.statusStripCustom1.Text = "statusStripCustom1";
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(610, -2);
            this.panel_close.MarginSize = 6;
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 25;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_ontop
            // 
            this.panel_ontop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ontop.BackColor = System.Drawing.SystemColors.Control;
            this.panel_ontop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_ontop.DrawnImage = null;
            this.panel_ontop.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Floating;
            this.panel_ontop.ImageText = null;
            this.panel_ontop.Location = new System.Drawing.Point(550, -2);
            this.panel_ontop.MarginSize = 6;
            this.panel_ontop.MouseOverColor = System.Drawing.Color.White;
            this.panel_ontop.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_ontop.Name = "panel_ontop";
            this.panel_ontop.Size = new System.Drawing.Size(24, 24);
            this.panel_ontop.TabIndex = 24;
            this.panel_ontop.Click += new System.EventHandler(this.panel_ontop_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.DrawnImage = null;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.ImageText = null;
            this.panel_minimize.Location = new System.Drawing.Point(580, -2);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 24;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // UserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 558);
            this.Controls.Add(this.statusStripCustom1);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.panel_ontop);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.labelControlText);
            this.Controls.Add(this.label_index);
            this.Name = "UserControlForm";
            this.Text = "UserControlForm";
            this.Activated += new System.EventHandler(this.UserControlForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TabControlForm_FormClosing);
            this.Load += new System.EventHandler(this.UserControlForm_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlForm_Layout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_index;
        private ExtendedControls.DrawnPanel panel_minimize;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.StatusStripCustom statusStripCustom1;
        private System.Windows.Forms.Label labelControlText;
        private ExtendedControls.DrawnPanel panel_ontop;
    }
}