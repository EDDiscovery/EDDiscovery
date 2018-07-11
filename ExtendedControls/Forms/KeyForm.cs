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

using BaseUtils.Win32Constants;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public partial class KeyForm : DraggableForm
    {
        public string KeyList { get { return textBoxKeys.Text; } }
        public string ProcessSelected { get { return textBoxSendTo.Text.Equals(DefaultProcessID)? "" :textBoxSendTo.Text; } }    // Default means program default hence "", else target
        public int DefaultDelay { get { int t; return textBoxDelay.Text.InvariantParse(out t) ? t : DefaultDelayID; } }     // -1 means program default, else number

        public const int DefaultDelayID = -1;

        KeyFormMessageFilter keyformmessagefilter;
        string basekeystroke = "";
        
        int curinsertpoint = 0;
        string seperator;
        const string DefaultProcessID = "Default";
        BaseUtils.EnhancedSendKeysParser.IAdditionalKeyParser additionalkeyparser;

        public KeyForm()
        {
            InitializeComponent();
            textBoxSendTo.SetAutoCompletor(AutoList);
            //System.Diagnostics.Debug.WriteLine("HASH " + GetHashCode());
        }


        protected override bool AllowResize { get { return false; } }   // Tell dragger no resizing.

        public void Init(Icon i, bool showprocess, 
                                string separ = " ",   // and separ between entries
                                string keystring = "", // current key string
                                string process = "",  // empty means program default, return empty back
                                int defdelay = 50,     // -1 means program default, return -1 back
                                bool allowkeysedit = false,
                                List<string> additionalkeys = null,
                                BaseUtils.EnhancedSendKeysParser.IAdditionalKeyParser parser = null)
        {
            if ( i != null )
                Icon = i;

            seperator = separ;

            textBoxKeys.Text = keystring;
            curinsertpoint = keystring.Length;

            textBoxSendTo.Text = process.Alt(DefaultProcessID);
            textBoxNextDelay.Text = textBoxDelay.Text = defdelay != DefaultDelayID ? defdelay.ToStringInvariant() : "Default";
            radioButtonPress.Checked = true;

            if ( additionalkeys!=null )
                comboBoxKeySelector.Items.AddRange(additionalkeys);
            comboBoxKeySelector.Items.AddRange(KeyObjectExtensions.KeyListString());

            additionalkeyparser = parser;

            //System.Diagnostics.Debug.WriteLine(String.Join(",",KeyObjectExtensions.VKeyList()));
            comboBoxKeySelector.Text = "";
            comboBoxKeySelector.SelectedIndexChanged += new System.EventHandler(this.comboBoxKeySelector_SelectedIndexChanged);

            if ( !showprocess)
            {
                textBoxNextDelay.Visible = panelRadio.Visible = labelNextDelay.Visible = labelDelay.Visible = textBoxDelay.Visible = labelSendTo.Visible = textBoxSendTo.Visible = buttonTest.Visible = false;

                int d = textBoxKeys.Top - textBoxDelay.Top;
                foreach (Control c in new Control[] { buttonReset, buttonDelete, buttonNext, textBoxKeys, labelKeys })
                    c.Top -= d;     // shift down

                d += 40;        // cause of the Send
                foreach (Control c in new Control[] { buttonOK, buttonCancel})
                    c.Top -= d;
                panelOuter.Height -= d;
                this.Height -= d;
            }

            //System.Diagnostics.Debug.WriteLine("T" + textBoxKeys.Text + " at " + curinsertpoint + " >" + textBoxSendTo.Text);

            bool border = true;
            ITheme theme = ThemeableFormsInstance.Instance;
            if (theme != null)  // paranoid
            {
                border = theme.ApplyToFormStandardFontSize(this);
            }

            labelCaption.Visible = !border;

            AddMF();

            if (allowkeysedit)
                textBoxKeys.ReadOnly = false;

            DisplayKeyString();

            BaseUtils.Translator.Instance.Translate(this);

            labelCaption.Text = this.Text;
        }

        void AddMF()
        {
            if (keyformmessagefilter == null)       // leave ends up double making it
            {
                keyformmessagefilter = new KeyFormMessageFilter(this);
                Application.AddMessageFilter(keyformmessagefilter);
                //System.Diagnostics.Debug.WriteLine("Make " + keyformmessagefilter.GetHashCode() + " for " + GetHashCode());
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine("DOUBLE Make " + keyformmessagefilter.GetHashCode() + " for " + GetHashCode());
            }
        }

        void RemoveMF()
        {
            if (keyformmessagefilter != null)
            {
                //System.Diagnostics.Debug.WriteLine("Remove " + keyformmessagefilter.GetHashCode() + " for " + GetHashCode());
                Application.RemoveMessageFilter(keyformmessagefilter);
                keyformmessagefilter = null;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            RemoveMF();
        }

        public static List<string> AutoList(string input, AutoCompleteTextBox t)
        {
            Process[] pa = Process.GetProcesses();

            List<string> res = (from e in pa where e.ProcessName.StartsWith(input,StringComparison.InvariantCultureIgnoreCase) select e.ProcessName).ToList();

            return res;
        }


        private void SetToggle(CheckBoxCustom c, bool ch , bool right)
        { 
            c.Checked = ch; 
            string last = c.Text.Split(' ').Last();
            if (right)
                c.Text = ((c.Checked) ? "Right" : "") + " " + last;
            else
                c.Text = ((c.Checked) ? "Left" : "") + " " + last;
        }

        public void PressedKey(Keys k, int extsc, Keys modifiers)
        {
            bool extendedkey = (extsc & (1 << 24)) != 0;
            int sc = (extsc >> 16) & 0xff;

            Keys k1 = k.VKeyAdjust(extendedkey, sc);

            if (k1 == Keys.ShiftKey || k1 == Keys.RShiftKey)
            {
                SetToggle(checkBoxShift, !checkBoxShift.Checked, k1 == Keys.RShiftKey);
            }
            else if (k1 == Keys.ControlKey || k1 == Keys.RControlKey)
            {
                SetToggle(checkBoxCtrl, !checkBoxCtrl.Checked, k1 == Keys.RControlKey);
            }
            else if (k1 == Keys.Menu || k1 == Keys.RMenu)
            {
                SetToggle(checkBoxAlt, !checkBoxAlt.Checked, k1 == Keys.RMenu);
            }
            else
            {
                basekeystroke = k1.VKeyToString();
                comboBoxKeySelector.Enabled = false;
                comboBoxKeySelector.SelectedItem = k.VKeyToString();
                comboBoxKeySelector.Enabled = true;
                //string name = k1.VKeyToString(); Keys kback = name.ToVkey(); System.Diagnostics.Debug.WriteLine("Key press " + k + " Full SC " + extsc.ToString("X8") + " SC " + sc.ToString("X2") + " Ext " + extendedkey + " MOD " + modifiers + " ==> " + name + " ==> " + kback.ToString());
            }

            DisplayKeyString();
        }

        void DisplayKeyString()
        { 
            Keys shiftKey = KeyObjectExtensions.ShiftKey(checkBoxShift.Checked, checkBoxShift.Text.Contains("Right"));
            Keys ctrlKey = KeyObjectExtensions.ControlKey(checkBoxCtrl.Checked, checkBoxCtrl.Text.Contains("Right"));
            Keys altKey = KeyObjectExtensions.MenuKey(checkBoxAlt.Checked, checkBoxAlt.Text.Contains("Right"));

            checkBoxKey.Text = basekeystroke.HasChars() ? basekeystroke : "Press Key".Tx(this,"PK");

            //System.Diagnostics.Debug.WriteLine("T" + textBoxKeys.Text + " at " + curinsertpoint + " " + fullname);

            textBoxNextDelay.ReadOnly = (curinsertpoint == 0);

            int nextdelay = textBoxNextDelay.Text.InvariantParseInt(DefaultDelay);      // if its an int, use it, else use default delay

            string res = textBoxKeys.Text.Substring(0, curinsertpoint);

            string partstring = "";

            if (DefaultDelay != nextdelay)              // delay is different to default, so it must be signalled
                partstring += "[" + nextdelay.ToStringInvariant() + "]";

            if (radioButtonDown.Checked)
                partstring += "!";
            else if (radioButtonUp.Checked)
                partstring += "^";

            string shifters = KeyObjectExtensions.ShiftersToString(shiftKey, altKey, ctrlKey);
            string keyname = shifters;
            keyname = keyname.AppendPrePad(basekeystroke, "+");
            partstring += keyname;
            System.Diagnostics.Debug.WriteLine("Stroke " + partstring);

            if (partstring.Length > 0)
                res = res.AppendPrePad(partstring, seperator);

            buttonNext.Enabled = keyname.Length > 0;
            buttonDelete.Enabled = keyname.Length> 0 || curinsertpoint > 0;

            textBoxKeys.Text = res;
            textBoxKeys.Select(res.Length, res.Length);
        }

        void ResetCurrent()
        {
            SetToggle(checkBoxShift, false, false);
            SetToggle(checkBoxCtrl, false, false);
            SetToggle(checkBoxAlt, false, false);
            basekeystroke = "";
            comboBoxKeySelector.Enabled = false;
            comboBoxKeySelector.SelectedIndex = -1;
            comboBoxKeySelector.Enabled = true;
            DisplayKeyString();
        }

        private void checkBoxsac_MouseDown(object sender, MouseEventArgs e)
        {
            CheckBoxCustom c = sender as CheckBoxCustom;
            SetToggle(c, !c.Checked, e.Button == MouseButtons.Right);
            DisplayKeyString();
        }


        private void comboBoxKeySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxKeySelector.Enabled)
            {
                basekeystroke = comboBoxKeySelector.Text;
                DisplayKeyString();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            curinsertpoint = textBoxKeys.Text.Length;
            textBoxNextDelay.Text = textBoxDelay.Text;
            ResetCurrent();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            curinsertpoint = 0;
            textBoxNextDelay.Text = textBoxDelay.Text;
            ResetCurrent();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (basekeystroke.HasChars())
                ResetCurrent();
            else
            {
                int lastcomma = textBoxKeys.Text.Substring(0, curinsertpoint).LastIndexOf(seperator);
                curinsertpoint = Math.Max(lastcomma, 0);
                DisplayKeyString();
            }

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string target = textBoxSendTo.Text;

            if (target.HasChars())
            {
                if (target == BaseUtils.EnhancedSendKeys.CurrentWindow || target.Equals(DefaultProcessID))
                    MessageBoxTheme.Show(this, "Name a process to test sending keys".Tx(this,"NOPN"));
                else
                {
                    string err = BaseUtils.EnhancedSendKeys.SendToProcess(textBoxKeys.Text, DefaultDelay <= DefaultDelayID ? 10 : DefaultDelay, 2 , 2, textBoxSendTo.Text, additionalkeyparser);
                    if (err.Length > 0)
                        MessageBoxTheme.Show(this, string.Format("Error {0} - check entry".Tx(this,"KERR") , err));
                }
            }
            else
                MessageBoxTheme.Show(this, "No process names to send keys to".Tx(this,"NOP"));
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            checkBoxKey.Visible = false;
            RemoveMF();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            checkBoxKey.Visible = true;
            AddMF();
        }

        private void textBoxNextDelay_TextChanged(object sender, EventArgs e)
        {
            DisplayKeyString();
        }

        private void panelOuter_MouseDown(object sender, MouseEventArgs e)
        {
            checkBoxKey.Select();   // better than Focus
            checkBoxKey.Visible = true;
            AddMF();
        }

        private void radioButtonPress_CheckedChanged(object sender, EventArgs e)
        {
            DisplayKeyString();
        }

        private void KeyForm_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender,e);
        }

        private void KeyForm_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected class KeyFormMessageFilter : IMessageFilter
        {
            KeyForm keyform;
            public KeyFormMessageFilter(KeyForm k)
            {
                keyform = k;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM.KEYDOWN || m.Msg == WM.SYSKEYDOWN)
                {
                    Keys k = (Keys)m.WParam;
                    int sc = (int)m.LParam;
                    keyform.PressedKey(k, sc, Control.ModifierKeys);
                    return true;
                }

                return false;
            }
        }

        private void textBoxKeys_Enter(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0 && (Control.ModifierKeys & Keys.Shift) != 0)     // manual mode override.. use with care.g
            {
                textBoxKeys.ReadOnly = false;
                ResetCurrent();
            }

            if ( !textBoxKeys.ReadOnly )
            {
                checkBoxKey.Visible = false;
                RemoveMF();
            }
        }

        private void textBoxKeys_Leave(object sender, EventArgs e)
        {
            if (!textBoxKeys.ReadOnly)
            {
                curinsertpoint = textBoxKeys.Text.Length;
                checkBoxKey.Visible = true;
                AddMF();
            }
        }

    }
}
