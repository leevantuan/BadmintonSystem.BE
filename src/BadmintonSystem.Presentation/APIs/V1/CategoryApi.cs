using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Category;
using BadmintonSystem.Persistence.Helpers;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.Category.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class CategoryApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/categories";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("categories")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateCategoryV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetCategoriesV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.READ);

        group1.MapGet("filter-and-sort", GetCategoriesFilterAndSortValueV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.READ);

        group1.MapGet(
                "{categoryId}/services/filter-and-sort", GetServicesByCategoryIdFilterAndSortValueV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{categoryId}", GetCategoryByIdV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{categoryId}", UpdateCategoryV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteCategoriesV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CATEGORY.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateCategoryV1
    (
        ISender sender,
        [FromBody] Request.CreateCategoryRequest createCategory, IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result = await sender.Send(new Command.CreateCategoryCommand(userId ?? Guid.Empty, createCategory));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteCategoriesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteCategoriesCommand(ids));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateCategoryV1
    (
        ISender sender, Guid id,
        [FromBody] Request.UpdateCategoryRequest updateCategory)
    {
        updateCategory.Id = id;
        Result result = await sender.Send(new Command.UpdateCategoryCommand(updateCategory));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetCategoriesV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.CategoryResponse>> result =
            await sender.Send(new Query.GetCategoriesQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetCategoryByIdV1(ISender sender, Guid categoryId)
    {
        Result<Response.CategoryResponse> result =
            await sender.Send(new Query.GetCategoryByIdQuery(categoryId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetServicesByCategoryIdFilterAndSortValueV1
    (
        ISender sender,
        Guid CategoryId,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetServicesByCategoryIdResponse>> result =
            await sender.Send(
                new Query.GetServicesByCategoryIdWithFilterAndSortValueQuery(CategoryId, pagedQueryRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetCategoriesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.CategoryDetailResponse>> result =
            await sender.Send(
                new Query.GetCategoriesWithFilterAndSortValueQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }
}
