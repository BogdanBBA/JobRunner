using System.IO;

namespace DataLayer
{
    public class Const
    {
        public static readonly string FOLDER_ROOT;
        public static readonly string FOLDER_BIN;
        public static readonly string FOLDER_PROGRAM_FILES;
        public static readonly string FOLDER_DATABASES;
        public static readonly string FOLDER_JOBS_IMAGE_RESOURCES;
        public static readonly string FOLDER_WEBSTORE_PRODUCT_THUMBNAILS;

        public static readonly string FILE_JOBS_EXE;
        public static readonly string FILE_TEMP_HTML;
        public static readonly string FILE_TEMP_JSON;
        public static readonly string FILE_TEMP_XML;

        public const int JOB_UNKNOWN = -1;
        public const int JOB_HEARTBEAT = 1;
        public const int JOB_DISK_SPACE = 2;
        public const int JOB_ANNIVERSARIES = 3;
        public const int JOB_WEBSTORE_PRODUCTS = 4;
        public const int JOB_OPEN_WEATHER = 5;

        public const int FREQUENCY_HEARTBEAT = 2 * 3600;
        public const int FREQUENCY_DISK_SPACE = 4 * 3600;
        public const int FREQUENCY_ANNIVERSARIES = 2 * 3600;
        public const int FREQUENCY_WEBSTORE_PRODUCTS = 900;
        public const int FREQUENCY_OPEN_WEATHER = 300;

        public static readonly string DATABASE_JOB_LOGGING;
        public static readonly string DATABASE_HEARTBEAT;
        public static readonly string DATABASE_DISK_SPACE;
        public static readonly string DATABASE_ANNIVERSARIES;
        public static readonly string DATABASE_WEBSTORE_PRODUCTS;
        public static readonly string DATABASE_OPEN_WEATHER;

        static Const()
        {
            FOLDER_ROOT = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\");
            FOLDER_BIN = Directory.GetCurrentDirectory() + @"\";
            FOLDER_PROGRAM_FILES = FOLDER_ROOT + @"program-files\";
            FOLDER_DATABASES = FOLDER_PROGRAM_FILES + @"databases\";
            FOLDER_JOBS_IMAGE_RESOURCES = FOLDER_ROOT + @"Jobs\Properties\Images\";
            FOLDER_WEBSTORE_PRODUCT_THUMBNAILS = FOLDER_DATABASES + @"webstore-product-thumbnails\";

            FILE_JOBS_EXE = FOLDER_BIN + @"Jobs.exe";
            FILE_TEMP_HTML = FOLDER_PROGRAM_FILES + @"temp.html";
            FILE_TEMP_JSON = FOLDER_PROGRAM_FILES + @"temp.json";
            FILE_TEMP_XML = FOLDER_PROGRAM_FILES + @"temp.xml";

            DATABASE_JOB_LOGGING = FOLDER_DATABASES + "job-logging.sqlite";
            DATABASE_HEARTBEAT = FOLDER_DATABASES + "heartbeat.sqlite";
            DATABASE_DISK_SPACE = FOLDER_DATABASES + "disk-space.sqlite";
            DATABASE_ANNIVERSARIES = FOLDER_DATABASES + "anniversaries.sqlite";
            DATABASE_WEBSTORE_PRODUCTS = FOLDER_DATABASES + "webstore-products.sqlite";
            DATABASE_OPEN_WEATHER = FOLDER_DATABASES + "open-weather.sqlite";

            foreach (string path in new string[] { FOLDER_PROGRAM_FILES, FOLDER_DATABASES, FOLDER_WEBSTORE_PRODUCT_THUMBNAILS })
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
        }
    }
}
