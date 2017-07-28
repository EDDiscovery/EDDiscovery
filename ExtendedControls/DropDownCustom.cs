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
        private ListControlCustom _listcontrol;

        public event EventHandler DropDown;
        public event EventHandler SelectedIndexChanged;
        public event KeyPressEventHandler KeyPressed;
        public event KeyEventHandler OtherKeyPressed;

        public Color MouseOverBackgroundColor { get { return _listcontrol.MouseOverBackgroundColor; } set { _listcontrol.MouseOverBackgroundColor = value; } }
        public int SelectedIndex { get { return _listcontrol.SelectedIndex; } set { _listcontrol.SelectedIndex = value; } }
        public Color SelectionBackColor { get { return _listcontrol.SelectionBackColor; } set { _listcontrol.SelectionBackColor = value; this.BackColor = value; } }
        public List<string> Items { get { return _listcontrol.Items; } set { _listcontrol.Items = value; } }
        public Color BorderColor { get { return _listcontrol.BorderColor; } set { _listcontrol.BorderColor = value; } }
        public Color ScrollBarColor { get { return _listcontrol.ScrollBarColor; } set { _listcontrol.ScrollBarColor = value; } }
        public Color ScrollBarButtonColor { get { return _listcontrol.ScrollBarButtonColor; } set { _listcontrol.ScrollBarButtonColor = value; } }
        public FlatStyle FlatStyle { get { return _listcontrol.FlatStyle; } set { _listcontrol.FlatStyle = value; } }
        public new Font Font { get { return base.Font; } set { base.Font = value; _listcontrol.Font = value; } }

        public int ItemHeight { get { return _listcontrol.ItemHeight; } set { _listcontrol.ItemHeight = value; } }

        private bool closeondeactivate;

        public DropDownCustom(string name = "", bool closeondeact = true)
        {
            closeondeactivate = closeondeact;

            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this._listcontrol = new ListControlCustom();
            this.Name = this._listcontrol.Name = name;
            this._listcontrol.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this._listcontrol.Dock = DockStyle.Fill;
            this._listcontrol.Visible = true;
            this._listcontrol.SelectedIndexChanged += _listcontrol_SelectedIndexChanged;
            this._listcontrol.KeyPressed += _listcontrol_KeyPressed;
            this._listcontrol.OtherKeyPressed += _listcontrol_OtherKeyPressed;
            this._listcontrol.Margin = new Padding(0);
            this.Padding = new Padding(0);
            this.Controls.Add(this._listcontrol);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            DropDown(this, e);
        }

        private void _listcontrol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
            this.Close();
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

            if ( closeondeactivate)
                this.Close();
        }
    }

}
