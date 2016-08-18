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
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLoc = new System.Windows.Forms.TextBox();
            this.textBoxDir = new System.Windows.Forms.TextBox();
            this.textBoxZoom = new System.Windows.Forms.TextBox();
            this.textBoxFly = new System.Windows.Forms.TextBox();
            this.textBoxPan = new System.Windows.Forms.TextBox();
            this.textBoxDelta = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPauseHere = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
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
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Time to change to this pos";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Time to change direction";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 250);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Delay before next action allowed";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Zoom";
            // 
            // textBoxLoc
            // 
            this.textBoxLoc.Location = new System.Drawing.Point(196, 49);
            this.textBoxLoc.Name = "textBoxLoc";
            this.textBoxLoc.ReadOnly = true;
            this.textBoxLoc.Size = new System.Drawing.Size(186, 20);
            this.textBoxLoc.TabIndex = 1;
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
            this.textBoxZoom.Location = new System.Drawing.Point(196, 172);
            this.textBoxZoom.Name = "textBoxZoom";
            this.textBoxZoom.ReadOnly = true;
            this.textBoxZoom.Size = new System.Drawing.Size(100, 20);
            this.textBoxZoom.TabIndex = 1;
            // 
            // textBoxFly
            // 
            this.textBoxFly.Location = new System.Drawing.Point(196, 76);
            this.textBoxFly.Name = "textBoxFly";
            this.textBoxFly.Size = new System.Drawing.Size(100, 20);
            this.textBoxFly.TabIndex = 1;
            this.textBoxFly.TextChanged += new System.EventHandler(this.textBoxFly_TextChanged);
            // 
            // textBoxPan
            // 
            this.textBoxPan.Location = new System.Drawing.Point(196, 142);
            this.textBoxPan.Name = "textBoxPan";
            this.textBoxPan.Size = new System.Drawing.Size(100, 20);
            this.textBoxPan.TabIndex = 2;
            this.textBoxPan.TextChanged += new System.EventHandler(this.textBoxPan_TextChanged);
            // 
            // textBoxDelta
            // 
            this.textBoxDelta.Location = new System.Drawing.Point(196, 19);
            this.textBoxDelta.Name = "textBoxDelta";
            this.textBoxDelta.Size = new System.Drawing.Size(100, 20);
            this.textBoxDelta.TabIndex = 0;
            this.textBoxDelta.TextChanged += new System.EventHandler(this.textBoxDelta_TextChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(307, 331);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(211, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(196, 212);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(100, 20);
            this.textBoxMessage.TabIndex = 3;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(26, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Message when arrived";
            // 
            // textBoxPauseHere
            // 
            this.textBoxPauseHere.Location = new System.Drawing.Point(196, 250);
            this.textBoxPauseHere.Name = "textBoxPauseHere";
            this.textBoxPauseHere.Size = new System.Drawing.Size(100, 20);
            this.textBoxPauseHere.TabIndex = 4;
            this.textBoxPauseHere.TextChanged += new System.EventHandler(this.textBoxDelta_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Wait for this time before action";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 283);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(189, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "0 means maximum of the change times";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 305);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Empty means no minimum time";
            // 
            // RecordStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 362);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.textBoxPauseHere);
            this.Controls.Add(this.textBoxDelta);
            this.Controls.Add(this.textBoxPan);
            this.Controls.Add(this.textBoxZoom);
            this.Controls.Add(this.textBoxFly);
            this.Controls.Add(this.textBoxDir);
            this.Controls.Add(this.textBoxLoc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RecordStep";
            this.Text = "RecordStep";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLoc;
        private System.Windows.Forms.TextBox textBoxDir;
        private System.Windows.Forms.TextBox textBoxZoom;
        private System.Windows.Forms.TextBox textBoxFly;
        private System.Windows.Forms.TextBox textBoxPan;
        private System.Windows.Forms.TextBox textBoxDelta;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPauseHere;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}