using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Tenant;

public class Command
{
    public record CreateTenantCommand(Request.CreateTenantRequest Data)
        : ICommand;

    public record UpdaterTenantCommand(Request.UpdateTenantRequest Data)
    : ICommand;
}
