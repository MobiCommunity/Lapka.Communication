using System;
using Lapka.Communication.Core.Exceptions.Abstract;

namespace Lapka.Communication.Core.Exceptions
{
    public class InvalidPetIdValueException : DomainException
    {
        public Guid PetId { get; }
        public InvalidPetIdValueException(Guid petId) : base($"Invalid pet id: {petId}")
        {
            PetId = petId;
        }

        public override string Code => "invalid_pet_id";
    }
}