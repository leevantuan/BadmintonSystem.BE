using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class CreateAddressByUserIdCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Address, Guid> addressRepository)
    : ICommandHandler<Command.CreateAddressByUserIdCommand>
{
    public async Task<Result> Handle(Command.CreateAddressByUserIdCommand request, CancellationToken cancellationToken)
    {
        _ = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        Address? address = mapper.Map<Address>(request.Data);

        address.Id = Guid.NewGuid();

        var userAddress = new UserAddress
        {
            UserId = request.UserId,
            AddressId = address.Id,
            IsDefault = DefaultEnum.FALSE
        };

        context.Address.Add(address);

        context.UserAddress.Add(userAddress);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
