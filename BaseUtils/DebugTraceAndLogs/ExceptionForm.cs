/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaseUtils
{
    public partial class ExceptionForm : Form
    {
        #region Public interfaces

        public static DialogResult ShowException(Exception ex, string desc, string reportIssueURL, bool isFatal = false, Form parent = null)
        {
            ExceptionForm f = new ExceptionForm(ex, desc, reportIssueURL, isFatal)
            {
                Owner = parent,
                StartPosition = parent != null ? FormStartPosition.CenterParent : FormStartPosition.WindowsDefaultLocation
            };

            DialogResult res = f.ShowDialog(parent);

            if (isFatal || res != DialogResult.Ignore)
            {
                Environment.Exit(1);
            }

            f.Dispose();
            return res;
        }

        #endregion


        #region Implementation

        private static string DoubleNewLine { get; } = Environment.NewLine + Environment.NewLine;

        private string _Description;
        private Exception _Exception;
        private bool _IsFatal;
        private string _ReportURL;

        private Image _icon = null;
        private bool _IsExpanded = true;
        private int _lastExpandedHeight = -1;

        private readonly int CollapsedHeight = -1;
        private readonly int MinimumHeightExpanded = -1;

        private ExceptionForm(Exception ex, string desc, string reportUrl, bool isFatal = false)
        {
            InitializeComponent();

            _Description = desc;
            _Exception = ex;
            _IsFatal = isFatal;
            _ReportURL = reportUrl;

            // Get some information from the designer.
            CollapsedHeight = Height - pnlDetails.Height - (pnlDetails.Top - pnlHeader.Bottom);
            MinimumHeightExpanded = CollapsedHeight + 80;   // 80 ≈ 4 lines of text.
        }


        // Sanitize keyboard-accelerated Control.Text values. "a&bc" = "abc", "&a &b && &c" = "a b & c", "abc&" = "abc&", etc.
        private string NoAmp(Control c)
        {
            if (!string.IsNullOrEmpty(c?.Text))
                return Regex.Replace(c.Text, "&(.?)", "$1");
            else
                return string.Empty;
        }

        // Show or hide the extended details panel, and resize the form to fit appropriately.
        private void ToggleDetails()
        {
            SuspendLayout();

            _IsExpanded = !_IsExpanded;
            btnDetails.Text = (_IsExpanded ? "▲" : "▼") + "  &Details";
            pnlDetails.Visible = _IsExpanded;

            if (_IsExpanded)
            {
                MinimumSize = new Size(MinimumSize.Width, MinimumHeightExpanded);
                MaximumSize = Size.Empty;
                Height = _lastExpandedHeight;
            }
            else
            {
                _lastExpandedHeight = Height;
                MaximumSize = new Size(int.MaxValue, CollapsedHeight);
                MinimumSize = new Size(MinimumSize.Width, CollapsedHeight);
            }

            ResumeLayout(true);
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            _icon?.Dispose();
            _Description = null;
            _Exception = null;
            _icon = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ToggleDetails();
            _icon = SystemIcons.Error.ToBitmap();
            pnlIcon.BackgroundImage = _icon;

            if (_IsFatal)
            {
                // Remove btnContinue
                pnlHeader.Controls.Remove(btnContinue);
                btnContinue.Dispose();

                // Center btnReport in the panel
                btnReport.Anchor &= ~AnchorStyles.Left;
                btnReport.Left = pnlHeader.ClientSize.Width / 2 - btnReport.Width / 2;
            }

            string appShortName = Assembly.GetEntryAssembly().GetName().Name;
            Text = BrowserInfo.UserAgent + (_IsFatal ? " Fatal" : string.Empty) + " Error";

            lblHeader.Text =
                  _Description + DoubleNewLine
                + (!string.IsNullOrEmpty(_ReportURL) ? $"Click the \"{NoAmp(btnReport)}\" button to copy diagnostic information to your clipboard, and provide this info to help us make {appShortName} better. Otherwise, click " : "Click ")
                + (_IsFatal ? $"{NoAmp(btnExit)} to close the program." : $"{NoAmp(btnContinue)} to try and ignore the error, or {NoAmp(btnExit)} to close the program.");
            textboxDetails.Text =
                  _Description + DoubleNewLine
                + "==== BEGIN ====" + Environment.NewLine
                + _Exception.ToString() + Environment.NewLine
                + "===== END =====";

            if (!string.IsNullOrWhiteSpace(_ReportURL))
            {
                // Tag the report button with markdown-formatted information about this exception.
                btnReport.Tag =
                      "### Description" + Environment.NewLine
                    + "<!-- Please write a brief description about what you were doing when the exception was encountered. -->" + DoubleNewLine
                    + "### Additional Information" + Environment.NewLine
                    + "<!-- Please attach (drag and drop) any relevant trace logs, screenshots, and/or journal files here to provide information about the problem. -->" + DoubleNewLine
                    + "### Exception Details:" + Environment.NewLine
                    + ">" + BrowserInfo.UserAgent + " " + _Description + Environment.NewLine
                    + ">```" + Environment.NewLine
                    + ">==== BEGIN ====" + Environment.NewLine
                    + ">" + _Exception.ToString().Replace(Environment.NewLine, Environment.NewLine + ">") + Environment.NewLine
                    + ">===== END =====" + Environment.NewLine
                    + ">```";
            }
            else
            {
                pnlHeader.Controls.Remove(btnReport);
                btnReport.Dispose();
            }
        }


        private void btnDetails_Click(object sender, EventArgs e)
        {
            ToggleDetails();
        }

        private void btnContinueOrExit_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Close();
                DialogResult = ((Button)sender).DialogResult;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            bool clipSet = false;
            Control ctl = sender as Control;

            if (ctl?.Tag != null)
            {
                try
                {
                    Clipboard.SetText(ctl.Tag as string);
                    clipSet = true;
                }
                catch { }
            }

            try
            {
                Process.Start(_ReportURL);
                if (clipSet)
                    MessageBox.Show(this, "Diagnostic information has been copied to your clipboard. Please include this in your issue submission.", string.Empty, MessageBoxButtons.OK);
            }
            catch { }
        }

        #endregion
    }
}
