using Newtonsoft.Json;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }
}
