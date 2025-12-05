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
using System.Windows.Forms;
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    public class ActionTimer : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && s.Count >= 1) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "Timer command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure Timer Dialog", cp.Icon,
                            new string[] { "TimerName", "Milliseconds", "Opt JID" }, l?.ToArray());
            if (r != null)
            {
                userdata = r.ToStringCommaList(1);       // min 2
            }

            return (r != null);
        }

        static List<Timer> timers = new List<Timer>();          // timers are static and shared between all instances..  Programs instances of this class come and go
        static int timerid = 1;

        class TimerInfo
        {
            public int timerid;                 // id is assigned just to help debug this, increments per timer definition
            public string name;
            public ActionProgramRun ap;
            public HistoryEntry he;
            public Variables vars;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null )
            {
                List<string> exp;

                if (ap.Functions.ExpandStrings(ctrl, out exp) != BaseUtils.Functions.ExpandResult.Failed)
                {
                    if (exp[0].StartsWith("-"))         // - means cancel
                    {
                        exp[0] = exp[0].Substring(1);

                        Timer told = timers.Find(x => ((TimerInfo)x.Tag).name.Equals(exp[0]));

                        if (told != null)
                        {
                            //System.Diagnostics.Debug.WriteLine($"Delete timer {((TimerInfo)told.Tag).timerid} {exp[0]}");
                            told.Stop();
                            told.Dispose();
                            timers.Remove(told);
                        }
                    }
                    else
                    {
                        int time;
                        if (exp[1].InvariantParse(out time))        // if number decodes
                        {
                            Variables timervars = new Variables();

                            HistoryEntry he = null;
                            if (exp.Count >= 3)                     // if jid present
                            {
                                if (exp[2].InvariantParse(out long jid))
                                {
                                    if (jid >= 0)
                                    {
                                        he = (ap.ActionController as ActionController).HistoryList.GetByJID(jid);

                                        if (he == null)
                                        {
                                            ap.ReportError("Timer could not find event " + jid);
                                            return true;
                                        }
                                    }

                                    // pick up any passing variables
                                    int counter = 3;
                                    while( counter<exp.Count)
                                    {
                                        string key = exp[counter];
                                        int asterisk = key.IndexOf('*');
                                        if ( asterisk < 0 )                     // no wildcard
                                        {
                                            if (ap.variables.TryGet(key, out string ex))        // don't fail if not there, its okay
                                                timervars[key] = ex;
                                        }
                                        else
                                        {
                                            string prefix = key.Substring(0, asterisk);
                                            foreach( string vname in ap.variables.NameEnumuerable)  // check all variables against start pattern
                                            {
                                                if (vname.StartsWith(prefix) && ap.variables.TryGet(vname, out string ex))
                                                    timervars[vname] = ex;
                                            }
                                        }

                                        counter++;
                                    }
                                }
                                else
                                {
                                    ap.ReportError("Timer JID is not an integer");
                                    return true;
                                }
                            }

                            if (exp[0].StartsWith("+"))         // + name means replace if running - restart
                            {
                                exp[0] = exp[0].Substring(1);

                                Timer told = timers.Find(x => ((TimerInfo)x.Tag).name.Equals(exp[0]));

                                if (told != null)
                                {
                                    //System.Diagnostics.Debug.WriteLine($"Replace timer {((TimerInfo)told.Tag).timerid} {exp[0]}");
                                    told.Stop();
                                    told.Interval = time;
                                    told.Start();
                                    return true;
                                }
                            }

                            timervars["TimerName"] = exp[0];

                            Timer t = new Timer() { Interval = time };
                            t.Tick += Timer_Tick;
                            t.Tag = new TimerInfo() { timerid = timerid, ap = ap, name = exp[0], he = he, vars = timervars };
                            timers.Add(t);
                            t.Start();
                            
                            //System.Diagnostics.Debug.WriteLine($"Timer {timerid} Go {exp[0]}");
                            
                            timerid++;
                        }
                        else
                            ap.ReportError("Timer bad name or time count");
                    }
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("Timer command line not in correct format");

            return true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer t = sender as Timer;
            t.Stop();

            TimerInfo ti = t.Tag as TimerInfo;

           // System.Diagnostics.Debug.WriteLine($"Timer ticked {ti.timerid} {ti.name} {ti.vars.ToString()}");
            (ti.ap.ActionController as ActionController).ActionRun(Actions.ActionEventEDList.onTimer, ti.he, ti.vars, now: false);    // queue at end an event

            timers.Remove(t);   // done with it
            t.Dispose();
        }
    }
}
