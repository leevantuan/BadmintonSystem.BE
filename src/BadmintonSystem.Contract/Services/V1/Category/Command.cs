using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Category;

public static class Command
{
    public record CreateCategoryCommand(Guid UserId, Request.CreateCategoryRequest Data)
        : ICommand<Response.CategoryResponse>;

    public record UpdateCategoryCommand(Request.UpdateCategoryRequest Data)
        : ICommand<Response.CategoryResponse>;

    public record DeleteCategoriesCommand(List<string> Ids)
        : ICommand;
}
