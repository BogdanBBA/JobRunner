using CommonCode.DataLayer;
using Jobs.JobLogging;

namespace WebAPI
{
	public static class DLs
	{
		public const bool LOCAL_DEBUGGING_MODE = false;

		private static JobLoggingDL jobLogging;
		public static JobLoggingDL JobLogging { get { if (jobLogging == null) jobLogging = new JobLoggingDL(); return jobLogging; } }

		public static void InitializeConstsAsWebAPI()
		{
			if (!Const.IsInitialized)
				Const.Initialize(LOCAL_DEBUGGING_MODE ? @"WebAPI\" : @"WebAPI\bin\Release\netcoreapp2.2\publish\");
		}
	}
}
