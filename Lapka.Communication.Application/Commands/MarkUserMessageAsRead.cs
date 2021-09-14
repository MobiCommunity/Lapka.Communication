using System;
using Convey.CQRS.Commands;

namespace Lapka.Communication.Application.Commands
{
    public class MarkUserMessageAsRead : ICommand
    {
        public Guid UserId { get; }
        public Guid ConversationId { get; }

        public MarkUserMessageAsRead(Guid userId, Guid conversationId)
        {
            UserId = userId;
            ConversationId = conversationId;
        }
    }
}