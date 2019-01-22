using CommonCode.DataLayer;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.DiskSpace
{
    public enum DiskSpaceTables
    {
        DiskSpaceRecording
    }

    public class DiskSpaceDL : BaseDL<DiskSpaceTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_DISK_SPACE);

        protected override void InitializeTableOverview()
        {
            AddTable(DiskSpaceTables.DiskSpaceRecording, "Moment DATETIME, PartitionName TEXT, TotalSpace INTEGER, FreeSpace INTEGER", "Moment, PartitionName, TotalSpace, FreeSpace");
        }

        public DiskSpaceRecordingDTO SelectMostRecentDiskSpaceRecording()
        {
            var list = DiskSpaceRecordingDTO.ParseList(Select(Tables[DiskSpaceTables.DiskSpaceRecording], null, "Moment DESC", 1));
            return list == null || list.Count == 0 ? null : list[0];
        }

        public List<DiskSpaceRecordingDTO> SelectDiskSpaceRecordings(string where = null, string orderBy = null, int limit = 0)
        {
            return DiskSpaceRecordingDTO.ParseList(Select(Tables[DiskSpaceTables.DiskSpaceRecording], where, orderBy, limit));
        }
    }
}
