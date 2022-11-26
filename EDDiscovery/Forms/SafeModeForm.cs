/*
 * Copyright © 2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore.DB;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class SafeModeForm : Form
    {
        public Action<bool, bool, bool,bool> Run;
        bool theme = false;
        bool pos = false;
        bool resettabs = false;
        bool resetlang = false;

        public SafeModeForm()
        {
            InitializeComponent();

            BaseUtils.Translator tx = new BaseUtils.Translator();
            tx.LoadTranslation("Auto", CultureInfo.CurrentUICulture, new string[] { System.IO.Path.GetDirectoryName(Application.ExecutablePath) }, 0,System.IO.Path.GetTempPath());
            //tx.LoadTranslation("example-ex", CultureInfo.CurrentUICulture, new string[] { @"c:\code\eddiscovery\eddiscovery\translations" }, 2,System.IO.Path.GetTempPath());

            var enumlist = new Enum[] { EDTx.SafeModeForm, EDTx.SafeModeForm_buttonCancel, EDTx.SafeModeForm_buttonRun, EDTx.SafeModeForm_buttonRemoveJournals, 
                                        EDTx.SafeModeForm_buttonDeleteUserDB, EDTx.SafeModeForm_buttonDeleteSystemDB, EDTx.SafeModeForm_buttonResetDBLoc, 
                                        EDTx.SafeModeForm_buttonBackup, EDTx.SafeModeForm_buttonDbs, EDTx.SafeModeForm_buttonLang, 
                                        EDTx.SafeModeForm_buttonActionPacks, EDTx.SafeModeForm_buttonRemoveDLLs, EDTx.SafeModeForm_buttonResetTabs, 
                                        EDTx.SafeModeForm_buttonPositions, EDTx.SafeModeForm_buttonResetTheme, EDTx.SafeModeForm_buttonRemoveJournalsCommanders };
            tx.TranslateControls(this, enumlist);
        }

        public SafeModeForm(bool userdbgood) : this()
        {
            buttonRun.Enabled = buttonResetTheme.Enabled = buttonActionPacks.Enabled = buttonBackup.Enabled =
            buttonPositions.Enabled = buttonResetTabs.Enabled = buttonRemoveDLLs.Enabled = buttonLang.Enabled = buttonDbs.Enabled =
            buttonRemoveJournals.Enabled = userdbgood;       // can't do this if can't run
        }

        private void Run_Click(object sender, EventArgs e)
        {
            Run(pos,theme,resettabs,resetlang);
        }

        private void buttonResetTheme_Click(object sender, EventArgs e)
        {
            theme = !theme;
            buttonResetTheme.Enabled = false;
        }

        private void buttonPositions_Click(object sender, EventArgs e)
        {
            pos = !pos;
            buttonPositions.Enabled = false;
        }

        private void buttonResetTabs_Click(object sender, EventArgs e)
        {
            resettabs = !resettabs;
            buttonResetTabs.Enabled = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonDbs_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            if (File.Exists(opt.UserDatabasePath) && File.Exists(opt.SystemDatabasePath))
            {
                if (MessageBox.Show(this, "Current databases are located at:" + Environment.NewLine + Environment.NewLine + 
                                "User: " + opt.UserDatabasePath + Environment.NewLine + "System: " + opt.SystemDatabasePath +
                                Environment.NewLine + Environment.NewLine + "Do you wish to change their location?", "Move Databases", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    buttonRun.Visible = false;      // can't run, must exit

                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.Description = "Select new folder";

                    if (fbd.ShowDialog(this) == DialogResult.OK) 
                    {

                        string pathto = fbd.SelectedPath;
                        //string pathto = @"c:\code";   // debug

                        string userfilename = Path.Combine(pathto, Path.GetFileName(opt.UserDatabasePath));
                        string systemfilename = Path.Combine(pathto, Path.GetFileName(opt.SystemDatabasePath));
                        string optionfile = Path.Combine(opt.AppDataDirectory, "dboptions.txt");

                        ExtendedControls.MessageBoxTheme mbt = ExtendedControls.MessageBoxTheme.ShowModeless(this, "Copying User DB", "Copy databases", MessageBoxIcon.Information, this.Icon);

                        try
                        {
                            
                            File.Copy(opt.UserDatabasePath, userfilename, true);
                            mbt.MessageText = "Copying System DB";
                            File.Copy(opt.SystemDatabasePath, systemfilename, true);

                            // this file tells us where they are now, placed in the appdatadirectory                
                            File.WriteAllLines(optionfile, new string[] { "userdbpath " + userfilename, "systemsdbpath " + systemfilename });

                            mbt.Close();

                            if (MessageBox.Show(this, "Copy of databases to " + pathto + " succeeded." + Environment.NewLine +
                                                       "EDD will use the databases from this location from now on." + Environment.NewLine +
                                                       Environment.NewLine +
                                                       "Confirm you wish to delete the DB from their old location?", "Move Databases",
                                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                            {
                                File.Delete(opt.UserDatabasePath);
                                File.Delete(opt.SystemDatabasePath);
                                EDDOptions.Instance.ReRead();
                            }
                        }
                        catch
                        {
                            mbt.Close();
                            MessageBox.Show(this, "Copy failed! Check Path", "EDDiscovery", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            else
                MessageBox.Show(this, "You need to run EDD first and let it create the dBs before it can move them!", "Move Databases", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            if (File.Exists(opt.UserDatabasePath))
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select folder to backup to";
                    folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string localtime = DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss");
                            string topath = Path.Combine(folderDialog.SelectedPath, "EDDUser." + localtime + ".sqlite");
                            File.Copy(opt.UserDatabasePath, topath);
                            MessageBox.Show(this, "Backup made to " + topath, "EDDiscovery", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            MessageBox.Show(this, "Copy failed, check destination folder", "EDDiscovery", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
                MessageBox.Show(this, "No user database present", "EDDiscovery", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }
        private void buttonDeleteSystemDB_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            if (File.Exists(opt.SystemDatabasePath))
            {
                if (MessageBox.Show(this, "Current system database is located at:" + Environment.NewLine + Environment.NewLine +
                                "System: " + opt.SystemDatabasePath +
                                Environment.NewLine + Environment.NewLine + "Do you wish to delete this and let EDD rebuild it" + Environment.NewLine +
                                "No user settings will be lost" + Environment.NewLine + 
                                "Afterwards, Exit and restart EDD",
                                "Delete/Rebuild System Database", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    File.Delete(opt.SystemDatabasePath);
                    buttonRun.Visible = false;      // can't run, must exit
                }
            }
            else
                MessageBox.Show(this, "You need to run EDD first and let it create the dBs before it can delete any!", "Delete/Rebuild System Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void buttonDeleteUserDB_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            if (File.Exists(opt.UserDatabasePath))
            {
                if (MessageBox.Show(this, "Current user database is located at:" + Environment.NewLine + Environment.NewLine +
                                "User: " + opt.UserDatabasePath +
                                Environment.NewLine + Environment.NewLine + "Do you wish to delete this and let EDD rebuild it" + Environment.NewLine +
                                "**All user settings will be lost**" + Environment.NewLine +
                                "Afterwards, Exit and restart EDD",
                                "Delete/Rebuild User Database", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    File.Delete(opt.UserDatabasePath);
                    buttonRun.Visible = false;      // can't run, must exit
                }
            }
            else
                MessageBox.Show(this, "You need to run EDD first and let it create the dBs before it can delete any!", "Delete/Rebuild User Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void buttonResetDBLoc_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            string optionfile = Path.Combine(opt.AppDataDirectory, "dboptions.txt");

            if (File.Exists(optionfile))
            {
                if (MessageBox.Show(this, "Current databases are located at:" + Environment.NewLine + Environment.NewLine +
                                "User: " + opt.UserDatabasePath + Environment.NewLine + "System: " + opt.SystemDatabasePath +
                                Environment.NewLine + Environment.NewLine +
                                "Do you wish to change their back to the default in " + EDDiscovery.EDDOptions.Instance.AppDataDirectory + "?",
                                "Reset Databases", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    BaseUtils.FileHelpers.DeleteFileNoError(optionfile);
                    EDDiscovery.EDDOptions.Instance.ResetSystemDatabasePath();
                    EDDiscovery.EDDOptions.Instance.ResetUserDatabasePath();

                    buttonRun.Visible = false;      // can't run, must exit
                }
            }
            else
            {
                MessageBox.Show(this, "There is no dboptions.txt file present to override the DB locations", "Reset Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void buttonRemoveDLLs_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            string dllfolder = opt.DLLAppDirectory();

            if (MessageBox.Show(this, "Current add on DLL folder is located at:" + Environment.NewLine  +
                            dllfolder + 
                            Environment.NewLine + Environment.NewLine + "Do you wish to delete all DLLs?",
                            "Delete Extension DLLs", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
            {
                var dir = new DirectoryInfo(dllfolder);
                foreach (var file in dir.EnumerateFiles("*.dll"))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch { }
                }
            }
        }

        private void buttonActions_Click(object sender, EventArgs e)
        {
            EDDiscovery.EDDOptions opt = EDDiscovery.EDDOptions.Instance;

            string apfolder = opt.ActionsAppDirectory();

            if (MessageBox.Show(this, "Current Action Pack folder is located at:" + Environment.NewLine +
                            apfolder +
                            Environment.NewLine + Environment.NewLine + "Do you wish to delete all Action Packs?",
                            "Delete Action Packs", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
            {
                var dir = new DirectoryInfo(apfolder);
                foreach (var file in dir.EnumerateFiles("*.act"))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch { }
                }
            }

        }

        private void buttonLang_Click(object sender, EventArgs e)
        {
            resetlang = true;
            buttonLang.Enabled = false;
        }

        private void buttonRemoveJournals_Click(object sender, EventArgs e)
        {
            if (UserDatabase.Instance.Name != "UserDB")       // this means never initialised.. as we never got to set the name. See EDDApplicationContext. If it is, we should not be enabled..
            {
                if (MessageBox.Show(this, "Confirm you want all journal entries removed from the DB" + Environment.NewLine +
                                "This will keep all other settings. Make sure you still have all your Frontier Journal logs before you do this." + Environment.NewLine +
                                "EDD on start will then rescan any journal logs it finds from the folder locations of commanders listed in the settings panels" + Environment.NewLine + 
                                "Notes will be kept but may be on the wrong entries",
                                "Delete Journal Entries", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    UserDatabase.Instance.Initialize();
                    UserDatabase.Instance.ClearJournals();
                }
            }
        }

        private void buttonRemoveJournalsCommanders_Click(object sender, EventArgs e)
        {
            if (UserDatabase.Instance.Name != "UserDB")       // this means never initialised.. as we never got to set the name. See EDDApplicationContext. If it is, we should not be enabled..
            {
                if (MessageBox.Show(this, "Confirm you want all journal entries and commanders removed from the DB" + Environment.NewLine +
                                "This will keep all other settings. Make sure you still have all your Frontier Journal logs before you do this." + Environment.NewLine +
                                "EDD on start will then rescan any journal logs it finds in the standard Elite folders and create only commanders found in those logs" + Environment.NewLine +
                                "Notes will be kept but may be on the wrong entries",
                                "Delete Journal Entries", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                {
                    UserDatabase.Instance.Initialize();
                    UserDatabase.Instance.ClearJournals();
                    UserDatabase.Instance.ClearCommanderTable();
                }
            }

        }
    }
}
