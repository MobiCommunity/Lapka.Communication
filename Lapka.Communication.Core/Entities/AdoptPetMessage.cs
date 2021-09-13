using System;
using Lapka.Communication.Core.Events.Concrete;
using Lapka.Communication.Core.Exceptions;
using Lapka.Communication.Core.ValueObjects;

namespace Lapka.Communication.Core.Entities
{
    public class AdoptPetMessage : AggregateShelterMessage
    {
        public Guid PetId { get; }

        public AdoptPetMessage(Guid id, Guid userId, Guid shelterId, Guid petId, MessageDescription description,
            FullName fullName, PhoneNumber phoneNumber, DateTime createdAt) : base(id, userId, shelterId, description,
            fullName, phoneNumber, createdAt)
        {
            ValidateMessage(petId);
            
            PetId = petId;
        }

        public static AdoptPetMessage Create(Guid id, Guid userId, Guid shelterId, Guid petId, string description,
            string fullName, string phoneNumber)
        {
            AdoptPetMessage message = new AdoptPetMessage(id, userId, shelterId, petId,
                new MessageDescription(description), new FullName(fullName), new PhoneNumber(phoneNumber),
                DateTime.Now);

            message.AddEvent(new AdoptPetMessageCreated(message));
            return message;
        }

        public override void Delete()
        {
            AddEvent(new AdoptPetMessageDeleted(this));
        }

        private void ValidateMessage(Guid petId)
        {
            ValidatePetId(petId);
        }

        private void ValidatePetId(Guid petId)
        {
            if (petId == Guid.Empty)
            {
                throw new InvalidPetIdValueException(petId);
            }
        }
    }
}