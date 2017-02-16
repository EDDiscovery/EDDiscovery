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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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

        bool FromString(string input, out string timername, out string time)
        {
            StringParser sp = new StringParser(input);
            timername = sp.NextQuotedWord(", ");
            time = null;

            if (timername != null)
            {
                timername.ReplaceEscapeControlChars();
                sp.IsCharMoveOn(',');
                time = sp.NextQuotedWord(", ");

                if (time != null)
                    return true;
            }

            timername = time = "";
            return false;
        }

        string ToString(string timername, string time)
        {
            return timername.QuoteString(comma: true) + "," + time.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string name,time;
            return FromString(userdata, out name, out time) ? null : "Timer not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string name, time;
            FromString(UserData, out name, out time);
            Tuple<string, string> promptValue = PromptDoubleLine.ShowDialog(parent, discoveryform.theme, "Timer Name", "Time", name, time, "Configure Timer Dialog");
            if (promptValue != null)
            {
                userdata = ToString(promptValue.Item1.EscapeControlChars(), promptValue.Item2);
            }

            return (promptValue != null);
        }

        List<Timer> timers = new List<Timer>();
        class TimerInfo
        {
            public string name;
            public ActionProgramRun ap;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string timername, timervalue;
            if (FromString(UserData, out timername, out timervalue))
            {
                string timernameres, timervaluesres;
                if (ap.functions.ExpandString(timername, ap.currentvars, out timernameres) != ConditionLists.ExpandResult.Failed)
                {
                    if (ap.functions.ExpandString(timervalue, ap.currentvars, out timervaluesres) != ConditionLists.ExpandResult.Failed)
                    {
                        int time;
                        if (timernameres.Length > 0 && timervaluesres.InvariantParse(out time))
                        {
                            Timer t = new Timer() { Interval = time };
                            t.Tick += Timer_Tick;
                            t.Tag = new TimerInfo() { ap = ap, name = timernameres };
                            timers.Add(t);
                            t.Start();
                        }
                        else
                            ap.ReportError("Timer bad name or time count");
                    }
                    else
                        ap.ReportError(timervaluesres);
                }
                else
                    ap.ReportError(timernameres);
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
           
            ConditionVariables additionalvars = new ConditionVariables();
            additionalvars["TimerName"] = ti.name;
            ti.ap.actioncontroller.ActionRun("onTimer", "ActionTimer", null,additionalvars, now: false);    // queue at end an event

            timers.Remove(t);   // done with it
        }
    }
}
