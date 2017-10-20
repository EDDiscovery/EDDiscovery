/*
 * Copyright © 2016 EDDiscovery development team
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public partial class FatalExceptionForm : Form
    {
        #region Public interfaces

        public static void ShowAndDie(Form parent, string briefDesc, string reportIssueURL, Exception e)
        {
            FatalExceptionForm f = new FatalExceptionForm()
            {
                _Description = briefDesc,
                _Exception = e,
                _ReportURL = reportIssueURL,
                Owner = parent,
                StartPosition = parent != null ? FormStartPosition.CenterParent : FormStartPosition.WindowsDefaultLocation
            };
            f.ShowDialog(parent);
        }

        #endregion

        #region Implementation

        private bool _IsExpanded = false;
        private Image _icon = null;
        private string _Description = string.Empty;
        private Exception _Exception = null;
        private string _ReportURL = null;

        private string DetailBtnText { get { return (_IsExpanded ? "▲" : "▼") + "   &Details     "; } }

        private FatalExceptionForm()
        {
            InitializeComponent();
            pnlDetails.Visible = _IsExpanded;
            Height += (_IsExpanded ? (pnlDetails.Height + 12) : -(pnlDetails.Height + 12));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(1);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _icon?.Dispose();

            _Description = null;
            _Exception = null;
            _icon = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Text = $"Fatal Error While {_Description}";
            lblHeader.Text =
                $"A fatal exception was encountered while {_Description}.{Environment.NewLine}{Environment.NewLine}" +
                $"Click the \"Report An Issue\" button to copy diagnostic information to your clipboard, and provide this info to help us make {Assembly.GetEntryAssembly().GetName().Name} better, or click \"Exit\" to end the program.";
            textboxDetails.Text =
                $"Fatal exception while {_Description}.{Environment.NewLine}{Environment.NewLine}" +
                "==== BEGIN ====" + Environment.NewLine +
                _Exception.ToString() + Environment.NewLine +
                "===== END =====";

            _icon = SystemIcons.Error.ToBitmap();
            pnlIcon.BackgroundImage = _icon;
        }

        private void ToggleDetails()
        {
            _IsExpanded = !_IsExpanded;

            btnDetails.Text = DetailBtnText;
            pnlDetails.Visible = _IsExpanded;
            Height += (_IsExpanded ? (pnlDetails.Height + 12) : -(pnlDetails.Height + 12));
        }


        private void btnDetails_Click(object sender, EventArgs e)
        {
            ToggleDetails();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_ReportURL))
            {
                string mdText = textboxDetails.Text
                    .Replace("==== BEGIN", "```" + Environment.NewLine + "==== BEGIN")
                    .Replace("END =====", "END =====" + Environment.NewLine + "```" + Environment.NewLine);

                try { Clipboard.SetText(mdText); }
                catch {}
                try { Process.Start(_ReportURL); }
                catch {}
            }
            else
            {
                MessageBox.Show(this, "You forget to give me the URL/executable path. Bad developer.");
            }
        }

        #endregion
    }
}
