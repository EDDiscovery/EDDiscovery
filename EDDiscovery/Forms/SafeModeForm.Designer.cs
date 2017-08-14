namespace EDDiscovery.Forms
{
    partial class SafeModeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SafeModeForm));
            this.buttonDbs = new System.Windows.Forms.Button();
            this.buttonNormal = new System.Windows.Forms.Button();
            this.buttonPositions = new System.Windows.Forms.Button();
            this.buttonResetTheme = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDbs
            // 
            this.buttonDbs.Location = new System.Drawing.Point(89, 120);
            this.buttonDbs.Name = "buttonDbs";
            this.buttonDbs.Size = new System.Drawing.Size(193, 23);
            this.buttonDbs.TabIndex = 0;
            this.buttonDbs.Text = "Move Databases";
            this.buttonDbs.UseVisualStyleBackColor = true;
            this.buttonDbs.Click += new System.EventHandler(this.buttonDbs_Click);
            // 
            // buttonNormal
            // 
            this.buttonNormal.Location = new System.Drawing.Point(89, 160);
            this.buttonNormal.Name = "buttonNormal";
            this.buttonNormal.Size = new System.Drawing.Size(193, 23);
            this.buttonNormal.TabIndex = 0;
            this.buttonNormal.Text = "Run";
            this.buttonNormal.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNormal.UseVisualStyleBackColor = true;
            this.buttonNormal.Click += new System.EventHandler(this.Run_Click);
            // 
            // buttonPositions
            // 
            this.buttonPositions.Location = new System.Drawing.Point(89, 80);
            this.buttonPositions.Name = "buttonPositions";
            this.buttonPositions.Size = new System.Drawing.Size(193, 23);
            this.buttonPositions.TabIndex = 0;
            this.buttonPositions.Text = "Reset Window Positions";
            this.buttonPositions.UseVisualStyleBackColor = true;
            this.buttonPositions.Click += new System.EventHandler(this.buttonPositions_Click);
            // 
            // buttonResetTheme
            // 
            this.buttonResetTheme.Location = new System.Drawing.Point(89, 40);
            this.buttonResetTheme.Name = "buttonResetTheme";
            this.buttonResetTheme.Size = new System.Drawing.Size(193, 23);
            this.buttonResetTheme.TabIndex = 0;
            this.buttonResetTheme.Text = "Reset Theme";
            this.buttonResetTheme.UseVisualStyleBackColor = true;
            this.buttonResetTheme.Click += new System.EventHandler(this.buttonResetTheme_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(89, 200);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(193, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // SafeModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 252);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonNormal);
            this.Controls.Add(this.buttonDbs);
            this.Controls.Add(this.buttonPositions);
            this.Controls.Add(this.buttonResetTheme);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SafeModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery Safe Mode";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonDbs;
        private System.Windows.Forms.Button buttonNormal;
        private System.Windows.Forms.Button buttonPositions;
        private System.Windows.Forms.Button buttonResetTheme;
        private System.Windows.Forms.Button buttonCancel;
    }
}