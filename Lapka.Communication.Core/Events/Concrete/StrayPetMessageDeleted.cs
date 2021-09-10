using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class StrayPetMessageDeleted : IDomainEvent
    {
        public StrayPetMessage Message { get; }

        public StrayPetMessageDeleted(StrayPetMessage message)
        {
            Message = message;
        }
    }
}