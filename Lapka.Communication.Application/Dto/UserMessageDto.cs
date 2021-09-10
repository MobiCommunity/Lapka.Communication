using System;

namespace Lapka.Communication.Application.Dto
{
    public class UserMessageDto
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUserSender { get; set; }
    }
}