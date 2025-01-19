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

public sealed class DeleteAddressByUserIdCommandHandler(
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Address, Guid> addressRepository)
    : ICommandHandler<Command.DeleteAddressByUserIdCommand>
{
    public async Task<Result> Handle(Command.DeleteAddressByUserIdCommand request, CancellationToken cancellationToken)
    {
        UserAddress userAddress =
            await context.UserAddress.FirstOrDefaultAsync(
                x => x.UserId == request.UserId && x.AddressId == request.AddressId, cancellationToken)
            ?? throw new UserAddressException.UserAddressNotFoundException();

        Domain.Entities.Address address = await addressRepository.FindByIdAsync(request.AddressId, cancellationToken);

        List<UserAddress> userAddresses =
            await context.UserAddress.Where(x => x.UserId == request.UserId).ToListAsync(cancellationToken);

        if (userAddresses.Count <= 1)
        {
            throw new ApplicationException("Address not deleted Because User just one Address.");
        }

        context.UserAddress.Remove(userAddress);
        context.Address.Remove(address);

        if (userAddress.IsDefault == DefaultEnum.TRUE)
        {
            UserAddress? filteredList = userAddresses.FirstOrDefault(x => x != userAddress);

            filteredList.IsDefault = DefaultEnum.TRUE;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
