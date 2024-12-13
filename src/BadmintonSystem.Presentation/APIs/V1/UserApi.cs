using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.User.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class UserApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/users";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("users")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost("register", RegisterV1).AllowAnonymous();
    }

    private static async Task<IResult> RegisterV1(ISender sender, [FromBody] Request.CreateUserAndAddress request)
    {
        Result result = await sender.Send(new Query.RegisterByCustomerQuery(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
