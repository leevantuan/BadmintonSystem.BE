using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Service;

public sealed class GetServiceByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Service, Guid> serviceRepository)
    : IQueryHandler<Query.GetServiceByIdQuery, Response.ServiceResponse>
{
    public async Task<Result<Response.ServiceResponse>> Handle
        (Query.GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Service service = await serviceRepository.FindByIdAsync(request.Id, cancellationToken)
                                          ?? throw new ServiceException.ServiceNotFoundException(request.Id);

        Response.ServiceResponse? result = mapper.Map<Response.ServiceResponse>(service);

        return Result.Success(result);
    }
}
