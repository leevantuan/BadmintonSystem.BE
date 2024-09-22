using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Commands;
public sealed class CreateAdditionalServiceCommandHandler : ICommandHandler<Command.CreateAdditionalServiceCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;
    private readonly IRepositoryBase<Domain.Entities.AdditionalService, Guid> _additionalServiceRepository;

    public CreateAdditionalServiceCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IPublisher publisher,
                                      IRepositoryBase<Domain.Entities.AdditionalService, Guid> additionalServiceRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publisher = publisher;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Result> Handle(Command.CreateAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var isNameExists = await _additionalServiceRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();
        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Name Exists"));

        var additionalService = Domain.Entities.AdditionalService.CreateAdditionalService(request.Data.Name, request.Data.Price, request.Data.ClubsId, request.Data.CategoryId);

        _additionalServiceRepository.Add(additionalService);

        await Task.WhenAll(
            _publisher.Publish(new DomainEvent.AdditionalServiceCreated(additionalService.Id), cancellationToken),
            _publisher.Publish(new DomainEvent.AdditionalServiceDeleted(additionalService.Id), cancellationToken));

        return Result.Success();
    }
}
