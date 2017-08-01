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
        public Control control;
        public Action<UserControlContainerResizable> ResizeStart;
        public Action<UserControlContainerResizable> ResizeEnd;
        public bool Selected { get { return selected; } set { SetSelected(value); } }

        private const int margin = 3;
        private bool selected = false;

        public UserControlContainerResizable()
        {
            InitializeComponent();
        }

        public void Init( UserControlCommonBase c)
        {
            control = c;
            Controls.Add(control);
            PerformLayout();
        }

        private void SetSelected(bool s)
        {
            selected = s;
            Invalidate();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            control.Location = new Point(margin, margin);
            control.Size = new Size(ClientRectangle.Width - margin * 2, ClientRectangle.Height - margin * 2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Pen pb = new Pen(Color.Black, 1.0F))
            {
                using (Pen p1 = new Pen(Selected ? Color.Green : Color.Red, 1.0F))
                {
                    Rectangle r = ClientRectangle;
                    r.Inflate(-1, -1);
                    e.Graphics.DrawRectangle(p1, r);
                    r.Inflate(-1, -1);
                    e.Graphics.DrawRectangle(pb, r);
                    r.Inflate(-1, -1);
                    e.Graphics.DrawRectangle(pb, r);
                }
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
            System.Diagnostics.Debug.WriteLine("Drag start here " + dp);

            ResizeStart?.Invoke(this);
        }

        private void UserControlContainerResizable_MouseMove(object sender, MouseEventArgs e)
        {
            if (dp == DragPos.None)
            {
                DragType(e);
                
            }
            else
            {
                int xdelta = Control.MousePosition.X - dragstart.X;
                int ydelta = Control.MousePosition.Y - dragstart.Y;
                int absxdelta = Math.Abs(xdelta);
                int absydelta = Math.Abs(ydelta);

                System.Diagnostics.Debug.WriteLine("Drag " + dp + " moved " + dragmoved + " delta " + absxdelta + "," + absydelta);

                if (dragmoved == false)
                {
                    if (dp == DragPos.Top || dp == DragPos.Bottom)
                        dragmoved = absydelta > margin;
                    else
                        dragmoved = absxdelta > margin;
                }

                if (dragmoved)
                {
                    SuspendLayout();
                    if (dp == DragPos.Top)
                    {
                        Top = startpos.Y + ydelta;
                        Left = startpos.X + xdelta;
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
                    System.Diagnostics.Debug.WriteLine("Drag " + Location + " " + Size);
                }
            }
        }

        private void UserControlContainerResizable_MouseUp(object sender, MouseEventArgs e)
        {
            if ( dp != DragPos.None )
            {
                if (dragmoved)
                {
                    ResizeEnd?.Invoke(this);
                    Cursor.Current = Cursors.Default;
                }

                dragmoved = false;
                dp = DragPos.None;
            }
        }
    }
}
