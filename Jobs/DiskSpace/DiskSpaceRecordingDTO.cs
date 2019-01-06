using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.DiskSpace
{
    public class DiskSpaceRecordingDTO : BaseDTO
    {
        public DateTime Moment { get; set; }
        public string PartitionName { get; set; }
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }

        public double PercentageFree { get => (double)FreeSpace / TotalSpace; }

        public static List<DiskSpaceRecordingDTO> ParseList(SQLiteDataReader reader)
        {
            List<DiskSpaceRecordingDTO> result = new List<DiskSpaceRecordingDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static DiskSpaceRecordingDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new DiskSpaceRecordingDTO()
            {
                Moment = reader.GetDateTime(0),
                PartitionName = reader.GetString(1),
                TotalSpace = reader.GetInt64(2),
                FreeSpace = reader.GetInt64(3)
            };
        }

        public override string ToSQL
            => $"{Moment.ToSqlDateTime()}, '{PartitionName}', {TotalSpace}, {FreeSpace}";
    }
}
