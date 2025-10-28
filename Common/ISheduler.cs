using MediatR;
using System.Threading.Tasks;

namespace Common;
public interface IScheduler
{
    Task ExecuteLater<T>(T command) where T: IRequest;
}
