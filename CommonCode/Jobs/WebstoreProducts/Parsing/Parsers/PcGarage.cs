using Jobs.WebstoreProducts.DTOs;
using System;
using System.Net;

namespace Jobs.WebstoreProducts.Parsing.Parsers
{
    public class PcGarage : Parser
    {
        private const string REGEX_TITLE = "<title>(.*)</title>";
        private const string REGEX_IMAGE = "<img data-rel=\"gallery0\" id=\".+?\" src=\"(.+?)\"";
        private const string REGEX_CURRENT_PRICE = "<meta itemprop=\"price\" content=\"([\\d\\.,]+)\" ?\\/>";
        private const string REGEX_OLD_PRICE = "Pret vechi: <span>([\\d\\.,]+) ?RON<\\/span>";

        public override PriceDTO ParseInternal(WebClient web, ProductDTO product, string page)
        {
            product.FullName = RegexItem("title/full name", REGEX_TITLE, page, 1).Replace("- PC Garage", "").Trim();

            string imageURL = RegexItem("thumbnail/image URL", REGEX_IMAGE, page, 1);
            DownloadWebstoreProductThumbnail(web, product, imageURL);

            string currentPriceS = RegexItem("current price", REGEX_CURRENT_PRICE, page, 1);
            double currentPrice = double.Parse(currentPriceS);

            string oldPriceS = RegexItem("old price", REGEX_OLD_PRICE, page, 1, "0.0");
            double oldPrice = double.Parse(oldPriceS.Replace(",", "."));
            if (oldPrice == 0.0)
                oldPrice = currentPrice;

            return new PriceDTO()
            {
                Moment = DateTime.Now,
                ProductID = product.ID,
                Product = product,
                Currency = "lei",
                PreviousPrice = oldPrice,
                CurrentPrice = currentPrice
            };
        }
    }
}
