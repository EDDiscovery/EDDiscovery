/*
 * Copyright 2016 - 2024 EDDiscovery development team
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
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTestOverlay : UserControlCommonBase
    {
        public UserControlTestOverlay()
        {
            InitializeComponent();
        }

        protected override void Init()
        {
        }

        protected override void InitialDisplay()
        {
        }

        protected override void Closing()
        {
        }

        public override bool SupportTransparency => true;
        protected override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            this.panel1.BackColor = curcol;
            tabPage1.BackColor = curcol;
            tabPage2.BackColor = curcol;
            extCheckBox1.BackColor = curcol;        // note if in groupbox, you'd use the groupbox colour when on
            //System.Diagnostics.Debug.WriteLine($"TestOverlay set transparency {on} {curcol}");
            foreach (Control c in Controls) System.Diagnostics.Debug.WriteLine($"TO Control {c.Name} {c.Bounds}");
        }
    }
}
