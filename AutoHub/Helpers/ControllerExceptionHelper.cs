using AutoHub.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AutoHub.Helpers
{
    public class ControllerExceptionHelper
    {
        public static (int StatusCode, string Message) HandleControllerException(Exception ex)
        {
            // TODO: This is just for debugging. Remove eventually
            Console.WriteLine("--------------- EXCEPTION OCCURRED ---------------");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("--------------- EXCEPTION END ---------------");
            int statusCode = AutoHubServerException.StatusCode;
            string message = AutoHubServerException.FriendlyMessage;

            if (ex is ResourceForbiddenException)
            {
                statusCode = ResourceForbiddenException.StatusCode;
                message = ResourceForbiddenException.FriendlyMessage;
            }
            else if (ex is ResourceNotFoundException)
            {
                statusCode = ResourceNotFoundException.StatusCode;
                message = ResourceNotFoundException.FriendlyMessage;
            }

            return (statusCode, message);
        }
    }
}
