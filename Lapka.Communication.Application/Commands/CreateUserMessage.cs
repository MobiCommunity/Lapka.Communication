using System;
using Convey.CQRS.Commands;

namespace Lapka.Communication.Application.Commands
{
    public class CreateUserMessage : ICommand
    {
        public Guid UserId { get; }
        public Guid ReceiverId { get; }
        public string Message { get; }
        public DateTime CreatedAt { get; }

        public CreateUserMessage(Guid userId, Guid receiverId, string message, DateTime createdAt)
        {
            UserId = userId;
            ReceiverId = receiverId;
            Message = message;
            CreatedAt = createdAt;
        }
    }
}