using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class DraggableFormPos : ExtendedControls.DraggableForm
    {
        public DraggableFormPos()
        {
            InitializeComponent();
        }

        protected bool FormIsMaximised { get {return formMax;} }
        protected string RestoreFormPositionRegKey { private get;  set; } = null;

        private bool formMax;
        private int formWidth;
        private int formHeight;
        private int formTop;
        private int formLeft;

        protected void RestoreFormPosition()
        {
            var top = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Top", -999);

            if (top != -999 && EDDOptions.Instance.NoWindowReposition == false)
            {
                var left = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Left", 0);
                var height = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Height", 800);
                var width = SQLiteDBClass.GetSettingInt(RestoreFormPositionRegKey+"Width", 800);

                System.Diagnostics.Debug.WriteLine("Restore {0},{1} {2},{3}", left, top, width, height);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                System.Diagnostics.Debug.WriteLine("Bounded {0},{1} {2},{3}", left, top, width, height);

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;

                formMax = SQLiteDBClass.GetSettingBool(RestoreFormPositionRegKey+"Max", false);

                if (formMax)
                    this.WindowState = FormWindowState.Maximized;
            }

            formLeft = Left;
            formTop = Top;
            formHeight = Height;
            formWidth = Width;
        }

        protected void SaveFormPosition()
        {
            System.Diagnostics.Debug.WriteLine("Store {0},{1} {2},{3}", formLeft, formTop, formWidth, formHeight);
            SQLiteDBClass.PutSettingBool(RestoreFormPositionRegKey+"Max", formMax);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Width", formWidth);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Height", formHeight);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Top", formTop);
            SQLiteDBClass.PutSettingInt(RestoreFormPositionRegKey+"Left", formLeft);
        }

        protected void RecordFormPosition()     // HOOK into Resize (for Max) AND ResizeEnd (for drag and size)
        {
            System.Diagnostics.Debug.WriteLine("Resize Event {0} {1},{2} {3},{4}", WindowState, Left, Top, Width, Height);

            if (FormWindowState.Maximized == WindowState)       // if maximized, note..
            {
                formMax = true;
            }
            else if (FormWindowState.Normal == WindowState) // if normal, size size..  If minimise, don't save
            {
                formLeft = this.Left;
                formTop = this.Top;
                formWidth = this.Width;
                formHeight = this.Height;
                formMax = false;
            }
        }
    }
}
