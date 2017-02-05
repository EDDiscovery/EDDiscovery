using EDDiscovery.DB;
using EDDiscovery.Forms;
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

        private ConditionVariables internalglobalvariables;         // internally set variables, either program or user program ones
        private ConditionVariables usercontrolledglobalvariables;   // user variables, set by user only, including user setting vars like SpeechVolume
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

            usercontrolledglobalvariables = new ConditionVariables();
            usercontrolledglobalvariables.FromString(SQLiteConnectionUser.GetSettingString("UserGlobalActionVars", ""), ConditionVariables.FromMode.MultiEntryComma);

            globalvariables = new ConditionVariables(usercontrolledglobalvariables);        // copy existing user ones into to shared buffer..

            internalglobalvariables = new ConditionVariables();

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
            events.Add("onStartup");
            events.Add("onShutdown");
            events.Add("onKeyPress");
            //events.Add("onClosedown");

            frm.InitAction("Actions: Define actions", events, globalvariables.KeyList, usercontrolledglobalvariables, actionfiles, discoveryform.theme);
            frm.TopMost = discoveryform.FindForm().TopMost;

            frm.ShowDialog(discoveryform.FindForm()); // don't care about the result, the form does all the saving

            usercontrolledglobalvariables = frm.userglobalvariables;
            globalvariables = new ConditionVariables(internalglobalvariables, usercontrolledglobalvariables);    // remake

            ActionConfigureKeys();
        }

        public void ManageAddOns()
        {
            DownloadManagerForm dmf = new DownloadManagerForm();
            dmf.Init(discoveryform.theme);
            dmf.ShowDialog(discoveryform);
            if (dmf.performedupdate)
            {
                actionrunasync.TerminateAll();
                Actions.ActionSay.KillSpeech();
                ReLoad();
                ActionRunOnEvent("onStartup", "ProgramEvent");
            }
        }

        public void ConfigureVoice()
        {
            Speech.SpeechConfigure cfg = new Speech.SpeechConfigure();

            string voicename = usercontrolledglobalvariables.ContainsKey(Actions.ActionSay.globalvarspeechvoice) ? usercontrolledglobalvariables[Actions.ActionSay.globalvarspeechvoice] : "Default";
            string volume = usercontrolledglobalvariables.ContainsKey(Actions.ActionSay.globalvarspeechvolume) ? usercontrolledglobalvariables[Actions.ActionSay.globalvarspeechvolume] : "Default";
            string rate = usercontrolledglobalvariables.ContainsKey(Actions.ActionSay.globalvarspeechrate) ? usercontrolledglobalvariables[Actions.ActionSay.globalvarspeechrate] : "Default";

            Speech.QueuedSynthesizer synth = new Speech.QueuedSynthesizer();           // STATIC only one synth throught the whole program

            cfg.Init("Select voice synthesizer defaults", "Configure Voice Synthesis", discoveryform.theme,
                        null, false,
                        synth.GetVoiceNames(),
                        voicename,
                        volume,
                        rate);

            if (cfg.ShowDialog(discoveryform) == DialogResult.OK)
            {
                int i;
                if (cfg.Volume.Equals("Default", StringComparison.InvariantCultureIgnoreCase) || (cfg.Volume.InvariantParse(out i) && i >= 0 && i <= 100))
                {
                    if (cfg.Rate.Equals("Default", StringComparison.InvariantCultureIgnoreCase) || (cfg.Rate.InvariantParse(out i) && i >= -10 && i <= 10))
                    {
                        SetUserControlledGlobal(Actions.ActionSay.globalvarspeechvoice, cfg.VoiceName);
                        SetUserControlledGlobal(Actions.ActionSay.globalvarspeechvolume, cfg.Volume);
                        SetUserControlledGlobal(Actions.ActionSay.globalvarspeechrate, cfg.Rate);

                        return;
                    }
                }

                MessageBox.Show("Speech values not within range, values not saved");
            }

        }

        public void ConfigureSpeechText()
        {
            if (internalglobalvariables.ContainsKey("SpeechDefinitionFile"))
            {
                string prog = internalglobalvariables["SpeechDefinitionFile"];

                Tuple<ActionFile, ActionProgram> ap = actionfiles.FindProgram(prog);

                if (ap.Item2.EditInEditor())
                {
                    ap.Item1.SaveFile();
                }
            }
            else
                MessageBox.Show("Voice pack not loaded, or needs updating to support this functionality");
        }

        public void ActionRunOnRefresh()
        {
            string prevcommander = internalglobalvariables.ContainsKey("Commander") ? internalglobalvariables["Commander"] : "None";
            string commander = (discoverycontroller.history.CommanderId < 0) ? "Hidden" : EDDConfig.Instance.CurrentCommander.Name;

            string refreshcount = prevcommander.Equals(commander) ? internalglobalvariables.AddToVar("RefreshCount", 1, 1) : "1";
            SetInternalGlobal("RefreshCount", refreshcount);
            SetInternalGlobal("Commander", commander);

            if (actionfiles.IsConditionFlagSet(ConditionVariables.flagRunAtRefresh))      // any events have this flag? .. don't usually do this, so worth checking first
            {
                foreach (HistoryEntry he in discoverycontroller.history.EntryOrder)
                    ActionRunOnEntry(he, "onRefresh", ConditionVariables.flagRunAtRefresh);
            }

            ActionRunOnEvent("onRefreshEnd", "ProgramEvent");
        }


        public int ActionRunOnEntry(HistoryEntry he, string triggertype, string flagstart = null)       //set flagstart to be the first flag of the actiondata..
        {
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(he.journalEntry.EventTypeStr, flagstart);

            if (ale.Count > 0)
            {
                ConditionVariables testvars = new ConditionVariables(globalvariables);
                Actions.ActionVars.TriggerVars(testvars, he.journalEntry.EventTypeStr, triggertype);
                Actions.ActionVars.HistoryEventVars(testvars, he, "Event");

                ConditionFunctions functions = new ConditionFunctions();

                if (actionfiles.CheckActions(ale, he.journalEntry.EventDataString, testvars, functions.ExpandString) > 0)
                {
                    ConditionVariables eventvars = new ConditionVariables();        // we don't pass globals in - added when they are run
                    Actions.ActionVars.TriggerVars(eventvars, he.journalEntry.EventTypeStr, triggertype);
                    Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");
                    eventvars.GetJSONFieldNamesAndValues(he.journalEntry.EventDataString, "EventJS_");        // for all events, add to field list

                    actionfiles.RunActions(ale, actionrunasync, eventvars);  // add programs to action run

                    actionrunasync.Execute();       // will execute
                }
            }

            return ale.Count;
        }

        public int ActionRunOnEvent(string name, string triggertype, ConditionVariables additionalvars = null)
        {
            List<Actions.ActionFileList.MatchingSets> ale = actionfiles.GetMatchingConditions(name);

            if (ale.Count > 0)
            {
                ConditionVariables testvars = new ConditionVariables(globalvariables);
                Actions.ActionVars.TriggerVars(testvars, name, triggertype);

                ConditionFunctions functions = new ConditionFunctions();

                if (actionfiles.CheckActions(ale, null, testvars, functions.ExpandString) > 0)
                {
                    ConditionVariables eventvars = new ConditionVariables();
                    Actions.ActionVars.TriggerVars(eventvars, name, triggertype);

                    if (additionalvars != null)
                        eventvars.Add(additionalvars);

                    actionfiles.RunActions(ale, actionrunasync, eventvars);  // add programs to action run

                    actionrunasync.Execute();       // will execute
                }
            }

            return ale.Count;
        }

        public void SetUserControlledGlobal(string name, string value)     // saved on exit
        {
            usercontrolledglobalvariables[name] = globalvariables[name] = value;
        }

        public void SetInternalGlobal(string name, string value)           // internal program vars
        {
            internalglobalvariables[name] = globalvariables[name] = value;
        }

        public void SetProgramGlobal(string name, string value)         // different name for identification purposes, for sets
        {
            internalglobalvariables[name] = globalvariables[name] = value;
        }

        public void TerminateAll()
        {
            actionrunasync.TerminateAll();
        }

        public void CloseDown()
        {
            actionrunasync.WaitTillFinished(10000);
            Actions.ActionSay.KillSpeech();
            SQLiteConnectionUser.PutSettingString("UserGlobalActionVars", usercontrolledglobalvariables.ToString());
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
                ActionRunOnEvent("onKeyPress", "KeyPress");
                return true;
            }
            else
                return false;
        }


        const int WM_KEYDOWN = 0x100;
        const int WM_KEYCHAR = 0x102;
        const int WM_SYSKEYDOWN = 0x104;

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
                if ((m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN) && discoveryform.CanFocus)
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
