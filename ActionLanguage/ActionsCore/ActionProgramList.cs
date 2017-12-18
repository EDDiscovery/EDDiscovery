/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionLanguage
{
    public class ActionProgramList                  // Holder of programs loaded into memory
    {
        public ActionProgramList()
        {
            Clear();
        }

        private List<ActionProgram> programs;

        public ActionProgram Get(string name)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(name));

            return (existing >= 0) ? programs[existing] : null;
        }

        public int Count { get { return programs.Count; } }

        public IEnumerable<ActionProgram> Enumerable { get { return programs; } }

        public ActionProgram Get(int n)
        {
            return (n < programs.Count) ? programs[n] : null;
        }

        public void Add(ActionProgram p)
        {
            programs.Add(p);
        }

        public string[] GetActionProgramList(bool markext = false)
        {
            string[] ret = new string[programs.Count];
            for (int i = 0; i < programs.Count; i++)
                ret[i] = programs[i].Name + ((markext && programs[i].StoredInSubFile!=null) ? " (Ext)":"");

            return ret;
        }

        public void Clear()
        {
            programs = new List<ActionProgram>();
        }

        public void AddOrChange(ActionProgram ap)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(ap.Name));

            if (existing >= 0)
                programs[existing] = ap;
            else
                programs.Add(ap);
        }

        public void Delete(string name)
        {
            int existing = programs.FindIndex(x => x.Name.Equals(name));

            if (existing >= 0)
                programs.RemoveAt(existing);
        }

        string edoutername;
        ActionCoreController cp;
        string appfolder;

        public bool EditProgram(string s, string outername , ActionCoreController cp, string appfolder)
        {
            int colon = s.IndexOf(':');
            if (colon >= 0)
                s = s.Substring(0, colon);
            int bracket = s.IndexOf('(');
            if (bracket >= 0)
                s = s.Substring(0, bracket);

            ActionProgram p = Get(s);

            if ( p != null )
            {
                if (p.StoredInSubFile != null)
                {
                    p.EditInEditor(p.StoredInSubFile);         // Edit in the editor..
                }
                else
                {
                    ActionProgramEditForm apf = new ActionProgramEditForm();
                    apf.EditProgram += EditProgram;

                    this.edoutername = outername;
                    this.cp = cp;
                    this.appfolder = appfolder;

                    List<string> additionalfieldnames = new List<string>(); // We don't have any names to contribute going thru this path

                    apf.Init("Action program ", cp.Icon, cp, appfolder, additionalfieldnames, outername, p, GetActionProgramList(), "");

                    System.Windows.Forms.DialogResult res = apf.ShowDialog();

                    if (res == System.Windows.Forms.DialogResult.OK)
                    {
                        ActionProgram np = apf.GetProgram();
                        AddOrChange(np);                // replaces or adds (if its a new name) same as rename
                    }

                    return true;
                }
            }

            return false;
        }

        public void EditProgram(string s)
        {
            if ( !EditProgram(s, edoutername, cp, appfolder) )
                ExtendedControls.MessageBoxTheme.Show("Unknown program or not in this file " + s);
        }
    }
}
