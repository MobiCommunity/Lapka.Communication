using System.Threading.Tasks;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Application.Events.Abstract
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}