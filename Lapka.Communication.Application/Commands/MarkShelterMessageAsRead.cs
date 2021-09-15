using System;
using Convey.CQRS.Commands;

namespace Lapka.Communication.Application.Commands
{
    public class MarkShelterMessageAsRead : ICommand
    {
        public Guid UserId { get; }
        public Guid MessageId { get; }

        public MarkShelterMessageAsRead(Guid userId, Guid messageId)
        {
            UserId = userId;
            MessageId = messageId;
        }
    }
}