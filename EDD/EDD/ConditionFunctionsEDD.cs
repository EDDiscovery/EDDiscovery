/*
 * Copyright © 2017-2024 EDDiscovery development team
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

using BaseUtils;
using System;
using System.Collections.Generic;
using System.IO;

namespace EDDiscovery
{
    // inherit from base functions and add/override some functions

    public class ConditionEDDFunctions : FunctionsBasic
    {
        static public BaseUtils.FunctionHandlers DefaultGetCFH(BaseUtils.Functions c, BaseUtils.Variables vars, BaseUtils.FunctionPersistentData handles, int recdepth)
        {
            return new ConditionEDDFunctions(c, vars, handles, recdepth);
        }

        public ConditionEDDFunctions(Functions c, Variables v, FunctionPersistentData h, int recd) : base(c, v, h, recd)
        {
            if (functions == null)        // one time init, done like this cause can't do it in {}
            {
                functions = new Dictionary<string, FuncEntry>();
                functions.Add("body", new FuncEntry(Body, 3, FuncEntry.PT.MESE, FuncEntry.PT.MESE, FuncEntry.PT.LmeSE, FuncEntry.PT.LS));
                functions.Add("systempath", new FuncEntry(SystemPath, FuncEntry.PT.LmeSE));   // literal
                functions.Add("version", new FuncEntry(Version, FuncEntry.PT.ImeSE));
                functions.Add("star", new FuncEntry(Star, 2, FuncEntry.PT.MESE, FuncEntry.PT.LmeSE, FuncEntry.PT.LS));
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
            Version edversion = System.Reflection.Assembly.GetExecutingAssembly().GetAssemblyVersion();
            int para = paras[0].Int;

            if (para >= 0 && para <= 4)
            {
                if (para == 0)
                    output = edversion.ToString();
                else if (para == 1)
                    output = edversion.Major.ToString(ct);
                else if (para == 2)
                    output = edversion.Minor.ToString(ct);
                else if (para == 3)
                    output = edversion.Revision.ToString(ct);
                else 
                    output = edversion.Build.ToString(ct);
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
            // 2 or 3 paras. [0] = value, [1] = replace, optional [3] = splitcaps

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

            paras[0].Value = paras[0].Value.ReplaceIfStartsWith(", ", "");
            paras[0].Value = paras[0].Value.Replace("ABC, ", "A B C, ");
            paras[0].Value = paras[0].Value.Replace("ABCD, ", "A B C D, ");
            paras[0].Value = paras[0].Value.Replace("ABCE, ", "A B C D E, ");
            paras[0].Value = paras[0].Value.Replace("AB, ", "A B, ");
            paras[0].Value = paras[0].Value.Replace("BC, ", "B C, ");
            paras[0].Value = paras[0].Value.Replace("CD, ", "C D, ");
            paras[0].Value = paras[0].Value.Replace("DE, ", "D E, ");
            paras[0].Value = paras[0].Value.Replace("EF, ", "E F, ");
            paras[0].Value = paras[0].Value.Replace("FG, ", "F G, ");

            paras[0].Value = paras[0].Value.Replace("  ", " ");
            paras[0].Value = paras[0].Value.Replace("Belt Cluster", ", Belt Cluster");

            bool splitcaps = true;
            if (paras.Count >= 3)
            {
                if (paras[2].Value.EqualsIIC("SplitCaps"))
                { }
                else if (paras[2].Value.EqualsIIC("NoSplitCaps") || paras[2].Value.EqualsIIC("NS"))
                {
                    splitcaps = false;
                }
                else
                {
                    output = "Incorrect Split caps control word";
                    return false;
                }
            }

            // expects para[0] = value, para[1] = replace var.
            return ReplaceVarCommon(out output, splitcaps);
        }

        protected bool Body(out string output)
        {
            paras[0].Value = paras[0].Value.Trim();

            // if [0] starts with [1], and there is more in [0] then [1], remove
            if (paras[0].Value.StartsWith(paras[1].Value, StringComparison.InvariantCultureIgnoreCase)) 
            {
                string v = paras[0].Value.Substring(paras[1].Value.Length).Trim();
                if (v.Length > 0)       // don't replace if its all spaces..
                    paras[0].Value = v;
            }

            paras[1].Value = paras[2].Value;    // move root var name to [1] where Star expects it to be.

            if (paras.Count >= 4)           // if split caps control
            {
                paras[2].Value = paras[3].Value;    // move split caps control to position 2 and remove 3 so count the same
                paras.RemoveAt(3);          // make count = 3
            }
            else
                paras.RemoveAt(2);          // else remove para 3 to get length 2


            return Star(out output);        // do star
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
            string actionfolderperms = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("ActionFolderPerms", "");

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
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ActionFolderPerms", actionfolderperms + folder + ";");
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
            string actionprocessperms = EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("ActionProcessPerms", "");

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
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ActionProcessPerms", actionprocessperms + "!" + proc + ";");
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
