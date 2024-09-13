﻿using BadmintonSystem.Contract.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.Abstractions;
public abstract class ApiEndpoint
{
    protected static IResult HandlerFailure(Result result) =>
       result switch
       {
           // If Fail == True && IsSuccess == True ==> Throw
           // Custom Result Failure
           { IsSuccess: true } => throw new InvalidOperationException(),
           IValidationResult validationResult =>
               Results.BadRequest(
                   CreateProblemDetails(
                       "Validation Error", StatusCodes.Status400BadRequest,
                       result.Error,
                       validationResult.Errors)),
           _ =>
               Results.BadRequest(
                   CreateProblemDetails(
                       "Bab Request", StatusCodes.Status400BadRequest,
                       result.Error))
       };

    private static ProblemDetails CreateProblemDetails(string title, int status,
        Error error,
        Error[]? errors = null) =>
        new ProblemDetails()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}