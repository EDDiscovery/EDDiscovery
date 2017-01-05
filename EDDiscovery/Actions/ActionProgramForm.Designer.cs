namespace EDDiscovery.Actions
{
    partial class ActionProgramForm
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
            this.panelVScroll = new ExtendedControls.PanelVScroll();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.buttonMore = new ExtendedControls.ButtonExt();
            this.panelName = new System.Windows.Forms.Panel();
            this.checkBoxCustomRefresh = new ExtendedControls.CheckBoxCustom();
            this.textBoxBorderName = new ExtendedControls.TextBoxBorder();
            this.labelName = new System.Windows.Forms.Label();
            this.statusStripCustom = new ExtendedControls.StatusStripCustom();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOK = new System.Windows.Forms.Panel();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.buttonExtDelete = new ExtendedControls.ButtonExt();
            this.panelOuter.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.panelName.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelVScroll);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(3, 60);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Padding = new System.Windows.Forms.Padding(3);
            this.panelOuter.Size = new System.Drawing.Size(862, 388);
            this.panelOuter.TabIndex = 9;
            // 
            // panelVScroll
            // 
            this.panelVScroll.Controls.Add(this.vScrollBarCustom1);
            this.panelVScroll.Controls.Add(this.buttonMore);
            this.panelVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelVScroll.Location = new System.Drawing.Point(3, 3);
            this.panelVScroll.Name = "panelVScroll";
            this.panelVScroll.ScrollBarWidth = 20;
            this.panelVScroll.Size = new System.Drawing.Size(854, 380);
            this.panelVScroll.TabIndex = 8;
            this.panelVScroll.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(834, 0);
            this.vScrollBarCustom1.Maximum = -336;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 380);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -336;
            this.vScrollBarCustom1.ValueLimited = -336;
            // 
            // buttonMore
            // 
            this.buttonMore.BorderColorScaling = 1.25F;
            this.buttonMore.ButtonColorScaling = 0.5F;
            this.buttonMore.ButtonDisabledScaling = 0.5F;
            this.buttonMore.Location = new System.Drawing.Point(6, 6);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(24, 24);
            this.buttonMore.TabIndex = 5;
            this.buttonMore.Text = "+";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.buttonExtDelete);
            this.panelName.Controls.Add(this.checkBoxCustomRefresh);
            this.panelName.Controls.Add(this.textBoxBorderName);
            this.panelName.Controls.Add(this.labelName);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelName.Location = new System.Drawing.Point(3, 24);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(862, 36);
            this.panelName.TabIndex = 8;
            // 
            // checkBoxCustomRefresh
            // 
            this.checkBoxCustomRefresh.AutoSize = true;
            this.checkBoxCustomRefresh.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomRefresh.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomRefresh.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomRefresh.FontNerfReduction = 0.5F;
            this.checkBoxCustomRefresh.Location = new System.Drawing.Point(224, 4);
            this.checkBoxCustomRefresh.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomRefresh.Name = "checkBoxCustomRefresh";
            this.checkBoxCustomRefresh.Size = new System.Drawing.Size(98, 17);
            this.checkBoxCustomRefresh.TabIndex = 24;
            this.checkBoxCustomRefresh.Text = "Run at Refresh";
            this.checkBoxCustomRefresh.TickBoxReductionSize = 10;
            this.checkBoxCustomRefresh.UseVisualStyleBackColor = true;
            // 
            // textBoxBorderName
            // 
            this.textBoxBorderName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderName.BorderColorScaling = 0.5F;
            this.textBoxBorderName.Location = new System.Drawing.Point(44, 4);
            this.textBoxBorderName.Name = "textBoxBorderName";
            this.textBoxBorderName.Size = new System.Drawing.Size(154, 20);
            this.textBoxBorderName.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 7);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 23;
            this.labelName.Text = "Name";
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(3, 478);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(862, 22);
            this.statusStripCustom.TabIndex = 28;
            this.statusStripCustom.Text = "statusStripCustom1";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(862, 24);
            this.panelTop.TabIndex = 29;
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(839, 0);
            this.panel_close.MarginSize = 6;
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.DrawnImage = null;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.ImageText = null;
            this.panel_minimize.Location = new System.Drawing.Point(809, 0);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(27, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "N/A";
            // 
            // panelOK
            // 
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(3, 448);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(862, 30);
            this.panelOK.TabIndex = 9;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(686, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.BorderColorScaling = 1.25F;
            this.buttonOK.ButtonColorScaling = 0.5F;
            this.buttonOK.ButtonDisabledScaling = 0.5F;
            this.buttonOK.Location = new System.Drawing.Point(783, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonExtDelete
            // 
            this.buttonExtDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtDelete.BorderColorScaling = 1.25F;
            this.buttonExtDelete.ButtonColorScaling = 0.5F;
            this.buttonExtDelete.ButtonDisabledScaling = 0.5F;
            this.buttonExtDelete.Location = new System.Drawing.Point(833, 4);
            this.buttonExtDelete.Name = "buttonExtDelete";
            this.buttonExtDelete.Size = new System.Drawing.Size(25, 23);
            this.buttonExtDelete.TabIndex = 25;
            this.buttonExtDelete.Text = "X";
            this.buttonExtDelete.UseVisualStyleBackColor = true;
            this.buttonExtDelete.Click += new System.EventHandler(this.buttonExtDelete_Click);
            // 
            // ActionProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 500);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelName);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "ActionProgramForm";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ActionProgramForm";
            this.panelOuter.ResumeLayout(false);
            this.panelVScroll.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.PanelVScroll panelVScroll;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.ButtonExt buttonMore;
        private ExtendedControls.StatusStripCustom statusStripCustom;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelName;
        private ExtendedControls.TextBoxBorder textBoxBorderName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.CheckBoxCustom checkBoxCustomRefresh;
        private ExtendedControls.ButtonExt buttonExtDelete;
    }
}