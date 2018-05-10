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
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonExtCAPI = new ExtendedControls.ButtonExt();
            this.checkBoxCustomEDDNTo = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomEDSMFrom = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomEDSMTo = new ExtendedControls.CheckBoxCustom();
            this.buttonExtBrowse = new ExtendedControls.ButtonExt();
            this.checkBoxEGOSync = new ExtendedControls.CheckBoxCustom();
            this.textBoxEGOName = new ExtendedControls.TextBoxBorder();
            this.textBoxEGOAPI = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCompanionPassword = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCompanionLogin = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderEDSMName = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderEDSMAPI = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderJournal = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCmdr = new ExtendedControls.TextBoxBorder();
            this.checkBoxCustomInara = new ExtendedControls.CheckBoxCustom();
            this.textBoxBorderInaraName = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderInaraAPIKey = new ExtendedControls.TextBoxBorder();
            this.groupBoxCustom4 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxCAPIEnable = new ExtendedControls.CheckBoxCustom();
            this.labelCAPIState = new System.Windows.Forms.Label();
            this.labelCAPIPassword = new System.Windows.Forms.Label();
            this.labelCAPILogin = new System.Windows.Forms.Label();
            this.groupBoxCustom3 = new ExtendedControls.GroupBoxCustom();
            this.groupBoxCustom2 = new ExtendedControls.GroupBoxCustom();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBoxCustom1 = new ExtendedControls.GroupBoxCustom();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxCustom5 = new ExtendedControls.GroupBoxCustom();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBoxCustom6 = new ExtendedControls.GroupBoxCustom();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.groupBoxCustom4.SuspendLayout();
            this.groupBoxCustom3.SuspendLayout();
            this.groupBoxCustom2.SuspendLayout();
            this.groupBoxCustom1.SuspendLayout();
            this.groupBoxCustom5.SuspendLayout();
            this.groupBoxCustom6.SuspendLayout();
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
            this.panelTop.Size = new System.Drawing.Size(606, 26);
            this.panelTop.TabIndex = 31;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(583, 0);
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
            this.panel_minimize.Location = new System.Drawing.Point(553, 0);
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
            this.panel1.Size = new System.Drawing.Size(606, 10);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 281);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(606, 10);
            this.panel2.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 130);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(606, 10);
            this.panel4.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 26);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(606, 10);
            this.panel5.TabIndex = 12;
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // buttonExtCAPI
            // 
            this.buttonExtCAPI.Location = new System.Drawing.Point(418, 27);
            this.buttonExtCAPI.Name = "buttonExtCAPI";
            this.buttonExtCAPI.Size = new System.Drawing.Size(102, 23);
            this.buttonExtCAPI.TabIndex = 8;
            this.buttonExtCAPI.Text = "CAPI State";
            this.toolTip1.SetToolTip(this.buttonExtCAPI, "Perform operation indicated");
            this.buttonExtCAPI.UseVisualStyleBackColor = true;
            this.buttonExtCAPI.Click += new System.EventHandler(this.buttonExtCAPI_Click);
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
            this.toolTip1.SetToolTip(this.checkBoxCustomEDDNTo, "Click to send journal information to EDDN. EDDN feeds tools such as EDDB, EDSM, I" +
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
            this.checkBoxCustomEDSMFrom.Location = new System.Drawing.Point(427, 60);
            this.checkBoxCustomEDSMFrom.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMFrom.Name = "checkBoxCustomEDSMFrom";
            this.checkBoxCustomEDSMFrom.Size = new System.Drawing.Size(110, 17);
            this.checkBoxCustomEDSMFrom.TabIndex = 4;
            this.checkBoxCustomEDSMFrom.Text = "Sync From EDSM";
            this.checkBoxCustomEDSMFrom.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxCustomEDSMFrom, "Receive any FSD jumps from EDSM that are on their database but not in EDDiscovery" +
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
            this.checkBoxCustomEDSMTo.Location = new System.Drawing.Point(427, 32);
            this.checkBoxCustomEDSMTo.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMTo.Name = "checkBoxCustomEDSMTo";
            this.checkBoxCustomEDSMTo.Size = new System.Drawing.Size(96, 17);
            this.checkBoxCustomEDSMTo.TabIndex = 4;
            this.checkBoxCustomEDSMTo.Text = "Sync to EDSM";
            this.checkBoxCustomEDSMTo.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxCustomEDSMTo, "Send your travel and ship data to EDSM");
            this.checkBoxCustomEDSMTo.UseVisualStyleBackColor = true;
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.Location = new System.Drawing.Point(494, 60);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonExtBrowse.TabIndex = 4;
            this.buttonExtBrowse.Text = "Browse";
            this.toolTip1.SetToolTip(this.buttonExtBrowse, "Browse to the the journal folder");
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
            this.checkBoxEGOSync.Location = new System.Drawing.Point(427, 32);
            this.checkBoxEGOSync.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEGOSync.Name = "checkBoxEGOSync";
            this.checkBoxEGOSync.Size = new System.Drawing.Size(88, 17);
            this.checkBoxEGOSync.TabIndex = 4;
            this.checkBoxEGOSync.Text = "Sync to EGO";
            this.checkBoxEGOSync.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxEGOSync, "Send your scan data to EGO");
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
            this.textBoxEGOName.Location = new System.Drawing.Point(160, 30);
            this.textBoxEGOName.Multiline = false;
            this.textBoxEGOName.Name = "textBoxEGOName";
            this.textBoxEGOName.ReadOnly = false;
            this.textBoxEGOName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxEGOName.SelectionLength = 0;
            this.textBoxEGOName.SelectionStart = 0;
            this.textBoxEGOName.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOName.TabIndex = 3;
            this.textBoxEGOName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxEGOName, "Give the user name for this commander on EGO");
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
            this.textBoxEGOAPI.Location = new System.Drawing.Point(160, 60);
            this.textBoxEGOAPI.Multiline = false;
            this.textBoxEGOAPI.Name = "textBoxEGOAPI";
            this.textBoxEGOAPI.ReadOnly = false;
            this.textBoxEGOAPI.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxEGOAPI.SelectionLength = 0;
            this.textBoxEGOAPI.SelectionStart = 0;
            this.textBoxEGOAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOAPI.TabIndex = 3;
            this.textBoxEGOAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxEGOAPI, "Enter the API key from the EGO Website\\nGet an EGO API key from https://www.elite" +
        "galaxyonline.com in the account menu");
            this.textBoxEGOAPI.WordWrap = true;
            // 
            // textBoxBorderCompanionPassword
            // 
            this.textBoxBorderCompanionPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderCompanionPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderCompanionPassword.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderCompanionPassword.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCompanionPassword.BorderColorScaling = 0.5F;
            this.textBoxBorderCompanionPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderCompanionPassword.ClearOnFirstChar = false;
            this.textBoxBorderCompanionPassword.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderCompanionPassword.InErrorCondition = false;
            this.textBoxBorderCompanionPassword.Location = new System.Drawing.Point(160, 88);
            this.textBoxBorderCompanionPassword.Multiline = false;
            this.textBoxBorderCompanionPassword.Name = "textBoxBorderCompanionPassword";
            this.textBoxBorderCompanionPassword.ReadOnly = false;
            this.textBoxBorderCompanionPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderCompanionPassword.SelectionLength = 0;
            this.textBoxBorderCompanionPassword.SelectionStart = 0;
            this.textBoxBorderCompanionPassword.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCompanionPassword.TabIndex = 7;
            this.textBoxBorderCompanionPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderCompanionPassword, "Enter your Frontier Password");
            this.textBoxBorderCompanionPassword.WordWrap = true;
            this.textBoxBorderCompanionPassword.TextChanged += new System.EventHandler(this.textBoxBorderCompanionPassword_TextChanged);
            // 
            // textBoxBorderCompanionLogin
            // 
            this.textBoxBorderCompanionLogin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorderCompanionLogin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorderCompanionLogin.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorderCompanionLogin.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCompanionLogin.BorderColorScaling = 0.5F;
            this.textBoxBorderCompanionLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorderCompanionLogin.ClearOnFirstChar = false;
            this.textBoxBorderCompanionLogin.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorderCompanionLogin.InErrorCondition = false;
            this.textBoxBorderCompanionLogin.Location = new System.Drawing.Point(160, 58);
            this.textBoxBorderCompanionLogin.Multiline = false;
            this.textBoxBorderCompanionLogin.Name = "textBoxBorderCompanionLogin";
            this.textBoxBorderCompanionLogin.ReadOnly = false;
            this.textBoxBorderCompanionLogin.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderCompanionLogin.SelectionLength = 0;
            this.textBoxBorderCompanionLogin.SelectionStart = 0;
            this.textBoxBorderCompanionLogin.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCompanionLogin.TabIndex = 5;
            this.textBoxBorderCompanionLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderCompanionLogin, "Enter your Frontier Login ID");
            this.textBoxBorderCompanionLogin.WordWrap = true;
            this.textBoxBorderCompanionLogin.TextChanged += new System.EventHandler(this.textBoxBorderCompanionLogin_TextChanged);
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
            this.textBoxBorderEDSMName.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderEDSMName.Multiline = false;
            this.textBoxBorderEDSMName.Name = "textBoxBorderEDSMName";
            this.textBoxBorderEDSMName.ReadOnly = false;
            this.textBoxBorderEDSMName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMName.SelectionLength = 0;
            this.textBoxBorderEDSMName.SelectionStart = 0;
            this.textBoxBorderEDSMName.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMName.TabIndex = 3;
            this.textBoxBorderEDSMName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderEDSMName, "Give the name this commander is known as in EDSM");
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
            this.textBoxBorderEDSMAPI.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderEDSMAPI.Multiline = false;
            this.textBoxBorderEDSMAPI.Name = "textBoxBorderEDSMAPI";
            this.textBoxBorderEDSMAPI.ReadOnly = false;
            this.textBoxBorderEDSMAPI.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderEDSMAPI.SelectionLength = 0;
            this.textBoxBorderEDSMAPI.SelectionStart = 0;
            this.textBoxBorderEDSMAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMAPI.TabIndex = 3;
            this.textBoxBorderEDSMAPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderEDSMAPI, "Enter the API key from the EDSM Website\\nGet an EDSM API key from https://www.eds" +
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
            this.textBoxBorderJournal.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderJournal.Multiline = false;
            this.textBoxBorderJournal.Name = "textBoxBorderJournal";
            this.textBoxBorderJournal.ReadOnly = false;
            this.textBoxBorderJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderJournal.SelectionLength = 0;
            this.textBoxBorderJournal.SelectionStart = 0;
            this.textBoxBorderJournal.Size = new System.Drawing.Size(310, 20);
            this.textBoxBorderJournal.TabIndex = 3;
            this.textBoxBorderJournal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderJournal, resources.GetString("textBoxBorderJournal.ToolTip"));
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
            this.textBoxBorderCmdr.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderCmdr.Multiline = false;
            this.textBoxBorderCmdr.Name = "textBoxBorderCmdr";
            this.textBoxBorderCmdr.ReadOnly = false;
            this.textBoxBorderCmdr.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderCmdr.SelectionLength = 0;
            this.textBoxBorderCmdr.SelectionStart = 0;
            this.textBoxBorderCmdr.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCmdr.TabIndex = 3;
            this.textBoxBorderCmdr.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderCmdr, "Enter commander name as used in Elite Dangerous");
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
            this.checkBoxCustomInara.Location = new System.Drawing.Point(427, 32);
            this.checkBoxCustomInara.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomInara.Name = "checkBoxCustomInara";
            this.checkBoxCustomInara.Size = new System.Drawing.Size(89, 17);
            this.checkBoxCustomInara.TabIndex = 4;
            this.checkBoxCustomInara.Text = "Sync to Inara";
            this.checkBoxCustomInara.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxCustomInara, "Sync with Inara");
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
            this.textBoxBorderInaraName.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderInaraName.Multiline = false;
            this.textBoxBorderInaraName.Name = "textBoxBorderInaraName";
            this.textBoxBorderInaraName.ReadOnly = false;
            this.textBoxBorderInaraName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraName.SelectionLength = 0;
            this.textBoxBorderInaraName.SelectionStart = 0;
            this.textBoxBorderInaraName.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderInaraName.TabIndex = 3;
            this.textBoxBorderInaraName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderInaraName, "Give the user name for this commander on Inara");
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
            this.textBoxBorderInaraAPIKey.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderInaraAPIKey.Multiline = false;
            this.textBoxBorderInaraAPIKey.Name = "textBoxBorderInaraAPIKey";
            this.textBoxBorderInaraAPIKey.ReadOnly = false;
            this.textBoxBorderInaraAPIKey.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorderInaraAPIKey.SelectionLength = 0;
            this.textBoxBorderInaraAPIKey.SelectionStart = 0;
            this.textBoxBorderInaraAPIKey.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderInaraAPIKey.TabIndex = 3;
            this.textBoxBorderInaraAPIKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxBorderInaraAPIKey, "Enter the API key from the Inara Website\\nGet an Inara API key from https://inara" +
        ".cz");
            this.textBoxBorderInaraAPIKey.WordWrap = true;
            // 
            // groupBoxCustom4
            // 
            this.groupBoxCustom4.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom4.BackColorScaling = 0.5F;
            this.groupBoxCustom4.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom4.BorderColorScaling = 0.5F;
            this.groupBoxCustom4.Controls.Add(this.checkBoxCAPIEnable);
            this.groupBoxCustom4.Controls.Add(this.labelCAPIState);
            this.groupBoxCustom4.Controls.Add(this.buttonExtCAPI);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorderCompanionPassword);
            this.groupBoxCustom4.Controls.Add(this.labelCAPIPassword);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorderCompanionLogin);
            this.groupBoxCustom4.Controls.Add(this.labelCAPILogin);
            this.groupBoxCustom4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom4.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom4.Location = new System.Drawing.Point(0, 291);
            this.groupBoxCustom4.Name = "groupBoxCustom4";
            this.groupBoxCustom4.Size = new System.Drawing.Size(606, 122);
            this.groupBoxCustom4.TabIndex = 6;
            this.groupBoxCustom4.TabStop = false;
            this.groupBoxCustom4.Text = "Frontier Companion API (optional)";
            this.groupBoxCustom4.TextPadding = 0;
            this.groupBoxCustom4.TextStartPosition = -1;
            // 
            // checkBoxCAPIEnable
            // 
            this.checkBoxCAPIEnable.AutoSize = true;
            this.checkBoxCAPIEnable.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCAPIEnable.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCAPIEnable.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCAPIEnable.FontNerfReduction = 0.5F;
            this.checkBoxCAPIEnable.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCAPIEnable.Location = new System.Drawing.Point(418, 70);
            this.checkBoxCAPIEnable.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCAPIEnable.Name = "checkBoxCAPIEnable";
            this.checkBoxCAPIEnable.Size = new System.Drawing.Size(88, 17);
            this.checkBoxCAPIEnable.TabIndex = 4;
            this.checkBoxCAPIEnable.Text = "Enable Login";
            this.checkBoxCAPIEnable.TickBoxReductionSize = 10;
            this.checkBoxCAPIEnable.UseVisualStyleBackColor = true;
            this.checkBoxCAPIEnable.CheckedChanged += new System.EventHandler(this.checkBoxCAPIEnable_CheckedChanged);
            // 
            // labelCAPIState
            // 
            this.labelCAPIState.AutoSize = true;
            this.labelCAPIState.Location = new System.Drawing.Point(6, 28);
            this.labelCAPIState.Name = "labelCAPIState";
            this.labelCAPIState.Size = new System.Drawing.Size(93, 13);
            this.labelCAPIState.TabIndex = 9;
            this.labelCAPIState.Text = "CAPI current state";
            // 
            // labelCAPIPassword
            // 
            this.labelCAPIPassword.AutoSize = true;
            this.labelCAPIPassword.Location = new System.Drawing.Point(6, 88);
            this.labelCAPIPassword.Name = "labelCAPIPassword";
            this.labelCAPIPassword.Size = new System.Drawing.Size(85, 13);
            this.labelCAPIPassword.TabIndex = 6;
            this.labelCAPIPassword.Text = "Login Password:";
            // 
            // labelCAPILogin
            // 
            this.labelCAPILogin.AutoSize = true;
            this.labelCAPILogin.Location = new System.Drawing.Point(6, 58);
            this.labelCAPILogin.Name = "labelCAPILogin";
            this.labelCAPILogin.Size = new System.Drawing.Size(50, 13);
            this.labelCAPILogin.TabIndex = 4;
            this.labelCAPILogin.Text = "Login ID:";
            // 
            // groupBoxCustom3
            // 
            this.groupBoxCustom3.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom3.BackColorScaling = 0.5F;
            this.groupBoxCustom3.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom3.BorderColorScaling = 0.5F;
            this.groupBoxCustom3.Controls.Add(this.checkBoxCustomEDDNTo);
            this.groupBoxCustom3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom3.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom3.Location = new System.Drawing.Point(0, 240);
            this.groupBoxCustom3.Name = "groupBoxCustom3";
            this.groupBoxCustom3.Size = new System.Drawing.Size(606, 41);
            this.groupBoxCustom3.TabIndex = 5;
            this.groupBoxCustom3.TabStop = false;
            this.groupBoxCustom3.Text = "EDDN";
            this.groupBoxCustom3.TextPadding = 0;
            this.groupBoxCustom3.TextStartPosition = -1;
            // 
            // groupBoxCustom2
            // 
            this.groupBoxCustom2.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom2.BackColorScaling = 0.5F;
            this.groupBoxCustom2.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom2.BorderColorScaling = 0.5F;
            this.groupBoxCustom2.Controls.Add(this.checkBoxCustomEDSMFrom);
            this.groupBoxCustom2.Controls.Add(this.checkBoxCustomEDSMTo);
            this.groupBoxCustom2.Controls.Add(this.label5);
            this.groupBoxCustom2.Controls.Add(this.textBoxBorderEDSMName);
            this.groupBoxCustom2.Controls.Add(this.textBoxBorderEDSMAPI);
            this.groupBoxCustom2.Controls.Add(this.label6);
            this.groupBoxCustom2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom2.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom2.Location = new System.Drawing.Point(0, 140);
            this.groupBoxCustom2.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustom2.Name = "groupBoxCustom2";
            this.groupBoxCustom2.Size = new System.Drawing.Size(606, 90);
            this.groupBoxCustom2.TabIndex = 4;
            this.groupBoxCustom2.TabStop = false;
            this.groupBoxCustom2.Text = "EDSM Information (optional)";
            this.groupBoxCustom2.TextPadding = 0;
            this.groupBoxCustom2.TextStartPosition = -1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "EDSM Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "EDSM API Key:";
            // 
            // groupBoxCustom1
            // 
            this.groupBoxCustom1.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom1.BackColorScaling = 0.5F;
            this.groupBoxCustom1.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom1.BorderColorScaling = 0.5F;
            this.groupBoxCustom1.Controls.Add(this.buttonExtBrowse);
            this.groupBoxCustom1.Controls.Add(this.textBoxBorderJournal);
            this.groupBoxCustom1.Controls.Add(this.textBoxBorderCmdr);
            this.groupBoxCustom1.Controls.Add(this.label3);
            this.groupBoxCustom1.Controls.Add(this.label4);
            this.groupBoxCustom1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom1.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom1.Location = new System.Drawing.Point(0, 36);
            this.groupBoxCustom1.Name = "groupBoxCustom1";
            this.groupBoxCustom1.Size = new System.Drawing.Size(606, 94);
            this.groupBoxCustom1.TabIndex = 3;
            this.groupBoxCustom1.TabStop = false;
            this.groupBoxCustom1.Text = "Journal Related Information";
            this.groupBoxCustom1.TextPadding = 0;
            this.groupBoxCustom1.TextStartPosition = -1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Commander Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Journal Location:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(418, 629);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(519, 629);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 413);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(606, 10);
            this.panel3.TabIndex = 32;
            // 
            // groupBoxCustom5
            // 
            this.groupBoxCustom5.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom5.BackColorScaling = 0.5F;
            this.groupBoxCustom5.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom5.BorderColorScaling = 0.5F;
            this.groupBoxCustom5.Controls.Add(this.checkBoxEGOSync);
            this.groupBoxCustom5.Controls.Add(this.label7);
            this.groupBoxCustom5.Controls.Add(this.textBoxEGOName);
            this.groupBoxCustom5.Controls.Add(this.textBoxEGOAPI);
            this.groupBoxCustom5.Controls.Add(this.label8);
            this.groupBoxCustom5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom5.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom5.Location = new System.Drawing.Point(0, 423);
            this.groupBoxCustom5.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustom5.Name = "groupBoxCustom5";
            this.groupBoxCustom5.Size = new System.Drawing.Size(606, 91);
            this.groupBoxCustom5.TabIndex = 33;
            this.groupBoxCustom5.TabStop = false;
            this.groupBoxCustom5.Text = "Elite Galaxy Online Information (optional)";
            this.groupBoxCustom5.TextPadding = 0;
            this.groupBoxCustom5.TextStartPosition = -1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "EGO Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "EGO API Key:";
            // 
            // groupBoxCustom6
            // 
            this.groupBoxCustom6.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom6.BackColorScaling = 0.5F;
            this.groupBoxCustom6.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom6.BorderColorScaling = 0.5F;
            this.groupBoxCustom6.Controls.Add(this.checkBoxCustomInara);
            this.groupBoxCustom6.Controls.Add(this.label10);
            this.groupBoxCustom6.Controls.Add(this.textBoxBorderInaraName);
            this.groupBoxCustom6.Controls.Add(this.textBoxBorderInaraAPIKey);
            this.groupBoxCustom6.Controls.Add(this.label11);
            this.groupBoxCustom6.Controls.Add(this.label12);
            this.groupBoxCustom6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom6.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom6.Location = new System.Drawing.Point(0, 524);
            this.groupBoxCustom6.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustom6.Name = "groupBoxCustom6";
            this.groupBoxCustom6.Size = new System.Drawing.Size(606, 88);
            this.groupBoxCustom6.TabIndex = 34;
            this.groupBoxCustom6.TabStop = false;
            this.groupBoxCustom6.Text = "Inara Information (optional)";
            this.groupBoxCustom6.TextPadding = 0;
            this.groupBoxCustom6.TextStartPosition = -1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Inara Name:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Inara API Key:";
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
            this.panel6.Location = new System.Drawing.Point(0, 514);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(606, 10);
            this.panel6.TabIndex = 35;
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 662);
            this.Controls.Add(this.groupBoxCustom6);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.groupBoxCustom5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBoxCustom4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBoxCustom3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBoxCustom2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.groupBoxCustom1);
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
            this.groupBoxCustom4.ResumeLayout(false);
            this.groupBoxCustom4.PerformLayout();
            this.groupBoxCustom3.ResumeLayout(false);
            this.groupBoxCustom3.PerformLayout();
            this.groupBoxCustom2.ResumeLayout(false);
            this.groupBoxCustom2.PerformLayout();
            this.groupBoxCustom1.ResumeLayout(false);
            this.groupBoxCustom1.PerformLayout();
            this.groupBoxCustom5.ResumeLayout(false);
            this.groupBoxCustom5.PerformLayout();
            this.groupBoxCustom6.ResumeLayout(false);
            this.groupBoxCustom6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.ButtonExt buttonCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private ExtendedControls.TextBoxBorder textBoxBorderJournal;
        private ExtendedControls.TextBoxBorder textBoxBorderCmdr;
        private ExtendedControls.TextBoxBorder textBoxBorderEDSMAPI;
        private ExtendedControls.TextBoxBorder textBoxBorderEDSMName;
        private ExtendedControls.GroupBoxCustom groupBoxCustom1;
        private ExtendedControls.ButtonExt buttonExtBrowse;
        private ExtendedControls.GroupBoxCustom groupBoxCustom2;
        private ExtendedControls.GroupBoxCustom groupBoxCustom3;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDSMFrom;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDSMTo;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDDNTo;
        private ExtendedControls.GroupBoxCustom groupBoxCustom4;
        private ExtendedControls.TextBoxBorder textBoxBorderCompanionPassword;
        private System.Windows.Forms.Label labelCAPIPassword;
        private ExtendedControls.TextBoxBorder textBoxBorderCompanionLogin;
        private System.Windows.Forms.Label labelCAPILogin;
        private ExtendedControls.ButtonExt buttonExtCAPI;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelCAPIState;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel3;
        private ExtendedControls.GroupBoxCustom groupBoxCustom5;
        private ExtendedControls.CheckBoxCustom checkBoxEGOSync;
        private System.Windows.Forms.Label label7;
        private ExtendedControls.TextBoxBorder textBoxEGOName;
        private ExtendedControls.TextBoxBorder textBoxEGOAPI;
        private System.Windows.Forms.Label label8;
        private ExtendedControls.CheckBoxCustom checkBoxCAPIEnable;
        private ExtendedControls.GroupBoxCustom groupBoxCustom6;
        private ExtendedControls.CheckBoxCustom checkBoxCustomInara;
        private System.Windows.Forms.Label label10;
        private ExtendedControls.TextBoxBorder textBoxBorderInaraName;
        private ExtendedControls.TextBoxBorder textBoxBorderInaraAPIKey;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel6;
    }
}