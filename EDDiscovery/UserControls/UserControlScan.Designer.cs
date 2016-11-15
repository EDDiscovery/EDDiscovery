namespace EDDiscovery.UserControls
{
    partial class UserControlScan
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
            this.panelStars = new ExtendedControls.PanelVScroll();
            this.richTextBoxInfo = new ExtendedControls.RichTextBoxScroll();
            this.vScrollBarCustom = new ExtendedControls.VScrollBarCustom();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelStars.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStars
            // 
            this.panelStars.Controls.Add(this.richTextBoxInfo);
            this.panelStars.Controls.Add(this.vScrollBarCustom);
            this.panelStars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStars.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelStars.Location = new System.Drawing.Point(0, 0);
            this.panelStars.Name = "panelStars";
            this.panelStars.ScrollBarWidth = 20;
            this.panelStars.Size = new System.Drawing.Size(748, 682);
            this.panelStars.TabIndex = 1;
            this.panelStars.VerticalScrollBarDockRight = true;
            this.panelStars.Click += new System.EventHandler(this.panelStars_Click);
            // 
            // richTextBoxInfo
            // 
            this.richTextBoxInfo.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxInfo.BorderColorScaling = 0.5F;
            this.richTextBoxInfo.HideScrollBar = true;
            this.richTextBoxInfo.Location = new System.Drawing.Point(468, 99);
            this.richTextBoxInfo.Name = "richTextBoxInfo";
            this.richTextBoxInfo.ScrollBarWidth = 20;
            this.richTextBoxInfo.ShowLineCount = false;
            this.richTextBoxInfo.Size = new System.Drawing.Size(200, 100);
            this.richTextBoxInfo.TabIndex = 3;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = false;
            this.vScrollBarCustom.LargeChange = 10;
            this.vScrollBarCustom.Location = new System.Drawing.Point(728, 0);
            this.vScrollBarCustom.Maximum = -479;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 682);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 2;
            this.vScrollBarCustom.Text = "vScrollBarCustom1";
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -479;
            this.vScrollBarCustom.ValueLimited = -479;
            // 
            // UserControlScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStars);
            this.Name = "UserControlScan";
            this.Size = new System.Drawing.Size(748, 682);
            this.Resize += new System.EventHandler(this.UserControlScan_Resize);
            this.panelStars.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.PanelVScroll panelStars;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom;
        private ExtendedControls.RichTextBoxScroll richTextBoxInfo;
    }
}
