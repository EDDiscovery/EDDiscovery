using Conditions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    // inherit from base functions and add/override some functions

    public class ConditionEDDFunctions : ConditionFunctionsBase
    {
        public ConditionEDDFunctions(ConditionFunctions c, ConditionVariables v, ConditionFileHandles h, int recd) : base(c, v, h, recd)
        {
            if (functions == null)        // one time init, done like this cause can't do it in {}
            {
                functions = new Dictionary<string, FuncEntry>();
                functions.Add("systempath", new FuncEntry(SystemPath, 1, 1, 0, 0));   // literal
                functions.Add("version", new FuncEntry(Version, 1, 1, 0));     // don't check first para
            }
        }

        static Dictionary<string, FuncEntry> functions = null;

        protected override FuncEntry FindFunction(string name)
        {
            return functions.ContainsKey(name) ? functions[name] : base.FindFunction(name);
        }

        #region Macro Functions

        protected new bool SystemPath(out string output)
        {
            string id = paras[0].value;

            if (id.Equals("EDDAPPFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = EDDConfig.Options.AppDataDirectory;
            else if (id.Equals("EDDACTIONSFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = Path.Combine(EDDConfig.Options.AppDataDirectory, "Actions");
            else if (id.Equals("EDDVIDEOFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = Path.Combine(EDDConfig.Options.AppDataDirectory, "Videos");
            else if (id.Equals("EDDSOUNDFOLDER", StringComparison.InvariantCultureIgnoreCase))
                output = Path.Combine(EDDConfig.Options.AppDataDirectory, "Sounds");
            else
                return base.SystemPath(out output);

            return true;
        }

        protected bool Version(out string output)
        {
            int[] edversion = System.Reflection.Assembly.GetExecutingAssembly().GetVersion();

            int para;
            if (paras[0].value.InvariantParse(out para) && para >= 0 && para <= edversion.Length)
            {
                if (para == 0)
                    output = edversion[0] + "." + edversion[1] + "." + edversion[2] + "." + edversion[3];
                else
                    output = edversion[para - 1].ToString(ct);
                return true;
            }
            else
            {
                output = "Parameter number must be between 1 and 4";
                return false;
            }
        }


        protected override bool VerifyFileAccess(string file, FileMode fm)
        {
            if (fm != FileMode.Open)
            {
                string folder = Path.GetDirectoryName(file);
                string actionfolderperms = DB.SQLiteConnectionUser.GetSettingString("ActionFolderPerms", "");

                if (!actionfolderperms.Contains(folder + ";"))
                {
                    bool ok = ExtendedControls.MessageBoxTheme.Show("Warning - This programy is attempting to write to folder" + Environment.NewLine + Environment.NewLine +
                                                         folder + Environment.NewLine + Environment.NewLine +
                                                         "with file " + Path.GetFileName(file) + Environment.NewLine + Environment.NewLine +
                                                           "!!! Verify you are happy for the program to write to ANY files in that folder!!!",
                                                           "WARNING - WRITE FILE ACCESS REQUESTED",
                                                        System.Windows.Forms.MessageBoxButtons.YesNo,
                                                        System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes;

                    if (ok)
                    {
                        DB.SQLiteConnectionUser.PutSettingString("ActionFolderPerms", actionfolderperms + folder + ";");
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;
            }
            else
                return true;
        }


    #endregion
}
}
