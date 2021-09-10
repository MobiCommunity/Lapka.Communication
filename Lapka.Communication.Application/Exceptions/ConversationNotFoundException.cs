using System;

namespace Lapka.Communication.Application.Exceptions
{
    public class ConversationNotFoundException : AppException
    {
        public Guid ConversationId { get; set; }
        public ConversationNotFoundException(Guid conversationId) : base($"conversation not found: {conversationId}")
        {
            ConversationId = conversationId;
        }

        public override string Code => "conversation_not_found";
    }
}