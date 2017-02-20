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
using EDDiscovery.DB;
using EDDiscovery.Forms;
using EDDiscovery.Win32Constants;
using EDDiscovery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionController
    {
        private Actions.ActionFileList actionfiles;
        private string actionfileskeyevents;
        private ActionMessageFilter actionfilesmessagefilter;
        private Actions.ActionRun actionrunasync;

        private ConditionVariables programrunglobalvariables;         // program run, lost at power off, set by GLOBAL or internal 
        private ConditionVariables persistentglobalvariables;   // user variables, set by user only, including user setting vars like SpeechVolume
        private ConditionVariables globalvariables;                  // combo of above.

        private EDDiscoveryForm discoveryform;
        private EDDiscoveryController discoverycontroller;

        public ConditionVariables Globals { get { return globalvariables; } }
        public HistoryList HistoryList { get { return discoverycontroller.history; } }
        public EDDiscoveryForm DiscoveryForm { get { return discoveryform; } }

        public ActionController(EDDiscoveryForm frm, EDDiscoveryController ctrl)
        {
            discoveryform = frm;
            discoverycontroller = ctrl;

            persistentglobalvariables = new ConditionVariables();
            persistentglobalvariables.FromString(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), ConditionVariables.FromMode.MultiEntryComma);

            globalvariables = new ConditionVariables(persistentglobalvariables);        // copy existing user ones into to shared buffer..

            programrunglobalvariables = new ConditionVariables();

            SetInternalGlobal("CurrentCulture", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            SetInternalGlobal("CurrentCultureInEnglish", System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName);
            SetInternalGlobal("CurrentCultureISO", System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

            ReLoad();
        }

        private void ReLoad()
        {
            actionfiles = new Actions.ActionFileList();
            actionfiles.LoadAllActionFiles();
            actionrunasync = new Actions.ActionRun(this, actionfiles);        // this is the guy who runs programs asynchronously
            ActionConfigureKeys();
        }

        public void EditAddOnActionFile()
        {
            EDDiscovery2.ConditionFilterForm frm = new ConditionFilterForm();

            List<string> events = EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(false);
            events.Add("All");
            events.Add("onRefreshStart");
            events.Add("onRefreshEnd");
            events.Add("onInstall");
            events.Add("onStartup");
            events.Add("onShutdown");
            events.Add("onKeyPress");
            events.Add("onTimer");
            events.Add("onPopUp");
            events.Add("onPopDown");
            events.Add("onTabChange");
            events.Add("onPanelChange");
            events.Add("onHistorySelection");
            events.Add("onSayStarted");
            events.Add("onSayFinished");
            events.Add("onPlayStarted");
            events.Add("onPlayFinished");
            events.Add("onMenuItem");

            frm.InitAction("Actions: Define actions", events, globalvariables.KeyList, persistentglobalvariables, actionfiles, discoveryform);
            frm.TopMost = discoveryform.FindForm().TopMost;

            frm.ShowDialog(discoveryform.FindForm()); // don't care about the result, the form does all the saving

            persistentglobalvariables = frm.userglobalvariables;
            globalvariables = new ConditionVariables(programrunglobalvariables, persistentglobalvariables);    // remake

            ActionConfigureKeys();
        }

        public void ManageAddOns()
        {
            using (DownloadManagerForm dmf = new DownloadManagerForm())
            {
                dmf.Init(discoveryform.theme);
                dmf.ShowDialog(discoveryform);
                if (dmf.changelist.Count > 0)
                {
                    actionrunasync.TerminateAll();
                    discoveryform.AudioQueueSpeech.StopAll();
                    ReLoad();

                    string changes = "";
                    foreach (KeyValuePair<string, string> kv in dmf.changelist)
                    {
                        if (kv.Value.Equals("+"))
                            changes += kv.Key + ";";
                        if (kv.Value.Equals("-"))
                            discoveryform.RemoveMenuItemsFromAddOns(kv.Key);
                    }

                    ActionRun("onInstall", "ProgramEvent", null, new ConditionVariables("InstallList", changes));
                }
            }
        }

        public void ConfigureVoice(string title)
        {
            string voicename = persistentglobalvariables.GetString(Actions.ActionSay.globalvarspeechvoice, "Default");
            string volume = persistentglobalvariables.GetString(Actions.ActionSay.globalvarspeechvolume,"Default");
            string rate = persistentglobalvariables.GetString(Actions.ActionSay.globalvarspeechrate,"Default");
            ConditionVariables effects = new ConditionVariables( persistentglobalvariables.GetString(Actions.ActionSay.globalvarspeecheffects, ""),ConditionVariables.FromMode.MultiEntryComma);

            Audio.SpeechConfigure cfg = new Audio.SpeechConfigure();
            cfg.Init( discoveryform.AudioQueueSpeech, discoveryform.SpeechSynthesizer,
                        "Select voice synthesizer defaults", title, 
                        null, false, Audio.AudioQueue.Priority.Normal, "", "",
                        voicename,
                        volume,
                        rate, 
                        effects);

            if (cfg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                SetPeristentGlobal(Actions.ActionSay.globalvarspeechvoice, cfg.VoiceName);
                SetPeristentGlobal(Actions.ActionSay.globalvarspeechvolume, cfg.Volume);
                SetPeristentGlobal(Actions.ActionSay.globalvarspeechrate, cfg.Rate);
                SetPeristentGlobal(Actions.ActionSay.globalvarspeecheffects, cfg.Effects.ToString());

                EDDConfig.Instance.DefaultVoiceDevice = discoveryform.AudioQueueSpeech.Driver.GetAudioEndpoint();
            }
        }

        public void ConfigureWave(string title)
        {
            string volume = persistentglobalvariables.GetString(Actions.ActionPlay.globalvarplayvolume, "60");
            ConditionVariables effects = new ConditionVariables(persistentglobalvariables.GetString(Actions.ActionPlay.globalvarplayeffects, ""), ConditionVariables.FromMode.MultiEntryComma);

            Audio.WaveConfigureDialog dlg = new Audio.WaveConfigureDialog();
            dlg.Init(discoveryform.AudioQueueWave, true, "Select Default device, volume and effects", title, "",
                        false, Audio.AudioQueue.Priority.Normal, "", "",
                        volume, effects);

            if (dlg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                ConditionVariables cond = new ConditionVariables(dlg.Effects);// add on any effects variables (and may add in some previous variables, since we did not purge)

                SetPeristentGlobal(Actions.ActionPlay.globalvarplayvolume, dlg.Volume);
                SetPeristentGlobal(Actions.ActionPlay.globalvarplayeffects, dlg.Effects.ToString());

                EDDConfig.Instance.DefaultWaveDevice = discoveryform.AudioQueueWave.Driver.GetAudioEndpoint();
            }
        }


        public void EditSpeechText()
        {
            if (programrunglobalvariables.ContainsKey("SpeechDefinitionFile"))
            {
                string prog = programrunglobalvariables["SpeechDefinitionFile"];

                Tuple<ActionFile, ActionProgram> ap = actionfiles.FindProgram(prog);

                if (ap != null && ap.Item2.EditInEditor())
                {
                    ap.Item1.SaveFile();
                    return;
                }
            }

            Forms.MessageBoxTheme.Show(discoveryform, "Voice pack not loaded, or needs updating to support this functionality");
        }

        public void EditLastTextFile()
        {
            if (ActionProgramForm.LastTextEditedFile != null )
            {
                Tuple<ActionFile, ActionProgram> ap = actionfiles.FindProgram(ActionProgramForm.LastTextEditedFile);

                if (ap != null && ap.Item2.EditInEditor())
                {
                    ap.Item1.SaveFile();
                    return;
                }
            }
        }

        public void ActionRunOnRefresh()
        {
            string prevcommander = programrunglobalvariables.ContainsKey("Commander") ? programrunglobalvariables["Commander"] : "None";
            string commander = (discoverycontroller.history.CommanderId < 0) ? "Hidden" : EDDConfig.Instance.CurrentCommander.Name;

            string refreshcount = prevcommander.Equals(commander) ? programrunglobalvariables.AddToVar("RefreshCount", 1, 1) : "1";
            SetInternalGlobal("RefreshCount", refreshcount);
            SetInternalGlobal("Commander", commander);

            if (actionfiles.IsConditionFlagSet(ConditionVariables.flagRunAtRefresh))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in discoverycontroller.history.EntryOrder)
                    ActionRunOnEntry(he, "onRefresh", ConditionVariables.flagRunAtRefresh);
            }

            ActionRun("onRefreshEnd", "ProgramEvent");
        }

        public int ActionRunOnEntry(HistoryEntry he, string triggertype, string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            return ActionRun(he.journalEntry.EventTypeStr, triggertype, he, null, flagstart, now);
        }

        public int ActionRun(string triggername, string triggertype, HistoryEntry he = null, ConditionVariables additionalvars = null ,
                                string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(triggername, flagstart);

            if (ale.Count > 0)
            {
                ConditionVariables eventvars = new ConditionVariables();
                Actions.ActionVars.TriggerVars(eventvars, triggername, triggertype);
                Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
                eventvars.Add(additionalvars);   // adding null is allowed

                ConditionVariables testvars = new ConditionVariables(globalvariables);
                testvars.Add(eventvars);

                ConditionFunctions functions = new ConditionFunctions();

                if (actionfiles.CheckActions(ale, (he!=null) ? he.journalEntry.EventDataString : null, testvars, functions.ExpandString) > 0)
                {
                    actionfiles.RunActions(now, ale, actionrunasync, eventvars);  // add programs to action run

                    actionrunasync.Execute();       // See if needs executing
                }
            }

            return ale.Count;
        }

        public bool ActionRun(string packname, string programname , ConditionVariables runvars , bool now = false )
        {
            Tuple<ActionFile, ActionProgram> found = actionfiles.FindProgram(packname, programname);

            if (found != null)
            {
                actionrunasync.Run(now, found.Item1, found.Item2, runvars);
                actionrunasync.Execute();       // See if needs executing

                return true;
            }
            else
                return false;
        }

        public void SetPeristentGlobal(string name, string value)     // saved on exit
        {
            persistentglobalvariables[name] = globalvariables[name] = value;
        }

        public void SetInternalGlobal(string name, string value)           // internal program vars
        {
            programrunglobalvariables[name] = globalvariables[name] = value;
        }

        public void SetNonPersistentGlobal(string name, string value)         // different name for identification purposes, for sets
        {
            programrunglobalvariables[name] = globalvariables[name] = value;
        }

        public void DeleteVariable(string name)
        {
            programrunglobalvariables.Delete(name);
            persistentglobalvariables.Delete(name);
            globalvariables.Delete(name); 
        }

        public void TerminateAll()
        {
            actionrunasync.TerminateAll();
        }

        public void CloseDown()
        {
            actionrunasync.WaitTillFinished(10000);
            SQLiteConnectionUser.PutSettingString("UserGlobalActionVars", persistentglobalvariables.ToString());
        }

        public void LogLine(string s)
        {
            discoveryform.LogLine(s);
        }

        void ActionConfigureKeys()
        {
            List<Tuple<string, ConditionLists.MatchType>> ret = actionfiles.ReturnValuesOfSpecificConditions("KeyPress", new List<ConditionLists.MatchType>() { ConditionLists.MatchType.Equals, ConditionLists.MatchType.IsOneOf });        // need these to decide

            if (ret.Count > 0)
            {
                actionfileskeyevents = "";
                foreach (Tuple<string, ConditionLists.MatchType> t in ret)                  // go thru the list, making up a comparision string with Name, on it..
                {
                    if (t.Item2 == ConditionLists.MatchType.Equals)
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
                    actionfilesmessagefilter = new ActionMessageFilter(discoveryform,this);
                    Application.AddMessageFilter(actionfilesmessagefilter);
                    System.Diagnostics.Debug.WriteLine("Installed message filter for keys");
                }
            }
            else if (actionfilesmessagefilter != null)
            {
                Application.RemoveMessageFilter(actionfilesmessagefilter);
                actionfilesmessagefilter = null;
                System.Diagnostics.Debug.WriteLine("Removed message filter for keys");
            }
        }

        public bool CheckKeys(string keyname)
        {
            if (actionfileskeyevents.Contains("<" + keyname + ">"))  // fast string comparision to determine if key is overridden..
            {
                globalvariables["KeyPress"] = keyname;          // only add it to global variables, its not kept in internals.
                ActionRun("onKeyPress", "KeyPress");
                return true;
            }
            else
                return false;
        }

        private class ActionMessageFilter : IMessageFilter
        {
            EDDiscoveryForm discoveryform;
            ActionController actcontroller;
            public ActionMessageFilter(EDDiscoveryForm frm, ActionController ac)
            {
                discoveryform = frm;
                actcontroller = ac;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg == WM.KEYDOWN || m.Msg == WM.SYSKEYDOWN) && discoveryform.CanFocus)
                {
                    Keys k = (Keys)m.WParam;
                    if (k != Keys.ControlKey && k != Keys.ShiftKey && k != Keys.Menu)
                    {
                        //System.Diagnostics.Debug.WriteLine("Keydown " + m.LParam + " " + k.ToString(Control.ModifierKeys) + " " + m.WParam + " " + Control.ModifierKeys);
                        if (actcontroller.CheckKeys(k.ToString(Control.ModifierKeys)))
                            return true;    // swallow, we did it
                    }
                }

                return false;
            }
        }


    }
}
