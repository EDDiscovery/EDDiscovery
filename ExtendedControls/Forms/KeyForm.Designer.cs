namespace ExtendedControls
{
    partial class KeyForm
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
            this.checkBoxShift = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCtrl = new ExtendedControls.CheckBoxCustom();
            this.checkBoxAlt = new ExtendedControls.CheckBoxCustom();
            this.checkBoxKey = new ExtendedControls.CheckBoxCustom();
            this.buttonReset = new ExtendedControls.ButtonExt();
            this.buttonNext = new ExtendedControls.ButtonExt();
            this.textBoxKeys = new ExtendedControls.TextBoxBorder();
            this.labelKeys = new System.Windows.Forms.Label();
            this.buttonTest = new ExtendedControls.ButtonExt();
            this.textBoxSendTo = new ExtendedControls.AutoCompleteTextBox();
            this.labelSendTo = new System.Windows.Forms.Label();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.SuspendLayout();
            // 
            // checkBoxShift
            // 
            this.checkBoxShift.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxShift.AutoCheck = false;
            this.checkBoxShift.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxShift.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxShift.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxShift.FontNerfReduction = 0.5F;
            this.checkBoxShift.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxShift.Location = new System.Drawing.Point(29, 32);
            this.checkBoxShift.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxShift.Name = "checkBoxShift";
            this.checkBoxShift.Size = new System.Drawing.Size(56, 56);
            this.checkBoxShift.TabIndex = 0;
            this.checkBoxShift.Text = "Shift";
            this.checkBoxShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxShift.TickBoxReductionSize = 10;
            this.checkBoxShift.UseVisualStyleBackColor = true;
            this.checkBoxShift.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxsac_MouseDown);
            // 
            // checkBoxCtrl
            // 
            this.checkBoxCtrl.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCtrl.AutoCheck = false;
            this.checkBoxCtrl.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCtrl.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCtrl.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCtrl.FontNerfReduction = 0.5F;
            this.checkBoxCtrl.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCtrl.Location = new System.Drawing.Point(104, 32);
            this.checkBoxCtrl.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCtrl.Name = "checkBoxCtrl";
            this.checkBoxCtrl.Size = new System.Drawing.Size(56, 56);
            this.checkBoxCtrl.TabIndex = 0;
            this.checkBoxCtrl.Text = "Ctrl";
            this.checkBoxCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCtrl.TickBoxReductionSize = 10;
            this.checkBoxCtrl.UseVisualStyleBackColor = true;
            this.checkBoxCtrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxsac_MouseDown);
            // 
            // checkBoxAlt
            // 
            this.checkBoxAlt.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxAlt.AutoCheck = false;
            this.checkBoxAlt.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAlt.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAlt.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAlt.FontNerfReduction = 0.5F;
            this.checkBoxAlt.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAlt.Location = new System.Drawing.Point(180, 32);
            this.checkBoxAlt.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAlt.Name = "checkBoxAlt";
            this.checkBoxAlt.Size = new System.Drawing.Size(56, 56);
            this.checkBoxAlt.TabIndex = 0;
            this.checkBoxAlt.Text = "Alt";
            this.checkBoxAlt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxAlt.TickBoxReductionSize = 10;
            this.checkBoxAlt.UseVisualStyleBackColor = true;
            this.checkBoxAlt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkBoxsac_MouseDown);
            // 
            // checkBoxKey
            // 
            this.checkBoxKey.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxKey.AutoCheck = false;
            this.checkBoxKey.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxKey.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxKey.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxKey.FontNerfReduction = 0.5F;
            this.checkBoxKey.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxKey.Location = new System.Drawing.Point(259, 32);
            this.checkBoxKey.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxKey.Name = "checkBoxKey";
            this.checkBoxKey.Size = new System.Drawing.Size(100, 56);
            this.checkBoxKey.TabIndex = 0;
            this.checkBoxKey.Text = "Press Key";
            this.checkBoxKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxKey.TickBoxReductionSize = 10;
            this.checkBoxKey.UseVisualStyleBackColor = true;
            // 
            // buttonReset
            // 
            this.buttonReset.BorderColorScaling = 1.25F;
            this.buttonReset.ButtonColorScaling = 0.5F;
            this.buttonReset.ButtonDisabledScaling = 0.5F;
            this.buttonReset.Location = new System.Drawing.Point(29, 113);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.BorderColorScaling = 1.25F;
            this.buttonNext.ButtonColorScaling = 0.5F;
            this.buttonNext.ButtonDisabledScaling = 0.5F;
            this.buttonNext.Location = new System.Drawing.Point(121, 113);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 1;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // textBoxKeys
            // 
            this.textBoxKeys.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxKeys.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxKeys.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxKeys.BorderColorScaling = 0.5F;
            this.textBoxKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxKeys.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxKeys.Location = new System.Drawing.Point(104, 165);
            this.textBoxKeys.Multiline = false;
            this.textBoxKeys.Name = "textBoxKeys";
            this.textBoxKeys.ReadOnly = true;
            this.textBoxKeys.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxKeys.SelectionLength = 0;
            this.textBoxKeys.SelectionStart = 0;
            this.textBoxKeys.Size = new System.Drawing.Size(255, 20);
            this.textBoxKeys.TabIndex = 2;
            this.textBoxKeys.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxKeys.WordWrap = true;
            // 
            // labelKeys
            // 
            this.labelKeys.AutoSize = true;
            this.labelKeys.Location = new System.Drawing.Point(29, 165);
            this.labelKeys.Name = "labelKeys";
            this.labelKeys.Size = new System.Drawing.Size(30, 13);
            this.labelKeys.TabIndex = 3;
            this.labelKeys.Text = "Keys";
            // 
            // buttonTest
            // 
            this.buttonTest.BorderColorScaling = 1.25F;
            this.buttonTest.ButtonColorScaling = 0.5F;
            this.buttonTest.ButtonDisabledScaling = 0.5F;
            this.buttonTest.Location = new System.Drawing.Point(29, 258);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 1;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // textBoxSendTo
            // 
            this.textBoxSendTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSendTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSendTo.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSendTo.BorderColorScaling = 0.5F;
            this.textBoxSendTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSendTo.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSendTo.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxSendTo.DropDownBorderColor = System.Drawing.Color.Gray;
            this.textBoxSendTo.DropDownHeight = 400;
            this.textBoxSendTo.DropDownItemHeight = 20;
            this.textBoxSendTo.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxSendTo.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxSendTo.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxSendTo.DropDownWidth = 0;
            this.textBoxSendTo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxSendTo.Location = new System.Drawing.Point(104, 208);
            this.textBoxSendTo.Multiline = false;
            this.textBoxSendTo.Name = "textBoxSendTo";
            this.textBoxSendTo.ReadOnly = false;
            this.textBoxSendTo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSendTo.SelectionLength = 0;
            this.textBoxSendTo.SelectionStart = 0;
            this.textBoxSendTo.Size = new System.Drawing.Size(255, 20);
            this.textBoxSendTo.TabIndex = 2;
            this.textBoxSendTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSendTo.WordWrap = true;
            this.textBoxSendTo.Enter += new System.EventHandler(this.textBoxSendTo_Enter);
            this.textBoxSendTo.Leave += new System.EventHandler(this.textBoxSendTo_Leave);
            // 
            // labelSendTo
            // 
            this.labelSendTo.AutoSize = true;
            this.labelSendTo.Location = new System.Drawing.Point(29, 208);
            this.labelSendTo.Name = "labelSendTo";
            this.labelSendTo.Size = new System.Drawing.Size(48, 13);
            this.labelSendTo.TabIndex = 3;
            this.labelSendTo.Text = "Send To";
            // 
            // buttonOK
            // 
            this.buttonOK.BorderColorScaling = 1.25F;
            this.buttonOK.ButtonColorScaling = 0.5F;
            this.buttonOK.ButtonDisabledScaling = 0.5F;
            this.buttonOK.Location = new System.Drawing.Point(289, 296);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(193, 296);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // KeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 336);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelSendTo);
            this.Controls.Add(this.labelKeys);
            this.Controls.Add(this.textBoxSendTo);
            this.Controls.Add(this.textBoxKeys);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.checkBoxKey);
            this.Controls.Add(this.checkBoxAlt);
            this.Controls.Add(this.checkBoxCtrl);
            this.Controls.Add(this.checkBoxShift);
            this.Name = "KeyForm";
            this.Text = "Define Keys";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.CheckBoxCustom checkBoxShift;
        private CheckBoxCustom checkBoxCtrl;
        private CheckBoxCustom checkBoxAlt;
        private CheckBoxCustom checkBoxKey;
        private ButtonExt buttonReset;
        private ButtonExt buttonNext;
        private TextBoxBorder textBoxKeys;
        private System.Windows.Forms.Label labelKeys;
        private ButtonExt buttonTest;
        private AutoCompleteTextBox textBoxSendTo;
        private System.Windows.Forms.Label labelSendTo;
        private ButtonExt buttonOK;
        private ButtonExt buttonCancel;
    }
}