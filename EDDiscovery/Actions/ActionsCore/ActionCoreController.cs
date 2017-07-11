using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    public class ActionCoreController
    {
        protected Actions.ActionFileList actionfiles;
        protected Actions.ActionRun actionrunasync;

        protected ConditionVariables programrunglobalvariables;         // program run, lost at power off, set by GLOBAL or internal 
        protected ConditionVariables persistentglobalvariables;   // user variables, set by user only, including user setting vars like SpeechVolume
        protected ConditionVariables globalvariables;                  // combo of above.

        public ConditionVariables Globals { get { return globalvariables; } }

        public ActionCoreController()
        {
            persistentglobalvariables = new ConditionVariables();
            globalvariables = new ConditionVariables(persistentglobalvariables);        // copy existing user ones into to shared buffer..
            programrunglobalvariables = new ConditionVariables();

            SetInternalGlobal("CurrentCulture", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            SetInternalGlobal("CurrentCultureInEnglish", System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName);
            SetInternalGlobal("CurrentCultureISO", System.Threading.Thread.CurrentThread.CurrentCulture.ThreeLetterISOLanguageName);
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

    }
}
