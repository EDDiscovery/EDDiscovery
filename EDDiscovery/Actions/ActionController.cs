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

        private string lasteditedpack;

        public ConditionVariables Globals { get { return globalvariables; } }
        public HistoryList HistoryList { get { return discoverycontroller.history; } }
        public EDDiscoveryForm DiscoveryForm { get { return discoveryform; } }

        public ActionController(EDDiscoveryForm frm, EDDiscoveryController ctrl)
        {
            Action.cmdlist = cmdlist;       // tell actions which commands we support.

            discoveryform = frm;
            discoverycontroller = ctrl;

            persistentglobalvariables = new ConditionVariables();
            persistentglobalvariables.FromString(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), ConditionVariables.FromMode.MultiEntryComma);

            globalvariables = new ConditionVariables(persistentglobalvariables);        // copy existing user ones into to shared buffer..

            programrunglobalvariables = new ConditionVariables();

            SetInternalGlobal("CurrentCulture", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            SetInternalGlobal("CurrentCultureInEnglish", System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName);
            SetInternalGlobal("CurrentCultureISO", System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

            lasteditedpack = SQLiteConnectionUser.GetSettingString("ActionPackLastFile", "");
            ReLoad();
        }

        // Controller owns this, any commands can be plugged into the action system

        static private Action.Commands[] cmdlist = new Action.Commands[]
        {
            new Action.Commands("Break", typeof(ActionBreak) , Action.ActionType.Cmd),
            new Action.Commands("Call", typeof(ActionCall) , Action.ActionType.Call),
            new Action.Commands("Commodities", typeof(ActionCommodities) , Action.ActionType.Cmd),
            new Action.Commands("Dialog", typeof(ActionDialog) , Action.ActionType.Cmd),
            new Action.Commands("DialogControl", typeof(ActionDialogControl) , Action.ActionType.Cmd),
            new Action.Commands("Do", typeof(ActionDo) , Action.ActionType.Do),
            new Action.Commands("DeleteVariable", typeof(ActionDeleteVariable) , Action.ActionType.Cmd),
            new Action.Commands("Expr", typeof(ActionExpr), Action.ActionType.Cmd),
            new Action.Commands("Else", typeof(ActionElse), Action.ActionType.Else),
            new Action.Commands("ElseIf", typeof(ActionElseIf) , Action.ActionType.ElseIf),
            new Action.Commands("EliteBindings", typeof(ActionEliteBindings) , Action.ActionType.Cmd),
            new Action.Commands("End", typeof(ActionEnd) , Action.ActionType.Cmd),
            new Action.Commands("ErrorIf", typeof(ActionErrorIf) , Action.ActionType.Cmd),
            new Action.Commands("Event", typeof(ActionEvent) , Action.ActionType.Cmd),
            new Action.Commands("FileDialog", typeof(ActionFileDialog) , Action.ActionType.Cmd),
            new Action.Commands("GlobalLet", typeof(ActionGlobalLet) , Action.ActionType.Cmd),
            new Action.Commands("Global", typeof(ActionGlobal) , Action.ActionType.Cmd),
            new Action.Commands("Historytab", typeof(ActionHistoryTab) , Action.ActionType.Cmd),
            new Action.Commands("If", typeof(ActionIf) , Action.ActionType.If),
            new Action.Commands("InputBox", typeof(ActionInputBox) , Action.ActionType.Cmd),
            new Action.Commands("InfoBox", typeof(ActionInfoBox) , Action.ActionType.Cmd),
            new Action.Commands("Ledger", typeof(ActionLedger) , Action.ActionType.Cmd),
            new Action.Commands("Let", typeof(ActionLet) , Action.ActionType.Cmd),
            new Action.Commands("Loop", typeof(ActionLoop) , Action.ActionType.Loop),
            new Action.Commands("Materials", typeof(ActionMaterials) , Action.ActionType.Cmd),
            new Action.Commands("MessageBox", typeof(ActionMessageBox) , Action.ActionType.Cmd),
            new Action.Commands("MenuItem", typeof(ActionMenuItem) , Action.ActionType.Cmd),
            new Action.Commands("NonModalDialog", typeof(ActionNonModalDialog) , Action.ActionType.Cmd),
            new Action.Commands("Rem", typeof(ActionRem) , Action.ActionType.Cmd),
            new Action.Commands("Return", typeof(ActionReturn) , Action.ActionType.Return),
            new Action.Commands("Perform", typeof(ActionPerform) , Action.ActionType.Cmd),
            new Action.Commands("PersistentGlobal", typeof(ActionPersistentGlobal) , Action.ActionType.Cmd),
            new Action.Commands("Play", typeof(ActionPlay) , Action.ActionType.Cmd),
            new Action.Commands("Popout", typeof(ActionPopout) , Action.ActionType.Cmd),
            new Action.Commands("Pragma", typeof(ActionPragma) , Action.ActionType.Cmd),
            new Action.Commands("Print", typeof(ActionPrint) , Action.ActionType.Cmd),
            new Action.Commands("ProgramWindow", typeof(ActionProgramwindow) , Action.ActionType.Cmd),
            new Action.Commands("Say", typeof(ActionSay), Action.ActionType.Cmd ),
            new Action.Commands("Scan", typeof(ActionScan) , Action.ActionType.Cmd),
            new Action.Commands("Set", typeof(ActionSet) , Action.ActionType.Cmd),
            new Action.Commands("Ship", typeof(ActionShip) , Action.ActionType.Cmd),
            new Action.Commands("Star", typeof(ActionStar) , Action.ActionType.Cmd),
            new Action.Commands("Static", typeof(ActionStatic) , Action.ActionType.Cmd),
            new Action.Commands("Sleep", typeof(ActionSleep) , Action.ActionType.Cmd),
            new Action.Commands("Timer", typeof(ActionTimer) , Action.ActionType.Cmd),
            new Action.Commands("While", typeof(ActionWhile) , Action.ActionType.While),
            new Action.Commands("//", typeof(ActionFullLineComment) , Action.ActionType.Cmd),
            new Action.Commands("Else If", typeof(ActionElseIf) , Action.ActionType.ElseIf),
        };

        public void ReLoad( bool completereload = true)        // COMPLETE reload..
        {
            if ( completereload )
                actionfiles = new Actions.ActionFileList();     // clear the list

            string errlist = actionfiles.LoadAllActionFiles();
            if (errlist.Length > 0)
                EDDiscovery.Forms.MessageBoxTheme.Show("Failed to load files\r\n" + errlist, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            actionrunasync = new Actions.ActionRun(this, actionfiles);        // this is the guy who runs programs asynchronously
            ActionConfigureKeys();
        }

        public void EditLastPack()
        {
            if (lasteditedpack.Length > 0 && EditActionFile(lasteditedpack))
                return;
            else
                EDDiscovery.Forms.MessageBoxTheme.Show("Action pack does not exist anymore or never set", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool EditActionFile(string name)
        {
            List<string> jevents = EDDiscovery.EliteDangerous.JournalEntry.GetListOfEventsWithOptMethod(towords: false);
            jevents.Sort();

            List<Tuple<string, string>> eventnames = new List<Tuple<string, string>>();
            foreach (string s in jevents)
                eventnames.Add(new Tuple<string, string>(s, "Journal"));

            eventnames.Add(new Tuple<string, string>("All", "Misc"));
            eventnames.Add(new Tuple<string, string>("onRefreshStart", "Program"));
            eventnames.Add(new Tuple<string, string>("onRefreshEnd", "Program"));
            eventnames.Add(new Tuple<string, string>("onInstall", "Program"));
            eventnames.Add(new Tuple<string, string>("onStartup", "Program"));
            eventnames.Add(new Tuple<string, string>("onPostStartup", "Program"));
            eventnames.Add(new Tuple<string, string>("onShutdown", "Program"));
            eventnames.Add(new Tuple<string, string>("onKeyPress", "UI"));
            eventnames.Add(new Tuple<string, string>("onTimer", "Action"));
            eventnames.Add(new Tuple<string, string>("onPopUp", "UI"));
            eventnames.Add(new Tuple<string, string>("onPopDown", "UI"));
            eventnames.Add(new Tuple<string, string>("onTabChange", "UI"));
            eventnames.Add(new Tuple<string, string>("onPanelChange", "UI"));
            eventnames.Add(new Tuple<string, string>("onHistorySelection", "UI"));
            eventnames.Add(new Tuple<string, string>("onSayStarted", "Audio"));
            eventnames.Add(new Tuple<string, string>("onSayFinished", "Audio"));
            eventnames.Add(new Tuple<string, string>("onPlayStarted", "Audio"));
            eventnames.Add(new Tuple<string, string>("onPlayFinished", "Audio"));
            eventnames.Add(new Tuple<string, string>("onMenuItem", "UI"));
            eventnames.Add(new Tuple<string, string>("onEliteInputRaw", "EliteUI"));
            eventnames.Add(new Tuple<string, string>("onEliteInput", "EliteUI"));
            eventnames.Add(new Tuple<string, string>("onEliteInputOff", "EliteUI"));

            ActionPackEditorForm frm = new ActionPackEditorForm();
            ActionFile f = actionfiles.Get(name);

            if (f != null)
            {
                frm.Init("Edit pack " + name, f, eventnames, discoveryform, discoveryform.Globals.NameList);
                frm.TopMost = discoveryform.FindForm().TopMost;

                frm.ShowDialog(discoveryform.FindForm()); // don't care about the result, the form does all the saving

                ActionConfigureKeys();

                lasteditedpack = name;
                SQLiteConnectionUser.PutSettingString("ActionPackLastFile", lasteditedpack);
                return true;
            }
            else
                return false;
        }

        private void Dmf_OnEditActionFile(string name)
        {
            EditActionFile(name);
        }

        private void Dmf_OnEditGlobals()
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Global User variables to pass to program on run", discoveryform.theme, persistentglobalvariables, showone: true);

            if (avf.ShowDialog(discoveryform.FindForm()) == DialogResult.OK)
            {
                persistentglobalvariables = avf.result;
                globalvariables = new ConditionVariables(programrunglobalvariables, persistentglobalvariables);    // remake
            }
        }

        private void Dmf_OnCreateActionFile()
        {
            String r = Forms.PromptSingleLine.ShowDialog(discoveryform.FindForm(), "New name", "", "Create new action file");
            if ( r != null )
            {
                if (actionfiles.Get(r, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    actionfiles.CreateSet(r);
                }
                else
                    Forms.MessageBoxTheme.Show(discoveryform.FindForm(), "Duplicate name", "Create Action File Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void ManageAddOns()
        {
            RunAddOns(true);
        }

        public void EditAddOns()
        {
            RunAddOns(false);
        }

        private void RunAddOns(bool manage)
        {
            using (AddOnManagerForm dmf = new AddOnManagerForm())
            {
                dmf.Init(manage);

                dmf.EditActionFile += Dmf_OnEditActionFile;     // only used when manage = false
                dmf.EditGlobals += Dmf_OnEditGlobals;
                dmf.CreateActionFile += Dmf_OnCreateActionFile;
                dmf.CheckActionLoaded += Dmf_checkActionLoaded;

                dmf.ShowDialog(discoveryform);

                if (dmf.changelist.Count > 0)
                {
                    actionrunasync.TerminateAll();
                    discoveryform.AudioQueueSpeech.StopAll();
                    ReLoad(false);      // reload from disk, new ones if required, refresh old ones and keep the vars

                    string changes = "";
                    foreach (KeyValuePair<string, string> kv in dmf.changelist)
                    {
                        if (kv.Value.Equals("+"))
                        {

                            changes += kv.Key + ";";

                        }
                        if (kv.Value.Equals("-"))
                            discoveryform.RemoveMenuItemsFromAddOns(kv.Key);
                    }

                    ActionRun("onInstall", "ProgramEvent", null, new ConditionVariables("InstallList", changes));
                }
            }
        }

        private bool Dmf_checkActionLoaded(string name)
        {
            ActionFile f = actionfiles.Get(name);
            return f != null;
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
                        null, false, false, Audio.AudioQueue.Priority.Normal, "", "",
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
            if (programrunglobalvariables.Exists("SpeechDefinitionFile"))
            {
                string prog = programrunglobalvariables["SpeechDefinitionFile"];

                Tuple<ActionFile, ActionProgram> ap = actionfiles.FindProgram(prog);

                if (ap != null && ap.Item2.EditInEditor())
                {
                    ap.Item1.WriteFile();
                    return;
                }
            }

            Forms.MessageBoxTheme.Show(discoveryform, "Voice pack not loaded, or needs updating to support this functionality");
        }

        public void ActionRunOnRefresh()
        {
            string prevcommander = programrunglobalvariables.Exists("Commander") ? programrunglobalvariables["Commander"] : "None";
            string commander = (discoverycontroller.history.CommanderId < 0) ? "Hidden" : EDCommander.Current.Name;

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
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(triggername, flagstart);      // look thru all actions, find matching ones

            if (ale.Count > 0)                  
            {
                ConditionVariables eventvars = new ConditionVariables();
                Actions.ActionVars.TriggerVars(eventvars, triggername, triggertype);
                Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
                Actions.ActionVars.ShipBasicInformation(eventvars, he?.ShipInformation, "Event");     // if He null, or si null, ignore
                Actions.ActionVars.SystemVars(eventvars, he?.System, "Event");
                eventvars.Add(additionalvars);   // adding null is allowed

                ConditionVariables testvars = new ConditionVariables(globalvariables);
                testvars.Add(eventvars);

                ConditionFunctions functions = new ConditionFunctions(testvars,null);

                if (actionfiles.CheckActions(ale, he?.journalEntry, testvars, functions) > 0)
                {
                    actionfiles.RunActions(now, ale, actionrunasync, eventvars);  // add programs to action run

                    actionrunasync.Execute();       // See if needs executing
                }
            }

            return ale.Count;
        }

        public bool ActionRunProgram(string packname, string programname , ConditionVariables runvars , bool now = false )
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

        public void onStartup()
        {
            ActionRun("onStartup", "ProgramEvent");
            ActionRun("onPostStartup", "ProgramEvent");
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
                    actionfilesmessagefilter = new ActionMessageFilter(discoveryform,this);
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
