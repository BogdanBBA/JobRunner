using Jobs.WebstoreProducts.DTOs;
using System;
using System.Net;

namespace Jobs.WebstoreProducts.Parsing.Parsers
{
    public class MicrosoftStore : Parser
    {
        private const string REGEX_TITLE = "<h1 id=\"DynamicHeading_productTitle\".*?>(.+?)<\\/h1>";
        private const string REGEX_IMAGE = "<picture id=\"dynamicImage_image_picture\".*?>.*?<img src=\"(.+?)\".*?>.*?<\\/picture>";
        private const string REGEX_CURRENT_PRICE = "urrent price \\$(\\d+(?>\\.\\d+)?)";
        private const string REGEX_OLD_PRICE = "riginal price was \\$(\\d+(?>\\.\\d+)?)";

        public override int ThreadSleepDuration 
            => 5000;

        public override PriceDTO ParseInternal(WebClient web, ProductDTO product, string page)
        {
            product.FullName = RegexItem("title/full name", REGEX_TITLE, page, 1).Trim();

            string imageURL = RegexItem("thumbnail/image URL", REGEX_IMAGE, page, 1);
            DownloadWebstoreProductThumbnail(web, product, imageURL.Replace("&amp;", "&"));

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
                Currency = "$",
                PreviousPrice = oldPrice,
                CurrentPrice = currentPrice
            };
        }
    }
}
