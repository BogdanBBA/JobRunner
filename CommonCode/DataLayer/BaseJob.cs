using CommonCode.Utils;
using System;

namespace CommonCode.DataLayer
{
    public abstract class BaseJob
    {
        public JobRunResult Run(Action<bool, string> log, object input = null)
        {
            bool shouldRun = ShouldRun();
            log(true, $"Determining whether the job should run... {shouldRun.ToYesNo()}.");
            if (!shouldRun)
                return new JobRunResult(JobID, DateTime.Now, JobRunErrorCodes.SHOULD_NOT_RUN);

            int resultCode = JobRunErrorCodes.NO_ERROR;
            string errorDescription = null;
            InternalRun(log, input, ref resultCode, ref errorDescription);
            return new JobRunResult(JobID, DateTime.Now, resultCode, errorDescription);
        }

        protected abstract int JobID { get; }

        protected abstract bool ShouldRun();

        protected abstract void InternalRun(Action<bool, string> log, object input, ref int resultCode, ref string errorDescription);
    }
}
