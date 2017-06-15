namespace EDDiscovery.Actions
{
    partial class ActionEditorForm
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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOK = new System.Windows.Forms.Panel();
            this.checkBoxCustomSetEnabled = new ExtendedControls.CheckBoxCustom();
            this.comboBoxCustomEditProg = new ExtendedControls.ComboBoxCustom();
            this.labelEditProg = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelVScroll = new ExtendedControls.PanelVScroll();
            this.buttonSort2 = new ExtendedControls.ButtonExt();
            this.buttonSort = new ExtendedControls.ButtonExt();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.buttonMore = new ExtendedControls.ButtonExt();
            this.contextMenuStripBottom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configureInstallationValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripCustom = new ExtendedControls.StatusStripCustom();
            this.panelTop.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.contextMenuStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1145, 24);
            this.panelTop.TabIndex = 5;
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(1122, 0);
            this.panel_close.MarginSize = 6;
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.DrawnImage = null;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.ImageText = null;
            this.panel_minimize.Location = new System.Drawing.Point(1092, 0);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
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
            this.panelOK.Controls.Add(this.checkBoxCustomSetEnabled);
            this.panelOK.Controls.Add(this.comboBoxCustomEditProg);
            this.panelOK.Controls.Add(this.labelEditProg);
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(0, 562);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(1145, 30);
            this.panelOK.TabIndex = 9;
            // 
            // checkBoxCustomSetEnabled
            // 
            this.checkBoxCustomSetEnabled.AutoSize = true;
            this.checkBoxCustomSetEnabled.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomSetEnabled.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomSetEnabled.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomSetEnabled.FontNerfReduction = 0.5F;
            this.checkBoxCustomSetEnabled.Location = new System.Drawing.Point(7, 7);
            this.checkBoxCustomSetEnabled.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomSetEnabled.Name = "checkBoxCustomSetEnabled";
            this.checkBoxCustomSetEnabled.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCustomSetEnabled.TabIndex = 10;
            this.checkBoxCustomSetEnabled.Text = "Enabled";
            this.checkBoxCustomSetEnabled.TickBoxReductionSize = 10;
            this.checkBoxCustomSetEnabled.UseVisualStyleBackColor = true;
            this.checkBoxCustomSetEnabled.CheckedChanged += new System.EventHandler(this.checkBoxCustomSetEnabled_CheckedChanged);
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
            this.comboBoxCustomEditProg.Location = new System.Drawing.Point(181, 3);
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
            this.comboBoxCustomEditProg.ValueMember = "";
            this.comboBoxCustomEditProg.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomEditProg_SelectedIndexChanged);
            // 
            // labelEditProg
            // 
            this.labelEditProg.AutoSize = true;
            this.labelEditProg.Location = new System.Drawing.Point(108, 7);
            this.labelEditProg.Name = "labelEditProg";
            this.labelEditProg.Size = new System.Drawing.Size(46, 13);
            this.labelEditProg.TabIndex = 8;
            this.labelEditProg.Text = "Program";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(969, 4);
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
            this.buttonOK.Location = new System.Drawing.Point(1066, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelVScroll);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 24);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(1145, 538);
            this.panelOuter.TabIndex = 10;
            // 
            // panelVScroll
            // 
            this.panelVScroll.Controls.Add(this.buttonSort2);
            this.panelVScroll.Controls.Add(this.buttonSort);
            this.panelVScroll.Controls.Add(this.vScrollBarCustom1);
            this.panelVScroll.Controls.Add(this.buttonMore);
            this.panelVScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelVScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelVScroll.Location = new System.Drawing.Point(0, 0);
            this.panelVScroll.Name = "panelVScroll";
            this.panelVScroll.ScrollBarWidth = 20;
            this.panelVScroll.Size = new System.Drawing.Size(1143, 536);
            this.panelVScroll.TabIndex = 8;
            this.panelVScroll.VerticalScrollBarDockRight = true;
            // 
            // buttonSort2
            // 
            this.buttonSort2.BorderColorScaling = 1.25F;
            this.buttonSort2.ButtonColorScaling = 0.5F;
            this.buttonSort2.ButtonDisabledScaling = 0.5F;
            this.buttonSort2.Location = new System.Drawing.Point(95, 7);
            this.buttonSort2.Name = "buttonSort2";
            this.buttonSort2.Size = new System.Drawing.Size(45, 23);
            this.buttonSort2.TabIndex = 6;
            this.buttonSort2.Text = "Sort2";
            this.buttonSort2.UseVisualStyleBackColor = true;
            this.buttonSort2.Click += new System.EventHandler(this.buttonSort2_Click);
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
            this.vScrollBarCustom1.LargeChange = 32;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(1123, 0);
            this.vScrollBarCustom1.Maximum = -470;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 536);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -470;
            this.vScrollBarCustom1.ValueLimited = -470;
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
            // contextMenuStripBottom
            // 
            this.contextMenuStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureInstallationValuesToolStripMenuItem});
            this.contextMenuStripBottom.Name = "contextMenuStripBottom";
            this.contextMenuStripBottom.Size = new System.Drawing.Size(225, 26);
            // 
            // configureInstallationValuesToolStripMenuItem
            // 
            this.configureInstallationValuesToolStripMenuItem.Name = "configureInstallationValuesToolStripMenuItem";
            this.configureInstallationValuesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.configureInstallationValuesToolStripMenuItem.Text = "Configure Installation Values";
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 592);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(1145, 22);
            this.statusStripCustom.TabIndex = 28;
            this.statusStripCustom.Text = "statusStripCustom1";
            // 
            // ActionEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 614);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "ActionEditorForm";
            this.Text = "ActionEditor";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.panelOK.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.panelVScroll.ResumeLayout(false);
            this.contextMenuStripBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.CheckBoxCustom checkBoxCustomSetEnabled;
        private ExtendedControls.ComboBoxCustom comboBoxCustomEditProg;
        private System.Windows.Forms.Label labelEditProg;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonOK;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.PanelVScroll panelVScroll;
        private ExtendedControls.ButtonExt buttonSort2;
        private ExtendedControls.ButtonExt buttonSort;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.ButtonExt buttonMore;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBottom;
        private System.Windows.Forms.ToolStripMenuItem configureInstallationValuesToolStripMenuItem;
        private ExtendedControls.StatusStripCustom statusStripCustom;
    }
}