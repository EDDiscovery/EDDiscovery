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
    public class UserControlCommonBase : UserControl
    {
        public virtual void LoadLayout() { }
        public virtual void SaveLayout() { }
    }
}
