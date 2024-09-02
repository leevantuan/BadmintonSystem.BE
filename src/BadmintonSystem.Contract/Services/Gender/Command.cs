using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Constants.Models;

namespace BadmintonSystem.Contract.Services.Gender;
public static class Command
{
    public record CreateGenderCommand(AddOrUpdateRequest<Guid> request) : ICommand;
    public record UpdateGenderCommand(Guid Id, AddOrUpdateRequest<Guid> request) : ICommand;
    public record DeleteGenderCommand(Guid Id) : ICommand;
}
