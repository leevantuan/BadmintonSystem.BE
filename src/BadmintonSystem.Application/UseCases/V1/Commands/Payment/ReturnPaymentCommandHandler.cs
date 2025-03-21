using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Payment;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Payment;

public sealed class ReturnPaymentCommandHandler(
        IPaymentHub paymentHub)
    : ICommandHandler<Command.ReturnPaymentCommand>
{
    public Task<Result> Handle(Command.ReturnPaymentCommand request, CancellationToken cancellationToken)
    {
        if (request.Data == null)
        {
            throw new ApplicationException("Data is null");
        }

        if (request.Data.Type)
        {
            var result = new Response.PaymentHubResponse
            {
                Type = request.Data.Type,
                OrderId = request.Data.OrderId
            };

            paymentHub.PaymentByUserAsync(result);
        }

        return Task.FromResult(Result.Success());
    }
}
