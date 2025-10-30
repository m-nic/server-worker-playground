using MediatR;
using System.Threading.Tasks;

namespace CommonContracts;
public interface IScheduler
{
    Task ExecuteLater<T>(T command) where T: IRequest;
}
