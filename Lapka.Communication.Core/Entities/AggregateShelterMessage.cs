using System;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public abstract class AggregateShelterMessage : AggregateRoot
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public MessageDescription Description { get; }
        public FullName FullName { get; }
        public PhoneNumber PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public AggregateShelterMessage(Guid id, Guid userId, Guid shelterId, MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt)
        {
            ValidateMessage(userId, shelterId);

            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }
        public abstract void Delete();

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