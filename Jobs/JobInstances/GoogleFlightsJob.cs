using CommonCode.DataLayer;
using CommonCode.Utils;
using Jobs.GoogleFlights.DTOs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Jobs.GoogleFlights
{
    public class GoogleFlightsJob : BaseJob
    {
        private const int MINIMUM_UPDATE_INTERVAL_IN_HOURS = 4;

        private GoogleFlightsDL DL;

        public GoogleFlightsJob()
            : base()
        {
            DL = new GoogleFlightsDL();
        }

        protected override int JobID
            => Const.JOB_GOOGLE_FLIGHTS;

        protected override bool ShouldRun()
        {
            return DL.ThereAreRoutesToUpdate(MINIMUM_UPDATE_INTERVAL_IN_HOURS);
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            List<RouteDTO> routesToUpdate = DL.SelectRoutesToUpdate(MINIMUM_UPDATE_INTERVAL_IN_HOURS);
            log(true, $"About to update {Utils.Plural("route", routesToUpdate.Count, true)}...");

            for (int index = 0; index < routesToUpdate.Count; index++)
            {
                RouteDTO route = routesToUpdate[index];
                log(true, $"Updating route {index + 1}/{routesToUpdate.Count} ({route.FromCity}-{route.ToCity})...");

                using (IWebDriver web = new ChromeDriver())
                {
                    log(false, "Scraping Google Flights...");
                    IOptions options = web.Manage();
                    ITimeouts timeouts = options.Timeouts();
                    timeouts.ImplicitWait = TimeSpan.FromSeconds(20);
                    timeouts.PageLoad = TimeSpan.FromSeconds(20);
                    web.Url = route.URL;
                    options.Window.Maximize();
                    IWebElement element = web.FindElement(By.XPath("//*[@id=\"flt-app\"]/div[2]/main[3]/div[9]/div[1]/div[3]/div[5]"));
                    string elementText = element.Text;
                    web.Close();

                    log(false, "Processing extracted data...");
                    List<PriceDTO> prices = ExtractPrices(elementText, route);
                    DL.InsertNewData(route, prices);

                    log(false, "...data extracted successfully.");
                    if (index < routesToUpdate.Count - 1)
                        Thread.Sleep(2500);
                }
            }


        }

        private List<PriceDTO> ExtractPrices(string elementText, RouteDTO route)
        {
            string[] lines = elementText.Substring(elementText.IndexOf(':') + 1).Trim('\r', '\n').Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<List<string>> flightData = new List<List<string>>();
            for (int iFlight = -1, iLine = 0; iLine < lines.Length; iLine++)
            {
                string line = lines[iLine];
                if (Regex.IsMatch(line, PriceDTO.TIMES_REGEX))
                {
                    iFlight++;
                    flightData.Add(new List<string>());
                }
                flightData[iFlight].Add(line);
            }

            List<PriceDTO> prices = new List<PriceDTO>();
            foreach (List<string> flight in flightData)
            {
                PriceDTO price = PriceDTO.FromGoogleFlightsStrings(flight);
                price.RouteID = route.ID;
                prices.Add(price);
            }

            route.LastUpdate = prices.Max(price => price.Moment);

            return prices;
        }
    }
}
