namespace EDDiscovery2.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.linkLabelFDForum = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.linkLabelEDSM = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHubIssue = new System.Windows.Forms.LinkLabel();
            this.linkLabelEDDB = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(331, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(325, 212);
            this.panel1.TabIndex = 2;
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 19);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(142, 24);
            this.labelVersion.TabIndex = 1;
            this.labelVersion.Text = "EDDicscovery v";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(16, 46);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(309, 185);
            this.textBox1.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(581, 399);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // linkLabelFDForum
            // 
            this.linkLabelFDForum.AutoSize = true;
            this.linkLabelFDForum.Location = new System.Drawing.Point(57, 270);
            this.linkLabelFDForum.Name = "linkLabelFDForum";
            this.linkLabelFDForum.Size = new System.Drawing.Size(74, 13);
            this.linkLabelFDForum.TabIndex = 3;
            this.linkLabelFDForum.TabStop = true;
            this.linkLabelFDForum.Text = "Frontier Forum";
            this.linkLabelFDForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForum_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(42, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Links";
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Location = new System.Drawing.Point(57, 283);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(40, 13);
            this.linkLabelGitHub.TabIndex = 5;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "GitHub";
            this.linkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHub_LinkClicked);
            // 
            // linkLabelEDSM
            // 
            this.linkLabelEDSM.AutoSize = true;
            this.linkLabelEDSM.Location = new System.Drawing.Point(57, 309);
            this.linkLabelEDSM.Name = "linkLabelEDSM";
            this.linkLabelEDSM.Size = new System.Drawing.Size(38, 13);
            this.linkLabelEDSM.TabIndex = 6;
            this.linkLabelEDSM.TabStop = true;
            this.linkLabelEDSM.Text = "EDSM";
            this.linkLabelEDSM.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEDSM_LinkClicked);
            // 
            // linkLabelGitHubIssue
            // 
            this.linkLabelGitHubIssue.AutoSize = true;
            this.linkLabelGitHubIssue.Location = new System.Drawing.Point(57, 296);
            this.linkLabelGitHubIssue.Name = "linkLabelGitHubIssue";
            this.linkLabelGitHubIssue.Size = new System.Drawing.Size(139, 13);
            this.linkLabelGitHubIssue.TabIndex = 7;
            this.linkLabelGitHubIssue.TabStop = true;
            this.linkLabelGitHubIssue.Text = "GitHub - Report idea / issue";
            this.linkLabelGitHubIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHubIssue_LinkClicked);
            // 
            // linkLabelEDDB
            // 
            this.linkLabelEDDB.AutoSize = true;
            this.linkLabelEDDB.Location = new System.Drawing.Point(57, 322);
            this.linkLabelEDDB.Name = "linkLabelEDDB";
            this.linkLabelEDDB.Size = new System.Drawing.Size(37, 13);
            this.linkLabelEDDB.TabIndex = 8;
            this.linkLabelEDDB.TabStop = true;
            this.linkLabelEDDB.Text = "EDDB";
            this.linkLabelEDDB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEDDB_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(675, 434);
            this.Controls.Add(this.linkLabelEDDB);
            this.Controls.Add(this.linkLabelGitHubIssue);
            this.Controls.Add(this.linkLabelEDSM);
            this.Controls.Add(this.linkLabelGitHub);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelFDForum);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About EDDiscovery";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonOK;
        internal System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.LinkLabel linkLabelFDForum;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.LinkLabel linkLabelEDSM;
        private System.Windows.Forms.LinkLabel linkLabelGitHubIssue;
        private System.Windows.Forms.LinkLabel linkLabelEDDB;
    }
}