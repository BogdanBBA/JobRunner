using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class CityResult
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; }

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; }

        [JsonProperty("main")]
        public MainWeatherData Main { get; set; }

        [JsonProperty("visibility", NullValueHandling = NullValueHandling.Ignore)]
        public long? Visibility { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
