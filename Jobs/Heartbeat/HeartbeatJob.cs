using CommonCode;
using DataLayer;
using System;

namespace Jobs.Heartbeat
{
    public class HeartbeatJob : BaseJob
    {
        private HeartbeatDL DL;

        public HeartbeatJob()
            : base()
        {
            DL = new HeartbeatDL();
        }

        protected override int JobID
            => Const.JOB_HEARTBEAT;

        protected override bool ShouldRun()
        {
            return !DL.DateExists(DateTime.Now);
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            log(true, "Sending heartbeat e-mail...");
            NotificationEmail.Send(
                NotificationEmail.GetEmailSubject(JobID, $"{DateTime.Now:ddd, d MMM yyyy} heartbeat"),
                $"Just to let you know - the jobs are alive and running.{Environment.NewLine}E-mail sent {DateTime.Now:dddd, d MMMM yyyy, HH:mm:ss}. Cheers.");
            DL.InsertData(HeartbeatTables.Dates, DateTime.Now.ToSqlDate());
        }
    }
}
