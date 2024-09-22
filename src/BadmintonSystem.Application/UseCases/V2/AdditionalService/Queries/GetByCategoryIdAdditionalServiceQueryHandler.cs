using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Queries;
public sealed class GetByCategoryIdAdditionalServiceQueryHandler : IQueryHandler<Query.GetAdditionalServiceByCategoryIdQuery, List<Response.AdditionalServiceResponse>>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryBase<Domain.Entities.AdditionalService, Guid> _additionalServiceRepository;

    public GetByCategoryIdAdditionalServiceQueryHandler(IMapper mapper, ApplicationDbContext context, IRepositoryBase<Domain.Entities.AdditionalService, Guid> additionalServiceRepository)
    {
        _mapper = mapper;
        _context = context;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Result<List<Response.AdditionalServiceResponse>>> Handle(Query.GetAdditionalServiceByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var listAdditionalServices = _additionalServiceRepository.FindAll(x => x.CategoryId == request.Id).ToList();
        if (!listAdditionalServices.Any())
            throw new AdditionalServiceException.AdditionalServiceNotFoundException(request.Id);

        var result = _mapper.Map<List<Response.AdditionalServiceResponse>>(listAdditionalServices);

        return result;
    }
}
