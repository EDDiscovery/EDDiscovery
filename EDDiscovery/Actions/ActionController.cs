/*
 * Copyright 2017-2023 EDDiscovery development team
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
using EliteDangerousCore;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public partial class ActionController : ActionCoreController
    {
        public HistoryList HistoryList { get { return DiscoveryForm.History; } }
        public EDDiscoveryForm DiscoveryForm { get; }

        public ActionFile Get(string name, StringComparison c = StringComparison.InvariantCultureIgnoreCase) { return actionfiles.Get(name, c); }     // get or return null
        public ActionFile[] Get(string[] name, StringComparison c = StringComparison.InvariantCultureIgnoreCase) { return actionfiles.Get(name, c); }
        public ActionFile[] Get(string[] name, bool enabledstate, StringComparison c = StringComparison.InvariantCultureIgnoreCase) { return actionfiles.Get(name, enabledstate, c); }

        public override AudioExtensions.AudioQueue AudioQueueWave { get;  }
        public override AudioExtensions.AudioQueue AudioQueueSpeech { get;  }
        public override AudioExtensions.SpeechSynthesizer SpeechSynthesizer { get; }
        public AudioExtensions.IVoiceRecognition VoiceRecon { get; }
        public BindingsFile FrontierBindings { get; }

        public ActionController(
                                EDDiscoveryForm eddiscoveryform,        // need this for access to data
                                Form uiform,        // what UI form is the parent
                                string actfolder,       // where the action files are located, for reload
                                string approotfolder,  // null if don't allow management, else root for Manage/Edit Addons
                                string otherinstalledfilesfolder, // null if don't do it
                                string globalvars,      // null if none, else load these global vars
                                AudioExtensions.AudioQueue wave, AudioExtensions.AudioQueue speech, AudioExtensions.SpeechSynthesizer synth, 
                                BindingsFile frontierbindings, 
                                bool nosound,
                                Action<string> logger,
                                System.Drawing.Icon ic, Type[] keyignoredforms = null) : base(uiform, ic)
        {
            this.actfolder = actfolder;
            this.approotfolder = approotfolder;
            this.otherinstalledfilesfolder = otherinstalledfilesfolder;
            this.DiscoveryForm = eddiscoveryform;
            this.keyignoredforms = keyignoredforms;
            this.logLineOut = logger;

            AudioQueueWave = wave;
            AudioQueueSpeech = speech;
            SpeechSynthesizer = synth;
            FrontierBindings = frontierbindings;

            inputdevices = new DirectInputDevices.InputDeviceList(a => DiscoveryForm.BeginInvoke(a));
            inputdevicesactions = new Actions.ActionsFromInputDevices(inputdevices, frontierbindings, this);

            // we own the voice recon
#if !NO_SYSTEM_SPEECH
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5 && !nosound)
                VoiceRecon = AudioHelper.GetVoiceRecognition(eddiscoveryform.LogLineHighlight);
            else
#endif
                VoiceRecon = new AudioExtensions.VoiceRecognitionDummy();

            VoiceRecon.SpeechRecognised += Voicerecon_SpeechRecognised;
            VoiceRecon.SpeechNotRecognised += Voicerecon_SpeechNotRecognised;

            if ( globalvars != null)
                LoadPeristentVariables(new Variables(globalvars, Variables.FromMode.MultiEntryComma));

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
            actionfiles?.CloseDown();

            if (completereload)
                actionfiles = new ActionFileList();     // clear the list

            errorlist = actionfiles.LoadAllActionFiles(actfolder);

            AdditionalChecks(ref errorlist);

            actionrun = new ActionRun(this, actionfiles);        // this is the guy who runs the programs (default async)
        }
        
        // Call this to add any PanelConfig panels defined by active action files to the panel IDs list
        public void CreatePanels()
        {
            foreach( ActionFile af in actionfiles.Enumerable)
            {
                if (af.Enabled && af.InstallationVariables.TryGet("PlugInPanel", out string cf))
                {
                    StringParser sp = new StringParser(cf);

                    string id = sp.NextQuotedWordComma();
                    string type = sp.NextQuotedWordComma();
                    string wintitle = sp.NextQuotedWordComma();
                    string refname = sp.NextQuotedWordComma();
                    string description = sp.NextQuotedWordComma();
                    string iconname = sp.NextQuotedWordComma();
                    string path = sp.NextQuotedWord();
                    string iconpath = null;
                    if (path.HasChars())
                    {
                        path = System.IO.Path.Combine(EDDOptions.Instance.AppDataDirectory, path);
                        iconpath = System.IO.Path.Combine(path, iconname);
                    }

                    Type ptype = type.HasChars() ? Type.GetType(type) : null;

                    if (ptype != null && id.HasChars() && wintitle.HasChars() && refname.HasChars() && description.HasChars() && path.HasChars() && System.IO.File.Exists(iconpath))
                    {
                        var image = System.Drawing.Image.FromFile(iconpath);

                        // registered panels, search the stored list, see if there, then it gets the index, else its added to the list
                        int panelid = EDDConfig.Instance.FindCreatePanelID(id);

                        DiscoveryForm.AddPanel(panelid,
                                            ptype,       // driver panel containing the UC to draw into, responsible for running action scripts/talking to the plugin
                                            path,
                                            wintitle, refname, description, image);
                    }
                    else
                        System.Diagnostics.Trace.WriteLine($"Panel {id} did not install due to missing items");
                }
            }
        }

        // is this panel a plug in panel to us, -1 if not, else panel id. Must have been registered by CreatePanels()
        public int GetPanelID(Variables vars)
        {
            if (vars.TryGet("PlugInPanel", out string cf))
            {
                StringParser sp = new StringParser(cf);
                string id = sp.NextQuotedWordComma();
                if (id.HasChars())
                    return EDDConfig.Instance.FindCreatePanelID(id, false);
            }
            return -1;
        }

        public void CheckWarn()
        {
            if (errorlist.Length > 0)
                ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Failed to load files\r\n" + errorlist, "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AdditionalChecks(ref string errlist)        // perform additional checks which can only be done when
        {                                                       // the full system is available at this level of code heirarchy
            foreach (ActionFile af in actionfiles.Enumerable)
            {
                if (af.Enabled)
                {
                    foreach (ActionProgram p in af.ProgramList.Enumerable)
                    {
                        foreach (ActionBase b in p.Enumerable)
                        {
                            if (b.Name == "Key")
                            {
                                string err = ActionKeyED.VerifyBinding(b.UserData, FrontierBindings);
                                if (err.Length > 0)
                                {   // just a warning.. not a full error, as we don't want to stop someone using the pack due to a missing key mapping
                                    LogLine("Key Mapping Warning: " + string.Format("{0}:{1}:{2} {3}", af.Name, p.Name, b.LineNumber, err));
                                }
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
                ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Action pack does not exist anymore or never set", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public bool EditPack(string name)            // edit pack name
        {
            var frm = new ActionPackEditPackForm();

            frm.AdditionalNames += Frm_onAdditionalNames;
            frm.GetEventEditor += GetEventEditorForCondition;
            frm.GetClassNameFromCondition += GetClassNameFromCondition;
            ActionFile f = actionfiles.Get(name);

            if (f != null)
            {
                string collapsestate = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ActionEditorCollapseState_" + name, "");  // get any collapsed state info for this pack

                frm.Init("Edit pack " + name, this.Icon, this, actfolder, f, ActionEventEDList.EventList(excludejournaluitranslatedevents:true), collapsestate);

                frm.ShowDialog(DiscoveryForm); // don't care about the result, the form does all the saving

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

        // we want a class name from the condition.. so we can work out which condition editor to use.
        private string GetClassNameFromCondition(Condition cd)
        {
            // special, since it uses OnEliteInputRaw

            if (cd.EventName == ActionEventEDList.onInputToKey.TriggerName && cd.Fields.Count == 3 && cd.Fields[0].ItemName == "Device" && cd.Fields[1].ItemName == "EventName" && cd.Fields[2].ItemName == "Pressed")
            {
                return ActionEventEDList.onInputToKey.UIClass;
            }
            else
            {
                var events = ActionEventEDList.EventList(excludejournaluitranslatedevents: true);
                ActionEvent ev = events.Find(x => x.TriggerName == cd.EventName);
                return (ev == null) ? "Misc" : ev.UIClass;
            }
        }


        // called when a new entry is set up, what editor do you want?  and you can alter the condition
        // for new entries, cd = AlwaysTrue. For older entries, the condition
        private ActionPackEditEventBase GetEventEditorForCondition(string classtype, Condition cd)
        {
            if (classtype == "Voice")
            {
                ActionPackEditEventVoice ev = new ActionPackEditEventVoice();
                ev.Height = 28;
                ev.onEditKeys = onEditKeys;
                ev.onEditSay = onEditSay;

                // make sure the voice condition is right.. if not, reset.

                if (cd.EventName != Actions.ActionEventEDList.onVoiceInput.TriggerName ||
                        (!cd.Is("VoiceInput", ConditionEntry.MatchType.MatchSemicolonList) && !cd.Is("VoiceInput", ConditionEntry.MatchType.MatchSemicolon)))
                {
                    cd.EventName = Actions.ActionEventEDList.onVoiceInput.TriggerName;
                    cd.Set(new ConditionEntry("VoiceInput", ConditionEntry.MatchType.MatchSemicolonList, "?"));     // Voiceinput being the variable set to the expression
                }

                return ev;
            }
            else if (classtype == "InputToKey")
            {
                ActionPackEditEventInputToKey ev = new ActionPackEditEventInputToKey();
                ev.Height = 28;
                ev.onEditKeys = onEditKeys;
                ev.onEditSay = onEditSay;
                ev.onInputButton = () =>
                {
                    DirectInputDevices.InputMapDialog mp = new DirectInputDevices.InputMapDialog();
                    ExtendedControls.Theme.Current.ApplyDialog(mp);
                    if ( mp.ShowDialog(DiscoveryForm) == DialogResult.OK )
                    {
                        return new string[] { mp.DeviceName, mp.ButtonName, mp.Press.ToStringIntValue() };
                    }
                    else
                        return null;
                };

                // make sure the voice condition is right.. if not, reset.

                if (cd.EventName != Actions.ActionEventEDList.onInputToKey.TriggerName || cd.Fields.Count != 3 || cd.Fields[0].ItemName != "Device" || cd.Fields[1].ItemName != "EventName" || cd.Fields[2].ItemName != "Pressed")
                {
                    cd.EventName = Actions.ActionEventEDList.onEliteInputRaw.TriggerName;
                    cd.InnerCondition = Condition.LogicalCondition.And;
                    cd.Set(new ConditionEntry("Device", ConditionEntry.MatchType.Equals, "?"));
                    cd.Add(new ConditionEntry("EventName", ConditionEntry.MatchType.Equals, "?"));
                    cd.Add(new ConditionEntry("Pressed", ConditionEntry.MatchType.NumericEquals, "1"));
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
            return ActionKeyED.Menu(p, i, keys, FrontierBindings);
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
                if (cd.EventName == "onKeyPress")
                    cd.Set(new ConditionEntry("KeyPress", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onTimer")
                    cd.Set(new ConditionEntry("TimerName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onPopUp" || cd.EventName == "onPopDown")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onTabChange")
                    cd.Set(new ConditionEntry("TabName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onPanelChange")
                    cd.Set(new ConditionEntry("PopOutName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onEliteInput" || cd.EventName == "onEliteInputOff")
                    cd.Set(new ConditionEntry("Binding", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onMenuItem")
                    cd.Set(new ConditionEntry("MenuName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onSayStarted" || cd.EventName == "onSayFinished" || cd.EventName == "onPlayStarted" || cd.EventName == "onPlayFinished")
                    cd.Set(new ConditionEntry("EventName", ConditionEntry.MatchType.Equals, "?"));
                else if (cd.EventName == "onVoiceInput")
                    cls = ActionProgram.ProgramConditionClass.KeySay;
            }

            return cls;
        }

        private List<TypeHelpers.PropertyNameInfo> Frm_onAdditionalNames(string evname)       // call back to discover name list.  evname may be empty
        {
            System.Diagnostics.Debug.WriteLine("Get variables for " + evname);

            List<TypeHelpers.PropertyNameInfo> fieldnames =
                (from x in DiscoveryForm.Globals.NameList select new BaseUtils.TypeHelpers.PropertyNameInfo(x, "Global Variable String or Number" + Environment.NewLine + "Not part of an event, set up by either EDD or one of the action packs", null, "Global")).ToList();

            if (evname.HasChars())
            {
                fieldnames.Add(new TypeHelpers.PropertyNameInfo("TriggerName", "Name of event, either the JournalEntryName, or UI<event>", ConditionEntry.MatchType.Equals, "Event Trigger Info"));
                fieldnames.Add(new TypeHelpers.PropertyNameInfo("TriggerType", "Type of trigger, either UIEvent or JournalEvent", ConditionEntry.MatchType.Equals, "Event Trigger Info"));

                var events = ActionEventEDList.StaticDefinedEvents();

                ActionEvent ty = events.Find(x => x.TriggerName == evname);
                if ( ty?.Properties != null )                                           // if its a static event and it has variables, add..
                    fieldnames.AddRange(ty.Properties);

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
            avf.Init("Global User variables to pass to program on run".TxID(EDTx.ActionPackVariablesForm_gv), this.Icon, PersistentVariables, showatleastoneentry: true);

            if (avf.ShowDialog(DiscoveryForm) == DialogResult.OK)
            {
                LoadPeristentVariables(avf.Result);
            }
        }

        private void Dmf_OnCreateActionFile()
        {
            String r = ExtendedControls.PromptSingleLine.ShowDialog(DiscoveryForm, "New name", "", "Create new action file", this.Icon);
            if (r != null && r.Length > 0)
            {
                if (actionfiles.Get(r, StringComparison.InvariantCultureIgnoreCase) == null)
                {
                    actionfiles.AddNewFile(r, actfolder);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Duplicate name", "Create Action File Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            if (approotfolder == null)
                return false;

            bool changed = false;

            using (ActionLanguage.Manager.AddOnManagerForm dmf = new ActionLanguage.Manager.AddOnManagerForm())
            {
                var edversion = System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersionValues();
                System.Diagnostics.Debug.Assert(edversion != null);

                dmf.Init("EDDiscovery", manage, this.Icon, edversion,
                                            approotfolder,
                                            actfolder,
                                            manage ? otherinstalledfilesfolder : null,      // only on manage!
                                            EDDOptions.Instance.TempDirectory(),
                                            EDDOptions.Instance.TempMoveDirectory(), Properties.Resources.URLGithubDataDownload,
                                            EDDOptions.Instance.CheckGithubFiles);

                dmf.EditActionFile += Dmf_OnEditActionFile;     // only used when manage = false
                dmf.EditGlobals += Dmf_OnEditGlobals;
                dmf.CreateActionFile += Dmf_OnCreateActionFile;
                dmf.CheckActionLoaded += Dmf_checkActionLoaded;
                dmf.DeleteActionFile += (item) =>
                {
                    int panelid = GetPanelID(item.LocalVars);
                    if (panelid != -1)
                    {
                        DiscoveryForm.RemovePanel((PanelInformation.PanelIDs)panelid);
                    };
                };

                dmf.ShowDialog(DiscoveryForm);

                if (dmf.ChangeList.Count > 0)
                {
                    actionrun?.TerminateAll();
                    AudioQueueSpeech?.StopAll();
                    AudioQueueWave?.StopAll();

                    ReLoad(false);      // reload from disk, new ones if required, refresh old ones and keep the vars
                    CreatePanels();
                    CheckWarn();

                    string changes = "", updates="", removes="";
                    foreach (KeyValuePair<string, string> kv in dmf.ChangeList)
                    {
                        if (kv.Value.Contains("+"))
                        {
                            changes += kv.Key + ";";
                            if (kv.Value.Contains("++"))
                                updates += kv.Key + ";";
                        }

                        if (kv.Value.Equals("-"))
                        {
                            DiscoveryForm.RemoveMenuItemsFromAddOns(kv.Key);
                            removes += kv.Key + ";";
                        }
                    }

                    ActionRun(ActionEventEDList.onInstall, null, new Variables(new string[] { "InstallList", changes, "UpdateList", updates, "RemoveList",removes }));

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

        // configure, using global vars, or passed values. If vcname == null the peristent variables are set on OK
        public Tuple<string,string,string,string> ConfigureVoice(string title, bool nodevice, bool novoicename, bool norate,
                            string vcname = null, string vcvolume = null, string vcrate = null, string vceffects = null)    
        {
            string voicename = vcname ?? Globals.GetString(ActionSay.globalvarspeechvoice, "Default");
            string volume = vcvolume ?? Globals.GetString(ActionSay.globalvarspeechvolume, "Default");
            string rate = vcrate ?? Globals.GetString(ActionSay.globalvarspeechrate, "Default");
            Variables effects = new Variables(vceffects ?? PersistentVariables.GetString(ActionSay.globalvarspeecheffects, ""), Variables.FromMode.MultiEntryComma);

            ExtendedAudioForms.SpeechConfigure cfg = new ExtendedAudioForms.SpeechConfigure();
            cfg.Init(true, nodevice, novoicename, norate, AudioQueueSpeech, SpeechSynthesizer,
                        title, this.Icon,
                        null, false, false, AudioExtensions.AudioQueue.Priority.Normal, "", "",
                        voicename,
                        volume,
                        rate,
                        effects);

            if (cfg.ShowDialog(DiscoveryForm) == DialogResult.OK)
            {
                if (vcname == null)
                {
                    SetPeristentGlobal(ActionSay.globalvarspeechvoice, cfg.VoiceName);
                    SetPeristentGlobal(ActionSay.globalvarspeechvolume, cfg.Volume);
                    SetPeristentGlobal(ActionSay.globalvarspeechrate, cfg.Rate);
                    SetPeristentGlobal(ActionSay.globalvarspeecheffects, cfg.Effects.ToString());
                }

                if ( !nodevice)
                    EDDConfig.Instance.DefaultVoiceDevice = AudioQueueSpeech.Driver.GetAudioEndpoint();

                return new Tuple<string, string, string, string>(cfg.VoiceName, cfg.Volume, cfg.Rate, cfg.Effects.ToString());
            }
            else
                return null;
        }

        // configure, using global vars, or passed values. If vcname == null the peristent variables are set on OK
        public Tuple<string, string> ConfigureWave(bool nodevice, string title, string vcvolume = null, string vceffects = null)
        {
            string volume = vcvolume ?? Globals.GetString(ActionPlay.globalvarplayvolume, "60");
            Variables effects = new Variables(vceffects ?? PersistentVariables.GetString(ActionPlay.globalvarplayeffects, ""), Variables.FromMode.MultiEntryComma);

            ExtendedAudioForms.WaveConfigureDialog dlg = new ExtendedAudioForms.WaveConfigureDialog();
            dlg.Init(true, nodevice, AudioQueueWave,
                        title, this.Icon,
                        "",
                        false, AudioExtensions.AudioQueue.Priority.Normal, "", "",
                        volume, effects);

            if (dlg.ShowDialog(DiscoveryForm) == DialogResult.OK)
            {
                if (vcvolume == null)
                {
                    SetPeristentGlobal(ActionPlay.globalvarplayvolume, dlg.Volume);
                    SetPeristentGlobal(ActionPlay.globalvarplayeffects, dlg.Effects.ToString());
                }

                EDDConfig.Instance.DefaultWaveDevice = AudioQueueWave.Driver.GetAudioEndpoint();

                return new Tuple<string, string>(dlg.Volume, dlg.Effects.ToString());
            }
            else
                return null;
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

            ExtendedControls.MessageBoxTheme.Show(DiscoveryForm, "Voice pack not loaded, or needs updating to support this functionality");
        }

        #region RUN!

        public void ActionRunOnRefresh()
        {
            string prevcommander = Globals.Exists("Commander") ? Globals["Commander"] : "None";
            string commander = HistoryList.IsRealCommanderId ? EDCommander.Current.Name : "Hidden";

            string refreshcount = prevcommander.Equals(commander) ? Globals.AddToVar("RefreshCount", 1, 1) : "1";
            SetInternalGlobal("RefreshCount", refreshcount);
            SetInternalGlobal("Commander", commander);

            if (actionfiles.IsActionVarDefined("RunAtRefresh"))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in HistoryList.EntryOrder())
                    ActionRunOnEntry(he, ActionEventEDList.RefreshJournal(he), "RunAtRefresh");
            }

            ActionRun(ActionEventEDList.onRefreshEnd);
        }

        public int ActionRunOnEntry(HistoryEntry he, ActionEvent ev, string flagstart = null, bool now = false)      
        {
            return ActionRun(ev, he, null, flagstart, now);
        }

        // override base
        public override int ActionRun(ActionEvent ev, Variables additionalvars = null, string flagstart = null, bool now = false)              
        { return ActionRun(ev, null, additionalvars, flagstart, now); }

        public int ActionRun(ActionEvent ev, 
                                HistoryEntry he = null, 
                                Variables additionalvars = null,
                                string actionvarnamepresent = null,                    // action var present, if not null, must be there
                                bool now = false)       
        {
            List<ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(ev.TriggerName, actionvarnamepresent);      // look thru all actions, find matching ones

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
                    actionfiles.RunActions(now, ale, actionrun, eventvars);  // add programs to action run

                    actionrun.Execute();       // See if needs executing
                }
            }

            return ale.Count;
        }

        #endregion

        // generate the start up events and configure the keys
        public void onStartup()     // on main thread
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            ActionRun(ActionEvent.onStartup);
            ActionRun(ActionEvent.onPostStartup);
            ActionConfigureKeys();          // reload keys
        }

        // wait until action program finishes
        public void HoldTillProgStops(int timeout = 10000)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);
            actionrun.WaitTillFinished(timeout, true);
        }

        // close down, return global variables to store
        public string CloseDown()
        {
            inputdevicesactions.Stop();
            inputdevices.Clear();

            VoiceRecon.Close();

            System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " AC Closedown complete");

            return PersistentVariables.ToString();
        }

        // output a log - overridden from core
        public override void LogLine(string s)
        {
            logLineOut.Invoke(s);
        }

        #region Misc overrides

        public override bool Pragma(string s)     // extra pragmas.
        {
            if (s.Equals("bindings"))
            {
                LogLine(FrontierBindings.ListBindings());
            }
            else if (s.Equals("bindingvalues"))
            {
                LogLine(FrontierBindings.ListValues());
            }
            else
                return false;

            return true;
        }

        #endregion


        #region Elite Input 

        private bool axisison = false;      // record if anywant wants axis events

        public void EliteInput(bool on, bool axisevents)
        {
            inputdevicesactions.Stop();
            inputdevices.Clear();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (on)
                {
                    axisison |= axisevents;

                    DirectInputDevices.InputDeviceJoystickWindows.CreateJoysticks(inputdevices, axisison);
                    DirectInputDevices.InputDeviceKeyboard.CreateKeyboard(inputdevices);              // Created.. not started..
                    DirectInputDevices.InputDeviceMouse.CreateMouse(inputdevices);
                    inputdevicesactions.Start();
                }
                else
                    axisison = false;
            }
        }

        public string EliteInputList() { return inputdevices.ListDevices(); }
        public string EliteInputCheck() { return inputdevicesactions.CheckBindings(); }
        public string EliteInputButtons()
        {
            string s = "";
            foreach (var i in inputdevices)
            {
                var list = i.EventButtonNames();
                var dlist = list.Select(x => i.Name() + ":" + x);
                s = s.AppendPrePad(string.Join(",", dlist), ",");
            }

            return s;
        }

        #endregion

        #region vars

        private string actfolder;       // where the action files are stored
        private string approotfolder;   // folder location root where to install stuff into
        private string otherinstalledfilesfolder;   // other installed files
        private string errorlist;       // set on Reload, use to display warnings at right point
        private string lasteditedpack;
        private DirectInputDevices.InputDeviceList inputdevices;
        private Actions.ActionsFromInputDevices inputdevicesactions;
        private Type[] keyignoredforms;
        private Action<string> logLineOut;

        #endregion

    }
}
