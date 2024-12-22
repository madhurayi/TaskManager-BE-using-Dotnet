namespace TaskManagerDapper.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context) {
            context.Response.Headers.Append("X-Transaction-Id", "1234");
            Console.WriteLine($"Request {context.Request.Method}{context.Request.Path}");
            await _next(context);
        }
    }
    public static class RequestLogginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }

    }
    
}
