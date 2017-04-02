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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using EDDiscovery2;
using EDDiscovery2.EDSM;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.Export
{
    public class ExportGrid : ExportBase
    {
        public delegate string GetHeader(int i);            // null if end of header line (null directly if no header line)
        public delegate Object GetCell(int r, int c);       // null is end of cell line.  if called with c=-1, return bool for do you want to continue with the spreadsheet
        public event GetCell onGetCell;
        public event GetHeader onGetHeader;

        public ExportGrid()
        {
        }

        override public bool ToCSV(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    string t;
                    for (int cols = 0; (t = onGetHeader(cols)) != null; cols++)
                    {
                        if (cols > 0)
                            writer.Write(delimiter);
                        writer.Write(MakeValueCsvFriendly(t, false));
                    }

                    writer.WriteLine();

                    for (int r = 0; (bool)onGetCell(r,-1); r++)
                    {
                        int cols;
                        Object o;
                        for (cols = 0; (o = onGetCell(r,cols)) != null; cols++)
                        {
                            if (cols > 0)
                                writer.Write(delimiter);
                            writer.Write(MakeValueCsvFriendly(o, false));
                        }

                        if (cols > 0)  // if wrote something, writeline
                            writer.WriteLine();
                    }
                }

                return true;
            }
            catch (IOException)
            {
                EDDiscovery.Forms.MessageBoxTheme.Show(String.Format("Is file {0} open?", filename), "Export Grid",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
 
    }
}
