﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDDiscovery.EliteDangerous.JournalEvents;
using System.Windows.Forms;
using System.IO;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.Export
{
    class ExportExplorationData : ExportBase
    {
        private const string TITLE = "Export Exploration Data";
        private bool datepopup;

        public ExportExplorationData(bool datepopup)
        {
            this.datepopup = datepopup;

        }

        private List<HistoryEntry> data;
        private List<HistoryEntry> scans;


        public override bool GetData(EDDiscoveryForm _discoveryForm)
        {
            bool datepicked = false;
            var picker = new DateTimePicker();
            if (this.datepopup)
            {
                Button button1 = new Button();
                button1.Text = "OK";
                button1.DialogResult = DialogResult.OK;
                Form f = new Form();
                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.ColumnCount = 2;
                tlp.RowCount = 2;
                tlp.Dock = DockStyle.Fill;
                tlp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                tlp.AutoSize = true;
                f.Name = TITLE;
                tlp.Controls.Add(picker, 0, 0);
                tlp.Controls.Add(button1, 1, 1);
                f.Controls.Add(tlp);
                f.AutoSize = true;
                var result = f.ShowDialog();
                if (result == DialogResult.OK)
                {
                    datepicked = true;
                }
            }

            int count = 0;
            data = HistoryList.FilterByJournalEvent(_discoveryForm.history.ToList(), "Sell Exploration Data", out count);

            scans = HistoryList.FilterByJournalEvent(_discoveryForm.history.ToList(), "Scan", out count);
            if (datepicked)
            {
                data = (from he in data where he.EventTimeUTC >= picker.Value.Date.ToUniversalTime() orderby he.EventTimeUTC descending select he).ToList();
            }
            return true;
        }

        public override bool ToCSV(string filename)
        {
            try
            {

                using (StreamWriter writer = new StreamWriter(filename))
                {
                    if (IncludeHeader)
                    {
                        writer.Write("Time" + delimiter);
                        writer.Write("System" + delimiter);
                        writer.Write("Star type" + delimiter);
                        writer.Write("Planet type" + delimiter);
                        writer.WriteLine();
                    }

                    foreach (HistoryEntry he in data)
                    {
                        JournalSellExplorationData jsed = he.journalEntry as JournalSellExplorationData;
                        if (jsed == null || jsed.Discovered == null)
                            continue;
                        foreach (String system in jsed.Discovered)
                        {
                            writer.Write(MakeValueCsvFriendly(jsed.EventTimeLocal));
                            writer.Write(MakeValueCsvFriendly(system));

                            EDStar star = EDStar.Unknown;
                            EDPlanet planet = EDPlanet.Unknown;

                            foreach (HistoryEntry scanhe in scans)
                            {
                                JournalScan scan = scanhe.journalEntry as JournalScan;
                                if (scan.BodyName.Equals(system, StringComparison.OrdinalIgnoreCase))
                                {
                                    star = scan.StarTypeID;
                                    planet = scan.PlanetTypeID;
                                    break;
                                }
                            }
                            writer.Write(MakeValueCsvFriendly((star != EDStar.Unknown) ? Enum.GetName(typeof(EDStar), star) : ""));
                            writer.Write(MakeValueCsvFriendly((planet != EDPlanet.Unknown) ? Enum.GetName(typeof(EDPlanet), planet) : "",false));
                            writer.WriteLine();
                        }
                    }
                }
                return true;
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), TITLE,
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
