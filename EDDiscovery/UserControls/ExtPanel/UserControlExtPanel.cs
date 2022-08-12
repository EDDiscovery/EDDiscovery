/*
 * Copyright © 2022 EDDiscovery development team
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

namespace EDDiscovery.UserControls
{
    public partial class UserControlExtPanel : UserControlCommonBase
    {
        public UserControlExtPanel()
        {
            InitializeComponent();
        }

        private IEDDPanelExtension panel;

        public override void Creation(PanelInformation.PanelInfo p)
        {
            base.Creation(p);
            System.Diagnostics.Debug.WriteLine($"Ext panel create class {p.WindowTitle}");
            panel = (IEDDPanelExtension)Activator.CreateInstance((Type)p.Tag, null);
            Controls.Add((Control)panel);
        }

        public override void Init()
        {
            panel.Init();
        }

        public override void InitialDisplay()
        {
            panel.InitialDisplay();
        }

        public override void Closing()
        {
            panel.Closing(); 
        }
    }

    // temp example

    class NewPanel1 : UserControl, IEDDPanelExtension
    {
        public void Closing()
        {
            System.Diagnostics.Debug.Write("New Panel close 1");
        }

        public void Init()
        {
            System.Diagnostics.Debug.Write("New Panel init 1");
        }

        public void InitialDisplay()
        {
        }
    }

    class NewPanel2 : UserControl, IEDDPanelExtension
    {
        public void Closing()
        {
            System.Diagnostics.Debug.Write("New Panel close 2");
        }

        public void Init()
        {
            System.Diagnostics.Debug.Write("New Panel init 2");
        }

        public void InitialDisplay()
        {
        }
    }

    public interface IEDDPanelExtension
    {
        void Init();
        void InitialDisplay();
        void Closing();

    }

}
