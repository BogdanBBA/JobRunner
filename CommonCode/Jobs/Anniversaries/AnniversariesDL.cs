using CommonCode.DataLayer;
using CommonCode.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.Anniversaries
{
    public enum AnniversariesTables
    {
        Dates,
        Anniversaries
    }

    public class AnniversariesDL : BaseDL<AnniversariesTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_ANNIVERSARIES);

        protected override void InitializeTableOverview()
        {
            AddTable(AnniversariesTables.Dates, "Moment DATE", "Moment");
            AddTable(AnniversariesTables.Anniversaries, "Title TEXT, Category INTEGER, OriginalEvent DATE, Frequency INTEGER, HeadsUpInDays INTEGER", "Title, Category, OriginalEvent, Frequency, HeadsUpInDays");
        }

        public bool DateExists(DateTime moment)
        {
            return (long)ExecuteSQLScalar($"SELECT EXISTS(SELECT Moment FROM Dates WHERE Moment = {DateTime.Now.ToSqlDate()})") == 1;
        }

        public bool AnyAnniversariesExist()
        {
            return (long)ExecuteSQLScalar("SELECT EXISTS(SELECT * FROM Anniversaries LIMIT 1)") == 1;
        }

        public void RecreateAnniversaries() // others may be added on top of these hard-coded ones in SQL
        {
            // The name of the celebrated person / holiday / event / etc. (text string)
            // The type of anniversary, which determines the thematic styling of the reminder (integer number: MetSomeone=0, Birthday=1, FirstTimeDidSomething=2, Romantic=3)
            // The date of the original event (text string in a 'yyyy-MM-dd' format)
            // Reminder frequency / at what type of time interval is an anniversary celebrated (integer number: OnlyOnce=0, Daily=1, Weekly=2, Monthly=3, Yearly=4)
            // The number of days before the event when the reminder should be sent (positive integer number; 0 means no heads-up). A single reminder on the anniversary day is sent indifferent of the heads up
            InsertData(AnniversariesTables.Anniversaries, "'Bogdan Blăniță', 1, DATE('1992-05-20'), 4, 0");
            InsertData(AnniversariesTables.Anniversaries, "'Laura Blăniță', 1, DATE('1968-05-31'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Gabriel Blăniță', 1, DATE('1967-06-23'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Adrian Șulea', 1, DATE('1974-05-14'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Elena Giurgi', 1, DATE('1900-01-01'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Lavinia Gomoi', 1, DATE('1991-07-05'), 4, 7");
            InsertData(AnniversariesTables.Anniversaries, "'Georgeta Gomoi', 1, DATE('1949-01-31'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Ioan Gomoi', 1, DATE('1956-10-21'), 4, 3");
            InsertData(AnniversariesTables.Anniversaries, "'Sorin Silaghiu', 1, DATE('1983-08-31'), 4, 1");
            InsertData(AnniversariesTables.Anniversaries, "'Lav+BBA anniversary', 3, DATE('2016-04-16'), 3, 0");
            InsertData(AnniversariesTables.Anniversaries, "'Lav+BBA first kiss', 3, DATE('2016-04-20'), 3, 0");
            InsertData(AnniversariesTables.Anniversaries, "'Friday! #endofwork #endjoyit', 2, DATE('2015-08-07'), 2, 0");
        }

        public List<AnniversaryDTO> SelectAnniversaries(string where = null, string orderBy = null, int limit = 0)
        {
            return AnniversaryDTO.ParseList(Select(AnniversariesTables.Anniversaries, where, orderBy, limit));
        }
    }
}
