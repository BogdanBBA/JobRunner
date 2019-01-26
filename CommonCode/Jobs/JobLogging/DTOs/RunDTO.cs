using CommonCode.DataLayer;
using CommonCode.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.JobLogging.DTOs
{
    public class RunDTO : BaseDTO
    {
        public long JobID { get; set; }
        public DateTime Moment { get; set; }
        public long ResultCode { get; set; }
        public string ErrorDescription { get; set; }

        public static List<RunDTO> ParseList(SQLiteDataReader reader)
        {
            List<RunDTO> result = new List<RunDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static RunDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new RunDTO()
            {
                JobID = reader.GetInt64(0),
                Moment = reader.GetDateTime(1),
                ResultCode = reader.GetInt64(2),
                ErrorDescription = reader.GetString(3)
            };
        }

        public override string ToSQL
            => $"{JobID}, {Moment.ToSqlDateTime()}, {ResultCode}, '{ErrorDescription}'";
    }
}
