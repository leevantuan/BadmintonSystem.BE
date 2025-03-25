using System.Runtime.CompilerServices;
using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetClubByIdQueryHandler(
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetClubByIdQuery, Response.ClubDetailByIdResponse>
{
    public async Task<Result<Response.ClubDetailByIdResponse>> Handle
        (Query.GetClubByIdQuery request, CancellationToken cancellationToken)
    {
        string clubColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Club,
                Response.ClubDetail>();

        string clubInformationColumns = StringExtension
            .TransformPropertiesToSqlAliases<ClubInformation,
                Contract.Services.V1.ClubInformation.Response.ClubInformationDetailResponse>();

        string clubImageColumns = StringExtension
            .TransformPropertiesToSqlAliases<ClubImage,
                Contract.Services.V1.ClubImage.Response.ClubImageDetailResponse>();

        string clubAddressColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Address,
                Contract.Services.V1.Address.Response.AddressDetailResponse>();

        string reviewColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.Review,
                Contract.Services.V1.Review.Response.ReviewDetailResponse>();

        string reviewImageColumns = StringExtension
            .TransformPropertiesToSqlAliases<Domain.Entities.ReviewImage,
                Contract.Services.V1.ReviewImage.Response.ReviewImageDetailResponse>();

        var queryBuilder = new StringBuilder();
        queryBuilder.Append($@"SELECT {clubColumns}, {clubInformationColumns}, {clubImageColumns}, {clubAddressColumns}, {reviewColumns}, {reviewImageColumns}
                                FROM ""{nameof(Domain.Entities.Club)}"" AS club
                                JOIN ""{nameof(ClubInformation)}"" AS clubInformation
                                ON clubInformation.""{nameof(ClubInformation.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(ClubImage)}"" AS clubImage
                                ON clubImage.""{nameof(ClubImage.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(ClubAddress)}"" AS clubAddress
                                ON clubAddress.""{nameof(ClubAddress.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(Domain.Entities.Address)}"" AS address
                                ON address.""{nameof(Domain.Entities.Address.Id)}"" = clubAddress.""{nameof(ClubAddress.AddressId)}""
                                LEFT JOIN ""{nameof(Domain.Entities.Review)}"" AS review
                                ON review.""{nameof(Domain.Entities.Review.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(Domain.Entities.ReviewImage)}"" AS reviewImage
                                ON reviewImage.""{nameof(Domain.Entities.ReviewImage.ReviewId)}"" = review.""{nameof(Domain.Entities.Review.Id)}""
                                WHERE club.""{nameof(Domain.Entities.Club.Id)}"" = '{request.Id.ToString()}'");

        List<Response.GetClubByIdDetailSql> queryResult = await clubRepository
            .ExecuteSqlQuery<Response.GetClubByIdDetailSql>(
                FormattableStringFactory.Create(queryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group By
        Response.ClubDetailByIdResponse? result = queryResult.GroupBy(p => p.Club_Id)
            .Select(g => new Response.ClubDetailByIdResponse
            {
                Id = g.Key ?? Guid.Empty,
                Name = g.First().Club_Name,
                Hotline = g.First().Club_Hotline,
                OpeningTime = g.First().Club_OpeningTime,
                ClosingTime = g.First().Club_ClosingTime,
                Code = g.First().Club_Code,
                ClubInformation = g.Where(x => x.ClubInformation_Id != null)
                    .Select(s => new Contract.Services.V1.ClubInformation.Response.ClubInformationDetailResponse
                    {
                        Id = s.ClubInformation_Id,
                        FacebookPageLink = s.ClubInformation_FacebookPageLink,
                        InstagramLink = s.ClubInformation_InstagramLink,
                        MapLink = s.ClubInformation_MapLink
                    })
                    .DistinctBy(s => s.Id)
                    .FirstOrDefault(),

                ClubImages = g.Where(x => x.ClubImage_Id != null)
                    .Select(s => new Contract.Services.V1.ClubImage.Response.ClubImageDetailResponse
                    {
                        Id = s.ClubImage_Id ?? Guid.Empty,
                        ImageLink = s.ClubImage_ImageLink
                    })
                    .Distinct()
                    .ToList(),

                ClubAddress = g.Where(x => x.Address_Id != null)
                    .Select(s => new Contract.Services.V1.Address.Response.AddressDetailResponse
                    {
                        Id = s.Address_Id ?? Guid.Empty,
                        Unit = s.Address_Unit,
                        Street = s.Address_Street,
                        AddressLine1 = s.Address_AddressLine1,
                        AddressLine2 = s.Address_AddressLine2,
                        City = s.Address_City,
                        Province = s.Address_Province
                    })
                    .DistinctBy(s => s.Id)
                    .FirstOrDefault(),

                Reviews = g.Where(x => x.Review_Id != null)
                    .Select(s => new Contract.Services.V1.Review.Response.GetReviewDetailResponse
                    {
                        Id = s.Review_Id ?? Guid.Empty,
                        RatingValue = s.Review_RatingValue,
                        Comment = s.Review_Comment,
                        UserId = s.Review_UserId,
                        ClubId = s.Review_ClubId,
                        ReviewImages = g.Where(x => x.Review_Id == s.Review_Id)
                            .Select(x => new Contract.Services.V1.ReviewImage.Response.ReviewImageDetailResponse
                            {
                                Id = x.ReviewImage_Id ?? Guid.Empty,
                                ImageLink = x.ReviewImage_ImageLink,
                                ReviewId = x.ReviewImage_ReviewId
                            })
                            .DistinctBy(x => x.Id)
                            .ToList()
                    })
                    .DistinctBy(s => s.Id)
                    .ToList()
            })
            .FirstOrDefault();

        return Result.Success(result);
    }
}
