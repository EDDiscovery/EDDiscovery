public static class ObjectExtensions
{
    public static string ToNullSafeString(this object obj)
        {
        return (obj ?? string.Empty).ToString();
    }
}