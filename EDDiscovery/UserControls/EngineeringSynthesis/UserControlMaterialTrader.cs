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

        HistoryEntry last_he = null;
        MaterialCommoditiesList current_mcl = null;

        #region Init

        public UserControlMaterialTrader()
        {
            InitializeComponent();
            var corner = dataGridViewTrades.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewTrades.MakeDoubleBuffered();
            dataGridViewTrades.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            extComboBoxTraderType.Items.AddRange(new string[] { "Raw".Tx(EDTx.UserControlMaterialTrader_Raw), "Encoded".Tx(EDTx.UserControlMaterialTrader_Encoded), "Manufactured".Tx(EDTx.UserControlMaterialTrader_Manufactured) });
            extComboBoxTraderType.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbTraderType, 0);
            extComboBoxTraderType.SelectedIndexChanged += ExtComboBoxTraderType_SelectedIndexChanged;
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

            uctg.OnTravelSelectionChanged -= TravelSelectionChanged;
        }


        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
            Display();
        }

        private void TravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if ( he != last_he && current_mcl == null )        // if changed HE, and not locked to a MCL
            {
                last_he = he;
                Display();
            }
        }

        List<Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>> mcl = null;
        class ElementTag
        {
            public MaterialCommodityData.MaterialGroupType type;
            public MaterialCommodityData element;
            public int level;
            public int offer;
            public int receive;
        }

        ElementTag selected = null;
        
        private void Display()  // last_he and current_mcl can be null
        {
            int sel = extComboBoxTraderType.SelectedIndex;

            mcl = new List<Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>>();

            foreach ( var t in Enum.GetValues(typeof(MaterialCommodityData.MaterialGroupType)))
            {
                var e = (MaterialCommodityData.MaterialGroupType)t;
                bool ok = (sel == 0 && e >= MaterialCommodityData.MaterialGroupType.RawCategory1 && e <= MaterialCommodityData.MaterialGroupType.RawCategory7);
                ok |= (sel == 1 && e >= MaterialCommodityData.MaterialGroupType.EncodedEmissionData && e <= MaterialCommodityData.MaterialGroupType.EncodedFirmware);
                ok |= (sel == 2 && e >= MaterialCommodityData.MaterialGroupType.ManufacturedChemical);
                if ( ok )
                {
                    var list = MaterialCommodityData.GetMaterialGroup(x => x.MaterialGroup == e);
                    mcl.Add(new Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>(e,list));
                }
            }

            const int badgemargin = 20;

            extPictureTrades.ClearImageList();

            int vpos = 0;
            foreach( var t in mcl )
            {
                int hpos = 0;
                int nextvpos = vpos;
                int lvl = 1;

                foreach( var b in t.Item2 )
                {
                    Bitmap background = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.encodedbackground") as Bitmap;

                    int offer = 0, receive = 0, mattotal = 0;
                    string name = b.Name;

                    var mc = current_mcl?.FindFDName(b.FDName) ?? (last_he?.MaterialCommodity.FindFDName(b.FDName));    // if locked to mcl, use that, else use last he mcl

                    mattotal = mc?.Count ?? -1;

                    Color wash = Color.Transparent;

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

                    Bitmap bmp = DrawBadge(background, offer, receive, lvl, name, mattotal, wash);

                    var ie = extPictureTrades.AddImage(new Rectangle(hpos, vpos, background.Width, background.Height), bmp, imgowned: true);
                    ie.tag = new ElementTag { type = t.Item1, element = b, level = lvl, offer = offer, receive = receive };

                    hpos += bmp.Width + badgemargin;
                    nextvpos = Math.Max(nextvpos, vpos+ bmp.Height + badgemargin);
                    lvl++;
                }

                vpos = nextvpos;
            }

            extPictureTrades.Render();

        }

        Color orange = Color.FromArgb(255, 184, 85, 8);

        Bitmap DrawBadge(Bitmap background , int offer, int receive , int level , string matname, int mattotal, Color wash)
        {
            Bitmap bmp = background.Clone() as Bitmap;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Brush b = new SolidBrush(orange))
                {
                    Font f = new Font("Arial", 16);

                    if (offer > 0 && receive > 0)
                    {
                        Bitmap arrow = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.materialexchange") as Bitmap;

                        g.DrawImage(arrow, new Point(8, 8));

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        g.DrawString(offer.ToStringInvariant(), f, b, new Point(8, 2));
                        g.DrawString(receive.ToStringInvariant(), f, b, new Point(8, 32));
                    }

                    using (StringFormat fmt = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        g.DrawString(matname, f, b, new Rectangle(0, 100, bmp.Width, 45), fmt);

                        if ( mattotal > 0 )
                            g.DrawString(mattotal.ToStringInvariant(), f, b, new Rectangle(0, 75, bmp.Width, 20), fmt);
                    }

                }

                if ( level > 0)
                {
                    Bitmap petal = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.petal" + level.ToStringInvariant()) as Bitmap;
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

        #endregion

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridViewTrades.Rows.Clear();
        }

        private void ExtComboBoxTraderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbTraderType, extComboBoxTraderType.SelectedIndex );
            selected = null;
            Display();
        }

        private void extPictureTrades_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null)
            {
                ElementTag e = (ElementTag)tag;
                System.Diagnostics.Debug.WriteLine("Clicked on " + e.type + " " + e.element.Name);

                if (selected != null)
                {
                    if (selected.element.FDName == e.element.FDName)        // clicked on same.. deselect
                        selected = null;
                    else
                    {
                        ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

                        int width = 250;
                        int margin = 20;

                        var butl = new ExtendedControls.ExtButton();
                        butl.Image = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.LeftArrow");
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(butl, "left", "", new Point(margin, 64), new Size(32, 32), null));
                        var butr = new ExtendedControls.ExtButton();
                        butr.Image = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.RightArrow");
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(butr, "right", "", new Point(width - margin - 32, 64), new Size(32, 32), null));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("olabel", typeof(Label), "Offer".Tx(EDTx.UserControlMaterialTrader_Offer), new Point(margin, 30), new Size(width-margin*2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("offer", typeof(Label), "0/0", new Point(width / 2 - 12, 50), new Size(width/2-20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        var bar = new PictureBox();
                        bar.SizeMode = PictureBoxSizeMode.StretchImage;
                        bar.Image = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.TraderBar");
                        f.Add(new ExtendedControls.ConfigurableForm.Entry(bar, "bar", "", new Point(width/2-32, 70), new Size(64, 16), null));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("receive", typeof(Label), "0", new Point(width / 2 - 12, 90), new Size(width/2-20, 20), null, 1.2f, ContentAlignment.MiddleLeft));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("rlabel", typeof(Label), "Receive".Tx(EDTx.UserControlMaterialTrader_Receive), new Point(margin, 110), new Size(width-margin*2, 20), null, 1.5f, ContentAlignment.MiddleCenter));

                        f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".T(EDTx.OK), new Point(width - margin - 80, 150), new Size(80, 24), "Press to Accept".T(EDTx.UserControlModules_PresstoAccept)));
                        f.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ExtButton), "Cancel".T(EDTx.Cancel), new Point(margin, 150), new Size(80, 24), "Press to Cancel".T(EDTx.UserControlModules_PresstoCancel)));

                        f.Trigger += (dialogname, controlname, xtag) =>
                        {
                            if (controlname == "OK")
                            {
                                f.ReturnResult(DialogResult.OK);
                            }
                            else if (controlname == "Cancel")
                            {
                                f.ReturnResult(DialogResult.Cancel);
                            }
                            else if (controlname == "Less")
                            {
                            }
                            else if (controlname == "More")
                            {
                            }
                        };

                        f.RightMargin = margin;

                        f.InitCentred(this.FindForm(), this.FindForm().Icon, "Trade".T(EDTx.UserControlMaterialTrader_Trade));
                        //f.GetControl<Label>("offer").Font = new Font()

                        DialogResult res = f.ShowDialog();

                        if (res == DialogResult.OK)
                        {
                            Display();
                        }
                    }
                }
                else
                {
                    selected = e;
                }
                Display();
            }
        }
    }
}
