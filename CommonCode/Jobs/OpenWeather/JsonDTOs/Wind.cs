using Newtonsoft.Json;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public double Deg { get; set; }
    }
}
