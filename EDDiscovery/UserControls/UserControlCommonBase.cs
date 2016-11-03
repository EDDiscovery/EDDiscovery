using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.DB;

namespace EDDiscovery.UserControls
{
    public class UserControlCommonBase : UserControl
    {
        public virtual void LoadLayout() { }
        public virtual void SaveLayout() { }

        #region DGV Column helpers - used to save/size the DGV of user controls dynamically.

        public void DGVLoadColumnLayout(DataGridView dgv, string root)
        {
            if (SQLiteConnectionUser.keyExists(root + "1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    int w = SQLiteDBClass.GetSettingInt(root + ((i + 1).ToString()), -1);
                    if (w >= 10)        // in case something is up (min 10 pixels)
                        dgv.Columns[i].Width = w;
                }
            }
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string root)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                SQLiteDBClass.PutSettingInt(root + ((i + 1).ToString()), dgv.Columns[i].Width);
            }
        }

        #endregion

    }
}
