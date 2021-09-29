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
        public bool IsRead { get; private set; }
        public string Title { get; }
        public MessageDescription Description { get; }
        public FullName FullName { get; }
        public PhoneNumber PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public ShelterMessage(Guid id, Guid userId, Guid shelterId, bool isRead, string title,
            MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt)
        {
            ValidateMessage(userId, shelterId);

            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            IsRead = isRead;
            Title = title;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }

        public static ShelterMessage Create(Guid id, Guid userId, Guid shelterId, bool isRead, string title,
            MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt)
        {
            ShelterMessage shelterMessage = new ShelterMessage(id, userId, shelterId, isRead, title, description,
                fullName, phoneNumber, createdAt);

            shelterMessage.AddEvent(new ShelterMessageCreated(shelterMessage));
            return shelterMessage;
        }

        public void MarkAsRead()
        {
            IsRead = true;
            AddEvent(new ShelterMessageRead(this));
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