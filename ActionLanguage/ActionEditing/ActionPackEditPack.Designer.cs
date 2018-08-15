namespace ActionLanguage
{
    partial class ActionPackEditPack 
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
            this.buttonInstallationVars = new ExtendedControls.ButtonExt();
            this.comboBoxCustomEditProg = new ExtendedControls.ComboBoxCustom();
            this.labelEditProg = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelVScroll = new ExtendedControls.PanelVScroll();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.buttonMore = new ExtendedControls.ButtonExt();
            this.statusStripCustom = new ExtendedControls.StatusStripCustom();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripAction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertNewEventAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToGroupAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToGroupBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripCollapse = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveGroupUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveGroupDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.panelVScroll.SuspendLayout();
            this.contextMenuStripAction.SuspendLayout();
            this.contextMenuStripCollapse.SuspendLayout();
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
            this.panelTop.Size = new System.Drawing.Size(1068, 24);
            this.panelTop.TabIndex = 5;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(1045, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(1015, 0);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
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
            this.panelOK.Controls.Add(this.buttonInstallationVars);
            this.panelOK.Controls.Add(this.comboBoxCustomEditProg);
            this.panelOK.Controls.Add(this.labelEditProg);
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(0, 534);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(1068, 30);
            this.panelOK.TabIndex = 9;
            // 
            // buttonInstallationVars
            // 
            this.buttonInstallationVars.BorderColorScaling = 1.25F;
            this.buttonInstallationVars.ButtonColorScaling = 0.5F;
            this.buttonInstallationVars.ButtonDisabledScaling = 0.5F;
            this.buttonInstallationVars.Location = new System.Drawing.Point(246, 3);
            this.buttonInstallationVars.Name = "buttonInstallationVars";
            this.buttonInstallationVars.Size = new System.Drawing.Size(135, 23);
            this.buttonInstallationVars.TabIndex = 6;
            this.buttonInstallationVars.Text = "Install Parameters";
            this.toolTip.SetToolTip(this.buttonInstallationVars, "Set Installation variables for pack (Advanced)");
            this.buttonInstallationVars.UseVisualStyleBackColor = true;
            this.buttonInstallationVars.Click += new System.EventHandler(this.buttonInstallationVars_Click);
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
            this.comboBoxCustomEditProg.Location = new System.Drawing.Point(78, 3);
            this.comboBoxCustomEditProg.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomEditProg.Name = "comboBoxCustomEditProg";
            this.comboBoxCustomEditProg.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEditProg.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEditProg.ScrollBarWidth = 16;
            this.comboBoxCustomEditProg.SelectedIndex = -1;
            this.comboBoxCustomEditProg.SelectedItem = null;
            this.comboBoxCustomEditProg.SelectedValue = null;
            this.comboBoxCustomEditProg.Size = new System.Drawing.Size(144, 21);
            this.comboBoxCustomEditProg.TabIndex = 9;
            this.toolTip.SetToolTip(this.comboBoxCustomEditProg, "Select a program to edit directly");
            this.comboBoxCustomEditProg.ValueMember = "";
            this.comboBoxCustomEditProg.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomEditProg_SelectedIndexChanged);
            // 
            // labelEditProg
            // 
            this.labelEditProg.AutoSize = true;
            this.labelEditProg.Location = new System.Drawing.Point(5, 7);
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
            this.buttonCancel.Location = new System.Drawing.Point(892, 4);
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
            this.buttonOK.Location = new System.Drawing.Point(989, 4);
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
            this.panelOuter.Padding = new System.Windows.Forms.Padding(3);
            this.panelOuter.Size = new System.Drawing.Size(1068, 510);
            this.panelOuter.TabIndex = 10;
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
            this.panelVScroll.Size = new System.Drawing.Size(1060, 502);
            this.panelVScroll.TabIndex = 8;
            this.panelVScroll.VerticalScrollBarDockRight = true;
            this.panelVScroll.Resize += new System.EventHandler(this.panelVScroll_Resize);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(1040, 0);
            this.vScrollBarCustom1.Maximum = -436;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 502);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -436;
            this.vScrollBarCustom1.ValueLimited = -436;
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
            this.toolTip.SetToolTip(this.buttonMore, "Add more events");
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 564);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(1068, 22);
            this.statusStripCustom.TabIndex = 28;
            this.statusStripCustom.Text = "statusStripCustom1";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // contextMenuStripAction
            // 
            this.contextMenuStripAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.moveToGroupAboveToolStripMenuItem,
            this.moveToGroupBelowToolStripMenuItem,
            this.moveGroupUpToolStripMenuItem,
            this.moveGroupDownToolStripMenuItem,
            this.insertNewEventAboveToolStripMenuItem,
            this.insertGroupToolStripMenuItem,
            this.renameGroupToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripAction.Name = "contextMenuStrip1";
            this.contextMenuStripAction.Size = new System.Drawing.Size(207, 246);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveDownToolStripMenuItem.Text = "Move Down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // insertGroupToolStripMenuItem
            // 
            this.insertGroupToolStripMenuItem.Name = "insertGroupToolStripMenuItem";
            this.insertGroupToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.insertGroupToolStripMenuItem.Text = "Insert New  Group Above";
            this.insertGroupToolStripMenuItem.Click += new System.EventHandler(this.insertGroupToolStripMenuItem_Click);
            // 
            // insertNewEventAboveToolStripMenuItem
            // 
            this.insertNewEventAboveToolStripMenuItem.Name = "insertNewEventAboveToolStripMenuItem";
            this.insertNewEventAboveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.insertNewEventAboveToolStripMenuItem.Text = "Insert New Event Above";
            this.insertNewEventAboveToolStripMenuItem.Click += new System.EventHandler(this.insertNewEventAboveToolStripMenuItem_Click);
            // 
            // moveToGroupAboveToolStripMenuItem
            // 
            this.moveToGroupAboveToolStripMenuItem.Name = "moveToGroupAboveToolStripMenuItem";
            this.moveToGroupAboveToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveToGroupAboveToolStripMenuItem.Text = "Move to Group Above";
            this.moveToGroupAboveToolStripMenuItem.Click += new System.EventHandler(this.moveToGroupAboveToolStripMenuItem_Click);
            // 
            // moveToGroupBelowToolStripMenuItem
            // 
            this.moveToGroupBelowToolStripMenuItem.Name = "moveToGroupBelowToolStripMenuItem";
            this.moveToGroupBelowToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveToGroupBelowToolStripMenuItem.Text = "Move to Group Below";
            this.moveToGroupBelowToolStripMenuItem.Click += new System.EventHandler(this.moveToGroupBelowToolStripMenuItem_Click);
            // 
            // contextMenuStripCollapse
            // 
            this.contextMenuStripCollapse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collapseAllToolStripMenuItem,
            this.expandAllToolStripMenuItem});
            this.contextMenuStripCollapse.Name = "contextMenuStrip1";
            this.contextMenuStripCollapse.Size = new System.Drawing.Size(137, 48);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // moveGroupUpToolStripMenuItem
            // 
            this.moveGroupUpToolStripMenuItem.Name = "moveGroupUpToolStripMenuItem";
            this.moveGroupUpToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveGroupUpToolStripMenuItem.Text = "Move Group Up";
            this.moveGroupUpToolStripMenuItem.Click += new System.EventHandler(this.moveGroupUpToolStripMenuItem_Click);
            // 
            // moveGroupDownToolStripMenuItem
            // 
            this.moveGroupDownToolStripMenuItem.Name = "moveGroupDownToolStripMenuItem";
            this.moveGroupDownToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.moveGroupDownToolStripMenuItem.Text = "Move Group Down";
            this.moveGroupDownToolStripMenuItem.Click += new System.EventHandler(this.moveGroupDownToolStripMenuItem_Click);
            // 
            // renameGroupToolStripMenuItem
            // 
            this.renameGroupToolStripMenuItem.Name = "renameGroupToolStripMenuItem";
            this.renameGroupToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.renameGroupToolStripMenuItem.Text = "Rename Group";
            this.renameGroupToolStripMenuItem.Click += new System.EventHandler(this.renameGroupToolStripMenuItem_Click);
            // 
            // ActionPackEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 586);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Name = "ActionPackEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ActionEditor";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.panelOK.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.panelVScroll.ResumeLayout(false);
            this.contextMenuStripAction.ResumeLayout(false);
            this.contextMenuStripCollapse.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelOK;
        private ExtendedControls.ComboBoxCustom comboBoxCustomEditProg;
        private System.Windows.Forms.Label labelEditProg;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonOK;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.PanelVScroll panelVScroll;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.ButtonExt buttonMore;
        private ExtendedControls.StatusStripCustom statusStripCustom;
        private ExtendedControls.ButtonExt buttonInstallationVars;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAction;
        private System.Windows.Forms.ToolStripMenuItem insertGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertNewEventAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToGroupAboveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToGroupBelowToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripCollapse;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveGroupUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveGroupDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameGroupToolStripMenuItem;
    }
}