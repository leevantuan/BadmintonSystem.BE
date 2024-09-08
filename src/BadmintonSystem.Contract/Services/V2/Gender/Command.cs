using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Gender;
public static class Command
{
    public record CreateGenderCommand(Request.GenderRequest Data) : ICommand;
    public record UpdateGenderCommand(Guid Id, string Name) : ICommand;
    public record DeleteGenderCommand(Guid Id) : ICommand;
}
