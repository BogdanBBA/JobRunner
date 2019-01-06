using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCode
{
    public static class ExtensionMethods
    {
        public static string ToSqlDateTime(this DateTime dateTime)
            => $"DATETIME('{dateTime:yyyy-MM-dd HH:mm:ss}')";

        public static string ToSqlDate(this DateTime dateTime)
            => $"DATE('{dateTime:yyyy-MM-dd}')";

        public static string ToYesNo(this bool boolean)
            => boolean ? "Yes" : "No";

        public static int To01(this bool boolean)
            => boolean ? 1 : 0;

        public static string FormatBytes(this long bytes)
        {
            string[] suffixes = new string[] { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double result = bytes;
            while (result >= 1024.0 && order < suffixes.Length - 1)
            {
                result /= 1024.0;
                order++;
            }
            return $"{result:N1} {suffixes[order]}";
        }

        public static string ToHMS(this TimeSpan span, bool includeZeroFields = false)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>() { { "h", span.Hours }, { "m", span.Minutes }, { "s", span.Seconds } };
            StringBuilder sb = new StringBuilder();
            foreach (string key in dict.Keys)
                if (includeZeroFields || dict[key] != 0)
                {
                    if (sb.Length > 0)
                        sb.Append(' ');
                    sb.Append(dict[key]).Append(key);
                }
            if (sb.Length == 0)
                sb.Append("0s");
            return sb.ToString();
        }
    }
}
