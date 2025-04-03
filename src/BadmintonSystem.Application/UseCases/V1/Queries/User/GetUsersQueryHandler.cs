using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using static BadmintonSystem.Contract.Services.V1.User.Query;
using static BadmintonSystem.Contract.Services.V1.User.Response;

namespace BadmintonSystem.Application.UseCases.V1.Queries.User;

public sealed class GetUsersQueryHandler(
    ApplicationDbContext context)
    : IQueryHandler<GetUsersQuery, PagedResult<GetUserInfoResponse>>
{
    public async Task<Result<PagedResult<GetUserInfoResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        // Pagination
        int pageIndex = request.Data.PageIndex <= 0
            ? PagedResult<Domain.Entities.Identity.AppUser>.DefaultPageIndex
            : request.Data.PageIndex;
        int pageSize = request.Data.PageSize <= 0
            ? PagedResult<AppUser>.DefaultPageSize
            : request.Data.PageSize > PagedResult<AppUser>.UpperPageSize
                ? PagedResult<AppUser>.UpperPageSize
                : request.Data.PageSize;

        var query =
                from user in context.AppUsers
                join userRole in context.UserRoles on user.Id equals userRole.UserId into userRoles
                from ur in userRoles.DefaultIfEmpty()
                join role in context.Roles on ur.RoleId equals role.Id into roles
                from r in roles.DefaultIfEmpty()
                group new { user, r } by user into grouped
                select new GetUserInfoResponse
                {
                    Id = grouped.Key.Id,
                    UserName = grouped.Key.UserName,
                    Email = grouped.Key.Email,
                    FullName = grouped.Key.FullName,
                    DateOfBirth = grouped.Key.DateOfBirth,
                    PhoneNumber = grouped.Key.PhoneNumber,
                    Gender = grouped.Key.Gender == (int)GenderEnum.MALE ? 0 : 1,
                    AvatarUrl = grouped.Key.AvatarUrl,
                    Roles = grouped.Select(x => x.r.Name).Where(name => name != null).ToList()
                };

        var result = await PagedResult<GetUserInfoResponse>.CreateAsync(
                query.AsNoTracking(),
                pageIndex,
                pageSize);

        return Result.Success(result);
    }
}
