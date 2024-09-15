using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Gender.Commands;
public sealed class CreateGenderCommandHandler : ICommandHandler<Command.CreateGenderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly IRepositoryBase<Domain.Entities.Gender, Guid> _genderRepository;

    public CreateGenderCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IPublisher publisher,
                                      IRepositoryBase<Domain.Entities.Gender, Guid> genderRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _genderRepository = genderRepository;
    }

    public async Task<Result> Handle(Command.CreateGenderCommand request, CancellationToken cancellationToken)
    {
        var isNameExists = await _genderRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();

        if (isNameExists.Any())
            return Result.Failure(new Error("400", "Name Exists!"));

        var gender = _mapper.Map<Domain.Entities.Gender>(request.Data);

        _genderRepository.Add(gender);

        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.GenderCreated(gender.Id), cancellationToken),
            _publisher.Publish(new DomainEvent.GenderDeleted(gender.Id), cancellationToken));

        return Result.Success();
    }
}
