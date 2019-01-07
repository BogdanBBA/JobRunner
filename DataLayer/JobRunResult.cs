using CommonCode;
using System;

namespace DataLayer
{
    public class JobRunResult : BaseDTO
    {
        public int JobID { get; private set; }
        public DateTime Moment { get; private set; }
        public int ResultCode { get; private set; }
        public string ErrorDescription { get; private set; }

        public JobRunResult(int jobID, DateTime moment, int resultCode, string errorDescription = null)
        {
            JobID = jobID;
            Moment = moment;
            ResultCode = resultCode;
            ErrorDescription = errorDescription;
        }

        public string ResultCodeDescription
            => JobRunErrorCodes.GetErrorCodeDescription(ResultCode);

        public override string ToSQL
            => $"{JobID}, {Moment.ToSqlDateTime()}, {ResultCode}, {(ErrorDescription == null ? "NULL" : $"'{ErrorDescription.Replace("'", "''")}'")}";
    }
}
