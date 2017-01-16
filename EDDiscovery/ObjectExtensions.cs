public static class ObjectExtensions
{
    public static string ToNullSafeString(this object obj)
        {
        return (obj ?? string.Empty).ToString();
    }

    public static string QuotedEscapeString(this string obj )
    {
        if (obj.Contains("\"")||obj.Contains(" "))
            obj = "\"" + obj.Replace("\"", "\\\"") + "\"";
        return obj;
    }

    public static int FirstCharNonWhiteSpace(this string obj )
    {
        int i = 0;
        while (i < obj.Length && char.IsWhiteSpace(obj[i]))
            i++;
        return i;
    }
}

