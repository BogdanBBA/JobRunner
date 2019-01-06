using CommonCode;
using DataLayer;
using Jobs.WebstoreProducts.DTOs;
using Jobs.WebstoreProducts.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Jobs.WebstoreProducts
{
    public class WebstoreProductsJob : BaseJob
    {
        private WebstoreProductsDL DL;

        public WebstoreProductsJob()
            : base()
        {
            DL = new WebstoreProductsDL();
        }

        protected override int JobID
            => Const.JOB_WEBSTORE_PRODUCTS;

        protected override bool ShouldRun()
        {
            return DL.ThereAreJobsToRunNow;
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            List<ProductDTO> products = DL.SelectJobsToRunNow();
            Dictionary<ProductDTO, string> failedUpdates = new Dictionary<ProductDTO, string>();
            Parser lastParser = null;
            log(true, $"Parsing {products.Count} product(s)...");
            using (WebClient web = new WebClient())
            {
                for (int index = 0; index < products.Count; index++)
                {
                    ProductDTO product = products[index];
                    log(false, $"Processing product {index + 1}/{products.Count} ({product.ShortName})...");
                    if (index > 0)
                        Thread.Sleep(lastParser.ThreadSleepDuration);
                    Parser parser = ParserFactory.GetParser(product.URL);
                    parser.ParseProductPrice(in web, in product, out PriceDTO price, out string error);
                    if (error != null)
                        failedUpdates.Add(product, error);
                    else
                        DL.UpdateProductAndInsertPrice(product, price);
                    lastParser = parser;
                    log(false, error != null ? $"... failed :(" : $"done (y)");
                }
            }

            List<Last2ProductPricesDTO> productsToNotifyAbout = products
                .Where(product => !failedUpdates.ContainsKey(product))
                .Select(product => DL.SelectLast2PricesForProduct(product))
                .Where(last2Prices => last2Prices.SomethingChanged)
                .ToList();

            List<KeyValuePair<string, string>> attachments = productsToNotifyAbout.Select(product => new KeyValuePair<string, string>(
                    product.Product.FullName.GetHashCode().ToString(),
                    Path.GetFullPath($"{Const.FOLDER_WEBSTORE_PRODUCT_THUMBNAILS}{product.Product.ID}.jpg"))).ToList();

            bool shouldNotify = productsToNotifyAbout.Count > 0 || failedUpdates.Count > 0;
            log(true, $"Determining whether a notification email should be sent... {shouldNotify.ToYesNo()}.");
            if (shouldNotify)
            {
                log(false, "Sending notification email...");
                NotificationEmail.Send(
                        NotificationEmail.GetEmailSubject(JobID, "prices changed"),
                        NotificationEmail.ComposeBody_WebstoreProducts(JobID, productsToNotifyAbout, failedUpdates),
                        true,
                        attachments);
            }

            log(true, "Done.");
        }
    }
}