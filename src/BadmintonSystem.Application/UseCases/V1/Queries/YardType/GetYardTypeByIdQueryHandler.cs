using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.YardType;

public sealed class GetYardTypeByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : IQueryHandler<Query.GetYardTypeByIdQuery, Response.YardTypeResponse>
{
    public async Task<Result<Response.YardTypeResponse>> Handle
        (Query.GetYardTypeByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardType yardType = await yardTypeRepository.FindByIdAsync(request.Id, cancellationToken)
                                            ?? throw new YardTypeException.YardTypeNotFoundException(request.Id);

        Response.YardTypeResponse? result = mapper.Map<Response.YardTypeResponse>(yardType);

        return Result.Success(result);
    }
}
