using System.Net;

namespace WeatherForecast.Middlewares
{
    public class GlobalExceptionHandlingMiddlewares
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddlewares> _logger;

        public GlobalExceptionHandlingMiddlewares(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddlewares> logger)
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
                var directoryPath = "WeatherForecast";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                await File.AppendAllTextAsync(Path.Combine(directoryPath, "logger.txt"), ex.ToString());

                _logger.LogError(ex, "An unhandled exception has occurred.\n");
                
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new
                {
                    Message = "An unexpected error occurred. Please try again later."
                };
                
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}