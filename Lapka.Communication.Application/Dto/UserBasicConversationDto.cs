using System;

namespace Lapka.Communication.Application.Dto
{
    public class UserBasicConversationDto
    {
        public Guid ConversationId { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageCreation { get; set; }
    }
}