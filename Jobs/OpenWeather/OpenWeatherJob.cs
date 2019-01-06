using CommonCode;
using DataLayer;
using Jobs.OpenWeather.DTOs;
using Jobs.OpenWeather.JsonDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Jobs.OpenWeather
{
    public class OpenWeatherJob : BaseJob
    {
        private const int MINIMUM_TIME_BETWEEN_UPDATES_IN_HOURS = 1;

        private OpenWeatherDL DL;

        public OpenWeatherJob()
            : base()
        {
            DL = new OpenWeatherDL();
        }

        protected override int JobID
            => Const.JOB_OPEN_WEATHER;

        protected override bool ShouldRun()
        {
            return DL.ThereAreCitiesToUpdateNow(MINIMUM_TIME_BETWEEN_UPDATES_IN_HOURS, OWM.MAX_CITIES_PER_REQUEST);
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            using (WebClient web = new WebClient())
            {
                for (int iteration = 1; iteration <= OWM.REQUESTS_PER_RUN; iteration++)
                {
                    log(true, $"Iteration {iteration} / max {OWM.REQUESTS_PER_RUN}");
                    List<CityDTO> cities = DL.SelectOldest20Cities(MINIMUM_TIME_BETWEEN_UPDATES_IN_HOURS, OWM.MAX_CITIES_PER_REQUEST);
                    if (cities.Count == 0)
                    {
                        log(false, "No more cities to update now.");
                        break;
                    }
                    log(false, $"Looking up {Utils.Plural("city", "cities", cities.Count, true)} ({string.Join(", ", cities.Select(city => city.Name))})...");
                    string url = string.Format(OWM.WEATHER_REQUEST_URL, string.Join(",", cities.Select(city => city.ID.ToString())));
                    web.DownloadFile(url, Const.FILE_TEMP_JSON);
                    OWMQueryResult result = JsonConvert.DeserializeObject<OWMQueryResult>(File.ReadAllText(Const.FILE_TEMP_JSON));
                    foreach (CityResult cityResult in result.List)
                    {
                        WeatherStateDTO weather = WeatherStateDTO.FromCityResult(cityResult);
                        DL.AddWeatherStateAndUpdateCity(weather);
                    }
                    if (iteration < OWM.REQUESTS_PER_RUN)
                        Thread.Sleep(2100);
                }
            }
        }
    }
}
