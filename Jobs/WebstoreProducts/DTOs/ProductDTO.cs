using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.WebstoreProducts.DTOs
{
    public class ProductDTO : BaseDTO
    {
        public long ID { get; set; }
        public bool Active { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string URL { get; set; }
        public long Frequency { get; set; }
        public DateTime LastRun { get; set; }

        public static List<ProductDTO> ParseList(SQLiteDataReader reader)
        {
            List<ProductDTO> result = new List<ProductDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static ProductDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new ProductDTO()
            {
                ID = reader.GetInt64(0),
                Active = reader.GetBoolean(1),
                ShortName = reader.GetString(2),
                FullName = reader.GetString(3),
                URL = reader.GetString(4),
                Frequency = reader.GetInt64(5),
                LastRun = reader.GetDateTime(6)
            };
        }

        public override string ToSQL
            => $"{ID}, {Active.To01()}, '{ShortName}', '{FullName}', '{URL}', {Frequency}, DATETIME('{LastRun}')";
    }
}
