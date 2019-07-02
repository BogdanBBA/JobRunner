using CommonCode.Utils;
using System;
using System.IO;

namespace CommonCode.DataLayer
{
	public class Const
	{
		public static bool IsInitialized { get; private set; } = false;

		public static string FOLDER_SOLUTION_ROOT;
		public static string FOLDER_JOBS_BIN;
		public static string FOLDER_PROGRAM_FILES;
		public static string FOLDER_DATABASES;
		public static string FOLDER_JOBS_IMAGE_RESOURCES;
		public static string FOLDER_WEBSTORE_PRODUCT_THUMBNAILS;

		public static string FILE_JOBS_EXE;
		public static string FILE_TEMP_HTML;
		public static string FILE_TEMP_JSON;
		public static string FILE_TEMP_XML;

		public const int JOB_UNKNOWN = -1;
		public const int JOB_HEARTBEAT = 1;
		public const int JOB_DISK_SPACE = 2;
		public const int JOB_ANNIVERSARIES = 3;
		public const int JOB_WEBSTORE_PRODUCTS = 4;
		public const int JOB_OPEN_WEATHER = 5;
		public const int JOB_GOOGLE_FLIGHTS = 6;

		public const int FREQUENCY_HEARTBEAT = 3600;
		public const int FREQUENCY_DISK_SPACE = 12 * 3600;
		public const int FREQUENCY_ANNIVERSARIES = 2 * 3600;
		public const int FREQUENCY_WEBSTORE_PRODUCTS = 900;
		public const int FREQUENCY_OPEN_WEATHER = 300;
		public const int FREQUENCY_GOOGLE_FLIGHTS = 4 * 3600;

		public static string DATABASE_JOB_LOGGING;
		public static string DATABASE_HEARTBEAT;
		public static string DATABASE_DISK_SPACE;
		public static string DATABASE_ANNIVERSARIES;
		public static string DATABASE_WEBSTORE_PRODUCTS;
		public static string DATABASE_OPEN_WEATHER;
		public static string DATABASE_GOOGLE_FLIGHTS;

		public static void Initialize(string binFolder_SectionAfterRoot)
		{
			//if (IsInitialized)
			//	throw new InvalidOperationException("Should probably not initialize Const twice.");

			FOLDER_SOLUTION_ROOT = @"C:\BBA\JobRunner\";
			FOLDER_JOBS_BIN = Path.Combine(FOLDER_SOLUTION_ROOT, binFolder_SectionAfterRoot).EnsureEndsWith(@"\");

			if (!FOLDER_JOBS_BIN.Equals(Environment.CurrentDirectory.EnsureEndsWith(@"\")))
				File.WriteAllText("this-log.txt", $"ERROR: The application is not running from where it's supposed to (Expected FOLDER_JOBS_BIN='{FOLDER_JOBS_BIN}', actual Environment.CurrentDirectory='{Environment.CurrentDirectory}').");
				//throw new ApplicationException($"ERROR: The application is not running from where it's supposed to (Expected FOLDER_JOBS_BIN='{FOLDER_JOBS_BIN}', actual Environment.CurrentDirectory='{Environment.CurrentDirectory}').");

			FOLDER_PROGRAM_FILES = FOLDER_SOLUTION_ROOT + @"program-files\";
			FOLDER_DATABASES = FOLDER_PROGRAM_FILES + @"databases\";
			FOLDER_JOBS_IMAGE_RESOURCES = FOLDER_SOLUTION_ROOT + @"Jobs\Properties\Images\";
			FOLDER_WEBSTORE_PRODUCT_THUMBNAILS = FOLDER_DATABASES + @"webstore-product-thumbnails\";

			FILE_JOBS_EXE = FOLDER_JOBS_BIN + @"Jobs.exe";
			FILE_TEMP_HTML = FOLDER_PROGRAM_FILES + @"temp.html";
			FILE_TEMP_JSON = FOLDER_PROGRAM_FILES + @"temp.json";
			FILE_TEMP_XML = FOLDER_PROGRAM_FILES + @"temp.xml";

			DATABASE_JOB_LOGGING = FOLDER_DATABASES + "job-logging.sqlite";
			DATABASE_HEARTBEAT = FOLDER_DATABASES + "heartbeat.sqlite";
			DATABASE_DISK_SPACE = FOLDER_DATABASES + "disk-space.sqlite";
			DATABASE_ANNIVERSARIES = FOLDER_DATABASES + "anniversaries.sqlite";
			DATABASE_WEBSTORE_PRODUCTS = FOLDER_DATABASES + "webstore-products.sqlite";
			DATABASE_OPEN_WEATHER = FOLDER_DATABASES + "open-weather.sqlite";
			DATABASE_GOOGLE_FLIGHTS = FOLDER_DATABASES + "google-flights.sqlite";

			foreach (string path in new string[] { FOLDER_PROGRAM_FILES, FOLDER_DATABASES, FOLDER_WEBSTORE_PRODUCT_THUMBNAILS })
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

			IsInitialized = true;
		}
	}
}
