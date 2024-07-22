namespace AutoHub.Exceptions
{
    public class AutoHubServerException : Exception
    {
        public int StatusCode;
        public static readonly string FriendlyMessage = "An unexpected error occurred";

        public AutoHubServerException(int statusCode) : base(FriendlyMessage)
        {
            StatusCode = statusCode;
        }

        public AutoHubServerException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
