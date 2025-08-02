/*
 * Copyright 2016-2024 EDDiscovery development team
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
 */

using System;
using System.Drawing;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSearch : UserControlCommonBase
    {
        private string dbSelectedSave = "Tab";

        #region Init

        public UserControlSearch()
        {
            InitializeComponent();
        }

        protected override void Init()
        {
            DBBaseName = "UCSearch";

            tabStrip.ImageList = new Image[] { EDDiscovery.Icons.Controls.SearchStars, EDDiscovery.Icons.Controls.SearchMaterials, EDDiscovery.Icons.Controls.Scan};
            tabStrip.TextList = new string[] { "Stars".T(EDTx.UserControlSearch_Stars), "Materials Commodities".T(EDTx.UserControlSearch_MaterialsCommodities), "Scans".T(EDTx.UserControlSearch_Scans) };
            tabStrip.TagList = new Type[] { typeof(SearchStars), typeof(SearchMaterialsCommodities), typeof(SearchScans)};

            tabStrip.OnCreateTab += (tab, si) =>
            {
                UserControlCommonBase uccb = (UserControlCommonBase)Activator.CreateInstance((Type)tab.TagList[si], null);
                uccb.Name = tab.TextList[si];
                uccb.ParentUCCB = this;
                return uccb;
            };

            tabStrip.OnPostCreateTab += (tab, ctrl, si) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.CallInit(DiscoveryForm, DisplayNumber);
                ExtendedControls.Theme.Current.ApplyStd(uccb);       // contract, in UCCB, states theming is between init and load
                uccb.CallLoadLayout();
                uccb.CallInitialDisplay();
            };

            tabStrip.OnRemoving += (tab, ctrl) =>
            {
                UserControlCommonBase uccb = ctrl as UserControlCommonBase;
                uccb.CallCloseDown();
            };
        }

        protected override void InitialDisplay()
        {
            int seltab = GetSetting(dbSelectedSave, 0);
            seltab = seltab.Range(0, tabStrip.ImageList.Length - 1);
            tabStrip.SelectedIndex = seltab;
        }

        protected override void Closing()
        {
            PutSetting(dbSelectedSave, tabStrip.SelectedIndex);
            tabStrip.Close();
        }

        #endregion


    }
}
