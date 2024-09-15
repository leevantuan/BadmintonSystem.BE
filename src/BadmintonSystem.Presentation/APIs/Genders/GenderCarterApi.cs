using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Gender;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.Genders;
public class GenderCarterApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/genders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // V1
        var group = app.NewVersionedApi("Gender")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group.MapGet(string.Empty, GetAllGender);
        group.MapGet("{genderId}", GetByIdGender);
        group.MapPost(string.Empty, CreateGender);
        group.MapPut("{genderId}", UpdateGender);
        group.MapDelete("{genderId}", DeleteGender);

        // V2
        var groupV2 = app.NewVersionedApi("Gender")
            .MapGroup(BaseUrl).HasApiVersion(2).RequireAuthorization();

        groupV2.MapGet(string.Empty, GetAllGender);

    }

    public static async Task<IResult> CreateGender(ISender sender, [FromBody] Command.CreateGenderCommand CreateGender)
    {
        var result = await sender.Send(CreateGender);

        //if (result.IsFailure)
        //    return HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> GetAllGender(ISender sender, string? searchTerm = null,
                                                  string? sortColumn = null,
                                                  string? sortOrder = null,
                                                  string? sortColumnAndOrder = null,
                                                  int pageIndex = 1,
                                                  int pageSize = 10)
    {
        return Results.Ok(await sender.Send(new Query.GetAllGender(searchTerm, sortColumn, SortOrderExtension.ConvertStringToSortOrder(sortOrder), SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder), pageIndex, pageSize)));
    }

    public static async Task<IResult> GetByIdGender(ISender sender, Guid genderId)
    {
        return Results.Ok(await sender.Send(new Query.GetGenderByIdQuery(genderId)));
    }

    public static async Task<IResult> DeleteGender(ISender sender, Guid genderId)
    {
        var result = await sender.Send(new Command.DeleteGenderCommand(genderId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateGender(ISender sender, Guid genderId, [FromBody] Command.UpdateGenderCommand updateProduct)
    {
        var updateProductCommand = new Command.UpdateGenderCommand(genderId, updateProduct.Name);
        return Results.Ok(await sender.Send(updateProductCommand));
    }
}

