using System.Runtime.CompilerServices;
using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Club;

public sealed class GetClubsWithFilterAndSortValueQueryHandler(
    IMapper mapper,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Club, Guid> clubRepository)
    : IQueryHandler<Query.GetClubsWithFilterAndSortValueQuery, PagedResult<Response.ClubDetailResponse>>
{
    public async Task<Result<PagedResult<Response.ClubDetailResponse>>> Handle
        (Query.GetClubsWithFilterAndSortValueQuery request, CancellationToken cancellationToken)
    {
        // Page Index and Page Size
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Notification>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<Domain.Entities.Notification>.DefaultPageSize
            : request.Data.PageSize > PagedResult<Domain.Entities.Notification>.UpperPageSize
                ? PagedResult<Domain.Entities.Notification>.UpperPageSize
                : request.Data.PageSize;

        // Handle Query SQL
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

        var queryBuilder = new StringBuilder();
        queryBuilder.Append($@"SELECT {clubColumns}, {clubInformationColumns}, {clubImageColumns}, {clubAddressColumns}
                                FROM ""{nameof(Domain.Entities.Club)}"" AS club
                                JOIN ""{nameof(ClubInformation)}"" AS clubInformation
                                ON clubInformation.""{nameof(ClubInformation.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(ClubImage)}"" AS clubImage
                                ON clubImage.""{nameof(ClubImage.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(ClubAddress)}"" AS clubAddress
                                ON clubAddress.""{nameof(ClubAddress.ClubId)}"" = club.""{nameof(Domain.Entities.Club.Id)}""
                                JOIN ""{nameof(Domain.Entities.Address)}"" AS address
                                ON address.""{nameof(Domain.Entities.Address.Id)}"" = clubAddress.""{nameof(ClubAddress.AddressId)}""");

        List<Response.GetClubDetailSql> queryResult = await clubRepository
            .ExecuteSqlQuery<Response.GetClubDetailSql>(
                FormattableStringFactory.Create(queryBuilder.ToString()))
            .ToListAsync(cancellationToken);

        // Group By
        List<Response.ClubDetailResponse>? resultClub = queryResult.GroupBy(p => p.Club_Id)
            .Select(g => new Response.ClubDetailResponse
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
                    .FirstOrDefault()
            })
            .ToList();

        var results =
            PagedResult<Response.ClubDetailResponse>.Create(resultClub, pageIndex, pageSize, resultClub.Count());

        return Result.Success(results);
    }
}
