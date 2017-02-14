namespace EDDiscovery.Speech
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxBorderText = new ExtendedControls.TextBoxBorder();
            this.checkBoxCustomComplete = new ExtendedControls.CheckBoxCustom();
            this.textBoxBorderVolume = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderRate = new ExtendedControls.TextBoxBorder();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.comboBoxCustomVoice = new ExtendedControls.ComboBoxCustom();
            this.label6 = new System.Windows.Forms.Label();
            this.panelOuter.SuspendLayout();
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
            this.label3.Location = new System.Drawing.Point(11, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Rate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Default, or 0-100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Default, or -10 to 10";
            // 
            // textBoxBorderText
            // 
            this.textBoxBorderText.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderText.BorderColorScaling = 0.5F;
            this.textBoxBorderText.Location = new System.Drawing.Point(15, 42);
            this.textBoxBorderText.Multiline = true;
            this.textBoxBorderText.Name = "textBoxBorderText";
            this.textBoxBorderText.Size = new System.Drawing.Size(400, 74);
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
            // textBoxBorderVolume
            // 
            this.textBoxBorderVolume.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderVolume.BorderColorScaling = 0.5F;
            this.textBoxBorderVolume.Location = new System.Drawing.Point(59, 213);
            this.textBoxBorderVolume.Name = "textBoxBorderVolume";
            this.textBoxBorderVolume.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderVolume.TabIndex = 3;
            // 
            // textBoxBorderRate
            // 
            this.textBoxBorderRate.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderRate.BorderColorScaling = 0.5F;
            this.textBoxBorderRate.Location = new System.Drawing.Point(59, 246);
            this.textBoxBorderRate.Name = "textBoxBorderRate";
            this.textBoxBorderRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderRate.TabIndex = 4;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.buttonExtCancel);
            this.panelOuter.Controls.Add(this.buttonExtOK);
            this.panelOuter.Controls.Add(this.comboBoxCustomVoice);
            this.panelOuter.Controls.Add(this.textBoxBorderRate);
            this.panelOuter.Controls.Add(this.textBoxBorderVolume);
            this.panelOuter.Controls.Add(this.checkBoxCustomComplete);
            this.panelOuter.Controls.Add(this.textBoxBorderText);
            this.panelOuter.Controls.Add(this.label4);
            this.panelOuter.Controls.Add(this.label2);
            this.panelOuter.Controls.Add(this.label3);
            this.panelOuter.Controls.Add(this.label6);
            this.panelOuter.Controls.Add(this.label1);
            this.panelOuter.Controls.Add(this.Title);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(431, 331);
            this.panelOuter.TabIndex = 0;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExtCancel.Location = new System.Drawing.Point(246, 289);
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
            this.buttonExtOK.Location = new System.Drawing.Point(340, 289);
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Voice";
            // 
            // SpeechConfigure
            // 
            this.AcceptButton = this.buttonExtOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(431, 331);
            this.Controls.Add(this.panelOuter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpeechConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Speech";
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private ExtendedControls.TextBoxBorder textBoxBorderText;
        private ExtendedControls.CheckBoxCustom checkBoxCustomComplete;
        private ExtendedControls.TextBoxBorder textBoxBorderVolume;
        private ExtendedControls.TextBoxBorder textBoxBorderRate;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.ComboBoxCustom comboBoxCustomVoice;
        private System.Windows.Forms.Label label6;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private ExtendedControls.ButtonExt buttonExtOK;
    }
}