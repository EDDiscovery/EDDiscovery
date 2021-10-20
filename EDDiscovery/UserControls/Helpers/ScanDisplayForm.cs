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
        // tag can be a Isystem or an He.. output depends on it.
        public static async void ShowScanOrMarketForm(Form parent, Object tag, bool checkedsm, HistoryList hl, float opacity = 1, Color? keycolour = null)     
        {
            if (tag == null)
                return;

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            Size infosize = parent.SizeWithinScreen(new Size(parent.Width * 6 / 8, parent.Height * 6 / 8), 128, 128 + 100);        // go for this, but allow this around window
            int topmargin = 40;

            HistoryEntry he = tag as HistoryEntry;                          // is tag HE?
            ISystem sys = he != null ? he.System : tag as ISystem;          // if so, sys is he.system, else its a direct sys
            ScanDisplayUserControl sd = null;
            string title = "System".T(EDTx.ScanDisplayForm_Sys) + ": " + sys.Name;

            AutoScaleMode asm = AutoScaleMode.Font;

            if (he != null && (he.EntryType == JournalTypeEnum.Market || he.EntryType == JournalTypeEnum.EDDCommodityPrices))  // station data..
            {
                he.FillInformation(out string info, out string detailed);

                f.Add(new ExtendedControls.ConfigurableForm.Entry("RTB", typeof(ExtendedControls.ExtRichTextBox), detailed, new Point(0, topmargin), infosize, null));

                JournalCommodityPricesBase jm = he.journalEntry as JournalCommodityPricesBase;
                title += ", " +"Station".T(EDTx.ScanDisplayForm_Station) + ": " + jm.Station;
            }
            else
            {      
                sd = new ScanDisplayUserControl();
                sd.SystemDisplay.ShowEDSMBodies =checkedsm;
                int selsize = (int)(EDDTheme.Instance.GetFont.Height / 10.0f * 48.0f);
                sd.SystemDisplay.SetSize( selsize );
                sd.Size = infosize;

                StarScan.SystemNode data = await hl.StarScan.FindSystemAsync(sys, checkedsm);    // look up system async
                    
                if ( data != null )
                {
                    long value = data.ScanValue(checkedsm);
                    title += " ~ " + value.ToString("N0") + " cr";
                }

                sd.BackColor = EDDTheme.Instance.Form;
                sd.DrawSystem( data, null , hl.MaterialCommoditiesMicroResources.GetLast());

                int wastedh = infosize.Height - sd.SystemDisplay.DisplayAreaUsed.Y - 10 - 40;
                if (wastedh > 0)
                    infosize.Height -= wastedh;

                asm = AutoScaleMode.None;   // because we are using a picture box, it does not autoscale, so we can't use that logic on it.

                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", null, null, new Point(0, topmargin), infosize, null) { control = sd });
            }

            f.AddOK(new Point(infosize.Width - 120, topmargin + infosize.Height + 10));

            f.Trigger += (dialogname, controlname, ttag) =>
            {
                if (controlname == "OK")
                    f.ReturnResult(DialogResult.OK);
                else if (controlname == "Close")
                    f.ReturnResult(DialogResult.Cancel);
            };

            f.InitCentred( parent, parent.Icon, title, null, null, asm , closeicon:true);

            if (opacity < 1)
            {
                f.Opacity = opacity;
                f.BackColor = keycolour.Value;
                f.TransparencyKey = keycolour.Value;
            }

            f.Show(parent);
        }
    }
}
