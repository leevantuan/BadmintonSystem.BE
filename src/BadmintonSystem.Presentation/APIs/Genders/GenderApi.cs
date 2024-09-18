using Asp.Versioning.Builder;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Gender;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSystem.Presentation.APIs.Genders;
public static class GenderApi
{
    private const string BaseUrl = "/api/v{version:apiVersion}/genders";

    // Config V1
    public static IVersionedEndpointRouteBuilder MapGenderApiV1(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(1);

        // Example
        //group.MapPost(string.Empty, () => "Hello"); // Out trên swagger body == Hello
        group.MapPost(string.Empty, CreateGender);
        group.MapGet(string.Empty, GetAllGender);
        group.MapDelete("{genderId}", DeleteGender);
        group.MapPut("{genderId}", UpdateGender);
        group.MapGet("{genderId}", GetByIdGender);

        return builder;
    }

    // Config V2
    public static IVersionedEndpointRouteBuilder MapGenderApiV2(this IVersionedEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet(string.Empty, GetAllGender);

        return builder;
    }

    public static async Task<IResult> CreateGender(ISender sender, [FromBody] Command.CreateGenderCommand CreateGender)
    {
        var result = await sender.Send(CreateGender);

        // Custom Result Failure
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
