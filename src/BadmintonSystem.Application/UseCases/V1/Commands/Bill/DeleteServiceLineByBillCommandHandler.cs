using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class DeleteServiceLineByBillCommandHandler(
    ApplicationDbContext context)
    : ICommandHandler<Command.DeleteServiceLineByBillCommand>
{
    public async Task<Result> Handle
        (Command.DeleteServiceLineByBillCommand request, CancellationToken cancellationToken)
    {
        _ = await context.ServiceLine.FirstOrDefaultAsync(x => x.Id == request.ServiceLineId, cancellationToken)
            ?? throw new ServiceLineException.ServiceLineNotFoundException(request.ServiceLineId);

        var deleteQueryBuilder = new StringBuilder();
        deleteQueryBuilder.Append(@$"DELETE FROM ""{nameof(ServiceLine)}""
            WHERE ""{nameof(ServiceLine.Id)}"" = '{request.ServiceLineId}'");

        await context.Database.ExecuteSqlRawAsync(deleteQueryBuilder.ToString(), cancellationToken);

        return Result.Success();
    }
}
