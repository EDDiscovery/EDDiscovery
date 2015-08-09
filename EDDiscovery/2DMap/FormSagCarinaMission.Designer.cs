namespace EDDiscovery2
{
    partial class FormSagCarinaMission
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSagCarinaMission));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.lower4thQuadrantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sC00ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sC01ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1158, 606);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1157, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lower4thQuadrantToolStripMenuItem,
            this.sC00ToolStripMenuItem,
            this.sC01ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // lower4thQuadrantToolStripMenuItem
            // 
            this.lower4thQuadrantToolStripMenuItem.Checked = true;
            this.lower4thQuadrantToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lower4thQuadrantToolStripMenuItem.Name = "lower4thQuadrantToolStripMenuItem";
            this.lower4thQuadrantToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.lower4thQuadrantToolStripMenuItem.Text = "Lower 4th quadrant";
            // 
            // sC00ToolStripMenuItem
            // 
            this.sC00ToolStripMenuItem.Name = "sC00ToolStripMenuItem";
            this.sC00ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.sC00ToolStripMenuItem.Text = "SC-00";
            // 
            // sC01ToolStripMenuItem
            // 
            this.sC01ToolStripMenuItem.Name = "sC01ToolStripMenuItem";
            this.sC01ToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.sC01ToolStripMenuItem.Text = "SC-01";
            // 
            // FormSagCarinaMission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 638);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormSagCarinaMission";
            this.Text = "FormSagCarinaMission";
            this.Load += new System.EventHandler(this.FormSagCarinaMission_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem lower4thQuadrantToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sC00ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sC01ToolStripMenuItem;
    }
}