/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSearch : UserControlCommonBase
    {
        private string DbSelectedSave { get { return DBName("UCSearch", "Tab"); } }

        #region Init

        public UserControlSearch()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            tabStrip.ImageList = new Image[] { EDDiscovery.Icons.Controls.SearchStars, EDDiscovery.Icons.Controls.SearchMaterials, EDDiscovery.Icons.Controls.SearchScan};
            tabStrip.TextList = new string[] { "Stars".Tx(this), "Materials Commodities".Tx(this) , "Scans".Tx(this) };
            tabStrip.TagList = new Type[] { typeof(UserControlSearchStars), typeof(UserControlSearchMaterialsCommodities), typeof(UserControlSearchScans)};

            tabStrip.OnCreateTab += (tab, si) =>
            {
                UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance((Type)tab.TagList[si], null);
                uccb.Name = tab.TextList[si];
                return uccb;
            };

            tabStrip.OnPostCreateTab += (tab, ctrl, si) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.Init(discoveryform, displaynumber);
                uccb.LoadLayout();
                uccb.InitialDisplay();
                discoveryform.theme.ApplyToControls(tab);
            };

            tabStrip.OnRemoving += (tab, ctrl) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.Closing();
            };
        }

        public override void InitialDisplay()
        {
            int seltab = SQLiteConnectionUser.GetSettingInt(DbSelectedSave, 0);
            if (seltab >= 0 && seltab <= tabStrip.ImageList.Length)
                tabStrip.SelectedIndex = seltab;
        }

        public override void Closing()
        {
            SQLiteConnectionUser.PutSettingInt(DbSelectedSave, tabStrip.SelectedIndex);
            tabStrip.Close();
        }

        #endregion


    }
}
