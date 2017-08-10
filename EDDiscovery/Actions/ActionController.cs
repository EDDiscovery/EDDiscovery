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

        static public ConditionFunctionHandlers DefaultGetCFH(ConditionFunctions c, ConditionVariables vars, ConditionFileHandles handles, int recdepth)
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
            ActionBase.AddCommand("Event", typeof(ActionEvent), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Historytab", typeof(ActionHistoryTab), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Ledger", typeof(ActionLedger), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Materials", typeof(ActionMaterials), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Perform", typeof(ActionPerform), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Play", typeof(ActionPlay), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Popout", typeof(ActionPopout), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("ProgramWindow", typeof(ActionProgramwindow), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Scan", typeof(ActionScan), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Ship", typeof(ActionShip), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Star", typeof(ActionStar), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Timer", typeof(ActionTimer), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("MenuItem", typeof(ActionMenuItem), ActionBase.ActionType.Cmd);

            ReLoad();
        }

        static public string AppFolder { get { return System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, "Actions"); } }

        public void ReLoad( bool completereload = true)        // COMPLETE reload..
        {
            if ( completereload )
                actionfiles = new ActionFileList();     // clear the list

            string errlist = actionfiles.LoadAllActionFiles(AppFolder);
            if (errlist.Length > 0)
                ExtendedControls.MessageBoxTheme.Show("Failed to load files\r\n" + errlist, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            actionrunasync = new ActionRun(this, actionfiles);        // this is the guy who runs programs asynchronously
            ActionConfigureKeys();
        }

        public void EditLastPack()
        {
            if (lasteditedpack.Length > 0 && EditActionFile(lasteditedpack))
                return;
            else
                ExtendedControls.MessageBoxTheme.Show("Action pack does not exist anymore or never set", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool EditActionFile(string name)
        {
            List<string> jevents = JournalEntry.GetListOfEventsWithOptMethod(towords: false);
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

            frm.onAdditionalNames += Frm_onAdditionalNames;
            ActionFile f = actionfiles.Get(name);

            if (f != null)
            {
                frm.Init("Edit pack " + name, this.Icon, this, AppFolder, f, eventnames);
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

        private List<string> Frm_onAdditionalNames(string evname)       // call back to discover name list.  evname may be empty
        {
            List<string> fieldnames = new List<string>(discoveryform.Globals.NameList);
            fieldnames.Sort();

            if ( evname != null && evname.Length>0 )
            { 
                List<string> classnames = BaseUtils.FieldNames.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(evname), "EventClass_");
                if (classnames != null)
                    fieldnames.InsertRange(0, classnames);
            }

            return fieldnames;
        }

        private void Dmf_OnEditActionFile(string name)
        {
            EditActionFile(name);
        }

        private void Dmf_OnEditGlobals()
        {
            ConditionVariablesForm avf = new ConditionVariablesForm();
            avf.Init("Global User variables to pass to program on run", this.Icon, PersistentVariables, showone: true);

            if (avf.ShowDialog(discoveryform.FindForm()) == DialogResult.OK)
            {
                LoadPeristentVariables(avf.result);
            }
        }

        private void Dmf_OnCreateActionFile()
        {
            String r = ExtendedControls.PromptSingleLine.ShowDialog(discoveryform.FindForm(), "New name", "", "Create new action file" , this.Icon);
            if ( r != null && r.Length>0 )
            {
                if (actionfiles.Get(r, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    actionfiles.CreateSet(r,AppFolder);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(discoveryform.FindForm(), "Duplicate name", "Create Action File Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    ActionRunOnEntry(he, "onRefresh", ConditionVariables.flagRunAtRefresh);
            }

            ActionRun("onRefreshEnd", "ProgramEvent");
        }

        public int ActionRunOnEntry(HistoryEntry he, string triggertype, string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            return ActionRun(he.journalEntry.EventTypeStr, triggertype, he, null, flagstart, now);
        }

        public override int ActionRun(string triggername, string triggertype, ConditionVariables additionalvars = null,
                                string flagstart = null, bool now = false)              // override base
        { return ActionRun(triggername, triggertype, null, additionalvars, flagstart, now); }

        public int ActionRun(string triggername, string triggertype, HistoryEntry he = null, ConditionVariables additionalvars = null,
                                string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            List<ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(triggername, flagstart);      // look thru all actions, find matching ones

            if (ale.Count > 0)                  
            {
                ConditionVariables eventvars = new ConditionVariables();
                Actions.ActionVars.TriggerVars(eventvars, triggername, triggertype);
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

        public void onStartup()
        {
            ActionRun("onStartup", "ProgramEvent");
            ActionRun("onPostStartup", "ProgramEvent");
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
                        //System.Diagnostics.Debug.WriteLine("Keydown " + m.LParam + " " + k.ToString(Control.ModifierKeys) + " " + m.WParam + " " + Control.ModifierKeys);
                        if (actcontroller.CheckKeys(k.ToString(Control.ModifierKeys)))
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
                ActionRun("onKeyPress", "KeyPress");
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
