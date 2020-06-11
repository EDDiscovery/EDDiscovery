namespace EliteDangerousCore.Forms
{ 
    partial class CommanderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommanderForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelI3 = new System.Windows.Forms.Panel();
            this.panelI4 = new System.Windows.Forms.Panel();
            this.panelI2 = new System.Windows.Forms.Panel();
            this.panelI1 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxCustomEDDNTo = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomEDSMFrom = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomEDSMTo = new ExtendedControls.ExtCheckBox();
            this.buttonExtBrowse = new ExtendedControls.ExtButton();
            this.textBoxBorderEDSMName = new ExtendedControls.ExtTextBox();
            this.textBoxBorderEDSMAPI = new ExtendedControls.ExtTextBox();
            this.textBoxBorderJournal = new ExtendedControls.ExtTextBox();
            this.textBoxBorderCmdr = new ExtendedControls.ExtTextBox();
            this.checkBoxCustomInara = new ExtendedControls.ExtCheckBox();
            this.textBoxBorderInaraName = new ExtendedControls.ExtTextBox();
            this.textBoxBorderInaraAPIKey = new ExtendedControls.ExtTextBox();
            this.textBoxDefaultZoom = new ExtendedControls.NumberBoxDouble();
            this.radioButtonHistorySelection = new ExtendedControls.ExtRadioButton();
            this.radioButtonCentreHome = new ExtendedControls.ExtRadioButton();
            this.panel_defaultmapcolor = new ExtendedControls.PanelNoTheme();
            this.checkBoxIGAUSync = new ExtendedControls.ExtCheckBox();
            this.groupBoxCustomEDDN = new ExtendedControls.ExtGroupBox();
            this.groupBoxCustomEDSM = new ExtendedControls.ExtGroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelEDSMAPI = new System.Windows.Forms.Label();
            this.labelEDSMN = new System.Windows.Forms.Label();
            this.groupBoxCustomJournal = new ExtendedControls.ExtGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCN = new System.Windows.Forms.Label();
            this.labelJL = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.groupBoxCustomInara = new ExtendedControls.ExtGroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelINARAN = new System.Windows.Forms.Label();
            this.labelInaraAPI = new System.Windows.Forms.Label();
            this.panelI6 = new System.Windows.Forms.Panel();
            this.panelOK = new System.Windows.Forms.Panel();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.extGroupBoxCommanderInfo = new ExtendedControls.ExtGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelMapCol = new System.Windows.Forms.Label();
            this.extTextBoxAutoCompleteHomeSystem = new ExtendedControls.ExtTextBoxAutoComplete();
            this.HomeSys = new System.Windows.Forms.Label();
            this.labelZoom = new System.Windows.Forms.Label();
            this.labelOpenOn = new System.Windows.Forms.Label();
            this.groupBoxCustomIGAU = new ExtendedControls.ExtGroupBox();
            this.panelTop.SuspendLayout();
            this.groupBoxCustomEDDN.SuspendLayout();
            this.groupBoxCustomEDSM.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxCustomJournal.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxCustomInara.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panelOK.SuspendLayout();
            this.extGroupBoxCommanderInfo.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBoxCustomIGAU.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(615, 27);
            this.panelTop.TabIndex = 31;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(592, 0);
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
            this.panel_minimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(562, 0);
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
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(111, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Commander Configure";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panelI3
            // 
            this.panelI3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelI3.Location = new System.Drawing.Point(0, 195);
            this.panelI3.Name = "panelI3";
            this.panelI3.Size = new System.Drawing.Size(615, 10);
            this.panelI3.TabIndex = 10;
            // 
            // panelI4
            // 
            this.panelI4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelI4.Location = new System.Drawing.Point(0, 269);
            this.panelI4.Name = "panelI4";
            this.panelI4.Size = new System.Drawing.Size(615, 10);
            this.panelI4.TabIndex = 11;
            // 
            // panelI2
            // 
            this.panelI2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelI2.Location = new System.Drawing.Point(0, 114);
            this.panelI2.Name = "panelI2";
            this.panelI2.Size = new System.Drawing.Size(615, 10);
            this.panelI2.TabIndex = 11;
            // 
            // panelI1
            // 
            this.panelI1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelI1.Location = new System.Drawing.Point(0, 27);
            this.panelI1.Name = "panelI1";
            this.panelI1.Size = new System.Drawing.Size(615, 10);
            this.panelI1.TabIndex = 12;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // checkBoxCustomEDDNTo
            // 
            this.checkBoxCustomEDDNTo.AutoSize = true;
            this.checkBoxCustomEDDNTo.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDDNTo.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomEDDNTo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDDNTo.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDDNTo.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDDNTo.ImageIndeterminate = null;
            this.checkBoxCustomEDDNTo.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEDDNTo.ImageUnchecked = null;
            this.checkBoxCustomEDDNTo.Location = new System.Drawing.Point(9, 28);
            this.checkBoxCustomEDDNTo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDDNTo.Name = "checkBoxCustomEDDNTo";
            this.checkBoxCustomEDDNTo.Size = new System.Drawing.Size(183, 17);
            this.checkBoxCustomEDDNTo.TabIndex = 4;
            this.checkBoxCustomEDDNTo.Text = "Send Event Information to EDDN";
            this.checkBoxCustomEDDNTo.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEDDNTo, "Click to send journal information to EDDN. EDDN feeds tools such as EDDB, EDSM, I" +
        "nara with data from commanders. All data is made anonymised");
            this.checkBoxCustomEDDNTo.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomEDSMFrom
            // 
            this.checkBoxCustomEDSMFrom.AutoSize = true;
            this.checkBoxCustomEDSMFrom.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMFrom.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMFrom.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMFrom.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMFrom.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMFrom.ImageIndeterminate = null;
            this.checkBoxCustomEDSMFrom.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEDSMFrom.ImageUnchecked = null;
            this.checkBoxCustomEDSMFrom.Location = new System.Drawing.Point(409, 29);
            this.checkBoxCustomEDSMFrom.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMFrom.Name = "checkBoxCustomEDSMFrom";
            this.checkBoxCustomEDSMFrom.Size = new System.Drawing.Size(110, 17);
            this.checkBoxCustomEDSMFrom.TabIndex = 4;
            this.checkBoxCustomEDSMFrom.Text = "Sync From EDSM";
            this.checkBoxCustomEDSMFrom.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMFrom, "Receive any FSD jumps from EDSM that are on their database but not in EDDiscovery" +
        "");
            this.checkBoxCustomEDSMFrom.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomEDSMTo
            // 
            this.checkBoxCustomEDSMTo.AutoSize = true;
            this.checkBoxCustomEDSMTo.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMTo.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMTo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMTo.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMTo.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMTo.ImageIndeterminate = null;
            this.checkBoxCustomEDSMTo.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEDSMTo.ImageUnchecked = null;
            this.checkBoxCustomEDSMTo.Location = new System.Drawing.Point(409, 3);
            this.checkBoxCustomEDSMTo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMTo.Name = "checkBoxCustomEDSMTo";
            this.checkBoxCustomEDSMTo.Size = new System.Drawing.Size(96, 17);
            this.checkBoxCustomEDSMTo.TabIndex = 4;
            this.checkBoxCustomEDSMTo.Text = "Sync to EDSM";
            this.checkBoxCustomEDSMTo.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMTo, "Send your travel and ship data to EDSM");
            this.checkBoxCustomEDSMTo.UseVisualStyleBackColor = true;
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.Location = new System.Drawing.Point(409, 32);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(100, 23);
            this.buttonExtBrowse.TabIndex = 4;
            this.buttonExtBrowse.Text = "Browse";
            this.toolTip.SetToolTip(this.buttonExtBrowse, "Browse to the the journal folder");
            this.buttonExtBrowse.UseVisualStyleBackColor = true;
            this.buttonExtBrowse.Click += new System.EventHandler(this.buttonExtBrowse_Click);
            // 
            // textBoxBorderEDSMName
            // 
            this.textBoxBorderEDSMName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderEDSMName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderEDSMName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderEDSMName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMName.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderEDSMName.ClearOnFirstChar = false;
            this.textBoxBorderEDSMName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderEDSMName.EndButtonEnable = true;
            this.textBoxBorderEDSMName.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderEDSMName.EndButtonVisible = false;
            this.textBoxBorderEDSMName.InErrorCondition = false;
            this.textBoxBorderEDSMName.Location = new System.Drawing.Point(206, 3);
            this.textBoxBorderEDSMName.Multiline = false;
            this.textBoxBorderEDSMName.Name = "textBoxBorderEDSMName";
            this.textBoxBorderEDSMName.ReadOnly = false;
            this.textBoxBorderEDSMName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMName.SelectionLength = 0;
            this.textBoxBorderEDSMName.SelectionStart = 0;
            this.textBoxBorderEDSMName.Size = new System.Drawing.Size(197, 20);
            this.textBoxBorderEDSMName.TabIndex = 3;
            this.textBoxBorderEDSMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderEDSMName, "Give the name this commander is known as in EDSM");
            this.textBoxBorderEDSMName.WordWrap = true;
            // 
            // textBoxBorderEDSMAPI
            // 
            this.textBoxBorderEDSMAPI.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderEDSMAPI.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderEDSMAPI.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderEDSMAPI.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMAPI.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderEDSMAPI.ClearOnFirstChar = false;
            this.textBoxBorderEDSMAPI.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderEDSMAPI.EndButtonEnable = true;
            this.textBoxBorderEDSMAPI.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderEDSMAPI.EndButtonVisible = false;
            this.textBoxBorderEDSMAPI.InErrorCondition = false;
            this.textBoxBorderEDSMAPI.Location = new System.Drawing.Point(206, 29);
            this.textBoxBorderEDSMAPI.Multiline = false;
            this.textBoxBorderEDSMAPI.Name = "textBoxBorderEDSMAPI";
            this.textBoxBorderEDSMAPI.ReadOnly = false;
            this.textBoxBorderEDSMAPI.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMAPI.SelectionLength = 0;
            this.textBoxBorderEDSMAPI.SelectionStart = 0;
            this.textBoxBorderEDSMAPI.Size = new System.Drawing.Size(197, 20);
            this.textBoxBorderEDSMAPI.TabIndex = 3;
            this.textBoxBorderEDSMAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderEDSMAPI, "Enter the API key from the EDSM Website\\nGet an EDSM API key from https://www.eds" +
        "m.net in \"My account\" menu");
            this.textBoxBorderEDSMAPI.WordWrap = true;
            // 
            // textBoxBorderJournal
            // 
            this.textBoxBorderJournal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderJournal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderJournal.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderJournal.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderJournal.BorderColorScaling = 0.5F;
            this.textBoxBorderJournal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderJournal.ClearOnFirstChar = false;
            this.textBoxBorderJournal.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderJournal.EndButtonEnable = true;
            this.textBoxBorderJournal.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderJournal.EndButtonVisible = false;
            this.textBoxBorderJournal.InErrorCondition = false;
            this.textBoxBorderJournal.Location = new System.Drawing.Point(206, 32);
            this.textBoxBorderJournal.Multiline = false;
            this.textBoxBorderJournal.Name = "textBoxBorderJournal";
            this.textBoxBorderJournal.ReadOnly = false;
            this.textBoxBorderJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderJournal.SelectionLength = 0;
            this.textBoxBorderJournal.SelectionStart = 0;
            this.textBoxBorderJournal.Size = new System.Drawing.Size(197, 20);
            this.textBoxBorderJournal.TabIndex = 3;
            this.textBoxBorderJournal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderJournal, global::EliteDangerous.WinForms.Properties.Resources.Tooltip_JournalLocation);
            this.textBoxBorderJournal.WordWrap = true;
            // 
            // textBoxBorderCmdr
            // 
            this.textBoxBorderCmdr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderCmdr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderCmdr.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderCmdr.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCmdr.BorderColorScaling = 0.5F;
            this.textBoxBorderCmdr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderCmdr.ClearOnFirstChar = false;
            this.textBoxBorderCmdr.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderCmdr.EndButtonEnable = true;
            this.textBoxBorderCmdr.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderCmdr.EndButtonVisible = false;
            this.textBoxBorderCmdr.InErrorCondition = false;
            this.textBoxBorderCmdr.Location = new System.Drawing.Point(206, 3);
            this.textBoxBorderCmdr.Multiline = false;
            this.textBoxBorderCmdr.Name = "textBoxBorderCmdr";
            this.textBoxBorderCmdr.ReadOnly = false;
            this.textBoxBorderCmdr.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderCmdr.SelectionLength = 0;
            this.textBoxBorderCmdr.SelectionStart = 0;
            this.textBoxBorderCmdr.Size = new System.Drawing.Size(197, 20);
            this.textBoxBorderCmdr.TabIndex = 3;
            this.textBoxBorderCmdr.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderCmdr, "Enter commander name as used in Elite Dangerous");
            this.textBoxBorderCmdr.WordWrap = true;
            // 
            // checkBoxCustomInara
            // 
            this.checkBoxCustomInara.AutoSize = true;
            this.checkBoxCustomInara.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomInara.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomInara.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomInara.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomInara.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomInara.ImageIndeterminate = null;
            this.checkBoxCustomInara.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomInara.ImageUnchecked = null;
            this.checkBoxCustomInara.Location = new System.Drawing.Point(409, 3);
            this.checkBoxCustomInara.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomInara.Name = "checkBoxCustomInara";
            this.checkBoxCustomInara.Size = new System.Drawing.Size(89, 17);
            this.checkBoxCustomInara.TabIndex = 4;
            this.checkBoxCustomInara.Text = "Sync to Inara";
            this.checkBoxCustomInara.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomInara, "Sync with Inara");
            this.checkBoxCustomInara.UseVisualStyleBackColor = true;
            // 
            // textBoxBorderInaraName
            // 
            this.textBoxBorderInaraName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderInaraName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderInaraName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderInaraName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderInaraName.BorderColorScaling = 0.5F;
            this.textBoxBorderInaraName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderInaraName.ClearOnFirstChar = false;
            this.textBoxBorderInaraName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderInaraName.EndButtonEnable = true;
            this.textBoxBorderInaraName.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderInaraName.EndButtonVisible = false;
            this.textBoxBorderInaraName.InErrorCondition = false;
            this.textBoxBorderInaraName.Location = new System.Drawing.Point(206, 3);
            this.textBoxBorderInaraName.Multiline = false;
            this.textBoxBorderInaraName.Name = "textBoxBorderInaraName";
            this.textBoxBorderInaraName.ReadOnly = false;
            this.textBoxBorderInaraName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraName.SelectionLength = 0;
            this.textBoxBorderInaraName.SelectionStart = 0;
            this.textBoxBorderInaraName.Size = new System.Drawing.Size(197, 19);
            this.textBoxBorderInaraName.TabIndex = 3;
            this.textBoxBorderInaraName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderInaraName, "Give the user name for this commander on Inara");
            this.textBoxBorderInaraName.WordWrap = true;
            // 
            // textBoxBorderInaraAPIKey
            // 
            this.textBoxBorderInaraAPIKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderInaraAPIKey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderInaraAPIKey.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderInaraAPIKey.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderInaraAPIKey.BorderColorScaling = 0.5F;
            this.textBoxBorderInaraAPIKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderInaraAPIKey.ClearOnFirstChar = false;
            this.textBoxBorderInaraAPIKey.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderInaraAPIKey.EndButtonEnable = true;
            this.textBoxBorderInaraAPIKey.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxBorderInaraAPIKey.EndButtonVisible = false;
            this.textBoxBorderInaraAPIKey.InErrorCondition = false;
            this.textBoxBorderInaraAPIKey.Location = new System.Drawing.Point(206, 28);
            this.textBoxBorderInaraAPIKey.Multiline = false;
            this.textBoxBorderInaraAPIKey.Name = "textBoxBorderInaraAPIKey";
            this.textBoxBorderInaraAPIKey.ReadOnly = false;
            this.textBoxBorderInaraAPIKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraAPIKey.SelectionLength = 0;
            this.textBoxBorderInaraAPIKey.SelectionStart = 0;
            this.textBoxBorderInaraAPIKey.Size = new System.Drawing.Size(197, 20);
            this.textBoxBorderInaraAPIKey.TabIndex = 3;
            this.textBoxBorderInaraAPIKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderInaraAPIKey, "Enter the API key from the Inara Website\\nGet an Inara API key from https://inara" +
        ".cz");
            this.textBoxBorderInaraAPIKey.WordWrap = true;
            // 
            // textBoxDefaultZoom
            // 
            this.textBoxDefaultZoom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxDefaultZoom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxDefaultZoom.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxDefaultZoom.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxDefaultZoom.BorderColorScaling = 0.5F;
            this.textBoxDefaultZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDefaultZoom.ClearOnFirstChar = false;
            this.textBoxDefaultZoom.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxDefaultZoom.DelayBeforeNotification = 0;
            this.textBoxDefaultZoom.EndButtonEnable = true;
            this.textBoxDefaultZoom.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.textBoxDefaultZoom.EndButtonVisible = false;
            this.textBoxDefaultZoom.Format = "0.#######";
            this.textBoxDefaultZoom.InErrorCondition = true;
            this.textBoxDefaultZoom.Location = new System.Drawing.Point(206, 65);
            this.textBoxDefaultZoom.Maximum = 300D;
            this.textBoxDefaultZoom.Minimum = 0.01D;
            this.textBoxDefaultZoom.Multiline = false;
            this.textBoxDefaultZoom.Name = "textBoxDefaultZoom";
            this.textBoxDefaultZoom.ReadOnly = false;
            this.textBoxDefaultZoom.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxDefaultZoom.SelectionLength = 0;
            this.textBoxDefaultZoom.SelectionStart = 0;
            this.textBoxDefaultZoom.Size = new System.Drawing.Size(51, 14);
            this.textBoxDefaultZoom.TabIndex = 11;
            this.textBoxDefaultZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxDefaultZoom, "Set the zoom level of the map. 1 is normal");
            this.textBoxDefaultZoom.Value = 0D;
            this.textBoxDefaultZoom.WordWrap = true;
            // 
            // radioButtonHistorySelection
            // 
            this.radioButtonHistorySelection.AutoSize = true;
            this.radioButtonHistorySelection.Location = new System.Drawing.Point(409, 34);
            this.radioButtonHistorySelection.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonHistorySelection.Name = "radioButtonHistorySelection";
            this.radioButtonHistorySelection.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonHistorySelection.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonHistorySelection.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonHistorySelection.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonHistorySelection.Size = new System.Drawing.Size(126, 17);
            this.radioButtonHistorySelection.TabIndex = 9;
            this.radioButtonHistorySelection.TabStop = true;
            this.radioButtonHistorySelection.Text = "History Grid Selection";
            this.toolTip.SetToolTip(this.radioButtonHistorySelection, "Select history entry as opening location");
            this.radioButtonHistorySelection.UseVisualStyleBackColor = true;
            // 
            // radioButtonCentreHome
            // 
            this.radioButtonCentreHome.AutoSize = true;
            this.radioButtonCentreHome.Location = new System.Drawing.Point(206, 34);
            this.radioButtonCentreHome.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonCentreHome.Name = "radioButtonCentreHome";
            this.radioButtonCentreHome.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonCentreHome.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonCentreHome.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonCentreHome.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonCentreHome.Size = new System.Drawing.Size(90, 17);
            this.radioButtonCentreHome.TabIndex = 8;
            this.radioButtonCentreHome.TabStop = true;
            this.radioButtonCentreHome.Text = "Home System";
            this.toolTip.SetToolTip(this.radioButtonCentreHome, "Select home system as opening location");
            this.radioButtonCentreHome.UseVisualStyleBackColor = true;
            // 
            // panel_defaultmapcolor
            // 
            this.panel_defaultmapcolor.AccessibleDescription = "";
            this.panel_defaultmapcolor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_defaultmapcolor.Location = new System.Drawing.Point(206, 85);
            this.panel_defaultmapcolor.Name = "panel_defaultmapcolor";
            this.panel_defaultmapcolor.Size = new System.Drawing.Size(28, 15);
            this.panel_defaultmapcolor.TabIndex = 12;
            this.panel_defaultmapcolor.Tag = "";
            this.toolTip.SetToolTip(this.panel_defaultmapcolor, "New travel entries get this colour on the map");
            // 
            // checkBoxIGAUSync
            // 
            this.checkBoxIGAUSync.AutoSize = true;
            this.checkBoxIGAUSync.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxIGAUSync.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxIGAUSync.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxIGAUSync.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxIGAUSync.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxIGAUSync.ImageIndeterminate = null;
            this.checkBoxIGAUSync.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxIGAUSync.ImageUnchecked = null;
            this.checkBoxIGAUSync.Location = new System.Drawing.Point(6, 23);
            this.checkBoxIGAUSync.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxIGAUSync.Name = "checkBoxIGAUSync";
            this.checkBoxIGAUSync.Size = new System.Drawing.Size(228, 17);
            this.checkBoxIGAUSync.TabIndex = 38;
            this.checkBoxIGAUSync.Text = "Send Codex Entry Discovery Data to IGAU";
            this.checkBoxIGAUSync.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxIGAUSync, "https://github.com/Elite-IGAU/publications/blob/master/IGAU_Codex.csv");
            this.checkBoxIGAUSync.UseVisualStyleBackColor = true;
            // 
            // groupBoxCustomEDDN
            // 
            this.groupBoxCustomEDDN.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEDDN.AutoSize = true;
            this.groupBoxCustomEDDN.BackColorScaling = 0.5F;
            this.groupBoxCustomEDDN.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDDN.BorderColorScaling = 0.5F;
            this.groupBoxCustomEDDN.Controls.Add(this.checkBoxCustomEDDNTo);
            this.groupBoxCustomEDDN.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomEDDN.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEDDN.Location = new System.Drawing.Point(0, 205);
            this.groupBoxCustomEDDN.Name = "groupBoxCustomEDDN";
            this.groupBoxCustomEDDN.Size = new System.Drawing.Size(615, 64);
            this.groupBoxCustomEDDN.TabIndex = 5;
            this.groupBoxCustomEDDN.TabStop = false;
            this.groupBoxCustomEDDN.Text = "EDDN";
            this.groupBoxCustomEDDN.TextPadding = 0;
            this.groupBoxCustomEDDN.TextStartPosition = -1;
            // 
            // groupBoxCustomEDSM
            // 
            this.groupBoxCustomEDSM.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEDSM.AutoSize = true;
            this.groupBoxCustomEDSM.BackColorScaling = 0.5F;
            this.groupBoxCustomEDSM.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDSM.BorderColorScaling = 0.5F;
            this.groupBoxCustomEDSM.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxCustomEDSM.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomEDSM.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEDSM.Location = new System.Drawing.Point(0, 124);
            this.groupBoxCustomEDSM.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustomEDSM.Name = "groupBoxCustomEDSM";
            this.groupBoxCustomEDSM.Size = new System.Drawing.Size(615, 71);
            this.groupBoxCustomEDSM.TabIndex = 4;
            this.groupBoxCustomEDSM.TabStop = false;
            this.groupBoxCustomEDSM.Text = "EDSM Information (optional)";
            this.groupBoxCustomEDSM.TextPadding = 0;
            this.groupBoxCustomEDSM.TextStartPosition = -1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.checkBoxCustomEDSMFrom, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelEDSMAPI, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelEDSMN, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBoxCustomEDSMTo, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxBorderEDSMAPI, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxBorderEDSMName, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(609, 52);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // labelEDSMAPI
            // 
            this.labelEDSMAPI.AutoSize = true;
            this.labelEDSMAPI.Location = new System.Drawing.Point(3, 26);
            this.labelEDSMAPI.Name = "labelEDSMAPI";
            this.labelEDSMAPI.Size = new System.Drawing.Size(82, 13);
            this.labelEDSMAPI.TabIndex = 2;
            this.labelEDSMAPI.Text = "EDSM API Key:";
            // 
            // labelEDSMN
            // 
            this.labelEDSMN.AutoSize = true;
            this.labelEDSMN.Location = new System.Drawing.Point(3, 0);
            this.labelEDSMN.Name = "labelEDSMN";
            this.labelEDSMN.Size = new System.Drawing.Size(72, 13);
            this.labelEDSMN.TabIndex = 2;
            this.labelEDSMN.Text = "EDSM Name:";
            // 
            // groupBoxCustomJournal
            // 
            this.groupBoxCustomJournal.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomJournal.AutoSize = true;
            this.groupBoxCustomJournal.BackColorScaling = 0.5F;
            this.groupBoxCustomJournal.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomJournal.BorderColorScaling = 0.5F;
            this.groupBoxCustomJournal.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxCustomJournal.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomJournal.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomJournal.Location = new System.Drawing.Point(0, 37);
            this.groupBoxCustomJournal.Name = "groupBoxCustomJournal";
            this.groupBoxCustomJournal.Size = new System.Drawing.Size(615, 77);
            this.groupBoxCustomJournal.TabIndex = 3;
            this.groupBoxCustomJournal.TabStop = false;
            this.groupBoxCustomJournal.Text = "Journal Related Information";
            this.groupBoxCustomJournal.TextPadding = 0;
            this.groupBoxCustomJournal.TextStartPosition = -1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.labelCN, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonExtBrowse, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBorderCmdr, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBorderJournal, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelJL, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(609, 58);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // labelCN
            // 
            this.labelCN.AutoSize = true;
            this.labelCN.Location = new System.Drawing.Point(3, 0);
            this.labelCN.Name = "labelCN";
            this.labelCN.Size = new System.Drawing.Size(97, 13);
            this.labelCN.TabIndex = 2;
            this.labelCN.Text = "Commander Name:";
            // 
            // labelJL
            // 
            this.labelJL.AutoSize = true;
            this.labelJL.Location = new System.Drawing.Point(3, 29);
            this.labelJL.Name = "labelJL";
            this.labelJL.Size = new System.Drawing.Size(88, 13);
            this.labelJL.TabIndex = 2;
            this.labelJL.Text = "Journal Location:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(382, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(503, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBoxCustomInara
            // 
            this.groupBoxCustomInara.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomInara.AutoSize = true;
            this.groupBoxCustomInara.BackColorScaling = 0.5F;
            this.groupBoxCustomInara.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomInara.BorderColorScaling = 0.5F;
            this.groupBoxCustomInara.Controls.Add(this.tableLayoutPanel5);
            this.groupBoxCustomInara.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomInara.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomInara.Location = new System.Drawing.Point(0, 279);
            this.groupBoxCustomInara.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustomInara.Name = "groupBoxCustomInara";
            this.groupBoxCustomInara.Size = new System.Drawing.Size(615, 70);
            this.groupBoxCustomInara.TabIndex = 34;
            this.groupBoxCustomInara.TabStop = false;
            this.groupBoxCustomInara.Text = "Inara Information (optional)";
            this.groupBoxCustomInara.TextPadding = 0;
            this.groupBoxCustomInara.TextStartPosition = -1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Controls.Add(this.labelINARAN, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBoxBorderInaraName, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.labelInaraAPI, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.checkBoxCustomInara, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.textBoxBorderInaraAPIKey, 1, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.01961F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.98039F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(609, 51);
            this.tableLayoutPanel5.TabIndex = 37;
            // 
            // labelINARAN
            // 
            this.labelINARAN.AutoSize = true;
            this.labelINARAN.Location = new System.Drawing.Point(3, 0);
            this.labelINARAN.Name = "labelINARAN";
            this.labelINARAN.Size = new System.Drawing.Size(65, 13);
            this.labelINARAN.TabIndex = 2;
            this.labelINARAN.Text = "Inara Name:";
            // 
            // labelInaraAPI
            // 
            this.labelInaraAPI.AutoSize = true;
            this.labelInaraAPI.Location = new System.Drawing.Point(3, 25);
            this.labelInaraAPI.Name = "labelInaraAPI";
            this.labelInaraAPI.Size = new System.Drawing.Size(75, 13);
            this.labelInaraAPI.TabIndex = 2;
            this.labelInaraAPI.Text = "Inara API Key:";
            // 
            // panelI6
            // 
            this.panelI6.AutoSize = true;
            this.panelI6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelI6.Location = new System.Drawing.Point(0, 279);
            this.panelI6.Name = "panelI6";
            this.panelI6.Size = new System.Drawing.Size(615, 0);
            this.panelI6.TabIndex = 35;
            // 
            // panelOK
            // 
            this.panelOK.AutoSize = true;
            this.panelOK.Controls.Add(this.buttonCancel);
            this.panelOK.Controls.Add(this.buttonOK);
            this.panelOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOK.Location = new System.Drawing.Point(0, 547);
            this.panelOK.Margin = new System.Windows.Forms.Padding(10);
            this.panelOK.Name = "panelOK";
            this.panelOK.Size = new System.Drawing.Size(615, 29);
            this.panelOK.TabIndex = 36;
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 576);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(615, 22);
            this.statusStripCustom.TabIndex = 32;
            // 
            // extGroupBoxCommanderInfo
            // 
            this.extGroupBoxCommanderInfo.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.extGroupBoxCommanderInfo.BackColorScaling = 0.5F;
            this.extGroupBoxCommanderInfo.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBoxCommanderInfo.BorderColorScaling = 0.5F;
            this.extGroupBoxCommanderInfo.Controls.Add(this.tableLayoutPanel4);
            this.extGroupBoxCommanderInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.extGroupBoxCommanderInfo.FillClientAreaWithAlternateColor = false;
            this.extGroupBoxCommanderInfo.Location = new System.Drawing.Point(0, 408);
            this.extGroupBoxCommanderInfo.Name = "extGroupBoxCommanderInfo";
            this.extGroupBoxCommanderInfo.Size = new System.Drawing.Size(615, 122);
            this.extGroupBoxCommanderInfo.TabIndex = 37;
            this.extGroupBoxCommanderInfo.TabStop = false;
            this.extGroupBoxCommanderInfo.Text = "Other";
            this.extGroupBoxCommanderInfo.TextPadding = 0;
            this.extGroupBoxCommanderInfo.TextStartPosition = -1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Controls.Add(this.radioButtonHistorySelection, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.textBoxDefaultZoom, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.labelMapCol, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.radioButtonCentreHome, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.extTextBoxAutoCompleteHomeSystem, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.HomeSys, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelZoom, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.labelOpenOn, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel_defaultmapcolor, 1, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(609, 103);
            this.tableLayoutPanel4.TabIndex = 14;
            // 
            // labelMapCol
            // 
            this.labelMapCol.AutoSize = true;
            this.labelMapCol.Location = new System.Drawing.Point(3, 82);
            this.labelMapCol.Name = "labelMapCol";
            this.labelMapCol.Size = new System.Drawing.Size(92, 13);
            this.labelMapCol.TabIndex = 13;
            this.labelMapCol.Text = "Default Map Color";
            // 
            // extTextBoxAutoCompleteHomeSystem
            // 
            this.extTextBoxAutoCompleteHomeSystem.AutoCompleteCommentMarker = null;
            this.extTextBoxAutoCompleteHomeSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.extTextBoxAutoCompleteHomeSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.extTextBoxAutoCompleteHomeSystem.BackErrorColor = System.Drawing.Color.Red;
            this.extTextBoxAutoCompleteHomeSystem.BorderColor = System.Drawing.Color.Transparent;
            this.extTextBoxAutoCompleteHomeSystem.BorderColorScaling = 0.5F;
            this.extTextBoxAutoCompleteHomeSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.extTextBoxAutoCompleteHomeSystem.ClearOnFirstChar = false;
            this.extTextBoxAutoCompleteHomeSystem.ControlBackground = System.Drawing.SystemColors.Control;
            this.extTextBoxAutoCompleteHomeSystem.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extTextBoxAutoCompleteHomeSystem.DropDownBorderColor = System.Drawing.Color.Green;
            this.extTextBoxAutoCompleteHomeSystem.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.extTextBoxAutoCompleteHomeSystem.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extTextBoxAutoCompleteHomeSystem.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.extTextBoxAutoCompleteHomeSystem.EndButtonEnable = false;
            this.extTextBoxAutoCompleteHomeSystem.EndButtonImage = global::EDDiscovery.Icons.Controls.Dropdown;
            this.extTextBoxAutoCompleteHomeSystem.EndButtonVisible = false;
            this.extTextBoxAutoCompleteHomeSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTextBoxAutoCompleteHomeSystem.InErrorCondition = false;
            this.extTextBoxAutoCompleteHomeSystem.Location = new System.Drawing.Point(206, 3);
            this.extTextBoxAutoCompleteHomeSystem.Multiline = false;
            this.extTextBoxAutoCompleteHomeSystem.Name = "extTextBoxAutoCompleteHomeSystem";
            this.extTextBoxAutoCompleteHomeSystem.ReadOnly = false;
            this.extTextBoxAutoCompleteHomeSystem.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.extTextBoxAutoCompleteHomeSystem.SelectionLength = 0;
            this.extTextBoxAutoCompleteHomeSystem.SelectionStart = 0;
            this.extTextBoxAutoCompleteHomeSystem.Size = new System.Drawing.Size(152, 23);
            this.extTextBoxAutoCompleteHomeSystem.TabIndex = 3;
            this.extTextBoxAutoCompleteHomeSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.extTextBoxAutoCompleteHomeSystem.WordWrap = true;
            // 
            // HomeSys
            // 
            this.HomeSys.AutoSize = true;
            this.HomeSys.Location = new System.Drawing.Point(3, 0);
            this.HomeSys.Name = "HomeSys";
            this.HomeSys.Size = new System.Drawing.Size(75, 13);
            this.HomeSys.TabIndex = 2;
            this.HomeSys.Text = "Home System:";
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new System.Drawing.Point(3, 62);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(71, 13);
            this.labelZoom.TabIndex = 10;
            this.labelZoom.Text = "Default Zoom";
            // 
            // labelOpenOn
            // 
            this.labelOpenOn.AutoSize = true;
            this.labelOpenOn.Location = new System.Drawing.Point(3, 31);
            this.labelOpenOn.Name = "labelOpenOn";
            this.labelOpenOn.Size = new System.Drawing.Size(90, 13);
            this.labelOpenOn.TabIndex = 7;
            this.labelOpenOn.Text = "Open Centred On";
            // 
            // groupBoxCustomIGAU
            // 
            this.groupBoxCustomIGAU.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomIGAU.AutoSize = true;
            this.groupBoxCustomIGAU.BackColorScaling = 0.5F;
            this.groupBoxCustomIGAU.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomIGAU.BorderColorScaling = 0.5F;
            this.groupBoxCustomIGAU.Controls.Add(this.checkBoxIGAUSync);
            this.groupBoxCustomIGAU.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomIGAU.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomIGAU.Location = new System.Drawing.Point(0, 349);
            this.groupBoxCustomIGAU.Name = "groupBoxCustomIGAU";
            this.groupBoxCustomIGAU.Size = new System.Drawing.Size(615, 59);
            this.groupBoxCustomIGAU.TabIndex = 39;
            this.groupBoxCustomIGAU.TabStop = false;
            this.groupBoxCustomIGAU.Text = "Intergalactic Astronomical Union [IGAU]";
            this.groupBoxCustomIGAU.TextPadding = 0;
            this.groupBoxCustomIGAU.TextStartPosition = -1;
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(615, 598);
            this.Controls.Add(this.panelOK);
            this.Controls.Add(this.statusStripCustom);
            this.Controls.Add(this.extGroupBoxCommanderInfo);
            this.Controls.Add(this.groupBoxCustomIGAU);
            this.Controls.Add(this.groupBoxCustomInara);
            this.Controls.Add(this.panelI6);
            this.Controls.Add(this.panelI4);
            this.Controls.Add(this.groupBoxCustomEDDN);
            this.Controls.Add(this.panelI3);
            this.Controls.Add(this.groupBoxCustomEDSM);
            this.Controls.Add(this.panelI2);
            this.Controls.Add(this.groupBoxCustomJournal);
            this.Controls.Add(this.panelI1);
            this.Controls.Add(this.panelTop);
            this.Name = "CommanderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CommanderForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CommanderForm_FormClosed);
            this.Load += new System.EventHandler(this.CommanderForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.groupBoxCustomEDDN.ResumeLayout(false);
            this.groupBoxCustomEDDN.PerformLayout();
            this.groupBoxCustomEDSM.ResumeLayout(false);
            this.groupBoxCustomEDSM.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBoxCustomJournal.ResumeLayout(false);
            this.groupBoxCustomJournal.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxCustomInara.ResumeLayout(false);
            this.groupBoxCustomInara.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panelOK.ResumeLayout(false);
            this.extGroupBoxCommanderInfo.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBoxCustomIGAU.ResumeLayout(false);
            this.groupBoxCustomIGAU.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtButton buttonOK;
        private ExtendedControls.ExtButton buttonCancel;
        private System.Windows.Forms.Label labelCN;
        private System.Windows.Forms.Label labelJL;
        private System.Windows.Forms.Label labelEDSMN;
        private System.Windows.Forms.Label labelEDSMAPI;
        private ExtendedControls.ExtTextBox textBoxBorderJournal;
        private ExtendedControls.ExtTextBox textBoxBorderCmdr;
        private ExtendedControls.ExtTextBox textBoxBorderEDSMAPI;
        private ExtendedControls.ExtTextBox textBoxBorderEDSMName;
        private ExtendedControls.ExtGroupBox groupBoxCustomJournal;
        private ExtendedControls.ExtButton buttonExtBrowse;
        private ExtendedControls.ExtGroupBox groupBoxCustomEDSM;
        private ExtendedControls.ExtGroupBox groupBoxCustomEDDN;
        private ExtendedControls.ExtCheckBox checkBoxCustomEDSMFrom;
        private ExtendedControls.ExtCheckBox checkBoxCustomEDSMTo;
        private ExtendedControls.ExtCheckBox checkBoxCustomEDDNTo;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelI3;
        private System.Windows.Forms.Panel panelI4;
        private System.Windows.Forms.Panel panelI2;
        private System.Windows.Forms.Panel panelI1;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtGroupBox groupBoxCustomInara;
        private ExtendedControls.ExtCheckBox checkBoxCustomInara;
        private System.Windows.Forms.Label labelINARAN;
        private ExtendedControls.ExtTextBox textBoxBorderInaraName;
        private ExtendedControls.ExtTextBox textBoxBorderInaraAPIKey;
        private System.Windows.Forms.Label labelInaraAPI;
        private System.Windows.Forms.Panel panelI6;
        private System.Windows.Forms.Panel panelOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
        private ExtendedControls.ExtGroupBox extGroupBoxCommanderInfo;
        private ExtendedControls.ExtTextBoxAutoComplete extTextBoxAutoCompleteHomeSystem;
        private System.Windows.Forms.Label HomeSys;
        private ExtendedControls.NumberBoxDouble textBoxDefaultZoom;
        private System.Windows.Forms.Label labelZoom;
        private ExtendedControls.ExtRadioButton radioButtonHistorySelection;
        private ExtendedControls.ExtRadioButton radioButtonCentreHome;
        private System.Windows.Forms.Label labelOpenOn;
        private System.Windows.Forms.Label labelMapCol;
        private ExtendedControls.PanelNoTheme panel_defaultmapcolor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private ExtendedControls.ExtGroupBox groupBoxCustomIGAU;
        private ExtendedControls.ExtCheckBox checkBoxIGAUSync;
    }
}