/*
 * Copyright © 2019 EDDiscovery development team
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
using System.Drawing;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public static class ScanDisplayForm
    {
        public static void ShowScanOrMarketForm(Form parent, Object tag, bool checkedsm, HistoryList hl)     // tag can be a Isystem or an He.. output depends on it.
        {
            Type ty = typeof(ScanDisplayForm);

            if (tag == null)
                return;

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            int width = Math.Max(250, parent.Width * 4 / 5);
            int height = 800;

            HistoryEntry he = tag as HistoryEntry;                          // is tag HE?
            ISystem sys = he != null ? he.System : tag as ISystem;          // if so, sys is he.system, else its a direct sys
            ScanDisplayControl sd = null;
            string title = "System".Tx(ty, "Sys") + ": " + sys.Name;

            if (he != null && (he.EntryType == JournalTypeEnum.Market || he.EntryType == JournalTypeEnum.EDDCommodityPrices))  // station data..
            {
                JournalCommodityPricesBase jm = he.journalEntry as JournalCommodityPricesBase;
                jm.FillInformation(out string info, out string detailed,1);

                f.Add(new ExtendedControls.ConfigurableForm.Entry("RTB", typeof(ExtendedControls.ExtRichTextBox), detailed, new Point(0, 40), new Size(width - 20, height - 85), null));

                title += ", " +"Station".Tx(ty) + ": " + jm.Station;
            }
            else
            {      
                sd = new ScanDisplayControl();
                sd.CheckEDSM =checkedsm;
                sd.ShowMoons = sd.ShowMaterials = sd.ShowOverlays = true;
                sd.SetSize(48);
                sd.Size = new Size(width - 20, 1024);
                sd.DrawSystem(sys, null, hl);

                height = Math.Min(800, Math.Max(400, sd.DisplayAreaUsed.Y)) + 100;

                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", null, null, new Point(0, 40), new Size(width - 20, height - 85), null) { control = sd });
            }

            f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".Tx(), new Point(width - 20 - 80, height - 40), new Size(80, 24), ""));

            f.Trigger += (dialogname, controlname, ttag) =>
            {
                if (controlname == "OK" || controlname == "Cancel")
                    f.Close();
            };

            f.Init(parent.Icon, new Size(width, height), new Point(-999, -999), title, null, null);

            f.Show(parent);
        }
    }
}
