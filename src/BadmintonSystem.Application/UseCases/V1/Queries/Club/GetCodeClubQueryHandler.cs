using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetCodeClubQueryHandler(
    ApplicationDbContext context)
    : IQueryHandler<Query.GetCodeClubQuery, string>
{
    public async Task<Result<string>> Handle(Query.GetCodeClubQuery request, CancellationToken cancellationToken)
    {
        var club = await context.Club.Where(x => x.Name.ToLower().Trim().Contains(request.Name.ToLower())).FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Club not found");

        return Result.Success(club.Code ?? string.Empty);
    }
}
