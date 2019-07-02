using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCode.Utils
{
    public static class ExtensionMethods
    {
        public static string ToSqlDateTime(this DateTime dateTime)
            => $"DATETIME('{dateTime:yyyy-MM-dd HH:mm:ss}')";

        public static string ToSqlDate(this DateTime dateTime)
            => $"DATE('{dateTime:yyyy-MM-dd}')";

        public static string ToSqlTime(this TimeSpan timeSpan, bool includeSeconds = false)
            => $"TIME('{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}{(includeSeconds ? $":{timeSpan.Seconds:D2}" : "")}')";

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

        public static string ToDHMS(this TimeSpan span, bool includeZeroFields = false)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>() { { "d", span.Days }, { "h", span.Hours }, { "m", span.Minutes }, { "s", span.Seconds } };
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

        public static string Cleanup(this string text, bool makeLowercase = true)
        {
            string temp = new string(text.ToCharArray());
            temp = temp.Replace("\t", " ");
            while (temp.Contains("  "))
                temp = temp.Replace("  ", " ");
            temp = temp.Trim().ReplaceDiacritics();
            if (makeLowercase)
                temp = temp.ToLowerInvariant();
            return temp;
        }

        public static string ReplaceDiacritics(this string text)
        {
            var dict = new Dictionary<string, string>() {
                { "ă", "a" }, { "ã", "a" }, { "ā", "a" }, { "â", "a" }, { "î", "i" },
                { "ș", "s" }, { "ş", "s" },
                { "ţ", "t" }, { "ț", "t" }
            };
            string temp = new string(text.ToCharArray());
            foreach (string key in dict.Keys)
                temp = temp.Replace(key, dict[key]).Replace(key.ToUpperInvariant(), dict[key].ToUpperInvariant());
            return temp;
        }

        public static bool ContainsAll<TYPE>(this IEnumerable<TYPE> list, IEnumerable<TYPE> other)
        {
            return !other.ToList().Except(list.ToList()).Any();
        }

        public static bool ContainsNone<TYPE>(this IEnumerable<TYPE> list, IEnumerable<TYPE> other)
        {
            return !list.ToList().Intersect(other.ToList()).Any();
        }

		public static string EnsureEndsWith(this string text, string suffix)
		{
			return text.EndsWith(suffix) ? text : text + suffix;
		}
    }
}
