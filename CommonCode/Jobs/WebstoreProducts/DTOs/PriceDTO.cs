using CommonCode.DataLayer;
using CommonCode.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.WebstoreProducts.DTOs
{
    public class PriceDTO : BaseDTO
    {
        public DateTime Moment { get; set; }
        public long ProductID { get; set; }
        public string Currency { get; set; }
        public double CurrentPrice { get; set; }
        public double PreviousPrice { get; set; }

        public ProductDTO Product { get; set; }

        public double PriceReduction
            => CurrentPrice - PreviousPrice;

        public double PriceReductionPercentage
            => (CurrentPrice - PreviousPrice) / PreviousPrice;

        public static List<PriceDTO> ParseList(SQLiteDataReader reader)
        {
            List<PriceDTO> result = new List<PriceDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static PriceDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new PriceDTO()
            {
                Moment = reader.GetDateTime(0),
                ProductID = reader.GetInt64(1),
                Currency = reader.GetString(2),
                CurrentPrice = reader.GetDouble(3),
                PreviousPrice = reader.GetDouble(4)
            };
        }

        public override string ToSQL
            => $"{Moment.ToSqlDateTime()}, {ProductID}, '{Currency}', {CurrentPrice}, {PreviousPrice}";
    }
}
