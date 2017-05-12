using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace EDDiscovery.Joystick
{
    class JoystickIntoActions
    {
        JoystickInterface ji;
        Actions.ActionController ac;
        Timer t;

        public JoystickIntoActions(JoystickInterface pi, Actions.ActionController pc )
        {
            ji = pi;
            ac = pc;
        }

        public void Start()
        {
            ji.Start();
            t = new Timer();
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            List<JoystickEvent> list = ji.Poll();
            foreach (JoystickEvent j in list)
                ac.DiscoveryForm.LogLine(j.ToString() + ": " + j.Device + "." + j.Item);
        }

        public void Stop()
        {
            t.Stop();
        }

        public void Dispose()
        {
            t.Stop();
            ji.Stop();
            ji.Dispose();
        }

    }
}
