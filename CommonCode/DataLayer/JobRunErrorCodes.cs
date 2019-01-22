namespace CommonCode.DataLayer
{
    public static class JobRunErrorCodes
    {
        public const int UNKNOWN_ERROR = -1;
        public const int NO_ERROR = 0;
        public const int SHOULD_NOT_RUN = 1;
        public const int EMAIL_SENDING_ERROR = 2;

        public static bool ThereWasAProblem(int code)
            => code != NO_ERROR && code != SHOULD_NOT_RUN;

        public static string GetErrorCodeDescription(int code)
        {
            switch (code)
            {
                case UNKNOWN_ERROR:
                    return "Unknown error (the error was not expected and it wasn't returned as such; see the error description field associated to the job run result)";
                case NO_ERROR:
                    return "There is no error (the error description field associated to the job run result should be null)";
                case SHOULD_NOT_RUN:
                    return "There is no error (the job was not run, because that's what was decided)";
                case EMAIL_SENDING_ERROR:
                    return "There was some error related to the sending of the notification email (see the error description for more information)";
                default:
                    return $"Unknown error code {code} (not an unknown error, but an unknown error code)";
            }
        }
    }
}
