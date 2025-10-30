using MediatR;

namespace CommonContracts.Payment;
public class ProcessPaymentCommand : IRequest
{
    public int Amount { get; set; }
    public string Currency { get; set; }
}
