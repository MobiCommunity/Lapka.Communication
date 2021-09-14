using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class ShelterMessageCreated : IDomainEvent
    {
        public ShelterMessage Message { get; }

        public ShelterMessageCreated(ShelterMessage message)
        {
            Message = message;
        }
    }
}