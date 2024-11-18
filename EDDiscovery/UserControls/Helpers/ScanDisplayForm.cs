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
        public static async void ShowScanOrMarketForm(Form parent, Object tag, HistoryList hl, float opacity = 1, Color? keycolour = null, 
                                WebExternalDataLookup? forcedlookup = null)     
        {
            if (tag == null)
                return;

            ExtendedControls.ConfigurableForm form = new ExtendedControls.ConfigurableForm();

            Screen scr = Screen.FromPoint(parent.PointToScreen(new Point(0,0)));
            Size maincontentsize = new Size(scr.WorkingArea.Width * 5 / 8, scr.WorkingArea.Height * 5 / 8);
            int topmargin = 28+28;

            HistoryEntry he = tag as HistoryEntry;                          // is tag HE?
            ISystem sys = he != null ? he.System : tag as ISystem;          // if so, sys is he.system, else its a direct sys
            string title = "System".T(EDTx.ScanDisplayForm_Sys) + $": {sys.Name} @ {sys.X:N3}, {sys.Y:N3}, {sys.Z:N3}";

            AutoScaleMode asm = AutoScaleMode.Font;

            if (he != null && (he.EntryType == JournalTypeEnum.Market || he.EntryType == JournalTypeEnum.EDDCommodityPrices))  // station data..
            {
                form.Add(new ExtendedControls.ConfigurableEntryList.Entry("Content", typeof(ExtendedControls.ExtRichTextBox), he.GetDetailed()??"", new Point(0, topmargin), maincontentsize, null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom, MinimumSize = new Size(200, 200) });

                JournalCommodityPricesBase jm = he.journalEntry as JournalCommodityPricesBase;
                title += ", " +"Station".T(EDTx.ScanDisplayForm_Station) + ": " + jm.Station;
            }
            else
            {
                EliteDangerousCore.DB.UserDatabaseSettingsSaver db = new EliteDangerousCore.DB.UserDatabaseSettingsSaver(EliteDangerousCore.DB.UserDatabase.Instance, "ScanDisplayFormCommon_");

                ScanDisplayBodyFiltersButton filterbut = new ScanDisplayBodyFiltersButton();
                ScanDisplayConfigureButton configbut = new ScanDisplayConfigureButton();
                EDSMSpanshButton edsmSpanshButton = null;
                ScanDisplayUserControl sd = new ScanDisplayUserControl();

                StarScan.SystemNode nodedata = null;

                if (forcedlookup == null)   // if we not forced into the mode
                {
                    edsmSpanshButton = new EDSMSpanshButton();
                    edsmSpanshButton.Init(db, "EDSMSpansh", "");
                    edsmSpanshButton.ValueChanged += (s, e) =>
                    {
                        nodedata = hl.StarScan.FindSystemSynchronous(sys, edsmSpanshButton.WebLookup);    // look up system, unfort must be sync due to limitations in c#
                        sd.SystemDisplay.ShowWebBodies = edsmSpanshButton.WebLookup != WebExternalDataLookup.None;
                        sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                    };
                }

                sd.SystemDisplay.ShowWebBodies = (forcedlookup.HasValue ? forcedlookup.Value : edsmSpanshButton.WebLookup) != WebExternalDataLookup.None;
                int selsize = (int)(ExtendedControls.Theme.Current.GetFont.Height / 10.0f * 48.0f);
                sd.SystemDisplay.SetSize( selsize );
                sd.Size = maincontentsize;

                nodedata = await hl.StarScan.FindSystemAsync(sys, forcedlookup.HasValue ? forcedlookup.Value : edsmSpanshButton.WebLookup);    // look up system async

                filterbut.Init(db, "BodyFilter");
                filterbut.Image = EDDiscovery.Icons.Controls.EventFilter;
                filterbut.ValueChanged += (s, e) =>
                {
                    sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                };

                configbut.Init(db, "DisplayFilter");
                configbut.Image = EDDiscovery.Icons.Controls.DisplayFilters;
                configbut.ValueChanged += (s, e) =>
                {
                    configbut.ApplyDisplayFilters(sd);
                    sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                };

                sd.BackColor = ExtendedControls.Theme.Current.Form;
                configbut.ApplyDisplayFilters(sd);
                sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);

                asm = AutoScaleMode.None;   // because we are using a picture box, it does not autoscale, so we can't use that logic on it.

                form.Add(new ExtendedControls.ConfigurableEntryList.Entry(filterbut, "Body", null, new Point(4, 28), new Size(28, 28), null));
                form.Add(new ExtendedControls.ConfigurableEntryList.Entry(configbut, "Con",  null, new Point(4 + 28 + 8, 28), new Size(28, 28), null));
                if ( !forcedlookup.HasValue)
                    form.Add(new ExtendedControls.ConfigurableEntryList.Entry(edsmSpanshButton, "edsm", null, new Point(4 + 28 + 8 + 28 + 8, 28), new Size(28, 28), null));
                form.Add(new ExtendedControls.ConfigurableEntryList.Entry(sd, "Content", null, new Point(0, topmargin), maincontentsize, null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom, MinimumSize = new Size(100, 40) });

                form.Trigger += (dialogname, controlname, ttag) =>
                {
                    if (controlname == "Resize")
                    {
                        //System.Diagnostics.Debug.WriteLine($"SDF Resize sd size {sd.Size}");
                        sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
                    }
                };
            }

            form.AllowResize = true;
            
            // removed OK for now.. not needed
            //form.AddOK(new Point(maincontentsize.Width - 120, topmargin + maincontentsize.Height + 10), anchor: AnchorStyles.Right | AnchorStyles.Bottom );

            form.Trigger += (dialogname, controlname, ttag) =>
            {
                if (controlname == "OK")
                    form.ReturnResult(DialogResult.OK);
                else if (controlname == "Close")
                    form.ReturnResult(DialogResult.Cancel);
            };

            form.InitCentred( parent, parent.Icon, title, null, null, asm , closeicon:true, minsize:new Size(200,200));


            if (opacity < 1)
            {
                form.Opacity = opacity;
                form.BackColor = keycolour.Value;
                form.TransparencyKey = keycolour.Value;
            }

            form.Show(parent);
        }
    }
}
