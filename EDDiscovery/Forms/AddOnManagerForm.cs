/*
 * Copyright © 2017 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EDDiscovery.Versions;

namespace EDDiscovery.Forms
{
    public partial class AddOnManagerForm : ExtendedControls.DraggableForm
    {
        public Dictionary<string,string> changelist = new Dictionary<string,string>();      //+ enabled/installed, - deleted/disabled

        class Group
        {
            public VersioningManager.DownloadItem di;
            public Panel panel;
            public Button info;
            public Label type;
            public Label name;
            public Label version;
            public Label shortdesc;
            public Label actionlabel;
            public ExtendedControls.ExtButton actionbutton;
            public ExtendedControls.ExtButton deletebutton;
            public ExtendedControls.ExtCheckBox enabled;
        }

        List<Group> groups = new List<Group>();
        VersioningManager mgr;

        int panelheightmargin = 1;
        int labelheightmargin = 6;
        int panelleftmargin = 3;

        public Action<string> EditActionFile;
        public Action EditGlobals;
        public Action CreateActionFile;
        public delegate bool IsActionLoaded(string name);
        public event IsActionLoaded CheckActionLoaded;

        bool managedownloadmode;

        string downloadactfolder;
        string downloadflightfolder;
        string downloadaddonfolder;
#if DEBUG
        string downloadactdebugfolder;
#endif

        public AddOnManagerForm()
        {
            InitializeComponent();
        }

        public void Init(bool ad, Icon ic)           // true = manage downloads, else just show actions and allow editing of them
        {
            this.Icon = ic;
            managedownloadmode = ad;
            
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToFormStandardFontSize(this);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            richTextBoxScrollDescription.ReadOnly = true;

            buttonExtGlobals.Visible = !managedownloadmode;
            buttonMore.Visible = !managedownloadmode;

            BaseUtils.Translator.Instance.Translate(this);

            label_index.Text = this.Text = (managedownloadmode) ? "Add-On Manager".Tx(this, "AddOnTitle") : "Edit Add-Ons".Tx(this, "EditTitle");
        }

        private System.Threading.Thread CheckThread;

        private void DownloadManager_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            CheckThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckState));
            CheckThread.Start();
        }

        private void CheckState()
        {
            downloadactfolder = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "temp", "act");
            if (!System.IO.Directory.Exists(downloadactfolder))
                System.IO.Directory.CreateDirectory(downloadactfolder);

            downloadflightfolder = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "temp", "flights");
            if (!System.IO.Directory.Exists(downloadflightfolder))
                System.IO.Directory.CreateDirectory(downloadflightfolder);

            downloadaddonfolder = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "temp", "addonfiles");
            if (!System.IO.Directory.Exists(downloadaddonfolder))
                System.IO.Directory.CreateDirectory(downloadaddonfolder);

#if DEBUG
            downloadactdebugfolder = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "temp", "Debug");
            if (!System.IO.Directory.Exists(downloadactdebugfolder))
                System.IO.Directory.CreateDirectory(downloadactdebugfolder);
#endif

            if (managedownloadmode && EDDOptions.Instance.DontAskGithubForPacks == false )
            {
                BaseUtils.GitHubClass ghc = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);
                System.Diagnostics.Debug.WriteLine("Checking github");
                 ghc.Download(downloadactfolder, "ActionFiles/V1", "*.act");
                 ghc.Download(downloadflightfolder, "VideoFiles/V1", "*.vid");
                 ghc.Download(downloadaddonfolder, "AddonFiles/V1", "*.inf");
#if DEBUG
                 ghc.Download(downloadactdebugfolder, "ActionFiles/Debug", "*.act");
#endif
            }

            BeginInvoke((MethodInvoker)ReadyToDisplay);
        }

        private void DownloadManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckThread != null && CheckThread.IsAlive)     // can't close if its alive, it will call back nothing
                CheckThread.Join();
        }

        static public void ReadLocalFiles(VersioningManager mgr, bool othertypes)
        {
            mgr.ReadLocalFiles(EDDOptions.Instance.AppDataDirectory, "Actions", "*.act", "Action File");

            if (othertypes)
            {
                mgr.ReadLocalFiles(EDDOptions.Instance.AppDataDirectory, "Flights", "*.vid", "Video File");
                mgr.ReadLocalFiles(EDDOptions.Instance.AppDataDirectory, "AddonFiles", "*.inf", "Other Files");
            }
        }


        void ReadyToDisplay()
        {
            this.Cursor = Cursors.Default;
            panelVScroll.RemoveAllControls(new List<Control>() { buttonMore});

            mgr = new VersioningManager();

            int[] edversion = System.Reflection.Assembly.GetExecutingAssembly().GetVersionInts();
            System.Diagnostics.Debug.Assert(edversion != null);

            ReadLocalFiles(mgr, managedownloadmode);

            if (managedownloadmode)
            {
                mgr.ReadInstallFiles(EDDiscovery.Properties.Resources.URLGithubDataDownload, "ActionFiles/V1", downloadactfolder, EDDOptions.Instance.AppDataDirectory, "*.act", edversion, "Action File");
                mgr.ReadInstallFiles(EDDiscovery.Properties.Resources.URLGithubDataDownload, "VideoFiles/V1", downloadflightfolder, EDDOptions.Instance.AppDataDirectory, "*.vid", edversion, "Video File");
                mgr.ReadInstallFiles(EDDiscovery.Properties.Resources.URLGithubDataDownload, "AddonFiles/V1", downloadaddonfolder, EDDOptions.Instance.AppDataDirectory, "*.inf", edversion, "Other File");
#if DEBUG
                mgr.ReadInstallFiles(EDDiscovery.Properties.Resources.URLGithubDataDownload, "ActionFiles/Debug", downloadactdebugfolder, EDDOptions.Instance.AppDataDirectory, "*.act", edversion, "Action File");
#endif
            }

            mgr.Sort();

            panelVScroll.SuspendLayout();

            int[] tabs;
            if ( managedownloadmode )
                tabs = new int[] { 0, 100, 300, 400, 600, 780, 930, 1030 };
            else
                tabs = new int[] { 0, 100, 300, 400, 600, 600, 780, 930 };

            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[0] + panelleftmargin, panelheightmargin), Size = new Size(tabs[1] - tabs[0] - 2, 24), Text = "Type".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[1] + panelleftmargin, panelheightmargin), Size = new Size(tabs[2] - tabs[1] - 2, 24), Text = "Name".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[2] + panelleftmargin, panelheightmargin), Size = new Size(tabs[3] - tabs[2] - 2, 24), Text = "Version".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[3] + panelleftmargin, panelheightmargin), Size = new Size(tabs[4] - tabs[3] - 2, 24), Text = "Description".Tx(this) });
            if ( managedownloadmode)
                panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[4] + panelleftmargin, panelheightmargin), Size = new Size(tabs[5] - tabs[4] - 2, 24), Text = "Status".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[5] + panelleftmargin, panelheightmargin), Size = new Size(tabs[6] - tabs[5]-2, 24), Text = "Action".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[6] + panelleftmargin, panelheightmargin), Size = new Size(tabs[7] - tabs[6]-2, 24), Text = "Delete".Tx(this) });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[7] + panelleftmargin, panelheightmargin), Size = new Size(120, 24), Text = "Enabled".Tx(this) });

            int vpos = panelheightmargin + 30;

            foreach ( VersioningManager.DownloadItem di in mgr.DownloadItems )
            {
                Group g = new Group();
                g.di = di;
                g.panel = new Panel();
                g.panel.BorderStyle = BorderStyle.FixedSingle;
                g.panel.Tag = g;
                g.panel.MouseEnter += MouseEnterControl;

                g.type = new Label();
                g.type.Location = new Point(tabs[0], labelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.type.Size = new Size(tabs[1] - tabs[0], 24);
                g.type.Text = di.itemtype;
                g.panel.Controls.Add(g.type);

                g.info = new ExtendedControls.ExtButton();
                g.info.Location = new Point(tabs[1], labelheightmargin + 2);      // 8 spacing, allow 8*4 to indent
                g.info.Size = new Size(16,16);
                g.info.Text = "i";
                g.info.Click += Info_Click;
                g.info.Tag = g;
                g.panel.Controls.Add(g.info);

                g.name = new Label();
                g.name.Location = new Point(tabs[1]+18, labelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.name.Size = new Size(tabs[2] - tabs[1] - 18, 24);
                g.name.Text = di.itemname;
                g.panel.Controls.Add(g.name);

                g.version = new Label();
                g.version.Location = new Point(tabs[2], labelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.version.Size = new Size(tabs[3] - tabs[2], 24);
                g.version.Text = (di.localversion != null) ? di.localversion.ToString(".") : "N/A";
                g.panel.Controls.Add(g.version);

                g.shortdesc = new Label();
                g.shortdesc.Location = new Point(tabs[3], labelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.shortdesc.Size = new Size(tabs[4] - tabs[3], 24);
                g.shortdesc.Text = di.ShortLocalDescription;
                if (g.shortdesc.Text.Length == 0)
                    g.shortdesc.Text = "N/A";
                g.panel.Controls.Add(g.shortdesc);

                if (managedownloadmode)
                {
                    bool isversion = false;
                    string text;
                    if (di.state == VersioningManager.ItemState.EDOutOfDate)
                        text = "Newer EDD required".Tx(this,"Newer");
                    else if (di.state == VersioningManager.ItemState.EDTooOld)
                        text = "Too old for EDD".Tx(this, "Old");
                    else if (di.state == VersioningManager.ItemState.UpToDate)
                        text = (di.localmodified) ? "Locally modified".Tx(this) : "Up to Date".Tx(this);
                    else if (di.state == VersioningManager.ItemState.LocalOnly)
                        text = "Local Only".Tx(this, "LocalOnly");
                    else if (di.state == VersioningManager.ItemState.NotPresent)
                    {
                        isversion = true;
                        text = "Version".Tx(this) + " " + di.downloadedversion.ToString(".") + ((di.localmodified) ? "*" : "");
                    }
                    else
                    {
                        isversion = true;
                        text = "New version".Tx(this) + " " + di.downloadedversion.ToString(".") + ((di.localmodified) ? "*" : "");
                    }

                    g.actionlabel = new Label();
                    g.actionlabel.Location = new Point(tabs[4], labelheightmargin);      // 8 spacing, allow 8*4 to indent
                    g.actionlabel.Size = new Size(tabs[5] - tabs[4], 24);
                    g.actionlabel.Text = text;
                    g.panel.Controls.Add(g.actionlabel);

                    if (isversion)        
                    {
                        g.actionbutton = new ExtendedControls.ExtButton();
                        g.actionbutton.Location = new Point(tabs[5], labelheightmargin - 4);      // 8 spacing, allow 8*4 to indent
                        g.actionbutton.Size = new Size(tabs[6] - tabs[5] - 20, 24);
                        g.actionbutton.Text = (di.state == VersioningManager.ItemState.NotPresent) ? "Install".Tx(this) : "Update".Tx(this);
                        g.actionbutton.Click += Actionbutton_Click;
                        g.actionbutton.Tag = g;
                        g.panel.Controls.Add(g.actionbutton);
                    }
                }
                else
                {
                    bool loaded = CheckActionLoaded != null ? CheckActionLoaded(g.di.itemname) : false;

                    if (loaded)     // may not be loaded IF its got an error.
                    {
                        g.actionbutton = new ExtendedControls.ExtButton();
                        g.actionbutton.Location = new Point(tabs[5], labelheightmargin - 4);      // 8 spacing, allow 8*4 to indent
                        g.actionbutton.Size = new Size(tabs[6] - tabs[5] - 20, 24);
                        g.actionbutton.Text = "Edit".Tx(this);
                        g.actionbutton.Click += ActionbuttonEdit_Click;
                        g.actionbutton.Tag = g;
                        g.panel.Controls.Add(g.actionbutton);
                    }
                }

                if ( di.HasLocalCopy)
                {
                    g.deletebutton = new ExtendedControls.ExtButton();
                    g.deletebutton.Location = new Point(tabs[6]+20, labelheightmargin - 4);      // 8 spacing, allow 8*4 to indent
                    g.deletebutton.Size = new Size(24, 24);
                    g.deletebutton.Text = "X";
                    g.deletebutton.Click += Deletebutton_Click;
                    g.deletebutton.Tag = g;
                    g.panel.Controls.Add(g.deletebutton);
                }

                if ( di.localenable.HasValue )
                {
                    g.enabled = new ExtendedControls.ExtCheckBox();
                    g.enabled.Location = new Point(tabs[7], labelheightmargin);
                    g.enabled.Size = new Size(100, 20);
                    g.enabled.Text = "Enabled".Tx(this);
                    g.enabled.Checked = di.localenable.Value;
                    g.enabled.Click += Enabled_Click;
                    g.enabled.Tag = g;
                    g.panel.Controls.Add(g.enabled);
                }

                int panelwidth = Math.Max(panelVScroll.Width - panelVScroll.ScrollBarWidth, 10) - panelleftmargin*2;

                g.panel.Location= new Point(panelleftmargin, vpos);
                g.panel.Size = new Size(panelwidth, 32);

                vpos += g.panel.Height + 4;

                panelVScroll.Controls.Add(g.panel);
            }

            buttonMore.Location = new Point(panelleftmargin, vpos);

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            if ( theme != null )
                theme.ApplyToControls(panelVScroll, label_index.Font);

            panelVScroll.ResumeLayout();
        }

        bool infoclicked = false;
        private void Info_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            Group g = c.Tag as Group;

            infoclicked = true;
            string d = g.di.LongDownloadedDescription;
            if (d == "")
                d = g.di.LongLocalDescription;

            richTextBoxScrollDescription.Text = d.ReplaceEscapeControlChars();
        }

        private void MouseEnterControl(object sender, EventArgs e)
        {
            Control c = sender as Control;
            Group g = c.Tag as Group;

            if (!infoclicked)
            {
                string d = g.di.LongDownloadedDescription;
                if (d == "")
                    d = g.di.LongLocalDescription;

                richTextBoxScrollDescription.Text = d.ReplaceEscapeControlChars();
            }
        }

        private void Enabled_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtCheckBox cb = sender as ExtendedControls.ExtCheckBox;
            Group g = cb.Tag as Group;
            VersioningManager.SetEnableFlag(g.di, cb.Checked, EDDOptions.Instance.AppDataDirectory);
            changelist[g.di.itemname] = cb.Checked ? "+" : "-";
        }

        private void Actionbutton_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton cb = sender as ExtendedControls.ExtButton;
            Group g = cb.Tag as Group;

            if (g.di.localmodified)
            {
                if (ExtendedControls.MessageBoxTheme.Show(this, "Modified locally, do you wish to overwrite the changes".Tx(this, "Modwarn"), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
            }

            if (mgr.InstallFiles(g.di, EDDOptions.Instance.AppDataDirectory))
            {
                changelist[g.di.itemname] = "+";
                ExtendedControls.MessageBoxTheme.Show(this, "Add-on updated");
                ReadyToDisplay();
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "Add-on failed to update. Check files for read only status".Tx(this, "Failed"), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ActionbuttonEdit_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton cb = sender as ExtendedControls.ExtButton;
            Group g = cb.Tag as Group;
            EditActionFile?.Invoke(g.name.Text);
            ReadyToDisplay();
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton cb = sender as ExtendedControls.ExtButton;
            Group g = cb.Tag as Group;

            if (ExtendedControls.MessageBoxTheme.Show(this, string.Format("Do you really want to delete {0}".Tx(this,"DeleteWarn"), g.di.itemname), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                VersioningManager.DeleteInstall(g.di, EDDOptions.Instance.AppDataDirectory);
                ReadyToDisplay();
                changelist[g.di.itemname] = "-";
            }
        }

        private void buttonMore_Click(object sender, EventArgs e)
        {
            CreateActionFile?.Invoke();
            ReadyToDisplay();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonExtGlobals_Click(object sender, EventArgs e)
        {
            EditGlobals?.Invoke();
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
