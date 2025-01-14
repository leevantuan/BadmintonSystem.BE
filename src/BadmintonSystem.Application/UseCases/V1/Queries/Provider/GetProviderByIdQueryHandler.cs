using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Provider;

public sealed class GetProviderByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Provider, Guid> providerRepository)
    : IQueryHandler<Query.GetProviderByIdQuery, Response.ProviderDetailResponse>
{
    public async Task<Result<Response.ProviderDetailResponse>> Handle
        (Query.GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Provider provider = await providerRepository.FindByIdAsync(request.Id)
                                           ?? throw new ProviderException.ProviderNotFoundException(request.Id);

        Response.ProviderDetailResponse? result = mapper.Map<Response.ProviderDetailResponse>(provider);

        return Result.Success(result);
    }
}
