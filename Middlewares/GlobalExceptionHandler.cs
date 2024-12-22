namespace TaskManagerDapper.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandler(RequestDelegate next)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    public static class GlobalExceptionHandlerExtention
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
