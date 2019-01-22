using CommonCode.DataLayer;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;

namespace Jobs
{
    class Program
    {
        const int DISPLAY_TIME_SUCCESS = 3000;
        const int DISPLAY_TIME_INCORRECT_CALL = 5000;
        const int DISPLAY_TIME_ERROR = 15000;

        enum LogLevel { Big, Medium, Small };
        static Dictionary<LogLevel, string> _prefixes = new Dictionary<LogLevel, string>()
            { { LogLevel.Big, $"{Environment.NewLine} ### " }, { LogLevel.Medium, " * " }, { LogLevel.Small, " - " } };
        static Dictionary<LogLevel, string> _suffixes = new Dictionary<LogLevel, string>()
            { { LogLevel.Big, $"{Environment.NewLine}" }, { LogLevel.Medium, "" }, { LogLevel.Small, "" } };

        static void Log(LogLevel level, string log, ConsoleColor consoleColor = ConsoleColor.White)
        {
            if (consoleColor != ConsoleColor.White)
                Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{_prefixes[level]}{log}{_suffixes[level]}");
            if (consoleColor != ConsoleColor.White)
                Console.ForegroundColor = ConsoleColor.White;
        }

        static void Log(bool medium, string log)
            => Log(medium ? LogLevel.Medium : LogLevel.Small, log);

        static JobLoggingDL DL = new JobLoggingDL();

        static void Finalize(JobRunResult result)
        {
            bool problem = JobRunErrorCodes.ThereWasAProblem(result.ResultCode);
            Log(LogLevel.Big,
                $"Finished {(problem ? $": {JobRunErrorCodes.GetErrorCodeDescription(result.ResultCode)} / {result.ErrorDescription}" : (result.ResultCode == JobRunErrorCodes.NO_ERROR ? "normally! Great job ;)" : "(without actually running, but it's ok)"))}",
                problem ? ConsoleColor.Red : ConsoleColor.Yellow);
            DL.InsertData(JobLoggingTables.Runs, result.ToSQL);
            Thread.Sleep(problem ? DISPLAY_TIME_ERROR : DISPLAY_TIME_SUCCESS);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Log(LogLevel.Big, $"Incorrect console app call: argument list is empty.\n\nThis will not record any entries (it doesn't count as a run).", ConsoleColor.Magenta);
                Thread.Sleep(DISPLAY_TIME_INCORRECT_CALL);
                return;
            }

            int? jobID = null;
            JobRunResult result = null;
            try
            {
                jobID = int.Parse(args[0]);
                Log(LogLevel.Big, $"Running job \"{JobFactory.GetName(jobID.Value)}\"...", ConsoleColor.Yellow);
                BaseJob job = JobFactory.GetJob(jobID.Value);
                result = job.Run(Log);
            }
            catch (SmtpException smtp)
            {
                result = new JobRunResult(jobID ?? Const.JOB_UNKNOWN, DateTime.Now, JobRunErrorCodes.EMAIL_SENDING_ERROR, smtp.ToString());
            }
            catch (Exception e)
            {
                result = new JobRunResult(jobID ?? Const.JOB_UNKNOWN, DateTime.Now, JobRunErrorCodes.UNKNOWN_ERROR, e.ToString());
            }
            Finalize(result);
        }
    }
}
