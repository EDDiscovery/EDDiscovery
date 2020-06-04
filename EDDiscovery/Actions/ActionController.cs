/*
 * Copyright © 2017-2019 EDDiscovery development team
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
using AudioExtensions;
using BaseUtils;
using EDDiscovery.Forms;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public partial class ActionController : ActionCoreController
    {
        private EDDiscoveryForm discoveryform;
        private EDDiscoveryController discoverycontroller;

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
        public BindingsFile FrontierBindings { get { return frontierbindings; } }

        public string ErrorList;        // set on Reload, use to display warnings at right point

        AudioExtensions.IAudioDriver audiodriverwave;
        AudioExtensions.AudioQueue audioqueuewave;
        AudioExtensions.IAudioDriver audiodriverspeech;
        AudioExtensions.AudioQueue audioqueuespeech;
        AudioExtensions.SpeechSynthesizer speechsynth;

        DirectInputDevices.InputDeviceList inputdevices;
        Actions.ActionsFromInputDevices inputdevicesactions;
        BindingsFile frontierbindings;

        Type[] keyignoredforms;

        public ActionController(EDDiscoveryForm frm, EDDiscoveryController ctrl, System.Drawing.Icon ic, Type[] keyignoredforms = null) : base(frm, ic)
        {
            discoveryform = frm;
            discoverycontroller = ctrl;
            this.keyignoredforms = keyignoredforms;

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

            LoadPeristentVariables(new Variables(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("UserGlobalActionVars", ""), Variables.FromMode.MultiEntryComma));

            lasteditedpack = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ActionPackLastFile", "");

            ActionBase.AddCommand("Bookmarks", typeof(ActionBookmarks), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Captainslog", typeof(ActionCaptainsLog), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Commanders", typeof(ActionCommanders), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Commodities", typeof(ActionCommodities), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("DLLCall", typeof(ActionDLLCall), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("EliteBindings", typeof(ActionEliteBindings), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Event", typeof(ActionEventCmd), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("GMO", typeof(ActionGMO), ActionBase.ActionType.Cmd);
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
            ActionBase.AddCommand("Target", typeof(ActionTarget), ActionBase.ActionType.Cmd);
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
                string collapsestate = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ActionEditorCollapseState_" + name, "");  // get any collapsed state info for this pack

                frm.Init("Edit pack " + name, this.Icon, this, EDDOptions.Instance.ActionsAppDirectory(), f, ActionEventEDList.EventList(excludejournaluitranslatedevents:true), collapsestate);

                frm.ShowDialog(discoveryform); // don't care about the result, the form does all the saving

                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ActionEditorCollapseState_" + name, frm.CollapsedState());  // get any collapsed state info for this pack

                ActionConfigureKeys();  // kick it to load in case its changed
                VoiceLoadEvents();      

                lasteditedpack = name;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ActionPackLastFile", lasteditedpack);
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

        private List<TypeHelpers.PropertyNameInfo> Frm_onAdditionalNames(string evname)       // call back to discover name list.  evname may be empty
        {
            System.Diagnostics.Debug.WriteLine("Get variables for " + evname);

            List<TypeHelpers.PropertyNameInfo> fieldnames =
                (from x in discoveryform.Globals.NameList select new BaseUtils.TypeHelpers.PropertyNameInfo(x, "Global Variable String or Number" + Environment.NewLine + "Not part of an event, set up by either EDD or one of the action packs", null, "Global")).ToList();

            if (evname.HasChars())
            {
                fieldnames.Add(new TypeHelpers.PropertyNameInfo("TriggerName", "Name of event, either the JournalEntryName, or UI<event>", ConditionEntry.MatchType.Equals, "Event Trigger Info"));
                fieldnames.Add(new TypeHelpers.PropertyNameInfo("TriggerType", "Type of trigger, either UIEvent or JournalEvent", ConditionEntry.MatchType.Equals, "Event Trigger Info"));

                var events = ActionEventEDList.StaticDefinedEvents();

                ActionEvent ty = events.Find(x => x.TriggerName == evname);
                if ( ty?.Variables != null )                                           // if its a static event and it has variables, add..
                    fieldnames.AddRange(ty.Variables);

                // first see if its a journal event..
                List<TypeHelpers.PropertyNameInfo> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(evname), "EventClass_", comment: "Event Variable");
                // then if its an UI
                if (classnames == null)
                    classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(UIEvent.TypeOfUIEvent(evname), "EventClass_", comment: "Event Variable");    

                if (classnames != null)
                    fieldnames.AddRange(classnames);
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
            avf.Init("Global User variables to pass to program on run", this.Icon, PersistentVariables, showatleastoneentry: true);

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

            if (actionfiles.IsActionVarDefined("RunAtRefresh"))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in discoverycontroller.history.EntryOrder)
                    ActionRunOnEntry(he, ActionEventEDList.RefreshJournal(he), "RunAtRefresh");
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
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            actionrunasync.WaitTillFinished(10000, true);
        }

        public void CloseDown()
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("UserGlobalActionVars", PersistentVariables.ToString());

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


        #region Elite Input 

        public void EliteInput(bool on, bool axisevents)
        {
            inputdevicesactions.Stop();
            inputdevices.Clear();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && on)
            {
                DirectInputDevices.InputDeviceJoystickWindows.CreateJoysticks(inputdevices, axisevents);
                DirectInputDevices.InputDeviceKeyboard.CreateKeyboard(inputdevices);              // Created.. not started..
                DirectInputDevices.InputDeviceMouse.CreateMouse(inputdevices);
                inputdevicesactions.Start();
            }
        }

        public string EliteInputList() { return inputdevices.ListDevices(); }
        public string EliteInputCheck() { return inputdevicesactions.CheckBindings(); }

        #endregion

    }
}
