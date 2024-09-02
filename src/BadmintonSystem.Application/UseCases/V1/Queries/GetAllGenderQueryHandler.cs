using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries;
public class GetAllGenderQueryHandler : IQueryHandler<Query.GetAllGender, List<Response.GenderResponse>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Domain.Entities.Gender, Guid> _genderRepository;

    public GetAllGenderQueryHandler(IMapper mapper,
                                    IRepositoryBase<Domain.Entities.Gender, Guid> genderRepository)
    {
        _mapper = mapper;
        _genderRepository = genderRepository;
    }

    public async Task<Result<List<Response.GenderResponse>>> Handle(Query.GetAllGender request, CancellationToken cancellationToken)
    {
        var genders = await _genderRepository.FindAll().ToListAsync();
        return _mapper.Map<List<Response.GenderResponse>>(genders);
    }
}
