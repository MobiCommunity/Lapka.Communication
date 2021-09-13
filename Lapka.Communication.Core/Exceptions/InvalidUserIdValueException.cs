using System;
using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidUserIdValueException : DomainException
    {
        public Guid UserId { get; }
        public InvalidUserIdValueException(Guid userId) : base($"Invalid user id: {userId}")
        {
            UserId = userId;
        }

        public override string Code => "invalid_user_id";
    }
}