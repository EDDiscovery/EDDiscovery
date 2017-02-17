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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionTimer : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        List<string> FromString(string input)
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList();
            return (s != null && s.Count >= 2) ? s : null;
        }

        string ToString(List<string> list)
        {
            string r = "";
            for( int i = 0; i < list.Count; i++)
            {
                if (i == 2 && list[i].Length == 0)
                    break;

                if (i > 0)
                    r += ",";

                r += list[i].QuoteString(comma: true);
            }

            return r;
        }

        public override string VerifyActionCorrect()
        {
            List<string> l = FromString(userdata);
            return (l != null) ? null : "Timer command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            List<string> l = FromString(userdata);
            List<string> r = Forms.PromptMultiLine.ShowDialog(parent, discoveryform.theme, "Configure Timer Dialog",
                            new string[] { "TimerName", "Milliseconds", "Opt JID" }, l?.ToArray());
            if (r != null)
            {
                userdata = ToString(r);
            }

            return (r != null);
        }

        List<Timer> timers = new List<Timer>();
        class TimerInfo
        {
            public string name;
            public ActionProgramRun ap;
            public HistoryEntry he;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null && ctrl.Count >= 2)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp, ap.currentvars) != ConditionLists.ExpandResult.Failed)
                {
                    int time;
                    if (exp[1].InvariantParse(out time))
                    {
                        HistoryEntry he = null;
                        long jid;
                        if (exp.Count >= 3 && exp[2].InvariantParse(out jid))
                        {
                            he = ap.actioncontroller.HistoryList.GetByJID(jid);

                            if (he == null)
                            {
                                ap.ReportError("Timer could not find event " + jid);
                                return true;
                            }
                        }

                        Timer t = new Timer() { Interval = time };
                        t.Tick += Timer_Tick;
                        t.Tag = new TimerInfo() { ap = ap, name = exp[0] , he = he};
                        timers.Add(t);
                        t.Start();
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

            ti.ap.actioncontroller.ActionRun("onTimer", "ActionProgram", ti.he, new ConditionVariables("TimerName", ti.name), now: false);    // queue at end an event

            timers.Remove(t);   // done with it
        }
    }
}
