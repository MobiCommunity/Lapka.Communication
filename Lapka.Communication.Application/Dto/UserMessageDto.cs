using System;

namespace Lapka.Communication.Application.Dto
{
    public class UserMessageDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUserSender { get; set; }
    }
}