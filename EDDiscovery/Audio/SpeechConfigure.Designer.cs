namespace EDDiscovery.Audio
{
    partial class SpeechConfigure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeechConfigure));
            this.Title = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBorderText = new ExtendedControls.TextBoxBorder();
            this.checkBoxCustomComplete = new ExtendedControls.CheckBoxCustom();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.buttonExtTest = new ExtendedControls.ButtonExt();
            this.buttonExtEffects = new ExtendedControls.ButtonExt();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.comboBoxCustomVoice = new ExtendedControls.ComboBoxCustom();
            this.textBoxBorderTest = new ExtendedControls.TextBoxBorder();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarRate = new System.Windows.Forms.TrackBar();
            this.checkBoxCustomR = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomV = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomPreempt = new ExtendedControls.CheckBoxCustom();
            this.panelOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(12, 12);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(35, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 216);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Volume";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Rate";
            // 
            // textBoxBorderText
            // 
            this.textBoxBorderText.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderText.BorderColorScaling = 0.5F;
            this.textBoxBorderText.Location = new System.Drawing.Point(14, 42);
            this.textBoxBorderText.Multiline = true;
            this.textBoxBorderText.Name = "textBoxBorderText";
            this.textBoxBorderText.Size = new System.Drawing.Size(401, 74);
            this.textBoxBorderText.TabIndex = 0;
            this.textBoxBorderText.WordWrap = false;
            // 
            // checkBoxCustomComplete
            // 
            this.checkBoxCustomComplete.AutoSize = true;
            this.checkBoxCustomComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomComplete.FontNerfReduction = 0.5F;
            this.checkBoxCustomComplete.Location = new System.Drawing.Point(15, 132);
            this.checkBoxCustomComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomComplete.Name = "checkBoxCustomComplete";
            this.checkBoxCustomComplete.Size = new System.Drawing.Size(159, 17);
            this.checkBoxCustomComplete.TabIndex = 1;
            this.checkBoxCustomComplete.Text = "Wait until speech completes";
            this.checkBoxCustomComplete.TickBoxReductionSize = 10;
            this.checkBoxCustomComplete.UseVisualStyleBackColor = true;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.buttonExtTest);
            this.panelOuter.Controls.Add(this.buttonExtEffects);
            this.panelOuter.Controls.Add(this.trackBarVolume);
            this.panelOuter.Controls.Add(this.buttonExtCancel);
            this.panelOuter.Controls.Add(this.buttonExtOK);
            this.panelOuter.Controls.Add(this.comboBoxCustomVoice);
            this.panelOuter.Controls.Add(this.checkBoxCustomPreempt);
            this.panelOuter.Controls.Add(this.checkBoxCustomComplete);
            this.panelOuter.Controls.Add(this.textBoxBorderTest);
            this.panelOuter.Controls.Add(this.textBoxBorderText);
            this.panelOuter.Controls.Add(this.label3);
            this.panelOuter.Controls.Add(this.label6);
            this.panelOuter.Controls.Add(this.label1);
            this.panelOuter.Controls.Add(this.trackBarRate);
            this.panelOuter.Controls.Add(this.checkBoxCustomR);
            this.panelOuter.Controls.Add(this.checkBoxCustomV);
            this.panelOuter.Controls.Add(this.Title);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(437, 421);
            this.panelOuter.TabIndex = 0;
            // 
            // buttonExtTest
            // 
            this.buttonExtTest.BorderColorScaling = 1.25F;
            this.buttonExtTest.ButtonColorScaling = 0.5F;
            this.buttonExtTest.ButtonDisabledScaling = 0.5F;
            this.buttonExtTest.Location = new System.Drawing.Point(340, 327);
            this.buttonExtTest.Name = "buttonExtTest";
            this.buttonExtTest.Size = new System.Drawing.Size(75, 23);
            this.buttonExtTest.TabIndex = 42;
            this.buttonExtTest.Text = "Test";
            this.buttonExtTest.UseVisualStyleBackColor = true;
            this.buttonExtTest.Click += new System.EventHandler(this.buttonExtTest_Click);
            // 
            // buttonExtEffects
            // 
            this.buttonExtEffects.BorderColorScaling = 1.25F;
            this.buttonExtEffects.ButtonColorScaling = 0.5F;
            this.buttonExtEffects.ButtonDisabledScaling = 0.5F;
            this.buttonExtEffects.Location = new System.Drawing.Point(340, 166);
            this.buttonExtEffects.Name = "buttonExtEffects";
            this.buttonExtEffects.Size = new System.Drawing.Size(75, 23);
            this.buttonExtEffects.TabIndex = 42;
            this.buttonExtEffects.Text = "Effects";
            this.buttonExtEffects.UseVisualStyleBackColor = true;
            this.buttonExtEffects.Click += new System.EventHandler(this.buttonExtEffects_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(59, 204);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(203, 45);
            this.trackBarVolume.TabIndex = 7;
            this.trackBarVolume.TickFrequency = 5;
            this.trackBarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarVolume.Value = 60;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExtCancel.Location = new System.Drawing.Point(249, 383);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 6;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(340, 383);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
            this.buttonExtOK.TabIndex = 5;
            this.buttonExtOK.Text = "OK";
            this.buttonExtOK.UseVisualStyleBackColor = true;
            this.buttonExtOK.Click += new System.EventHandler(this.buttonExtOK_Click);
            // 
            // comboBoxCustomVoice
            // 
            this.comboBoxCustomVoice.ArrowWidth = 1;
            this.comboBoxCustomVoice.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomVoice.ButtonColorScaling = 0.5F;
            this.comboBoxCustomVoice.DataSource = null;
            this.comboBoxCustomVoice.DisplayMember = "";
            this.comboBoxCustomVoice.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomVoice.DropDownHeight = 106;
            this.comboBoxCustomVoice.DropDownWidth = 203;
            this.comboBoxCustomVoice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomVoice.ItemHeight = 13;
            this.comboBoxCustomVoice.Location = new System.Drawing.Point(59, 166);
            this.comboBoxCustomVoice.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomVoice.Name = "comboBoxCustomVoice";
            this.comboBoxCustomVoice.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomVoice.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomVoice.ScrollBarWidth = 16;
            this.comboBoxCustomVoice.SelectedIndex = -1;
            this.comboBoxCustomVoice.SelectedItem = null;
            this.comboBoxCustomVoice.SelectedValue = null;
            this.comboBoxCustomVoice.Size = new System.Drawing.Size(203, 23);
            this.comboBoxCustomVoice.TabIndex = 2;
            this.comboBoxCustomVoice.Text = "comboBoxCustom1";
            this.comboBoxCustomVoice.ValueMember = "";
            // 
            // textBoxBorderTest
            // 
            this.textBoxBorderTest.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderTest.BorderColorScaling = 0.5F;
            this.textBoxBorderTest.Location = new System.Drawing.Point(15, 327);
            this.textBoxBorderTest.Multiline = true;
            this.textBoxBorderTest.Name = "textBoxBorderTest";
            this.textBoxBorderTest.Size = new System.Drawing.Size(247, 38);
            this.textBoxBorderTest.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Voice";
            // 
            // trackBarRate
            // 
            this.trackBarRate.LargeChange = 2;
            this.trackBarRate.Location = new System.Drawing.Point(59, 261);
            this.trackBarRate.Minimum = -10;
            this.trackBarRate.Name = "trackBarRate";
            this.trackBarRate.Size = new System.Drawing.Size(203, 45);
            this.trackBarRate.TabIndex = 41;
            this.trackBarRate.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // checkBoxCustomR
            // 
            this.checkBoxCustomR.AutoSize = true;
            this.checkBoxCustomR.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomR.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomR.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomR.FontNerfReduction = 0.5F;
            this.checkBoxCustomR.Location = new System.Drawing.Point(271, 274);
            this.checkBoxCustomR.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomR.Name = "checkBoxCustomR";
            this.checkBoxCustomR.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomR.TabIndex = 8;
            this.checkBoxCustomR.Text = "Override";
            this.checkBoxCustomR.TickBoxReductionSize = 10;
            this.checkBoxCustomR.UseVisualStyleBackColor = true;
            this.checkBoxCustomR.CheckedChanged += new System.EventHandler(this.checkBoxCustomR_CheckedChanged);
            // 
            // checkBoxCustomV
            // 
            this.checkBoxCustomV.AutoSize = true;
            this.checkBoxCustomV.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomV.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomV.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomV.FontNerfReduction = 0.5F;
            this.checkBoxCustomV.Location = new System.Drawing.Point(271, 216);
            this.checkBoxCustomV.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomV.Name = "checkBoxCustomV";
            this.checkBoxCustomV.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomV.TabIndex = 8;
            this.checkBoxCustomV.Text = "Override";
            this.checkBoxCustomV.TickBoxReductionSize = 10;
            this.checkBoxCustomV.UseVisualStyleBackColor = true;
            this.checkBoxCustomV.CheckedChanged += new System.EventHandler(this.checkBoxCustomV_CheckedChanged);
            // 
            // checkBoxCustomPreempt
            // 
            this.checkBoxCustomPreempt.AutoSize = true;
            this.checkBoxCustomPreempt.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomPreempt.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomPreempt.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomPreempt.FontNerfReduction = 0.5F;
            this.checkBoxCustomPreempt.Location = new System.Drawing.Point(210, 132);
            this.checkBoxCustomPreempt.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomPreempt.Name = "checkBoxCustomPreempt";
            this.checkBoxCustomPreempt.Size = new System.Drawing.Size(116, 17);
            this.checkBoxCustomPreempt.TabIndex = 1;
            this.checkBoxCustomPreempt.Text = "Preempt all speech";
            this.checkBoxCustomPreempt.TickBoxReductionSize = 10;
            this.checkBoxCustomPreempt.UseVisualStyleBackColor = true;
            // 
            // SpeechConfigure
            // 
            this.AcceptButton = this.buttonExtOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(437, 421);
            this.Controls.Add(this.panelOuter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpeechConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Speech";
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.TextBoxBorder textBoxBorderText;
        private ExtendedControls.CheckBoxCustom checkBoxCustomComplete;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.ComboBoxCustom comboBoxCustomVoice;
        private System.Windows.Forms.Label label6;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private ExtendedControls.ButtonExt buttonExtOK;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.TrackBar trackBarRate;
        private ExtendedControls.ButtonExt buttonExtEffects;
        private ExtendedControls.CheckBoxCustom checkBoxCustomR;
        private ExtendedControls.CheckBoxCustom checkBoxCustomV;
        private ExtendedControls.ButtonExt buttonExtTest;
        private ExtendedControls.TextBoxBorder textBoxBorderTest;
        private ExtendedControls.CheckBoxCustom checkBoxCustomPreempt;
    }
}