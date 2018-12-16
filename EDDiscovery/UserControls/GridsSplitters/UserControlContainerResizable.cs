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
    public partial class UserControlContainerResizable : UserControl
    {
        public Color BorderColor { get; set; } = Color.Red;
        public Color SelectedBorderColor { get; set; } = Color.Green;
        public Control control { get; private set; }
        public Action<UserControlContainerResizable> ResizeStart;
        public Action<UserControlContainerResizable,bool> ResizeEnd;
        public bool Selected { get { return selected; } set { SetSelected(value); } }

        private const int margin = 3;
        private const int ycontroltext = 18;    // space for title/control text. Control text replaces title on UCs where its called.
        private bool selected = false;

        private Label lcontroltext;

        public UserControlContainerResizable()
        {
            InitializeComponent();
        }

        public void Init( UserControlCommonBase c, string title)
        {
            control = c;
            lcontroltext = new Label() { Top = margin, Left = margin, Text = title };
            Controls.Add(control);
            Controls.Add(lcontroltext);
            PerformLayout();
        }

        public void SetControlText(string s)
        {
            lcontroltext.Text = s;
        }

        private void SetSelected(bool s)
        {
            selected = s;
            Invalidate();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            lcontroltext.Width = ClientRectangle.Width - lcontroltext.Left - margin*2;
            control.Location = new Point(margin, margin+ycontroltext);
            control.Size = new Size(ClientRectangle.Width - margin * 2, ClientRectangle.Height - margin * 2 - ycontroltext);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Pen pb = new Pen(BackColor, 1.0F))
            using (Pen p1 = new Pen(Selected ? SelectedBorderColor : BorderColor, 1.0F))
            {
                Rectangle r = ClientRectangle;
                r.Inflate(-1, -1);
                e.Graphics.DrawRectangle(p1, r);
                r.Inflate(-1, -1);
                e.Graphics.DrawRectangle(pb, r);
                r.Inflate(-1, -1);
                e.Graphics.DrawRectangle(pb, r);
            }

            base.OnPaint(e);
        }

        Point dragstart;
        Point startpos;
        Size startsize;
        enum DragPos { None, Left, Right, Top, Bottom, Size};
        DragPos dp = DragPos.None;
        bool dragmoved = false;

        DragPos DragType(MouseEventArgs e)
        {
            if (e.Y < margin)
            {
                Cursor.Current = Cursors.Hand;
                return DragPos.Top;
            }
            else if (e.X < margin)
            {
                Cursor.Current = Cursors.SizeWE;
                return DragPos.Left;
            }
            else if (e.X >= Width - margin)
            {
                if (e.Y >= Height - margin)
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    return DragPos.Size;
                }
                else
                {
                    Cursor.Current = Cursors.SizeWE;
                    return DragPos.Right;
                }
            }
            else if (e.Y >= Height - margin)
            {
                Cursor.Current = Cursors.SizeNS;
                return DragPos.Bottom;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                return DragPos.None;
            }
        }

        private void UserControlContainerResizable_MouseDown(object sender, MouseEventArgs e)
        {
            dragstart = Control.MousePosition;
            startpos = Location;
            startsize = Size;
            dp = DragType(e);
            dragmoved = false;
            //System.Diagnostics.Debug.WriteLine("Drag start here " + dp);

            ResizeStart?.Invoke(this);
        }

        private void UserControlContainerResizable_MouseMove(object sender, MouseEventArgs e)
        {
            if (dp == DragPos.None)
            {
                DragType(e);
            }
            else if ( e.Button == MouseButtons.Left )
            {
                int xdelta = Control.MousePosition.X - dragstart.X;
                int ydelta = Control.MousePosition.Y - dragstart.Y;
                int absxdelta = Math.Abs(xdelta);
                int absydelta = Math.Abs(ydelta);

                //System.Diagnostics.Debug.WriteLine("Drag " + dp + " moved " + dragmoved + " delta " + absxdelta + "," + absydelta);

                if (dragmoved == false)
                {
                    if (dp == DragPos.Top || dp == DragPos.Bottom)
                        dragmoved = absydelta > margin;
                    else
                        dragmoved = absxdelta > margin;
                }

                if (dragmoved)
                {
                    int m = 10;

                    SuspendLayout();
                    if (dp == DragPos.Top)
                    {
                        Top = Math.Min(Math.Max(startpos.Y + ydelta, 0), Parent.Height - m);
                        Left = Math.Min(Math.Max(startpos.X + xdelta, -startsize.Width + m), Parent.Width - m);
                    }
                    else if (dp == DragPos.Bottom)
                        Height = startsize.Height + ydelta;
                    else if (dp == DragPos.Left)
                    {
                        Left = startpos.X + xdelta;
                        Width = startsize.Width - xdelta;
                    }
                    else if (dp == DragPos.Right)
                        Width = startsize.Width + xdelta;
                    else
                    {
                        Width = startsize.Width + xdelta;
                        Height = startsize.Height + ydelta;
                    }

                    ResumeLayout();
                    Invalidate();
                    //System.Diagnostics.Debug.WriteLine("Drag " + Location + " " + Size);
                }
            }
            else
            {
                StopDrag();
            }

        }

        private void UserControlContainerResizable_MouseUp(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Mouse up " + dp);
            StopDrag();
        }

        void StopDrag()
        {
            //System.Diagnostics.Debug.WriteLine("Stop drag " + dp + " " + dragmoved);
            if (dragmoved)
                Cursor.Current = Cursors.Default;

            if ( dp != DragPos.None )
                ResizeEnd?.Invoke(this, dragmoved);

            dragmoved = false;
            dp = DragPos.None;
        }
    }
}
