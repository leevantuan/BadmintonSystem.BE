using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Address;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Response = BadmintonSystem.Contract.Services.V1.User.Response;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Address;

public sealed class GetAddressByIdQueryHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Address, Guid> addressRepository,
    IMapper mapper)
    : IQueryHandler<Query.GetAddressesByIdQuery, Response.AddressByUserDetailResponse>
{
    public async Task<Result<Response.AddressByUserDetailResponse>> Handle
        (Query.GetAddressesByIdQuery request, CancellationToken cancellationToken)
    {
        var query = from address in context.Address
            join userAddress in context.UserAddress
                on address.Id equals userAddress.AddressId
                into userAddresses
            from userAddress in userAddresses.DefaultIfEmpty()
            where address.IsDeleted == false && address.Id == request.AddressId
            select new { address, userAddress };

        Response.AddressByUserDetailResponse? result = await query.AsTracking()
            .Select(x => new Response.AddressByUserDetailResponse
            {
                Id = x.address.Id,
                Unit = x.address.Unit,
                Street = x.address.Street,
                AddressLine1 = x.address.AddressLine1,
                AddressLine2 = x.address.AddressLine2,
                City = x.address.City,
                Province = x.address.Province,
                IsDefault = x.userAddress.IsDefault == DefaultEnum.TRUE ? 1 : 0
            }).FirstOrDefaultAsync(cancellationToken);

        return Result.Success(result);
    }
}
