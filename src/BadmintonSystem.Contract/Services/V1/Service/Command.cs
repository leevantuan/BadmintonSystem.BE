using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Command
{
    public record CreateServicesCommand(Guid UserId, Request.CreateServiceRequest Data)
        : ICommand;

    public record UpdateServiceCommand(Request.UpdateServiceRequest Data)
        : ICommand;

    public record DeleteServicesCommand(List<string> Ids)
        : ICommand;
}
