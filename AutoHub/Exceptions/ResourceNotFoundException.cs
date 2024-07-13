namespace AutoHub.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public static readonly int StatusCode = StatusCodes.Status404NotFound;
        public static readonly string FriendlyMessage = "The requested resource was null or not found";

        public ResourceNotFoundException() : base(FriendlyMessage) { }

        public ResourceNotFoundException(string message) : base(message) { }
    }
}
