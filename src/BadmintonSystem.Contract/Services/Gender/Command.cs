using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.Gender;
public static class Command
{
    public record CreateGenderCommand(Request Data) : ICommand;
    public record UpdateGenderCommand(Guid Id, string Name) : ICommand;
    public record DeleteGenderCommand(Guid Id) : ICommand;
}
