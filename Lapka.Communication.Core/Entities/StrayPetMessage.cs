using System;
using System.Collections.Generic;
using System.IO;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Core.Entities
{
    public class StrayPetMessage : AggregateRoot
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public List<Guid> PhotoIds { get; }
        public string Description { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public StrayPetMessage(Guid id, Guid userId, Guid shelterId, List<Guid> photoIds, string description,
            string fullName, string phoneNumber, DateTime createdAt)
        {
            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            PhotoIds = photoIds;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }

        public static StrayPetMessage Create(Guid id, Guid userId, Guid shelterId, List<Guid> photoIds,
            string description, string fullName, string phoneNumber)
        {
            StrayPetMessage message = new StrayPetMessage(id, userId, shelterId, photoIds, description, fullName,
                phoneNumber, DateTime.Now);

            message.AddEvent(new StrayPetMessageCreated(message));
            return message;
        }

        public void Delete()
        {
            AddEvent(new StrayPetMessageDeleted(this));
        }
    }
}