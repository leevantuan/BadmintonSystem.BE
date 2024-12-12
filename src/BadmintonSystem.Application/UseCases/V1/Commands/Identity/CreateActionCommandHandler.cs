using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class CreateActionCommandHandler : ICommandHandler<Command.CreateActionCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Identity.Action, int> _actionRepository;

    public CreateActionCommandHandler(IRepositoryBase<Domain.Entities.Identity.Action, int> actionRepository)
    {
        _actionRepository = actionRepository;
    }

    public async Task<Result> Handle(Command.CreateActionCommand request, CancellationToken cancellationToken)
    {
        // check exist
        var actionByName = await _actionRepository.FindSingleAsync(x => x.Name.ToUpper() == request.Data.Name.Trim().ToUpper());

        if (actionByName != null)
        {
            throw new IdentityException.ActionAlreadyExistException(actionByName.Name);
        }

        // get current highest order
        var actionHasHighestSortOrder = await _actionRepository.FindAll().OrderByDescending(x => x.SortOrder).FirstOrDefaultAsync(cancellationToken);

        const int DEFAULT_SORT_ORDER = 1;

        var currentHighestOrder = actionHasHighestSortOrder != null ? actionHasHighestSortOrder.SortOrder + 1 : DEFAULT_SORT_ORDER;

        var action = new Domain.Entities.Identity.Action()
        {
            Name = request.Data.Name.Trim().ToUpper(),
            SortOrder = currentHighestOrder ?? DEFAULT_SORT_ORDER
        };

        _actionRepository.Add(action);

        return Result.Success();
    }
}
