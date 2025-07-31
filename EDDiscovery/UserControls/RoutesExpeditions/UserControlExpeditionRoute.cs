/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlExpedition
    {
        #region Routes

        // insert or append (insertindex=-1) to the grid, either systementry, Isystems or strings
        // rows will be inserted with cells[0].tag set to null, so it will cause them to autoupdate
        // totals will be asked to update
        public void AppendOrInsertSystems(int insertIndex, IEnumerable<object> sysnames)
        {
            int i = 0;
            foreach (var system in sysnames)
            {
                object[] data;
                long? id64 = null;

                if (system is string)
                {
                    data = new object[] { system, "" };
                }
                else if (system is SavedRouteClass.SystemEntry)
                {
                    var se = (SavedRouteClass.SystemEntry)system;           
                    bool known = se.HasCoordinate;                          
                    data = new object[] { se.Name, se.Note, known ? se.X.ToString("N5") : "", known ? se.Y.ToString("N5") : "", known ? se.Z.ToString("N5") : "" };
                }
                else
                {
                    var se = (ISystem)system;
                    id64 = se.SystemAddress;
                    string note = se.Tag as string ?? "";// Tag may have info in it for notes, if so, use it
                    bool known = se.HasCoordinate;
                    data = new object[] { se.Name, note , known ? se.X.ToString("N5") : "", known ? se.Y.ToString("N5") : "", known ? se.Z.ToString("N5"): "" };         
                }

                if (((string)data[0]).HasChars())       // must have a name
                {
                    if (insertIndex < 0)
                    {
                        int rowno = dataGridView.Rows.Add(data);
                        dataGridView.Rows[rowno].Cells[id64imported].Tag = id64;
                    }
                    else
                    {
                        dataGridView.Rows[insertIndex].Cells[id64imported].Tag = id64;
                        dataGridView.Rows.Insert(insertIndex++, data);
                    }

                    forcetotalsupdate = true;           // update the totals
                }

                i++;
            }
        }

        // if the data in the grid is set, and different to the loadedroute, or the grid is not empty.  
        // not dirty if the grid is empty (name and systems empty)
        public bool IsDirty()
        {
            var gridroute = CopyGridIntoRoute();
            return (gridroute != null && loadedroute != null) ? !gridroute.Equals(loadedroute) : gridroute != null;
        }

        // move systems in grid into this route class
        private SavedRouteClass CopyGridIntoRoute()
        {
            SavedRouteClass route = new SavedRouteClass();
            route.Name = textBoxRouteName.Text.Trim();

            route.Systems.Clear();

            // grid xyz is in culture specific mode

            var data = dataGridView.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridView.NewRowIndex && (r.Cells[0].Value as string).HasChars())
                .Select(r => new SavedRouteClass.SystemEntry(
                                    (r.Cells[SystemName.Index].Value as string) ?? "", // sometimes they can end up null
                                    (r.Cells[Note.Index].Value as string) ?? "",
                                    ((string)r.Cells[ColumnX.Index].Value).ParseDouble(SavedRouteClass.SystemEntry.NotKnown, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number),
                                    ((string)r.Cells[ColumnY.Index].Value).ParseDouble(SavedRouteClass.SystemEntry.NotKnown, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number),
                                    ((string)r.Cells[ColumnZ.Index].Value).ParseDouble(SavedRouteClass.SystemEntry.NotKnown, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.NumberStyles.Number)
                                    ));

            route.Systems.AddRange(data);

            if (dateTimePickerStartDate.Checked)
            {
                route.StartDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerStartDate.Value.Date);
                if (dateTimePickerStartTime.Checked)
                    route.StartDateUTC += dateTimePickerStartTime.Value.TimeOfDay;
            }
            else
            {
                route.StartDateUTC = null;
            }

            if (dateTimePickerEndDate.Checked)
            {
                route.EndDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromPicker(dateTimePickerEndDate.Value.Date);
                route.EndDateUTC += dateTimePickerEndTime.Checked ? dateTimePickerEndTime.Value.TimeOfDay : new TimeSpan(23, 59, 59);
            }
            else
            {
                route.EndDateUTC = null;
            }

            return route.Systems.Count > 0 || route.Name.HasChars() ? route : null;           // if systems or name, there is a route
        }

        // Move the current route into the DB
        private bool StoreCurrentRouteIntoDB(SavedRouteClass newrt)
        {
            if (newrt.Name.IsEmpty())
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please specify a name for the route.".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();
            var edsmroute = savedroutes.Find(x => x.Name.Equals(newrt.Name, StringComparison.InvariantCultureIgnoreCase) && x.EDSM);

            if (edsmroute != null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), ("The current route name conflicts with a well-known expedition." + Environment.NewLine
                    + "Please specify a new name to save your changes.").Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return false;
            }

            var overwriteroute = savedroutes.Where(r => r.Name.Equals(newrt.Name)).FirstOrDefault();

            if (overwriteroute != null)
            {
                if (MessageBoxTheme.Show(FindForm(), "Warning: route already exists. Would you like to overwrite it?".Tx(), "Warning".Tx(), MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                overwriteroute.Delete();
            }

            if (overwriteroute == null)
                return newrt.Add();
            else
            {
                newrt.Id = overwriteroute.Id;
                return newrt.Add();
            }
        }

        // true if grid is empty, or it has saved
        private bool SaveGrid()
        {
            SavedRouteClass route = CopyGridIntoRoute();
            if (route != null)
            {
                if (StoreCurrentRouteIntoDB(route))
                {
                    loadedroute = route;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        private bool PromptAndSaveIfNeeded()
        {
            if (IsDirty())
            {
                MakeVisible();      // we may not be on this screen if called (shutdown, import) make visible

                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), ("Expedition - There are unsaved changes to the current route." + Environment.NewLine
                    + "Would you like to save the current route before proceeding?").Tx(), "Warning".Tx(), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveGrid();

                    case DialogResult.No:
                        return true;

                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            else
                return true;
        }


        #endregion

    }
}
