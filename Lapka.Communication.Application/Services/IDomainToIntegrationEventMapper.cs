using Convey.CQRS.Events;
using System.Collections.Generic;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Application.Services
{
    public interface IDomainToIntegrationEventMapper
    {
        IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events);
        IEvent Map(IDomainEvent @event);
        
    }
}