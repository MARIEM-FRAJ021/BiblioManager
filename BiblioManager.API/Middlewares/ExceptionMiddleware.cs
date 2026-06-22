namespace BiblioManager.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue");
                context.Response.ContentType = "application/json";

                if(ex is InvalidOperationException)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"{{\"error\": \"{ex.Message}\"}}");
                }
                else if (ex is KeyNotFoundException)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync($"{{\"error\": \"{ex.Message}\"}}");
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("{\"error\": \"Erreur interne du serveur\"}");
                }
            }
        }
    }
}
