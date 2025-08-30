/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
 * 
 */
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialTrader : UserControlCommonBase
    {
        private string dbTraderType = "TraderType";
        private string dbTrades = "Trades";
        private string dbSplitter = "Splitter";
        private string dbLatest = "Latest";

        private Color orange = Color.FromArgb(255, 184, 85, 8);

        private uint? last_mcl = null;

        public class ElementTrade
        {
            public MaterialCommodityMicroResourceType.MaterialGroupType type;    // for display purposes, group type
            public int level;                                       // and level

            public MaterialCommodityMicroResourceType element;               // element of entry, or for trades, element received
            public int offer;
            public int receive;
            public MaterialCommodityMicroResourceType fromelement;           // only for trades, the element that offered up

            public override string ToString()                   // serialise to string
            {
                return fromelement?.FDName + "," + element?.FDName + "," + offer.ToStringInvariant() + "," + receive.ToStringInvariant();
            }

            public bool FromString(string s)                    // serialise from string
            {
                string[] parts = s.Split(',');
                if (parts.Length == 4)
                {
                    fromelement = MaterialCommodityMicroResourceType.GetByFDName(parts[0]);
                    element = MaterialCommodityMicroResourceType.GetByFDName(parts[1]);
                    return fromelement != null && element != null && parts[2].InvariantParse(out offer) && parts[3].InvariantParse(out receive);
                }
                else
                    return false;
            }
        }

        ElementTrade selected = null;         // selected entry to trade to

        List<ElementTrade> tradelist = new List<ElementTrade>();           // trades established.

        #region Init

        public UserControlMaterialTrader()
        {
            InitializeComponent();

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuStrip);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip,this);

            DBBaseName = "MaterialTrade";
        }

        protected override void Init()
        {

            dataGridViewTrades.MakeDoubleBuffered();
            dataGridViewTrades.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            extComboBoxTraderType.Items.AddRange(new string[] { "Raw".Tx(), "Encoded".Tx(), "Manufactured".Tx()});
            extComboBoxTraderType.SelectedIndex = GetSetting(dbTraderType, 0);
            extComboBoxTraderType.SelectedIndexChanged += ExtComboBoxTraderType_SelectedIndexChanged;

            checkBoxCursorToTop.Checked = GetSetting(dbLatest, true);
            checkBoxCursorToTop.CheckedChanged += CheckBoxCursorToTop_CheckedChanged;

            string[] strades = GetSetting(dbTrades, "").Split(';');
            foreach (var t in strades)      // deserialise the trades and populate the trades list
            {
                ElementTrade et = new ElementTrade();
                if (et.FromString(t))
                    tradelist.Add(et);
            }

            splitContainer.SplitterDistance(GetSetting(dbSplitter, 0.75));

            DiscoveryForm.OnThemeChanged += Discoveryform_OnThemeChanged;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        protected override void LoadLayout()
        {
            dataGridViewTrades.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewTrades);
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewTrades);
            PutSetting(dbSplitter, splitContainer.GetSplitterDistance());

            DiscoveryForm.OnThemeChanged -= Discoveryform_OnThemeChanged;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }


        private void Discoveryform_OnThemeChanged()
        {
            DisplayTradeSelection();
            DisplayTradeList();
        }

        #endregion

        #region Display

        protected override void InitialDisplay()
        {
            if (checkBoxCursorToTop.Checked)
            {
                last_mcl = DiscoveryForm.History.GetLast?.MaterialCommodity;
                DisplayTradeSelection();
                DisplayTradeList();
            }
            else
            {
                RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
            }
        }

        private void Discoveryform_OnHistoryChange()
        {
            InitialDisplay();
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (checkBoxCursorToTop.Checked == false && last_mcl != he.MaterialCommodity)
            {
                last_mcl = he.MaterialCommodity;
                DisplayTradeSelection();
                DisplayTradeList();
            }
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if ( checkBoxCursorToTop.Checked )
            { 
                if (he.journalEntry is IMaterialJournalEntry)
                {
                    last_mcl = he.MaterialCommodity;
                    DisplayTradeSelection();
                    DisplayTradeList();
                }
            }
        }

        private void DisplayTradeSelection(MaterialCommodityMicroResourceType highlight = null)  // last_he and current_mcl can be null
        {
            int sel = extComboBoxTraderType.SelectedIndex;

            var mcl = new List<Tuple<MaterialCommodityMicroResourceType.MaterialGroupType, MaterialCommodityMicroResourceType[]>>();      // list of groups vs data
            Dictionary<MaterialCommodityMicroResourceType.MaterialGroupType, string> mattxgroupnames = new Dictionary<MaterialCommodityMicroResourceType.MaterialGroupType, string>(); // translated names of groups

            foreach ( var t in Enum.GetValues(typeof(MaterialCommodityMicroResourceType.MaterialGroupType)))     // relies on MCD being in order
            {
                var matgroup = (MaterialCommodityMicroResourceType.MaterialGroupType)t;
                bool ok = (sel == 0 && matgroup >= MaterialCommodityMicroResourceType.MaterialGroupType.RawCategory1 && matgroup <= MaterialCommodityMicroResourceType.MaterialGroupType.RawCategory7);
                ok |= (sel == 1 && matgroup >= MaterialCommodityMicroResourceType.MaterialGroupType.EncodedEmissionData && matgroup <= MaterialCommodityMicroResourceType.MaterialGroupType.EncodedFirmware);
                ok |= (sel == 2 && matgroup >= MaterialCommodityMicroResourceType.MaterialGroupType.ManufacturedChemical && matgroup <= MaterialCommodityMicroResourceType.MaterialGroupType.ManufacturedAlloys);
                if ( ok )
                {
                    var list = MaterialCommodityMicroResourceType.Get(x => x.MaterialGroup == matgroup);
                  //  System.Diagnostics.Debug.WriteLine($"\nMTrader {matgroup} = {String.Join(",",list.Select(x=>x.FDName))}");
                    mcl.Add(new Tuple<MaterialCommodityMicroResourceType.MaterialGroupType, MaterialCommodityMicroResourceType[]>(matgroup,list));
                    mattxgroupnames[matgroup] = list[0].TranslatedMaterialGroup;
                }
            }

            var curmcl = last_mcl != null ? DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value) : null;       // get mcl at last_mcl position. May be null if we don't have any list

            Font titlefont = ExtendedControls.Theme.Current.GetFont;
            Font badgefont = ExtendedControls.Theme.Current.GetScaledFont(16f / 12f, max:21);

            const int hbadgemargin = 20;
            const int vbadgemargin = 12;

            extPictureTrades.ClearImageList();

            int vpos = 0;
            int maxhpos = 0;

            foreach (var t in mcl)      // for each type of material/commd
            {
                int hpos = 0;
                int nextvpos = vpos;
                int lvl = 1;

                extPictureTrades.AddOwnerDraw((g, ie) =>
                {
                    int tlen;
                    using (Brush b = new SolidBrush(orange))
                    {
                        string s = mattxgroupnames[t.Item1];
                        s = s.Substring(s.IndexOf(" ") + 1);
                        tlen = (int)(g.MeasureString(s, titlefont).Width+2);
                        g.DrawString(s, titlefont, b, new Point(ie.Location.Left, ie.Location.Top));
                    }

                    using (Pen p = new Pen(orange))
                    {
                        g.DrawLine(p, new Point(tlen, ie.Location.Top + titlefont.Height / 2), new Point(maxhpos, ie.Location.Top + titlefont.Height / 2));
                    }

                }, new Rectangle(0, vpos, 2000, 24), t.Item1.ToString());

                vpos += titlefont.Height + 6;

                string backname = "encodedbackground";
                if (t.Item1.ToString().Contains("Manu"))
                    backname = "manubackground";
                else if (t.Item1.ToString().Contains("Raw"))
                    backname = "rawbackground";

                Bitmap background = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader." + backname ) as Bitmap;

                foreach ( var mat in t.Item2 )
                {
                    int offer = 0, receive = 0;
                    string name = mat.TranslatedName;

                    int mattotal = last_mcl == null ? int.MinValue : curmcl.Find(x=>x.Details.FDName == mat.FDName)?.Count ?? 0;    // find mcl in material list if there, and its count

                    if (mattotal >= 0)                                                  // if we have an he, adjust the totals by the trades
                    {
                        foreach (var trade in tradelist)
                        {
                            if (trade.fromelement.FDName == mat.FDName)
                                mattotal -= trade.offer;                              // may go negative if over offered
                            if (trade.element.FDName == mat.FDName)
                                mattotal += trade.receive;
                        }
                    }

                    Color wash = Color.Transparent;

                    if (highlight?.FDName == mat.FDName)
                        wash = Color.FromArgb(80, 0, 75, 0);

                    if (selected != null)
                    {
                        bool difflevel = selected.type != t.Item1;

                        if (lvl < selected.level)
                        {
                            offer = (int)Math.Pow(6, (selected.level - lvl) + (difflevel ? 1 : 0));
                            receive = 1;
                        }
                        else if (lvl > selected.level)
                        {
                            offer = difflevel ? 6 : 1;
                            receive = (int)Math.Pow(3, (lvl - selected.level) );
                            while( offer % 3 == 0 )
                            {
                                offer /= 3;
                                receive /= 3;
                            }
                        }
                        else if (selected.type == t.Item1)
                        {
                            name = "Cancel";
                            wash = Color.FromArgb(80, 0, 32, 0);
                        }
                        else
                        {
                            offer = 6;
                            receive = 1;

                        }

                        if (offer > mattotal)
                        {
                            wash = Color.FromArgb(80, 128, 0, 0);
                        }
                    }

                    Bitmap bmp = DrawBadge(background, badgefont, offer, receive, lvl, name, mattotal, wash);

                    var ie = extPictureTrades.AddImage(new Rectangle(hpos, vpos, background.Width, background.Height), bmp, imgowned: true);
                    ie.Tag = new ElementTrade { type = t.Item1, element = mat, level = lvl, offer = offer, receive = receive };

                    maxhpos = Math.Max(maxhpos, hpos + bmp.Width);
                    hpos += bmp.Width + hbadgemargin;
                    nextvpos = Math.Max(nextvpos, vpos+ bmp.Height + vbadgemargin);
                    lvl++;
                }

                vpos = nextvpos;
            }

            extPictureTrades.Render();
        }

        Bitmap DrawBadge(Bitmap background , Font displayfont, int offer, int receive , int level , string matname, int mattotal, Color wash)
        {

            Bitmap bmp = background.Clone() as Bitmap;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Brush b = new SolidBrush(orange))
                {
                    if (offer > 0 && receive > 0)
                    {
                        Bitmap arrow = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.materialexchange") as Bitmap;

                        g.DrawImage(arrow, new Point(8, 8));

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        g.DrawString(offer.ToStringInvariant(), displayfont, b, new Point(8, 2));
                        g.DrawString(receive.ToStringInvariant(), displayfont, b, new Point(8, 32));
                    }

                    using (StringFormat fmt = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(matname, displayfont, b, new Rectangle(0, 100, bmp.Width, 45), fmt);

                        if ( mattotal != int.MinValue )
                            g.DrawString(mattotal.ToStringInvariant(), displayfont, b, new Rectangle(0, 75, bmp.Width, 20), fmt);
                    }

                }

                if ( level > 0)
                {
                    Bitmap petal = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.petal" + level.ToStringInvariant()) as Bitmap;
                    g.DrawImage(petal, new Point(bmp.Width/2-petal.Width/2,42-petal.Height/2));
                }

                if ( !wash.IsFullyTransparent() )
                {
                    using (Brush b = new SolidBrush(wash))
                    {
                        g.FillRectangle(b, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    }
                }
            }

            return bmp;
        }

        void DisplayTradeList()
        {
            dataGridViewTrades.Rows.Clear();

            if (tradelist.Count > 0)        
            {
                // last_mcl can be null
                List<MaterialCommodityMicroResource> mcl = last_mcl == null ? null : DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value);

                var totals = mcl == null ? null : MaterialCommoditiesRecipe.TotalList(mcl);                  // start with totals present, null if we don't have an mcl

                foreach (var trade in tradelist)
                {
                    var rw = dataGridViewTrades.RowTemplate.Clone() as DataGridViewRow;

                    if (mcl != null)
                    {
                        if (!totals.ContainsKey(trade.fromelement.FDName))      // make sure they are both there, so we don't crash.  the from should always be here
                            totals[trade.fromelement.FDName] = 0;
                        if (!totals.ContainsKey(trade.element.FDName))          // the to, if 0, will not
                            totals[trade.element.FDName] = 0;

                        totals[trade.fromelement.FDName] -= trade.offer;

                        if (totals[trade.fromelement.FDName] >= 0)
                        {
                            totals[trade.element.FDName] += trade.receive;

                            rw.CreateCells(dataGridViewTrades, trade.fromelement.TranslatedName, trade.offer.ToString(), totals[trade.fromelement.FDName].ToString(), trade.element.TranslatedName, trade.receive.ToString(), totals[trade.element.FDName].ToString());
                        }
                        else
                        {
                            rw.CreateCells(dataGridViewTrades, trade.fromelement.TranslatedName, trade.offer.ToString(), "- !!!", trade.element.TranslatedName, trade.receive.ToString(), "-");
                        }
                    }
                    else
                    {
                        rw.CreateCells(dataGridViewTrades, trade.fromelement.TranslatedName, trade.offer.ToString(), "-", trade.element.TranslatedName, trade.receive.ToString(), "-");
                    }

                    dataGridViewTrades.Rows.Add(rw);
                }

            }
        }

        #endregion

        private void buttonClear_Click(object sender, EventArgs e)
        {
            tradelist.Clear();
            StoreTrades();
            DisplayTradeSelection();
            DisplayTradeList();
        }

        private void ExtComboBoxTraderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbTraderType, extComboBoxTraderType.SelectedIndex );
            selected = null;
            DisplayTradeSelection();
        }

        private void extPictureTrades_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null && tag is ElementTrade && last_mcl != null ) // must be an element, with a tag, must have a current mcl
            {
                ElementTrade current = (ElementTrade)tag;
                System.Diagnostics.Debug.WriteLine("Clicked on " + current.type + " " + current.element.TranslatedName);

                if (selected != null)
                {
                    List<MaterialCommodityMicroResource> mcl = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value);
                    int currenttotal = mcl.Find(x=>x.Details==current.element)?.Count ?? 0;   // current mat total. If not there, its zero
                    foreach (var trade in tradelist)
                    {
                        if (trade.fromelement.FDName == current.element.FDName)
                            currenttotal -= trade.offer;                              // may go negative if over offered
                        if (trade.element.FDName == current.element.FDName)
                            currenttotal += trade.receive;
                    }

                    if (selected.element.FDName == current.element.FDName)        // clicked on same.. deselect
                    {
                        selected = null;
                    }
                    else if ( currenttotal >= current.offer )                       // if we have enough for at least 1 trade
                    {
                        DisplayTradeSelection(current.element);

                        ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

                        int width = 250;
                        int margin = 20;

                        var butl = new ExtendedControls.ExtButton();
                        butl.Image = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.LeftArrow");
                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry(butl, "less", "", new Point(margin, 64), new Size(32, 32), null));
                        var butr = new ExtendedControls.ExtButton();
                        butr.Image = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.RightArrow");
                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry(butr, "more", "", new Point(width - margin - 32, 64), new Size(32, 32), null));

                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("olabel", typeof(Label), "Offer".Tx(), new Point(margin, 30), new Size(width - margin * 2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("offer", typeof(Label), "0/" + currenttotal.ToStringInvariant(), new Point(width / 2 - 12, 50), new Size(width / 2 - 20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        var bar = new PictureBox();
                        bar.SizeMode = PictureBoxSizeMode.StretchImage;
                        bar.Image = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.TraderBar");
                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry(bar, "bar", "", new Point(width / 2 - 32, 70), new Size(64, 16), null));

                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("receive", typeof(Label), "0", new Point(width / 2 - 12, 90), new Size(width / 2 - 20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        f.Add(new ExtendedControls.ConfigurableEntryList.Entry("rlabel", typeof(Label), "Receive".Tx(), new Point(margin, 110), new Size(width - margin * 2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.AddOK(new Point(width - margin - 80, 150), "Press to Accept".Tx());
                        f.AddCancel(new Point(margin, 150), "Press to Cancel".Tx());

                        int currentoffer = 0;
                        int currentreceive = 0;

                        f.Trigger += (dialogname, controlname, xtag) =>
                        {
                            if (controlname == "OK")
                            {
                                f.ReturnResult(DialogResult.OK);
                            }
                            else if (controlname == "Cancel" || controlname == "Close")
                            {
                                f.ReturnResult(DialogResult.Cancel);
                            }
                            else if ( controlname == "less" || controlname == "more" )
                            {
                                if (controlname == "less")
                                {
                                    if (currentoffer > 0)
                                    {
                                        currentoffer -= current.offer;
                                        currentreceive -= current.receive;
                                    }
                                }
                                else 
                                {
                                    int newoffer = currentoffer + current.offer;
                                    if (newoffer <= currenttotal)
                                    {
                                        currentoffer = newoffer;
                                        currentreceive += current.receive;
                                    }
                                }

                                f.GetControl<Label>("offer").Text = currentoffer.ToStringInvariant() + "/" + currenttotal.ToStringInvariant();
                                f.GetControl<Label>("receive").Text = currentreceive.ToStringInvariant();
                            }
                        };

                        f.RightMargin = margin;

                        f.InitCentred(this.FindForm(), this.FindForm().Icon, " ", closeicon: true);

                        DialogResult res = f.ShowDialog();

                        if (res == DialogResult.OK)
                        {
                            ElementTrade t = new ElementTrade() { element = selected.element, fromelement = current.element, offer = currentoffer, receive = currentreceive };
                            tradelist.Add(t);
                            selected = null;

                            StoreTrades();

                            DisplayTradeList();
                        }

                        DisplayTradeSelection();
                    }
                }
                else
                {
                    selected = current;
                }

                DisplayTradeSelection();
            }
        }

        private void clearTradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewTrades.RightClickRowValid)
            {
                tradelist.RemoveAt(dataGridViewTrades.RightClickRow);
                StoreTrades();
                DisplayTradeSelection();
                DisplayTradeList();
            }
        }

        private void StoreTrades()
        {
            string strades = "";
            foreach (var td in tradelist)
                strades = strades.AppendPrePad(td.ToString(), ";");

            PutSetting(dbTrades, strades);
        }


        private void CheckBoxCursorToTop_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbLatest, checkBoxCursorToTop.Checked);
            InitialDisplay();
        }

    }
}
