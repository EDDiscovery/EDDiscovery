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
}