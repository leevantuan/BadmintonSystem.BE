using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Yard;

public sealed class GetYardByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : IQueryHandler<Query.GetYardByIdQuery, Response.YardResponse>
{
    public async Task<Result<Response.YardResponse>> Handle
        (Query.GetYardByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Yard yard = await yardRepository.FindByIdAsync(request.Id, cancellationToken)
                                    ?? throw new YardException.YardNotFoundException(request.Id);

        Response.YardResponse? result = mapper.Map<Response.YardResponse>(yard);

        return Result.Success(result);
    }
}
