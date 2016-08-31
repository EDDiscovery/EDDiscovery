namespace EDDiscovery2
{
    partial class RecordStep
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordStep));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPos = new System.Windows.Forms.TextBox();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.textBoxZoom = new System.Windows.Forms.TextBox();
            this.textBoxFlyTime = new System.Windows.Forms.TextBox();
            this.textBoxPanTime = new System.Windows.Forms.TextBox();
            this.textBoxWait = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxZoomTime = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxMsgTime = new System.Windows.Forms.TextBox();
            this.checkBoxWaitForSlew = new System.Windows.Forms.CheckBox();
            this.checkBoxPos = new System.Windows.Forms.CheckBox();
            this.checkBoxPan = new System.Windows.Forms.CheckBox();
            this.checkBoxChangeZoom = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.checkBoxWaitComplete = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayMessageWhenComplete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Go to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Pointing At";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Time to take to change pos";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Time to take to change direction";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Zoom";
            // 
            // textBoxPos
            // 
            this.textBoxPos.Location = new System.Drawing.Point(196, 49);
            this.textBoxPos.Name = "textBoxPos";
            this.textBoxPos.ReadOnly = true;
            this.textBoxPos.Size = new System.Drawing.Size(186, 20);
            this.textBoxPos.TabIndex = 1;
            // 
            // textBoxDir
            // 
            this.textBoxDir.Location = new System.Drawing.Point(196, 112);
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.ReadOnly = true;
            this.textBoxDir.Size = new System.Drawing.Size(186, 20);
            this.textBoxDir.TabIndex = 1;
            // 
            // textBoxZoom
            // 
            this.textBoxZoom.Location = new System.Drawing.Point(196, 182);
            this.textBoxZoom.Name = "textBoxZoom";
            this.textBoxZoom.ReadOnly = true;
            this.textBoxZoom.Size = new System.Drawing.Size(100, 20);
            this.textBoxZoom.TabIndex = 1;
            // 
            // textBoxFlyTime
            // 
            this.textBoxFlyTime.Location = new System.Drawing.Point(196, 76);
            this.textBoxFlyTime.Name = "textBoxFlyTime";
            this.textBoxFlyTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxFlyTime.TabIndex = 1;
            this.textBoxFlyTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxPanTime
            // 
            this.textBoxPanTime.Location = new System.Drawing.Point(196, 142);
            this.textBoxPanTime.Name = "textBoxPanTime";
            this.textBoxPanTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxPanTime.TabIndex = 2;
            this.textBoxPanTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxWait
            // 
            this.textBoxWait.Location = new System.Drawing.Point(196, 12);
            this.textBoxWait.Name = "textBoxWait";
            this.textBoxWait.Size = new System.Drawing.Size(100, 20);
            this.textBoxWait.TabIndex = 0;
            this.textBoxWait.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(302, 391);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(198, 391);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(196, 248);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(186, 20);
            this.textBoxMessage.TabIndex = 3;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(25, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Message to display";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Wait for this time before action";
            // 
            // textBoxZoomTime
            // 
            this.textBoxZoomTime.Location = new System.Drawing.Point(196, 208);
            this.textBoxZoomTime.Name = "textBoxZoomTime";
            this.textBoxZoomTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxZoomTime.TabIndex = 2;
            this.textBoxZoomTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 208);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Time to take to Zoom";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 280);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Message on screen time";
            // 
            // textBoxMsgTime
            // 
            this.textBoxMsgTime.Location = new System.Drawing.Point(195, 277);
            this.textBoxMsgTime.Name = "textBoxMsgTime";
            this.textBoxMsgTime.Size = new System.Drawing.Size(101, 20);
            this.textBoxMsgTime.TabIndex = 3;
            this.textBoxMsgTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // checkBoxWaitForSlew
            // 
            this.checkBoxWaitForSlew.AutoSize = true;
            this.checkBoxWaitForSlew.Location = new System.Drawing.Point(302, 15);
            this.checkBoxWaitForSlew.Name = "checkBoxWaitForSlew";
            this.checkBoxWaitForSlew.Size = new System.Drawing.Size(87, 17);
            this.checkBoxWaitForSlew.TabIndex = 7;
            this.checkBoxWaitForSlew.Text = "Wait for slew";
            this.checkBoxWaitForSlew.UseVisualStyleBackColor = true;
            this.checkBoxWaitForSlew.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // checkBoxPos
            // 
            this.checkBoxPos.AutoSize = true;
            this.checkBoxPos.Location = new System.Drawing.Point(302, 80);
            this.checkBoxPos.Name = "checkBoxPos";
            this.checkBoxPos.Size = new System.Drawing.Size(56, 17);
            this.checkBoxPos.TabIndex = 7;
            this.checkBoxPos.Text = "Go To";
            this.checkBoxPos.UseVisualStyleBackColor = true;
            this.checkBoxPos.CheckedChanged += new System.EventHandler(this.checkBoxGoTo_CheckedChanged);
            // 
            // checkBoxPan
            // 
            this.checkBoxPan.AutoSize = true;
            this.checkBoxPan.Location = new System.Drawing.Point(302, 145);
            this.checkBoxPan.Name = "checkBoxPan";
            this.checkBoxPan.Size = new System.Drawing.Size(79, 17);
            this.checkBoxPan.TabIndex = 7;
            this.checkBoxPan.Text = "Change Dir";
            this.checkBoxPan.UseVisualStyleBackColor = true;
            this.checkBoxPan.CheckedChanged += new System.EventHandler(this.checkBoxChangeDir_CheckedChanged);
            // 
            // checkBoxChangeZoom
            // 
            this.checkBoxChangeZoom.AutoSize = true;
            this.checkBoxChangeZoom.Location = new System.Drawing.Point(302, 211);
            this.checkBoxChangeZoom.Name = "checkBoxChangeZoom";
            this.checkBoxChangeZoom.Size = new System.Drawing.Size(93, 17);
            this.checkBoxChangeZoom.TabIndex = 7;
            this.checkBoxChangeZoom.Text = "Change Zoom";
            this.checkBoxChangeZoom.UseVisualStyleBackColor = true;
            this.checkBoxChangeZoom.CheckedChanged += new System.EventHandler(this.checkBoxChangeZoom_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(303, 284);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "0=default time";
            // 
            // checkBoxWaitComplete
            // 
            this.checkBoxWaitComplete.AutoSize = true;
            this.checkBoxWaitComplete.Location = new System.Drawing.Point(198, 317);
            this.checkBoxWaitComplete.Name = "checkBoxWaitComplete";
            this.checkBoxWaitComplete.Size = new System.Drawing.Size(158, 17);
            this.checkBoxWaitComplete.TabIndex = 7;
            this.checkBoxWaitComplete.Text = "Wait for actions to complete";
            this.checkBoxWaitComplete.UseVisualStyleBackColor = true;
            this.checkBoxWaitComplete.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // checkBoxDisplayMessageWhenComplete
            // 
            this.checkBoxDisplayMessageWhenComplete.AutoSize = true;
            this.checkBoxDisplayMessageWhenComplete.Location = new System.Drawing.Point(195, 349);
            this.checkBoxDisplayMessageWhenComplete.Name = "checkBoxDisplayMessageWhenComplete";
            this.checkBoxDisplayMessageWhenComplete.Size = new System.Drawing.Size(174, 17);
            this.checkBoxDisplayMessageWhenComplete.TabIndex = 7;
            this.checkBoxDisplayMessageWhenComplete.Text = "Display Message with complete";
            this.checkBoxDisplayMessageWhenComplete.UseVisualStyleBackColor = true;
            this.checkBoxDisplayMessageWhenComplete.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // RecordStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 426);
            this.Controls.Add(this.checkBoxChangeZoom);
            this.Controls.Add(this.checkBoxPan);
            this.Controls.Add(this.checkBoxPos);
            this.Controls.Add(this.checkBoxDisplayMessageWhenComplete);
            this.Controls.Add(this.checkBoxWaitComplete);
            this.Controls.Add(this.checkBoxWaitForSlew);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxMsgTime);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.textBoxWait);
            this.Controls.Add(this.textBoxZoomTime);
            this.Controls.Add(this.textBoxPanTime);
            this.Controls.Add(this.textBoxZoom);
            this.Controls.Add(this.textBoxFlyTime);
            this.Controls.Add(this.textBoxDir);
            this.Controls.Add(this.textBoxPos);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecordStep";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RecordStep";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPos;
        private System.Windows.Forms.TextBox textBoxDir;
        private System.Windows.Forms.TextBox textBoxZoom;
        private System.Windows.Forms.TextBox textBoxFlyTime;
        private System.Windows.Forms.TextBox textBoxPanTime;
        private System.Windows.Forms.TextBox textBoxWait;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxZoomTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxMsgTime;
        private System.Windows.Forms.CheckBox checkBoxWaitForSlew;
        private System.Windows.Forms.CheckBox checkBoxPos;
        private System.Windows.Forms.CheckBox checkBoxPan;
        private System.Windows.Forms.CheckBox checkBoxChangeZoom;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBoxWaitComplete;
        private System.Windows.Forms.CheckBox checkBoxDisplayMessageWhenComplete;
    }
}