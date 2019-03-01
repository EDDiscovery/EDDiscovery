namespace EDDiscovery.Forms
{
    partial class ProfileEditor
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
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelVScrollMain = new ExtendedControls.ExtPanelScroll();
            this.buttonMore = new ExtendedControls.ExtButton();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOK = new System.Windows.Forms.Panel();
            this.buttonExtGlobals = new ExtendedControls.ExtButton();
            this.buttonExtCancel = new ExtendedControls.ExtButton();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.panelOuter.SuspendLayout();
            this.panelVScrollMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelVScrollMain);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 26);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(885, 504);
            this.panelOuter.TabIndex = 0;
            // 
            // panelVScrollMain
            // 
            this.panelVScrollMain.Controls.Add(this.buttonMore);
            this.panelVScrollMain.Controls.Add(this.vScrollBarCustom1);
            this.panelVScrollMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScrollMain.Location = new System.Drawing.Point(0, 0);
            this.panelVScrollMain.Name = "panelVScrollMain";
            this.panelVScrollMain.ScrollBarWidth = 20;
            this.panelVScrollMain.Size = new System.Drawing.Size(883, 502);
            this.panelVScrollMain.TabIndex = 0;
            this.panelVScrollMain.VerticalScrollBarDockRight = true;
            // 
            // buttonMore
            // 
            this.buttonMore.Location = new System.Drawing.Point(12, 6);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(24, 24);
            this.buttonMore.TabIndex = 6;
            this.buttonMore.Text = "+";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
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
            this.vScrollBarCustom1.LargeChange = 10;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(863, 0);
            this.vScrollBarCustom1.Maximum = -462;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 502);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -462;
            this.vScrollBarCustom1.ValueLimited = -462;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(885, 26);
            this.panelTop.TabIndex = 31;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(862, 0);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(832, 0);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 3);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(66, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Profile Editor";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panelOK
            // 
            this.panelOK.Controls.Add(this.buttonExtGlobals);
            this.panelOK.Controls.Add(this.buttonExtCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(0, 530);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(885, 30);
            this.panelOK.TabIndex = 32;
            // 
            // buttonExtGlobals
            // 
            this.buttonExtGlobals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtGlobals.Location = new System.Drawing.Point(-199, 3);
            this.buttonExtGlobals.Name = "buttonExtGlobals";
            this.buttonExtGlobals.Size = new System.Drawing.Size(75, 23);
            this.buttonExtGlobals.TabIndex = 7;
            this.buttonExtGlobals.Text = "Globals";
            this.buttonExtGlobals.UseVisualStyleBackColor = true;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtCancel.Location = new System.Drawing.Point(666, 5);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonExtCancel.TabIndex = 7;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(782, 5);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 560);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(885, 22);
            this.statusStripCustom.TabIndex = 32;
            // 
            // ProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 582);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "ProfileEditor";
            this.Text = "ProfileEditor";
            this.panelOuter.ResumeLayout(false);
            this.panelVScrollMain.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.ExtPanelScroll panelVScrollMain;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ExtButton buttonExtGlobals;
        private ExtendedControls.ExtButton buttonOK;
        private ExtendedControls.ExtButton buttonExtCancel;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private ExtendedControls.ExtButton buttonMore;
    }
}