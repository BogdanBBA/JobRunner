using DataLayer;
using Jobs.GoogleFlights.DTOs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jobs.GoogleFlights
{
    public class GoogleFlightsJob : BaseJob
    {
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
            return true;
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            string elementText = null;
            using (IWebDriver web = new ChromeDriver())
            {
                web.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
                web.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                web.Manage().Window.Maximize();
                web.Url = @"https://www.google.com/flights#flt=x/m/01q205./m/06c62.2019-02-15;b:1;c:RON;e:1;s:0;px:2;so:4;sd:1;t:f;tt:o";
                var element = web.FindElement(By.XPath("//*[@id=\"flt-app\"]/div[2]/main[3]/div[9]/div[1]/div[3]/div[5]"));
                elementText = element.Text;
                web.Close();
            }

            string[] lines = elementText.Substring(elementText.IndexOf(':') + 1).Trim('\r', '\n').Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<List<string>> flightData = new List<List<string>>();
            for (int iFlight = -1, iLine = 0; iLine < lines.Length; iLine++)
            {
                string line = lines[iLine];
                if (Regex.IsMatch(line, FlightDTO.TIMES_REGEX))
                {
                    iFlight++;
                    flightData.Add(new List<string>());
                }
                flightData[iFlight].Add(line);
            }

            List<FlightDTO> flights = new List<FlightDTO>();
            foreach (List<string> flight in flightData)
                flights.Add(FlightDTO.FromStrings(flight));

            DL.InsertNewData(null, flights);
        }
    }
}
