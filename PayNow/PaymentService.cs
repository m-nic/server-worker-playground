using CommonContracts;
using CommonContracts.Payment;
using Hangfire;
using MediatR;
using System;

namespace PayNow;

internal class PaymentService
{
    internal static void Pay()
    {
        var scheduler = new HangFireScheduler();
        scheduler.Do(
            new ProcessPaymentCommand { Amount = 100, Currency = "USD" }
        );
        Console.WriteLine("Payment scheduled.");
    }
}

internal class HangFireScheduler
{
    public void Do<T>(T command) where T : IRequest
    {
        BackgroundJob.Enqueue<IScheduler>((s) => s.ExecuteLater(command));
    }
}