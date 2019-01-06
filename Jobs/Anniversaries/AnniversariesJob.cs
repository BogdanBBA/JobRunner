using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jobs.Anniversaries
{
    public class AnniversariesJob : BaseJob
    {
        private AnniversariesDL DL;

        public AnniversariesJob()
            : base()
        {
            DL = new AnniversariesDL();
        }

        protected override int JobID
            => Const.JOB_ANNIVERSARIES;

        protected override bool ShouldRun()
        {
            if (!DL.AnyAnniversariesExist())
                DL.RecreateAnniversaries(); // others may have been added on top of these hard-coded ones in SQL
            return !DL.DateExists(DateTime.Now);
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            // retrieve all anniversaries - they shouldn't be too many, then filter those who should be considered for today
            log(false, "Determining whether there are any anniversaries to celebrate...");
            DateTime today = DateTime.Now.Date;
            List<AnniversaryDTO> anniversaries = DL.SelectAnniversaries();
            List<AnniversaryDTO> shouldNotifyToday = anniversaries.Where(anniversary => anniversary.ShouldSendReminder(today)).ToList();

            if (shouldNotifyToday?.Count == 0)
                log(false, "There are not.");
            else
            {
                log(false, $"Yes, there {Utils.Conjugation("is", "are", shouldNotifyToday.Count)} {shouldNotifyToday.Count}. Sending notification email...");
                List<KeyValuePair<string, string>> attachments = shouldNotifyToday.Select(anniversary => new KeyValuePair<string, string>(
                    anniversary.Category.ToString().GetHashCode().ToString(),
                    Path.GetFullPath($"{Const.FOLDER_JOBS_IMAGE_RESOURCES}{anniversary.Category}.png"))).ToList();
                NotificationEmail.Send(
                    NotificationEmail.GetEmailSubject(JobID, "reminder"),
                    NotificationEmail.ComposeBody_Anniversaries(JobID, shouldNotifyToday),
                    true,
                    attachments);
            }

            DL.InsertData(AnniversariesTables.Dates, DateTime.Now.ToSqlDate());
        }
    }
}
