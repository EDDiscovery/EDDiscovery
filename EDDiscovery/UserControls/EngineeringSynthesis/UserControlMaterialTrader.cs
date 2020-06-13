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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialTrader : UserControlCommonBase
    {
        const string PrefixName = "MaterialTrade";

        private string DbColumnSave { get { return (PrefixName + "Grid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbTraderType { get { return DBName(PrefixName, "TraderType"); } }
        private string DbTrades { get { return DBName(PrefixName, "Trades"); } }
        private string DbSplitter { get { return DBName(PrefixName, "Splitter"); } }
        private string DbWordWrap { get { return DBName(PrefixName, "WrapText"); } }
        private string DbLatest { get { return DBName(PrefixName, "Latest"); } }

        private Color orange = Color.FromArgb(255, 184, 85, 8);

        private MaterialCommoditiesList last_mcl = null;

        public class ElementTrade
        {
            public MaterialCommodityData.MaterialGroupType type;    // for display purposes, group type
            public int level;                                       // and level

            public MaterialCommodityData element;               // element of entry, or for trades, element received
            public int offer;
            public int receive;
            public MaterialCommodityData fromelement;           // only for trades, the element that offered up

            public override string ToString()                   // serialise to string
            {
                return fromelement?.FDName + "," + element?.FDName + "," + offer.ToStringInvariant() + "," + receive.ToStringInvariant();
            }

            public bool FromString(string s)                    // serialise from string
            {
                string[] parts = s.Split(',');
                if (parts.Length == 4)
                {
                    fromelement = MaterialCommodityData.GetByFDName(parts[0]);
                    element = MaterialCommodityData.GetByFDName(parts[1]);
                    return fromelement != null && element != null && parts[2].InvariantParse(out offer) && parts[3].InvariantParse(out receive);
                }
                else
                    return false;
            }
        }

        ElementTrade selected = null;         // selected entry to trade to

        List<ElementTrade> trades = new List<ElementTrade>();           // trades established.

        #region Init

        public UserControlMaterialTrader()
        {
            InitializeComponent();
            var corner = dataGridViewTrades.TopLeftHeaderCell; // work around #1487

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void Init()
        {
            dataGridViewTrades.MakeDoubleBuffered();
            dataGridViewTrades.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            extComboBoxTraderType.Items.AddRange(new string[] { "Raw".T(EDTx.UserControlMaterialTrader_Raw), "Encoded".T(EDTx.UserControlMaterialTrader_Encoded), "Manufactured".T(EDTx.UserControlMaterialTrader_Manufactured) });
            extComboBoxTraderType.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbTraderType, 0);
            extComboBoxTraderType.SelectedIndexChanged += ExtComboBoxTraderType_SelectedIndexChanged;

            checkBoxCursorToTop.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbLatest, true);
            checkBoxCursorToTop.CheckedChanged += CheckBoxCursorToTop_CheckedChanged;

            string[] strades = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbTrades, "").Split(';');
            foreach (var t in strades)      // deserialise the trades and populate the trades list
            {
                ElementTrade et = new ElementTrade();
                if (et.FromString(t))
                    trades.Add(et);
            }

            splitContainer.SplitterDistance(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSplitter, 0.75));

            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= TravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += TravelSelectionChanged;
        }

        public override void LoadLayout()
        {
            dataGridViewTrades.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += TravelSelectionChanged;
            DGVLoadColumnLayout(dataGridViewTrades, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewTrades, DbColumnSave);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSplitter, splitContainer.GetSplitterDistance());
            uctg.OnTravelSelectionChanged -= TravelSelectionChanged;

            discoveryform.OnThemeChanged -= Discoveryform_OnThemeChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }


        private void Discoveryform_OnThemeChanged()
        {
            DisplayTradeSelection();
            DisplayTradeList();
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_mcl = checkBoxCursorToTop.Checked ? discoveryform.history.GetLast?.MaterialCommodity : uctg.GetCurrentHistoryEntry?.MaterialCommodity;
            DisplayTradeSelection();
            DisplayTradeList();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            InitialDisplay();
        }

        private void TravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (checkBoxCursorToTop.Checked == false)
            {
                if (he != null && last_mcl != he?.MaterialCommodity)        // if changed MCL
                {
                    last_mcl = he.MaterialCommodity;
                    DisplayTradeSelection();
                    DisplayTradeList();
                }
            }
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
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

        private void DisplayTradeSelection(MaterialCommodityData highlight = null)  // last_he and current_mcl can be null
        {
            int sel = extComboBoxTraderType.SelectedIndex;

            var mcl = new List<Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>>();      // list of groups vs data
            Dictionary<MaterialCommodityData.MaterialGroupType, string> mattxgroupnames = new Dictionary<MaterialCommodityData.MaterialGroupType, string>(); // translated names of groups

            foreach ( var t in Enum.GetValues(typeof(MaterialCommodityData.MaterialGroupType)))     // relies on MCD being in order
            {
                var matgroup = (MaterialCommodityData.MaterialGroupType)t;
                bool ok = (sel == 0 && matgroup >= MaterialCommodityData.MaterialGroupType.RawCategory1 && matgroup <= MaterialCommodityData.MaterialGroupType.RawCategory7);
                ok |= (sel == 1 && matgroup >= MaterialCommodityData.MaterialGroupType.EncodedEmissionData && matgroup <= MaterialCommodityData.MaterialGroupType.EncodedFirmware);
                ok |= (sel == 2 && matgroup >= MaterialCommodityData.MaterialGroupType.ManufacturedChemical);
                if ( ok )
                {
                    var list = MaterialCommodityData.Get(x => x.MaterialGroup == matgroup);
                    mcl.Add(new Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>(matgroup,list));
                    mattxgroupnames[matgroup] = list[0].TranslatedMaterialGroup;
                }
            }

            Font titlefont = EDDTheme.Instance.GetFont;
            Font badgefont = EDDTheme.Instance.GetScaledFont(16f / 12f, max:21);

            const int hbadgemargin = 20;
            const int vbadgemargin = 12;

            extPictureTrades.ClearImageList();

            int vpos = 0;
            int maxhpos = 0;

            foreach (var t in mcl)
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
                    string name = mat.Name;

                    int mattotal = last_mcl == null ? int.MinValue : last_mcl.FindFDName(mat.FDName)?.Count ?? 0;    // find mcl in material list if there, and its count

                    if (mattotal >= 0)                                                  // if we have an he, adjust the totals by the trades
                    {
                        foreach (var trade in trades)
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

            if (trades.Count > 0)
            {
                MaterialCommoditiesList mcl = last_mcl;

                var totals = mcl == null ? null : MaterialCommoditiesRecipe.TotalList(mcl.List);                  // start with totals present, null if we don't have an mcl

                foreach (var trade in trades)
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

                            rw.CreateCells(dataGridViewTrades, trade.fromelement.Name, trade.offer.ToString(), totals[trade.fromelement.FDName].ToString(), trade.element.Name, trade.receive.ToString(), totals[trade.element.FDName].ToString());
                        }
                        else
                        {
                            rw.CreateCells(dataGridViewTrades, trade.fromelement.Name, trade.offer.ToString(), "- !!!", trade.element.Name, trade.receive.ToString(), "-");
                        }
                    }
                    else
                    {
                        rw.CreateCells(dataGridViewTrades, trade.fromelement.Name, trade.offer.ToString(), "-", trade.element.Name, trade.receive.ToString(), "-");
                    }

                    dataGridViewTrades.Rows.Add(rw);
                }

            }
        }

        #endregion

        private void buttonClear_Click(object sender, EventArgs e)
        {
            trades.Clear();
            StoreTrades();
            DisplayTradeSelection();
            DisplayTradeList();
        }

        private void ExtComboBoxTraderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbTraderType, extComboBoxTraderType.SelectedIndex );
            selected = null;
            DisplayTradeSelection();
        }

        private void extPictureTrades_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null && tag is ElementTrade && last_mcl != null ) // must be an element, with a tag, must have a current mcl
            {
                ElementTrade current = (ElementTrade)tag;
                System.Diagnostics.Debug.WriteLine("Clicked on " + current.type + " " + current.element.Name);

                if (selected != null)
                {
                    int currenttotal = last_mcl.Find(current.element)?.Count ?? 0;   // current mat total. If not there, its zero
                    foreach (var trade in trades)
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
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(butl, "less", "", new Point(margin, 64), new Size(32, 32), null));
                        var butr = new ExtendedControls.ExtButton();
                        butr.Image = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.RightArrow");
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(butr, "more", "", new Point(width - margin - 32, 64), new Size(32, 32), null));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("olabel", typeof(Label), "Offer".T(EDTx.UserControlMaterialTrader_Offer), new Point(margin, 30), new Size(width - margin * 2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("offer", typeof(Label), "0/" + currenttotal.ToStringInvariant(), new Point(width / 2 - 12, 50), new Size(width / 2 - 20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        var bar = new PictureBox();
                        bar.SizeMode = PictureBoxSizeMode.StretchImage;
                        bar.Image = BaseUtils.Icons.IconSet.GetIcon("Controls.MaterialTrader.TraderBar");
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(bar, "bar", "", new Point(width / 2 - 32, 70), new Size(64, 16), null));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("receive", typeof(Label), "0", new Point(width / 2 - 12, 90), new Size(width / 2 - 20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("rlabel", typeof(Label), "Receive".T(EDTx.UserControlMaterialTrader_Receive), new Point(margin, 110), new Size(width - margin * 2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.AddOK(new Point(width - margin - 80, 150), "Press to Accept".T(EDTx.UserControlModules_PresstoAccept));
                        f.AddCancel(new Point(margin, 150), "Press to Cancel".T(EDTx.UserControlModules_PresstoCancel));

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
                            else
                            {
                                if (controlname == "less")
                                {
                                    if (currentoffer > 0)
                                    {
                                        currentoffer -= current.offer;
                                        currentreceive -= current.receive;
                                    }
                                }
                                else if (controlname == "more")
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
                            trades.Add(t);
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

        int rightclickrow = -1;

        private void dataGridViewTrades_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewTrades.HandleClickOnDataGrid(e, out int unusedleftclickrow, out rightclickrow);
        }

        private void clearTradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( rightclickrow >= 0 && rightclickrow < trades.Count)
            {
                trades.RemoveAt(rightclickrow);
                StoreTrades();
                DisplayTradeSelection();
                DisplayTradeList();
            }
        }

        private void StoreTrades()
        {
            string strades = "";
            foreach (var td in trades)
                strades = strades.AppendPrePad(td.ToString(), ";");

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbTrades, strades);
        }


        private void CheckBoxCursorToTop_CheckedChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbLatest, checkBoxCursorToTop.Checked);
            InitialDisplay();
        }

    }
}
