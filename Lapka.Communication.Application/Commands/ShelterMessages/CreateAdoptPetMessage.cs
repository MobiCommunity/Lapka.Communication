using System;
using Convey.CQRS.Commands;

namespace Lapka.Communication.Application.Commands.ShelterMessages
{
    public class CreateAdoptPetMessage : ICommand
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid PetId { get; }
        public string Description { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }

        public CreateAdoptPetMessage(Guid id, Guid userId, Guid petId, string description, string fullName,
            string phoneNumber)
        {
            Id = id;
            UserId = userId;
            PetId = petId;
            Description = description;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
    }
}