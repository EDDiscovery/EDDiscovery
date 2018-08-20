using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class DropDownCustom : Form
    {
        private ListControlCustom listcontrol;

        public event EventHandler SelectedIndexChanged;
        public event KeyPressEventHandler KeyPressed;
        public event KeyEventHandler OtherKeyPressed;

        public List<string> Items { get { return listcontrol.Items; } set { listcontrol.Items = value; } }
        public int[] ItemSeperators { get { return listcontrol.ItemSeperators; } set { listcontrol.ItemSeperators = value; } }     // set to array giving index of each separator
        public List<Image> ImageItems { get { return listcontrol.ImageItems; } set { listcontrol.ImageItems = value; } }
        public Color MouseOverBackgroundColor { get { return listcontrol.MouseOverBackgroundColor; } set { listcontrol.MouseOverBackgroundColor = value; } }
        public int SelectedIndex { get { return listcontrol.SelectedIndex; } set { listcontrol.SelectedIndex = value; } }
        public Color SelectionBackColor { get { return listcontrol.SelectionBackColor; } set { listcontrol.SelectionBackColor = value; this.BackColor = value; } }
        public Color BorderColor { get { return listcontrol.BorderColor; } set { listcontrol.BorderColor = value; } }
        public Color ScrollBarColor { get { return listcontrol.ScrollBarColor; } set { listcontrol.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return listcontrol.ScrollBarButtonColor; } set { listcontrol.ScrollBarButtonColor = value; } }
        public FlatStyle FlatStyle { get { return listcontrol.FlatStyle; } set { listcontrol.FlatStyle = value; } }
        public new Font Font { get { return base.Font; } set { base.Font = value; listcontrol.Font = value; } }
        public bool FitToItemsHeight { get { return listcontrol.FitToItemsHeight; } set { listcontrol.FitToItemsHeight = value; } }
        public int ScrollBarWidth { get { return listcontrol.ScrollBarWidth; } set { listcontrol.ScrollBarWidth = value; } }
        public float GradientColorScaling { get { return listcontrol.GradientColorScaling; } set { listcontrol.GradientColorScaling = value; } }
        public int ItemHeight { get { return listcontrol.ItemHeight; } set { listcontrol.ItemHeight = value; } }
        public Color ItemSeperatorColor { get { return listcontrol.ItemSeperatorColor; } set { listcontrol.ItemSeperatorColor = value; } }

        private bool closeondeactivateselected;

        public DropDownCustom(string name = "", bool closeondeact = true)
        {
            closeondeactivateselected = closeondeact;

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.listcontrol = new ListControlCustom();
            this.Name = this.listcontrol.Name = name;
            this.listcontrol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.listcontrol.Dock = DockStyle.Fill;
            this.listcontrol.Visible = true;
            this.listcontrol.SelectedIndexChanged += _listcontrol_SelectedIndexChanged;
            this.listcontrol.KeyPressed += _listcontrol_KeyPressed;
            this.listcontrol.OtherKeyPressed += _listcontrol_OtherKeyPressed;
            this.listcontrol.Margin = new Padding(0);
            this.listcontrol.FitToItemsHeight = false;
            this.Padding = new Padding(0);
            this.Controls.Add(this.listcontrol);
        }

        public void KeyDownAction(KeyEventArgs e)
        {
            listcontrol.KeyDownAction(e);
        }

        private void _listcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (closeondeactivateselected)
                this.Close();

            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);

        }

        private void _listcontrol_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (KeyPressed != null)
                KeyPressed(this, e);
        }

        private void _listcontrol_OtherKeyPressed(object sender, KeyEventArgs e)
        {
            if (OtherKeyPressed != null)
                OtherKeyPressed(this, e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if ( closeondeactivateselected)
                this.Close();
        }
    }

}
