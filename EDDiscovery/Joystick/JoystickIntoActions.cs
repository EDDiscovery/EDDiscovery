using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;


namespace EDDiscovery.Joystick
{
    class JoystickIntoActions
    {
        JoystickInterface ji;
        Actions.ActionController ac;
        BindingsFile bf;
        Timer t;
        List<JoystickIdentity> joylist;

        Dictionary<string, string> devicetable = new Dictionary<string, string>();      // 

        public JoystickIntoActions(JoystickInterface pi, BindingsFile b , Actions.ActionController pc )
        {
            ji = pi;
            bf = b;
            ac = pc;
        }

        public void Start()
        {
            ji.Start();
            joylist = ji.List();
            t = new Timer();
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            List<JoystickEvent> list = ji.Poll();
            foreach (JoystickEvent je in list)
            {
                ac.DiscoveryForm.LogLine(je.ToString() + ": " + je.Device.Name + "." + je.Item);

                System.Diagnostics.Debug.WriteLine("Event " + je.ToString());

                string nametolookup = je.Device.Name;
                if (devicetable.ContainsKey(je.Device.Name))
                    nametolookup = devicetable[je.Device.Name];

                BindingsFile.Device dv = bf.FindDevice(nametolookup, je.Device.Instanceguid, je.Device.Productguid);

                if ( dv != null )
                {
                    List<string> assignments = null;

                    bool released = false;
                    if (je.IsButtonEvent)
                    {
                        assignments = Verify(dv.Find(je.Item), je , je.Pressed, je.Pressed ? "" : "_Up");

                    }

                    not right, need to someone know that joy4 is up.

                    //UserControl je.Pressed to determine state
                    else if (je.IsPOVEvent)
                    {
                        assignments = new List<string>();

                        if (je.isPOVLeft)       // Frontier do not seem to guarantee the order between lrup and its modifier.. need to test all combos
                            assignments.AddRange(Verify(dv.Find(je.Item + "Left"), je , true));
                        if (je.isPOVRight)
                            assignments.AddRange(Verify(dv.Find(je.Item + "Right"), je , true));
                        if (je.isPOVUp)
                            assignments.AddRange(Verify(dv.Find(je.Item + "Up"), je , true));
                        if (je.isPOVDown)
                            assignments.AddRange(Verify(dv.Find(je.Item + "Down"), je , true));

                        string povkey = nametolookup + "P" + je.Item;       // see if we have a recorded pos
                        int lastpos = -1;
                        if (povpos.ContainsKey(povkey))
                            lastpos = povpos[povkey];

                        povpos[povkey] = je.Value;      // record one

                        if ( lastpos != je.Value)       // if its changed
                        {
                            List<string> oldpe = new List<string>();
                            foreach (string s in poveventsineffect)     // ones in effect are turning off..
                                oldpe.Add(s + "_Up");       

                            poveventsineffect = new List<string>(assignments);  // copy array, needs a clean new one
                            assignments.AddRange(oldpe);        // and add on previous assignments but as _Up
                        }
                    }

                    if (assignments != null )
                    {
                        foreach (string a in assignments)
                        {
                            System.Diagnostics.Debug.WriteLine("  Event " +  a.ToString());
                        }
                    }
                }
            }
        }

        Dictionary<string, int> povpos = new Dictionary<string, int>();
        static List<string> poveventsineffect = new List<string>();
        

        public List<string> Verify(List<BindingsFile.Assignment> list, JoystickEvent je , bool substate , string eventpostfix = "")
        {
            List<string> ret = new List<string>();

            if (list != null)
            {
                foreach (BindingsFile.Assignment a in list)
                {
                    bool hit = true;
                    bool povmod = false;
                    if (a.modifiersrequired != null && substate == true )   
                    {
                        foreach (BindingsFile.DeviceKeyPair dkp in a.modifiersrequired)
                        {
                            bool thisok = false;

                            JoystickIdentity matchingdevice = GetID(dkp.device);

                            if (matchingdevice != null)
                            {
                                int n;

                                if (dkp.key.Truncate(0, 7).Equals("Joy_POV"))  // pov works
                                {
                                    if (dkp.key.Length >= 7 + 1 + 2)
                                    {
                                        int num = dkp.key[7] - '0';
                                        string dir = dkp.key.Substring(8);

                                        povmod = true;
                                        thisok = ji.IsPOVPressed(matchingdevice, num, dir);
                                    }
                                }
                                else if (dkp.key.Truncate(0, 4).Equals("Joy_") && dkp.key.Substring(4).InvariantParse(out n)) // button
                                {
                                    thisok = ji.IsButtonPressed(matchingdevice, n);
                                }
                            }

                            if (!thisok)
                            {
                                hit = false;
                                break;
                            }
                        }
                    }

                    if (je.IsPOVEvent && !je.IsPOVDirectionStraight && povmod == false)
                        hit = false;     // entry with just up/left/down/right, its not straight.. no mods to override

                    if (hit)
                    {
                        ret.Add(a.assignedfunc + eventpostfix);
                    }
                }
            }

            return ret;
        }


        JoystickIdentity GetID(string fdname)       // given the fdname, which is out identity..
        {
            foreach( JoystickIdentity j in joylist )
            {
                BindingsFile.Device dv = bf.FindDevice(j.Name, j.Instanceguid, j.Productguid);
                if (dv != null && dv.name.Equals(fdname))
                    return j;
            }

            return null;
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
