using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Tenant;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Tenant;

public sealed class UpdateTenantCommandHandler(
    TenantDbContext context)
    : ICommandHandler<Command.UpdaterTenantCommand>
{
    public async Task<Result> Handle(Command.UpdaterTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(x => x.Id == request.Data.Id, cancellationToken)
            ?? throw new ApplicationException($"Tenant invalid");

        tenant.Name = request.Data.Name;
        tenant.Email = request.Data.Email;
        tenant.Address = request.Data.Address;
        tenant.City = request.Data.City;
        tenant.Image = request.Data.Image;
        tenant.Slogan = request.Data.Slogan;
        tenant.HotLine = request.Data.HotLine;
        tenant.Description = request.Data.Description;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
