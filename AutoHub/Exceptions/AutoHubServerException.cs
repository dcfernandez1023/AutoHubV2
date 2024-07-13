namespace AutoHub.Exceptions
{
    public class AutoHubServerException : Exception
    {
        public static readonly int StatusCode = StatusCodes.Status500InternalServerError;
        public static readonly string FriendlyMessage = "An unexpected error ocurred";

        public AutoHubServerException() : base(FriendlyMessage) { }

        public AutoHubServerException(string message) : base(message) { }
    }
}
