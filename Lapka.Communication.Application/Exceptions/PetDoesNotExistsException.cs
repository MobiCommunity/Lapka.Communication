using System;

namespace Lapka.Communication.Application.Exceptions
{
    public class PetDoesNotExistsException : AppException
    {
        public Guid PetId { get; }
        public PetDoesNotExistsException(Guid petId) : base($"Pet does not exists: {petId}")
        {
            PetId = petId;
        }

        public override string Code => "pet_does_not_exists";
    }
}