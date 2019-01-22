namespace EDDiscovery.Forms
{
    partial class ChatConsoleSetup
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
            this.groupBoxUserTokens = new ExtendedControls.GroupBoxCustom();
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.buttonExt2 = new ExtendedControls.ButtonExt();
            this.labelCmdrDiaryEntry = new ExtendedControls.LabelExt();
            this.textBoxCmdrLogEntry = new ExtendedControls.TextBoxBorder();
            this.labelCmdrLogClear = new ExtendedControls.LabelExt();
            this.textBoxCmdrLogClear = new ExtendedControls.TextBoxBorder();
            this.textReplaceSystemNote = new ExtendedControls.TextBoxBorder();
            this.labelExt1 = new ExtendedControls.LabelExt();
            this.textAppendSystemNote = new ExtendedControls.TextBoxBorder();
            this.labelExt2 = new ExtendedControls.LabelExt();
            this.textClearSystemNote = new ExtendedControls.TextBoxBorder();
            this.labelExt3 = new ExtendedControls.LabelExt();
            this.textBoxBorder2 = new ExtendedControls.TextBoxBorder();
            this.labelExt5 = new ExtendedControls.LabelExt();
            this.textTargetSet = new ExtendedControls.TextBoxBorder();
            this.labelSetTarget = new ExtendedControls.LabelExt();
            this.textBoxBorder1 = new ExtendedControls.TextBoxBorder();
            this.labelExt4 = new ExtendedControls.LabelExt();
            this.textBoxBorder3 = new ExtendedControls.TextBoxBorder();
            this.labelExt6 = new ExtendedControls.LabelExt();
            this.textBoxBorder4 = new ExtendedControls.TextBoxBorder();
            this.labelExt7 = new ExtendedControls.LabelExt();
            this.textBoxBorder6 = new ExtendedControls.TextBoxBorder();
            this.labelExt9 = new ExtendedControls.LabelExt();
            this.groupBoxUserTokens.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxUserTokens
            // 
            this.groupBoxUserTokens.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxUserTokens.BackColorScaling = 0.5F;
            this.groupBoxUserTokens.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxUserTokens.BorderColorScaling = 0.5F;
            this.groupBoxUserTokens.Controls.Add(this.textBoxBorder6);
            this.groupBoxUserTokens.Controls.Add(this.labelExt9);
            this.groupBoxUserTokens.Controls.Add(this.textBoxBorder1);
            this.groupBoxUserTokens.Controls.Add(this.labelExt4);
            this.groupBoxUserTokens.Controls.Add(this.textBoxBorder3);
            this.groupBoxUserTokens.Controls.Add(this.labelExt6);
            this.groupBoxUserTokens.Controls.Add(this.textBoxBorder4);
            this.groupBoxUserTokens.Controls.Add(this.labelExt7);
            this.groupBoxUserTokens.Controls.Add(this.textBoxBorder2);
            this.groupBoxUserTokens.Controls.Add(this.labelExt5);
            this.groupBoxUserTokens.Controls.Add(this.textTargetSet);
            this.groupBoxUserTokens.Controls.Add(this.labelSetTarget);
            this.groupBoxUserTokens.Controls.Add(this.textClearSystemNote);
            this.groupBoxUserTokens.Controls.Add(this.labelExt3);
            this.groupBoxUserTokens.Controls.Add(this.textReplaceSystemNote);
            this.groupBoxUserTokens.Controls.Add(this.labelExt1);
            this.groupBoxUserTokens.Controls.Add(this.textAppendSystemNote);
            this.groupBoxUserTokens.Controls.Add(this.labelExt2);
            this.groupBoxUserTokens.Controls.Add(this.textBoxCmdrLogClear);
            this.groupBoxUserTokens.Controls.Add(this.labelCmdrLogClear);
            this.groupBoxUserTokens.Controls.Add(this.textBoxCmdrLogEntry);
            this.groupBoxUserTokens.Controls.Add(this.labelCmdrDiaryEntry);
            this.groupBoxUserTokens.FillClientAreaWithAlternateColor = false;
            this.groupBoxUserTokens.Location = new System.Drawing.Point(2, 2);
            this.groupBoxUserTokens.Name = "groupBoxUserTokens";
            this.groupBoxUserTokens.Size = new System.Drawing.Size(228, 386);
            this.groupBoxUserTokens.TabIndex = 0;
            this.groupBoxUserTokens.TabStop = false;
            this.groupBoxUserTokens.Text = "User Defined Tokens";
            this.groupBoxUserTokens.TextPadding = 0;
            this.groupBoxUserTokens.TextStartPosition = -1;
            // 
            // buttonExt1
            // 
            this.buttonExt1.Location = new System.Drawing.Point(21, 398);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(75, 23);
            this.buttonExt1.TabIndex = 0;
            this.buttonExt1.Text = "Cancel";
            this.buttonExt1.UseVisualStyleBackColor = true;
            this.buttonExt1.Click += new System.EventHandler(this.buttonExt1_Click);
            // 
            // buttonExt2
            // 
            this.buttonExt2.Location = new System.Drawing.Point(137, 398);
            this.buttonExt2.Name = "buttonExt2";
            this.buttonExt2.Size = new System.Drawing.Size(75, 23);
            this.buttonExt2.TabIndex = 1;
            this.buttonExt2.Text = "Save";
            this.buttonExt2.UseVisualStyleBackColor = true;
            this.buttonExt2.Click += new System.EventHandler(this.buttonExt2_Click);
            // 
            // labelCmdrDiaryEntry
            // 
            this.labelCmdrDiaryEntry.AutoSize = true;
            this.labelCmdrDiaryEntry.Location = new System.Drawing.Point(11, 30);
            this.labelCmdrDiaryEntry.Name = "labelCmdrDiaryEntry";
            this.labelCmdrDiaryEntry.Size = new System.Drawing.Size(118, 13);
            this.labelCmdrDiaryEntry.TabIndex = 0;
            this.labelCmdrDiaryEntry.Text = "Create a CmdrLog entry";
            this.labelCmdrDiaryEntry.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxCmdrLogEntry
            // 
            this.textBoxCmdrLogEntry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCmdrLogEntry.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCmdrLogEntry.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxCmdrLogEntry.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCmdrLogEntry.BorderColorScaling = 0.5F;
            this.textBoxCmdrLogEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCmdrLogEntry.ClearOnFirstChar = false;
            this.textBoxCmdrLogEntry.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCmdrLogEntry.InErrorCondition = false;
            this.textBoxCmdrLogEntry.Location = new System.Drawing.Point(161, 30);
            this.textBoxCmdrLogEntry.Multiline = false;
            this.textBoxCmdrLogEntry.Name = "textBoxCmdrLogEntry";
            this.textBoxCmdrLogEntry.ReadOnly = false;
            this.textBoxCmdrLogEntry.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCmdrLogEntry.SelectionLength = 0;
            this.textBoxCmdrLogEntry.SelectionStart = 0;
            this.textBoxCmdrLogEntry.Size = new System.Drawing.Size(56, 20);
            this.textBoxCmdrLogEntry.TabIndex = 1;
            this.textBoxCmdrLogEntry.Text = "/la";
            this.textBoxCmdrLogEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxCmdrLogEntry.WordWrap = true;
            // 
            // labelCmdrLogClear
            // 
            this.labelCmdrLogClear.AutoSize = true;
            this.labelCmdrLogClear.Location = new System.Drawing.Point(11, 56);
            this.labelCmdrLogClear.Name = "labelCmdrLogClear";
            this.labelCmdrLogClear.Size = new System.Drawing.Size(118, 13);
            this.labelCmdrLogClear.TabIndex = 2;
            this.labelCmdrLogClear.Text = "Create a CmdrLog entry";
            this.labelCmdrLogClear.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxCmdrLogClear
            // 
            this.textBoxCmdrLogClear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCmdrLogClear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCmdrLogClear.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxCmdrLogClear.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCmdrLogClear.BorderColorScaling = 0.5F;
            this.textBoxCmdrLogClear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCmdrLogClear.ClearOnFirstChar = false;
            this.textBoxCmdrLogClear.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCmdrLogClear.InErrorCondition = false;
            this.textBoxCmdrLogClear.Location = new System.Drawing.Point(161, 56);
            this.textBoxCmdrLogClear.Multiline = false;
            this.textBoxCmdrLogClear.Name = "textBoxCmdrLogClear";
            this.textBoxCmdrLogClear.ReadOnly = false;
            this.textBoxCmdrLogClear.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCmdrLogClear.SelectionLength = 0;
            this.textBoxCmdrLogClear.SelectionStart = 0;
            this.textBoxCmdrLogClear.Size = new System.Drawing.Size(56, 20);
            this.textBoxCmdrLogClear.TabIndex = 3;
            this.textBoxCmdrLogClear.Text = "/lc";
            this.textBoxCmdrLogClear.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxCmdrLogClear.WordWrap = true;
            // 
            // textReplaceSystemNote
            // 
            this.textReplaceSystemNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textReplaceSystemNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textReplaceSystemNote.BackErrorColor = System.Drawing.Color.Red;
            this.textReplaceSystemNote.BorderColor = System.Drawing.Color.Transparent;
            this.textReplaceSystemNote.BorderColorScaling = 0.5F;
            this.textReplaceSystemNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textReplaceSystemNote.ClearOnFirstChar = false;
            this.textReplaceSystemNote.ControlBackground = System.Drawing.SystemColors.Control;
            this.textReplaceSystemNote.InErrorCondition = false;
            this.textReplaceSystemNote.Location = new System.Drawing.Point(161, 123);
            this.textReplaceSystemNote.Multiline = false;
            this.textReplaceSystemNote.Name = "textReplaceSystemNote";
            this.textReplaceSystemNote.ReadOnly = false;
            this.textReplaceSystemNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textReplaceSystemNote.SelectionLength = 0;
            this.textReplaceSystemNote.SelectionStart = 0;
            this.textReplaceSystemNote.Size = new System.Drawing.Size(56, 20);
            this.textReplaceSystemNote.TabIndex = 7;
            this.textReplaceSystemNote.Text = "/nr";
            this.textReplaceSystemNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textReplaceSystemNote.WordWrap = true;
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(11, 123);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(115, 13);
            this.labelExt1.TabIndex = 6;
            this.labelExt1.Text = "Replace a system note";
            this.labelExt1.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textAppendSystemNote
            // 
            this.textAppendSystemNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textAppendSystemNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textAppendSystemNote.BackErrorColor = System.Drawing.Color.Red;
            this.textAppendSystemNote.BorderColor = System.Drawing.Color.Transparent;
            this.textAppendSystemNote.BorderColorScaling = 0.5F;
            this.textAppendSystemNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textAppendSystemNote.ClearOnFirstChar = false;
            this.textAppendSystemNote.ControlBackground = System.Drawing.SystemColors.Control;
            this.textAppendSystemNote.InErrorCondition = false;
            this.textAppendSystemNote.Location = new System.Drawing.Point(161, 97);
            this.textAppendSystemNote.Multiline = false;
            this.textAppendSystemNote.Name = "textAppendSystemNote";
            this.textAppendSystemNote.ReadOnly = false;
            this.textAppendSystemNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textAppendSystemNote.SelectionLength = 0;
            this.textAppendSystemNote.SelectionStart = 0;
            this.textAppendSystemNote.Size = new System.Drawing.Size(56, 20);
            this.textAppendSystemNote.TabIndex = 5;
            this.textAppendSystemNote.Text = "/na";
            this.textAppendSystemNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textAppendSystemNote.WordWrap = true;
            // 
            // labelExt2
            // 
            this.labelExt2.AutoSize = true;
            this.labelExt2.Location = new System.Drawing.Point(11, 97);
            this.labelExt2.Name = "labelExt2";
            this.labelExt2.Size = new System.Drawing.Size(112, 13);
            this.labelExt2.TabIndex = 4;
            this.labelExt2.Text = "Append a system note";
            this.labelExt2.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textClearSystemNote
            // 
            this.textClearSystemNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textClearSystemNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textClearSystemNote.BackErrorColor = System.Drawing.Color.Red;
            this.textClearSystemNote.BorderColor = System.Drawing.Color.Transparent;
            this.textClearSystemNote.BorderColorScaling = 0.5F;
            this.textClearSystemNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textClearSystemNote.ClearOnFirstChar = false;
            this.textClearSystemNote.ControlBackground = System.Drawing.SystemColors.Control;
            this.textClearSystemNote.InErrorCondition = false;
            this.textClearSystemNote.Location = new System.Drawing.Point(161, 149);
            this.textClearSystemNote.Multiline = false;
            this.textClearSystemNote.Name = "textClearSystemNote";
            this.textClearSystemNote.ReadOnly = false;
            this.textClearSystemNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textClearSystemNote.SelectionLength = 0;
            this.textClearSystemNote.SelectionStart = 0;
            this.textClearSystemNote.Size = new System.Drawing.Size(56, 20);
            this.textClearSystemNote.TabIndex = 9;
            this.textClearSystemNote.Text = "/nc";
            this.textClearSystemNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textClearSystemNote.WordWrap = true;
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(11, 149);
            this.labelExt3.Name = "labelExt3";
            this.labelExt3.Size = new System.Drawing.Size(108, 13);
            this.labelExt3.TabIndex = 8;
            this.labelExt3.Text = "Clear the system note";
            this.labelExt3.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxBorder2
            // 
            this.textBoxBorder2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorder2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorder2.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorder2.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder2.BorderColorScaling = 0.5F;
            this.textBoxBorder2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorder2.ClearOnFirstChar = false;
            this.textBoxBorder2.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorder2.InErrorCondition = false;
            this.textBoxBorder2.Location = new System.Drawing.Point(161, 213);
            this.textBoxBorder2.Multiline = false;
            this.textBoxBorder2.Name = "textBoxBorder2";
            this.textBoxBorder2.ReadOnly = false;
            this.textBoxBorder2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorder2.SelectionLength = 0;
            this.textBoxBorder2.SelectionStart = 0;
            this.textBoxBorder2.Size = new System.Drawing.Size(56, 20);
            this.textBoxBorder2.TabIndex = 13;
            this.textBoxBorder2.Text = "/tu";
            this.textBoxBorder2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorder2.WordWrap = true;
            // 
            // labelExt5
            // 
            this.labelExt5.AutoSize = true;
            this.labelExt5.Location = new System.Drawing.Point(11, 213);
            this.labelExt5.Name = "labelExt5";
            this.labelExt5.Size = new System.Drawing.Size(87, 13);
            this.labelExt5.TabIndex = 12;
            this.labelExt5.Text = "Unset the Target";
            this.labelExt5.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textTargetSet
            // 
            this.textTargetSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textTargetSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textTargetSet.BackErrorColor = System.Drawing.Color.Red;
            this.textTargetSet.BorderColor = System.Drawing.Color.Transparent;
            this.textTargetSet.BorderColorScaling = 0.5F;
            this.textTargetSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textTargetSet.ClearOnFirstChar = false;
            this.textTargetSet.ControlBackground = System.Drawing.SystemColors.Control;
            this.textTargetSet.InErrorCondition = false;
            this.textTargetSet.Location = new System.Drawing.Point(161, 187);
            this.textTargetSet.Multiline = false;
            this.textTargetSet.Name = "textTargetSet";
            this.textTargetSet.ReadOnly = false;
            this.textTargetSet.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textTargetSet.SelectionLength = 0;
            this.textTargetSet.SelectionStart = 0;
            this.textTargetSet.Size = new System.Drawing.Size(56, 20);
            this.textTargetSet.TabIndex = 11;
            this.textTargetSet.Text = "/ts :[name]";
            this.textTargetSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textTargetSet.WordWrap = true;
            // 
            // labelSetTarget
            // 
            this.labelSetTarget.AutoSize = true;
            this.labelSetTarget.Location = new System.Drawing.Point(11, 187);
            this.labelSetTarget.Name = "labelSetTarget";
            this.labelSetTarget.Size = new System.Drawing.Size(57, 13);
            this.labelSetTarget.TabIndex = 10;
            this.labelSetTarget.Text = "Set Target";
            this.labelSetTarget.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxBorder1
            // 
            this.textBoxBorder1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorder1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorder1.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorder1.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder1.BorderColorScaling = 0.5F;
            this.textBoxBorder1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorder1.ClearOnFirstChar = false;
            this.textBoxBorder1.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorder1.InErrorCondition = false;
            this.textBoxBorder1.Location = new System.Drawing.Point(161, 302);
            this.textBoxBorder1.Multiline = false;
            this.textBoxBorder1.Name = "textBoxBorder1";
            this.textBoxBorder1.ReadOnly = false;
            this.textBoxBorder1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorder1.SelectionLength = 0;
            this.textBoxBorder1.SelectionStart = 0;
            this.textBoxBorder1.Size = new System.Drawing.Size(56, 20);
            this.textBoxBorder1.TabIndex = 19;
            this.textBoxBorder1.Text = "/mc";
            this.textBoxBorder1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorder1.WordWrap = true;
            // 
            // labelExt4
            // 
            this.labelExt4.AutoSize = true;
            this.labelExt4.Location = new System.Drawing.Point(11, 302);
            this.labelExt4.Name = "labelExt4";
            this.labelExt4.Size = new System.Drawing.Size(141, 13);
            this.labelExt4.TabIndex = 18;
            this.labelExt4.Text = "Clear Begin/End Trip marker";
            this.labelExt4.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxBorder3
            // 
            this.textBoxBorder3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorder3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorder3.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorder3.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder3.BorderColorScaling = 0.5F;
            this.textBoxBorder3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorder3.ClearOnFirstChar = false;
            this.textBoxBorder3.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorder3.InErrorCondition = false;
            this.textBoxBorder3.Location = new System.Drawing.Point(161, 276);
            this.textBoxBorder3.Multiline = false;
            this.textBoxBorder3.Name = "textBoxBorder3";
            this.textBoxBorder3.ReadOnly = false;
            this.textBoxBorder3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorder3.SelectionLength = 0;
            this.textBoxBorder3.SelectionStart = 0;
            this.textBoxBorder3.Size = new System.Drawing.Size(56, 20);
            this.textBoxBorder3.TabIndex = 17;
            this.textBoxBorder3.Text = "/me";
            this.textBoxBorder3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorder3.WordWrap = true;
            // 
            // labelExt6
            // 
            this.labelExt6.AutoSize = true;
            this.labelExt6.Location = new System.Drawing.Point(11, 276);
            this.labelExt6.Name = "labelExt6";
            this.labelExt6.Size = new System.Drawing.Size(119, 13);
            this.labelExt6.TabIndex = 16;
            this.labelExt6.Text = "Set the Trip End marker";
            this.labelExt6.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxBorder4
            // 
            this.textBoxBorder4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorder4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorder4.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorder4.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder4.BorderColorScaling = 0.5F;
            this.textBoxBorder4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorder4.ClearOnFirstChar = false;
            this.textBoxBorder4.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorder4.InErrorCondition = false;
            this.textBoxBorder4.Location = new System.Drawing.Point(161, 250);
            this.textBoxBorder4.Multiline = false;
            this.textBoxBorder4.Name = "textBoxBorder4";
            this.textBoxBorder4.ReadOnly = false;
            this.textBoxBorder4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorder4.SelectionLength = 0;
            this.textBoxBorder4.SelectionStart = 0;
            this.textBoxBorder4.Size = new System.Drawing.Size(56, 20);
            this.textBoxBorder4.TabIndex = 15;
            this.textBoxBorder4.Text = "/mb";
            this.textBoxBorder4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorder4.WordWrap = true;
            // 
            // labelExt7
            // 
            this.labelExt7.AutoSize = true;
            this.labelExt7.Location = new System.Drawing.Point(11, 250);
            this.labelExt7.Name = "labelExt7";
            this.labelExt7.Size = new System.Drawing.Size(127, 13);
            this.labelExt7.TabIndex = 14;
            this.labelExt7.Text = "Set the Trip Begin marker";
            this.labelExt7.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // textBoxBorder6
            // 
            this.textBoxBorder6.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBorder6.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBorder6.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBorder6.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorder6.BorderColorScaling = 0.5F;
            this.textBoxBorder6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBorder6.ClearOnFirstChar = false;
            this.textBoxBorder6.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBorder6.InErrorCondition = false;
            this.textBoxBorder6.Location = new System.Drawing.Point(161, 341);
            this.textBoxBorder6.Multiline = false;
            this.textBoxBorder6.Name = "textBoxBorder6";
            this.textBoxBorder6.ReadOnly = false;
            this.textBoxBorder6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBorder6.SelectionLength = 0;
            this.textBoxBorder6.SelectionStart = 0;
            this.textBoxBorder6.Size = new System.Drawing.Size(56, 20);
            this.textBoxBorder6.TabIndex = 21;
            this.textBoxBorder6.Text = "/c [cmd]";
            this.textBoxBorder6.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBorder6.WordWrap = true;
            // 
            // labelExt9
            // 
            this.labelExt9.AutoSize = true;
            this.labelExt9.Location = new System.Drawing.Point(11, 341);
            this.labelExt9.Name = "labelExt9";
            this.labelExt9.Size = new System.Drawing.Size(105, 13);
            this.labelExt9.TabIndex = 20;
            this.labelExt9.Text = "Execute a Command";
            this.labelExt9.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // ChatConsoleSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 434);
            this.Controls.Add(this.buttonExt2);
            this.Controls.Add(this.buttonExt1);
            this.Controls.Add(this.groupBoxUserTokens);
            this.Name = "ChatConsoleSetup";
            this.Text = "ChatConsoleSetup";
            this.groupBoxUserTokens.ResumeLayout(false);
            this.groupBoxUserTokens.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.GroupBoxCustom groupBoxUserTokens;
        private ExtendedControls.ButtonExt buttonExt1;
        private ExtendedControls.ButtonExt buttonExt2;
        private ExtendedControls.TextBoxBorder textBoxCmdrLogClear;
        private ExtendedControls.LabelExt labelCmdrLogClear;
        private ExtendedControls.TextBoxBorder textBoxCmdrLogEntry;
        private ExtendedControls.LabelExt labelCmdrDiaryEntry;
        private ExtendedControls.TextBoxBorder textClearSystemNote;
        private ExtendedControls.LabelExt labelExt3;
        private ExtendedControls.TextBoxBorder textReplaceSystemNote;
        private ExtendedControls.LabelExt labelExt1;
        private ExtendedControls.TextBoxBorder textAppendSystemNote;
        private ExtendedControls.LabelExt labelExt2;
        private ExtendedControls.TextBoxBorder textBoxBorder6;
        private ExtendedControls.LabelExt labelExt9;
        private ExtendedControls.TextBoxBorder textBoxBorder1;
        private ExtendedControls.LabelExt labelExt4;
        private ExtendedControls.TextBoxBorder textBoxBorder3;
        private ExtendedControls.LabelExt labelExt6;
        private ExtendedControls.TextBoxBorder textBoxBorder4;
        private ExtendedControls.LabelExt labelExt7;
        private ExtendedControls.TextBoxBorder textBoxBorder2;
        private ExtendedControls.LabelExt labelExt5;
        private ExtendedControls.TextBoxBorder textTargetSet;
        private ExtendedControls.LabelExt labelSetTarget;
    }
}