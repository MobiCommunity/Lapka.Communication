using System;
using Lapka.Communication.Core.Entities;

namespace Lapka.Communication.Application.Exceptions
{
    public class UserDoesNotOwnMessageException : AppException
    {
        public Guid UserId { get; }
        public Guid MessageId { get; }

        public UserDoesNotOwnMessageException(Guid userId, Guid messageId) : base(
            $"user {userId} does not own message {messageId}")
        {
            UserId = userId;
            MessageId = messageId;
        }

        public override string Code => "user_does_not_own_message";
    }
}