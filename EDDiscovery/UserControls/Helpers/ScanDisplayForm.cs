/*
 * Copyright © 2019-2023 EDDiscovery development team
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
        public static async void ShowScanOrMarketForm(Form parent, Object tag, WebExternalDataLookup lookup, HistoryList hl, float opacity = 1, Color? keycolour = null)     
        {
            if (tag == null)
                return;

            ExtendedControls.ConfigurableForm form = new ExtendedControls.ConfigurableForm();

            Size infosize = parent.SizeWithinScreen(new Size(parent.Width * 6 / 8, parent.Height * 6 / 8), 128, 128 + 100);        // go for this, but allow this around window
            int topmargin = 28+28;

            HistoryEntry he = tag as HistoryEntry;                          // is tag HE?
            ISystem sys = he != null ? he.System : tag as ISystem;          // if so, sys is he.system, else its a direct sys
            ScanDisplayUserControl sd = null;
            string title = "System".T(EDTx.ScanDisplayForm_Sys) + ": " + sys.Name;

            AutoScaleMode asm = AutoScaleMode.Font;

            if (he != null && (he.EntryType == JournalTypeEnum.Market || he.EntryType == JournalTypeEnum.EDDCommodityPrices))  // station data..
            {
                he.FillInformation(out string info, out string detailed);

                form.Add(new ExtendedControls.ConfigurableForm.Entry("RTB", typeof(ExtendedControls.ExtRichTextBox), detailed, new Point(0, topmargin), infosize, null));

                JournalCommodityPricesBase jm = he.journalEntry as JournalCommodityPricesBase;
                title += ", " +"Station".T(EDTx.ScanDisplayForm_Station) + ": " + jm.Station;
            }
            else
            {      
                sd = new ScanDisplayUserControl();
                sd.SystemDisplay.ShowWebBodies = lookup != WebExternalDataLookup.None;
                int selsize = (int)(ExtendedControls.Theme.Current.GetFont.Height / 10.0f * 48.0f);
                sd.SystemDisplay.SetSize( selsize );
                sd.Size = infosize;

                StarScan.SystemNode data = await hl.StarScan.FindSystemAsync(sys, lookup);    // look up system async

                if (data != null)
                {
                    long value = data.ScanValue(lookup != WebExternalDataLookup.None);
                    title += " ~ " + value.ToString("N0") + " cr";
                }

                DBSettingsSaver db = new DBSettingsSaver();

                ScanDisplayBodyFiltersButton filterbut = new ScanDisplayBodyFiltersButton();
                filterbut.Init(db, "BodyFilter");
                filterbut.Image = EDDiscovery.Icons.Controls.EventFilter;
                filterbut.ValueChanged += (s, e) =>
                {
                    sd.DrawSystem(data, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                };

                ScanDisplayConfigureButton configbut = new ScanDisplayConfigureButton();
                configbut.Init(db, "DisplayFilter");
                configbut.Image = EDDiscovery.Icons.Controls.DisplayFilters;
                configbut.ValueChanged += (s, e) =>
                {
                    configbut.ApplyDisplayFilters(sd);
                    sd.DrawSystem(data, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                };

                sd.BackColor = ExtendedControls.Theme.Current.Form;
                sd.DrawSystem(data, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);

                int wastedh = infosize.Height - sd.SystemDisplay.DisplayAreaUsed.Y - 10 - 40;
                if (wastedh > 0)
                    infosize.Height -= wastedh;

                asm = AutoScaleMode.None;   // because we are using a picture box, it does not autoscale, so we can't use that logic on it.

                form.Add(new ExtendedControls.ConfigurableForm.Entry("Body", null, null, new Point(4, 28), new Size(28, 28), null) { control = filterbut });
                form.Add(new ExtendedControls.ConfigurableForm.Entry("Con", null, null, new Point(4+28+4, 28), new Size(28, 28), null) { control = configbut });
                form.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", null, null, new Point(0, topmargin), infosize, null) { control = sd });
            }

            form.AddOK(new Point(infosize.Width - 120, topmargin + infosize.Height + 10));

            form.Trigger += (dialogname, controlname, ttag) =>
            {
                if (controlname == "OK")
                    form.ReturnResult(DialogResult.OK);
                else if (controlname == "Close")
                    form.ReturnResult(DialogResult.Cancel);
            };

            form.InitCentred( parent, parent.Icon, title, null, null, asm , closeicon:true);

            if (opacity < 1)
            {
                form.Opacity = opacity;
                form.BackColor = keycolour.Value;
                form.TransparencyKey = keycolour.Value;
            }

            form.Show(parent);
        }

        // class needed for buttons for save/restore - global for all instances
        public class DBSettingsSaver : UserControlCommonBase.ISettingsSaver     
        {
            const string root = "ScanDisplayFormCommon_";
            public T GetSetting<T>(string key, T defaultvalue)
            {
                return EliteDangerousCore.DB.UserDatabase.Instance.GetSetting(root + key, defaultvalue);
            }

            public bool PutSetting<T>(string key, T value)
            {
                return EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(root + key, value);
            }
        }
    }
}
