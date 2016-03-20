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
            this.textBoxDefaultZoom = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonHistorySelection = new System.Windows.Forms.RadioButton();
            this.radioButtonCentreHome = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHomeSystem = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label_opacity = new System.Windows.Forms.Label();
            this.trackBar_theme_opacity = new System.Windows.Forms.TrackBar();
            this.checkBox_theme_windowframe = new System.Windows.Forms.CheckBox();
            this.comboBoxTheme = new System.Windows.Forms.ComboBox();
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
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(897, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elite Dangerous netlog location (If not ovverride by commander settings belove)";
            // 
            // button_Browse
            // 
            this.button_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Browse.Location = new System.Drawing.Point(707, 46);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 3;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBoxNetLogDir
            // 
            this.textBoxNetLogDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNetLogDir.Location = new System.Drawing.Point(97, 48);
            this.textBoxNetLogDir.Name = "textBoxNetLogDir";
            this.textBoxNetLogDir.Size = new System.Drawing.Size(604, 20);
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
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.buttonAddCommander);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dataGridViewCommanders);
            this.groupBox4.Location = new System.Drawing.Point(0, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(977, 184);
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
            this.dataGridViewCommanders.Size = new System.Drawing.Size(868, 128);
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
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.Name = "ColumnCommander";
            this.ColumnCommander.Width = 130;
            // 
            // ColumnEDSMAPIKey
            // 
            this.ColumnEDSMAPIKey.DataPropertyName = "APIKey";
            this.ColumnEDSMAPIKey.HeaderText = "EDSM api key";
            this.ColumnEDSMAPIKey.Name = "ColumnEDSMAPIKey";
            this.ColumnEDSMAPIKey.Width = 150;
            // 
            // ColumnNetLogPath
            // 
            this.ColumnNetLogPath.DataPropertyName = "NetLogPath";
            this.ColumnNetLogPath.FillWeight = 300F;
            this.ColumnNetLogPath.HeaderText = "NetLog path";
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
            this.checkboxSkipSlowUpdates.BackColor = System.Drawing.Color.Gold;
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
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxDefaultZoom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.radioButtonHistorySelection);
            this.groupBox2.Controls.Add(this.radioButtonCentreHome);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxHomeSystem);
            this.groupBox2.Location = new System.Drawing.Point(440, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(372, 100);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D Map Settings";
            // 
            // textBoxDefaultZoom
            // 
            this.textBoxDefaultZoom.Location = new System.Drawing.Point(118, 65);
            this.textBoxDefaultZoom.Name = "textBoxDefaultZoom";
            this.textBoxDefaultZoom.Size = new System.Drawing.Size(107, 20);
            this.textBoxDefaultZoom.TabIndex = 6;
            this.textBoxDefaultZoom.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDefaultZoom_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 68);
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
            this.label4.Location = new System.Drawing.Point(22, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Open Centred On";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 22);
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
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label_opacity);
            this.groupBox5.Controls.Add(this.trackBar_theme_opacity);
            this.groupBox5.Controls.Add(this.checkBox_theme_windowframe);
            this.groupBox5.Controls.Add(this.comboBoxTheme);
            this.groupBox5.Location = new System.Drawing.Point(3, 394);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(891, 117);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Theme";
            // 
            // label_opacity
            // 
            this.label_opacity.AutoSize = true;
            this.label_opacity.Location = new System.Drawing.Point(711, 8);
            this.label_opacity.Name = "label_opacity";
            this.label_opacity.Size = new System.Drawing.Size(43, 13);
            this.label_opacity.TabIndex = 4;
            this.label_opacity.Text = "Opacity";
            // 
            // trackBar_theme_opacity
            // 
            this.trackBar_theme_opacity.Location = new System.Drawing.Point(681, 24);
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
            this.checkBox_theme_windowframe.Location = new System.Drawing.Point(791, 17);
            this.checkBox_theme_windowframe.Name = "checkBox_theme_windowframe";
            this.checkBox_theme_windowframe.Size = new System.Drawing.Size(97, 17);
            this.checkBox_theme_windowframe.TabIndex = 2;
            this.checkBox_theme_windowframe.Text = "Window Frame";
            this.checkBox_theme_windowframe.UseVisualStyleBackColor = true;
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTheme.FormattingEnabled = true;
            this.comboBoxTheme.Location = new System.Drawing.Point(17, 19);
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.Size = new System.Drawing.Size(267, 21);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.SelectedIndexChanged += new System.EventHandler(this.comboBoxTheme_SelectedIndexChanged);
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
            this.Size = new System.Drawing.Size(897, 657);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEDSMAPIKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNetLogPath;
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
    }
}
