using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.AdditionalServices;
public class AdditionalServiceApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/additionalServices";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // V1
        var group = app.NewVersionedApi("AdditionalService")
            .MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet(string.Empty, GetAllAdditionalService);
        group.MapGet("{additionalServiceId}", GetByIdAdditionalService);
        group.MapPost(string.Empty, CreateAdditionalService);
        group.MapPut("{additionalServiceId}", UpdateAdditionalService);
        group.MapDelete("{additionalServiceId}", DeleteAdditionalService);
    }

    public static async Task<IResult> CreateAdditionalService(ISender sender, [FromBody] Contract.Services.V2.AdditionalService.Command.CreateAdditionalServiceCommand CreateAdditionalService)
    {
        var result = await sender.Send(CreateAdditionalService);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteAdditionalService(ISender sender, Guid additionalServiceId)
    {
        var result = await sender.Send(new Contract.Services.V2.AdditionalService.Command.DeleteAdditionalServiceCommand(additionalServiceId));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetAllAdditionalService(ISender sender, string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        string? sortColumnAndOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var result = await sender.Send(new Contract.Services.V2.AdditionalService.Query.GetAllAdditionalService(searchTerm, sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder),
            pageIndex,
            pageSize));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetByIdAdditionalService(ISender sender, Guid additionalServiceId)
    {
        var result = await sender.Send(new Contract.Services.V2.AdditionalService.Query.GetAdditionalServiceByIdQuery(additionalServiceId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateAdditionalService(ISender sender, Guid additionalServiceId, [FromBody] Contract.Services.V2.AdditionalService.Command.UpdateAdditionalServiceCommand updateAdditionalService)
    {
        var updateAdditionalServiceCommand = new Contract.Services.V2.AdditionalService.Command.UpdateAdditionalServiceCommand(additionalServiceId, updateAdditionalService.Data);
        return Results.Ok(await sender.Send(updateAdditionalServiceCommand));
    }
}
