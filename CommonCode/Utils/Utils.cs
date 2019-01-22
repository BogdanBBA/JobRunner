namespace CommonCode.Utils
{
    public static class Utils
    {
        /// <summary></summary>
        public static string Plural(string singular, int count, bool includeCountInResult)
        {
            return string.Format("{0}{1}{2}", includeCountInResult ? count + " " : "", singular, count == 1 ? "" : "s");
        }

        /// <summary></summary>
        public static string Plural(string singular, string plural, int count, bool includeCountInResult)
        {
            return string.Format("{0}{1}", includeCountInResult ? count + " " : "", count == 1 ? singular : plural);
        }

        /// <summary></summary>
        public static string Conjugation(string singular, string plural, int count)
        {
            return count == 1 ? singular : plural;
        }

        /// <summary>Swaps the references between two variables.</summary>
        public static void Swap<TYPE>(ref TYPE objectA, ref TYPE objectB)
        {
            TYPE aux = objectA;
            objectA = objectB;
            objectB = aux;
        }
    }
}
