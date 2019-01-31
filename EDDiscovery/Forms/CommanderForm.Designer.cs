namespace EDDiscovery.Forms
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
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxCustomEDDNTo = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomEDSMFrom = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomEDSMTo = new ExtendedControls.ExtCheckBox();
            this.buttonExtBrowse = new ExtendedControls.ExtButton();
            this.checkBoxEGOSync = new ExtendedControls.ExtCheckBox();
            this.textBoxEGOName = new ExtendedControls.ExtTextBox();
            this.textBoxEGOAPI = new ExtendedControls.ExtTextBox();
            this.textBoxBorderEDSMName = new ExtendedControls.ExtTextBox();
            this.textBoxBorderEDSMAPI = new ExtendedControls.ExtTextBox();
            this.textBoxBorderJournal = new ExtendedControls.ExtTextBox();
            this.textBoxBorderCmdr = new ExtendedControls.ExtTextBox();
            this.checkBoxCustomInara = new ExtendedControls.ExtCheckBox();
            this.textBoxBorderInaraName = new ExtendedControls.ExtTextBox();
            this.textBoxBorderInaraAPIKey = new ExtendedControls.ExtTextBox();
            this.groupBoxCustomCAPI = new ExtendedControls.ExtGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxCustomEDDN = new ExtendedControls.ExtGroupBox();
            this.groupBoxCustomEDSM = new ExtendedControls.ExtGroupBox();
            this.labelEDSMN = new System.Windows.Forms.Label();
            this.labelEDSMAPI = new System.Windows.Forms.Label();
            this.groupBoxCustomJournal = new ExtendedControls.ExtGroupBox();
            this.labelCN = new System.Windows.Forms.Label();
            this.labelJL = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxCustomEGO = new ExtendedControls.ExtGroupBox();
            this.labelEGON = new System.Windows.Forms.Label();
            this.labelEGOAPI = new System.Windows.Forms.Label();
            this.groupBoxCustomInara = new ExtendedControls.ExtGroupBox();
            this.labelINARAN = new System.Windows.Forms.Label();
            this.labelInaraAPI = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.groupBoxCustomCAPI.SuspendLayout();
            this.groupBoxCustomEDDN.SuspendLayout();
            this.groupBoxCustomEDSM.SuspendLayout();
            this.groupBoxCustomJournal.SuspendLayout();
            this.groupBoxCustomEGO.SuspendLayout();
            this.groupBoxCustomInara.SuspendLayout();
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
            this.panelTop.Size = new System.Drawing.Size(652, 26);
            this.panelTop.TabIndex = 31;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(629, 0);
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
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(599, 0);
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
            this.label_index.Size = new System.Drawing.Size(111, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Commander Configure";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 230);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 10);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 281);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(652, 10);
            this.panel2.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 130);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(652, 10);
            this.panel4.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 26);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(652, 10);
            this.panel5.TabIndex = 12;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // checkBoxCustomEDDNTo
            // 
            this.checkBoxCustomEDDNTo.AutoSize = true;
            this.checkBoxCustomEDDNTo.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDDNTo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDDNTo.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDDNTo.FontNerfReduction = 0.5F;
            this.checkBoxCustomEDDNTo.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDDNTo.Location = new System.Drawing.Point(9, 18);
            this.checkBoxCustomEDDNTo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDDNTo.Name = "checkBoxCustomEDDNTo";
            this.checkBoxCustomEDDNTo.Size = new System.Drawing.Size(183, 17);
            this.checkBoxCustomEDDNTo.TabIndex = 4;
            this.checkBoxCustomEDDNTo.Text = "Send Event Information to EDDN";
            this.checkBoxCustomEDDNTo.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCustomEDDNTo, "Click to send journal information to EDDN. EDDN feeds tools such as EDDB, EDSM, I" +
        "nara with data from commanders. All data is made anonymised");
            this.checkBoxCustomEDDNTo.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomEDSMFrom
            // 
            this.checkBoxCustomEDSMFrom.AutoSize = true;
            this.checkBoxCustomEDSMFrom.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMFrom.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMFrom.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMFrom.FontNerfReduction = 0.5F;
            this.checkBoxCustomEDSMFrom.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMFrom.Location = new System.Drawing.Point(467, 60);
            this.checkBoxCustomEDSMFrom.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMFrom.Name = "checkBoxCustomEDSMFrom";
            this.checkBoxCustomEDSMFrom.Size = new System.Drawing.Size(110, 17);
            this.checkBoxCustomEDSMFrom.TabIndex = 4;
            this.checkBoxCustomEDSMFrom.Text = "Sync From EDSM";
            this.checkBoxCustomEDSMFrom.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMFrom, "Receive any FSD jumps from EDSM that are on their database but not in EDDiscovery" +
        "");
            this.checkBoxCustomEDSMFrom.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomEDSMTo
            // 
            this.checkBoxCustomEDSMTo.AutoSize = true;
            this.checkBoxCustomEDSMTo.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMTo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMTo.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMTo.FontNerfReduction = 0.5F;
            this.checkBoxCustomEDSMTo.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMTo.Location = new System.Drawing.Point(467, 32);
            this.checkBoxCustomEDSMTo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMTo.Name = "checkBoxCustomEDSMTo";
            this.checkBoxCustomEDSMTo.Size = new System.Drawing.Size(96, 17);
            this.checkBoxCustomEDSMTo.TabIndex = 4;
            this.checkBoxCustomEDSMTo.Text = "Sync to EDSM";
            this.checkBoxCustomEDSMTo.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMTo, "Send your travel and ship data to EDSM");
            this.checkBoxCustomEDSMTo.UseVisualStyleBackColor = true;
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.Location = new System.Drawing.Point(534, 60);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(100, 23);
            this.buttonExtBrowse.TabIndex = 4;
            this.buttonExtBrowse.Text = "Browse";
            this.toolTip.SetToolTip(this.buttonExtBrowse, "Browse to the the journal folder");
            this.buttonExtBrowse.UseVisualStyleBackColor = true;
            this.buttonExtBrowse.Click += new System.EventHandler(this.buttonExtBrowse_Click);
            // 
            // checkBoxEGOSync
            // 
            this.checkBoxEGOSync.AutoSize = true;
            this.checkBoxEGOSync.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxEGOSync.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEGOSync.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEGOSync.FontNerfReduction = 0.5F;
            this.checkBoxEGOSync.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEGOSync.Location = new System.Drawing.Point(467, 32);
            this.checkBoxEGOSync.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEGOSync.Name = "checkBoxEGOSync";
            this.checkBoxEGOSync.Size = new System.Drawing.Size(88, 17);
            this.checkBoxEGOSync.TabIndex = 4;
            this.checkBoxEGOSync.Text = "Sync to EGO";
            this.checkBoxEGOSync.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxEGOSync, "Send your scan data to EGO");
            this.checkBoxEGOSync.UseVisualStyleBackColor = true;
            // 
            // textBoxEGOName
            // 
            this.textBoxEGOName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxEGOName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxEGOName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxEGOName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEGOName.BorderColorScaling = 0.5F;
            this.textBoxEGOName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEGOName.ClearOnFirstChar = false;
            this.textBoxEGOName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxEGOName.InErrorCondition = false;
            this.textBoxEGOName.Location = new System.Drawing.Point(200, 30);
            this.textBoxEGOName.Multiline = false;
            this.textBoxEGOName.Name = "textBoxEGOName";
            this.textBoxEGOName.ReadOnly = false;
            this.textBoxEGOName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxEGOName.SelectionLength = 0;
            this.textBoxEGOName.SelectionStart = 0;
            this.textBoxEGOName.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOName.TabIndex = 3;
            this.textBoxEGOName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxEGOName, "Give the user name for this commander on EGO");
            this.textBoxEGOName.WordWrap = true;
            // 
            // textBoxEGOAPI
            // 
            this.textBoxEGOAPI.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxEGOAPI.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxEGOAPI.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxEGOAPI.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEGOAPI.BorderColorScaling = 0.5F;
            this.textBoxEGOAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEGOAPI.ClearOnFirstChar = false;
            this.textBoxEGOAPI.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxEGOAPI.InErrorCondition = false;
            this.textBoxEGOAPI.Location = new System.Drawing.Point(200, 60);
            this.textBoxEGOAPI.Multiline = false;
            this.textBoxEGOAPI.Name = "textBoxEGOAPI";
            this.textBoxEGOAPI.ReadOnly = false;
            this.textBoxEGOAPI.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxEGOAPI.SelectionLength = 0;
            this.textBoxEGOAPI.SelectionStart = 0;
            this.textBoxEGOAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOAPI.TabIndex = 3;
            this.textBoxEGOAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxEGOAPI, "Enter the API key from the EGO Website\\nGet an EGO API key from https://www.elite" +
        "galaxyonline.com in the account menu");
            this.textBoxEGOAPI.WordWrap = true;
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
            this.textBoxBorderEDSMName.InErrorCondition = false;
            this.textBoxBorderEDSMName.Location = new System.Drawing.Point(200, 30);
            this.textBoxBorderEDSMName.Multiline = false;
            this.textBoxBorderEDSMName.Name = "textBoxBorderEDSMName";
            this.textBoxBorderEDSMName.ReadOnly = false;
            this.textBoxBorderEDSMName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMName.SelectionLength = 0;
            this.textBoxBorderEDSMName.SelectionStart = 0;
            this.textBoxBorderEDSMName.Size = new System.Drawing.Size(231, 20);
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
            this.textBoxBorderEDSMAPI.InErrorCondition = false;
            this.textBoxBorderEDSMAPI.Location = new System.Drawing.Point(200, 60);
            this.textBoxBorderEDSMAPI.Multiline = false;
            this.textBoxBorderEDSMAPI.Name = "textBoxBorderEDSMAPI";
            this.textBoxBorderEDSMAPI.ReadOnly = false;
            this.textBoxBorderEDSMAPI.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMAPI.SelectionLength = 0;
            this.textBoxBorderEDSMAPI.SelectionStart = 0;
            this.textBoxBorderEDSMAPI.Size = new System.Drawing.Size(231, 20);
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
            this.textBoxBorderJournal.InErrorCondition = false;
            this.textBoxBorderJournal.Location = new System.Drawing.Point(200, 60);
            this.textBoxBorderJournal.Multiline = false;
            this.textBoxBorderJournal.Name = "textBoxBorderJournal";
            this.textBoxBorderJournal.ReadOnly = false;
            this.textBoxBorderJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderJournal.SelectionLength = 0;
            this.textBoxBorderJournal.SelectionStart = 0;
            this.textBoxBorderJournal.Size = new System.Drawing.Size(310, 20);
            this.textBoxBorderJournal.TabIndex = 3;
            this.textBoxBorderJournal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderJournal, resources.GetString("textBoxBorderJournal.ToolTip"));
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
            this.textBoxBorderCmdr.InErrorCondition = false;
            this.textBoxBorderCmdr.Location = new System.Drawing.Point(200, 30);
            this.textBoxBorderCmdr.Multiline = false;
            this.textBoxBorderCmdr.Name = "textBoxBorderCmdr";
            this.textBoxBorderCmdr.ReadOnly = false;
            this.textBoxBorderCmdr.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderCmdr.SelectionLength = 0;
            this.textBoxBorderCmdr.SelectionStart = 0;
            this.textBoxBorderCmdr.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCmdr.TabIndex = 3;
            this.textBoxBorderCmdr.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderCmdr, "Enter commander name as used in Elite Dangerous");
            this.textBoxBorderCmdr.WordWrap = true;
            // 
            // checkBoxCustomInara
            // 
            this.checkBoxCustomInara.AutoSize = true;
            this.checkBoxCustomInara.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomInara.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomInara.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomInara.FontNerfReduction = 0.5F;
            this.checkBoxCustomInara.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomInara.Location = new System.Drawing.Point(467, 32);
            this.checkBoxCustomInara.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomInara.Name = "checkBoxCustomInara";
            this.checkBoxCustomInara.Size = new System.Drawing.Size(89, 17);
            this.checkBoxCustomInara.TabIndex = 4;
            this.checkBoxCustomInara.Text = "Sync to Inara";
            this.checkBoxCustomInara.TickBoxReductionSize = 10;
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
            this.textBoxBorderInaraName.InErrorCondition = false;
            this.textBoxBorderInaraName.Location = new System.Drawing.Point(200, 30);
            this.textBoxBorderInaraName.Multiline = false;
            this.textBoxBorderInaraName.Name = "textBoxBorderInaraName";
            this.textBoxBorderInaraName.ReadOnly = false;
            this.textBoxBorderInaraName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraName.SelectionLength = 0;
            this.textBoxBorderInaraName.SelectionStart = 0;
            this.textBoxBorderInaraName.Size = new System.Drawing.Size(231, 20);
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
            this.textBoxBorderInaraAPIKey.InErrorCondition = false;
            this.textBoxBorderInaraAPIKey.Location = new System.Drawing.Point(200, 60);
            this.textBoxBorderInaraAPIKey.Multiline = false;
            this.textBoxBorderInaraAPIKey.Name = "textBoxBorderInaraAPIKey";
            this.textBoxBorderInaraAPIKey.ReadOnly = false;
            this.textBoxBorderInaraAPIKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraAPIKey.SelectionLength = 0;
            this.textBoxBorderInaraAPIKey.SelectionStart = 0;
            this.textBoxBorderInaraAPIKey.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderInaraAPIKey.TabIndex = 3;
            this.textBoxBorderInaraAPIKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxBorderInaraAPIKey, "Enter the API key from the Inara Website\\nGet an Inara API key from https://inara" +
        ".cz");
            this.textBoxBorderInaraAPIKey.WordWrap = true;
            // 
            // groupBoxCustomCAPI
            // 
            this.groupBoxCustomCAPI.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomCAPI.BackColorScaling = 0.5F;
            this.groupBoxCustomCAPI.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomCAPI.BorderColorScaling = 0.5F;
            this.groupBoxCustomCAPI.Controls.Add(this.label1);
            this.groupBoxCustomCAPI.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomCAPI.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomCAPI.Location = new System.Drawing.Point(0, 291);
            this.groupBoxCustomCAPI.Name = "groupBoxCustomCAPI";
            this.groupBoxCustomCAPI.Size = new System.Drawing.Size(652, 57);
            this.groupBoxCustomCAPI.TabIndex = 6;
            this.groupBoxCustomCAPI.TabStop = false;
            this.groupBoxCustomCAPI.Text = "Frontier Companion API (optional)";
            this.groupBoxCustomCAPI.TextPadding = 0;
            this.groupBoxCustomCAPI.TextStartPosition = -1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "CAPI is not supported for the current release";
            // 
            // groupBoxCustomEDDN
            // 
            this.groupBoxCustomEDDN.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEDDN.BackColorScaling = 0.5F;
            this.groupBoxCustomEDDN.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDDN.BorderColorScaling = 0.5F;
            this.groupBoxCustomEDDN.Controls.Add(this.checkBoxCustomEDDNTo);
            this.groupBoxCustomEDDN.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomEDDN.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEDDN.Location = new System.Drawing.Point(0, 240);
            this.groupBoxCustomEDDN.Name = "groupBoxCustomEDDN";
            this.groupBoxCustomEDDN.Size = new System.Drawing.Size(652, 41);
            this.groupBoxCustomEDDN.TabIndex = 5;
            this.groupBoxCustomEDDN.TabStop = false;
            this.groupBoxCustomEDDN.Text = "EDDN";
            this.groupBoxCustomEDDN.TextPadding = 0;
            this.groupBoxCustomEDDN.TextStartPosition = -1;
            // 
            // groupBoxCustomEDSM
            // 
            this.groupBoxCustomEDSM.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEDSM.BackColorScaling = 0.5F;
            this.groupBoxCustomEDSM.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDSM.BorderColorScaling = 0.5F;
            this.groupBoxCustomEDSM.Controls.Add(this.checkBoxCustomEDSMFrom);
            this.groupBoxCustomEDSM.Controls.Add(this.checkBoxCustomEDSMTo);
            this.groupBoxCustomEDSM.Controls.Add(this.labelEDSMN);
            this.groupBoxCustomEDSM.Controls.Add(this.textBoxBorderEDSMName);
            this.groupBoxCustomEDSM.Controls.Add(this.textBoxBorderEDSMAPI);
            this.groupBoxCustomEDSM.Controls.Add(this.labelEDSMAPI);
            this.groupBoxCustomEDSM.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomEDSM.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEDSM.Location = new System.Drawing.Point(0, 140);
            this.groupBoxCustomEDSM.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustomEDSM.Name = "groupBoxCustomEDSM";
            this.groupBoxCustomEDSM.Size = new System.Drawing.Size(652, 90);
            this.groupBoxCustomEDSM.TabIndex = 4;
            this.groupBoxCustomEDSM.TabStop = false;
            this.groupBoxCustomEDSM.Text = "EDSM Information (optional)";
            this.groupBoxCustomEDSM.TextPadding = 0;
            this.groupBoxCustomEDSM.TextStartPosition = -1;
            // 
            // labelEDSMN
            // 
            this.labelEDSMN.AutoSize = true;
            this.labelEDSMN.Location = new System.Drawing.Point(6, 30);
            this.labelEDSMN.Name = "labelEDSMN";
            this.labelEDSMN.Size = new System.Drawing.Size(72, 13);
            this.labelEDSMN.TabIndex = 2;
            this.labelEDSMN.Text = "EDSM Name:";
            // 
            // labelEDSMAPI
            // 
            this.labelEDSMAPI.AutoSize = true;
            this.labelEDSMAPI.Location = new System.Drawing.Point(6, 60);
            this.labelEDSMAPI.Name = "labelEDSMAPI";
            this.labelEDSMAPI.Size = new System.Drawing.Size(82, 13);
            this.labelEDSMAPI.TabIndex = 2;
            this.labelEDSMAPI.Text = "EDSM API Key:";
            // 
            // groupBoxCustomJournal
            // 
            this.groupBoxCustomJournal.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomJournal.BackColorScaling = 0.5F;
            this.groupBoxCustomJournal.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomJournal.BorderColorScaling = 0.5F;
            this.groupBoxCustomJournal.Controls.Add(this.buttonExtBrowse);
            this.groupBoxCustomJournal.Controls.Add(this.textBoxBorderJournal);
            this.groupBoxCustomJournal.Controls.Add(this.textBoxBorderCmdr);
            this.groupBoxCustomJournal.Controls.Add(this.labelCN);
            this.groupBoxCustomJournal.Controls.Add(this.labelJL);
            this.groupBoxCustomJournal.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomJournal.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomJournal.Location = new System.Drawing.Point(0, 36);
            this.groupBoxCustomJournal.Name = "groupBoxCustomJournal";
            this.groupBoxCustomJournal.Size = new System.Drawing.Size(652, 94);
            this.groupBoxCustomJournal.TabIndex = 3;
            this.groupBoxCustomJournal.TabStop = false;
            this.groupBoxCustomJournal.Text = "Journal Related Information";
            this.groupBoxCustomJournal.TextPadding = 0;
            this.groupBoxCustomJournal.TextStartPosition = -1;
            // 
            // labelCN
            // 
            this.labelCN.AutoSize = true;
            this.labelCN.Location = new System.Drawing.Point(6, 30);
            this.labelCN.Name = "labelCN";
            this.labelCN.Size = new System.Drawing.Size(97, 13);
            this.labelCN.TabIndex = 2;
            this.labelCN.Text = "Commander Name:";
            // 
            // labelJL
            // 
            this.labelJL.AutoSize = true;
            this.labelJL.Location = new System.Drawing.Point(6, 60);
            this.labelJL.Name = "labelJL";
            this.labelJL.Size = new System.Drawing.Size(88, 13);
            this.labelJL.TabIndex = 2;
            this.labelJL.Text = "Journal Location:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(434, 568);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(540, 568);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 348);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(652, 10);
            this.panel3.TabIndex = 32;
            // 
            // groupBoxCustomEGO
            // 
            this.groupBoxCustomEGO.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEGO.BackColorScaling = 0.5F;
            this.groupBoxCustomEGO.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEGO.BorderColorScaling = 0.5F;
            this.groupBoxCustomEGO.Controls.Add(this.checkBoxEGOSync);
            this.groupBoxCustomEGO.Controls.Add(this.labelEGON);
            this.groupBoxCustomEGO.Controls.Add(this.textBoxEGOName);
            this.groupBoxCustomEGO.Controls.Add(this.textBoxEGOAPI);
            this.groupBoxCustomEGO.Controls.Add(this.labelEGOAPI);
            this.groupBoxCustomEGO.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomEGO.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEGO.Location = new System.Drawing.Point(0, 358);
            this.groupBoxCustomEGO.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustomEGO.Name = "groupBoxCustomEGO";
            this.groupBoxCustomEGO.Size = new System.Drawing.Size(652, 91);
            this.groupBoxCustomEGO.TabIndex = 33;
            this.groupBoxCustomEGO.TabStop = false;
            this.groupBoxCustomEGO.Text = "Elite Galaxy Online Information (optional)";
            this.groupBoxCustomEGO.TextPadding = 0;
            this.groupBoxCustomEGO.TextStartPosition = -1;
            // 
            // labelEGON
            // 
            this.labelEGON.AutoSize = true;
            this.labelEGON.Location = new System.Drawing.Point(6, 30);
            this.labelEGON.Name = "labelEGON";
            this.labelEGON.Size = new System.Drawing.Size(64, 13);
            this.labelEGON.TabIndex = 2;
            this.labelEGON.Text = "EGO Name:";
            // 
            // labelEGOAPI
            // 
            this.labelEGOAPI.AutoSize = true;
            this.labelEGOAPI.Location = new System.Drawing.Point(6, 60);
            this.labelEGOAPI.Name = "labelEGOAPI";
            this.labelEGOAPI.Size = new System.Drawing.Size(74, 13);
            this.labelEGOAPI.TabIndex = 2;
            this.labelEGOAPI.Text = "EGO API Key:";
            // 
            // groupBoxCustomInara
            // 
            this.groupBoxCustomInara.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomInara.BackColorScaling = 0.5F;
            this.groupBoxCustomInara.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomInara.BorderColorScaling = 0.5F;
            this.groupBoxCustomInara.Controls.Add(this.checkBoxCustomInara);
            this.groupBoxCustomInara.Controls.Add(this.labelINARAN);
            this.groupBoxCustomInara.Controls.Add(this.textBoxBorderInaraName);
            this.groupBoxCustomInara.Controls.Add(this.textBoxBorderInaraAPIKey);
            this.groupBoxCustomInara.Controls.Add(this.labelInaraAPI);
            this.groupBoxCustomInara.Controls.Add(this.label12);
            this.groupBoxCustomInara.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustomInara.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomInara.Location = new System.Drawing.Point(0, 459);
            this.groupBoxCustomInara.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustomInara.Name = "groupBoxCustomInara";
            this.groupBoxCustomInara.Size = new System.Drawing.Size(652, 88);
            this.groupBoxCustomInara.TabIndex = 34;
            this.groupBoxCustomInara.TabStop = false;
            this.groupBoxCustomInara.Text = "Inara Information (optional)";
            this.groupBoxCustomInara.TextPadding = 0;
            this.groupBoxCustomInara.TextStartPosition = -1;
            // 
            // labelINARAN
            // 
            this.labelINARAN.AutoSize = true;
            this.labelINARAN.Location = new System.Drawing.Point(6, 30);
            this.labelINARAN.Name = "labelINARAN";
            this.labelINARAN.Size = new System.Drawing.Size(65, 13);
            this.labelINARAN.TabIndex = 2;
            this.labelINARAN.Text = "Inara Name:";
            // 
            // labelInaraAPI
            // 
            this.labelInaraAPI.AutoSize = true;
            this.labelInaraAPI.Location = new System.Drawing.Point(6, 60);
            this.labelInaraAPI.Name = "labelInaraAPI";
            this.labelInaraAPI.Size = new System.Drawing.Size(75, 13);
            this.labelInaraAPI.TabIndex = 2;
            this.labelInaraAPI.Text = "Inara API Key:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(162, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 13);
            this.label12.TabIndex = 1;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 449);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(652, 10);
            this.panel6.TabIndex = 35;
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 601);
            this.Controls.Add(this.groupBoxCustomInara);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.groupBoxCustomEGO);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBoxCustomCAPI);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBoxCustomEDDN);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBoxCustomEDSM);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.groupBoxCustomJournal);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "CommanderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CommanderForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CommanderForm_FormClosed);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.groupBoxCustomCAPI.ResumeLayout(false);
            this.groupBoxCustomCAPI.PerformLayout();
            this.groupBoxCustomEDDN.ResumeLayout(false);
            this.groupBoxCustomEDDN.PerformLayout();
            this.groupBoxCustomEDSM.ResumeLayout(false);
            this.groupBoxCustomEDSM.PerformLayout();
            this.groupBoxCustomJournal.ResumeLayout(false);
            this.groupBoxCustomJournal.PerformLayout();
            this.groupBoxCustomEGO.ResumeLayout(false);
            this.groupBoxCustomEGO.PerformLayout();
            this.groupBoxCustomInara.ResumeLayout(false);
            this.groupBoxCustomInara.PerformLayout();
            this.ResumeLayout(false);

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
        private ExtendedControls.ExtGroupBox groupBoxCustomCAPI;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panel3;
        private ExtendedControls.ExtGroupBox groupBoxCustomEGO;
        private ExtendedControls.ExtCheckBox checkBoxEGOSync;
        private System.Windows.Forms.Label labelEGON;
        private ExtendedControls.ExtTextBox textBoxEGOName;
        private ExtendedControls.ExtTextBox textBoxEGOAPI;
        private System.Windows.Forms.Label labelEGOAPI;
        private ExtendedControls.ExtGroupBox groupBoxCustomInara;
        private ExtendedControls.ExtCheckBox checkBoxCustomInara;
        private System.Windows.Forms.Label labelINARAN;
        private ExtendedControls.ExtTextBox textBoxBorderInaraName;
        private ExtendedControls.ExtTextBox textBoxBorderInaraAPIKey;
        private System.Windows.Forms.Label labelInaraAPI;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label1;
    }
}