using CommonCode.DataLayer;
using Jobs.JobLogging.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.JobLogging
{
    public enum JobLoggingTables
    {
        Runs
    }

    public class JobLoggingDL : BaseDL<JobLoggingTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_JOB_LOGGING);

        protected override void InitializeTableOverview()
        {
            AddTable(JobLoggingTables.Runs, "JobID INTEGER, Moment DATETIME, ResultCode INTEGER, ErrorDescription TEXT", "JobID, Moment, ResultCode, ErrorDescription");
        }

        private string Last24hLogs_WhereContents
            => "DATETIME('now', '-1 day') < Moment";

        private string Last24hErrorLogs_WhereContents
            => $"{Last24hLogs_WhereContents} AND ResultCode != 0 AND ResultCode != 1";

        public int SelectLast24hLogCount()
            => (int)(long)ExecuteSQLScalar($"SELECT COUNT(*) FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24hLogs_WhereContents}");

        public int SelectLast24hErrorLogCount()
            => (int)(long)ExecuteSQLScalar($"SELECT COUNT(*) FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24hErrorLogs_WhereContents}");

        public List<RunDTO> SelectLast24hLogs()
            => RunDTO.ParseList(ExecuteSQLReader($"SELECT {Tables[JobLoggingTables.Runs].ColumnList} FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24hLogs_WhereContents} ORDER BY Moment DESC"));

        public List<RunDTO> SelectLast24hErrorLogs()
            => RunDTO.ParseList(ExecuteSQLReader($"SELECT {Tables[JobLoggingTables.Runs].ColumnList} FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24hErrorLogs_WhereContents} ORDER BY Moment DESC"));
    }
}
