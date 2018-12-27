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
            return (s != null && s.Count >= 2 && s.Count <= 3) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "Timer command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = ExtendedControls.PromptMultiLine.ShowDialog(parent, "Configure Timer Dialog", cp.Icon,
                            new string[] { "TimerName", "Milliseconds", "Opt JID" }, l?.ToArray());
            if (r != null)
            {
                userdata = r.ToStringCommaList(2);       // min 2
            }

            return (r != null);
        }

        static List<Timer> timers = new List<Timer>();          // timers are static and shared between all instances..  Programs instances of this class come and go

        class TimerInfo
        {
            public string name;
            public ActionProgramRun ap;
            public HistoryEntry he;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null )
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != BaseUtils.Functions.ExpandResult.Failed)
                {
                    int time;
                    if (exp[1].InvariantParse(out time))
                    {
                        HistoryEntry he = null;
                        long jid;
                        if (exp.Count >= 3)
                        {
                            if (exp[2].InvariantParse(out jid))
                            {
                                he = (ap.actioncontroller as ActionController).HistoryList.GetByJID(jid);

                                if (he == null)
                                {
                                    ap.ReportError("Timer could not find event " + jid);
                                    return true;
                                }
                            }
                            else
                            {
                                ap.ReportError("Timer JID is not an integer ");
                                return true;
                            }
                        }

                        if (exp[0].StartsWith("+"))         // + name means replace if running
                        {
                            exp[0] = exp[0].Substring(1);

                            Timer told = timers.Find(x => ((TimerInfo)x.Tag).name.Equals(exp[0]));
                            //System.Diagnostics.Debug.WriteLine("Timers " + timers.Count);

                            if ( told != null )
                            {
                                System.Diagnostics.Debug.WriteLine("Replace timer " + exp[0]);
                                told.Stop();
                                told.Interval = time;
                                told.Start();
                                return true;
                            }
                        }

                        Timer t = new Timer() { Interval = time };
                        t.Tick += Timer_Tick;
                        t.Tag = new TimerInfo() { ap = ap, name = exp[0] , he = he};
                        timers.Add(t);
                        t.Start();
                        System.Diagnostics.Debug.WriteLine("Timer Go " + exp[0]);
                    }
                    else
                        ap.ReportError("Timer bad name or time count");
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

            (ti.ap.actioncontroller as ActionController).ActionRun(Actions.ActionEventEDList.onTimer, ti.he, new Variables("TimerName", ti.name), now: false);    // queue at end an event

            //System.Diagnostics.Debug.WriteLine("REMOVED Timers " + timers.Count);
            timers.Remove(t);   // done with it
            t.Dispose();
        }
    }
}
