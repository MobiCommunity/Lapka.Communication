using System;
using System.Collections.Generic;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class UserConversation : AggregateRoot
    {
        private ISet<Guid> _members = new HashSet<Guid>();
        private IList<UserMessage> _userMessages = new List<UserMessage>();

        public IEnumerable<Guid> Members
        {
            get => _members;
            private set => _members = new HashSet<Guid>(value);
        }
        public IEnumerable<UserMessage> Messages
        {
            get => _userMessages;
            private set => _userMessages = new List<UserMessage>(value);
        }

        public UserConversation(Guid id, IEnumerable<Guid> members, IEnumerable<UserMessage> messages)
        {
            Id = new AggregateId(id);
            Members = members;
            Messages = messages;
        }

        public static UserConversation Create(Guid id, IEnumerable<Guid> members, IEnumerable<UserMessage> messages)
        {
            UserConversation conversation = new UserConversation(id, members, messages);

            conversation.AddEvent(new UserConversationCreated(conversation));
            return conversation;
        }
        
        public void AddMessage(UserMessage message)
        {
            _userMessages.Add(message);
            AddEvent(new UserMessageAdded(this));
        }

        public void Delete()
        {
            AddEvent(new UserConversationDeleted(this));
        }
    }
}