using System.Text.Json;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.API.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, ICurrentTenantService currentTenantService)
    {
        try
        {
            var request = context.Request;
            var query = request.Query;
            string Tenant = query["tenant"];
            //string Email = query["email"];

            //if (!string.IsNullOrEmpty(Email))
            //{
            //    await currentUserInfoService.SetUserInfo(Email);
            //}

            if (!string.IsNullOrEmpty(Tenant))
            {
                await currentTenantService.SetTenant(Tenant);
            }
            else
            {
                context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader);
                if (string.IsNullOrEmpty(tenantFromHeader) == false)
                {
                    await currentTenantService.SetTenant(tenantFromHeader);
                }
            }

            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetErrors(exception),
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            AlreadyExistException => StatusCodes.Status400BadRequest,
            //Application.Exceptions.ValidationException => StatusCodes.Status422UnprocessableEntity,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            FormatException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            DomainException applicationException => applicationException.Title,
            _ => "Server Error"
        };

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
