/*
 * Copyright © 2019-2024 EDDiscovery development team
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

namespace EDDiscovery.Forms
{
    // Specialises the Import Export form for EDD purposes

    public partial class ImportExportForm : ExtendedForms.ImportExportForm
    {
        public DateTime StartTimeUTC { get { return EDDConfig.Instance.ConvertTimeToUTCFromPicker(base.StartTime); } }
        public DateTime EndTimeUTC { get { return EDDConfig.Instance.ConvertTimeToUTCFromPicker(base.EndTime); } }

        public void Export(string[] selectionlistp, ShowFlags[] showflagsp = null, string[] outputextp = null, string[] suggestedfilenamesp = null)
        {
            Init(false, selectionlistp, showflagsp, outputextp, suggestedfilenamesp);
        }

        public void Import(string[] selectionlistp, ShowFlags[] showflagsp = null, string[] inputextp = null)
        {
            Init(true, selectionlistp, showflagsp, inputextp, null);
        }

        private void Init(bool import, string[] selectionlistp, ShowFlags[] showflagsp = null, string[] outputextp = null, string[] suggestedfilenamesp = null )
        {
            base.Init(import, selectionlistp, showflagsp, outputextp, suggestedfilenamesp,
                           EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteDangerousCore.EliteReleaseDates.GameRelease),
                           EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay()),
                           EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("ExportFormIncludeHeader", true),
                           EliteDangerousCore.DB.UserDatabase.Instance.GetSetting("ExportFormOpenExcel", true),
                           Properties.Resources.edlogo_3mo_icon
                   );
        }

        protected override void OnExportClick()
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ExportFormIncludeHeader", IncludeHeader);
                EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ExportFormOpenExcel", AutoOpen);
            }
        }
    }
}
