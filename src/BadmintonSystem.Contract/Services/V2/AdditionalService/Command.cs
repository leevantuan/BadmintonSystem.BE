using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService;
public static class Command
{
    public record CreateAdditionalServiceCommand(Request.AdditionalServiceRequest Data) : ICommand;
    public record UpdateAdditionalServiceCommand(Guid Id, Request.AdditionalServiceRequest Data) : ICommand;
    public record DeleteAdditionalServiceCommand(Guid Id) : ICommand;
}
