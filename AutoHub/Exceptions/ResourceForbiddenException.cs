namespace AutoHub.Exceptions
{
    public class ResourceForbiddenException : Exception
    {
        public static readonly int StatusCode = StatusCodes.Status403Forbidden;
        public static readonly string FriendlyMessage = "User is not authorized to access this resource";

        public ResourceForbiddenException() : base(FriendlyMessage) { }

        public ResourceForbiddenException(string message) : base(message) { }
    }
}
