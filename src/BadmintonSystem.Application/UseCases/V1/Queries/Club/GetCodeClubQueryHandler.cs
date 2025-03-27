using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetCodeClubQueryHandler(
    ApplicationDbContext context)
    : IQueryHandler<Query.GetCodeClubQuery, Response.GetCodeClubDetailResponse>
{
    public async Task<Result<Response.GetCodeClubDetailResponse>> Handle(Query.GetCodeClubQuery request, CancellationToken cancellationToken)
    {
        var club = await context.Club.Where(x => x.Name.ToLower().Trim().Contains(request.Name.ToLower())).FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Club not found");

        var result = new Response.GetCodeClubDetailResponse
        {
            Id = club.Id,
            Code = club.Code
        };

        return Result.Success(result);
    }
}
