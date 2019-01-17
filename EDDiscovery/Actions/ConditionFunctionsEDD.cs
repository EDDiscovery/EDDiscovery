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
using BaseUtils;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    // inherit from base functions and add/override some functions

    public class ConditionEDDFunctions : FunctionsBasic
    {
        public ConditionEDDFunctions(Functions c, Variables v, FunctionPersistentData h, int recd) : base(c, v, h, recd)
        {
            if (functions == null)        // one time init, done like this cause can't do it in {}
            {
                functions = new Dictionary<string, FuncEntry>();
                functions.Add("systempath", new FuncEntry(SystemPath, FuncEntry.PT.LmeSE));   // literal
                functions.Add("version", new FuncEntry(Version, FuncEntry.PT.ImeSE));
                functions.Add("star", new FuncEntry(Star, FuncEntry.PT.MESE, FuncEntry.PT.LmeSE));
                functions.Add("ship", new FuncEntry(Ship, FuncEntry.PT.MESE));
                functions.Add("events", new FuncEntry(Events, FuncEntry.PT.MESE, FuncEntry.PT.MESE));
            }
        }

        static Dictionary<string, FuncEntry> functions = null;

        protected override FuncEntry FindFunction(string name)
        {
            name = name.ToLowerInvariant();      // case insensitive.
            return functions.ContainsKey(name) ? functions[name] : base.FindFunction(name);
        }

        #region Macro Functions

        protected new bool SystemPath(out string output)
        {
            string id = paras[0].Value;

            if (id.Equals("EDDAPPFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = EDDOptions.Instance.AppDataDirectory;
            else if (id.Equals("EDDACTIONSFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = EDDOptions.Instance.ActionsAppDirectory();
            else if (id.Equals("EDDVIDEOFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = EDDOptions.Instance.VideosAppDirectory();
            else if (id.Equals("EDDSOUNDFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = EDDOptions.Instance.SoundsAppDirectory();
            else
                return base.SystemPath(out output);

            return true;
        }

        protected bool Version(out string output)
        {
            int[] edversion = System.Reflection.Assembly.GetExecutingAssembly().GetVersionInts();

            int para = paras[0].Int;
            if (para >= 0 && para <= edversion.Length)
            {
                if (para == 0)
                    output = edversion[0] + "." + edversion[1] + "." + edversion[2] + "." + edversion[3];
                else
                    output = edversion[para - 1].ToString(ct);
                return true;
            }
            else
            {
                output = "Parameter number must be 0 (all), 1 to 4";
                return false;
            }
        }

        static public string PhoneticShipName(string inname)
        {
            return inname
                .Replace("Mk IV", "Mark 4").Replace("MkIV", "Mark 4")
                .Replace("Mk III", "Mark 3").Replace("MkIII", "Mark 3")
                .Replace("Mk II", "Mark 2").Replace("MkII", "Mark 2")
                .Replace("Mk I", "Mark 1").Replace("MkI", "Mark 1");



        }

        protected bool Ship(out string output)
        {
            output = PhoneticShipName(paras[0].Value);
            output = output.SplitCapsWordFull();
            return true;
        }

        protected bool Star(out string output)
        {
            paras[0].Value = paras[0].Value.Replace(" ", "  ");     // ensure we have enough spaces for regex

            paras[0].Value = System.Text.RegularExpressions.Regex.Replace(paras[0].Value,           // find space char space|EOL
                                     @"(\s+)[A-Za-z0-9](\s|$)",
                                     delegate (System.Text.RegularExpressions.Match match)
                                     {
                                         return ", " + match.Value.Trim();
                                     });

            paras[0].Value = System.Text.RegularExpressions.Regex.Replace(paras[0].Value, @"(\s*)([A-Za-z0-9]+)\-([A-Za-z0-9]+)",
                                    delegate (System.Text.RegularExpressions.Match match)
                                    {
                                        string r = System.Text.RegularExpressions.Regex.Replace(match.Value, @"([A-Za-z\-])", " $1 ");  // space out alphas and dash
                                        return ", " + r.Trim();
                                    });

            paras[0].Value = paras[0].Value.Replace("  ", " ");
            paras[0].Value = paras[0].Value.Replace("Belt Cluster", ", Belt Cluster");



            return ReplaceVarCommon(out output, true);
        }

        protected bool Events(out string output)
        {
            output = "";
            foreach (EliteDangerousCore.JournalTypeEnum v in Enum.GetValues(typeof(EliteDangerousCore.JournalTypeEnum)) )
            {
                if ((int)v>0&&(int)v<(int)EliteDangerousCore.JournalTypeEnum.EDDItemSet)
                {
                    output += paras[0].Value + v.ToString() + paras[1].Value;
                }
            }
            return true;
        }

        protected override bool VerifyFileAccess(string file, FileMode fm)
        {
            if (fm != FileMode.Open)
            {
                return VerifyFileAction("write", file);
            }
            else
                return true;
        }

        protected override bool VerifyFileAction(string action, string file)
        {
            string folder = Path.GetDirectoryName(file);
            string actionfolderperms = SQLiteConnectionUser.GetSettingString("ActionFolderPerms", "");

            if (!actionfolderperms.Contains(folder + ";"))
            {
                bool ok = ExtendedControls.MessageBoxTheme.Show("Warning - This program is attempting to "+ action + " folder" + Environment.NewLine + Environment.NewLine +
                                                     folder + Environment.NewLine + Environment.NewLine +
                                                     "with file " + Path.GetFileName(file) + Environment.NewLine + Environment.NewLine +
                                                       "!!! Verify you are happy for the program to perform this action and access ANY files in that folder!!!",
                                                       "WARNING - ACCESS REQUESTED",
                                                    System.Windows.Forms.MessageBoxButtons.YesNo,
                                                    System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes;

                if (ok)
                {
                    SQLiteConnectionUser.PutSettingString("ActionFolderPerms", actionfolderperms + folder + ";");
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        protected override bool VerifyProcessAllowed(string proc, string cmdline)
        {
            string actionprocessperms = SQLiteConnectionUser.GetSettingString("ActionProcessPerms", "");

            if (!actionprocessperms.Contains("!" + proc + ";"))
            {
                bool ok = ExtendedControls.MessageBoxTheme.Show("Warning - This program is attempting to run the following process" + Environment.NewLine + Environment.NewLine +
                                                        proc + Environment.NewLine + Environment.NewLine +
                                                        "!!! Verify you are happy for the process to run now, and in the future!!!",
                                                        "WARNING - PROCESS WANTS TO RUN",
                                                    System.Windows.Forms.MessageBoxButtons.YesNo,
                                                    System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes;

                if (ok)
                {
                    SQLiteConnectionUser.PutSettingString("ActionProcessPerms", actionprocessperms + "!" + proc + ";");
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        #endregion

    }
}
