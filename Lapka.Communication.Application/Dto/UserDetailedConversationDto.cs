using System;
using System.Collections.Generic;

namespace Lapka.Communication.Application.Dto
{
    public class UserDetailedConversationDto
    {
        public Guid Id { get; set; }
        public List<UserMessageDto> Messages { get; set; }
    }
}