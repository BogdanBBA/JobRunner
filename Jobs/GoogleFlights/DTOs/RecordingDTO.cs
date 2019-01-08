using CommonCode;
using DataLayer;
using System;

namespace Jobs.GoogleFlights.DTOs
{
    public class RecordingDTO : BaseDTO
    {
        public long ID { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime FlightDate { get; set; }
        public DateTime Moment { get; set; }
        public string URL { get; set; }

        public override string ToSQL
            => $"{ID}, '{FromCity}', '{ToCity}', {FlightDate.ToSqlDate()}, {Moment.ToSqlDateTime()}, '{URL}'";
    }
}
