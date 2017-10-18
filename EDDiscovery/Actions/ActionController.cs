﻿/*
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
using EDDiscovery.Forms;
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUtils;
using ActionLanguage;
using Conditions;
using AudioExtensions;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    public class ActionController : ActionCoreController
    {
        private EDDiscoveryForm discoveryform;
        private EDDiscoveryController discoverycontroller;

        protected ActionMessageFilter actionfilesmessagefilter;
        protected string actionfileskeyevents;

        public HistoryList HistoryList { get { return discoverycontroller.history; } }
        public EDDiscoveryForm DiscoveryForm { get { return discoveryform; } }

        private string lasteditedpack;

        static public ConditionFunctionHandlers DefaultGetCFH(ConditionFunctions c, ConditionVariables vars, ConditionPersistentData handles, int recdepth)
        {
            return new ConditionEDDFunctions(c, vars, handles, recdepth);
        }

        public ActionController(EDDiscoveryForm frm, EDDiscoveryController ctrl, System.Drawing.Icon ic) : base(frm.AudioQueueSpeech, frm.AudioQueueWave, frm.SpeechSynthesizer, frm , ic)
        {
            discoveryform = frm;
            discoverycontroller = ctrl;

            ConditionFunctions.GetCFH = DefaultGetCFH;

            LoadPeristentVariables(new ConditionVariables(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), ConditionVariables.FromMode.MultiEntryComma));

            lasteditedpack = SQLiteConnectionUser.GetSettingString("ActionPackLastFile", "");

            ActionBase.AddCommand("Commodities", typeof(ActionCommodities), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("EliteBindings", typeof(ActionEliteBindings), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Event", typeof(ActionEventCmd), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Historytab", typeof(ActionHistoryTab), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Key", typeof(ActionKeyED), ActionBase.ActionType.Cmd);       // override key
            ActionBase.AddCommand("Ledger", typeof(ActionLedger), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Materials", typeof(ActionMaterials), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("MenuItem", typeof(ActionMenuItem), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Perform", typeof(ActionPerform), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Play", typeof(ActionPlay), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Popout", typeof(ActionPopout), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("ProgramWindow", typeof(ActionProgramwindow), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Scan", typeof(ActionScan), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Ship", typeof(ActionShip), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Star", typeof(ActionStar), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Timer", typeof(ActionTimer), ActionBase.ActionType.Cmd);

            ReLoad();
        }

        static public string AppFolder { get { return System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "Actions"); } }

        public void ReLoad( bool completereload = true)        // COMPLETE reload..
        {
            if ( completereload )
                actionfiles = new ActionFileList();     // clear the list

            string errlist = actionfiles.LoadAllActionFiles(AppFolder);
            if (errlist.Length > 0)
                ExtendedControls.MessageBoxTheme.Show(discoveryform, "Failed to load files\r\n" + errlist, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            actionrunasync = new ActionRun(this, actionfiles);        // this is the guy who runs programs asynchronously
            ActionConfigureKeys();
        }

        #region Edit Action Packs

        public void EditLastPack()
        {
            if (lasteditedpack.Length > 0 && EditActionFile(lasteditedpack))
                return;
            else
                ExtendedControls.MessageBoxTheme.Show(discoveryform, "Action pack does not exist anymore or never set", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool EditActionFile(string name)            // edit pack name
        {
            List<string> jevents = JournalEntry.GetListOfEventsWithOptMethod(towords: false);
            jevents.Sort();
            List<ActionEvent> eventlist = ActionEventEDList.EventsFromNames(jevents, "Journal");
            eventlist.AddRange(ActionEventEDList.events); // our ED events
            eventlist.AddRange(ActionEvent.events);     // core events

            ActionPackEditorForm frm = new ActionPackEditorForm();

            frm.AdditionalNames += Frm_onAdditionalNames;
            frm.CreateActionPackEdit += SetPackEditorAndCondition;
            ActionFile f = actionfiles.Get(name);

            if (f != null)
            {
                frm.Init("Edit pack " + name, this.Icon, this, AppFolder, f, eventlist);

                frm.ShowDialog(discoveryform); // don't care about the result, the form does all the saving

                ActionConfigureKeys();

                lasteditedpack = name;
                SQLiteConnectionUser.PutSettingString("ActionPackLastFile", lasteditedpack);
                return true;
            }
            else
                return false;
        }

        // called when a new group is set up, what editor do you want?  and you can alter the condition
        // for new entries, cd = AlwaysTrue. For older entries, the condition
        private ActionPackEditBase SetPackEditorAndCondition(string group, Condition cd)        
        {
            List<string> addnames = new List<string>() { "{one}", "{two}" };

            if (group == "Voice")
            {
                ActionPackEditVoice ev = new ActionPackEditVoice();
                ev.Height = 28;
                ev.onEditKeys = onEditKeys;
                ev.onEditSay = onEditSay;

                // make sure the voice condition is right.. if not, reset.

                if (cd.eventname != Actions.ActionEventEDList.onVoiceInput.triggername || !cd.Is("VoiceInput", ConditionEntry.MatchType.Equals))
                {
                    cd.eventname = Actions.ActionEventEDList.onVoiceInput.triggername;
                    cd.Set(new ConditionEntry("VoiceInput", ConditionEntry.MatchType.Equals, "?"));     // Voiceinput being the variable set to the expression
                }

                return ev;
            }
            else
            {
                ActionPackEditEventProgramCondition eb = new ActionPackEditEventProgramCondition();
                eb.autosetcondition += SetEventCondition;
                eb.Height = 28;
                eb.onEditKeys = onEditKeys;
                eb.onEditSay = onEditSay;
                return eb;
            }
        }

        private string onEditKeys(Control p, System.Drawing.Icon i, string keys)      // called when program wants to edit Key
        {
            return ActionKeyED.Menu(p, i, keys, discoveryform.FrontierBindings);
        }

        private string onEditSay(Control p, string say, ActionCoreController cp)      // called when program wants to edit Key
        {
            return ActionSay.Menu(p, say, cp);
        }


        // called when a event name is selected, what is the initial condition set up?
        private ActionProgram.ProgramConditionClass SetEventCondition( Condition cd)          
        {                                                       // and what is the class selection for the program?
            ActionProgram.ProgramConditionClass cls = ActionProgram.ProgramConditionClass.Full;

            if (cd.IsAlwaysTrue())
            {
                if (cd.eventname == "onKeyPress")
                    cd.Set(new ConditionEntry("KeyPress", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTimer")
                    cd.Set(new ConditionEntry("TimerName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPopUp" || cd.eventname == "onPopDown")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onTabChange")
                    cd.Set(new ConditionEntry("TabName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onPanelChange")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onEliteInput" || cd.eventname == "onEliteInputOff")
                    cd.Set(new ConditionEntry("Binding", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onMenuItem")
                    cd.Set(new ConditionEntry("MenuName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onSayStarted" || cd.eventname == "onSayFinished" || cd.eventname == "onPlayStarted" || cd.eventname == "onPlayFinished")
                    cd.Set(new ConditionEntry("EventName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.eventname == "onVoiceInput")
                    cls = ActionProgram.ProgramConditionClass.KeySay;
            }

            return cls;
        }

        private List<string> Frm_onAdditionalNames(string evname)       // call back to discover name list.  evname may be empty
        {
            List<string> fieldnames = new List<string>(discoveryform.Globals.NameList);
            fieldnames.Sort();

            if ( evname.HasChars())
            { 
                List<string> classnames = BaseUtils.FieldNames.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(evname), "EventClass_");
                if (classnames != null)
                    fieldnames.InsertRange(0, classnames);
            }

            return fieldnames;
        }

        private void Dmf_OnEditActionFile(string name)      // wanna edit
        {
            EditActionFile(name);
        }

        private void Dmf_OnEditGlobals()                    // edit the globals
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Global User variables to pass to program on run", this.Icon, PersistentVariables, showone: true);

            if (avf.ShowDialog(discoveryform) == DialogResult.OK)
            {
                LoadPeristentVariables(avf.result);
            }
        }

        private void Dmf_OnCreateActionFile()
        {
            String r = ExtendedControls.PromptSingleLine.ShowDialog(discoveryform, "New name", "", "Create new action file" , this.Icon);
            if ( r != null && r.Length>0 )
            {
                if (actionfiles.Get(r, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    actionfiles.CreateSet(r,AppFolder);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(discoveryform, "Duplicate name", "Create Action File Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region Manage them

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
                dmf.Init(manage, this.Icon);

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

                    ActionRun(ActionEventEDList.onInstall, null, new ConditionVariables("InstallList", changes));
                }
            }
        }

        private bool Dmf_checkActionLoaded(string name)
        {
            ActionFile f = actionfiles.Get(name);
            return f != null;
        }

        #endregion

        public void ConfigureVoice(string title)
        {
            string voicename = Globals.GetString(ActionSay.globalvarspeechvoice, "Default");
            string volume = Globals.GetString(ActionSay.globalvarspeechvolume,"Default");
            string rate = Globals.GetString(ActionSay.globalvarspeechrate,"Default");
            ConditionVariables effects = new ConditionVariables( PersistentVariables.GetString(ActionSay.globalvarspeecheffects, ""),ConditionVariables.FromMode.MultiEntryComma);

            SpeechConfigure cfg = new SpeechConfigure();
            cfg.Init( discoveryform.AudioQueueSpeech, discoveryform.SpeechSynthesizer,
                        "Select voice synthesizer defaults", title, this.Icon,
                        null, false, false, AudioExtensions.AudioQueue.Priority.Normal, "", "",
                        voicename,
                        volume,
                        rate, 
                        effects);

            if (cfg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                SetPeristentGlobal(ActionSay.globalvarspeechvoice, cfg.VoiceName);
                SetPeristentGlobal(ActionSay.globalvarspeechvolume, cfg.Volume);
                SetPeristentGlobal(ActionSay.globalvarspeechrate, cfg.Rate);
                SetPeristentGlobal(ActionSay.globalvarspeecheffects, cfg.Effects.ToString());

                EDDConfig.Instance.DefaultVoiceDevice = discoveryform.AudioQueueSpeech.Driver.GetAudioEndpoint();
            }
        }

        public void ConfigureWave(string title)
        {
            string volume = Globals.GetString(ActionPlay.globalvarplayvolume, "60");
            ConditionVariables effects = new ConditionVariables(PersistentVariables.GetString(ActionPlay.globalvarplayeffects, ""), ConditionVariables.FromMode.MultiEntryComma);

            WaveConfigureDialog dlg = new WaveConfigureDialog();
            dlg.Init(discoveryform.AudioQueueWave, true, 
                        "Select Default device, volume and effects", title, this.Icon,
                        "",
                        false, AudioExtensions.AudioQueue.Priority.Normal, "", "",
                        volume, effects);

            if (dlg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                ConditionVariables cond = new ConditionVariables(dlg.Effects);// add on any effects variables (and may add in some previous variables, since we did not purge)

                SetPeristentGlobal(ActionPlay.globalvarplayvolume, dlg.Volume);
                SetPeristentGlobal(ActionPlay.globalvarplayeffects, dlg.Effects.ToString());

                EDDConfig.Instance.DefaultWaveDevice = discoveryform.AudioQueueWave.Driver.GetAudioEndpoint();
            }
        }


        public void EditSpeechText()
        {
            if (Globals.Exists("SpeechDefinitionFile"))
            {
                string prog = Globals["SpeechDefinitionFile"];

                Tuple<ActionFile, ActionProgram> ap = actionfiles.FindProgram(prog);

                if (ap != null && ap.Item2.EditInEditor())
                {
                    ap.Item1.WriteFile();
                    return;
                }
            }

            ExtendedControls.MessageBoxTheme.Show(discoveryform, "Voice pack not loaded, or needs updating to support this functionality");
        }

        #region RUN!

        public void ActionRunOnRefresh()
        {
            string prevcommander = Globals.Exists("Commander") ? Globals["Commander"] : "None";
            string commander = (discoverycontroller.history.CommanderId < 0) ? "Hidden" : EDCommander.Current.Name;

            string refreshcount = prevcommander.Equals(commander) ? Globals.AddToVar("RefreshCount", 1, 1) : "1";
            SetInternalGlobal("RefreshCount", refreshcount);
            SetInternalGlobal("Commander", commander);

            if (actionfiles.IsConditionFlagSet(ConditionVariables.flagRunAtRefresh))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in discoverycontroller.history.EntryOrder)
                    ActionRunOnEntry(he, ActionEventEDList.RefreshJournal(he), ConditionVariables.flagRunAtRefresh);
            }

            ActionRun(ActionEventEDList.onRefreshEnd);
        }

        public int ActionRunOnEntry(HistoryEntry he, ActionEvent ev, string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            return ActionRun(ev, he, null, flagstart, now);
        }

        public override int ActionRun(ActionEvent ev, ConditionVariables additionalvars = null,
                                string flagstart = null, bool now = false)              // override base
        { return ActionRun(ev, null, additionalvars, flagstart, now); }

        public int ActionRun(ActionEvent ev, HistoryEntry he = null, ConditionVariables additionalvars = null,
                                string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            List<ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(ev.triggername, flagstart);      // look thru all actions, find matching ones

            if (ale.Count > 0)                  
            {
                ConditionVariables eventvars = new ConditionVariables();
                Actions.ActionVars.TriggerVars(eventvars, ev.triggername, ev.triggertype);
                Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
                Actions.ActionVars.ShipBasicInformation(eventvars, he?.ShipInformation, "Event");     // if He null, or si null, ignore
                Actions.ActionVars.SystemVars(eventvars, he?.System, "Event");
                eventvars.Add(additionalvars);   // adding null is allowed

                ConditionVariables testvars = new ConditionVariables(Globals);
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

        #endregion

        public void onStartup()
        {
            ActionRun(ActionEvent.onStartup);
            ActionRun(ActionEvent.onPostStartup);
        }

        public void CloseDown()
        {
            actionrunasync.WaitTillFinished(10000);
            SQLiteConnectionUser.PutSettingString("UserGlobalActionVars", PersistentVariables.ToString());
        }

        public override void LogLine(string s)
        {
            discoveryform.LogLine(s);
        }

        protected class ActionMessageFilter : IMessageFilter
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
                        string name = k.VKeyToString(Control.ModifierKeys);
                        //System.Diagnostics.Debug.WriteLine("Keydown " + m.LParam + " " + name + " " + m.WParam + " " + Control.ModifierKeys);
                        if (actcontroller.CheckKeys(name))
                            return true;    // swallow, we did it
                    }
                }

                return false;
            }
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
                    actionfilesmessagefilter = new ActionMessageFilter(discoveryform, this);
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
                SetInternalGlobal("KeyPress", keyname);
                ActionRun(ActionEventEDList.onKeyPress);
                return true;
            }
            else
                return false;
        }

        public override bool Pragma(string cmd)     // extra pragmas.
        {
            if (cmd.Equals("bindings"))
            {
                LogLine(DiscoveryForm.FrontierBindings.ListBindings());
            }
            else if (cmd.Equals("bindingvalues"))
            {
                LogLine(DiscoveryForm.FrontierBindings.ListValues());
            }
            else
                return false;

            return true;
        }

    }
}
