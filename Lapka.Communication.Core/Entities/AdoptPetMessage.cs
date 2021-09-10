using System;
using System.IO;
using Lapka.Communication.Core.Events.Concrete;

namespace Lapka.Communication.Core.Entities
{
    public class AdoptPetMessage : AggregateRoot
    {
        public Guid UserId { get; }
        public Guid ShelterId { get; }
        public Guid PetId { get; }
        public string Description { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }
        public DateTime CreatedAt { get; }


        public AdoptPetMessage(Guid id, Guid userId, Guid shelterId, Guid petId, string description, string fullName,
            string phoneNumber, DateTime createdAt)
        {
            Id = new AggregateId(id);
            UserId = userId;
            ShelterId = shelterId;
            PetId = petId;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
        }

        public static AdoptPetMessage Create(Guid id, Guid userId, Guid shelterId, Guid petId, string description, string fullName,
            string phoneNumber)
        {
            AdoptPetMessage message = new AdoptPetMessage(id, userId, shelterId, petId, description, fullName, phoneNumber, DateTime.Now);

            message.AddEvent(new AdoptPetMessageCreated(message));
            return message;
        }

        public void Delete()
        {
            AddEvent(new AdoptPetMessageDeleted(this));
        }
    }
}