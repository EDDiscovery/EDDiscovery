using DirectInputDevices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public TestDirectInput()
        {
            InitializeComponent();
            idk = DirectInputDevices.InputDeviceKeyboard.CreateKeyboard();
            t.Interval = 250;
            t.Tick += T_Tick;
            t.Start();
        }

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
                        string t = "Event " + ev.EventNumber + " " + ev.EventName() + " " + ev.Pressed + " " + ev.EventNumber + " Keys " + k.VKeyToString();
                        richTextBox1.Text += t + Environment.NewLine;
                        richTextBox1.Select(richTextBox1.Text.Length, richTextBox1.Text.Length);
                    }
                }
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.Text += "vkey " + e.KeyCode + " " + e.KeyCode.VKeyToString() + Environment.NewLine;
            richTextBox1.Select(richTextBox1.Text.Length, richTextBox1.Text.Length);

        }
    }
}
