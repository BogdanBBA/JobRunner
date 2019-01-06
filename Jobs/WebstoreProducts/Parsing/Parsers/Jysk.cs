using Jobs.WebstoreProducts.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Jobs.WebstoreProducts.Parsing.Parsers
{
    public class Jysk : Parser
    {
        private const string REGEX_TITLE = "<title>(.*)</title>";
        private const string REGEX_ONE_PRICE_ONLY = "itemprop=\"price\">\\d Lei/buc</span>";
        private const string REGEX_PREVIOUS_PRICE = "itemprop=\"highPrice\">\\d* Lei/buc</span>";
        private const string REGEX_CURRENT_PRICE = "itemprop=\"lowPrice\">\\d* Lei/buc</span>";
        private const string REGEX_IMAGE = "meta content=\"(.*)\" property=\"og:image\"";

        public override PriceDTO ParseInternal(in System.Net.WebClient web, in ProductDTO product, string page)
        {
            product.FullName = RegexItem("title/full name", REGEX_TITLE, page, 1).Replace("| JYSK", "").Trim();

            string imageURL = RegexItem("thumbnail/image URL", REGEX_IMAGE, page, 1);
            DownloadWebstoreProductThumbnail(web, product, imageURL);

            double previousPrice = 0.0;
            double currentPrice = 0.0;

            Match priceMatch = Regex.Match(page, REGEX_ONE_PRICE_ONLY);
            if (priceMatch.Success)
            {
                string priceS = priceMatch.Value.Substring(priceMatch.Value.IndexOf('>') + 1);
                previousPrice = currentPrice = double.Parse(priceS.Substring(0, priceS.IndexOf(' ')));
            }
            else
            {
                Match previousPriceMatch = Regex.Match(page, REGEX_PREVIOUS_PRICE);
                Match currentPriceMatch = Regex.Match(page, REGEX_CURRENT_PRICE);

                if (!previousPriceMatch.Success || !currentPriceMatch.Success)
                    throw new Exception($"One of the prices (previous={previousPriceMatch.Success}, current={currentPriceMatch.Success}) for the product {product.ShortName} could not be parsed via regex");

                string previousPriceS = previousPriceMatch.Value.Substring(previousPriceMatch.Value.IndexOf('>') + 1);
                previousPrice = double.Parse(previousPriceS.Substring(0, previousPriceS.IndexOf(' ')));
                string currentPriceS = currentPriceMatch.Value.Substring(currentPriceMatch.Value.IndexOf('>') + 1);
                currentPrice = double.Parse(currentPriceS.Substring(0, currentPriceS.IndexOf(' ')));
            }

            return new PriceDTO()
            {
                Moment = DateTime.Now,
                ProductID = product.ID,
                Product = product,
                Currency = "lei",
                PreviousPrice = previousPrice,
                CurrentPrice = currentPrice
            };
        }
    }
}
