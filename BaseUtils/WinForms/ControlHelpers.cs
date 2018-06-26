using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Form;

public static class ControlHelpersStaticFunc
{
    #region Control

    static public void DisposeTree(this Control c, int lvl)     // pass lvl = 0 to dispose of this object itself..
    {
        //System.Diagnostics.Debug.WriteLine(lvl + " at " + c.GetType().Name + " " + c.Name);
        List<Control> todispose = new List<Control>();

        foreach (Control s in c.Controls)
        {
            if (s.Controls.Count > 0)
            {
                //System.Diagnostics.Debug.WriteLine(lvl + " Go into " + s.GetType().Name + " " + s.Name);
                s.DisposeTree(lvl + 1);
            }

            if (!(s is SplitterPanel))        // owned by their SCs..
                todispose.Add(s);
        }

        foreach (Control s in todispose)
        {
            //System.Diagnostics.Debug.WriteLine(lvl + " Dispose " + s.GetType().Name + " " + s.Name);
            s.Dispose();
        }

        if ( lvl ==0 && !( c is SplitterPanel))
        {
            //System.Diagnostics.Debug.WriteLine(lvl + " Dispose " + c.GetType().Name + " " + c.Name);
            c.Dispose();
        }

    }

    static public void DumpTree(this Control c, int lvl)
    {
        System.Diagnostics.Debug.WriteLine("                                             ".Substring(0,lvl*2) + "Control " + c.GetType().Name + ":" + c.Name);

        foreach (Control s in c.Controls)
        {
            s.DumpTree(lvl + 1);
        }
    }

    static public int RunActionOnTree(this Control c, Predicate<Control> cond, Action<Control> action )       // Given a condition, run action on it, count instances
    {
        //System.Diagnostics.Debug.WriteLine("Raot: " + c.Parent.GetType().Name + "->" + c.GetType().Name + ":" + c.Name);
        bool istype = cond(c);

        if (istype)
        {
            //System.Diagnostics.Debug.WriteLine("Action on " + c.GetType().Name + " " + c.Name + " ==? " + istype);
            action(c);
        }

        int total = istype ? 1 : 0;

        foreach (Control s in c.Controls)                   // all sub controls get a chance to play!
            total += RunActionOnTree(s, cond, action);

        return total;
    }

    // DO a refresh after this. presumes you have sorted the order of controls added in the designer file
    // from C, offset either up/down dependent on on.  Remember in tag of c direction you shifted.  Don't shift if in same direction
    // useful for autolayout forms
    static public void ShiftControls(this Control.ControlCollection coll, Control c, int offset, bool on)
    {
        bool enabled = false;
        bool prevon = false;
        foreach (Control ctrl in coll)
        {
            if (ctrl == c)
            {
                prevon = (ctrl.Tag == null) ? true : (bool)ctrl.Tag;
                ctrl.Tag = on;
                enabled = prevon != on;
                //System.Diagnostics.Debug.WriteLine("Decided for enable " + enabled + " to " + on);
            }

            if (enabled)
            {
                ctrl.Location = new Point(ctrl.Left, ctrl.Top + ((on) ? offset : -offset));
                //System.Diagnostics.Debug.WriteLine("Control " + ctrl.Name + " to " + ctrl.Location + " offset " + offset + " on " + on);
            }
        }
    }

    #endregion

    #region Misc

    static public StringFormat StringFormatFromContentAlignment(ContentAlignment c)
    {
        StringFormat f = new StringFormat();
        if (c == ContentAlignment.BottomCenter || c == ContentAlignment.MiddleCenter || c == ContentAlignment.TopCenter)
            f.Alignment = StringAlignment.Center;
        else if (c == ContentAlignment.BottomLeft || c == ContentAlignment.MiddleLeft || c == ContentAlignment.TopLeft)
            f.Alignment = StringAlignment.Near;
        else
            f.Alignment = StringAlignment.Far;

        if (c == ContentAlignment.BottomCenter || c == ContentAlignment.BottomLeft || c == ContentAlignment.BottomRight)
            f.LineAlignment = StringAlignment.Far;
        else if (c == ContentAlignment.MiddleLeft || c == ContentAlignment.MiddleCenter || c == ContentAlignment.MiddleRight)
            f.LineAlignment = StringAlignment.Center;
        else
            f.LineAlignment = StringAlignment.Near;

        return f;
    }

    static public Rectangle ImagePositionFromContentAlignment(this ContentAlignment c, Rectangle client, Size image, bool cliptorectangle = false)
    {
        int left = client.Left;

        if (c == ContentAlignment.BottomCenter || c == ContentAlignment.MiddleCenter || c == ContentAlignment.TopCenter)
            left += Math.Max((client.Width - image.Width) / 2, 0);
        else if (c == ContentAlignment.BottomLeft || c == ContentAlignment.MiddleLeft || c == ContentAlignment.TopLeft)
            left += 0;
        else
            left += Math.Max(client.Width - image.Width, 0);

        int top = client.Top;

        if (c == ContentAlignment.BottomCenter || c == ContentAlignment.BottomLeft || c == ContentAlignment.BottomRight)
            top += Math.Max(client.Height - image.Height, 0);
        else if (c == ContentAlignment.MiddleLeft || c == ContentAlignment.MiddleCenter || c == ContentAlignment.MiddleRight)
            top += Math.Max((client.Height - image.Height) / 2, 0);
        else
            top += 0;

        if (cliptorectangle)        // ensure we start in rectangle..
        {
            left = Math.Max(0, left);
            top = Math.Max(0, top);
        }

        return new Rectangle(left, top, image.Width, image.Height);
    }

    static public GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright)
    {
        GraphicsPath gr = new GraphicsPath();

        gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);
        gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
        gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
        gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
        gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
        return gr;
    }

    // produce a rounded rectangle with a cut out at the top..

    static public GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright, int topcutpos, int topcutlength)
    {
        GraphicsPath gr = new GraphicsPath();

        if (topcutlength > 0)
        {
            gr.AddLine(x + roundnessleft, y, x + topcutpos, y);
            gr.StartFigure();
            gr.AddLine(x + topcutpos + topcutlength, y, x + width - 1 - roundnessright, y);
        }
        else
            gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);

        gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
        gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
        gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
        gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
        return gr;
    }

    static public System.ComponentModel.IContainer GetParentContainerComponents(this Control p)
    {
        IContainerControl c = p.GetContainerControl();  // get container control (UserControl or Form)

        if (c != null)  // paranoia in case control is not connected
        {
            // find all fields, incl private of them
            var memcc = c.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            var icontainers = (from f in memcc      // pick out a list of IContainers (should be only 1)
                               where f.FieldType.FullName == "System.ComponentModel.IContainer"
                               select f);
            return icontainers?.FirstOrDefault()?.GetValue(c) as System.ComponentModel.IContainer;  // But IT may be null if no containers are on the form
        }

        return null;
    }

    static public void CopyToolTips( this System.ComponentModel.IContainer c, Control outerctrl, Control[] ctrlitems)
    {
        if ( c != null )
        {
            var clisttt = c.Components.OfType<ToolTip>().ToList(); // find all tooltips

            foreach (ToolTip t in clisttt)
            {
                string s = t.GetToolTip(outerctrl);
                if (s != null && s.Length>0)
                {
                    foreach (Control inner in ctrlitems)
                        t.SetToolTip(inner, s);
                }
            }
        }
    }

    // used to compute ImageAttributes, given a disabled scaling, a remap table, and a optional color matrix
    static public void ComputeDrawnPanel( out ImageAttributes Enabled, 
                    out ImageAttributes Disabled, 
                    float disabledscaling, System.Drawing.Imaging.ColorMap[] remap, float[][] colormatrix = null)
    {
        Enabled = new ImageAttributes();
        Enabled.SetRemapTable(remap, ColorAdjustType.Bitmap);
        if (colormatrix != null)
            Enabled.SetColorMatrix(new ColorMatrix(colormatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        Disabled = new ImageAttributes();
        Disabled.SetRemapTable(remap, ColorAdjustType.Bitmap);

        if (colormatrix != null)
        {
            colormatrix[0][0] *= disabledscaling;     // the identity positions are scaled by BDS 
            colormatrix[1][1] *= disabledscaling;
            colormatrix[2][2] *= disabledscaling;
            Disabled.SetColorMatrix(new ColorMatrix(colormatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }
        else
        {
            float[][] disabledMatrix = {
                        new float[] {disabledscaling,  0,  0,  0, 0},        // red scaling factor of BDS
                        new float[] {0,  disabledscaling,  0,  0, 0},        // green scaling factor of BDS
                        new float[] {0,  0,  disabledscaling,  0, 0},        // blue scaling factor of BDS
                        new float[] {0,  0,  0,  1, 0},        // alpha scaling factor of 1
                        new float[] {0,0,0, 0, 1}};    // three translations of 0

            Disabled.SetColorMatrix(new ColorMatrix(disabledMatrix), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }
    }

    static public Point PositionWithinScreen(this Control p, int x, int y)      // clamp to withing screen of control
    {
        Screen scr = Screen.FromControl(p);
        Rectangle scrb = scr.Bounds;
        //System.Diagnostics.Debug.WriteLine("Screen is " + scrb);
        x = Math.Min(Math.Max(x, scrb.Left), scrb.Right - p.Width);
        y = Math.Min(Math.Max(y, scrb.Top), scrb.Bottom - p.Height);
        return new Point(x, y);
    }

    static public Point PositionWithinRectangle(this Point p, Size ps, Rectangle other)      // clamp to within client rectangle of another
    {
        return new Point(Math.Min(p.X, other.Width - ps.Width),                   // respecting size, ensure we are within the rectangle of another
                                Math.Min(p.Y, other.Height - ps.Height));
    }

    #endregion

    #region Splitter

    static public void SplitterDistance(this SplitContainer sp, double value)           // set the splitter distance from a double value.. safe from exceptions.
    {
        if (!double.IsNaN(value) && !double.IsInfinity(value))
        {
            int a = (sp.Orientation == Orientation.Vertical) ? sp.Width : sp.Height;
            int curDist = sp.SplitterDistance;
            if (a == 0)     // Sometimes the size is {0,0} if minimized. Calc dimension from the inner panels. See issue #1508.
                a = (sp.Orientation == Orientation.Vertical ? sp.Panel1.Width + sp.Panel2.Width : sp.Panel1.Height + sp.Panel2.Height) + sp.SplitterWidth;
            sp.SplitterDistance = Math.Min(Math.Max((int)Math.Round(a * value), sp.Panel1MinSize), a - sp.Panel2MinSize);
            //System.Diagnostics.Debug.WriteLine($"SplitContainer {sp.Name} {sp.DisplayRectangle} {sp.Panel1MinSize}-{sp.Panel2MinSize} Set SplitterDistance to {value:N2} (was {curDist}, now {sp.SplitterDistance})");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"SplitContainer {sp.Name} {sp.DisplayRectangle} {sp.Panel1MinSize}-{sp.Panel2MinSize} Set SplitterDistance attempted with unsupported value ({value})");
        }
    }

    static public double GetSplitterDistance(this SplitContainer sp)                    // get the splitter distance as a fractional double
    {
        int a = (sp.Orientation == Orientation.Vertical) ? sp.Width : sp.Height;
        if (a == 0)     // Sometimes the size is {0,0} if minimized. Calc dimension from the inner panels. See issue #1508.
            a = (sp.Orientation == Orientation.Vertical ? sp.Panel1.Width + sp.Panel2.Width : sp.Panel1.Height + sp.Panel2.Height) + sp.SplitterWidth;
        double v = (double)sp.SplitterDistance / (double)a;
        //System.Diagnostics.Debug.WriteLine($"SplitContainer {sp.Name} {sp.DisplayRectangle} {sp.SplitterDistance} Get SplitterDistance {a} -> {v:N2}");
        return v;
    }

    static public double GetPanelsSizeSum(this SplitContainer sp)                    // get the splitter panels size sum
    {
        int a = (sp.Orientation == Orientation.Vertical) ? sp.Width : sp.Height;
        if (a == 0)     // Sometimes the size is {0,0} if minimized. Calc dimension from the inner panels. See issue #1508.
            a = (sp.Orientation == Orientation.Vertical ? sp.Panel1.Width + sp.Panel2.Width : sp.Panel1.Height + sp.Panel2.Height) + sp.SplitterWidth;
        double s = (double)a;
        //System.Diagnostics.Debug.WriteLine($"SplitContainer {sp.Name} {sp.DisplayRectangle} {sp.SplitterDistance} Get PanelSizeSum {a} -> {s:N2}");
        return s;
    }

    // Make a tree of splitters, controlled by the string in sp

    static public SplitContainer SplitterTreeMakeFromCtrlString(BaseUtils.StringParser sp, 
                                                            Func<Orientation, int, SplitContainer> MakeSC, 
                                                            Func<string, Control> MakeNode , int lvl)
    {
        char tomake;
        if (sp.SkipUntil(new char[] { 'H', 'V', 'U' }) && (tomake = sp.GetChar()) != 'U')
        {
            sp.IsCharMoveOn('(');   // ignore (

            SplitContainer sc = MakeSC(tomake == 'H' ? Orientation.Horizontal : Orientation.Vertical, lvl);

            double percent = sp.NextDouble(",") ?? 0.5;
            sc.SplitterDistance(percent);
            
            SplitContainer one = SplitterTreeMakeFromCtrlString(sp, MakeSC, MakeNode, lvl+1);

            if (one == null)
            {
                string para = sp.PeekChar() == '\'' ? sp.NextQuotedWord() : "";
                sc.Panel1.Controls.Add(MakeNode(para));
            }
            else
                sc.Panel1.Controls.Add(one);

            SplitContainer two = SplitterTreeMakeFromCtrlString(sp, MakeSC, MakeNode, lvl+1);

            if (two == null)
            {
                string para = sp.PeekChar() == '\'' ? sp.NextQuotedWord() : "";
                sc.Panel2.Controls.Add(MakeNode(para));
            }
            else
                sc.Panel2.Controls.Add(two);

            return sc;
        }
        else
            return null;
    }

    // Report control state of a tree of splitters

    static public string SplitterTreeState(this SplitContainer sc, string cur, Func<Control, string> getpara)
    {
        string state = sc.Orientation == Orientation.Horizontal ? "H( " : "V( ";
        state += sc.GetSplitterDistance().ToStringInvariant("0.##") + ", ";

        SplitContainer one = sc.Panel1.Controls[0] as SplitContainer;

        if (one != null)
        {
            string substate = SplitterTreeState(one, "", getpara);
            state = state + substate;
        }
        else
            state += "U'" + getpara(sc.Panel1.Controls[0]) + "'";

        state += ", ";
        SplitContainer two = sc.Panel2.Controls[0] as SplitContainer;

        if (two != null)
        {
            string substate = SplitterTreeState(two, "", getpara);
            state = state + substate;
        }
        else
            state += "U'" + getpara(sc.Panel2.Controls[0]) + "'";

        state += ") ";

        return state;
    }

    // Run actions at each Splitter Panel node

    static public void RunActionOnSplitterTree(this SplitContainer sc, Action<SplitterPanel, Control> action)       
    {
        SplitContainer one = sc.Panel1.Controls[0] as SplitContainer;

        if (one != null)
            RunActionOnSplitterTree(one, action);
        else
            action(sc.Panel1, sc.Panel1.Controls[0]);

        SplitContainer two = sc.Panel2.Controls[0] as SplitContainer;

        if (two != null)
            RunActionOnSplitterTree(two, action);
        else
            action(sc.Panel2, sc.Panel2.Controls[0]);
    }


    static public void Merge(this SplitContainer currentsplitter , int panel )      // currentsplitter has a splitter underneath it in panel (0/1)
    {
        SplitContainer tomerge = currentsplitter.Controls[panel].Controls[0] as SplitContainer;  // verified by enable on open

        Control keep = tomerge.Controls[0].Controls[0];      // we keep this tree..

        tomerge.Controls[0].Controls.Clear();    // clear control list - we want to keep these..
        tomerge.DisposeTree(0);               // tree dispose of all other stuff left, and dispose of tomerge.

        currentsplitter.Controls[panel].Controls.Add(keep);
    }

    static public void Split(this SplitContainer currentsplitter, int panel ,  SplitContainer sc, Control ctrl )    // currentsplitter, split panel into a SC with a ctrl
    {
        Control cur = currentsplitter.Controls[panel].Controls[0];      // what we current have attached..
        currentsplitter.Controls[panel].Controls.Clear();   // clear list
        sc.Panel1.Controls.Add(cur);
        sc.Panel2.Controls.Add(ctrl);
        currentsplitter.Controls[panel].Controls.Add(sc);
    }

    #endregion

    #region Data Grid Views

    static public void SortDataGridViewColumnNumeric(this DataGridViewSortCompareEventArgs e, string removetext= null)
    {
        string s1 = e.CellValue1?.ToString();
        string s2 = e.CellValue2?.ToString();

        if (removetext != null)
        {
            if ( s1 != null )
                s1 = s1.Replace(removetext, "");
            if ( s2 != null )
                s2 = s2.Replace(removetext, "");
        }

        double v1=0, v2=0;

        bool v1hasval = s1 != null && Double.TryParse(s1, out v1);
        bool v2hasval = s2 != null && Double.TryParse(s2, out v2);

        if (!v1hasval)
        {
            e.SortResult = 1;
        }
        else if (!v2hasval)
        {
            e.SortResult = -1;
        }
        else
        {
            e.SortResult = v1.CompareTo(v2);
        }

        e.Handled = true;
    }

    static public void SortDataGridViewColumnDate(this DataGridViewSortCompareEventArgs e)
    {
        string s1 = e.CellValue1?.ToString();
        string s2 = e.CellValue2?.ToString();

        DateTime v1 = DateTime.MinValue, v2 = DateTime.MinValue;

        bool v1hasval = s1!=null && DateTime.TryParse(e.CellValue1?.ToString(), out v1);
        bool v2hasval = s2!=null && DateTime.TryParse(e.CellValue2?.ToString(), out v2);

        if (!v1hasval)
        {
            e.SortResult = 1;
        }
        else if (!v2hasval)
        {
            e.SortResult = -1;
        }
        else
        {
            e.SortResult = v1.CompareTo(v2);
        }

        e.Handled = true;
    }

    // try and force this row to centre or top
    static public void DisplayRow(this DataGridView grid, int rown, bool centre)
    {
        int drows = centre ? grid.DisplayedRowCount(false) : 0;

        while (!grid.Rows[rown].Displayed && drows >= 0)
        {
            //System.Diagnostics.Debug.WriteLine("Set row to " + Math.Max(0, rowclosest - drows / 2));
            grid.FirstDisplayedScrollingRowIndex = Math.Max(0, rown - drows / 2);
            grid.Update();      //FORCE the update so we get an idea if its displayed
            drows--;
        }
    }

    public static void FilterGridView(this DataGridView vw, string searchstr)       // can be VERY SLOW for large grids
    {
        vw.SuspendLayout();
        vw.Enabled = false;

        bool[] visible = new bool[vw.RowCount];
        bool visibleChanged = false;

        foreach (DataGridViewRow row in vw.Rows.OfType<DataGridViewRow>())
        {
            bool found = false;

            if (searchstr.Length < 1)
                found = true;
            else
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        if (cell.Value.ToString().IndexOf(searchstr, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            visible[row.Index] = found;
            visibleChanged |= found != row.Visible;
        }

        if (visibleChanged)
        {
            var selectedrow = vw.SelectedRows.OfType<DataGridViewRow>().Select(r => r.Index).FirstOrDefault();
            DataGridViewRow[] rows = vw.Rows.OfType<DataGridViewRow>().ToArray();
            vw.Rows.Clear();

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].Visible = visible[i];
            }

            vw.Rows.Clear();
            vw.Rows.AddRange(rows.ToArray());

            vw.Rows[selectedrow].Selected = true;
        }

        vw.Enabled = true;
        vw.ResumeLayout();
    }


    #endregion
}
