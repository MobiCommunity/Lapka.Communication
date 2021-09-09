using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class AdoptPetMessageDeleted : IDomainEvent
    {
        public AdoptPetMessage Message { get; }

        public AdoptPetMessageDeleted(AdoptPetMessage message)
        {
            Message = message;
        }
    }
}