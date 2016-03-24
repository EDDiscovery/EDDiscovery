namespace EDDiscovery2
{
    partial class Settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.textBoxNetLogDir = new System.Windows.Forms.TextBox();
            this.radioButton_Manual = new System.Windows.Forms.RadioButton();
            this.radioButton_Auto = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonAddCommander = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewCommanders = new System.Windows.Forms.DataGridView();
            this.ColumnNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEDSMAPIKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNetLogPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_Distances = new System.Windows.Forms.CheckBox();
            this.checkBoxEDSMLog = new System.Windows.Forms.CheckBox();
            this.checkboxSkipSlowUpdates = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxDefaultZoom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonHistorySelection = new System.Windows.Forms.RadioButton();
            this.radioButtonCentreHome = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHomeSystem = new System.Windows.Forms.TextBox();
            this.panel_defaultmapcolor = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel_theme19 = new System.Windows.Forms.Panel();
            this.panel_theme18 = new System.Windows.Forms.Panel();
            this.textBox_Font = new System.Windows.Forms.TextBox();
            this.buttonSaveTheme = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_theme4 = new System.Windows.Forms.Panel();
            this.panel_theme3 = new System.Windows.Forms.Panel();
            this.panel_theme6 = new System.Windows.Forms.Panel();
            this.panel_theme10 = new System.Windows.Forms.Panel();
            this.panel_theme17 = new System.Windows.Forms.Panel();
            this.panel_theme16 = new System.Windows.Forms.Panel();
            this.panel_theme14 = new System.Windows.Forms.Panel();
            this.panel_theme13 = new System.Windows.Forms.Panel();
            this.panel_theme12 = new System.Windows.Forms.Panel();
            this.panel_theme11 = new System.Windows.Forms.Panel();
            this.panel_theme9 = new System.Windows.Forms.Panel();
            this.panel_theme8 = new System.Windows.Forms.Panel();
            this.panel_theme7 = new System.Windows.Forms.Panel();
            this.panel_theme5 = new System.Windows.Forms.Panel();
            this.panel_theme2 = new System.Windows.Forms.Panel();
            this.panel_theme1 = new System.Windows.Forms.Panel();
            this.label_opacity = new System.Windows.Forms.Label();
            this.trackBar_theme_opacity = new System.Windows.Forms.TrackBar();
            this.checkBox_theme_windowframe = new System.Windows.Forms.CheckBox();
            this.comboBoxTheme = new System.Windows.Forms.ComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel_theme15 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_theme_opacity)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Browse);
            this.groupBox1.Controls.Add(this.textBoxNetLogDir);
            this.groupBox1.Controls.Add(this.radioButton_Manual);
            this.groupBox1.Controls.Add(this.radioButton_Auto);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(819, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elite Dangerous netlog location (If not override by commander settings below)";
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(728, 46);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 3;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBoxNetLogDir
            // 
            this.textBoxNetLogDir.Location = new System.Drawing.Point(97, 48);
            this.textBoxNetLogDir.Name = "textBoxNetLogDir";
            this.textBoxNetLogDir.Size = new System.Drawing.Size(623, 20);
            this.textBoxNetLogDir.TabIndex = 2;
            // 
            // radioButton_Manual
            // 
            this.radioButton_Manual.AutoSize = true;
            this.radioButton_Manual.Location = new System.Drawing.Point(17, 49);
            this.radioButton_Manual.Name = "radioButton_Manual";
            this.radioButton_Manual.Size = new System.Drawing.Size(60, 17);
            this.radioButton_Manual.TabIndex = 1;
            this.radioButton_Manual.TabStop = true;
            this.radioButton_Manual.Text = "Manual";
            this.radioButton_Manual.UseVisualStyleBackColor = true;
            // 
            // radioButton_Auto
            // 
            this.radioButton_Auto.AutoSize = true;
            this.radioButton_Auto.Location = new System.Drawing.Point(17, 26);
            this.radioButton_Auto.Name = "radioButton_Auto";
            this.radioButton_Auto.Size = new System.Drawing.Size(47, 17);
            this.radioButton_Auto.TabIndex = 0;
            this.radioButton_Auto.TabStop = true;
            this.radioButton_Auto.Text = "Auto";
            this.radioButton_Auto.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonAddCommander);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dataGridViewCommanders);
            this.groupBox4.Location = new System.Drawing.Point(0, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(819, 184);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Commanders";
            // 
            // buttonAddCommander
            // 
            this.buttonAddCommander.Location = new System.Drawing.Point(11, 16);
            this.buttonAddCommander.Name = "buttonAddCommander";
            this.buttonAddCommander.Size = new System.Drawing.Size(104, 23);
            this.buttonAddCommander.TabIndex = 14;
            this.buttonAddCommander.Text = "Add commander";
            this.buttonAddCommander.UseVisualStyleBackColor = true;
            this.buttonAddCommander.Click += new System.EventHandler(this.buttonAddCommander_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Get Api key from http://www.edsm.net in  \"My account\" menu.";
            // 
            // dataGridViewCommanders
            // 
            this.dataGridViewCommanders.AllowUserToAddRows = false;
            this.dataGridViewCommanders.AllowUserToDeleteRows = false;
            this.dataGridViewCommanders.AllowUserToOrderColumns = true;
            this.dataGridViewCommanders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommanders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNr,
            this.ColumnCommander,
            this.ColumnEDSMAPIKey,
            this.ColumnNetLogPath});
            this.dataGridViewCommanders.Location = new System.Drawing.Point(11, 45);
            this.dataGridViewCommanders.Name = "dataGridViewCommanders";
            this.dataGridViewCommanders.Size = new System.Drawing.Size(792, 128);
            this.dataGridViewCommanders.TabIndex = 13;
            // 
            // ColumnNr
            // 
            this.ColumnNr.DataPropertyName = "Nr";
            this.ColumnNr.HeaderText = "Nr";
            this.ColumnNr.Name = "ColumnNr";
            this.ColumnNr.ReadOnly = true;
            this.ColumnNr.Width = 20;
            // 
            // ColumnCommander
            // 
            this.ColumnCommander.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.FillWeight = 80F;
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.MinimumWidth = 100;
            this.ColumnCommander.Name = "ColumnCommander";
            // 
            // ColumnEDSMAPIKey
            // 
            this.ColumnEDSMAPIKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnEDSMAPIKey.DataPropertyName = "APIKey";
            this.ColumnEDSMAPIKey.FillWeight = 120F;
            this.ColumnEDSMAPIKey.HeaderText = "EDSM api key";
            this.ColumnEDSMAPIKey.MinimumWidth = 100;
            this.ColumnEDSMAPIKey.Name = "ColumnEDSMAPIKey";
            // 
            // ColumnNetLogPath
            // 
            this.ColumnNetLogPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnNetLogPath.DataPropertyName = "NetLogPath";
            this.ColumnNetLogPath.FillWeight = 120F;
            this.ColumnNetLogPath.HeaderText = "NetLog path";
            this.ColumnNetLogPath.MinimumWidth = 100;
            this.ColumnNetLogPath.Name = "ColumnNetLogPath";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_Distances);
            this.groupBox3.Controls.Add(this.checkBoxEDSMLog);
            this.groupBox3.Controls.Add(this.checkboxSkipSlowUpdates);
            this.groupBox3.Location = new System.Drawing.Point(3, 280);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 100);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controls";
            // 
            // checkBox_Distances
            // 
            this.checkBox_Distances.AutoSize = true;
            this.checkBox_Distances.Location = new System.Drawing.Point(17, 18);
            this.checkBox_Distances.Name = "checkBox_Distances";
            this.checkBox_Distances.Size = new System.Drawing.Size(346, 17);
            this.checkBox_Distances.TabIndex = 8;
            this.checkBox_Distances.Text = "Get Distances from EDSM (Uses more memory normally not needed)";
            this.checkBox_Distances.UseVisualStyleBackColor = true;
            // 
            // checkBoxEDSMLog
            // 
            this.checkBoxEDSMLog.AutoSize = true;
            this.checkBoxEDSMLog.Location = new System.Drawing.Point(17, 41);
            this.checkBoxEDSMLog.Name = "checkBoxEDSMLog";
            this.checkBoxEDSMLog.Size = new System.Drawing.Size(121, 17);
            this.checkBoxEDSMLog.TabIndex = 9;
            this.checkBoxEDSMLog.Text = "Log EDSM requests";
            this.checkBoxEDSMLog.UseVisualStyleBackColor = true;
            // 
            // checkboxSkipSlowUpdates
            // 
            this.checkboxSkipSlowUpdates.AutoSize = true;
            this.checkboxSkipSlowUpdates.BackColor = System.Drawing.Color.Transparent;
            this.checkboxSkipSlowUpdates.Location = new System.Drawing.Point(17, 64);
            this.checkboxSkipSlowUpdates.Name = "checkboxSkipSlowUpdates";
            this.checkboxSkipSlowUpdates.Size = new System.Drawing.Size(238, 17);
            this.checkboxSkipSlowUpdates.TabIndex = 11;
            this.checkboxSkipSlowUpdates.Text = "DEBUG ONLY: Skip slow updates on startup";
            this.checkboxSkipSlowUpdates.UseVisualStyleBackColor = false;
            this.checkboxSkipSlowUpdates.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.textBoxDefaultZoom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.radioButtonHistorySelection);
            this.groupBox2.Controls.Add(this.radioButtonCentreHome);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxHomeSystem);
            this.groupBox2.Controls.Add(this.panel_defaultmapcolor);
            this.groupBox2.Location = new System.Drawing.Point(440, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 100);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D Map Settings";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(224, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 13);
            this.label17.TabIndex = 7;
            this.label17.Text = "Default Map Color";
            // 
            // textBoxDefaultZoom
            // 
            this.textBoxDefaultZoom.Location = new System.Drawing.Point(118, 70);
            this.textBoxDefaultZoom.Name = "textBoxDefaultZoom";
            this.textBoxDefaultZoom.Size = new System.Drawing.Size(51, 20);
            this.textBoxDefaultZoom.TabIndex = 6;
            this.textBoxDefaultZoom.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDefaultZoom_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Default Zoom";
            // 
            // radioButtonHistorySelection
            // 
            this.radioButtonHistorySelection.AutoSize = true;
            this.radioButtonHistorySelection.Location = new System.Drawing.Point(216, 45);
            this.radioButtonHistorySelection.Name = "radioButtonHistorySelection";
            this.radioButtonHistorySelection.Size = new System.Drawing.Size(137, 17);
            this.radioButtonHistorySelection.TabIndex = 4;
            this.radioButtonHistorySelection.TabStop = true;
            this.radioButtonHistorySelection.Text = "Travel History Selection";
            this.radioButtonHistorySelection.UseVisualStyleBackColor = true;
            // 
            // radioButtonCentreHome
            // 
            this.radioButtonCentreHome.AutoSize = true;
            this.radioButtonCentreHome.Location = new System.Drawing.Point(118, 45);
            this.radioButtonCentreHome.Name = "radioButtonCentreHome";
            this.radioButtonCentreHome.Size = new System.Drawing.Size(90, 17);
            this.radioButtonCentreHome.TabIndex = 3;
            this.radioButtonCentreHome.TabStop = true;
            this.radioButtonCentreHome.Text = "Home System";
            this.radioButtonCentreHome.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Open Centred On";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Home System";
            // 
            // textBoxHomeSystem
            // 
            this.textBoxHomeSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxHomeSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxHomeSystem.Location = new System.Drawing.Point(120, 19);
            this.textBoxHomeSystem.Name = "textBoxHomeSystem";
            this.textBoxHomeSystem.Size = new System.Drawing.Size(221, 20);
            this.textBoxHomeSystem.TabIndex = 0;
            // 
            // panel_defaultmapcolor
            // 
            this.panel_defaultmapcolor.AccessibleDescription = "";
            this.panel_defaultmapcolor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_defaultmapcolor.Location = new System.Drawing.Point(335, 70);
            this.panel_defaultmapcolor.Name = "panel_defaultmapcolor";
            this.panel_defaultmapcolor.Size = new System.Drawing.Size(28, 20);
            this.panel_defaultmapcolor.TabIndex = 5;
            this.panel_defaultmapcolor.Tag = "";
            this.panel_defaultmapcolor.Click += new System.EventHandler(this.panel_defaultmapcolor_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel_theme19);
            this.groupBox5.Controls.Add(this.panel_theme18);
            this.groupBox5.Controls.Add(this.textBox_Font);
            this.groupBox5.Controls.Add(this.buttonSaveTheme);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.panel_theme15);
            this.groupBox5.Controls.Add(this.panel_theme4);
            this.groupBox5.Controls.Add(this.panel_theme3);
            this.groupBox5.Controls.Add(this.panel_theme6);
            this.groupBox5.Controls.Add(this.panel_theme10);
            this.groupBox5.Controls.Add(this.panel_theme17);
            this.groupBox5.Controls.Add(this.panel_theme16);
            this.groupBox5.Controls.Add(this.panel_theme14);
            this.groupBox5.Controls.Add(this.panel_theme13);
            this.groupBox5.Controls.Add(this.panel_theme12);
            this.groupBox5.Controls.Add(this.panel_theme11);
            this.groupBox5.Controls.Add(this.panel_theme9);
            this.groupBox5.Controls.Add(this.panel_theme8);
            this.groupBox5.Controls.Add(this.panel_theme7);
            this.groupBox5.Controls.Add(this.panel_theme5);
            this.groupBox5.Controls.Add(this.panel_theme2);
            this.groupBox5.Controls.Add(this.panel_theme1);
            this.groupBox5.Controls.Add(this.label_opacity);
            this.groupBox5.Controls.Add(this.trackBar_theme_opacity);
            this.groupBox5.Controls.Add(this.checkBox_theme_windowframe);
            this.groupBox5.Controls.Add(this.comboBoxTheme);
            this.groupBox5.Location = new System.Drawing.Point(3, 394);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(816, 207);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Theme";
            // 
            // panel_theme19
            // 
            this.panel_theme19.AccessibleDescription = "";
            this.panel_theme19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme19.Location = new System.Drawing.Point(680, 70);
            this.panel_theme19.Name = "panel_theme19";
            this.panel_theme19.Size = new System.Drawing.Size(28, 28);
            this.panel_theme19.TabIndex = 9;
            this.panel_theme19.Tag = "";
            this.panel_theme19.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme18
            // 
            this.panel_theme18.AccessibleDescription = "";
            this.panel_theme18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme18.Location = new System.Drawing.Point(680, 38);
            this.panel_theme18.Name = "panel_theme18";
            this.panel_theme18.Size = new System.Drawing.Size(28, 28);
            this.panel_theme18.TabIndex = 9;
            this.panel_theme18.Tag = "";
            this.panel_theme18.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // textBox_Font
            // 
            this.textBox_Font.Location = new System.Drawing.Point(125, 122);
            this.textBox_Font.Name = "textBox_Font";
            this.textBox_Font.ReadOnly = true;
            this.textBox_Font.Size = new System.Drawing.Size(159, 20);
            this.textBox_Font.TabIndex = 8;
            this.textBox_Font.Text = "Font Elite Dangerous";
            this.textBox_Font.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxFont_MouseClick);
            // 
            // buttonSaveTheme
            // 
            this.buttonSaveTheme.Location = new System.Drawing.Point(17, 46);
            this.buttonSaveTheme.Name = "buttonSaveTheme";
            this.buttonSaveTheme.Size = new System.Drawing.Size(95, 23);
            this.buttonSaveTheme.TabIndex = 7;
            this.buttonSaveTheme.Text = "Save Theme";
            this.buttonSaveTheme.UseVisualStyleBackColor = true;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(315, 110);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Highlight";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(292, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Fore";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(292, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Back";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Form";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(673, 15);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Groups";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(733, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "Misc";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(626, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Visited";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(575, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Menu";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(517, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(52, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Grid Data";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(452, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Grid Border";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(410, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Button";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(377, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Text";
            // 
            // panel_theme4
            // 
            this.panel_theme4.AccessibleDescription = "";
            this.panel_theme4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme4.Location = new System.Drawing.Point(379, 102);
            this.panel_theme4.Name = "panel_theme4";
            this.panel_theme4.Size = new System.Drawing.Size(28, 28);
            this.panel_theme4.TabIndex = 5;
            this.panel_theme4.Tag = "";
            this.panel_theme4.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme3
            // 
            this.panel_theme3.AccessibleDescription = "";
            this.panel_theme3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme3.Location = new System.Drawing.Point(379, 70);
            this.panel_theme3.Name = "panel_theme3";
            this.panel_theme3.Size = new System.Drawing.Size(28, 28);
            this.panel_theme3.TabIndex = 5;
            this.panel_theme3.Tag = "";
            this.panel_theme3.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme6
            // 
            this.panel_theme6.AccessibleDescription = "";
            this.panel_theme6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme6.Location = new System.Drawing.Point(414, 70);
            this.panel_theme6.Name = "panel_theme6";
            this.panel_theme6.Size = new System.Drawing.Size(28, 28);
            this.panel_theme6.TabIndex = 5;
            this.panel_theme6.Tag = "";
            this.panel_theme6.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme10
            // 
            this.panel_theme10.AccessibleDescription = "";
            this.panel_theme10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme10.Location = new System.Drawing.Point(530, 70);
            this.panel_theme10.Name = "panel_theme10";
            this.panel_theme10.Size = new System.Drawing.Size(28, 28);
            this.panel_theme10.TabIndex = 5;
            this.panel_theme10.Tag = "";
            this.panel_theme10.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme17
            // 
            this.panel_theme17.AccessibleDescription = "";
            this.panel_theme17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme17.Location = new System.Drawing.Point(734, 134);
            this.panel_theme17.Name = "panel_theme17";
            this.panel_theme17.Size = new System.Drawing.Size(28, 28);
            this.panel_theme17.TabIndex = 5;
            this.panel_theme17.Tag = "";
            this.panel_theme17.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme16
            // 
            this.panel_theme16.AccessibleDescription = "";
            this.panel_theme16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme16.Location = new System.Drawing.Point(734, 102);
            this.panel_theme16.Name = "panel_theme16";
            this.panel_theme16.Size = new System.Drawing.Size(28, 28);
            this.panel_theme16.TabIndex = 5;
            this.panel_theme16.Tag = "";
            this.panel_theme16.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme14
            // 
            this.panel_theme14.AccessibleDescription = "";
            this.panel_theme14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme14.Location = new System.Drawing.Point(633, 134);
            this.panel_theme14.Name = "panel_theme14";
            this.panel_theme14.Size = new System.Drawing.Size(28, 28);
            this.panel_theme14.TabIndex = 5;
            this.panel_theme14.Tag = "";
            this.panel_theme14.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme13
            // 
            this.panel_theme13.AccessibleDescription = "";
            this.panel_theme13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme13.Location = new System.Drawing.Point(633, 102);
            this.panel_theme13.Name = "panel_theme13";
            this.panel_theme13.Size = new System.Drawing.Size(28, 28);
            this.panel_theme13.TabIndex = 5;
            this.panel_theme13.Tag = "";
            this.panel_theme13.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme12
            // 
            this.panel_theme12.AccessibleDescription = "";
            this.panel_theme12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme12.Location = new System.Drawing.Point(578, 70);
            this.panel_theme12.Name = "panel_theme12";
            this.panel_theme12.Size = new System.Drawing.Size(28, 28);
            this.panel_theme12.TabIndex = 5;
            this.panel_theme12.Tag = "";
            this.panel_theme12.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme11
            // 
            this.panel_theme11.AccessibleDescription = "";
            this.panel_theme11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme11.Location = new System.Drawing.Point(578, 38);
            this.panel_theme11.Name = "panel_theme11";
            this.panel_theme11.Size = new System.Drawing.Size(28, 28);
            this.panel_theme11.TabIndex = 5;
            this.panel_theme11.Tag = "";
            this.panel_theme11.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme9
            // 
            this.panel_theme9.AccessibleDescription = "";
            this.panel_theme9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme9.Location = new System.Drawing.Point(530, 38);
            this.panel_theme9.Name = "panel_theme9";
            this.panel_theme9.Size = new System.Drawing.Size(28, 28);
            this.panel_theme9.TabIndex = 5;
            this.panel_theme9.Tag = "";
            this.panel_theme9.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme8
            // 
            this.panel_theme8.AccessibleDescription = "";
            this.panel_theme8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme8.Location = new System.Drawing.Point(466, 70);
            this.panel_theme8.Name = "panel_theme8";
            this.panel_theme8.Size = new System.Drawing.Size(28, 28);
            this.panel_theme8.TabIndex = 5;
            this.panel_theme8.Tag = "";
            this.panel_theme8.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme7
            // 
            this.panel_theme7.AccessibleDescription = "";
            this.panel_theme7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme7.Location = new System.Drawing.Point(467, 38);
            this.panel_theme7.Name = "panel_theme7";
            this.panel_theme7.Size = new System.Drawing.Size(28, 28);
            this.panel_theme7.TabIndex = 5;
            this.panel_theme7.Tag = "";
            this.panel_theme7.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme5
            // 
            this.panel_theme5.AccessibleDescription = "";
            this.panel_theme5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme5.Location = new System.Drawing.Point(414, 38);
            this.panel_theme5.Name = "panel_theme5";
            this.panel_theme5.Size = new System.Drawing.Size(28, 28);
            this.panel_theme5.TabIndex = 5;
            this.panel_theme5.Tag = "";
            this.panel_theme5.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme2
            // 
            this.panel_theme2.AccessibleDescription = "";
            this.panel_theme2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme2.Location = new System.Drawing.Point(379, 38);
            this.panel_theme2.Name = "panel_theme2";
            this.panel_theme2.Size = new System.Drawing.Size(28, 28);
            this.panel_theme2.TabIndex = 5;
            this.panel_theme2.Tag = "";
            this.panel_theme2.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // panel_theme1
            // 
            this.panel_theme1.AccessibleDescription = "Button text";
            this.panel_theme1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme1.Location = new System.Drawing.Point(345, 38);
            this.panel_theme1.Name = "panel_theme1";
            this.panel_theme1.Size = new System.Drawing.Size(28, 28);
            this.panel_theme1.TabIndex = 5;
            this.panel_theme1.Tag = "";
            this.panel_theme1.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // label_opacity
            // 
            this.label_opacity.AutoSize = true;
            this.label_opacity.Location = new System.Drawing.Point(47, 82);
            this.label_opacity.Name = "label_opacity";
            this.label_opacity.Size = new System.Drawing.Size(43, 13);
            this.label_opacity.TabIndex = 4;
            this.label_opacity.Text = "Opacity";
            // 
            // trackBar_theme_opacity
            // 
            this.trackBar_theme_opacity.Location = new System.Drawing.Point(17, 98);
            this.trackBar_theme_opacity.Maximum = 100;
            this.trackBar_theme_opacity.Minimum = 30;
            this.trackBar_theme_opacity.Name = "trackBar_theme_opacity";
            this.trackBar_theme_opacity.Size = new System.Drawing.Size(104, 45);
            this.trackBar_theme_opacity.TabIndex = 3;
            this.trackBar_theme_opacity.Value = 30;
            this.trackBar_theme_opacity.MouseCaptureChanged += new System.EventHandler(this.trackBar_theme_opacity_MouseCaptureChanged);
            // 
            // checkBox_theme_windowframe
            // 
            this.checkBox_theme_windowframe.AutoSize = true;
            this.checkBox_theme_windowframe.Location = new System.Drawing.Point(125, 98);
            this.checkBox_theme_windowframe.Name = "checkBox_theme_windowframe";
            this.checkBox_theme_windowframe.Size = new System.Drawing.Size(97, 17);
            this.checkBox_theme_windowframe.TabIndex = 2;
            this.checkBox_theme_windowframe.Text = "Window Frame";
            this.checkBox_theme_windowframe.UseVisualStyleBackColor = true;
            this.checkBox_theme_windowframe.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkBox_theme_windowframe_MouseClick);
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheme.FormattingEnabled = true;
            this.comboBoxTheme.Location = new System.Drawing.Point(17, 19);
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.Size = new System.Drawing.Size(267, 21);
            this.comboBoxTheme.TabIndex = 0;
            // 
            // panel_theme15
            // 
            this.panel_theme15.AccessibleDescription = "";
            this.panel_theme15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_theme15.Location = new System.Drawing.Point(379, 134);
            this.panel_theme15.Name = "panel_theme15";
            this.panel_theme15.Size = new System.Drawing.Size(28, 28);
            this.panel_theme15.TabIndex = 5;
            this.panel_theme15.Tag = "";
            this.panel_theme15.Click += new System.EventHandler(this.panel_theme_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(315, 140);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(48, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Success";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(571, 110);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 13);
            this.label19.TabIndex = 6;
            this.label19.Text = "Unknown";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(571, 140);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(40, 13);
            this.label20.TabIndex = 6;
            this.label20.Text = "Known";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(667, 110);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(55, 13);
            this.label21.TabIndex = 6;
            this.label21.Text = "Checkbox";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(667, 140);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(33, 13);
            this.label22.TabIndex = 6;
            this.label22.Text = "Label";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(937, 725);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_theme_opacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox textBoxNetLogDir;
        private System.Windows.Forms.RadioButton radioButton_Manual;
        private System.Windows.Forms.RadioButton radioButton_Auto;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonAddCommander;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewCommanders;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_Distances;
        private System.Windows.Forms.CheckBox checkBoxEDSMLog;
        public System.Windows.Forms.CheckBox checkboxSkipSlowUpdates;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxDefaultZoom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButtonHistorySelection;
        private System.Windows.Forms.RadioButton radioButtonCentreHome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHomeSystem;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label_opacity;
        private System.Windows.Forms.TrackBar trackBar_theme_opacity;
        private System.Windows.Forms.CheckBox checkBox_theme_windowframe;
        private System.Windows.Forms.ComboBox comboBoxTheme;
        private System.Windows.Forms.Panel panel_theme1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_theme4;
        private System.Windows.Forms.Panel panel_theme3;
        private System.Windows.Forms.Panel panel_theme6;
        private System.Windows.Forms.Panel panel_theme10;
        private System.Windows.Forms.Panel panel_defaultmapcolor;
        private System.Windows.Forms.Panel panel_theme14;
        private System.Windows.Forms.Panel panel_theme13;
        private System.Windows.Forms.Panel panel_theme12;
        private System.Windows.Forms.Panel panel_theme11;
        private System.Windows.Forms.Panel panel_theme9;
        private System.Windows.Forms.Panel panel_theme8;
        private System.Windows.Forms.Panel panel_theme7;
        private System.Windows.Forms.Panel panel_theme5;
        private System.Windows.Forms.Panel panel_theme2;
        private System.Windows.Forms.Panel panel_theme17;
        private System.Windows.Forms.Panel panel_theme16;
        private System.Windows.Forms.Button buttonSaveTheme;
        private System.Windows.Forms.TextBox textBox_Font;
        private System.Windows.Forms.Panel panel_theme19;
        private System.Windows.Forms.Panel panel_theme18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEDSMAPIKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNetLogPath;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel_theme15;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
    }
}
