/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using EliteDangerousCore.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EliteDangerousCore.EDDN;
using Newtonsoft.Json.Linq;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlContainerSplit : UserControlCommonBase
    {        

        #region Initialisation

        public UserControlContainerSplit()
        {
            InitializeComponent();
        }

        public override void Init()
        {

        }

        public override void InitialDisplay()
        {
        
        }
                
        #endregion
                
        #region Layout

        public override void LoadLayout() 
        {        
            if (!EDDOptions.Instance.NoWindowReposition)
            {
                splitContainerMain.SplitterDistance(SQLiteDBClass.GetSettingDouble("UserControlContainerSplitVertical", 0.62));
                splitContainerMain.SplitterDistance(SQLiteDBClass.GetSettingDouble("UserControlContainerSplitHorizontal", 0.38));
            }

            // instantiate a grid on the first panel
            UserControlContainerGrid ucCG1 = new UserControlContainerGrid();
            ucCG1.Init();
            ucCG1.Dock = DockStyle.Fill;

            // create the first grid
            splitContainerMain.Panel1.Controls.Add(ucCG1);

            // instantiate a grid on the second panel
            UserControlContainerGrid ucCG2 = new UserControlContainerGrid();
            ucCG2.Init();
            ucCG2.Dock = DockStyle.Fill;

            // create the second grid
            splitContainerMain.Panel2.Controls.Add(ucCG2);
        }

        #endregion

        public override void Closing()     // called by form when closing
        {
            if (splitContainerMain.Orientation == Orientation.Vertical)
            {
                SQLiteDBClass.PutSettingDouble("UserControlContainerSplitVertical", splitContainerMain.GetSplitterDistance());
            }
            else
            {
                SQLiteDBClass.PutSettingDouble("UserControlContainerSplitHorizontal", splitContainerMain.GetSplitterDistance());
            }
        }               
        
        private void splitContainerMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (splitContainerMain.Orientation == Orientation.Horizontal)
                {
                    splitContainerMain.Orientation = Orientation.Vertical;
                }
                else
                {
                    splitContainerMain.Orientation = Orientation.Horizontal;
                }
            }
        }
    }
}
