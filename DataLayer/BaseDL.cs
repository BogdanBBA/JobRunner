using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace DataLayer
{
    public class Table
    {
        public string Name { get; private set; }
        public string ColumnDeclarations { get; private set; }
        public string ColumnList { get; private set; }

        public Table(string name, string columnDeclarations, string columnList)
        {
            Name = name;
            ColumnDeclarations = columnDeclarations;
            ColumnList = columnList;
        }
    }

    public abstract class BaseDL<TYPE>
        where TYPE : Enum
    {
        protected Dictionary<TYPE, Table> Tables { get; private set; }
        protected abstract SQLiteConnection Connection { get; }

        public BaseDL()
        {
            Tables = new Dictionary<TYPE, Table>();
            InitializeTableOverview();
            RecreateAllTables(false);
        }

        protected abstract void InitializeTableOverview();

        protected SQLiteConnection GetConnection(ref SQLiteConnection connection, string filePath)
        {
            if (connection == null || (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed))
            {
                connection = new SQLiteConnection($"Data Source={filePath};Version=3;");
                connection.Open();
            }
            return connection;
        }

        protected void AddTable(TYPE enumValue, string columnDeclarations, string columnList)
            => Tables.Add(enumValue, new Table(enumValue.ToString(), columnDeclarations, columnList));

        public SQLiteCommand GetSQLiteCommand(string sql, SQLiteConnection connection)
           => new SQLiteCommand(sql, connection);

        public SQLiteCommand GetSQLiteCommand(string sql)
           => GetSQLiteCommand(sql, Connection);

        public void ExecuteSQL(string sql)
            => GetSQLiteCommand(sql).ExecuteNonQuery();

        public object ExecuteSQLScalar(string sql)
            => GetSQLiteCommand(sql).ExecuteScalar();

        public SQLiteDataReader ExecuteSQLReader(string sql)
            => GetSQLiteCommand(sql).ExecuteReader();

        public bool TableExists(string name)
        {
            string sql = $"SELECT EXISTS(SELECT * FROM sqlite_master WHERE type = 'table' AND name = '{name}')";
            return ((long)ExecuteSQLScalar(sql)) == 1;
        }

        public void RecreateTable(bool force, string name, string definition)
        {
            if (force || !TableExists(name))
            {
                ExecuteSQL($"DROP TABLE IF EXISTS {name};");
                ExecuteSQL($"CREATE TABLE {name}({definition});");
            }
        }

        public void RecreateAllTables(bool force)
        {
            foreach (KeyValuePair<TYPE, Table> entry in Tables)
                RecreateTable(force, entry.Value.Name, entry.Value.ColumnDeclarations);
        }

        public SQLiteDataReader Select(Table table, string where = null, string orderBy = null, int limit = 0)
        {
            string whereSql = where == null ? "" : $" WHERE {where}";
            string orderBySql = orderBy == null ? "" : $" ORDER BY {orderBy}";
            string limitSql = limit <= 0 ? "" : $" LIMIT {limit}";
            return ExecuteSQLReader($"SELECT {table.ColumnList} FROM {table.Name}{whereSql}{orderBySql}{limitSql};");
        }

        public SQLiteDataReader Select(TYPE enumValue, string where = null, string orderBy = null, int limit = 0)
            => Select(Tables[enumValue], where, orderBy, limit);

        public void InsertData(Table table, params string[] values)
        {
            StringBuilder sb = new StringBuilder($"INSERT INTO {table.Name}({table.ColumnList}) VALUES ");
            foreach (string value in values)
                sb.Append($"({value}), ");
            ExecuteSQL(sb.Remove(sb.Length - 2, 2).Append(';').ToString());
        }

        public void InsertData(TYPE enumValue, params string[] values)
            => InsertData(Tables[enumValue], values);
    }
}
