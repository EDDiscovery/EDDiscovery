namespace EDDiscovery.UserControls
{
    partial class FindSystemsUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindSystemsUserControl));
            this.labelRadMin = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.labelRadMax = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.extCheckBoxExcludeVisitedSystems = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomCube = new ExtendedControls.ExtCheckBox();
            this.buttonExtNames = new ExtendedControls.ExtButton();
            this.buttonExtVisited = new ExtendedControls.ExtButton();
            this.buttonExtDB = new ExtendedControls.ExtButton();
            this.buttonExtEDSM = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.numberBoxMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.numberBoxDoubleZ = new ExtendedControls.NumberBoxDouble();
            this.numberBoxDoubleY = new ExtendedControls.NumberBoxDouble();
            this.numberBoxDoubleX = new ExtendedControls.NumberBoxDouble();
            this.numberBoxMinRadius = new ExtendedControls.NumberBoxDouble();
            this.textBoxSystemName = new ExtendedControls.ExtTextBoxAutoComplete();
            this.extButtonFromSpansh = new ExtendedControls.ExtButton();
            this.extButtonFromSpanshFindNames = new ExtendedControls.ExtButton();
            this.extButtonExpeditionSave = new ExtendedControls.ExtButton();
            this.extButtonExpeditionPush = new ExtendedControls.ExtButton();
            this.cmd3DMap = new ExtendedControls.ExtButton();
            this.SuspendLayout();
            // 
            // labelRadMin
            // 
            this.labelRadMin.AutoSize = true;
            this.labelRadMin.Location = new System.Drawing.Point(2, 71);
            this.labelRadMin.Name = "labelRadMin";
            this.labelRadMin.Size = new System.Drawing.Size(70, 13);
            this.labelRadMin.TabIndex = 34;
            this.labelRadMin.Text = "Radius ly Min";
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(3, 8);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(57, 13);
            this.labelFilter.TabIndex = 35;
            this.labelFilter.Text = "Star Name";
            // 
            // labelRadMax
            // 
            this.labelRadMax.AutoSize = true;
            this.labelRadMax.Location = new System.Drawing.Point(165, 71);
            this.labelRadMax.Name = "labelRadMax";
            this.labelRadMax.Size = new System.Drawing.Size(27, 13);
            this.labelRadMax.TabIndex = 34;
            this.labelRadMax.Text = "Max";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(6, 45);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 13);
            this.labelX.TabIndex = 35;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(135, 45);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(14, 13);
            this.labelY.TabIndex = 35;
            this.labelY.Text = "Y";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(267, 45);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(14, 13);
            this.labelZ.TabIndex = 35;
            this.labelZ.Text = "Z";
            // 
            // extCheckBoxExcludeVisitedSystems
            // 
            this.extCheckBoxExcludeVisitedSystems.AutoSize = true;
            this.extCheckBoxExcludeVisitedSystems.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxExcludeVisitedSystems.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxExcludeVisitedSystems.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxExcludeVisitedSystems.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxExcludeVisitedSystems.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxExcludeVisitedSystems.ImageIndeterminate = null;
            this.extCheckBoxExcludeVisitedSystems.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxExcludeVisitedSystems.ImageUnchecked = null;
            this.extCheckBoxExcludeVisitedSystems.Location = new System.Drawing.Point(9, 163);
            this.extCheckBoxExcludeVisitedSystems.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxExcludeVisitedSystems.Name = "extCheckBoxExcludeVisitedSystems";
            this.extCheckBoxExcludeVisitedSystems.Size = new System.Drawing.Size(140, 17);
            this.extCheckBoxExcludeVisitedSystems.TabIndex = 39;
            this.extCheckBoxExcludeVisitedSystems.Text = "Exclude Visited Systems";
            this.extCheckBoxExcludeVisitedSystems.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxExcludeVisitedSystems.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomCube
            // 
            this.checkBoxCustomCube.AutoSize = true;
            this.checkBoxCustomCube.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomCube.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomCube.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomCube.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomCube.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomCube.ImageIndeterminate = null;
            this.checkBoxCustomCube.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomCube.ImageUnchecked = null;
            this.checkBoxCustomCube.Location = new System.Drawing.Point(194, 163);
            this.checkBoxCustomCube.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomCube.Name = "checkBoxCustomCube";
            this.checkBoxCustomCube.Size = new System.Drawing.Size(51, 17);
            this.checkBoxCustomCube.TabIndex = 39;
            this.checkBoxCustomCube.Text = "Cube";
            this.checkBoxCustomCube.TickBoxReductionRatio = 0.75F;
            this.checkBoxCustomCube.UseVisualStyleBackColor = true;
            // 
            // buttonExtNames
            // 
            this.buttonExtNames.Location = new System.Drawing.Point(270, 3);
            this.buttonExtNames.Name = "buttonExtNames";
            this.buttonExtNames.Size = new System.Drawing.Size(176, 23);
            this.buttonExtNames.TabIndex = 37;
            this.buttonExtNames.Text = "From DB Find Names";
            this.buttonExtNames.UseVisualStyleBackColor = true;
            this.buttonExtNames.Click += new System.EventHandler(this.buttonExtNamesClick);
            // 
            // buttonExtVisited
            // 
            this.buttonExtVisited.Location = new System.Drawing.Point(9, 100);
            this.buttonExtVisited.Name = "buttonExtVisited";
            this.buttonExtVisited.Size = new System.Drawing.Size(176, 23);
            this.buttonExtVisited.TabIndex = 38;
            this.buttonExtVisited.Text = "From Visited Systems";
            this.buttonExtVisited.UseVisualStyleBackColor = true;
            this.buttonExtVisited.Click += new System.EventHandler(this.buttonExtVisitedClick);
            // 
            // buttonExtDB
            // 
            this.buttonExtDB.Location = new System.Drawing.Point(9, 129);
            this.buttonExtDB.Name = "buttonExtDB";
            this.buttonExtDB.Size = new System.Drawing.Size(176, 23);
            this.buttonExtDB.TabIndex = 38;
            this.buttonExtDB.Text = "From DB";
            this.buttonExtDB.UseVisualStyleBackColor = true;
            this.buttonExtDB.Click += new System.EventHandler(this.buttonExtDBClick);
            // 
            // buttonExtEDSM
            // 
            this.buttonExtEDSM.Location = new System.Drawing.Point(227, 100);
            this.buttonExtEDSM.Name = "buttonExtEDSM";
            this.buttonExtEDSM.Size = new System.Drawing.Size(176, 23);
            this.buttonExtEDSM.TabIndex = 38;
            this.buttonExtEDSM.Text = "From EDSM";
            this.buttonExtEDSM.UseVisualStyleBackColor = true;
            this.buttonExtEDSM.Click += new System.EventHandler(this.buttonExtEDSMClick);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(283, 158);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 36;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // numberBoxMaxRadius
            // 
            this.numberBoxMaxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMaxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMaxRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMaxRadius.BorderColorScaling = 0.5F;
            this.numberBoxMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMaxRadius.ClearOnFirstChar = false;
            this.numberBoxMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMaxRadius.DelayBeforeNotification = 0;
            this.numberBoxMaxRadius.EndButtonEnable = true;
            this.numberBoxMaxRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMaxRadius.EndButtonImage")));
            this.numberBoxMaxRadius.EndButtonSize16ths = 10;
            this.numberBoxMaxRadius.EndButtonVisible = false;
            this.numberBoxMaxRadius.Format = "0.##";
            this.numberBoxMaxRadius.InErrorCondition = false;
            this.numberBoxMaxRadius.Location = new System.Drawing.Point(211, 71);
            this.numberBoxMaxRadius.Maximum = 100000D;
            this.numberBoxMaxRadius.Minimum = 0D;
            this.numberBoxMaxRadius.Multiline = false;
            this.numberBoxMaxRadius.Name = "numberBoxMaxRadius";
            this.numberBoxMaxRadius.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxMaxRadius.ReadOnly = false;
            this.numberBoxMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMaxRadius.SelectionLength = 0;
            this.numberBoxMaxRadius.SelectionStart = 0;
            this.numberBoxMaxRadius.Size = new System.Drawing.Size(48, 20);
            this.numberBoxMaxRadius.TabIndex = 32;
            this.numberBoxMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMaxRadius.TextNoChange = "0";
            this.numberBoxMaxRadius.Value = 0D;
            this.numberBoxMaxRadius.WordWrap = true;
            // 
            // numberBoxDoubleZ
            // 
            this.numberBoxDoubleZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleZ.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleZ.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleZ.BorderColorScaling = 0.5F;
            this.numberBoxDoubleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleZ.ClearOnFirstChar = false;
            this.numberBoxDoubleZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleZ.DelayBeforeNotification = 0;
            this.numberBoxDoubleZ.EndButtonEnable = true;
            this.numberBoxDoubleZ.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxDoubleZ.EndButtonImage")));
            this.numberBoxDoubleZ.EndButtonSize16ths = 10;
            this.numberBoxDoubleZ.EndButtonVisible = false;
            this.numberBoxDoubleZ.Format = "0.##";
            this.numberBoxDoubleZ.InErrorCondition = false;
            this.numberBoxDoubleZ.Location = new System.Drawing.Point(287, 41);
            this.numberBoxDoubleZ.Maximum = 80000D;
            this.numberBoxDoubleZ.Minimum = -20000D;
            this.numberBoxDoubleZ.Multiline = false;
            this.numberBoxDoubleZ.Name = "numberBoxDoubleZ";
            this.numberBoxDoubleZ.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxDoubleZ.ReadOnly = false;
            this.numberBoxDoubleZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleZ.SelectionLength = 0;
            this.numberBoxDoubleZ.SelectionStart = 0;
            this.numberBoxDoubleZ.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleZ.TabIndex = 32;
            this.numberBoxDoubleZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleZ.TextNoChange = "0";
            this.numberBoxDoubleZ.Value = 0D;
            this.numberBoxDoubleZ.WordWrap = true;
            this.numberBoxDoubleZ.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // numberBoxDoubleY
            // 
            this.numberBoxDoubleY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleY.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleY.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleY.BorderColorScaling = 0.5F;
            this.numberBoxDoubleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleY.ClearOnFirstChar = false;
            this.numberBoxDoubleY.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleY.DelayBeforeNotification = 0;
            this.numberBoxDoubleY.EndButtonEnable = true;
            this.numberBoxDoubleY.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxDoubleY.EndButtonImage")));
            this.numberBoxDoubleY.EndButtonSize16ths = 10;
            this.numberBoxDoubleY.EndButtonVisible = false;
            this.numberBoxDoubleY.Format = "0.##";
            this.numberBoxDoubleY.InErrorCondition = false;
            this.numberBoxDoubleY.Location = new System.Drawing.Point(155, 41);
            this.numberBoxDoubleY.Maximum = 5000D;
            this.numberBoxDoubleY.Minimum = -5000D;
            this.numberBoxDoubleY.Multiline = false;
            this.numberBoxDoubleY.Name = "numberBoxDoubleY";
            this.numberBoxDoubleY.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxDoubleY.ReadOnly = false;
            this.numberBoxDoubleY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleY.SelectionLength = 0;
            this.numberBoxDoubleY.SelectionStart = 0;
            this.numberBoxDoubleY.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleY.TabIndex = 32;
            this.numberBoxDoubleY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleY.TextNoChange = "0";
            this.numberBoxDoubleY.Value = 0D;
            this.numberBoxDoubleY.WordWrap = true;
            this.numberBoxDoubleY.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // numberBoxDoubleX
            // 
            this.numberBoxDoubleX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleX.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleX.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleX.BorderColorScaling = 0.5F;
            this.numberBoxDoubleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleX.ClearOnFirstChar = false;
            this.numberBoxDoubleX.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleX.DelayBeforeNotification = 0;
            this.numberBoxDoubleX.EndButtonEnable = true;
            this.numberBoxDoubleX.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxDoubleX.EndButtonImage")));
            this.numberBoxDoubleX.EndButtonSize16ths = 10;
            this.numberBoxDoubleX.EndButtonVisible = false;
            this.numberBoxDoubleX.Format = "0.##";
            this.numberBoxDoubleX.InErrorCondition = false;
            this.numberBoxDoubleX.Location = new System.Drawing.Point(32, 41);
            this.numberBoxDoubleX.Maximum = 40000D;
            this.numberBoxDoubleX.Minimum = -40000D;
            this.numberBoxDoubleX.Multiline = false;
            this.numberBoxDoubleX.Name = "numberBoxDoubleX";
            this.numberBoxDoubleX.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxDoubleX.ReadOnly = false;
            this.numberBoxDoubleX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleX.SelectionLength = 0;
            this.numberBoxDoubleX.SelectionStart = 0;
            this.numberBoxDoubleX.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleX.TabIndex = 32;
            this.numberBoxDoubleX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleX.TextNoChange = "0";
            this.numberBoxDoubleX.Value = 0D;
            this.numberBoxDoubleX.WordWrap = true;
            this.numberBoxDoubleX.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // numberBoxMinRadius
            // 
            this.numberBoxMinRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMinRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMinRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMinRadius.BorderColorScaling = 0.5F;
            this.numberBoxMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMinRadius.ClearOnFirstChar = false;
            this.numberBoxMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMinRadius.DelayBeforeNotification = 0;
            this.numberBoxMinRadius.EndButtonEnable = true;
            this.numberBoxMinRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMinRadius.EndButtonImage")));
            this.numberBoxMinRadius.EndButtonSize16ths = 10;
            this.numberBoxMinRadius.EndButtonVisible = false;
            this.numberBoxMinRadius.Format = "0.##";
            this.numberBoxMinRadius.InErrorCondition = false;
            this.numberBoxMinRadius.Location = new System.Drawing.Point(91, 71);
            this.numberBoxMinRadius.Maximum = 100000D;
            this.numberBoxMinRadius.Minimum = 0D;
            this.numberBoxMinRadius.Multiline = false;
            this.numberBoxMinRadius.Name = "numberBoxMinRadius";
            this.numberBoxMinRadius.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxMinRadius.ReadOnly = false;
            this.numberBoxMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMinRadius.SelectionLength = 0;
            this.numberBoxMinRadius.SelectionStart = 0;
            this.numberBoxMinRadius.Size = new System.Drawing.Size(48, 20);
            this.numberBoxMinRadius.TabIndex = 32;
            this.numberBoxMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMinRadius.TextNoChange = "0";
            this.numberBoxMinRadius.Value = 0D;
            this.numberBoxMinRadius.WordWrap = true;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.AutoCompleteCommentMarker = null;
            this.textBoxSystemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSystemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSystemName.AutoCompleteTimeout = 500;
            this.textBoxSystemName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystemName.BorderColorScaling = 0.5F;
            this.textBoxSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSystemName.ClearOnFirstChar = false;
            this.textBoxSystemName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSystemName.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxSystemName.EndButtonEnable = false;
            this.textBoxSystemName.EndButtonSize16ths = 10;
            this.textBoxSystemName.EndButtonVisible = false;
            this.textBoxSystemName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxSystemName.InErrorCondition = false;
            this.textBoxSystemName.Location = new System.Drawing.Point(96, 4);
            this.textBoxSystemName.Multiline = false;
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = false;
            this.textBoxSystemName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSystemName.SelectionLength = 0;
            this.textBoxSystemName.SelectionStart = 0;
            this.textBoxSystemName.Size = new System.Drawing.Size(166, 20);
            this.textBoxSystemName.TabIndex = 33;
            this.textBoxSystemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSystemName.TextChangedEvent = "";
            this.textBoxSystemName.TextNoChange = "";
            this.textBoxSystemName.WordWrap = true;
            // 
            // extButtonFromSpansh
            // 
            this.extButtonFromSpansh.Location = new System.Drawing.Point(227, 129);
            this.extButtonFromSpansh.Name = "extButtonFromSpansh";
            this.extButtonFromSpansh.Size = new System.Drawing.Size(176, 23);
            this.extButtonFromSpansh.TabIndex = 38;
            this.extButtonFromSpansh.Text = "From Spansh";
            this.extButtonFromSpansh.UseVisualStyleBackColor = true;
            this.extButtonFromSpansh.Click += new System.EventHandler(this.extButtonFromSpansh_Click);
            // 
            // extButtonFromSpanshFindNames
            // 
            this.extButtonFromSpanshFindNames.Location = new System.Drawing.Point(452, 3);
            this.extButtonFromSpanshFindNames.Name = "extButtonFromSpanshFindNames";
            this.extButtonFromSpanshFindNames.Size = new System.Drawing.Size(176, 23);
            this.extButtonFromSpanshFindNames.TabIndex = 37;
            this.extButtonFromSpanshFindNames.Text = "From Spansh Find Names";
            this.extButtonFromSpanshFindNames.UseVisualStyleBackColor = true;
            this.extButtonFromSpanshFindNames.Click += new System.EventHandler(this.extButtonFromSpanshFindNames_Click);
            // 
            // extButtonExpeditionSave
            // 
            this.extButtonExpeditionSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonExpeditionSave.Image = global::EDDiscovery.Icons.Controls.Save;
            this.extButtonExpeditionSave.Location = new System.Drawing.Point(352, 158);
            this.extButtonExpeditionSave.Name = "extButtonExpeditionSave";
            this.extButtonExpeditionSave.Size = new System.Drawing.Size(28, 28);
            this.extButtonExpeditionSave.TabIndex = 62;
            this.extButtonExpeditionSave.UseVisualStyleBackColor = true;
            this.extButtonExpeditionSave.Click += new System.EventHandler(this.extButtonExpeditionSave_Click);
            // 
            // extButtonExpeditionPush
            // 
            this.extButtonExpeditionPush.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonExpeditionPush.Image = global::EDDiscovery.Icons.Controls.expedition;
            this.extButtonExpeditionPush.Location = new System.Drawing.Point(317, 158);
            this.extButtonExpeditionPush.Name = "extButtonExpeditionPush";
            this.extButtonExpeditionPush.Size = new System.Drawing.Size(28, 28);
            this.extButtonExpeditionPush.TabIndex = 61;
            this.extButtonExpeditionPush.UseVisualStyleBackColor = true;
            this.extButtonExpeditionPush.Click += new System.EventHandler(this.extButtonExpeditionPush_Click);
            // 
            // cmd3DMap
            // 
            this.cmd3DMap.Image = global::EDDiscovery.Icons.Controls.Map3D;
            this.cmd3DMap.Location = new System.Drawing.Point(250, 158);
            this.cmd3DMap.Name = "cmd3DMap";
            this.cmd3DMap.Size = new System.Drawing.Size(28, 28);
            this.cmd3DMap.TabIndex = 60;
            this.cmd3DMap.UseVisualStyleBackColor = true;
            this.cmd3DMap.Click += new System.EventHandler(this.cmd3DMap_Click);
            // 
            // FindSystemsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extButtonExpeditionSave);
            this.Controls.Add(this.extButtonExpeditionPush);
            this.Controls.Add(this.cmd3DMap);
            this.Controls.Add(this.extCheckBoxExcludeVisitedSystems);
            this.Controls.Add(this.checkBoxCustomCube);
            this.Controls.Add(this.extButtonFromSpanshFindNames);
            this.Controls.Add(this.buttonExtNames);
            this.Controls.Add(this.buttonExtVisited);
            this.Controls.Add(this.buttonExtDB);
            this.Controls.Add(this.extButtonFromSpansh);
            this.Controls.Add(this.buttonExtEDSM);
            this.Controls.Add(this.buttonExtExcel);
            this.Controls.Add(this.numberBoxMaxRadius);
            this.Controls.Add(this.numberBoxDoubleZ);
            this.Controls.Add(this.numberBoxDoubleY);
            this.Controls.Add(this.numberBoxDoubleX);
            this.Controls.Add(this.numberBoxMinRadius);
            this.Controls.Add(this.textBoxSystemName);
            this.Controls.Add(this.labelRadMax);
            this.Controls.Add(this.labelRadMin);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.labelFilter);
            this.Name = "FindSystemsUserControl";
            this.Size = new System.Drawing.Size(661, 199);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtButton buttonExtNames;
        private ExtendedControls.ExtButton buttonExtEDSM;
        private ExtendedControls.ExtButton buttonExtVisited;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtTextBoxAutoComplete textBoxSystemName;
        private System.Windows.Forms.Label labelRadMin;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label labelRadMax;
        private ExtendedControls.NumberBoxDouble numberBoxMinRadius;
        private ExtendedControls.NumberBoxDouble numberBoxMaxRadius;
        private ExtendedControls.ExtButton buttonExtDB;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleX;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleY;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleZ;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelZ;
        private ExtendedControls.ExtCheckBox checkBoxCustomCube;
        private ExtendedControls.ExtCheckBox extCheckBoxExcludeVisitedSystems;
        private ExtendedControls.ExtButton extButtonFromSpansh;
        private ExtendedControls.ExtButton extButtonFromSpanshFindNames;
        private ExtendedControls.ExtButton extButtonExpeditionSave;
        private ExtendedControls.ExtButton extButtonExpeditionPush;
        private ExtendedControls.ExtButton cmd3DMap;
    }
}
