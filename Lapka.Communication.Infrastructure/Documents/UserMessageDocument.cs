using System;

namespace Lapka.Communication.Infrastructure.Documents
{
    public class UserMessageDocument
    {
        public Guid SenderUserId { get; set; }
        public bool IsReadByReceiver { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}