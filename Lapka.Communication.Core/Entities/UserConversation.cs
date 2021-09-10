using System;
using System.Collections.Generic;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Core.Entities
{
    public class UserConversation : AggregateRoot
    {
        public List<Guid> Members { get; }
        public List<UserMessage> Messages { get; }
        
        public UserConversation(Guid id, List<Guid> members, List<UserMessage> messages)
        {
            Id = new AggregateId(id);
            Members = members;
            Messages = messages;
        }

        public static UserConversation Create(Guid id, List<Guid> members, List<UserMessage> messages)
        {
            UserConversation conversation = new UserConversation(id, members, messages);

            conversation.AddEvent(new UserConversationCreated(conversation));
            return conversation;
        }
        
        public void AddMessage(UserMessage message)
        {
            Messages.Add(message);
            AddEvent(new UserMessageAdded(this, message));
        }

        public void Delete()
        {
            AddEvent(new UserConversationDeleted(this));
        }
    }
}