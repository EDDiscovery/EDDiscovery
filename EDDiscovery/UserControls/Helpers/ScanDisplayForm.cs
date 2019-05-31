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

            Size infosize = parent.SizeWithinScreen(new Size(parent.Width * 2 / 4,parent.Height * 2 / 4), 128, 128+100);        // go for this, but allow this around window
            int topmargin = 40;

            HistoryEntry he = tag as HistoryEntry;                          // is tag HE?
            ISystem sys = he != null ? he.System : tag as ISystem;          // if so, sys is he.system, else its a direct sys
            ScanDisplayUserControl sd = null;
            string title = "System".Tx(ty, "Sys") + ": " + sys.Name;

            AutoScaleMode asm = AutoScaleMode.Font;

            if (he != null && (he.EntryType == JournalTypeEnum.Market || he.EntryType == JournalTypeEnum.EDDCommodityPrices))  // station data..
            {
                JournalCommodityPricesBase jm = he.journalEntry as JournalCommodityPricesBase;
                jm.FillInformation(out string info, out string detailed,1);

                f.Add(new ExtendedControls.ConfigurableForm.Entry("RTB", typeof(ExtendedControls.ExtRichTextBox), detailed, new Point(0, topmargin), infosize, null));

                title += ", " +"Station".Tx(ty) + ": " + jm.Station;
            }
            else
            {      
                sd = new ScanDisplayUserControl();
                sd.CheckEDSM =checkedsm;
                sd.ShowMoons = sd.ShowMaterials = sd.ShowOverlays = true;
                int selsize = (int)(EDDTheme.Instance.GetFont.Height / 16.0f * 48.0f);
                sd.SetSize( selsize );
                sd.Size = infosize;

                StarScan.SystemNode data = sd.FindSystem(sys, hl);
                if ( data != null )
                {
                    long value = data.ScanValue(sd.CheckEDSM);
                    title += " ~ " + value.ToString("N0") + " cr";
                }

                sd.BackColor = EDDTheme.Instance.Form;
                sd.DrawSystem( data, null , hl);

                infosize = new Size(Math.Max(400, sd.DisplayAreaUsed.X) , Math.Max(200, sd.DisplayAreaUsed.Y));

                asm = AutoScaleMode.None;   // because we are using a picture box, it does not autoscale, so we can't use that logic on it.

                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", null, null, new Point(0, topmargin), infosize, null) { control = sd });
            }

            f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".Tx(), new Point(infosize.Width - 120, topmargin + infosize.Height + 10), new Size(100, 24), ""));

            f.Trigger += (dialogname, controlname, ttag) =>
            {
                if (controlname == "OK" || controlname == "Cancel")
                    f.Close();
            };

            f.InitCentred( parent, parent.Icon, title, null, null, asm);

            f.Show(parent);
        }
    }
}
