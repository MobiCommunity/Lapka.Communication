using System;
using System.IO;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class HelpShelterMessage : AggregateRoot
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public HelpType HelpType { get; }
        public string Description { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public HelpShelterMessage(Guid id, Guid userId, Guid shelterId, HelpType helpType, string description,
            string fullName, string phoneNumber, DateTime createdAt)
        {
            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            HelpType = helpType;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }

        public static HelpShelterMessage Create(Guid id, Guid userId, Guid shelterId, HelpType helpType,
            string description, string fullName,
            string phoneNumber)
        {
            HelpShelterMessage message = new HelpShelterMessage(id, userId, shelterId, helpType, description, fullName,
                phoneNumber, DateTime.Now);

            message.AddEvent(new HelpShelterMessageCreated(message));
            return message;
        }

        public void Delete()
        {
            AddEvent(new HelpShelterMessageDeleted(this));
        }
    }
}