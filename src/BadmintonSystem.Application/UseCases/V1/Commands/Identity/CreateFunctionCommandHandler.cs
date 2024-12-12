using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class CreateFunctionCommandHandler : ICommandHandler<Command.CreateFunctionCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Identity.Function, int> _functionRepository;

    public CreateFunctionCommandHandler(IRepositoryBase<Domain.Entities.Identity.Function, int> functionRepository)
    {
        _functionRepository = functionRepository;
    }

    public async Task<Result> Handle(Command.CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        // check exist
        var functionByName = await _functionRepository.FindSingleAsync(x => x.Name.ToUpper() == request.Data.Name.Trim().ToUpper());

        if (functionByName != null)
        {
            throw new IdentityException.FunctionAlreadyExistException(functionByName.Name);
        }

        // get current highest order
        var functionHasHighestSortOrder = await _functionRepository.FindAll().OrderByDescending(x => x.SortOrder).FirstOrDefaultAsync(cancellationToken);

        const int DEFAULT_SORT_ORDER = 1;

        var currentHighestOrder = functionHasHighestSortOrder != null ? functionHasHighestSortOrder.SortOrder + 1 : DEFAULT_SORT_ORDER;

        var function = new Domain.Entities.Identity.Function()
        {
            Name = request.Data.Name.Trim(),
            Url = request.Data.Url.Trim(),
            SortOrder = currentHighestOrder ?? DEFAULT_SORT_ORDER,
            Key = request.Data.Name.Trim().ToUpper(),
            ActionValue = request.Data.ActionValue
        };

        _functionRepository.Add(function);

        return Result.Success();
    }
}
