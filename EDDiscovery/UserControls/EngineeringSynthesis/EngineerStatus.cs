using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class EngineerStatusPanel : UserControl
    {
        public ItemData.EngineeringInfo EngineerInfo { get; private set; }

        public EngineerStatusPanel()
        {
            InitializeComponent();
        }

        public void Init(string name,string starsystem, string planet, string basename, ItemData.EngineeringInfo ei)
        {
            EngineerInfo = ei;

            ExtendedControls.Theme.Current?.ApplyStd(this);
            var enumlist = new Enum[] { EDTx.UserControlEngineering_UpgradeCol, EDTx.UserControlEngineering_ModuleCol, EDTx.UserControlEngineering_LevelCol, 
                            EDTx.UserControlEngineering_MaxCol, EDTx.UserControlEngineering_WantedCol, EDTx.UserControlEngineering_AvailableCol, 
                            EDTx.UserControlEngineering_NotesCol, EDTx.UserControlEngineering_RecipeCol, EDTx.UserControlEngineering_EngineersCol,
                        };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, "UserControlEngineering");    // share IDs with Engineering panel./

            labelEngineerName.Text = name;
            labelEngineerStatus.Text = "";
            engineerImage.Image = BaseUtils.Icons.IconSet.GetIcon("Engineers." + name);
            labelEngineerStarSystem.Text = starsystem;
            labelPlanet.Text = planet;
            labelBaseName.Text = basename;
            labelEngineerDistance.Text = "";

        }

        public void UpdateStatus(string status, ISystem cur)
        {
            labelEngineerStatus.Text = status;
            string dist = "";
            if (cur != null && EngineerInfo != null)
            {
                var d = cur.Distance(EngineerInfo.X, EngineerInfo.Y, EngineerInfo.Z);
                dist = d.ToString("0.#") + " ly";
            }
            labelEngineerDistance.Text = dist;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            dataViewScrollerPanel.Width = ClientRectangle.Width - labelEngineerStarSystem.Left - 150;
        }
    }
}


