using Newtonsoft.Json;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class MainWeatherData
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("sea_level", NullValueHandling = NullValueHandling.Ignore)]
        public double? SeaLevel { get; set; }

        [JsonProperty("grnd_level", NullValueHandling = NullValueHandling.Ignore)]
        public double? GrndLevel { get; set; }
    }
}
