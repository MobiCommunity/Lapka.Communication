using System;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Exceptions
{
    public class UserDoesNotOwnConversationException : AppException
    {
        public Guid UserId { get; }
        public Guid ConversationId { get; }

        public UserDoesNotOwnConversationException(Guid userId, Guid conversationId) : base(
            $"user {userId} does not own conversation {conversationId}")
        {
            UserId = userId;
            ConversationId = conversationId;
        }

        public override string Code => "user_does_not_own_conversation";
    }
}