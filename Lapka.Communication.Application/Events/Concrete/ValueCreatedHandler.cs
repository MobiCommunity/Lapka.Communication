using System;
using System.Threading.Tasks;
using Lapka.Communication.Application.Events.Abstract;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Application.Events.Concrete
{
    public class ValueCreatedHandler : IDomainEventHandler<ValueCreated>
    {

        public Task HandleAsync(ValueCreated @event)
        {
            Console.WriteLine($"i caught {@event.Value.Name}");
            return Task.CompletedTask;
        }
    }
}