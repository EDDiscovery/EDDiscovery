namespace EDDiscovery.Forms
{
    partial class GalaxySectorSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GalaxySectorSelect));
            this.imageViewer = new EDDiscovery.Forms.ImageViewerWithGrid();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.buttonExtCancel = new ExtendedControls.ExtButton();
            this.buttonExtSet = new ExtendedControls.ExtButton();
            this.labelZ = new System.Windows.Forms.Label();
            this.labelZName = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelXName = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.labelSectorName = new System.Windows.Forms.Label();
            this.comboBoxSelections = new ExtendedControls.ExtComboBox();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageViewer
            // 
            this.imageViewer.AutoScroll = true;
            this.imageViewer.AutoSize = false;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 32);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Selection = null;
            this.imageViewer.Size = new System.Drawing.Size(782, 717);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.ZoomIncrement = 5;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.buttonExtCancel);
            this.panelTop.Controls.Add(this.buttonExtSet);
            this.panelTop.Controls.Add(this.labelZ);
            this.panelTop.Controls.Add(this.labelZName);
            this.panelTop.Controls.Add(this.labelX);
            this.panelTop.Controls.Add(this.labelXName);
            this.panelTop.Controls.Add(this.labelID);
            this.panelTop.Controls.Add(this.labelSectorName);
            this.panelTop.Controls.Add(this.comboBoxSelections);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(782, 32);
            this.panelTop.TabIndex = 1;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(752, 6);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 29;
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
            this.panel_minimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(722, 6);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 28;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Location = new System.Drawing.Point(593, 4);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 2;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // buttonExtSet
            // 
            this.buttonExtSet.Location = new System.Drawing.Point(507, 4);
            this.buttonExtSet.Name = "buttonExtSet";
            this.buttonExtSet.Size = new System.Drawing.Size(75, 23);
            this.buttonExtSet.TabIndex = 2;
            this.buttonExtSet.Text = "Set";
            this.buttonExtSet.UseVisualStyleBackColor = true;
            this.buttonExtSet.Click += new System.EventHandler(this.buttonExtSet_Click);
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(340, 9);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(43, 13);
            this.labelZ.TabIndex = 1;
            this.labelZ.Text = "<code>";
            // 
            // labelZName
            // 
            this.labelZName.AutoSize = true;
            this.labelZName.Location = new System.Drawing.Point(310, 9);
            this.labelZName.Name = "labelZName";
            this.labelZName.Size = new System.Drawing.Size(14, 13);
            this.labelZName.TabIndex = 1;
            this.labelZName.Text = "Z";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(260, 9);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(43, 13);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "<code>";
            // 
            // labelXName
            // 
            this.labelXName.AutoSize = true;
            this.labelXName.Location = new System.Drawing.Point(230, 9);
            this.labelXName.Name = "labelXName";
            this.labelXName.Size = new System.Drawing.Size(14, 13);
            this.labelXName.TabIndex = 1;
            this.labelXName.Text = "X";
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(450, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(43, 13);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "<code>";
            // 
            // labelSectorName
            // 
            this.labelSectorName.AutoSize = true;
            this.labelSectorName.Location = new System.Drawing.Point(390, 9);
            this.labelSectorName.Name = "labelSectorName";
            this.labelSectorName.Size = new System.Drawing.Size(38, 13);
            this.labelSectorName.TabIndex = 1;
            this.labelSectorName.Text = "Sector";
            // 
            // comboBoxSelections
            // 
            this.comboBoxSelections.BorderColor = System.Drawing.Color.White;
            this.comboBoxSelections.ButtonColorScaling = 0.5F;
            this.comboBoxSelections.DataSource = null;
            this.comboBoxSelections.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSelections.DisplayMember = "";
            this.comboBoxSelections.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSelections.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSelections.Location = new System.Drawing.Point(6, 6);
            this.comboBoxSelections.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSelections.Name = "comboBoxSelections";
            this.comboBoxSelections.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSelections.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSelections.SelectedIndex = -1;
            this.comboBoxSelections.SelectedItem = null;
            this.comboBoxSelections.SelectedValue = null;
            this.comboBoxSelections.Size = new System.Drawing.Size(178, 21);
            this.comboBoxSelections.TabIndex = 0;
            this.comboBoxSelections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxSelections.ValueMember = "";
            this.comboBoxSelections.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelections_SelectedIndexChanged);
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStripCustom.Location = new System.Drawing.Point(0, 751);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(784, 22);
            this.statusStripCustom.TabIndex = 33;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.imageViewer);
            this.panelOuter.Controls.Add(this.panelTop);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(784, 751);
            this.panelOuter.TabIndex = 0;
            // 
            // GalaxySectorSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 773);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.statusStripCustom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GalaxySectorSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GalaxySectorSelect";
            this.Resize += new System.EventHandler(this.GalaxySectorSelect_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageViewerWithGrid imageViewer;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtComboBox comboBoxSelections;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label labelZName;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelXName;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Label labelSectorName;
        private ExtendedControls.ExtButton buttonExtSet;
        private ExtendedControls.ExtButton buttonExtCancel;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private System.Windows.Forms.Panel panelOuter;
    }
}