using DirectInputDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    public partial class TestDirectInput : Form
    {
        DirectInputDevices.InputDeviceKeyboard idk;
        Timer t = new Timer();
        Timer t2 = new Timer();
        Timer t3 = new Timer();

        public TestDirectInput()
        {
            InitializeComponent();
            idk = DirectInputDevices.InputDeviceKeyboard.CreateKeyboard();
            t.Interval = 250;
            t.Tick += T_Tick;
            t.Start();

            t2.Interval = 2000;
            t2.Tick += T2_Tick;

            t3.Interval = 10000;
            t3.Tick += T3_Tick;
            // t3.Start(); // t3 is autostart test to check still sends when don't have focus
        }


        Keys vkey;

        private void T_Tick(object sender, EventArgs e)
        {
            List<InputDeviceEvent> events = idk.GetEvents();
            if (events != null)
            {
                foreach (InputDeviceEvent ev in events)
                {
                    if (ev.Pressed)
                    {
                        Keys k = InputDeviceKeyboard.ToKeys(ev);
                        string t = "Sharp Name: " + InputDeviceKeyboard.SharpKeyName(ev) + Environment.NewLine;
                        t += "Frontier Name " + ev.EventName() + Environment.NewLine;

                        System.Diagnostics.Debug.WriteLine("Direct Input detected " + t);

                        if (!InputDeviceKeyboard.CheckTranslation(ev, vkey))
                            t += Environment.NewLine + " ERROR TX WRONG!";

                        richTextBox1.Text += t + Environment.NewLine;
                        richTextBox1.Select(richTextBox1.Text.Length, richTextBox1.Text.Length);

                    }
                }
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            vkey = e.KeyCode;
            richTextBox1.Text += "VK:" + e.KeyCode.VKeyToString() + " " + e.KeyCode + " " + Environment.NewLine;
            richTextBox1.Select(richTextBox1.Text.Length, richTextBox1.Text.Length);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            return false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            t2.Start();
        }

        private void T3_Tick(object sender, EventArgs e)
        {
            t3.Stop();
            t2.Start();
        }

        private void T2_Tick(object sender, EventArgs e)
        {
            //t2.Stop();    // if you want it one time

            string ptarget = "Keylogger";
            ptarget = "elitedangerous64";

            Keys vkey = Keys.G;
            System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " t2 tick send G");
            string res = BaseUtils.EnhancedSendKeys.SendToProcess(vkey.VKeyToString(), 50, 50, 50, ptarget);
            System.Diagnostics.Debug.WriteLine("Send result <" + res + ">");
        }
    }


}
