/*
 * Copyright © 2016 EDDiscovery development team
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

using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    static public class CVSHelpers
    {
        static public bool WriteGrid( this BaseUtils.CSVWriteGrid grid , string file, bool autoopen, Form parent )
        {
            if (grid.WriteCSV(file))
            {
                if (autoopen)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(file);
                        return true;
                    }
                    catch
                    {
                        FailedToOpen(parent, file);
                    }
                }
                else
                    return true;
            }
            else
                WriteFailed(parent, file);

            return false;
        }

        static public void FailedToOpen(Form parent, string file)
        {
            ExtendedControls.MessageBoxTheme.Show(parent, "Failed to open ".T(EDTx.CSV_Helpers_OpenFailed) + file, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        static public void WriteFailed(Form parent, string file)
        {
            ExtendedControls.MessageBoxTheme.Show(parent, "Failed to write to ".T(EDTx.CSV_Helpers_WriteFailed) + file, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}