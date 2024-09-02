using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public class DeleteGenderCommandHandler : ICommandHandler<Command.DeleteGenderCommand>
{
    public Task<Result> Handle(Command.DeleteGenderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
