using Newtonsoft.Json;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class Coord
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
