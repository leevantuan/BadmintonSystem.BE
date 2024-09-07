using System.Text.Json;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.API.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
         => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Step 1: Try ==> if has an error => Catch
        // Call func HandlerException
        // If return next() ================> Controller
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    // Takes infor error to output
    private static async Task HandleExceptionAsync(HttpContext httpContext,
                                                   Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        // Information an error
        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception), // ==> Takes info an error Func GetErrors()
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    // Get Status Code
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            // Tự định nghĩa
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            //Application.Exceptions.ValidationException => StatusCodes.Status422UnprocessableEntity,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            FormatException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    // Get Title error
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            DomainException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    // Get Error
    // Step 4: Get information error
    // ========> Application.Exceptions.ValidationException
    private static IReadOnlyCollection<Application.Exceptions.ValidationError> GetErrors(Exception exception)
    {
        IReadOnlyCollection<Application.Exceptions.ValidationError> errors = null;

        if (exception is Application.Exceptions.ValidationException validationException)
        {
            errors = validationException.Errors;
        }

        return errors;
    }
}
