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

        public ActionFile Get(string name, StringComparison c = StringComparison.InvariantCulture) { return actionfiles.Get(name, c); }     // get or return null
        public ActionFile[] Get(string[] name, StringComparison c = StringComparison.InvariantCulture) { return actionfiles.Get(name, c); }
        public ActionFile[] Get(string[] name, bool enabledstate, StringComparison c = StringComparison.InvariantCulture) { return actionfiles.Get(name, enabledstate, c); }

        private string lasteditedpack;

        static public FunctionHandlers DefaultGetCFH(Functions c, Variables vars, FunctionPersistentData handles, int recdepth)
        {
            return new ConditionEDDFunctions(c, vars, handles, recdepth);
        }

        public override AudioExtensions.AudioQueue AudioQueueWave { get { return audioqueuewave; } }
        public override AudioExtensions.AudioQueue AudioQueueSpeech { get { return audioqueuespeech; } }
        public override AudioExtensions.SpeechSynthesizer SpeechSynthesizer { get { return speechsynth; } }
        public AudioExtensions.IVoiceRecognition VoiceRecognition { get { return voicerecon; } }
        public BindingsFile FrontierBindings { get { return frontierbindings; } }

        public string ErrorList;        // set on Reload, use to display warnings at right point

        AudioExtensions.IAudioDriver audiodriverwave;
        AudioExtensions.AudioQueue audioqueuewave;
        AudioExtensions.IAudioDriver audiodriverspeech;
        AudioExtensions.AudioQueue audioqueuespeech;
        AudioExtensions.SpeechSynthesizer speechsynth;
        AudioExtensions.IVoiceRecognition voicerecon;

        DirectInputDevices.InputDeviceList inputdevices;
        Actions.ActionsFromInputDevices inputdevicesactions;
        BindingsFile frontierbindings;

        public ActionController(EDDiscoveryForm frm, EDDiscoveryController ctrl, System.Drawing.Icon ic) : base(frm, ic)
        {
            discoveryform = frm;
            discoverycontroller = ctrl;

            #if !NO_SYSTEM_SPEECH
            // Windows TTS (2000 and above). Speech *recognition* will be Version.Major >= 6 (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5 && !EDDOptions.Instance.NoSound)
            {
                audiodriverwave = AudioHelper.GetAudioDriver(ctrl.LogLineHighlight, EDDConfig.Instance.DefaultWaveDevice);
                audiodriverspeech = AudioHelper.GetAudioDriver(ctrl.LogLineHighlight, EDDConfig.Instance.DefaultVoiceDevice);
                ISpeechEngine speechengine;

                speechengine = AudioHelper.GetSpeechEngine(ctrl.LogLineHighlight);
                speechsynth = new AudioExtensions.SpeechSynthesizer(speechengine);
                voicerecon = AudioHelper.GetVoiceRecognition(ctrl.LogLineHighlight);
            }
            else
            {
                audiodriverwave = new AudioExtensions.AudioDriverDummy();
                audiodriverspeech = new AudioExtensions.AudioDriverDummy();
                speechsynth = new AudioExtensions.SpeechSynthesizer(new AudioExtensions.DummySpeechEngine());
                voicerecon = new AudioExtensions.VoiceRecognitionDummy();
            }
#else
            audiodriverwave = new AudioExtensions.AudioDriverDummy();
            audiodriverspeech = new AudioExtensions.AudioDriverDummy();
            speechsynth = new AudioExtensions.SpeechSynthesizer(new AudioExtensions.DummySpeechEngine());
            voicerecon = new AudioExtensions.VoiceRecognitionDummy();
#endif
            audioqueuewave = new AudioExtensions.AudioQueue(audiodriverwave);
            audioqueuespeech = new AudioExtensions.AudioQueue(audiodriverspeech);

            frontierbindings = new BindingsFile();
            inputdevices = new DirectInputDevices.InputDeviceList(a => discoveryform.BeginInvoke(a));
            inputdevicesactions = new Actions.ActionsFromInputDevices(inputdevices, frontierbindings, this);

            frontierbindings.LoadBindingsFile();
            //System.Diagnostics.Debug.WriteLine("Bindings" + frontierbindings.ListBindings());
            //System.Diagnostics.Debug.WriteLine("Key Names" + frontierbindings.ListKeyNames("{","}"));

            voicerecon.SpeechRecognised += Voicerecon_SpeechRecognised;
            voicerecon.SpeechNotRecognised += Voicerecon_SpeechNotRecognised;

            Functions.GetCFH = DefaultGetCFH;

            LoadPeristentVariables(new Variables(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), Variables.FromMode.MultiEntryComma));

            lasteditedpack = SQLiteConnectionUser.GetSettingString("ActionPackLastFile", "");

            ActionBase.AddCommand("Bookmarks", typeof(ActionBookmarks), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Commodities", typeof(ActionCommodities), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("DLLCall", typeof(ActionDLLCall), ActionBase.ActionType.Cmd);
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
        }

        public void ReLoad(bool completereload = true)        // COMPLETE reload..
        {
            if (completereload)
                actionfiles = new ActionFileList();     // clear the list

            ErrorList = actionfiles.LoadAllActionFiles(EDDOptions.Instance.ActionsAppDirectory());

            AdditionalChecks(ref ErrorList);

            actionrunasync = new ActionRun(this, actionfiles);        // this is the guy who runs programs asynchronously
        }

        public void CheckWarn()
        {
            if (ErrorList.Length > 0)
                ExtendedControls.MessageBoxTheme.Show(discoveryform, "Failed to load files\r\n" + ErrorList, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AdditionalChecks(ref string errlist)        // perform additional checks which can only be done when
        {                                                       // the full system is available at this level of code heirarchy
            foreach (ActionFile af in actionfiles.Enumerable)
            {
                foreach (ActionProgram p in af.actionprogramlist.Enumerable)
                {
                    foreach (ActionBase b in p.Enumerable)
                    {
                        if (b.Name == "Key")
                        {
                            string err = ActionKeyED.VerifyBinding(b.UserData, frontierbindings);
                            if (err.Length > 0)
                            {   // just a warning.. not a full error, as we don't want to stop someone using the pack due to a missing key mapping
                                LogLine("Key Mapping error: " + string.Format( "{0}:{1}:{2} {3}", af.name, p.Name, b.LineNumber, err));
                            }
                        }
                    }
                }
            }
        }

        #region Edit Action Packs

        public void EditLastPack()
        {
            if (lasteditedpack.Length > 0 && EditPack(lasteditedpack))
                return;
            else
                ExtendedControls.MessageBoxTheme.Show(discoveryform, "Action pack does not exist anymore or never set", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public bool EditPack(string name)            // edit pack name
        {
            ActionPackEditPackForm frm = new ActionPackEditPackForm();

            frm.AdditionalNames += Frm_onAdditionalNames;
            frm.CreateActionPackEdit += SetPackEditorAndCondition;
            ActionFile f = actionfiles.Get(name);

            if (f != null)
            {
                string collapsestate = SQLiteConnectionUser.GetSettingString("ActionEditorCollapseState_" + name, "");  // get any collapsed state info for this pack

                frm.Init("Edit pack " + name, this.Icon, this, EDDOptions.Instance.ActionsAppDirectory(), f, ActionEventEDList.EventList(), collapsestate);

                frm.ShowDialog(discoveryform); // don't care about the result, the form does all the saving

                SQLiteConnectionUser.PutSettingString("ActionEditorCollapseState_" + name, frm.CollapsedState());  // get any collapsed state info for this pack

                ActionConfigureKeys();  // kick it to load in case its changed
                VoiceLoadEvents();      

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
            if (group == "Voice")
            {
                ActionPackEditVoice ev = new ActionPackEditVoice();
                ev.Height = 28;
                ev.onEditKeys = onEditKeys;
                ev.onEditSay = onEditSay;

                // make sure the voice condition is right.. if not, reset.

                if (cd.eventname != Actions.ActionEventEDList.onVoiceInput.TriggerName || 
                        ( !cd.Is("VoiceInput", ConditionEntry.MatchType.MatchSemicolonList) && !cd.Is("VoiceInput", ConditionEntry.MatchType.MatchSemicolon)))
                {
                    cd.eventname = Actions.ActionEventEDList.onVoiceInput.TriggerName;
                    cd.Set(new ConditionEntry("VoiceInput", ConditionEntry.MatchType.MatchSemicolonList, "?"));     // Voiceinput being the variable set to the expression
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

        private string onEditKeys(Form p, System.Drawing.Icon i, string keys)      // called when program wants to edit Key
        {
            return ActionKeyED.Menu(p, i, keys, frontierbindings);
        }

        private string onEditSay(Form p, string say, ActionCoreController cp)      // called when program wants to edit Key
        {
            return ActionSay.Menu(p, say, cp);
        }


        // called when a event name is selected, what is the initial condition set up?
        private ActionProgram.ProgramConditionClass SetEventCondition(Condition cd)
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

            if (evname.HasChars())
            {
                List<string> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(evname), "EventClass_");
                if (classnames != null)
                    fieldnames.InsertRange(0, classnames);
            }

            return fieldnames;
        }

        private void Dmf_OnEditActionFile(string name)      // wanna edit
        {
            EditPack(name);
        }

        private void Dmf_OnEditGlobals()                    // edit the globals
        {
            ExtendedConditionsForms.VariablesForm avf = new ExtendedConditionsForms.VariablesForm();
            avf.Init("Global User variables to pass to program on run", this.Icon, PersistentVariables, showone: true);

            if (avf.ShowDialog(discoveryform) == DialogResult.OK)
            {
                LoadPeristentVariables(avf.result);
            }
        }

        private void Dmf_OnCreateActionFile()
        {
            String r = ExtendedControls.PromptSingleLine.ShowDialog(discoveryform, "New name", "", "Create new action file", this.Icon);
            if (r != null && r.Length > 0)
            {
                if (actionfiles.Get(r, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    actionfiles.CreateSet(r, EDDOptions.Instance.ActionsAppDirectory());
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(discoveryform, "Duplicate name", "Create Action File Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region Manage them

        public bool ManageAddOns()
        {
            return ManagerAddOns(true);
        }

        public bool EditAddOns()
        {
            return ManagerAddOns(false);
        }

        private bool ManagerAddOns(bool manage)
        {
            bool changed = false;

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
                    AudioQueueSpeech.StopAll();
                    AudioQueueWave.StopAll();

                    ReLoad(false);      // reload from disk, new ones if required, refresh old ones and keep the vars
                    CheckWarn();

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

                    ActionRun(ActionEventEDList.onInstall, null, new Variables("InstallList", changes));

                    ActionConfigureKeys();

                    changed = true;
                }
            }

            return changed;
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
            string volume = Globals.GetString(ActionSay.globalvarspeechvolume, "Default");
            string rate = Globals.GetString(ActionSay.globalvarspeechrate, "Default");
            Variables effects = new Variables(PersistentVariables.GetString(ActionSay.globalvarspeecheffects, ""), Variables.FromMode.MultiEntryComma);

            ExtendedAudioForms.SpeechConfigure cfg = new ExtendedAudioForms.SpeechConfigure();
            cfg.Init(AudioQueueSpeech, SpeechSynthesizer,
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

                EDDConfig.Instance.DefaultVoiceDevice = AudioQueueSpeech.Driver.GetAudioEndpoint();
            }
        }

        public void ConfigureWave(string title)
        {
            string volume = Globals.GetString(ActionPlay.globalvarplayvolume, "60");
            Variables effects = new Variables(PersistentVariables.GetString(ActionPlay.globalvarplayeffects, ""), Variables.FromMode.MultiEntryComma);

            ExtendedAudioForms.WaveConfigureDialog dlg = new ExtendedAudioForms.WaveConfigureDialog();
            dlg.Init(AudioQueueWave, true,
                        "Select Default device, volume and effects", title, this.Icon,
                        "",
                        false, AudioExtensions.AudioQueue.Priority.Normal, "", "",
                        volume, effects);

            if (dlg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                Variables cond = new Variables(dlg.Effects);// add on any effects variables (and may add in some previous variables, since we did not purge)

                SetPeristentGlobal(ActionPlay.globalvarplayvolume, dlg.Volume);
                SetPeristentGlobal(ActionPlay.globalvarplayeffects, dlg.Effects.ToString());

                EDDConfig.Instance.DefaultWaveDevice = AudioQueueWave.Driver.GetAudioEndpoint();
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

            if (actionfiles.IsConditionFlagSet(Variables.flagRunAtRefresh))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in discoverycontroller.history.EntryOrder)
                    ActionRunOnEntry(he, ActionEventEDList.RefreshJournal(he), Variables.flagRunAtRefresh);
            }

            ActionRun(ActionEventEDList.onRefreshEnd);
        }

        public int ActionRunOnEntry(HistoryEntry he, ActionEvent ev, string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            return ActionRun(ev, he, null, flagstart, now);
        }

        public override int ActionRun(ActionEvent ev, Variables additionalvars = null,
                                string flagstart = null, bool now = false)              // override base
        { return ActionRun(ev, null, additionalvars, flagstart, now); }

        public int ActionRun(ActionEvent ev, HistoryEntry he = null, Variables additionalvars = null,
                                string flagstart = null, bool now = false)       //set flagstart to be the first flag of the actiondata..
        {
            List<ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(ev.TriggerName, flagstart);      // look thru all actions, find matching ones

            if (ale.Count > 0)
            {
                Variables eventvars = new Variables();
                Actions.ActionVars.TriggerVars(eventvars, ev.TriggerName, ev.TriggerType);
                Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
                Actions.ActionVars.ShipBasicInformation(eventvars, he?.ShipInformation, "Event");     // if He null, or si null, ignore
                Actions.ActionVars.SystemVars(eventvars, he?.System, "Event");
                eventvars.Add(additionalvars);   // adding null is allowed

                //System.Diagnostics.Debug.WriteLine("Dispatch Program with" + Environment.NewLine + eventvars.ToString(prefix:".. ", separ: Environment.NewLine));

                Variables testvars = new Variables(Globals);
                testvars.Add(eventvars);

                Functions functions = new Functions(testvars, null);

                if (actionfiles.CheckActions(ale, he?.journalEntry, testvars, functions) > 0)
                {
                    actionfiles.RunActions(now, ale, actionrunasync, eventvars);  // add programs to action run

                    actionrunasync.Execute();       // See if needs executing
                }
            }

            return ale.Count;
        }

        public bool ActionRunProgram(string packname, string programname, Variables runvars, bool now = false)
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

        public void onStartup()     // on main thread
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            ActionRun(ActionEvent.onStartup);
            ActionRun(ActionEvent.onPostStartup);
            ActionConfigureKeys();          // reload keys
        }

        public void HoldTillProgStops()
        {
            actionrunasync.WaitTillFinished(10000);
        }

        public void CloseDown()
        {
            SQLiteConnectionUser.PutSettingString("UserGlobalActionVars", PersistentVariables.ToString());

            audioqueuespeech.StopAll();
            audioqueuewave.StopAll();
            audioqueuespeech.Dispose();     // in order..
            audiodriverspeech.Dispose();
            audioqueuewave.Dispose();
            audiodriverwave.Dispose();

            inputdevicesactions.Stop();
            inputdevices.Clear();

            voicerecon.Close();

            System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " AC Closedown complete");
        }

        public override void LogLine(string s)
        {
            discoveryform.LogLine(s);
        }

        #region Keys

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
                ActionRun(ActionEventEDList.onKeyPress, new Variables("KeyPress", keyname));
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Misc overrides

        public override bool Pragma(string s)     // extra pragmas.
        {
            if (s.Equals("bindings"))
            {
                LogLine(frontierbindings.ListBindings());
            }
            else if (s.Equals("bindingvalues"))
            {
                LogLine(frontierbindings.ListValues());
            }
            else
                return false;

            return true;
        }

        #endregion

        #region Voice

        public bool VoiceReconOn(string culture = null)     // perform enableVR
        {
            voicerecon.Close(); // can close without stopping
            voicerecon.Open(System.Globalization.CultureInfo.GetCultureInfo(culture));
            return voicerecon.IsOpen;
        }

        public void VoiceReconOff()                         // perform disableVR
        {
            voicerecon.Close();
        }

        public void VoiceReconConfidence(float conf)
        {
            voicerecon.Confidence = conf;
        }

        public void VoiceReconParameters(int babble, int initialsilence, int endsilence, int endsilenceambigious)
        {
            if (voicerecon.IsOpen)
            {
                voicerecon.Stop(true);
                try
                {
                    voicerecon.BabbleTimeout = babble;
                    voicerecon.InitialSilenceTimeout = initialsilence;
                    voicerecon.EndSilenceTimeout = endsilence;
                    voicerecon.EndSilenceTimeoutAmbigious = endsilenceambigious;
                }
                catch { };

                voicerecon.Start();
            }
        }

        public void VoiceLoadEvents()       // kicked by Action.Perform so synchornised with voice pack (or via editor)
        {
            if ( voicerecon.IsOpen )
            {
                voicerecon.Stop(true);

                voicerecon.Clear(); // clear grammars

                List<Tuple<string, ConditionEntry.MatchType>> ret = actionfiles.ReturnValuesOfSpecificConditions("VoiceInput", new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.MatchSemicolonList, ConditionEntry.MatchType.MatchSemicolon });        // need these to decide

                if (ret.Count > 0)
                {
                    foreach (var vp in ret)
                    {
                        voicerecon.Add(vp.Item1);
                    }

                    voicerecon.Start();
                }
            }
        }

        public string VoicePhrases(string sep)
        {
            List<Tuple<string, ConditionEntry.MatchType>> ret = actionfiles.ReturnValuesOfSpecificConditions("VoiceInput", new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.MatchSemicolonList, ConditionEntry.MatchType.MatchSemicolon });        // need these to decide

            string s = "";
            foreach (var vp in ret)
            {
                BaseUtils.StringCombinations sb = new BaseUtils.StringCombinations();
                sb.ParseString(vp.Item1);
                s += String.Join(",", sb.Permutations.ToArray()) + sep;
            }

            return s;
        }

        private void Voicerecon_SpeechRecognised(string text, float confidence)
        {
            System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Recognised " + text + " " + confidence.ToStringInvariant("0.0"));
            ActionRun(ActionEventEDList.onVoiceInput, new Variables(new string[] { "VoiceInput", text, "VoiceConfidence", (confidence*100F).ToStringInvariant("0.00") }));
        }

        private void Voicerecon_SpeechNotRecognised(string text, float confidence)
        {
            System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Failed recognition " + text + " " + confidence.ToStringInvariant("0.00"));
            ActionRun(ActionEventEDList.onVoiceInputFailed, new Variables(new string[] { "VoiceInput", text, "VoiceConfidence", (confidence*100F).ToStringInvariant("0.00") }));
        }

        #endregion

        #region Elite Input 

        public void EliteInput(bool on, bool axisevents)
        {
            inputdevicesactions.Stop();
            inputdevices.Clear();

#if !__MonoCS__
            if (on)
            {
                DirectInputDevices.InputDeviceJoystickWindows.CreateJoysticks(inputdevices, axisevents);
                DirectInputDevices.InputDeviceKeyboard.CreateKeyboard(inputdevices);              // Created.. not started..
                DirectInputDevices.InputDeviceMouse.CreateMouse(inputdevices);
                inputdevicesactions.Start();
            }
#endif
        }

        public string EliteInputList() { return inputdevices.ListDevices(); }
        public string EliteInputCheck() { return inputdevicesactions.CheckBindings(); }

        #endregion

    }
}
