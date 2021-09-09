using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Application.Services
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent> events);
    }
}