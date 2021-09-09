using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class AdoptPetMessageCreated : IDomainEvent
    {
        public AdoptPetMessage Message { get; }

        public AdoptPetMessageCreated(AdoptPetMessage message)
        {
            Message = message;
        }
    }
}