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
            this.buttonExtSet = new ExtendedControls.ButtonExt();
            this.labelZ = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSelections = new ExtendedControls.ComboBoxCustom();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageViewer
            // 
            this.imageViewer.AutoScroll = true;
            this.imageViewer.AutoSize = false;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 38);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Selection = null;
            this.imageViewer.Size = new System.Drawing.Size(1184, 923);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.ZoomIncrement = 5;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonExtCancel);
            this.panelTop.Controls.Add(this.buttonExtSet);
            this.panelTop.Controls.Add(this.labelZ);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.labelX);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.labelID);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.comboBoxSelections);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1184, 38);
            this.panelTop.TabIndex = 1;
            // 
            // buttonExtSet
            // 
            this.buttonExtSet.Location = new System.Drawing.Point(496, 4);
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
            this.labelZ.Location = new System.Drawing.Point(353, 9);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(10, 13);
            this.labelZ.TabIndex = 1;
            this.labelZ.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Z";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(273, 9);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(10, 13);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(439, 9);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(10, 13);
            this.labelID.TabIndex = 1;
            this.labelID.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(399, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cell";
            // 
            // comboBoxSelections
            // 
            this.comboBoxSelections.ArrowWidth = 1;
            this.comboBoxSelections.BorderColor = System.Drawing.Color.White;
            this.comboBoxSelections.ButtonColorScaling = 0.5F;
            this.comboBoxSelections.DataSource = null;
            this.comboBoxSelections.DisplayMember = "";
            this.comboBoxSelections.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSelections.DropDownHeight = 106;
            this.comboBoxSelections.DropDownWidth = 178;
            this.comboBoxSelections.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSelections.ItemHeight = 13;
            this.comboBoxSelections.Location = new System.Drawing.Point(6, 6);
            this.comboBoxSelections.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSelections.Name = "comboBoxSelections";
            this.comboBoxSelections.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSelections.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSelections.ScrollBarWidth = 16;
            this.comboBoxSelections.SelectedIndex = -1;
            this.comboBoxSelections.SelectedItem = null;
            this.comboBoxSelections.SelectedValue = null;
            this.comboBoxSelections.Size = new System.Drawing.Size(178, 21);
            this.comboBoxSelections.TabIndex = 0;
            this.comboBoxSelections.Text = "comboBoxCustom1";
            this.comboBoxSelections.ValueMember = "";
            this.comboBoxSelections.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelections_SelectedIndexChanged);
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Location = new System.Drawing.Point(586, 4);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 2;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // GalaxySectorSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 961);
            this.Controls.Add(this.imageViewer);
            this.Controls.Add(this.panelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GalaxySectorSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GalaxySectorSelect";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ImageViewerWithGrid imageViewer;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ComboBoxCustom comboBoxSelections;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ButtonExt buttonExtSet;
        private ExtendedControls.ButtonExt buttonExtCancel;
    }
}