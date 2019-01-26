using Jobs.OpenWeather.JsonDTOs;
using Newtonsoft.Json;
using System.IO;

namespace CommonCode.Jobs.OpenWeather.JsonDTOs
{
    public static class DeserializeQueryResult
    {
        public static OWMQueryResult DeserializeJson(string filePath)
        {
            return JsonConvert.DeserializeObject<OWMQueryResult>(File.ReadAllText(filePath));
        }
    }
}
