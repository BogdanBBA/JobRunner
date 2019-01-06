using System;

namespace JobPoolUI.Classes
{
    public class JobDescription
    {
        public enum JobStatus { Waiting, Executing };

        public int JobID { get; private set; }
        public string JobName { get; private set; }
        public JobStatus Status { get; internal set; }
        public TimeSpan ExecutionInterval { get; private set; }
        public DateTime NextTargetExecutionMoment { get; internal set; }
        public int RunCount { get; internal set; }
        public int ErrorCount { get; internal set; }

        public JobDescription(int jobID, string jobName, int executionIntervalInSeconds)
        {
            JobID = jobID;
            JobName = jobName;
            Status = JobStatus.Waiting;
            ExecutionInterval = new TimeSpan(0, 0, executionIntervalInSeconds);
            NextTargetExecutionMoment = DateTime.Now.AddMilliseconds(FMain.INITIAL_TIMER_DURATION);
            RunCount = 0;
            ErrorCount = 0;
        }

        public bool ItsAboutTimeToRun()
        {
            if (JobAgency.CURRENTLY_DEGUBBING_JOB.HasValue && JobAgency.CURRENTLY_DEGUBBING_JOB.Value != JobID)
                return false;
            return DateTime.Now.CompareTo(NextTargetExecutionMoment) >= 0;
        }
    }
}