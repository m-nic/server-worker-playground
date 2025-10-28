using Common;
using MediatR;
using System.Threading.Tasks;

namespace PayNowWorker
{
    class HangFireScheduler : IScheduler
    {
        private readonly IMediator _mediator;

        public HangFireScheduler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task ExecuteLater<T>(T command) where T : IRequest
        {
            return _mediator.Send(command);
        }
    }
}