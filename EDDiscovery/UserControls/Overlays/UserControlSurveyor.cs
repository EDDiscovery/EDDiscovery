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
        private HistoryEntry last_he = null;

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
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void InitialDisplay()
        {
            CreateWantedBodiesList(uctg.GetCurrentHistoryEntry, false);
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
            CreateWantedBodiesList(he, he.EntryType == JournalTypeEnum.Scan);
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            pictureBoxHotspot.ClearImageList();

            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;

            CreateWantedBodiesList(he, false);

            pictureBoxHotspot.Render();
        }

        public class WantedBodies
        {
            public string name { get; set; }
            public Image img { get; set; }
            public bool terraformable, waterworld, earthlike, ammonia, volcanism, ringed;
        }
                
        public static WantedBodies WantedBodiesList(string bdName, Image bdImg, bool bodyHasRings, bool bodyIsTerraformable, bool bodyHasVolcanism, bool isAmmoniaWorld, bool isAnEarthLike, bool isWaterWorld)
        {
            return new WantedBodies()
            {
                name = bdName,
                img = bdImg,
                ringed = bodyHasRings,
                terraformable = bodyIsTerraformable,
                volcanism = bodyHasVolcanism,
                ammonia = isAmmoniaWorld,
                earthlike = isAnEarthLike,
                waterworld = isWaterWorld
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
                    if (sn.ScanData?.BodyName != null && !sn.ScanData.IsStar)
                    {
                        bool hasrings, terraformable, volcanism, ammonia, earthlike, waterworld;

                        if (sn.ScanData.HasRings || sn.ScanData.Terraformable || sn.ScanData.Volcanism != null || sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body || sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world || sn.ScanData.PlanetTypeID == EDPlanet.Water_world)
                        {
                            hasrings = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            terraformable = sn.ScanData.Terraformable ? true : false;
                            volcanism = sn.ScanData.Volcanism != null ? true : false;
                            ammonia = sn.ScanData.PlanetTypeID == EDPlanet.Ammonia_world ? true : false;
                            earthlike = sn.ScanData.PlanetTypeID == EDPlanet.Earthlike_body ? true : false;
                            waterworld = sn.ScanData.PlanetTypeID == EDPlanet.Water_world ? true : false;

                            wanted_nodes.Add(WantedBodiesList(sn.ScanData.BodyName, sn.ScanData.GetPlanetClassImage(), hasrings, terraformable, volcanism, ammonia, earthlike, waterworld));
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
                        if (body.Current.ammonia && ammoniaToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.earthlike && earthsLikeToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.waterworld && waterWorldToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.ringed && hasRingsToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.volcanism && hasVolcanismToolStripMenuItem.Checked)
                        {
                            bodiesCount++;
                            DrawToScreen(body.Current, bodiesCount);
                        }

                        if (body.Current.terraformable && terraformableToolStripMenuItem.Checked)
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

            // Name
            information.Append(body.name);

            // Additional information
            information.Append((body.ammonia) ? @" is an ammonia world." : null);
            information.Append((body.earthlike) ? @" is an earth like world." : null);
            information.Append((body.waterworld && !body.terraformable) ? @" is a water world." : null);
            information.Append((body.waterworld && body.terraformable) ? @" is a terraformable water world." : null);
            information.Append((body.terraformable) ? @" is a terraformable planet." : null);
            information.Append((body.ringed) ? @" Has ring." : null);
            information.Append((body.volcanism) ? @" Geological activity reported." : null);

            Debug.Print(information.ToString()); // for testing

            // Drawing Elements
            const int iconSize = 48;
            var vPos = (bodiesCount * iconSize) - iconSize;

            //! TODO: draw icons and brief information to panel
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
        }
    }
}
 