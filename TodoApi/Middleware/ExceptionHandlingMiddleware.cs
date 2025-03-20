using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TodoApi.Middleware
{
  /// <summary>
  /// Middleware that catches exceptions and returns a standardized error response.
  /// </summary>
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        // Proceed to the next middleware/component
        await _next(context);
      }
      catch (Exception ex)
      {
        // Log the error
        _logger.LogError(ex, "An unhandled exception occurred.");

        // Handle the exception and send the appropriate response
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      // Default to 500 Internal Server Error
      HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
      string message = exception.Message;

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;

      // You can also return a more structured response (e.g., ProblemDetails)
      var response = JsonSerializer.Serialize(new { error = message });
      return context.Response.WriteAsync(response);
    }
  }
}
