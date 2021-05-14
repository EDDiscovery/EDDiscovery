/*
 * Copyright © 2019 EDDiscovery development team
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

using ActionLanguage;
using BaseUtils;
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public partial class ActionController : ActionCoreController
    {
        protected ActionMessageFilter actionfilesmessagefilter;
        protected string actionfileskeyevents;

        protected class ActionMessageFilter : IMessageFilter
        {
            EDDiscoveryForm discoveryform;
            ActionController actcontroller;
            Type[] ignoredforms;

            public ActionMessageFilter(EDDiscoveryForm frm, ActionController ac, Type[] ignorewhenactive )
            {
                discoveryform = frm;
                actcontroller = ac;
                ignoredforms = ignorewhenactive;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg == WM.KEYDOWN || m.Msg == WM.SYSKEYDOWN) && discoveryform.CanFocus)
                {
                    var activeform = System.Windows.Forms.Form.ActiveForm;
                    //System.Diagnostics.Debug.WriteLine("Active form " + activeform?.GetType().Name);

                    if (ignoredforms == null || activeform == null || Array.IndexOf(ignoredforms, activeform.GetType()) == -1)
                    {
                        Keys k = (Keys)m.WParam;

                        if (k != Keys.ControlKey && k != Keys.ShiftKey && k != Keys.Menu)
                        {
                            string name = k.VKeyToString(Control.ModifierKeys);
                            //System.Diagnostics.Debug.WriteLine("Keydown " + m.LParam + " " + name + " " + m.WParam + " " + Control.ModifierKeys);
                            if (actcontroller.CheckKeys(name))
                                return true;    // swallow, we did it
                        }
                    }
                }

                return false;
            }
        }

        public void ActionConfigureKeys()
        {
            List<Tuple<string, ConditionEntry.MatchType>> ret = actionfiles.ReturnValuesOfSpecificConditions("KeyPress", new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.Equals, ConditionEntry.MatchType.IsOneOf });        // need these to decide

            if (ret.Count > 0)
            {
                actionfileskeyevents = "";
                foreach (Tuple<string, ConditionEntry.MatchType> t in ret)                  // go thru the list, making up a comparision string with Name, on it..
                {
                    if (t.Item2 == ConditionEntry.MatchType.Equals)
                        actionfileskeyevents += "<" + t.Item1 + ">";
                    else
                    {
                        StringParser p = new StringParser(t.Item1);
                        List<string> klist = p.NextQuotedWordList();
                        if (klist != null)
                        {
                            foreach (string s in klist)
                                actionfileskeyevents += "<" + s + ">";
                        }
                    }
                }

                if (actionfilesmessagefilter == null)
                {
                    actionfilesmessagefilter = new ActionMessageFilter(discoveryform, this, keyignoredforms);
                    Application.AddMessageFilter(actionfilesmessagefilter);
                }
            }
            else if (actionfilesmessagefilter != null)
            {
                Application.RemoveMessageFilter(actionfilesmessagefilter);
                actionfilesmessagefilter = null;
            }
        }


        public bool CheckKeys(string keyname)
        {
            if (actionfileskeyevents.Contains("<" + keyname + ">"))  // fast string comparision to determine if key is overridden..
            {
                ActionRun(ActionEventEDList.onKeyPress, new Variables("KeyPress", keyname));
                return true;
            }
            else
                return false;
        }
    }
}
