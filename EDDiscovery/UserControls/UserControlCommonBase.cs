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
    public enum PopOuts        // id's.. used in tab controls, and in button pop outs button
    {
        // IN TABS
        Log,
        NS,
        Materials,
        Commodities,
        Ledger,
        Journal,
        TravelGrid,
        ScreenShot,
        Statistics,
        Scan,
        // Not in TABS
        Spanel,
        Trippanel,
        NotePanel,
        RouteTracker
    };

    public class UserControlCommonBase : UserControl
    {
        public virtual void Init( EDDiscoveryForm ed, int displayno) { }
        public virtual void Display(HistoryEntry current, HistoryList history) { }

        public virtual void LoadLayout() { }
        public virtual void Closing() { }

        public virtual Color ColorTransparency { get { return Color.Transparent; } }        // override to say support transparency, and what colour you want.
        public virtual void SetTransparency(bool on, Color curcol) { }                      // set on/off transparency of components.

        public void SetControlText(string s)            // used to set heading text in either the form of the tabstrip
        {
            if (this.Parent is Controls.TabStrip)
                ((Controls.TabStrip)(this.Parent)).SetControlText(s);
            else if (this.Parent is Forms.UserControlForm)
                ((Forms.UserControlForm)(this.Parent)).SetControlText(s);
        }

        public bool IsTransparent
        {
            get
            {
                if (this.Parent is Forms.UserControlForm)
                    return ((Forms.UserControlForm)(this.Parent)).istransparent;
                else
                    return false;
            }
        }

        public static UserControlCommonBase Create(PopOuts i)
        {
            switch (i)
            {
                case PopOuts.Log: return new UserControlLog();
                case PopOuts.NS: return new UserControlStarDistance();
                case PopOuts.Materials: return new UserControlMaterials();
                case PopOuts.Commodities: return new UserControlCommodities();
                case PopOuts.Ledger: return new UserControlLedger();
                case PopOuts.Journal: return new UserControlJournalGrid();
                case PopOuts.TravelGrid: return new UserControlTravelGrid();
                case PopOuts.ScreenShot: return new UserControlScreenshot();
                case PopOuts.Statistics: return new UserControlStats();
                case PopOuts.Scan: return new UserControlScan();
                case PopOuts.Spanel: return new UserControlSpanel();
                case PopOuts.Trippanel: return new UserControlTrippanel();
                case PopOuts.NotePanel: return new UserControlNotePanel();
                case PopOuts.RouteTracker: return new UserControlRouteTracker();
                default: return null;
            }
        }


        #region Resize

        public bool inresizeduetoexpand = false;                                            // FUNCTIONS to allow a form to grow temporarily.  Does not work when inside the panels

        public void RequestTemporaryMinimumSize(Size w)         // w is client area
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryMinimiumSize(w);
                inresizeduetoexpand = false;
            }
        }

        public void RequestTemporaryResizeExpand(Size w)        // by this client size
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResizeExpand(w);
                inresizeduetoexpand = false;
            }
        }

        public void RequestTemporaryResize(Size w)              // w is client area
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RequestTemporaryResize(w);
                inresizeduetoexpand = false;
            }
        }

        public void RevertToNormalSize()                        // and to revert
        {
            if (this.Parent is Forms.UserControlForm)
            {
                inresizeduetoexpand = true;
                ((Forms.UserControlForm)(this.Parent)).RevertToNormalSize();
                inresizeduetoexpand = false;
            }
        }

        public bool IsInTemporaryResize                         // have we grown?
        { get
            {
                return (this.Parent is Forms.UserControlForm) ? ((Forms.UserControlForm)(this.Parent)).istemporaryresized : false;
            }
        }

        #endregion

        #region DGV Column helpers - used to save/size the DGV of user controls dynamically.

        public void DGVLoadColumnLayout(DataGridView dgv, string root)
        {
            if (SQLiteConnectionUser.keyExists(root + "1"))        // if stored values, set back to what they were..
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    string k = root + (i + 1).ToString();
                    int w = SQLiteDBClass.GetSettingInt(k, -1);
                    if (w >= 10)        // in case something is up (min 10 pixels)
                        dgv.Columns[i].Width = w;
                    //System.Diagnostics.Debug.WriteLine("Load {0} {1} {2} {3}", Name, k, w, dgv.Columns[i].Width);
                }
            }
        }

        public void DGVSaveColumnLayout(DataGridView dgv, string root)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                string k = root + (i + 1).ToString();
                SQLiteDBClass.PutSettingInt(k, dgv.Columns[i].Width);
                //System.Diagnostics.Debug.WriteLine("Save {0} {1} {2}", Name, k, dgv.Columns[i].Width);
            }
        }

        #endregion
    }
}
