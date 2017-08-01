using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Forms;
using EDDiscovery.UserControls;

namespace EDDiscovery
{
    public partial class GridControl : UserControl
    {
        EDDiscoveryForm discoveryForm;
        List<UserControlContainerResizable> list = new List<UserControlContainerResizable>();

        public GridControl()
        {
            InitializeComponent();
            comboBoxGridSelector.Items.AddRange(PopOutControl.spanelbuttonlist);
        }

        public void InitControl( EDDiscoveryForm f)
        {
            discoveryForm = f;
        }

        private void comboBoxGridSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            UserControlCommonBase uccb = PopOutControl.Create((PopOutControl.PopOuts)(comboBoxGridSelector.SelectedIndex));

            if ( uccb != null )
            {
                UserControlContainerResizable uccr = new UserControlContainerResizable();
                uccr.Init(uccb);
                uccr.ResizeStart += ResizeStart;

                int index = list.Count;
                uccr.Location = new Point(index * 50, index * 50);
                uccr.Size = new Size(300, 300);
                list.Add(uccr);
                panelPlayfield.Controls.Add(uccr);

                discoveryForm.TravelControl.UserControlPostCreate(index + 2000, uccb);

                uccr.Font = discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                discoveryForm.theme.ApplyToControls(uccr,discoveryForm.theme.GetFont);

                uccb.Display(discoveryForm.TravelControl.GetTravelHistoryCurrent, discoveryForm.history);
            }

            ResumeLayout();

        }

        private void ResizeStart( UserControlContainerResizable uccr)
        {
            uccr.BringToFront();
        }
    }
}
