namespace EDDiscovery.Controls
{
    partial class TabStrip
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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.drawnPanelPopOut = new ExtendedControls.DrawnPanel();
            this.panelArrowRight = new System.Windows.Forms.Panel();
            this.panelArrowLeft = new System.Windows.Forms.Panel();
            this.panelSelected = new System.Windows.Forms.Panel();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.drawnPanelPopOut);
            this.panelBottom.Controls.Add(this.panelArrowRight);
            this.panelBottom.Controls.Add(this.panelArrowLeft);
            this.panelBottom.Controls.Add(this.panelSelected);
            this.panelBottom.Controls.Add(this.labelCurrent);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 322);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(562, 30);
            this.panelBottom.TabIndex = 0;
            this.panelBottom.MouseEnter += new System.EventHandler(this.panelBottom_MouseEnter);
            this.panelBottom.MouseLeave += new System.EventHandler(this.panelBottom_MouseLeave);
            // 
            // drawnPanelPopOut
            // 
            this.drawnPanelPopOut.DrawnImage = global::EDDiscovery.Properties.Resources.popout;
            this.drawnPanelPopOut.ImageSelected = ExtendedControls.DrawnPanel.ImageType.DrawnImage;
            this.drawnPanelPopOut.ImageText = null;
            this.drawnPanelPopOut.Location = new System.Drawing.Point(387, 3);
            this.drawnPanelPopOut.MarginSize = 4;
            this.drawnPanelPopOut.MouseOverColor = System.Drawing.Color.White;
            this.drawnPanelPopOut.MouseSelectedColor = System.Drawing.Color.Green;
            this.drawnPanelPopOut.Name = "drawnPanelPopOut";
            this.drawnPanelPopOut.Size = new System.Drawing.Size(24, 24);
            this.drawnPanelPopOut.TabIndex = 3;
            this.drawnPanelPopOut.Click += new System.EventHandler(this.panelPopOut_Click);
            // 
            // panelArrowRight
            // 
            this.panelArrowRight.BackgroundImage = global::EDDiscovery.Properties.Resources.ArrowRight;
            this.panelArrowRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelArrowRight.Location = new System.Drawing.Point(304, 4);
            this.panelArrowRight.Name = "panelArrowRight";
            this.panelArrowRight.Size = new System.Drawing.Size(12, 20);
            this.panelArrowRight.TabIndex = 2;
            this.panelArrowRight.Visible = false;
            this.panelArrowRight.Click += new System.EventHandler(this.panelArrowRight_Click);
            this.panelArrowRight.MouseEnter += new System.EventHandler(this.panelBottom_MouseEnter);
            this.panelArrowRight.MouseLeave += new System.EventHandler(this.panelBottom_MouseLeave);
            // 
            // panelArrowLeft
            // 
            this.panelArrowLeft.BackgroundImage = global::EDDiscovery.Properties.Resources.ArrowLeft;
            this.panelArrowLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelArrowLeft.Location = new System.Drawing.Point(272, 4);
            this.panelArrowLeft.Name = "panelArrowLeft";
            this.panelArrowLeft.Size = new System.Drawing.Size(12, 20);
            this.panelArrowLeft.TabIndex = 2;
            this.panelArrowLeft.Visible = false;
            this.panelArrowLeft.Click += new System.EventHandler(this.panelArrowLeft_Click);
            this.panelArrowLeft.MouseEnter += new System.EventHandler(this.panelBottom_MouseEnter);
            this.panelArrowLeft.MouseLeave += new System.EventHandler(this.panelBottom_MouseLeave);
            // 
            // panelSelected
            // 
            this.panelSelected.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelSelected.Location = new System.Drawing.Point(3, 3);
            this.panelSelected.Name = "panelSelected";
            this.panelSelected.Size = new System.Drawing.Size(24, 24);
            this.panelSelected.TabIndex = 1;
            this.panelSelected.MouseEnter += new System.EventHandler(this.panelBottom_MouseEnter);
            this.panelSelected.MouseLeave += new System.EventHandler(this.panelBottom_MouseLeave);
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(33, 8);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(92, 13);
            this.labelCurrent.TabIndex = 0;
            this.labelCurrent.Text = "Tab Strip Control..";
            this.labelCurrent.MouseEnter += new System.EventHandler(this.panelBottom_MouseEnter);
            this.labelCurrent.MouseLeave += new System.EventHandler(this.panelBottom_MouseLeave);
            // 
            // TabStrip
            // 
            this.Controls.Add(this.panelBottom);
            this.Name = "TabStrip";
            this.Size = new System.Drawing.Size(562, 352);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.TabStrip_Layout);
            this.Resize += new System.EventHandler(this.TabStrip_Resize);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Panel panelSelected;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelArrowRight;
        private System.Windows.Forms.Panel panelArrowLeft;
        private ExtendedControls.DrawnPanel drawnPanelPopOut;
    }
}
