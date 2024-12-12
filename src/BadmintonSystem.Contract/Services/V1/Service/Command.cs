using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Service;

public static class Command
{
    public record CreateServiceCommand(Guid UserId, Request.CreateServiceRequest Data)
        : ICommand<Response.ServiceResponse>;

    public record UpdateServiceCommand(Request.UpdateServiceRequest Data)
        : ICommand<Response.ServiceResponse>;

    public record DeleteServicesCommand(List<string> Ids)
        : ICommand;
}
