using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class DeleteServicesCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.DeleteServicesCommand>
{
    public async Task<Result> Handle(Command.DeleteServicesCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Service> categories = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Service service = await serviceRepository.FindByIdAsync(idValue)
                                              ?? throw new ServiceException.ServiceNotFoundException(idValue);

            categories.Add(service);
        }

        serviceRepository.RemoveMultiple(categories);

        return Result.Success();
    }
}
