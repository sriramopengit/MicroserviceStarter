using System.Net;
using System.Text.Json;


namespace InventoryService.Middleware;
public class ErrorHandlerMiddleware
{
    // Reference to the next middleware in the pipeline
    private readonly RequestDelegate _next;

    // Logger to log any unhandled exceptions
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;


    // Constructor to inject dependencies: next middleware and logger
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    // This method is called automatically for each HTTP request
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Pass control to the next middleware component or the endpoint (controller)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception details with a custom message
            _logger.LogError(ex, "An unhandled exception occurred");

            // Set the HTTP status code to 500 (Internal Server Error)
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // Set the response content type to JSON so that the client receives a proper error object
            context.Response.ContentType = "application/json";

            // Create a simple error response object to return to the client
            //var result = new
            //{
            //    message = "An unexpected error occurred.",   // Generic error message
            //    details = ex.Message                         // Include actual exception message (hide in production if needed)
            //};


            var result = _env.IsDevelopment()
            ? new
            {
                message = "An unexpected error occurred.",
                details = ex.Message // Show detailed error in development
            }
            : new
            {
                message = "An unexpected error occurred. Please contact support.", // No internal details in production
                details = string.Empty
            };

            // Serialize and write the error response as JSON to the response body
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
