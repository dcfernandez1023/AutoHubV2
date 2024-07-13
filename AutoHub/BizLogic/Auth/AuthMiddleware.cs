using System.Security.Claims;

namespace AutoHub.BizLogic.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _excludedRoutes;

        public AuthMiddleware(RequestDelegate next, List<string> excludedRoutes)
        {
            _next = next;
            _excludedRoutes = excludedRoutes;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (path == null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }

            if (_excludedRoutes.Any(route => path.StartsWith(route, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var routeData = context.GetRouteData();
            var userIdFromRoute = routeData.Values["userId"]?.ToString();
            var userIdFromToken = context.User.Identity.Name;

            Console.WriteLine("UserId from Route: " + userIdFromRoute);
            Console.WriteLine("UserId from Token: " + context.User.Identity.Name);

            if (userIdFromRoute == null || userIdFromToken == null || userIdFromRoute != userIdFromToken)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }
    }

}
