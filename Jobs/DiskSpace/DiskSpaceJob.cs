using CommonCode.DataLayer;
using CommonCode.Utils;
using System;
using System.IO;

namespace Jobs.DiskSpace
{
    public class DiskSpaceJob : BaseJob
    {
        private DiskSpaceDL DL;

        public DiskSpaceJob()
            : base()
        {
            DL = new DiskSpaceDL();
        }

        protected override int JobID
            => Const.JOB_DISK_SPACE;

        protected override bool ShouldRun()
        {
            DiskSpaceRecordingDTO dto = DL.SelectMostRecentDiskSpaceRecording();
            return dto == null || dto.Moment.AddSeconds(Const.FREQUENCY_DISK_SPACE).CompareTo(DateTime.Now) <= 0;
        }

        protected override void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription)
        {
            log(false, "Retrieving drive info (only looking at the first one, that should be C:\\)...");
            DriveInfo driveC = DriveInfo.GetDrives()[0]; // for now, that's all we're interested in
            DiskSpaceRecordingDTO dto = new DiskSpaceRecordingDTO()
            {
                Moment = DateTime.Now,
                PartitionName = driveC.Name,
                TotalSpace = driveC.TotalSize,
                FreeSpace = driveC.AvailableFreeSpace
            };

            log(false, $"Logging space for drive \"{dto.PartitionName}\" - {dto.FreeSpace.FormatBytes()} / {dto.TotalSpace.FormatBytes()}");
            DL.InsertData(DiskSpaceTables.DiskSpaceRecording, dto.ToSQL);

            bool shouldAlert = dto.PercentageFree <= 0.2; //dto.FreeSpace < Math.Pow(1024, 3); 
            log(false, $"Determining whether an alert email should be sent... {shouldAlert.ToYesNo()}.");
            if (shouldAlert)
            {
                log(false, "Sending alert email...");
                NotificationEmail.Send(
                    NotificationEmail.GetEmailSubject(JobID, "alert"),
                    NotificationEmail.ComposeBody($"This is a warning that the partition \"<b>{dto.PartitionName}</b>\" on the machine where the job runner currently runs is low on disk space." +
                        $"<br><br>From a total of <b>{dto.TotalSpace.FormatBytes()}</b>, there is only <b>{dto.FreeSpace.FormatBytes()}</b> (or {dto.PercentageFree:P1}) of free space remaining." +
                        $"<br><br>You should maybe take a look at that."),
                    true
                );
            }

            log(false, "Done.");
        }
    }
}
