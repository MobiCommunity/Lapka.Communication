using System;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class ShelterMessage : AggregateRoot
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public string Title { get; }
        public MessageDescription Description { get; }
        public FullName FullName { get; }
        public PhoneNumber PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public ShelterMessage(Guid id, Guid userId, Guid shelterId, string title, MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt)
        {
            ValidateMessage(userId, shelterId);

            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            Title = title;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }

        public static ShelterMessage Create(Guid id, Guid userId, Guid shelterId, string title, string description,
            string fullName, string phoneNumber, DateTime createdAt)
        {
            ShelterMessage shelterMessage = new ShelterMessage(id, userId, shelterId, title,
                new MessageDescription(description), new FullName(fullName), new PhoneNumber(phoneNumber), createdAt);
            
            shelterMessage.AddEvent(new ShelterMessageCreated(shelterMessage));
            return shelterMessage;
        }

        private void ValidateMessage(Guid userId, Guid shelterId)
        {
            ValidateUserId(userId);
            ValidateShelterId(shelterId);
        }

        private void ValidateShelterId(Guid shelterId)
        {
            if (shelterId == Guid.Empty)
            {
                throw new InvalidShelterIdValueException(shelterId);
            }
        }

        private void ValidateUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new InvalidUserIdValueException(userId);
            }
        }
    }
}