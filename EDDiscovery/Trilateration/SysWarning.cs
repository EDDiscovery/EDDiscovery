/*
 * Copyright © 2017 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public enum SysWarningResult
    {
        eSubmitAnyway,
        eUpdateAndSubmit,
        eCancel
    }
    public partial class SysWarning : Form
    {
        public SysWarningResult Result = SysWarningResult.eCancel;

        public SysWarning()
        {
            InitializeComponent();
        }

        public void SetLabel(string TargetSys, string Position)
        {
            labelExt1.Text = string.Format("You are about to submit distances from {0}.\nThe most recent known location in your history is {1}.\nPlease ensure you are submitting against the correct system.", TargetSys, Position);
            this.Width = Math.Max(labelExt1.Left + labelExt1.Width + 60, buttonCancel.Left + buttonCancel.Width + 60);
        }

        private void buttonSubAnyway_Click(object sender, EventArgs e)
        {
            Result = SysWarningResult.eSubmitAnyway;
            this.Close();
        }

        private void buttonUpdateAndSub_Click(object sender, EventArgs e)
        {
            Result = SysWarningResult.eUpdateAndSubmit;
            this.Close();
        }
    }
}
