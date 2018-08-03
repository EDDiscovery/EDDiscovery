using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ObjectExtensionsDates
{
    // formatter is string with embedded [s text]..
    // s = seconds, printed if s>0
    // S = seconds, printed if s>0 and m=h=d=0
    // Sh = seconds, printed if s>0 and h=d=0
    // m = mins, printed if m>0
    // M = mins, printed if m>0 and h=d=0
    // Md = mins, printed if m>0 and d=0
    // h = hours, printed if h>0
    // H = hours, printed if h>0 and d=0
    // d = days, printed if d>0
    // D = Date, in formatoptions, if given

    public static string ToStringTimeDeltaFormatted(this double diff, string before, string after, DateTime? date = null, string formatoptions = null)
    {
        string fmt = (diff > 0) ? before : after;
        diff = Math.Abs(diff);

        int seconds = (int)(diff % 60);
        int minutes = (int)((diff / 60) % 60);
        int hours = (int)((diff / 3600) % 24);
        int days = (int)(diff / 3600 / 24);

        fmt = fmt.ReplaceArea("[s", "]", (seconds > 0) ? seconds.ToStringInvariant() : "", (seconds > 0) ? 2 : 0, seconds > 1);
        bool pSh = seconds > 0 && hours == 0 && days == 0;
        fmt = fmt.ReplaceArea("[Sh", "]", pSh ? seconds.ToStringInvariant() : "", pSh ? 3 : 0, seconds > 1);
        bool pS = seconds > 0 && minutes == 0 && hours == 0 && days == 0;
        fmt = fmt.ReplaceArea("[S", "]", pS ? seconds.ToStringInvariant() : "", pS ? 2 : 0, seconds > 1);

        fmt = fmt.ReplaceArea("[m", "]", (minutes > 0) ? minutes.ToStringInvariant() : "", (minutes > 0) ? 2 : 0, minutes > 1);
        bool pMd = minutes > 0 && days == 0;
        fmt = fmt.ReplaceArea("[Md", "]", pMd ? minutes.ToStringInvariant() : "", pMd ? 3 : 0, minutes > 1);
        bool pM = minutes > 0 && hours == 0 && days == 0;
        fmt = fmt.ReplaceArea("[M", "]", pM ? minutes.ToStringInvariant() : "", pM ? 2 : 0, minutes > 1);

        fmt = fmt.ReplaceArea("[h", "]", (hours > 0) ? hours.ToStringInvariant() : "", (hours > 0) ? 2 : 0, hours > 1);
        bool pH = hours > 0 && days == 0;
        fmt = fmt.ReplaceArea("[H", "]", pH ? hours.ToStringInvariant() : "", pH ? 2 : 0, hours > 1);

        fmt = fmt.ReplaceArea("[d", "]", (days > 0) ? days.ToStringInvariant() : "", (days > 0) ? 2 : 0, days > 1);

        bool validdte = (date != null && formatoptions != null);
        fmt = fmt.ReplaceArea("[D", "]", validdte ? date.Value.ToStringFormatted(formatoptions) : "", validdte ? 2 : 0);

        return fmt;
    }

    //format options are semicoloned.
    public static DateTime? ParseUSDateTimeNull(this string value, string formatoptions = null)   // formatoptions = Local only.  
    {
        DateTime res;

        System.Globalization.DateTimeStyles dts = System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal;

        string[] t = formatoptions?.ToLowerInvariant().Split(';');

        if (t != null && Array.IndexOf(t, "local") != -1)
        {
            dts = System.Globalization.DateTimeStyles.AssumeLocal;
        }

        // presuming its univeral means no translation in the values to local.
        if (DateTime.TryParse(value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), dts, out res))
            return res;
        else
            return null;
    }

    //format options are semicoloned.
    public static string ToStringFormatted(this DateTime res, string formatoptions)
    {
        string[] t = formatoptions.ToLowerInvariant().Split(';');

        string culture = System.Globalization.CultureInfo.CurrentCulture.Name;

        if (Array.IndexOf(t, "toutc") != -1)
            res = res.ToUniversalTime();
        else if (Array.IndexOf(t, "tolocal") != -1)
            res = res.ToLocalTime();

        string ci = Array.Find(t, x => x.IndexOf("culture:") != -1);
        if (ci != null)
            culture = ci.Substring(8);

        DateTimeFormatInfo dtfi = null;
        try
        {
            dtfi = CultureInfo.GetCultureInfo(culture).DateTimeFormat;
        }
        catch
        {
            return "Culture not defined " + culture;
        }

        System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.CurrentCulture;

        if (Array.IndexOf(t, "longtime") != -1)
        {
            return res.ToString(dtfi.LongTimePattern);
        }
        else if (Array.IndexOf(t, "shorttime") != -1)
        {
            return res.ToString(dtfi.ShortTimePattern);
        }
        else if (Array.IndexOf(t, "shortdate") != -1)
        {
            return res.ToString(dtfi.ShortDatePattern);
        }
        else if (Array.IndexOf(t, "longdate") != -1)
        {
            return res.ToString(dtfi.LongDatePattern);
        }
        else if (Array.IndexOf(t, "longdatetime") != -1)
        {
            return res.ToString(dtfi.LongDatePattern) + " " + res.ToString(dtfi.LongTimePattern);
        }
        else if (Array.IndexOf(t, "datetime") != -1)
        {
            return res.ToString(dtfi.ShortDatePattern) + " " + res.ToString(dtfi.ShortTimePattern);
        }
        else if (Array.IndexOf(t, "ticks") != -1)
        {
            return res.Ticks.ToStringInvariant();
        }
        else
        {
            return res.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }

    public static string ToStringUS(this DateTime dt)     // US fixed format . Use for ACTION programs
    {
        return dt.ToString("MM/dd/yyyy HH:mm:ss");
    }

    public static string ToStringYearFirst(this DateTime dt)     // year first format
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static string ToStringZulu(this DateTime dt)     // zulu warrior format web style
    {
        if (dt.Millisecond != 0)
            return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        else
            return dt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");

    }

    public static string SecondsToString(this int s)
    {
        TimeSpan ts = new TimeSpan(0, 0, s);
        return ts.ToString();
    }

    public static string SecondsToWeeksDaysHoursMinutesSeconds(this int seconds)
    {
        TimeSpan s = TimeSpan.FromSeconds(seconds);
        int days = s.Days % 7;
        int weeks = (s.Days - days) / 7;
        return (weeks>0 ? $"{weeks} weeks " : "" ) + (days>0 ? $"{days} days " : "") + 
                          $"{s.Hours} hours" + (weeks==0 ? $" {s.Minutes} minutes {s.Seconds} seconds" : "");
    }
}


