using Jobs.JobLogging;

namespace WebAPI
{
    public static class DLs
    {
        private static JobLoggingDL jobLogging;
        public static JobLoggingDL JobLogging { get { if (jobLogging == null) jobLogging = new JobLoggingDL(); return jobLogging; } }
    }
}
