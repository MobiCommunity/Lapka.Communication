using System;
using System.IO;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Core.Entities
{
    public class UserMessage
    {
        public Guid SenderUserId { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }


        public UserMessage(Guid senderUserId, string message, DateTime createdAt)
        {
            SenderUserId = senderUserId;
            Message = message;
            CreatedAt = createdAt;
        }
    }
}