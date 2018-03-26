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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conditions;
using AudioExtensions;

namespace ActionLanguage
{
    public class ActionCoreController
    {
        protected ActionFileList actionfiles;
        protected ActionRun actionrunasync;

        private ConditionVariables programrunglobalvariables;         // program run, lost at power off, set by GLOBAL or internal 
        private ConditionVariables persistentglobalvariables;   // user variables, set by user only, including user setting vars like SpeechVolume
        private ConditionVariables globalvariables;                  // combo of above.

        protected ConditionVariables PersistentVariables { get { return persistentglobalvariables; } }
        protected void LoadPeristentVariables(ConditionVariables list)
        {
            persistentglobalvariables = list;
            globalvariables = new ConditionVariables(persistentglobalvariables, programrunglobalvariables);
        }

        virtual public AudioExtensions.AudioQueue AudioQueueWave { get; }
        virtual public AudioExtensions.AudioQueue AudioQueueSpeech { get; }
        virtual public AudioExtensions.SpeechSynthesizer SpeechSynthesizer { get; }

        public ConditionVariables Globals { get { return globalvariables; } }

        protected System.Windows.Forms.Form form;

        public System.Windows.Forms.Form Form { get { return form; } }

        public System.Drawing.Icon Icon { get; private set;}

        public bool AsyncMode { get { return actionrunasync.AsyncMode; } set { actionrunasync.AsyncMode = value; } }
        public void DebugTrace(bool ll, string file = null) { actionrunasync.DebugTrace(ll, file); }

        public ActionCoreController(System.Windows.Forms.Form frm, System.Drawing.Icon ic )
        {
            Icon = ic;
            form = frm;

            persistentglobalvariables = new ConditionVariables();
            globalvariables = new ConditionVariables();
            programrunglobalvariables = new ConditionVariables();

            SetInternalGlobal("CurrentCulture", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            SetInternalGlobal("CurrentCultureInEnglish", System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName);
            SetInternalGlobal("CurrentCultureISO", System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);

            ActionBase.AddCommand("Break", typeof(ActionBreak), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Call", typeof(ActionCall), ActionBase.ActionType.Call);
            ActionBase.AddCommand("Dialog", typeof(ActionDialog), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("DialogControl", typeof(ActionDialogControl), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Do", typeof(ActionDo), ActionBase.ActionType.Do);
            ActionBase.AddCommand("DeleteVariable", typeof(ActionDeleteVariable), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Expr", typeof(ActionExpr), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Else", typeof(ActionElse), ActionBase.ActionType.Else);
            ActionBase.AddCommand("ElseIf", typeof(ActionElseIf), ActionBase.ActionType.ElseIf);
            ActionBase.AddCommand("End", typeof(ActionEnd), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("ErrorIf", typeof(ActionErrorIf), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("ForEach", typeof(ActionForEach), ActionBase.ActionType.ForEach);
            ActionBase.AddCommand("FileDialog", typeof(ActionFileDialog), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("GlobalLet", typeof(ActionGlobalLet), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Global", typeof(ActionGlobal), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("If", typeof(ActionIf), ActionBase.ActionType.If);
            ActionBase.AddCommand("InputBox", typeof(ActionInputBox), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("InfoBox", typeof(ActionInfoBox), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Key", typeof(ActionKey), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("MessageBox", typeof(ActionMessageBox), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("NonModalDialog", typeof(ActionNonModalDialog), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Return", typeof(ActionReturn), ActionBase.ActionType.Return);
            ActionBase.AddCommand("Pragma", typeof(ActionPragma), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Let", typeof(ActionLet), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Loop", typeof(ActionLoop), ActionBase.ActionType.Loop);
            ActionBase.AddCommand("Rem", typeof(ActionRem), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("PersistentGlobal", typeof(ActionPersistentGlobal), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Print", typeof(ActionPrint), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Say", typeof(ActionSay), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Set", typeof(ActionSet), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("StaticLet", typeof(ActionStaticLet), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Static", typeof(ActionStatic), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Sleep", typeof(ActionSleep), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("While", typeof(ActionWhile), ActionBase.ActionType.While);
            ActionBase.AddCommand("//", typeof(ActionFullLineComment), ActionBase.ActionType.Cmd);
            ActionBase.AddCommand("Else If", typeof(ActionElseIf), ActionBase.ActionType.ElseIf);
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

        public virtual void LogLine(string s)
        { }

        public virtual int ActionRun(ActionEvent ev, ConditionVariables additionalvars = null,
                                string flagstart = null, bool now = false)
        { return 0; }

        public virtual bool Pragma(string s)    // extend pragmas
        {
            return false;
        }
    }
}
