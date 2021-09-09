using Convey.CQRS.Events;
using System.Collections.Generic;
using System.Linq;
using Lapka.Communication.Application.Services;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Infrastructure.Services
{
    public class DomainToIntegrationEventMapper : IDomainToIntegrationEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

        public IEvent Map(IDomainEvent @event) => @event switch
        {
            _ => null
        };
    }
}