using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class UserConversationDeleted : IDomainEvent
    {
        public UserConversation Conversation { get; }

        public UserConversationDeleted(UserConversation conversation)
        {
            Conversation = conversation;
        }
    }
}