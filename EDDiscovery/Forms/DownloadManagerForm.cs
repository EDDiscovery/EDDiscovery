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

// WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
// Blamming GITHUB gets you banned for a while during debugging. 
// turn this on only for releasing
#define GITHUBDOWNLOAD
// turn this on only for releasing

using EDDiscovery.HTTP;
using EDDiscovery.Win32Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class DownloadManagerForm : Form
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
            public ExtendedControls.ButtonExt actionbutton;
            public ExtendedControls.ButtonExt deletebutton;
            public ExtendedControls.CheckBoxCustom enabled;
        }

        List<Group> groups = new List<Group>();
        VersioningManager mgr;

        int panelheightmargin = 1;
        int labelheightmargin = 6;
        int panelleftmargin = 3;
        Font font;

        string downloadactfolder;
        string downloadflightfolder;
#if DEBUG
        string downloadactdebugfolder;
#endif

        public DownloadManagerForm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            font = new Font(theme.FontName, 10);
            bool winborder = theme.ApplyToForm(this, font);
            statusStripCustom.Visible = panelTop.Visible = panelTop.Enabled = !winborder;
            richTextBoxScrollDescription.TextBox.ReadOnly = true;
        }

        public bool DownloadFromGitHub(string downloadfolder, string gitdir)
        {
            DirectoryInfo di = new DirectoryInfo(downloadfolder);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            GitHubClass ghc = new GitHubClass();

            List<GitHubFile> files = ghc.GetDataFiles(gitdir);

            return ghc.DownloadFiles(files, downloadfolder);
        }

        private System.Threading.Thread CheckThread;

        private void DownloadManager_Shown(object sender, EventArgs e)
        {
            CheckThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckState));
            CheckThread.Start();
        }

        private void CheckState()
        {
            downloadactfolder = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "temp\\act");
            if (!System.IO.Directory.Exists(downloadactfolder))
                System.IO.Directory.CreateDirectory(downloadactfolder);

            downloadflightfolder = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "temp\\flights");
            if (!System.IO.Directory.Exists(downloadflightfolder))
                System.IO.Directory.CreateDirectory(downloadflightfolder);

#if DEBUG
            downloadactdebugfolder = System.IO.Path.Combine(Tools.GetAppDataDirectory(), "temp\\Debug");
            if (!System.IO.Directory.Exists(downloadactdebugfolder))
                System.IO.Directory.CreateDirectory(downloadactdebugfolder);
#endif

#if GITHUBDOWNLOAD

            DownloadFromGitHub(downloadactfolder, "ActionFiles/V1");
            DownloadFromGitHub(downloadflightfolder, "VideoFiles/V1");
#if DEBUG
            DownloadFromGitHub(downloadactdebugfolder, "ActionFiles/Debug");
#endif
#endif

            BeginInvoke((MethodInvoker)ReadyToDisplay);
        }

        private void DownloadManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckThread != null && CheckThread.IsAlive)     // can't close if its alive, it will call back nothing
                CheckThread.Join();

            font?.Dispose();
        }

        void ReadyToDisplay()
        {
            panelVScroll.RemoveAllControls();

            mgr = new VersioningManager();

            int[] edversion = ObjectExtensionsNumbersBool.GetEDVersion();
            System.Diagnostics.Debug.Assert(edversion != null);

            mgr.ReadLocalFiles(Tools.GetAppDataDirectory(), "Actions", "*.act", "Action File");
            mgr.ReadInstallFiles(downloadactfolder, Tools.GetAppDataDirectory(), "*.act", edversion, "Action File");

#if DEBUG
            mgr.ReadInstallFiles(downloadactdebugfolder, Tools.GetAppDataDirectory(), "*.act", edversion, "Action File");
#endif

            mgr.ReadLocalFiles(Tools.GetAppDataDirectory(), "Flights", "*.vid", "Video File");
            mgr.ReadInstallFiles(downloadflightfolder, Tools.GetAppDataDirectory(), "*.vid", edversion, "Video File");

            mgr.Sort();

            panelVScroll.SuspendLayout();

            int[] tabs = { 0, 100, 260, 340, 550, 730, 820, 900};

            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[0] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Type" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[1] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Name" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[2] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Version" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[3] + panelleftmargin, panelheightmargin), Size = new Size(120, 24), Text = "Description" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[4] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Status" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[5] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Action" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[6] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Delete" });
            panelVScroll.Controls.Add(new Label() { Location = new Point(tabs[7] + panelleftmargin, panelheightmargin), Size = new Size(80, 24), Text = "Enabled" });

            int vpos = panelheightmargin + 30;

            foreach ( VersioningManager.DownloadItem di in mgr.downloaditems )
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

                g.info = new ExtendedControls.ButtonExt();
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

                string text;
                if (di.state == VersioningManager.ItemState.EDOutOfDate)
                    text = "Newer EDD required";
                else if (di.state == VersioningManager.ItemState.UpToDate)
                    text = (di.localmodified) ? "Locally modified" : "Up to Date";
                else if (di.state == VersioningManager.ItemState.LocalOnly)
                    text = "Local Only";
                else if ( di.state == VersioningManager.ItemState.NotPresent)
                    text = "Version " + di.downloadedversion.ToString(".") + ((di.localmodified) ? "*" : "");
                else
                    text = "New version " + di.downloadedversion.ToString(".") + ((di.localmodified) ? "*" : "");

                g.actionlabel = new Label();
                g.actionlabel.Location = new Point(tabs[4], labelheightmargin);      // 8 spacing, allow 8*4 to indent
                g.actionlabel.Size = new Size(tabs[5]-tabs[4], 24);
                g.actionlabel.Text = text;
                g.panel.Controls.Add(g.actionlabel);

                if (text.Contains("ersion"))        // cheap and nasty way
                {
                    g.actionbutton = new ExtendedControls.ButtonExt();
                    g.actionbutton.Location = new Point(tabs[5], labelheightmargin - 4);      // 8 spacing, allow 8*4 to indent
                    g.actionbutton.Size = new Size(80, 24);
                    g.actionbutton.Text = (di.state == VersioningManager.ItemState.NotPresent) ? "Install" : "Update";
                    g.actionbutton.Click += Actionbutton_Click;
                    g.actionbutton.Tag = g;
                    g.panel.Controls.Add(g.actionbutton);
                }

                if ( di.HasLocalCopy)
                {
                    g.deletebutton = new ExtendedControls.ButtonExt();
                    g.deletebutton.Location = new Point(tabs[6], labelheightmargin - 4);      // 8 spacing, allow 8*4 to indent
                    g.deletebutton.Size = new Size(24, 24);
                    g.deletebutton.Text = "X";
                    g.deletebutton.Click += Deletebutton_Click;
                    g.deletebutton.Tag = g;
                    g.panel.Controls.Add(g.deletebutton);
                }

                if ( di.localenable.HasValue )
                {
                    g.enabled = new ExtendedControls.CheckBoxCustom();
                    g.enabled.Location = new Point(tabs[7], labelheightmargin);
                    g.enabled.Size = new Size(100, 20);
                    g.enabled.Text = "Enabled";
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

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            if ( theme != null )
                theme.ApplyToControls(panelVScroll, font);

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
            ExtendedControls.CheckBoxCustom cb = sender as ExtendedControls.CheckBoxCustom;
            Group g = cb.Tag as Group;
            VersioningManager.SetEnableFlag(g.di, cb.Checked, Tools.GetAppDataDirectory());
            changelist[g.di.itemname] = cb.Checked ? "+" : "-";
        }

        private void Actionbutton_Click(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt cb = sender as ExtendedControls.ButtonExt;
            Group g = cb.Tag as Group;

            if (g.di.localmodified)
            {
                if (Forms.MessageBoxTheme.Show(this, "Modified locally, do you wish to overwrite the changes", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
            }

            if (mgr.InstallFiles(g.di, Tools.GetAppDataDirectory()))
            {
                changelist[g.di.itemname] = "+";
                Forms.MessageBoxTheme.Show(this, "Add-on updated");
                ReadyToDisplay();
            }
            else
                Forms.MessageBoxTheme.Show(this, "Add-on failed to update. Check files for read only status", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            ExtendedControls.ButtonExt cb = sender as ExtendedControls.ButtonExt;
            Group g = cb.Tag as Group;

            if (Forms.MessageBoxTheme.Show(this, "Do you really want to delete " + g.di.itemname, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                VersioningManager.DeleteInstall(g.di, Tools.GetAppDataDirectory());
                ReadyToDisplay();
                changelist[g.di.itemname] = "-";
            }
        }
                

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

#region Window Control

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        protected override void WndProc(ref Message m)
        {
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;
            // Compatibility movement for Mono
            if (m.Msg == WM.LBUTTONDOWN && m.WParam == (IntPtr)1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && m.WParam == (IntPtr)1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM.LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM.NCHITTEST)
            {
                base.WndProc(ref m);

                if (m.Result == (IntPtr)HT.CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStripCustom.Height && p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStripCustom.Height)
                    {
                        m.Result = (IntPtr)HT.BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT.RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT.LEFT;
                    }
                    else if (!windowsborder)
                    {
                        m.Result = (IntPtr)HT.CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

    }
}
