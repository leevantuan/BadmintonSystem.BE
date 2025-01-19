using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Provider;

public sealed class UpdateProviderCommandHandler(
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : ICommandHandler<Command.UpdateProviderCommand>
{
    public async Task<Result> Handle
        (Command.UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Provider provider =
            await providerRepository.FindByIdAsync(request.Data.Id.Value, cancellationToken)
            ?? throw new ProviderException.ProviderNotFoundException(request.Data.Id.Value);

        provider.Name = request.Data.Name ?? provider.Name;
        provider.PhoneNumber = request.Data.PhoneNumber ?? provider.PhoneNumber;
        provider.Address = request.Data.Address ?? provider.Address;

        return Result.Success();
    }
}
