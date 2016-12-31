using EDDiscovery.DB;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;

namespace EDDiscovery.Export
{
    class ExportFilteredSystems
    {
        private const string TITLE = "Export filtered systems";


        internal void Execute(String search)
        {

            if (search == null || search.Trim().Length == 0)
            {
                MessageBox.Show("Search field should not be empty and ideally great than 2 letters",
                    TITLE);
                return;
            }

            int total = 0;
            List<String> systems = new List<string>();

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Reader))
            {
                //using (DbCommand cmd = cn.CreateCommand("SELECT s.id, s.EdsmId, n.Name, s.x, s.y, s.z, s.UpdateTimestamp, s.gridid, s.randomid FROM EdsmSystems s JOIN SystemNames n ON n.EdsmId = s.EdsmId"))
                using (DbCommand cmd = cn.CreateCommand(
                    "SELECT Distinct Name from SystemNames where Name like @Name"))
                {
                    cmd.AddParameterWithValue("@Name", search + "%");
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            systems.Add((string)reader["name"]);
                            total++;
                        }
                    }
                }
            }

            if (total == 0)
            {
                MessageBox.Show("Search has found 0 records", TITLE);
                return;
            }

            if (MessageBox.Show(String.Format("Search has found {0} records, do you wish to contiune", total), 
                TITLE, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                systems.Clear();
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "ImportedStars export| *.txt";
            dlg.Title = TITLE;
            dlg.FileName = "ImportStars.txt";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                using (StreamWriter writer = new StreamWriter(dlg.FileName, false))
                {
                    systems.Sort();
                    foreach (String system in systems)
                    {
                        writer.WriteLine(system);
                    }
                }
                MessageBox.Show(String.Format("Export complete {0}", dlg.FileName), TITLE);
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", dlg.FileName), TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
