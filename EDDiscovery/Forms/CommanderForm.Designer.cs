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
            this.textBoxBorderCompanionPassword = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCompanionLogin = new ExtendedControls.TextBoxBorder();
            this.checkBoxCustomEDDNTo = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomEDSMFrom = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomEDSMTo = new ExtendedControls.CheckBoxCustom();
            this.textBoxBorderEDSMName = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderEDSMAPI = new ExtendedControls.TextBoxBorder();
            this.buttonExtBrowse = new ExtendedControls.ButtonExt();
            this.textBoxBorderJournal = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCmdr = new ExtendedControls.TextBoxBorder();
            this.groupBoxCustom4 = new ExtendedControls.GroupBoxCustom();
            this.labelCAPIState = new System.Windows.Forms.Label();
            this.labelCAPIPassword = new System.Windows.Forms.Label();
            this.labelCAPILogin = new System.Windows.Forms.Label();
            this.groupBoxCustom3 = new ExtendedControls.GroupBoxCustom();
            this.groupBoxCustom2 = new ExtendedControls.GroupBoxCustom();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxCustom1 = new ExtendedControls.GroupBoxCustom();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxCustom5 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxEGOSync = new ExtendedControls.CheckBoxCustom();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxEGOName = new ExtendedControls.TextBoxBorder();
            this.textBoxEGOAPI = new ExtendedControls.TextBoxBorder();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.groupBoxCustom4.SuspendLayout();
            this.groupBoxCustom3.SuspendLayout();
            this.groupBoxCustom2.SuspendLayout();
            this.groupBoxCustom1.SuspendLayout();
            this.groupBoxCustom5.SuspendLayout();
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
            this.panelTop.Size = new System.Drawing.Size(606, 32);
            this.panelTop.TabIndex = 31;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(583, 0);
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
            this.panel_minimize.Location = new System.Drawing.Point(553, 0);
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
            this.label_index.Size = new System.Drawing.Size(111, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Commander Configure";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 311);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(606, 10);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 387);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(606, 10);
            this.panel2.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 168);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(606, 10);
            this.panel4.TabIndex = 11;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 32);
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
            this.buttonExtCAPI.BorderColorScaling = 1.25F;
            this.buttonExtCAPI.ButtonColorScaling = 0.5F;
            this.buttonExtCAPI.ButtonDisabledScaling = 0.5F;
            this.buttonExtCAPI.Location = new System.Drawing.Point(418, 27);
            this.buttonExtCAPI.Name = "buttonExtCAPI";
            this.buttonExtCAPI.Size = new System.Drawing.Size(102, 23);
            this.buttonExtCAPI.TabIndex = 8;
            this.buttonExtCAPI.Text = "CAPI State";
            this.toolTip1.SetToolTip(this.buttonExtCAPI, "Perform operation indicated");
            this.buttonExtCAPI.UseVisualStyleBackColor = true;
            this.buttonExtCAPI.Click += new System.EventHandler(this.buttonExtCAPI_Click);
            // 
            // textBoxBorderCompanionPassword
            // 
            this.textBoxBorderCompanionPassword.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCompanionPassword.BorderColorScaling = 0.5F;
            this.textBoxBorderCompanionPassword.Location = new System.Drawing.Point(160, 88);
            this.textBoxBorderCompanionPassword.Name = "textBoxBorderCompanionPassword";
            this.textBoxBorderCompanionPassword.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCompanionPassword.TabIndex = 7;
            this.toolTip1.SetToolTip(this.textBoxBorderCompanionPassword, "Enter your Frontier Password");
            this.textBoxBorderCompanionPassword.TextChanged += new System.EventHandler(this.textBoxBorderCompanionPassword_TextChanged);
            // 
            // textBoxBorderCompanionLogin
            // 
            this.textBoxBorderCompanionLogin.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCompanionLogin.BorderColorScaling = 0.5F;
            this.textBoxBorderCompanionLogin.Location = new System.Drawing.Point(160, 58);
            this.textBoxBorderCompanionLogin.Name = "textBoxBorderCompanionLogin";
            this.textBoxBorderCompanionLogin.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCompanionLogin.TabIndex = 5;
            this.toolTip1.SetToolTip(this.textBoxBorderCompanionLogin, "Enter details for API");
            this.textBoxBorderCompanionLogin.TextChanged += new System.EventHandler(this.textBoxBorderCompanionLogin_TextChanged);
            // 
            // checkBoxCustomEDDNTo
            // 
            this.checkBoxCustomEDDNTo.AutoSize = true;
            this.checkBoxCustomEDDNTo.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDDNTo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDDNTo.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDDNTo.FontNerfReduction = 0.5F;
            this.checkBoxCustomEDDNTo.Location = new System.Drawing.Point(9, 29);
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
            // textBoxBorderEDSMName
            // 
            this.textBoxBorderEDSMName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMName.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMName.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderEDSMName.Name = "textBoxBorderEDSMName";
            this.textBoxBorderEDSMName.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMName.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxBorderEDSMName, "Give the name this commander is known as in EDSM");
            // 
            // textBoxBorderEDSMAPI
            // 
            this.textBoxBorderEDSMAPI.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMAPI.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMAPI.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderEDSMAPI.Name = "textBoxBorderEDSMAPI";
            this.textBoxBorderEDSMAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMAPI.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxBorderEDSMAPI, "Enter the API key from the EDSM Website");
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.BorderColorScaling = 1.25F;
            this.buttonExtBrowse.ButtonColorScaling = 0.5F;
            this.buttonExtBrowse.ButtonDisabledScaling = 0.5F;
            this.buttonExtBrowse.Location = new System.Drawing.Point(494, 60);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonExtBrowse.TabIndex = 4;
            this.buttonExtBrowse.Text = "Browse";
            this.toolTip1.SetToolTip(this.buttonExtBrowse, "Browse to the the journal folder");
            this.buttonExtBrowse.UseVisualStyleBackColor = true;
            this.buttonExtBrowse.Click += new System.EventHandler(this.buttonExtBrowse_Click);
            // 
            // textBoxBorderJournal
            // 
            this.textBoxBorderJournal.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderJournal.BorderColorScaling = 0.5F;
            this.textBoxBorderJournal.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderJournal.Name = "textBoxBorderJournal";
            this.textBoxBorderJournal.Size = new System.Drawing.Size(310, 20);
            this.textBoxBorderJournal.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxBorderJournal, "Enter the journal location folder.  Normally leave this field blank, EDDiscovery " +
        "knows where the standard location is");
            // 
            // textBoxBorderCmdr
            // 
            this.textBoxBorderCmdr.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCmdr.BorderColorScaling = 0.5F;
            this.textBoxBorderCmdr.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderCmdr.Name = "textBoxBorderCmdr";
            this.textBoxBorderCmdr.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCmdr.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxBorderCmdr, "Commander Name");
            // 
            // groupBoxCustom4
            // 
            this.groupBoxCustom4.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom4.BackColorScaling = 0.5F;
            this.groupBoxCustom4.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom4.BorderColorScaling = 0.5F;
            this.groupBoxCustom4.Controls.Add(this.labelCAPIState);
            this.groupBoxCustom4.Controls.Add(this.buttonExtCAPI);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorderCompanionPassword);
            this.groupBoxCustom4.Controls.Add(this.labelCAPIPassword);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorderCompanionLogin);
            this.groupBoxCustom4.Controls.Add(this.labelCAPILogin);
            this.groupBoxCustom4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom4.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom4.Location = new System.Drawing.Point(0, 397);
            this.groupBoxCustom4.Name = "groupBoxCustom4";
            this.groupBoxCustom4.Size = new System.Drawing.Size(606, 125);
            this.groupBoxCustom4.TabIndex = 6;
            this.groupBoxCustom4.TabStop = false;
            this.groupBoxCustom4.Text = "Frontier Companion API (optional)";
            this.groupBoxCustom4.TextPadding = 0;
            this.groupBoxCustom4.TextStartPosition = -1;
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
            this.groupBoxCustom3.Location = new System.Drawing.Point(0, 321);
            this.groupBoxCustom3.Name = "groupBoxCustom3";
            this.groupBoxCustom3.Size = new System.Drawing.Size(606, 66);
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
            this.groupBoxCustom2.Controls.Add(this.label1);
            this.groupBoxCustom2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom2.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom2.Location = new System.Drawing.Point(0, 178);
            this.groupBoxCustom2.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustom2.Name = "groupBoxCustom2";
            this.groupBoxCustom2.Size = new System.Drawing.Size(606, 133);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Get an EDSM API key from https://www.edsm.net in \"My account\" menu";
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
            this.groupBoxCustom1.Controls.Add(this.label2);
            this.groupBoxCustom1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom1.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom1.Location = new System.Drawing.Point(0, 42);
            this.groupBoxCustom1.Name = "groupBoxCustom1";
            this.groupBoxCustom1.Size = new System.Drawing.Size(606, 126);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(407, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Leave override journal location blank to use the standard Frontier location for j" +
    "ournals";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(418, 672);
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
            this.buttonOK.BorderColorScaling = 1.25F;
            this.buttonOK.ButtonColorScaling = 0.5F;
            this.buttonOK.ButtonDisabledScaling = 0.5F;
            this.buttonOK.Location = new System.Drawing.Point(519, 672);
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
            this.panel3.Location = new System.Drawing.Point(0, 522);
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
            this.groupBoxCustom5.Controls.Add(this.label9);
            this.groupBoxCustom5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom5.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom5.Location = new System.Drawing.Point(0, 532);
            this.groupBoxCustom5.Margin = new System.Windows.Forms.Padding(10);
            this.groupBoxCustom5.Name = "groupBoxCustom5";
            this.groupBoxCustom5.Size = new System.Drawing.Size(606, 133);
            this.groupBoxCustom5.TabIndex = 33;
            this.groupBoxCustom5.TabStop = false;
            this.groupBoxCustom5.Text = "Elite Galaxy Online Information (optional)";
            this.groupBoxCustom5.TextPadding = 0;
            this.groupBoxCustom5.TextStartPosition = -1;
            // 
            // checkBoxEGOSync
            // 
            this.checkBoxEGOSync.AutoSize = true;
            this.checkBoxEGOSync.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxEGOSync.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEGOSync.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEGOSync.FontNerfReduction = 0.5F;
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "EGO Name:";
            // 
            // textBoxEGOName
            // 
            this.textBoxEGOName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEGOName.BorderColorScaling = 0.5F;
            this.textBoxEGOName.Location = new System.Drawing.Point(160, 30);
            this.textBoxEGOName.Name = "textBoxEGOName";
            this.textBoxEGOName.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOName.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxEGOName, "Give the user name for this commander on EGO");
            // 
            // textBoxEGOAPI
            // 
            this.textBoxEGOAPI.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEGOAPI.BorderColorScaling = 0.5F;
            this.textBoxEGOAPI.Location = new System.Drawing.Point(160, 60);
            this.textBoxEGOAPI.Name = "textBoxEGOAPI";
            this.textBoxEGOAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxEGOAPI.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBoxEGOAPI, "Enter the API key from the EGO Website");
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(394, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Get an EGO API key from https://www.elitegalaxyonline.com in the account menu";
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 711);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.ButtonExt buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Label label9;
    }
}