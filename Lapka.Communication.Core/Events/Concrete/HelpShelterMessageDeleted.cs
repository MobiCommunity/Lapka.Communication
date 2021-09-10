using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class HelpShelterMessageDeleted : IDomainEvent
    {
        public HelpShelterMessage Message { get; }

        public HelpShelterMessageDeleted(HelpShelterMessage message)
        {
            Message = message;
        }
    }
}