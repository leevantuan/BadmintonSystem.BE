using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Provider;

public sealed class DeleteProvidersCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : ICommandHandler<Command.DeleteProvidersCommand>
{
    public async Task<Result> Handle(Command.DeleteProvidersCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Provider> providers = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Provider provider = await providerRepository.FindByIdAsync(idValue)
                                                ?? throw new ProviderException.ProviderNotFoundException(idValue);

            providers.Add(provider);
        }

        providerRepository.RemoveMultiple(providers);

        return Result.Success();
    }
}
