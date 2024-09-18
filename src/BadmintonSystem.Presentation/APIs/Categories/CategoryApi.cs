using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.Categories;
public class CategoryApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/categories";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // V1
        var group = app.NewVersionedApi("Category")
            .MapGroup(BaseUrl).HasApiVersion(2);

        group.MapGet(string.Empty, GetAllCategory);
        group.MapGet("{categoryId}", GetByIdCategory);
        group.MapPost(string.Empty, CreateCategory);
        group.MapPut("{categoryId}", UpdateCategory);
        group.MapDelete("{categoryId}", DeleteCategory);

    }

    public static async Task<IResult> CreateCategory(ISender sender, [FromBody] Contract.Services.V2.Category.Command.CreateCategoryCommand CreateCategory)
    {
        var result = await sender.Send(CreateCategory);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteCategory(ISender sender, Guid categoryId)
    {
        var result = await sender.Send(new Contract.Services.V2.Category.Command.DeleteCategoryCommand(categoryId));
        return Results.Ok(result);
    }

    public static async Task<IResult> GetAllCategory(ISender sender)
    {
        var result = await sender.Send(new Contract.Services.V2.Category.Query.GetAllCategory());
        return Results.Ok(result);
    }

    public static async Task<IResult> GetByIdCategory(ISender sender, Guid categoryId)
    {
        var result = await sender.Send(new Contract.Services.V2.Category.Query.GetCategoryByIdQuery(categoryId));
        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateCategory(ISender sender, Guid categoryId, [FromBody] Contract.Services.V2.Category.Command.UpdateCategoryCommand updateCategory)
    {
        var updateCategoryCommand = new Contract.Services.V2.Category.Command.UpdateCategoryCommand(categoryId, updateCategory.Name);
        return Results.Ok(await sender.Send(updateCategoryCommand));
    }
}
