using System;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class HelpShelterMessage : AggregateShelterMessage
    {
        public HelpType HelpType { get; }

        public HelpShelterMessage(Guid id, Guid userId, Guid shelterId, HelpType helpType, MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt) : base(id, userId,
            shelterId, description, fullName, phoneNumber, createdAt)
        {
            HelpType = helpType;
        }

        public static HelpShelterMessage Create(Guid id, Guid userId, Guid shelterId, HelpType helpType,
            string description, string fullName, string phoneNumber)
        {
            HelpShelterMessage message = new HelpShelterMessage(id, userId, shelterId, helpType,
                new MessageDescription(description), new FullName(fullName), new PhoneNumber(phoneNumber),
                DateTime.Now);

            message.AddEvent(new HelpShelterMessageCreated(message));
            return message;
        }

        public override void Delete()
        {
            AddEvent(new HelpShelterMessageDeleted(this));
        }
    }
}