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
    public partial class KeyForm : Form
    {
        public string KeyList { get { return textBoxKeys.Text; } }
        public string ProcessSelected { get { return textBoxSendTo.Text; } }

        ActionMessageFilter actionfilesmessagefilter;
        Keys basekey = Keys.None;
        
        int curinsertpoint = 0;

        public KeyForm()
        {
            InitializeComponent();
            actionfilesmessagefilter = new ActionMessageFilter(this);
            Application.AddMessageFilter(actionfilesmessagefilter);
            //  textBoxSendTo.Text = "Current window";
            //textBoxSendTo.Text = "KeyLogger";
            textBoxSendTo.SetAutoCompletor(AutoList);
        }

        public void Init(Icon i)
        {
            Icon = i;
        }

        public static List<string> AutoList(string input, AutoCompleteTextBox t)
        {
            Process[] pa = Process.GetProcesses();

            List<string> res = (from e in pa where e.ProcessName.StartsWith(input,StringComparison.InvariantCultureIgnoreCase) select e.ProcessName).ToList();

            return res;
        }


        private void checkBoxsac_MouseDown(object sender, MouseEventArgs e)
        {
            CheckBoxCustom c = sender as CheckBoxCustom;
            SetToggle(c, !c.Checked, e.Button == MouseButtons.Right);
            SetKeyString();
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
            System.Diagnostics.Debug.WriteLine("Key press " + k + " => " + k1.ToString());

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
                basekey = k1;

                //string name = k1.VKeyToString(); Keys kback = name.ToVkey(); System.Diagnostics.Debug.WriteLine("Key press " + k + " Full SC " + extsc.ToString("X8") + " SC " + sc.ToString("X2") + " Ext " + extendedkey + " MOD " + modifiers + " ==> " + name + " ==> " + kback.ToString());
            }

            SetKeyString();
        }

        void SetKeyString()
        { 
            Keys shiftKey = KeyObjectExtensions.ShiftKey(checkBoxShift.Checked, checkBoxShift.Text.Contains("Right"));
            Keys ctrlKey = KeyObjectExtensions.ControlKey(checkBoxCtrl.Checked, checkBoxCtrl.Text.Contains("Right"));
            Keys altKey = KeyObjectExtensions.MenuKey(checkBoxAlt.Checked, checkBoxAlt.Text.Contains("Right"));

            checkBoxKey.Text = (basekey != Keys.None) ? basekey.VKeyToString() : "Press Key";
            string fullname = basekey.VKeyToString(shiftKey, altKey, ctrlKey);

            textBoxKeys.Text = textBoxKeys.Text.Substring(0, curinsertpoint) + fullname;
        }

        void ResetCurrent()
        {
            SetToggle(checkBoxShift, false, false);
            SetToggle(checkBoxCtrl, false, false);
            SetToggle(checkBoxAlt, false, false);
            basekey = Keys.None;
            SetKeyString();
        }

        // maybe a full hook? https://github.com/shanselman/babysmash/blob/master/App.xaml.cs

        protected class ActionMessageFilter : IMessageFilter
        {
            KeyForm keyform;
            public ActionMessageFilter(KeyForm k)
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

        private void buttonNext_Click(object sender, EventArgs e)
        {
            textBoxKeys.Text += " ";
            curinsertpoint = textBoxKeys.Text.Length;
            ResetCurrent();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            curinsertpoint = 0;
            ResetCurrent();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string target = textBoxSendTo.Text;
            BaseUtils.SendExtendedKeys.Send(textBoxKeys.Text, target);
        }

        private void textBoxSendTo_Enter(object sender, EventArgs e)
        {
            Application.RemoveMessageFilter(actionfilesmessagefilter);
        }

        private void textBoxSendTo_Leave(object sender, EventArgs e)
        {
            Application.AddMessageFilter(actionfilesmessagefilter);
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
    }
}
