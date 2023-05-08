namespace UtilityLibraries
{
    public static class StringLibrary
    {
        private const string TruncationSuffix = "...";

        public static string Truncate(this string str, uint maxLength)
        {
            if (str.Length <= maxLength)
                return str;
            else
                return str.Substring(0, (int)(maxLength - 3)) + TruncationSuffix;
        }

        public static string FormatDateTime(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy hh:mm tt");
        }
    }
}