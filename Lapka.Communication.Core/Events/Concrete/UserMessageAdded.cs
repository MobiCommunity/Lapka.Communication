using Lapka.Communication.Core.Entities;
using Lapka.Communication.Core.Events.Abstract;

namespace Lapka.Communication.Core.Events.Concrete
{
    public class UserMessageAdded : IDomainEvent
    {
        public UserConversation Conversation { get; }
        public UserMessage Message { get; }

        public UserMessageAdded(UserConversation conversation, UserMessage message)
        {
            Conversation = conversation;
            Message = message;
        }
    }
}