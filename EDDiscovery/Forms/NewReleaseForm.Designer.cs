namespace EDDiscovery.Forms
{
    partial class NewReleaseForm
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
            this.textBoxReleaseName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGitHubURL = new System.Windows.Forms.TextBox();
            this.buttonUrlOpen = new System.Windows.Forms.Button();
            this.richTextBoxReleaseInfo = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExeInstaller = new System.Windows.Forms.Button();
            this.buttonPortablezip = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonMsiInstaller = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxReleaseName
            // 
            this.textBoxReleaseName.Location = new System.Drawing.Point(70, 12);
            this.textBoxReleaseName.Name = "textBoxReleaseName";
            this.textBoxReleaseName.ReadOnly = true;
            this.textBoxReleaseName.Size = new System.Drawing.Size(155, 20);
            this.textBoxReleaseName.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(0, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            this.labelName.Click += new System.EventHandler(this.labelName_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "GitHub url";
            // 
            // textBoxGitHubURL
            // 
            this.textBoxGitHubURL.Location = new System.Drawing.Point(72, 45);
            this.textBoxGitHubURL.Name = "textBoxGitHubURL";
            this.textBoxGitHubURL.ReadOnly = true;
            this.textBoxGitHubURL.Size = new System.Drawing.Size(278, 20);
            this.textBoxGitHubURL.TabIndex = 3;
            // 
            // buttonUrlOpen
            // 
            this.buttonUrlOpen.Location = new System.Drawing.Point(356, 43);
            this.buttonUrlOpen.Name = "buttonUrlOpen";
            this.buttonUrlOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonUrlOpen.TabIndex = 4;
            this.buttonUrlOpen.Text = "Open";
            this.buttonUrlOpen.UseVisualStyleBackColor = true;
            this.buttonUrlOpen.Click += new System.EventHandler(this.buttonUrlOpen_Click);
            // 
            // richTextBoxReleaseInfo
            // 
            this.richTextBoxReleaseInfo.Location = new System.Drawing.Point(72, 71);
            this.richTextBoxReleaseInfo.Name = "richTextBoxReleaseInfo";
            this.richTextBoxReleaseInfo.Size = new System.Drawing.Size(358, 180);
            this.richTextBoxReleaseInfo.TabIndex = 5;
            this.richTextBoxReleaseInfo.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Release info";
            // 
            // buttonExeInstaller
            // 
            this.buttonExeInstaller.Location = new System.Drawing.Point(72, 257);
            this.buttonExeInstaller.Name = "buttonExeInstaller";
            this.buttonExeInstaller.Size = new System.Drawing.Size(107, 23);
            this.buttonExeInstaller.TabIndex = 7;
            this.buttonExeInstaller.Text = "Exe installer";
            this.buttonExeInstaller.UseVisualStyleBackColor = true;
            this.buttonExeInstaller.Click += new System.EventHandler(this.buttonExeInstaller_Click);
            // 
            // buttonPortablezip
            // 
            this.buttonPortablezip.Location = new System.Drawing.Point(298, 257);
            this.buttonPortablezip.Name = "buttonPortablezip";
            this.buttonPortablezip.Size = new System.Drawing.Size(132, 23);
            this.buttonPortablezip.TabIndex = 8;
            this.buttonPortablezip.Text = "Portable Zip";
            this.buttonPortablezip.UseVisualStyleBackColor = true;
            this.buttonPortablezip.Click += new System.EventHandler(this.buttonPortablezip_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 262);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Download";
            // 
            // buttonMsiInstaller
            // 
            this.buttonMsiInstaller.Location = new System.Drawing.Point(185, 257);
            this.buttonMsiInstaller.Name = "buttonMsiInstaller";
            this.buttonMsiInstaller.Size = new System.Drawing.Size(107, 23);
            this.buttonMsiInstaller.TabIndex = 10;
            this.buttonMsiInstaller.Text = "Msi installer";
            this.buttonMsiInstaller.UseVisualStyleBackColor = true;
            this.buttonMsiInstaller.Click += new System.EventHandler(this.buttonMsiInstaller_Click);
            // 
            // NewReleaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 316);
            this.Controls.Add(this.buttonMsiInstaller);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonPortablezip);
            this.Controls.Add(this.buttonExeInstaller);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBoxReleaseInfo);
            this.Controls.Add(this.buttonUrlOpen);
            this.Controls.Add(this.textBoxGitHubURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxReleaseName);
            this.Name = "NewReleaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EDDiscovery release";
            this.Load += new System.EventHandler(this.NewReleaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxReleaseName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxGitHubURL;
        private System.Windows.Forms.Button buttonUrlOpen;
        private System.Windows.Forms.RichTextBox richTextBoxReleaseInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonExeInstaller;
        private System.Windows.Forms.Button buttonPortablezip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonMsiInstaller;
    }
}