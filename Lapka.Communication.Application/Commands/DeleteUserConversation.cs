using System;
using Convey.CQRS.Commands;

namespace Lapka.Communication.Application.Commands
{
    public class DeleteUserConversation : ICommand
    {
        public Guid UserId { get; }
        public Guid ConversationId { get; }

        public DeleteUserConversation(Guid userId, Guid conversationId)
        {
            UserId = userId;
            ConversationId = conversationId;
        }
    }
}