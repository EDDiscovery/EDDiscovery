using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class SplitContainerCustom : System.Windows.Forms.SplitContainer
    {
        public bool LayoutChanging { get; private set; } = false;
        protected override void OnLayout(LayoutEventArgs e)
        {
            LayoutChanging = true;
            base.OnLayout(e);
            LayoutChanging = false;
        }
    }
}
