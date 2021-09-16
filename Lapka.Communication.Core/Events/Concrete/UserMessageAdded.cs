using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class UserMessageAdded : IDomainEvent
    {
        public UserConversation Conversation { get; }

        public UserMessageAdded(UserConversation conversation)
        {
            Conversation = conversation;
        }
    }
}