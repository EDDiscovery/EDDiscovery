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
            this.buttonDbs = new System.Windows.Forms.Button();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonPositions = new System.Windows.Forms.Button();
            this.buttonResetTheme = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDeleteSystemDB = new System.Windows.Forms.Button();
            this.buttonResetTabs = new System.Windows.Forms.Button();
            this.buttonRemoveDLLs = new System.Windows.Forms.Button();
            this.buttonActionPacks = new System.Windows.Forms.Button();
            this.buttonLang = new System.Windows.Forms.Button();
            this.buttonResetDBLoc = new System.Windows.Forms.Button();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.buttonDeleteUserDB = new System.Windows.Forms.Button();
            this.buttonRemoveJournals = new System.Windows.Forms.Button();
            this.buttonRemoveJournalsCommanders = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonDbs
            // 
            this.buttonDbs.Location = new System.Drawing.Point(55, 353);
            this.buttonDbs.Name = "buttonDbs";
            this.buttonDbs.Size = new System.Drawing.Size(250, 23);
            this.buttonDbs.TabIndex = 0;
            this.buttonDbs.Text = "Move Databases";
            this.buttonDbs.UseVisualStyleBackColor = true;
            this.buttonDbs.Click += new System.EventHandler(this.buttonDbs_Click);
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(55, 462);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(250, 23);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "Run";
            this.buttonRun.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.Run_Click);
            // 
            // buttonPositions
            // 
            this.buttonPositions.Location = new System.Drawing.Point(55, 40);
            this.buttonPositions.Name = "buttonPositions";
            this.buttonPositions.Size = new System.Drawing.Size(250, 23);
            this.buttonPositions.TabIndex = 0;
            this.buttonPositions.Text = "Reset Window Positions";
            this.buttonPositions.UseVisualStyleBackColor = true;
            this.buttonPositions.Click += new System.EventHandler(this.buttonPositions_Click);
            // 
            // buttonResetTheme
            // 
            this.buttonResetTheme.Location = new System.Drawing.Point(55, 10);
            this.buttonResetTheme.Name = "buttonResetTheme";
            this.buttonResetTheme.Size = new System.Drawing.Size(250, 23);
            this.buttonResetTheme.TabIndex = 0;
            this.buttonResetTheme.Text = "Reset Theme";
            this.buttonResetTheme.UseVisualStyleBackColor = true;
            this.buttonResetTheme.Click += new System.EventHandler(this.buttonResetTheme_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(55, 500);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(250, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Exit";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDeleteSystemDB
            // 
            this.buttonDeleteSystemDB.Location = new System.Drawing.Point(55, 201);
            this.buttonDeleteSystemDB.Name = "buttonDeleteSystemDB";
            this.buttonDeleteSystemDB.Size = new System.Drawing.Size(250, 23);
            this.buttonDeleteSystemDB.TabIndex = 0;
            this.buttonDeleteSystemDB.Text = "Delete/Rebuild System DB";
            this.buttonDeleteSystemDB.UseVisualStyleBackColor = true;
            this.buttonDeleteSystemDB.Click += new System.EventHandler(this.buttonDeleteSystemDB_Click);
            // 
            // buttonResetTabs
            // 
            this.buttonResetTabs.Location = new System.Drawing.Point(55, 70);
            this.buttonResetTabs.Name = "buttonResetTabs";
            this.buttonResetTabs.Size = new System.Drawing.Size(250, 23);
            this.buttonResetTabs.TabIndex = 0;
            this.buttonResetTabs.Text = "Reset Tabs, Remove PopOuts";
            this.buttonResetTabs.UseVisualStyleBackColor = true;
            this.buttonResetTabs.Click += new System.EventHandler(this.buttonResetTabs_Click);
            // 
            // buttonRemoveDLLs
            // 
            this.buttonRemoveDLLs.Location = new System.Drawing.Point(55, 100);
            this.buttonRemoveDLLs.Name = "buttonRemoveDLLs";
            this.buttonRemoveDLLs.Size = new System.Drawing.Size(250, 23);
            this.buttonRemoveDLLs.TabIndex = 0;
            this.buttonRemoveDLLs.Text = "Remove all Extension DLLs";
            this.buttonRemoveDLLs.UseVisualStyleBackColor = true;
            this.buttonRemoveDLLs.Click += new System.EventHandler(this.buttonRemoveDLLs_Click);
            // 
            // buttonActionPacks
            // 
            this.buttonActionPacks.Location = new System.Drawing.Point(55, 130);
            this.buttonActionPacks.Name = "buttonActionPacks";
            this.buttonActionPacks.Size = new System.Drawing.Size(250, 23);
            this.buttonActionPacks.TabIndex = 0;
            this.buttonActionPacks.Text = "Remove all Action Packs";
            this.buttonActionPacks.UseVisualStyleBackColor = true;
            this.buttonActionPacks.Click += new System.EventHandler(this.buttonActions_Click);
            // 
            // buttonLang
            // 
            this.buttonLang.Location = new System.Drawing.Point(55, 159);
            this.buttonLang.Name = "buttonLang";
            this.buttonLang.Size = new System.Drawing.Size(250, 23);
            this.buttonLang.TabIndex = 0;
            this.buttonLang.Text = "Reset Language to English";
            this.buttonLang.UseVisualStyleBackColor = true;
            this.buttonLang.Click += new System.EventHandler(this.buttonLang_Click);
            // 
            // buttonResetDBLoc
            // 
            this.buttonResetDBLoc.Location = new System.Drawing.Point(55, 410);
            this.buttonResetDBLoc.Name = "buttonResetDBLoc";
            this.buttonResetDBLoc.Size = new System.Drawing.Size(250, 23);
            this.buttonResetDBLoc.TabIndex = 0;
            this.buttonResetDBLoc.Text = "Reset DB Location";
            this.buttonResetDBLoc.UseVisualStyleBackColor = true;
            this.buttonResetDBLoc.Click += new System.EventHandler(this.buttonResetDBLoc_Click);
            // 
            // buttonBackup
            // 
            this.buttonBackup.Location = new System.Drawing.Point(55, 380);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(250, 23);
            this.buttonBackup.TabIndex = 0;
            this.buttonBackup.Text = "Backup Database";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // buttonDeleteUserDB
            // 
            this.buttonDeleteUserDB.Location = new System.Drawing.Point(55, 230);
            this.buttonDeleteUserDB.Name = "buttonDeleteUserDB";
            this.buttonDeleteUserDB.Size = new System.Drawing.Size(250, 23);
            this.buttonDeleteUserDB.TabIndex = 0;
            this.buttonDeleteUserDB.Text = "Delete/Rebuild User DB";
            this.buttonDeleteUserDB.UseVisualStyleBackColor = true;
            this.buttonDeleteUserDB.Click += new System.EventHandler(this.buttonDeleteUserDB_Click);
            // 
            // buttonRemoveJournals
            // 
            this.buttonRemoveJournals.Location = new System.Drawing.Point(55, 276);
            this.buttonRemoveJournals.Name = "buttonRemoveJournals";
            this.buttonRemoveJournals.Size = new System.Drawing.Size(250, 23);
            this.buttonRemoveJournals.TabIndex = 0;
            this.buttonRemoveJournals.Text = "Remove Journal Entries";
            this.buttonRemoveJournals.UseVisualStyleBackColor = true;
            this.buttonRemoveJournals.Click += new System.EventHandler(this.buttonRemoveJournals_Click);
            // 
            // buttonRemoveJournalsCommanders
            // 
            this.buttonRemoveJournalsCommanders.Location = new System.Drawing.Point(55, 305);
            this.buttonRemoveJournalsCommanders.Name = "buttonRemoveJournalsCommanders";
            this.buttonRemoveJournalsCommanders.Size = new System.Drawing.Size(250, 23);
            this.buttonRemoveJournalsCommanders.TabIndex = 0;
            this.buttonRemoveJournalsCommanders.Text = "Remove Journal Entries && Commanders";
            this.buttonRemoveJournalsCommanders.UseVisualStyleBackColor = true;
            this.buttonRemoveJournalsCommanders.Click += new System.EventHandler(this.buttonRemoveJournalsCommanders_Click);
            // 
            // SafeModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 545);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.buttonRemoveJournalsCommanders);
            this.Controls.Add(this.buttonRemoveJournals);
            this.Controls.Add(this.buttonDeleteUserDB);
            this.Controls.Add(this.buttonDeleteSystemDB);
            this.Controls.Add(this.buttonResetDBLoc);
            this.Controls.Add(this.buttonBackup);
            this.Controls.Add(this.buttonDbs);
            this.Controls.Add(this.buttonLang);
            this.Controls.Add(this.buttonActionPacks);
            this.Controls.Add(this.buttonRemoveDLLs);
            this.Controls.Add(this.buttonResetTabs);
            this.Controls.Add(this.buttonPositions);
            this.Controls.Add(this.buttonResetTheme);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "SafeModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery Safe Mode";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonDbs;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonPositions;
        private System.Windows.Forms.Button buttonResetTheme;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDeleteSystemDB;
        private System.Windows.Forms.Button buttonResetTabs;
        private System.Windows.Forms.Button buttonRemoveDLLs;
        private System.Windows.Forms.Button buttonActionPacks;
        private System.Windows.Forms.Button buttonLang;
        private System.Windows.Forms.Button buttonResetDBLoc;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.Button buttonDeleteUserDB;
        private System.Windows.Forms.Button buttonRemoveJournals;
        private System.Windows.Forms.Button buttonRemoveJournalsCommanders;
    }
}