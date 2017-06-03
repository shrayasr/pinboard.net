using System.Collections.Generic;

namespace pinboard.net.Util
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string instance)
            => string.IsNullOrEmpty(instance);

        public static bool HasValue(this string instance)
            => !IsEmpty(instance);
    }

    public static class ListExtensions
    {
        public static bool HasValues<T>(this List<T> instance)
        {
            if (instance == null)
                return false;
            else
                return (instance.Count > 0);
        }
    }
}
