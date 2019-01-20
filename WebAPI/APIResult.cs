namespace WebAPI
{
    public class APIResult<TYPE>
    {
        public string ErrorMessage { get; private set; }
        public TYPE Result { get; private set; }

        public APIResult(TYPE result)
            : this(result, null)
        {
        }

        public APIResult(TYPE result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public bool Success
            => Result == null;
    }
}
