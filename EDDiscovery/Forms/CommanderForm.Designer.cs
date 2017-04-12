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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommanderForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBoxCustom5 = new ExtendedControls.GroupBoxCustom();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxBorderTravelLog = new ExtendedControls.TextBoxBorder();
            this.groupBoxCustom4 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxCustomLoggedIn = new ExtendedControls.CheckBoxCustom();
            this.buttonExtAuthenticate = new ExtendedControls.ButtonExt();
            this.textBoxBorder2 = new ExtendedControls.TextBoxBorder();
            this.textBoxBorder1 = new ExtendedControls.TextBoxBorder();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxBorderCompanionLogin = new ExtendedControls.TextBoxBorder();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBoxCustom3 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxCustomEDDNTo = new ExtendedControls.CheckBoxCustom();
            this.groupBoxCustom2 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxCustomEDSMFrom = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomEDSMTo = new ExtendedControls.CheckBoxCustom();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBorderEDSMName = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderEDSMAPI = new ExtendedControls.TextBoxBorder();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxCustom1 = new ExtendedControls.GroupBoxCustom();
            this.buttonExtBrowseJ = new ExtendedControls.ButtonExt();
            this.textBoxBorderJournal = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderCmdr = new ExtendedControls.TextBoxBorder();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.groupBoxCustom5.SuspendLayout();
            this.groupBoxCustom4.SuspendLayout();
            this.groupBoxCustom3.SuspendLayout();
            this.groupBoxCustom2.SuspendLayout();
            this.groupBoxCustom1.SuspendLayout();
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
            this.panelTop.Size = new System.Drawing.Size(668, 32);
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
            this.panel_close.Location = new System.Drawing.Point(645, 0);
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
            this.panel_minimize.Location = new System.Drawing.Point(615, 0);
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
            this.panel1.Size = new System.Drawing.Size(668, 10);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 387);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(668, 10);
            this.panel2.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 522);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(668, 10);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 168);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(668, 10);
            this.panel4.TabIndex = 11;
            // 
            // groupBoxCustom5
            // 
            this.groupBoxCustom5.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom5.BackColorScaling = 0.5F;
            this.groupBoxCustom5.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom5.BorderColorScaling = 0.5F;
            this.groupBoxCustom5.Controls.Add(this.label7);
            this.groupBoxCustom5.Controls.Add(this.textBoxBorderTravelLog);
            this.groupBoxCustom5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom5.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom5.Location = new System.Drawing.Point(0, 532);
            this.groupBoxCustom5.Name = "groupBoxCustom5";
            this.groupBoxCustom5.Size = new System.Drawing.Size(668, 62);
            this.groupBoxCustom5.TabIndex = 7;
            this.groupBoxCustom5.TabStop = false;
            this.groupBoxCustom5.Text = "Pre 2.2 Travel Log (optional)";
            this.groupBoxCustom5.TextPadding = 0;
            this.groupBoxCustom5.TextStartPosition = -1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Travel Log Location:";
            // 
            // textBoxBorderTravelLog
            // 
            this.textBoxBorderTravelLog.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderTravelLog.BorderColorScaling = 0.5F;
            this.textBoxBorderTravelLog.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderTravelLog.Name = "textBoxBorderTravelLog";
            this.textBoxBorderTravelLog.Size = new System.Drawing.Size(312, 20);
            this.textBoxBorderTravelLog.TabIndex = 5;
            // 
            // groupBoxCustom4
            // 
            this.groupBoxCustom4.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustom4.BackColorScaling = 0.5F;
            this.groupBoxCustom4.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustom4.BorderColorScaling = 0.5F;
            this.groupBoxCustom4.Controls.Add(this.checkBoxCustomLoggedIn);
            this.groupBoxCustom4.Controls.Add(this.buttonExtAuthenticate);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorder2);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorder1);
            this.groupBoxCustom4.Controls.Add(this.label8);
            this.groupBoxCustom4.Controls.Add(this.label12);
            this.groupBoxCustom4.Controls.Add(this.textBoxBorderCompanionLogin);
            this.groupBoxCustom4.Controls.Add(this.label11);
            this.groupBoxCustom4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom4.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom4.Location = new System.Drawing.Point(0, 397);
            this.groupBoxCustom4.Name = "groupBoxCustom4";
            this.groupBoxCustom4.Size = new System.Drawing.Size(668, 125);
            this.groupBoxCustom4.TabIndex = 6;
            this.groupBoxCustom4.TabStop = false;
            this.groupBoxCustom4.Text = "Frontier Companion API (optional)";
            this.groupBoxCustom4.TextPadding = 0;
            this.groupBoxCustom4.TextStartPosition = -1;
            // 
            // checkBoxCustomLoggedIn
            // 
            this.checkBoxCustomLoggedIn.AutoSize = true;
            this.checkBoxCustomLoggedIn.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomLoggedIn.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomLoggedIn.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomLoggedIn.Checked = true;
            this.checkBoxCustomLoggedIn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCustomLoggedIn.FontNerfReduction = 0.5F;
            this.checkBoxCustomLoggedIn.Location = new System.Drawing.Point(417, 32);
            this.checkBoxCustomLoggedIn.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomLoggedIn.Name = "checkBoxCustomLoggedIn";
            this.checkBoxCustomLoggedIn.Size = new System.Drawing.Size(74, 17);
            this.checkBoxCustomLoggedIn.TabIndex = 9;
            this.checkBoxCustomLoggedIn.Text = "Logged In";
            this.checkBoxCustomLoggedIn.TickBoxReductionSize = 10;
            this.checkBoxCustomLoggedIn.UseVisualStyleBackColor = true;
            // 
            // buttonExtAuthenticate
            // 
            this.buttonExtAuthenticate.BorderColorScaling = 1.25F;
            this.buttonExtAuthenticate.ButtonColorScaling = 0.5F;
            this.buttonExtAuthenticate.ButtonDisabledScaling = 0.5F;
            this.buttonExtAuthenticate.Location = new System.Drawing.Point(417, 60);
            this.buttonExtAuthenticate.Name = "buttonExtAuthenticate";
            this.buttonExtAuthenticate.Size = new System.Drawing.Size(75, 23);
            this.buttonExtAuthenticate.TabIndex = 8;
            this.buttonExtAuthenticate.Text = "Authenticate";
            this.buttonExtAuthenticate.UseVisualStyleBackColor = true;
            // 
            // textBoxBorder2
            // 
            this.textBoxBorder2.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder2.BorderColorScaling = 0.5F;
            this.textBoxBorder2.Enabled = false;
            this.textBoxBorder2.Location = new System.Drawing.Point(160, 90);
            this.textBoxBorder2.Name = "textBoxBorder2";
            this.textBoxBorder2.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorder2.TabIndex = 7;
            // 
            // textBoxBorder1
            // 
            this.textBoxBorder1.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder1.BorderColorScaling = 0.5F;
            this.textBoxBorder1.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorder1.Name = "textBoxBorder1";
            this.textBoxBorder1.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorder1.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Confirmation Code:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(85, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Login Password:";
            // 
            // textBoxBorderCompanionLogin
            // 
            this.textBoxBorderCompanionLogin.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCompanionLogin.BorderColorScaling = 0.5F;
            this.textBoxBorderCompanionLogin.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderCompanionLogin.Name = "textBoxBorderCompanionLogin";
            this.textBoxBorderCompanionLogin.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCompanionLogin.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Login ID:";
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
            this.groupBoxCustom3.Size = new System.Drawing.Size(668, 66);
            this.groupBoxCustom3.TabIndex = 5;
            this.groupBoxCustom3.TabStop = false;
            this.groupBoxCustom3.Text = "EDDN";
            this.groupBoxCustom3.TextPadding = 0;
            this.groupBoxCustom3.TextStartPosition = -1;
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
            this.checkBoxCustomEDDNTo.UseVisualStyleBackColor = true;
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
            this.groupBoxCustom2.Size = new System.Drawing.Size(668, 133);
            this.groupBoxCustom2.TabIndex = 4;
            this.groupBoxCustom2.TabStop = false;
            this.groupBoxCustom2.Text = "EDSM Information (optional)";
            this.groupBoxCustom2.TextPadding = 0;
            this.groupBoxCustom2.TextStartPosition = -1;
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
            this.checkBoxCustomEDSMTo.UseVisualStyleBackColor = true;
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
            // textBoxBorderEDSMName
            // 
            this.textBoxBorderEDSMName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMName.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMName.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderEDSMName.Name = "textBoxBorderEDSMName";
            this.textBoxBorderEDSMName.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMName.TabIndex = 3;
            // 
            // textBoxBorderEDSMAPI
            // 
            this.textBoxBorderEDSMAPI.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEDSMAPI.BorderColorScaling = 0.5F;
            this.textBoxBorderEDSMAPI.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderEDSMAPI.Name = "textBoxBorderEDSMAPI";
            this.textBoxBorderEDSMAPI.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderEDSMAPI.TabIndex = 3;
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
            this.groupBoxCustom1.Controls.Add(this.buttonExtBrowseJ);
            this.groupBoxCustom1.Controls.Add(this.textBoxBorderJournal);
            this.groupBoxCustom1.Controls.Add(this.textBoxBorderCmdr);
            this.groupBoxCustom1.Controls.Add(this.label3);
            this.groupBoxCustom1.Controls.Add(this.label4);
            this.groupBoxCustom1.Controls.Add(this.label2);
            this.groupBoxCustom1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCustom1.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustom1.Location = new System.Drawing.Point(0, 42);
            this.groupBoxCustom1.Name = "groupBoxCustom1";
            this.groupBoxCustom1.Size = new System.Drawing.Size(668, 126);
            this.groupBoxCustom1.TabIndex = 3;
            this.groupBoxCustom1.TabStop = false;
            this.groupBoxCustom1.Text = "Journal Related Information";
            this.groupBoxCustom1.TextPadding = 0;
            this.groupBoxCustom1.TextStartPosition = -1;
            // 
            // buttonExtBrowseJ
            // 
            this.buttonExtBrowseJ.BorderColorScaling = 1.25F;
            this.buttonExtBrowseJ.ButtonColorScaling = 0.5F;
            this.buttonExtBrowseJ.ButtonDisabledScaling = 0.5F;
            this.buttonExtBrowseJ.Location = new System.Drawing.Point(494, 60);
            this.buttonExtBrowseJ.Name = "buttonExtBrowseJ";
            this.buttonExtBrowseJ.Size = new System.Drawing.Size(75, 23);
            this.buttonExtBrowseJ.TabIndex = 4;
            this.buttonExtBrowseJ.Text = "Browse";
            this.buttonExtBrowseJ.UseVisualStyleBackColor = true;
            // 
            // textBoxBorderJournal
            // 
            this.textBoxBorderJournal.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderJournal.BorderColorScaling = 0.5F;
            this.textBoxBorderJournal.Location = new System.Drawing.Point(160, 60);
            this.textBoxBorderJournal.Name = "textBoxBorderJournal";
            this.textBoxBorderJournal.Size = new System.Drawing.Size(310, 20);
            this.textBoxBorderJournal.TabIndex = 3;
            // 
            // textBoxBorderCmdr
            // 
            this.textBoxBorderCmdr.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderCmdr.BorderColorScaling = 0.5F;
            this.textBoxBorderCmdr.Location = new System.Drawing.Point(160, 30);
            this.textBoxBorderCmdr.Name = "textBoxBorderCmdr";
            this.textBoxBorderCmdr.Size = new System.Drawing.Size(231, 20);
            this.textBoxBorderCmdr.TabIndex = 3;
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
            this.label2.Size = new System.Drawing.Size(416, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Leave Override Journal Location blank to use the standard Frontier location for j" +
    "ournals";
            // 
            // buttonCancel
            // 
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(480, 604);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.BorderColorScaling = 1.25F;
            this.buttonOK.ButtonColorScaling = 0.5F;
            this.buttonOK.ButtonDisabledScaling = 0.5F;
            this.buttonOK.Location = new System.Drawing.Point(581, 605);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 32);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(668, 10);
            this.panel5.TabIndex = 12;
            // 
            // CommanderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 642);
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
            this.groupBoxCustom5.ResumeLayout(false);
            this.groupBoxCustom5.PerformLayout();
            this.groupBoxCustom4.ResumeLayout(false);
            this.groupBoxCustom4.PerformLayout();
            this.groupBoxCustom3.ResumeLayout(false);
            this.groupBoxCustom3.PerformLayout();
            this.groupBoxCustom2.ResumeLayout(false);
            this.groupBoxCustom2.PerformLayout();
            this.groupBoxCustom1.ResumeLayout(false);
            this.groupBoxCustom1.PerformLayout();
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
        private System.Windows.Forms.Label label7;
        private ExtendedControls.TextBoxBorder textBoxBorderJournal;
        private ExtendedControls.TextBoxBorder textBoxBorderCmdr;
        private ExtendedControls.TextBoxBorder textBoxBorderEDSMAPI;
        private ExtendedControls.TextBoxBorder textBoxBorderEDSMName;
        private ExtendedControls.GroupBoxCustom groupBoxCustom1;
        private ExtendedControls.ButtonExt buttonExtBrowseJ;
        private ExtendedControls.GroupBoxCustom groupBoxCustom2;
        private ExtendedControls.GroupBoxCustom groupBoxCustom3;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDSMFrom;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDSMTo;
        private ExtendedControls.CheckBoxCustom checkBoxCustomEDDNTo;
        private ExtendedControls.GroupBoxCustom groupBoxCustom4;
        private ExtendedControls.TextBoxBorder textBoxBorder1;
        private System.Windows.Forms.Label label12;
        private ExtendedControls.TextBoxBorder textBoxBorderCompanionLogin;
        private System.Windows.Forms.Label label11;
        private ExtendedControls.GroupBoxCustom groupBoxCustom5;
        private ExtendedControls.TextBoxBorder textBoxBorderTravelLog;
        private ExtendedControls.ButtonExt buttonExtAuthenticate;
        private ExtendedControls.TextBoxBorder textBoxBorder2;
        private System.Windows.Forms.Label label8;
        private ExtendedControls.CheckBoxCustom checkBoxCustomLoggedIn;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}