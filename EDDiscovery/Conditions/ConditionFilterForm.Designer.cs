namespace EDDiscovery2
{
    partial class ConditionFilterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionFilterForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelOK = new System.Windows.Forms.Panel();
            this.labelProgSet = new System.Windows.Forms.Label();
            this.labelEditProg = new System.Windows.Forms.Label();
            this.panelVScroll = new ExtendedControls.PanelVScroll();
            this.buttonSort = new ExtendedControls.ButtonExt();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.buttonMore = new ExtendedControls.ButtonExt();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.buttonImport = new ExtendedControls.ButtonExt();
            this.buttonExtGlobals = new ExtendedControls.ButtonExt();
            this.checkBoxCustomSetEnabled = new ExtendedControls.CheckBoxCustom();
            this.comboBoxCustomProgSet = new ExtendedControls.ComboBoxCustom();
            this.comboBoxCustomEditProg = new ExtendedControls.ComboBoxCustom();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.statusStripCustom = new ExtendedControls.StatusStripCustom();
            this.panelTop.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(890, 24);
            this.panelTop.TabIndex = 4;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(27, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "N/A";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelVScroll);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(3, 24);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(890, 400);
            this.panelOuter.TabIndex = 8;
            // 
            // panelOK
            // 
            this.panelOK.Controls.Add(this.buttonImport);
            this.panelOK.Controls.Add(this.buttonExtGlobals);
            this.panelOK.Controls.Add(this.checkBoxCustomSetEnabled);
            this.panelOK.Controls.Add(this.comboBoxCustomProgSet);
            this.panelOK.Controls.Add(this.comboBoxCustomEditProg);
            this.panelOK.Controls.Add(this.labelProgSet);
            this.panelOK.Controls.Add(this.labelEditProg);
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(3, 424);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(890, 30);
            this.panelOK.TabIndex = 8;
            // 
            // labelProgSet
            // 
            this.labelProgSet.AutoSize = true;
            this.labelProgSet.Location = new System.Drawing.Point(4, 7);
            this.labelProgSet.Name = "labelProgSet";
            this.labelProgSet.Size = new System.Drawing.Size(23, 13);
            this.labelProgSet.TabIndex = 8;
            this.labelProgSet.Text = "Set";
            // 
            // labelEditProg
            // 
            this.labelEditProg.AutoSize = true;
            this.labelEditProg.Location = new System.Drawing.Point(298, 7);
            this.labelEditProg.Name = "labelEditProg";
            this.labelEditProg.Size = new System.Drawing.Size(46, 13);
            this.labelEditProg.TabIndex = 8;
            this.labelEditProg.Text = "Program";
            // 
            // panelVScroll
            // 
            this.panelVScroll.Controls.Add(this.buttonSort);
            this.panelVScroll.Controls.Add(this.vScrollBarCustom1);
            this.panelVScroll.Controls.Add(this.buttonMore);
            this.panelVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelVScroll.Location = new System.Drawing.Point(0, 0);
            this.panelVScroll.Name = "panelVScroll";
            this.panelVScroll.ScrollBarWidth = 20;
            this.panelVScroll.Size = new System.Drawing.Size(888, 398);
            this.panelVScroll.TabIndex = 8;
            this.panelVScroll.VerticalScrollBarDockRight = true;
            this.panelVScroll.Resize += new System.EventHandler(this.panelVScroll_Resize);
            // 
            // buttonSort
            // 
            this.buttonSort.BorderColorScaling = 1.25F;
            this.buttonSort.ButtonColorScaling = 0.5F;
            this.buttonSort.ButtonDisabledScaling = 0.5F;
            this.buttonSort.Location = new System.Drawing.Point(44, 6);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(45, 23);
            this.buttonSort.TabIndex = 6;
            this.buttonSort.Text = "Sort";
            this.buttonSort.UseVisualStyleBackColor = true;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(868, 0);
            this.vScrollBarCustom1.Maximum = -354;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 398);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -354;
            this.vScrollBarCustom1.ValueLimited = -354;
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
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(867, 0);
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
            this.panel_minimize.Location = new System.Drawing.Point(837, 0);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.BorderColorScaling = 1.25F;
            this.buttonImport.ButtonColorScaling = 0.5F;
            this.buttonImport.ButtonDisabledScaling = 0.5F;
            this.buttonImport.Location = new System.Drawing.Point(615, 4);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 12;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // buttonExtGlobals
            // 
            this.buttonExtGlobals.BorderColorScaling = 1.25F;
            this.buttonExtGlobals.ButtonColorScaling = 0.5F;
            this.buttonExtGlobals.ButtonDisabledScaling = 0.5F;
            this.buttonExtGlobals.Location = new System.Drawing.Point(534, 4);
            this.buttonExtGlobals.Name = "buttonExtGlobals";
            this.buttonExtGlobals.Size = new System.Drawing.Size(75, 23);
            this.buttonExtGlobals.TabIndex = 11;
            this.buttonExtGlobals.Text = "Globals";
            this.buttonExtGlobals.UseVisualStyleBackColor = true;
            this.buttonExtGlobals.Click += new System.EventHandler(this.buttonExtGlobals_Click);
            // 
            // checkBoxCustomSetEnabled
            // 
            this.checkBoxCustomSetEnabled.AutoSize = true;
            this.checkBoxCustomSetEnabled.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomSetEnabled.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomSetEnabled.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomSetEnabled.FontNerfReduction = 0.5F;
            this.checkBoxCustomSetEnabled.Location = new System.Drawing.Point(206, 4);
            this.checkBoxCustomSetEnabled.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomSetEnabled.Name = "checkBoxCustomSetEnabled";
            this.checkBoxCustomSetEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCustomSetEnabled.TabIndex = 10;
            this.checkBoxCustomSetEnabled.Text = "Enabled";
            this.checkBoxCustomSetEnabled.TickBoxReductionSize = 10;
            this.checkBoxCustomSetEnabled.UseVisualStyleBackColor = true;
            // 
            // comboBoxCustomProgSet
            // 
            this.comboBoxCustomProgSet.ArrowWidth = 1;
            this.comboBoxCustomProgSet.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomProgSet.ButtonColorScaling = 0.5F;
            this.comboBoxCustomProgSet.DataSource = null;
            this.comboBoxCustomProgSet.DisplayMember = "";
            this.comboBoxCustomProgSet.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomProgSet.DropDownHeight = 200;
            this.comboBoxCustomProgSet.DropDownWidth = 200;
            this.comboBoxCustomProgSet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomProgSet.ItemHeight = 13;
            this.comboBoxCustomProgSet.Location = new System.Drawing.Point(45, 3);
            this.comboBoxCustomProgSet.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomProgSet.Name = "comboBoxCustomProgSet";
            this.comboBoxCustomProgSet.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomProgSet.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomProgSet.ScrollBarWidth = 16;
            this.comboBoxCustomProgSet.SelectedIndex = -1;
            this.comboBoxCustomProgSet.SelectedItem = null;
            this.comboBoxCustomProgSet.SelectedValue = null;
            this.comboBoxCustomProgSet.Size = new System.Drawing.Size(145, 23);
            this.comboBoxCustomProgSet.TabIndex = 9;
            this.comboBoxCustomProgSet.Text = "comboBoxCustom1";
            this.comboBoxCustomProgSet.ValueMember = "";
            this.comboBoxCustomProgSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomProgSet_SelectedIndexChanged);
            // 
            // comboBoxCustomEditProg
            // 
            this.comboBoxCustomEditProg.ArrowWidth = 1;
            this.comboBoxCustomEditProg.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomEditProg.ButtonColorScaling = 0.5F;
            this.comboBoxCustomEditProg.DataSource = null;
            this.comboBoxCustomEditProg.DisplayMember = "";
            this.comboBoxCustomEditProg.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomEditProg.DropDownHeight = 200;
            this.comboBoxCustomEditProg.DropDownWidth = 200;
            this.comboBoxCustomEditProg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomEditProg.ItemHeight = 13;
            this.comboBoxCustomEditProg.Location = new System.Drawing.Point(371, 3);
            this.comboBoxCustomEditProg.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomEditProg.Name = "comboBoxCustomEditProg";
            this.comboBoxCustomEditProg.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEditProg.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEditProg.ScrollBarWidth = 16;
            this.comboBoxCustomEditProg.SelectedIndex = -1;
            this.comboBoxCustomEditProg.SelectedItem = null;
            this.comboBoxCustomEditProg.SelectedValue = null;
            this.comboBoxCustomEditProg.Size = new System.Drawing.Size(145, 23);
            this.comboBoxCustomEditProg.TabIndex = 9;
            this.comboBoxCustomEditProg.Text = "comboBoxCustom1";
            this.comboBoxCustomEditProg.ValueMember = "";
            this.comboBoxCustomEditProg.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomEditProg_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(714, 4);
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
            this.buttonOK.Location = new System.Drawing.Point(811, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(3, 454);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(890, 22);
            this.statusStripCustom.TabIndex = 27;
            this.statusStripCustom.Text = "statusStripCustom1";
            // 
            // ConditionFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 476);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConditionFilterForm";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JSONFiltersForm";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.panelOK.ResumeLayout(false);
            this.panelOK.PerformLayout();
            this.panelVScroll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonMore;
        private ExtendedControls.DrawnPanel panel_minimize;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.PanelVScroll panelVScroll;
        private ExtendedControls.StatusStripCustom statusStripCustom;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ComboBoxCustom comboBoxCustomEditProg;
        private System.Windows.Forms.Label labelEditProg;
        private ExtendedControls.ComboBoxCustom comboBoxCustomProgSet;
        private System.Windows.Forms.Label labelProgSet;
        private ExtendedControls.CheckBoxCustom checkBoxCustomSetEnabled;
        private ExtendedControls.ButtonExt buttonExtGlobals;
        private ExtendedControls.ButtonExt buttonImport;
        private ExtendedControls.ButtonExt buttonSort;
    }
}