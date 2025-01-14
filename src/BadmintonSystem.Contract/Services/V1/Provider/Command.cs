using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Provider;

public static class Command
{
    public record CreateProviderCommand(Guid UserId, Request.CreateProviderRequest Data)
        : ICommand;

    public record UpdateProviderCommand(Request.UpdateProviderRequest Data)
        : ICommand;

    public record DeleteProvidersCommand(List<string> Ids)
        : ICommand;
}
