using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public class CreateGenderCommandHandler : ICommandHandler<Command.CreateGenderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Gender, Guid> _genderRepository;

    public CreateGenderCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IRepositoryBase<Gender, Guid> genderRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _genderRepository = genderRepository;
    }

    public async Task<Result> Handle(Command.CreateGenderCommand request, CancellationToken cancellationToken)
    {
        // Is Name Exists
        var isNameExists = await _genderRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();
        //var isNameExists = await _genderRepository.FindSingleAsync(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim()));
        if (isNameExists != null)
            throw new GenderException.GenderBadRequestException("Name Exists!");

        // Map data into Entities
        var gender = _mapper.Map<Gender>(request.Data);

        _genderRepository.Add(gender);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
