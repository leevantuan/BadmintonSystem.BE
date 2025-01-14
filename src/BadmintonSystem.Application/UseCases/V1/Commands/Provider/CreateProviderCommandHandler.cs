using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Provider;

public sealed class CreateProviderCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : ICommandHandler<Command.CreateProviderCommand>
{
    public Task<Result> Handle
        (Command.CreateProviderCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Provider provider = mapper.Map<Domain.Entities.Provider>(request.Data);

        providerRepository.Add(provider);

        return Task.FromResult(Result.Success());
    }
}
