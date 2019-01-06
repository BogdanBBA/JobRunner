using DataLayer;
using System;
using System.Collections.Generic;

namespace JobPoolUI.Classes
{
    public static class JobAgency
    {
        /// <summary>Used for debugging, in order to skip any other job than the currently debugged one. Contains the name of the job, or null.</summary>
        public static readonly int? CURRENTLY_DEGUBBING_JOB = null;

        public static readonly List<Tuple<int, int>> JOB_TEMPLATES = new List<Tuple<int, int>>() {
            new Tuple<int, int>(Const.JOB_HEARTBEAT, Const.FREQUENCY_HEARTBEAT),
            new Tuple<int, int>(Const.JOB_DISK_SPACE, Const.FREQUENCY_DISK_SPACE),
            new Tuple<int, int>(Const.JOB_ANNIVERSARIES, Const.FREQUENCY_ANNIVERSARIES),
            new Tuple<int, int>(Const.JOB_WEBSTORE_PRODUCTS, Const.FREQUENCY_WEBSTORE_PRODUCTS),
            new Tuple<int, int>(Const.JOB_OPEN_WEATHER, Const.FREQUENCY_OPEN_WEATHER)
        };
    }
}