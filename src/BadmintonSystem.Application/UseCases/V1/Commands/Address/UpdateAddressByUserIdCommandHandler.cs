using System.Text;
using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Address;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Address;

public sealed class UpdateAddressByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Address, Guid> addressRepository)
    : ICommandHandler<Command.UpdateAddressByUserIdCommand>
{
    public async Task<Result> Handle(Command.UpdateAddressByUserIdCommand request, CancellationToken cancellationToken)
    {
        UserAddress userAddress =
            context.UserAddress.FirstOrDefault(x => x.UserId == request.UserId && x.AddressId == request.Data.Id)
            ?? throw new UserAddressException.UserAddressNotFoundException();

        Domain.Entities.Address address = await addressRepository.FindByIdAsync(request.Data.Id, cancellationToken);

        address.Unit = request.Data.Unit ?? address.Unit;
        address.Street = request.Data.Street ?? address.Street;
        address.AddressLine1 = request.Data.AddressLine1 ?? address.AddressLine1;
        address.AddressLine2 = request.Data.AddressLine2 ?? address.AddressLine2;
        address.City = request.Data.City ?? address.City;
        address.Province = request.Data.Province ?? address.Province;

        if (request.Data.IsDefault == 1)
        {
            var updateQueryBuilder = new StringBuilder();
            updateQueryBuilder.Append(@$"UPDATE ""{nameof(UserAddress)}""
                                         SET ""{nameof(UserAddress.IsDefault)}"" = 0
                                         WHERE ""{nameof(UserAddress.UserId)}"" = '{request.UserId}' ");

            context.Database.ExecuteSqlRaw(updateQueryBuilder.ToString());

            await context.SaveChangesAsync(cancellationToken);
        }

        userAddress.IsDefault = (DefaultEnum)request.Data.IsDefault;

        return Result.Success();
    }
}
