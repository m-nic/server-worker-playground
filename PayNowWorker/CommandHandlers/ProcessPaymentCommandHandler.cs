using CommonContracts.Payment;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PayNow.CommandHandlers
{
    internal class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand>
    {
        public async Task Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Processing payment of {request.Amount} {request.Currency}");
        }
    }
}
