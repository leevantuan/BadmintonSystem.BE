using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Category;
public static class Query
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<Response.CategoryResponse>;
    public record GetAllCategory() : IQuery<List<Response.CategoryResponse>>;
}
