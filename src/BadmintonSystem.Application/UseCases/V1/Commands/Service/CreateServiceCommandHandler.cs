using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Service;

public sealed class CreateServiceCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : ICommandHandler<Command.CreateServiceCommand, Response.ServiceResponse>
{
    public Task<Result<Response.ServiceResponse>> Handle
        (Command.CreateServiceCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = mapper.Map<Domain.Entities.Service>(request.Data);

        serviceRepository.Add(service);

        Response.ServiceResponse? result = mapper.Map<Response.ServiceResponse>(service);

        return Task.FromResult(Result.Success(result));
    }
}
