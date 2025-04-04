using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.BookingHistory;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.BookingHistory.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class BookingHistoryApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/booking-histories";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("booking histories")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateBookingHistoryV1)
            .AllowAnonymous();

        group1.MapDelete("{id}", DeleteBookingHistorysV1)
            .AllowAnonymous();

        group1.MapGet("{id}", GetBookingHistorysV1)
            .AllowAnonymous();
    }

    private static async Task<IResult> GetBookingHistorysV1
    (
        ISender sender,
        Guid id,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        var result =
            await sender.Send(new Query.GetBookingHistoriesWithFilterAndSortValueQuery(id, pagedQueryRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateBookingHistoryV1
    (
        ISender sender,
        [FromBody] Request.CreateBookingHistoryRequest createBookingHistory)
    {
        var result =
            await sender.Send(new Command.CreateBookingHistoryCommand(createBookingHistory));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteBookingHistorysV1(ISender sender, Guid id)
    {
        Result result = await sender.Send(new Command.DeleteBookingHistoryCommand(id));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }
}
