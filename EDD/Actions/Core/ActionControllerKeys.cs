/*
 * Copyright 2019 EDDiscovery development team
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
        protected string actionfileskeyeventskeydown;
        protected string actionfileskeyeventskeyup;

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
                if (discoveryform.CanFocus)
                {
                    if (m.Msg == WM.KEYDOWN || m.Msg == WM.SYSKEYDOWN)
                    {
                        var activeform = System.Windows.Forms.Form.ActiveForm;

                        if (ignoredforms == null || activeform == null || Array.IndexOf(ignoredforms, activeform.GetType()) == -1)
                        {
                            Keys k = (Keys)m.WParam;
                            string name = k.WMKeyToString((ulong)m.LParam, Control.ModifierKeys);
                            //System.Diagnostics.Debug.WriteLine($"Keydown {(ulong)m.LParam:X} {(ulong)m.WParam:X4} {Control.ModifierKeys} = {name}");
                            if (actcontroller.CheckKeys(name))
                                return true;    // swallow, we did it
                        }
                    }
                    else if (m.Msg == WM.KEYUP)
                    {
                        var activeform = System.Windows.Forms.Form.ActiveForm;
                        //System.Diagnostics.Debug.WriteLine("Active form " + activeform?.GetType().Name);

                        if (ignoredforms == null || activeform == null || Array.IndexOf(ignoredforms, activeform.GetType()) == -1)
                        {
                            Keys k = (Keys)m.WParam;
                            string name = k.WMKeyToString((ulong)m.LParam, Control.ModifierKeys);
                            //  System.Diagnostics.Debug.WriteLine($"Keyup {(ulong)m.LParam:X} {(ulong)m.WParam:X4} {Control.ModifierKeys} = {name}");

                            if (actcontroller.CheckReleaseKeys(name))
                                return true;    // swallow, we did it
                        }
                    }
                }

                return false;
            }
        }

        private string GetKeyEvent(string actionname, string eventname)
        {
            var list = actionfiles.ReturnSpecificConditions(actionname, eventname, new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.Equals, ConditionEntry.MatchType.IsOneOf });        // need these to decide

            string ret = "";

            foreach (var t in list)                  // go thru the list, making up a comparision string with Name, on it..
            {
                if (t.Item2.MatchCondition == ConditionEntry.MatchType.Equals)
                    ret+= "<" + t.Item2.MatchString + ">";
                else
                {
                    StringParser p = new StringParser(t.Item2.MatchString);
                    List<string> klist = p.NextQuotedWordList();
                    if (klist != null)
                    {
                        foreach (string s in klist)
                            ret += "<" + s + ">";
                    }
                }
            }

            return ret;
        }

        public void ActionConfigureKeys()
        {
            actionfileskeyeventskeydown = GetKeyEvent(ActionEventEDList.onKeyPress.TriggerName, "KeyPress");
            actionfileskeyeventskeyup = GetKeyEvent(ActionEventEDList.onKeyReleased.TriggerName, "KeyPress");

            if ( actionfileskeyeventskeydown.HasChars() || actionfileskeyeventskeyup.HasChars())
            {
                if (actionfilesmessagefilter == null)
                {
                    actionfilesmessagefilter = new ActionMessageFilter(DiscoveryForm, this, keyignoredforms);
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
            if (actionfileskeyeventskeydown.Contains("<" + keyname + ">"))  // fast string comparision to determine if key is overridden..
            {
                // do not run directly, we are in a message filter here, so can't add new ones (see Keyform). Instead delay invoke on discovery form

                DiscoveryForm.BeginInvoke(new Action(() => ActionRun(ActionEventEDList.onKeyPress, new Variables("KeyPress", keyname)) ) );
                return true;
            }
            else
                return false;
        }
        public bool CheckReleaseKeys(string keyname)
        {
            if (actionfileskeyeventskeyup.Contains("<" + keyname + ">"))  // fast string comparision to determine if key is overridden..
            {
                DiscoveryForm.BeginInvoke(new Action(() => ActionRun(ActionEventEDList.onKeyReleased, new Variables("KeyPress", keyname))));
                return true;
            }
            else
                return false;
        }
    }
}
