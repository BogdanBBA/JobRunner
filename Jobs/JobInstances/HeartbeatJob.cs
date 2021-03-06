﻿using CommonCode.DataLayer;
using CommonCode.Utils;
using Jobs.JobLogging;
using System;

namespace Jobs.Heartbeat
{
    public class HeartbeatJob : BaseJob
    {
        private HeartbeatDL hbDL;
        private JobLoggingDL jlDL;

        public HeartbeatJob()
            : base()
        {
            hbDL = new HeartbeatDL();
            jlDL = new JobLoggingDL();
        }

        protected override int JobID
            => Const.JOB_HEARTBEAT;

        protected override bool ShouldRun()
        {
            if (hbDL.DateExists(DateTime.Now))
                return false;
            return DateTime.Now.Hour >= 8;
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            log(true, "Determining error logs from the past 24 hours...");
            int totalLogs = jlDL.SelectLast24hLogCount();
            int errorLogCount = jlDL.SelectLast24hErrorLogCount();

            log(true, "Obtaining external IP address...");
            string externalIP = Utils.GetExternalIp(true);

            log(true, "Sending heartbeat e-mail...");
            NotificationEmail.Send(
                NotificationEmail.GetEmailSubject(JobID, $"{DateTime.Now:ddd, d MMM yyyy} heartbeat"),
                NotificationEmail.ComposeBody_Heartbeat(JobID, totalLogs, errorLogCount, externalIP), 
                true);
            hbDL.InsertData(HeartbeatTables.Dates, DateTime.Now.ToSqlDate());
        }
    }
}
