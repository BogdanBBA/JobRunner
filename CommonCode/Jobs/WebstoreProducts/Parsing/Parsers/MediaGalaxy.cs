using Jobs.WebstoreProducts.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Jobs.WebstoreProducts.Parsing.Parsers
{
    public class MediaGalaxy : Parser
    {
        private const string REGEX_TITLE = "<title.*?>(.*)</title>";
        private const string REGEX_JSON_SCRIPT = "<input type=\"hidden\" name=\"product\" value=\"(\\d+)\"";
        private const string REGEX_PRICE = "<span class=\"Price-int\">(\\d+)<\\/span>";
        private const string REGEX_PRICE_DECIMALS = "<sup class=\"Price-dec\">.*?(\\d+).*?<\\/sup>";
        private const string REGEX_OLD_PRICE = "<div class=\"Price-old\">((\\d+\\.)?\\d+(,\\d+)?)<\\/div>";
        private const string REGEX_IMAGE = "<meta property=\"og:image\"[ \t]+?content=\"(.+?)\"";
        private const string REGEX_DOUBLE_NUMBER = @"-?(\d+\.)?\d+(\,\d+)?";

        public override PriceDTO ParseInternal(System.Net.WebClient web, ProductDTO product, string page)
        {
            product.FullName = RegexItem("title/full name", REGEX_TITLE, page, 1).Trim();

            string imageURL = RegexItem("thumbnail/image URL", REGEX_IMAGE, page, 1);
            DownloadWebstoreProductThumbnail(web, product, imageURL);

            string priceMatch = RegexItem("integer part of the price", REGEX_PRICE, page, 1);
            double priceInteger = double.Parse(priceMatch);

            string priceDecimalMatch = RegexItem("decimal part of the price", REGEX_PRICE_DECIMALS, page, 1);
            double priceDecimal = double.Parse(priceDecimalMatch) / 100.0;

            string oldPriceMatch = RegexItem("old price", REGEX_OLD_PRICE, page, 1, "0.0");
            double oldPrice = double.Parse(oldPriceMatch.Replace(",", "."));
            if (oldPrice == 0.0)
                oldPrice = priceInteger + priceDecimal;

            return new PriceDTO()
            {
                Moment = DateTime.Now,
                ProductID = product.ID,
                Product = product,
                Currency = "lei",
                PreviousPrice = oldPrice,
                CurrentPrice = priceInteger + priceDecimal
            };
        }
    }
}
