using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class HelpShelterMessageCreated : IDomainEvent
    {
        public HelpShelterMessage Message { get; }

        public HelpShelterMessageCreated(HelpShelterMessage message)
        {
            Message = message;
        }
    }
}