namespace Jobs.OpenWeather
{
    public static class OWM
    {
        /// <summary>
        /// Should OWM's restrictions change and MAX_REQUESTS_PER_MINUTE/MAX_CITIES_PER_REQUEST results in a floating-point number, 
        /// the number of requests per run should be kept at Math.Floor(REQUESTS_PER_RUN) to avoid making too many API requests.
        /// </summary>
        public const int REQUESTS_PER_RUN = 3;
        private const int MAX_REQUESTS_PER_MINUTE = 60;
        public const int MAX_CITIES_PER_REQUEST = 20;

        /// <summary>
        /// The OWM request URL pattern, to be used in string.Format() with a comma-concatenated list of city IDs.
        /// </summary>
        public const string WEATHER_REQUEST_URL = @"http://api.openweathermap.org/data/2.5/group?id={0}&APPID=" + API_KEY;
        private const string API_KEY = "57b084731adaa4e16c09a1cf27446cec";
    }
}
