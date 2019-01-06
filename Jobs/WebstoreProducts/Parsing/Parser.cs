using DataLayer;
using Jobs.WebstoreProducts.DTOs;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Jobs.WebstoreProducts.Parsing
{
    public abstract class Parser
    {
        public virtual int ThreadSleepDuration
            => 2000;

        public void ParseProductPrice(in WebClient web, in ProductDTO product, out PriceDTO price, out string error)
        {
            try
            {
                web.DownloadFile(product.URL, Const.FILE_TEMP_HTML);
                string page = File.ReadAllText(Const.FILE_TEMP_HTML);
                price = ParseInternal(web, product, page);
                error = null;
            }
            catch (Exception e)
            {
                price = null;
                error = e.ToString();
            }
        }

        public abstract PriceDTO ParseInternal(in WebClient web, in ProductDTO product, string page);

        protected void DownloadWebstoreProductThumbnail(in WebClient web, in ProductDTO product, in string imageURL)
        {
            if (string.IsNullOrWhiteSpace(imageURL))
                throw new ApplicationException("image URL is not valid (hey, should this be an exception?)");
            string path = Path.Combine(Const.FOLDER_WEBSTORE_PRODUCT_THUMBNAILS, $"{product.ID}.jpg");
            if (!File.Exists(path) || false)
                web.DownloadFile(imageURL, path);
        }

        protected string RegexItem(string infoName, string regex, string page, int group = 0, string resultOnRegexFail = null)
        {
            Match match = Regex.Match(page, regex);
            if (!match.Success)
            {
                if (resultOnRegexFail == null)
                    throw new Exception($"The {infoName} information could not be parsed from the webpage contents via regex");
                return resultOnRegexFail;
            }
            return WebUtility.HtmlDecode(match.Groups[group].Value);
        }
    }
}