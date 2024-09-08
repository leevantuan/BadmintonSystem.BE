using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Gender;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Queries;

public sealed class GetByIdGenderQueryHandler : IQueryHandler<Query.GetGenderByIdQuery, Response.GenderResponse>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Gender, Guid> _genderRepository;

    public GetByIdGenderQueryHandler(IMapper mapper, IRepositoryBase<Gender, Guid> genderRepository)
    {
        _mapper = mapper;
        _genderRepository = genderRepository;
    }

    public async Task<Result<Response.GenderResponse>> Handle(Query.GetGenderByIdQuery request, CancellationToken cancellationToken)
    {
        var gender = await _genderRepository.FindByIdAsync(request.Id) ??
            throw new GenderException.GenderNotFoundException(request.Id);

        return _mapper.Map<Response.GenderResponse>(gender);
    }
}
