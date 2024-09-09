using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.Identity;
public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/authentications";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(2).ReportApiVersions();

        group1.MapPost("login", AuthenticationV2);
    }

    public static async Task<IResult> AuthenticationV2(ISender sender, [FromBody] Contract.Services.V2.Authen.Query.Login login)
    {
        var result = await sender.Send(login);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}
