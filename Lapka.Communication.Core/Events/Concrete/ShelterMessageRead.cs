using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class ShelterMessageRead : IDomainEvent
    {
        public ShelterMessage Message { get; }

        public ShelterMessageRead(ShelterMessage message)
        {
            Message = message;
        }
    }
}