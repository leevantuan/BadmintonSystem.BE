using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetTopClubsQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetTopClubsQuery, List<Response.ClubDetailResponseChatBot>>
{
    public async Task<Result<List<Response.ClubDetailResponseChatBot>>> Handle(Query.GetTopClubsQuery request, CancellationToken cancellationToken)
    {

        var queryBuilder = new StringBuilder();

        queryBuilder.Append($@"SELECT 
                            club.""{nameof(Domain.Entities.Club.Name)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_Name)}"",
                            club.""{nameof(Domain.Entities.Club.Hotline)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_Hotline)}"",
                            club.""{nameof(Domain.Entities.Club.OpeningTime)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_OpeningTime)}"",
                            club.""{nameof(Domain.Entities.Club.ClosingTime)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_ClosingTime)}"",
                            club.""{nameof(Domain.Entities.Club.Code)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_Code)}"",
                            club.""{nameof(Domain.Entities.Club.Id)}"" AS ""{nameof(Response.ClubDetailResponseChatBotSql.Club_Id)}"",
                            COALESCE(AVG(review.""RatingValue""), 0) AS ""{nameof(Response.ClubDetailResponseChatBotSql.Average_Rating)}""
                        FROM ""{nameof(Domain.Entities.Club)}"" AS club
                        LEFT JOIN ""{nameof(Domain.Entities.Review)}"" AS review ON review.""{nameof(Domain.Entities.Review.ClubId)}"" = club.""Id""
                        WHERE review.""RatingValue"" IS NOT NULL
                        GROUP BY club.""{nameof(Domain.Entities.Club.Id)}"", club.""{nameof(Domain.Entities.Club.Name)}"", club.""{nameof(Domain.Entities.Club.Hotline)}"", club.""{nameof(Domain.Entities.Club.OpeningTime)}"", club.""{nameof(Domain.Entities.Club.ClosingTime)}"", club.""{nameof(Domain.Entities.Club.Code)}""
                        HAVING COUNT(review.""{nameof(Domain.Entities.Review.Id)}"") > 0
                        ORDER BY ""Average_Rating"" DESC
                        LIMIT {request.quantity};");

        List<Response.ClubDetailResponseChatBotSql> queryResult = await clubRepository
                .ExecuteSqlQuery<Response.ClubDetailResponseChatBotSql>(
                    FormattableStringFactory.Create(queryBuilder.ToString()))
                .ToListAsync(cancellationToken);

        // Group By
        List<Response.ClubDetailResponseChatBot>? resultClub = queryResult.GroupBy(p => p.Club_Id)
            .Select(g => new Response.ClubDetailResponseChatBot
            {
                Id = g.Key ?? Guid.Empty,
                Name = g.First().Club_Name,
                Description = g.First().Club_Description,
                Hotline = g.First().Club_Hotline,
                OpeningTime = g.First().Club_OpeningTime,
                ClosingTime = g.First().Club_ClosingTime,
                Code = g.First().Club_Code,
                AverageRating = g.First().Average_Rating
            })
            .ToList();

        return Result.Success(resultClub);
    }
}
