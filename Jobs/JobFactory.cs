using DataLayer;
using System;

namespace Jobs
{
    public static class JobFactory
    {
        public static BaseJob GetJob(int jobID)
        {
            switch (jobID)
            {
                case Const.JOB_HEARTBEAT:
                    return new Heartbeat.HeartbeatJob();
                case Const.JOB_DISK_SPACE:
                    return new DiskSpace.DiskSpaceJob();
                case Const.JOB_ANNIVERSARIES:
                    return new Anniversaries.AnniversariesJob();
                case Const.JOB_WEBSTORE_PRODUCTS:
                    return new WebstoreProducts.WebstoreProductsJob();
                case Const.JOB_OPEN_WEATHER:
                    return new OpenWeather.OpenWeatherJob();
                case Const.JOB_GOOGLE_FLIGHTS:
                    return new GoogleFlights.GoogleFlightsJob();
                default:
                    throw new ApplicationException($"JobFactory.Get*() ERROR: invalid jobID={jobID}");
            }
        }

        public static string GetName(int jobID)
        {
            switch (jobID)
            {
                case Const.JOB_HEARTBEAT:
                    return "Heartbeat";
                case Const.JOB_DISK_SPACE:
                    return "Disk space";
                case Const.JOB_ANNIVERSARIES:
                    return "Anniversaries";
                case Const.JOB_WEBSTORE_PRODUCTS:
                    return "Webstore products";
                case Const.JOB_OPEN_WEATHER:
                    return "Open Weather";
                case Const.JOB_GOOGLE_FLIGHTS:
                    return "Google Flights";
                default:
                    throw new ApplicationException($"JobFactory.Get*() ERROR: invalid jobID={jobID}");
            }
        }
    }
}
