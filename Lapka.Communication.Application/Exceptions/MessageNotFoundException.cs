using System;

namespace Lapka.Communication.Application.Exceptions
{
    public class MessageNotFoundException : AppException
    {
        public Guid MessageId { get; }
        public MessageNotFoundException(Guid messageId) : base($"Message not found: {messageId}")
        {
            MessageId = messageId;
        }

        public override string Code => "meesage_not_found";
    }
}