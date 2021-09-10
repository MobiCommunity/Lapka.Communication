using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class StrayPetMessageCreated : IDomainEvent
    {
        public StrayPetMessage Message { get; }

        public StrayPetMessageCreated(StrayPetMessage message)
        {
            Message = message;
        }
    }
}