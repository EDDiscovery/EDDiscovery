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
    public abstract class UserControlCommonBase : UserControl
    {
        public abstract void LoadLayout();
        public abstract void SaveLayout();
    }
}
