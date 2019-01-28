using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EMK.LightGeometry;
using ExtendedControls;
using BaseUtils;
using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System.Diagnostics;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSurveyor : UserControlCommonBase
    {
        private HistoryEntry last_he;

        private string DbSave { get { return DBName("Surveyor"); } }
        private Font displayfont;

        public UserControlSurveyor()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            displayfont = discoveryform.theme.GetFont;

            discoveryform.OnNewEntry += NewEntry;

            EDDTheme.Instance.ApplyToControls(this);

            // set context menu checkboxes
            ammoniaToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showAmmonia", true);
            earthsLikeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showEarthlike", true);
            waterWorldToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showWaterWorld", true);
            terraformableToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showTerraformable", true);
            hasVolcanismToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showVolcanism", true);
            hasRingsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRinged", true);

            // allow strings translation
            Translator.Instance.Translate(this);
            Translator.Instance.Translate(contextMenuStrip, this);

            pictureBoxHotspot.Render();
        }

        public override void LoadLayout()
        {
            pictureBoxHotspot.Render();
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void InitialDisplay()
        {
            CreateWantedBodiesList(uctg.GetCurrentHistoryEntry, true);
            pictureBoxHotspot.Render();
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= NewEntry;
            uctg.OnTravelSelectionChanged -= Display;
        }

        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            CreateWantedBodiesList(uctg.GetCurrentHistoryEntry, he.EntryType == JournalTypeEnum.Scan);
        }

        public override Color ColorTransparency => Color.Green;

        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            pictureBoxHotspot.ClearImageList();
            CreateWantedBodiesList(he, false);
            pictureBoxHotspot.Render();
        }

        public class WantedBodies
        {
            public string Name { get; set; }
            public Image Img { get; set; }
            public string DistanceFromArrival { get; internal set; }

            public bool Ammonia, Earthlike, WaterWorld, Terraformable, Volcanism, Ringed, Mapped;
        }
                
        public static WantedBodies WantedBodiesList(string bdName, Image bdImg, string distance, bool bodyHasRings, bool bodyIsTerraformable, bool bodyHasVolcanism, bool isAmmoniaWorld, bool isAnEarthLike, bool isWaterWorld, bool mapped)
        {
            return new WantedBodies()
            {
                Name = bdName,
                Img = bdImg,
                DistanceFromArrival = distance,
                Ringed = bodyHasRings,
                Terraformable = bodyIsTerraformable,
                Volcanism = bodyHasVolcanism,
                Ammonia = isAmmoniaWorld,
                Earthlike = isAnEarthLike,
                WaterWorld = isWaterWorld,
                Mapped = mapped
            };
        }

        private void CreateWantedBodiesList(HistoryEntry he, bool force)
        {
            StarScan.SystemNode scannode = null;

            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;

            if (he == null)     //  no he, no display
            {
                last_he = he;                
                SetControlText("No Scan".Tx());
                return;
            }
            else
            {
                scannode = discoveryform.history.starscan.FindSystem(he.System, true);        // get data with EDSM

                if (scannode == null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    last_he = null;                
                    SetControlText("No Scan".Tx());
                    return;
                }

                if (samesys)      // same system, no force, no redisplay
                    return;
            }

            last_he = he;

            var all_nodes = scannode.Bodies.ToList();

            if (all_nodes != null)
            {
                var wanted_nodes = new List<WantedBodies>();

                foreach (StarScan.ScanNode sn in all_nodes)
                {
                    if (sn.ScanData?.BodyName != null && !sn.ScanData.IsStar)
                    {
                        bool hasrings, terraformable, volcanism, ammonia, earthlike, waterworld, mapped;

                        if (sn.ScanData.HasRings || sn.ScanData.Terraformable || sn.ScanData.Volcanism != null || sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body || sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world || sn.ScanData.PlanetTypeID == EDPlanet.Water_world)
                        {
                            hasrings = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            terraformable = sn.ScanData.Terraformable ? true : false;
                            volcanism = sn.ScanData.Volcanism != null ? true : false;
                            ammonia = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            earthlike = sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body ? true : false;
                            waterworld = sn.ScanData.PlanetTypeID == EDPlanet.Water_world ? true : false;

                            mapped = sn.IsMapped;

                            var distanceString = new StringBuilder();

                            distanceString.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);

                            wanted_nodes.Add(WantedBodiesList(sn.ScanData.BodyName, sn.ScanData.GetPlanetClassImage(), distanceString.ToString(), hasrings, terraformable, volcanism, ammonia, earthlike, waterworld, mapped));
                        }
                    }
                }

                SelectBodiesToDisplay(wanted_nodes);
            }
        }

        private void SelectBodiesToDisplay(List<WantedBodies> wanted_nodes)
        {
            if (wanted_nodes != null)
            {
                var bodiesCount = 0;

                using (var body = wanted_nodes.GetEnumerator())
                {
                    while (body.MoveNext())
                    {
                        if (body.Current.Ammonia && ammoniaToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Earthlike && earthsLikeToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.WaterWorld && waterWorldToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Ringed && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && hasRingsToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Volcanism && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && hasVolcanismToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.Terraformable && !body.Current.Ammonia && !body.Current.Earthlike && !body.Current.WaterWorld && terraformableToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }
                    }
                }
            }
        }

        private void DrawToScreen(WantedBodies body, int bodiesCount)
        {
            var information = new StringBuilder();

            // Is already surface mapped or not?
            if (body.Mapped)
            {
                information.Append("o "); // let the cmdr see that this body is already mapped
            }
            else
            {
                information.Append("  ");
            }

            // Name
            information.Append(body.Name);

            // Additional information
            information.Append((body.Ammonia) ? @" is an ammonia world." : null);
            information.Append((body.Earthlike) ? @" is an earth like world." : null);
            information.Append((body.WaterWorld && !body.Terraformable) ? @" is a water world." : null);
            information.Append((body.WaterWorld && body.Terraformable) ? @" is a terraformable water world." : null);
            information.Append((body.Terraformable && !body.WaterWorld) ? @" is a terraformable planet." : null);
            information.Append((body.Ringed) ? @" Has ring." : null);
            information.Append((body.Volcanism) ? @" Geological activity reported." : null);

            information.Append(@" " + body.DistanceFromArrival);

            //Debug.Print(information.ToString()); // for testing

            // Drawing Elements
            const int rowHeight = 24;
            var vPos = (bodiesCount * rowHeight) - rowHeight;

            var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

            if (body != null)
            {
                pictureBoxHotspot?.AddTextAutoSize(
                    new Point(0, vPos + 4),
                    new Size(pictureBoxHotspot.Width, 24),
                    information.ToString(),
                    DefaultFont,
                    textcolour,
                    backcolour,
                    1.0F);
            }

            pictureBoxHotspot.Refresh();
        }

        private void ammoniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showAmmonia", ammoniaToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void earthsLikeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showEarthlike", earthsLikeToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void waterWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showWaterWorld", waterWorldToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void terraformableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showTerraformable", terraformableToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void hasVolcanismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showVolcanism", hasVolcanismToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void hasRingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SQLiteDBClass.PutSettingBool(DbSave + "showRinged", hasRingsToolStripMenuItem.Checked);
            CreateWantedBodiesList(last_he, true);
        }

        private void pictureBoxHotspot_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip.Visible |= e.Button == MouseButtons.Right;
            contextMenuStrip.Top = MousePosition.Y;
            contextMenuStrip.Left = MousePosition.X;
        }
    }
}