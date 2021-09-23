using System;

namespace Lapka.Communication.Core.ValueObjects
{
    public class UserMessage
    {
        public Guid SenderUserId { get; }
        public string Message { get; }
        public bool IsReadByReceiver { get; private set; }
        public DateTime CreatedAt { get; }


        public UserMessage(Guid senderUserId, string message, bool isReadByReceiver, DateTime createdAt)
        {
            SenderUserId = senderUserId;
            Message = message;
            IsReadByReceiver = isReadByReceiver;
            CreatedAt = createdAt;
        }

        public void MarkAsRead()
        {
            IsReadByReceiver = true;
        }
    }
}