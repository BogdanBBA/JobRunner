using System;
using System.Security.Principal;

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

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>Attempts to parse https://ipinfo.io/ip and returns the IP as a string if successful, or a message otherwise.</summary>
        public static string GetExternalIp(bool includeErrorDescription = false)
        {
            try
            {
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    webClient.DownloadFile(@"https://ipinfo.io/ip", DataLayer.Const.FILE_TEMP_HTML);
                    string stringIpResult = System.IO.File.ReadAllText(DataLayer.Const.FILE_TEMP_HTML).Replace(Environment.NewLine, "").Replace("\n", "");
                    return stringIpResult;
                }
            }
            catch (Exception e)
            {
                return $"A {e?.GetType()?.Name} error has occured while attempting to obtain the external IP address{(includeErrorDescription ? $" ({e})" : "")}.";
            }
        }
    }
}
