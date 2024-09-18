using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Category;
public static class Command
{
    public record CreateCategoryCommand(Request.CategoryRequest Data) : ICommand;
    public record UpdateCategoryCommand(Guid Id, string Name) : ICommand;
    public record DeleteCategoryCommand(Guid Id) : ICommand;
}
