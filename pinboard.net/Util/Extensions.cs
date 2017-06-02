namespace pinboard.net.Util
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string instance)
            => string.IsNullOrEmpty(instance);

        public static bool HasValue(this string instance)
            => !IsEmpty(instance);
    }
}
