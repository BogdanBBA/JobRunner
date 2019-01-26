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

        private string Last24HoursQuery
            => "DATETIME('now', '-1 day') < Moment";

        private string ErrorLogsInLast24HWhereContents
            => $"{Last24HoursQuery} AND ResultCode != 0 AND ResultCode != 1";

        public int SelectLast24HErrorLogCount()
            => (int)(long)ExecuteSQLScalar($"SELECT COUNT(*) FROM {Tables[JobLoggingTables.Runs].Name} WHERE {ErrorLogsInLast24HWhereContents}"); 

        public int SelectLast24HLogCount()
            => (int)(long)ExecuteSQLScalar($"SELECT COUNT(*) FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24HoursQuery}");

        public List<RunDTO> SelectLast24HLogs()
            => RunDTO.ParseList(ExecuteSQLReader($"SELECT {Tables[JobLoggingTables.Runs].ColumnList} FROM {Tables[JobLoggingTables.Runs].Name} WHERE {Last24HoursQuery} ORDER BY Moment DESC"));
    }
}
