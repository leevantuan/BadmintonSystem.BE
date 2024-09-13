using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Gender.Commands;
public sealed class UpdateGenderCommandHandler : ICommandHandler<Command.UpdateGenderCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Gender, Guid> _genderRepository;

    public UpdateGenderCommandHandler(IMapper mapper,
                                      IUnitOfWork unitOfWork,
                                      IRepositoryBase<Gender, Guid> genderRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _genderRepository = genderRepository;
    }

    public async Task<Result> Handle(Command.UpdateGenderCommand request, CancellationToken cancellationToken)
    {
        // Find By Id
        var gender = await _genderRepository.FindByIdAsync(request.Id) ??
            throw new GenderException.GenderNotFoundException(request.Id);

        // Check Is Name Exists
        var isNameExists = await _genderRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Name.ToLower().Trim())).ToListAsync();

        if (isNameExists.Any())
            throw new GenderException.GenderBadRequestException("Is Name Exists!");

        // Map
        // Domain Driven Design
        // Unprocessed
        //gender.Name = request.Name;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
