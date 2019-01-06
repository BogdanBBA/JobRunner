using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jobs.OpenWeather.JsonDTOs
{
    public partial class OWMQueryResult
    {
        [JsonProperty("cnt")]
        public long Cnt { get; set; }

        [JsonProperty("list")]
        public List<CityResult> List { get; set; }
    }
}
