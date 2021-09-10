using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class UserConversationCreated : IDomainEvent
    {
        public UserConversation Conversation { get; }

        public UserConversationCreated(UserConversation conversation)
        {
            Conversation = conversation;
        }
    }
}