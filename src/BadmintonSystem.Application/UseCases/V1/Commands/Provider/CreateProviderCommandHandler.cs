using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Provider;

public sealed class CreateProviderCommandHandler(
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : ICommandHandler<Command.CreateProviderCommand>
{
    public async Task<Result> Handle
        (Command.CreateProviderCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Provider? isNameExists =
            await providerRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

        if (isNameExists != null)
        {
            return Result.Failure(new Error("400", "Name Exists!"));
        }

        Domain.Entities.Provider provider = mapper.Map<Domain.Entities.Provider>(request.Data);

        providerRepository.Add(provider);

        return Result.Success();
    }
}
