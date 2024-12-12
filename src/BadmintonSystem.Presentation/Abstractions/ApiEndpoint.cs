using BadmintonSystem.Contract.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Abstractions;
public abstract class ApiEndpoint
{
    protected static IResult HandleFailure(Result result)
        => result switch
        {
            // IsFailure = true & IsSuccess = true
            { IsSuccess: true }
                => throw new InvalidOperationException(), // this case cannot exists

            // IsFailure = true & IsSuccess = false & value != null
            IValidationResult validationResult
                 => Results.UnprocessableEntity(CreateProblemDetails("Validation Error", StatusCodes.Status422UnprocessableEntity, result.Error, validationResult.Errors)),

            // IsFailure = true & IsSuccess = false & typeof(Error) != ValidationError
            _ => Results.BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error))
        };

    private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null)
        => new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
