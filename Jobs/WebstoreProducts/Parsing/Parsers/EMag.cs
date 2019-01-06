using Jobs.WebstoreProducts.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Jobs.WebstoreProducts.Parsing.Parsers
{
    public class EMag : Parser
    {
        private const string REGEX_TITLE = "EM.product_title = \"(.*)\"";
        private const string REGEX_IMAGE = "<meta property=\"og:image\"[ \t]+?content=\"(.+)\"";
        private const string REGEX_DOUBLE_NUMBER = @"-?\d+(?:\.\d+)?";
        private const string REGEX_FULL_PRICE = "EM.productFullPrice = " + REGEX_DOUBLE_NUMBER;
        private const string REGEX_DISCOUNT_PRICE = "EM.productDiscountedPrice = " + REGEX_DOUBLE_NUMBER;

        public override PriceDTO ParseInternal(in System.Net.WebClient web, in ProductDTO product, string page)
        {
            product.FullName = RegexItem("title/full name", REGEX_TITLE, page, 1).Trim();

            string imageURL = RegexItem("thumbnail/image URL", REGEX_IMAGE, page, 1);
            DownloadWebstoreProductThumbnail(web, product, imageURL);

            Match previousPriceMatch = Regex.Match(page, REGEX_FULL_PRICE);
            Match currentPriceMatch = Regex.Match(page, REGEX_DISCOUNT_PRICE);
            if (!previousPriceMatch.Success || !currentPriceMatch.Success)
                throw new Exception($"One of the prices (previous={previousPriceMatch.Success}, current={currentPriceMatch.Success}) for the product {product.ShortName} could not be parsed via regex");

            double previousPrice = double.Parse(previousPriceMatch.Value.Substring(previousPriceMatch.Value.IndexOf('=') + 1));
            double currentPrice = double.Parse(currentPriceMatch.Value.Substring(currentPriceMatch.Value.IndexOf('=') + 1));

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
